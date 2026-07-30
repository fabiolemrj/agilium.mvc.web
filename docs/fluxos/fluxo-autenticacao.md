# Fluxo de Autenticação

## Objetivo

Documentar o fluxo completo de autenticação e autorização do Agilium Manager, desde o login até a validação de permissões por ação.

---

## Fluxo Principal

```
[Browser] Acessa qualquer página protegida
      │
      ▼
[Middleware] UseAuthentication()
      │
      ├── Usuário autenticado?
      │     ├── Não → Redireciona para /Identity/Account/Login
      │     │
      │     └── Sim → Prossegue
      │
      ▼
[EmpresaSelecionadaMiddleware]
      │
      ├── Empresa na Session?
      │     ├── Não → Redireciona para seleção de empresa
      │     │
      │     └── Sim → Prossegue
      │
      ▼
[Controller] [Authorize]
      │
      ▼
[Action] [ClaimsAuthorizeAttribute(idTag)]
      │
      ├── RequisitoClaimFilter.OnAuthorization()
      │     │
      │     ├── Identity.IsAuthenticated?
      │     │     └── Não → Redirect /Identity/Account/Login
      │     │
      │     ├── Obtém UserId da Claim
      │     │
      │     └── ICaService.UsuarioTemPermissao(userId, idTag)
      │           │
      │           ├── True → Executa Action
      │           └── False → Redirect /Home/Error/403
      │
      ▼
[Action] Executada
```

---

## Fluxo de Login

```
[GET] /Identity/Account/Login
      │
      ▼
[View] Formulário: e-mail + senha
      │
      ▼
[POST] /Identity/Account/Login
      │
      ▼
SignInManager.PasswordSignInAsync(email, password)
      │
      ├── Falhou?
      │     └── Lockout? → /Identity/Account/Lockout
      │     └── Senha incorreta → Retorna View com erro
      │
      ├── Sucesso
      │     │
      │     ▼
      │  Criar Claims Principal
      │     ├── ClaimTypes.NameIdentifier (UserId)
      │     ├── ClaimTypes.Email
      │     └── ClaimTypes.Role
      │     │
      │     ▼
      │  Cookie Authentication (CookieAuthenticationDefaults)
      │     ├── HttpOnly = true
      │     ├── ExpireTimeSpan = 3 horas
      │     └── SlidingExpiration = true
      │     │
      │     ▼
      │  Redireciona para seleção de empresa
      │     │
      │     ▼
      │  [POST] /Empresa/SelecionarEmpresa
      │     │
      │     ▼
      │  Empresa armazenada na Session
      │     │
      │     ▼
      │  Redirect /Home/Index
```

---

## Fluxo de Logout

```
[GET] /Identity/Account/Logout
      │
      ▼
SignInManager.SignOutAsync()
      │
      ▼
Cookie removido (HttpContext.SignOutAsync)
      │
      ▼
Session.Clear()
      │
      ▼
Redirect /Identity/Account/Login
```

---

## Configuração de Senha

| Regra | Valor |
|-------|-------|
| Tamanho mínimo | 6 caracteres |
| Exigir dígito | Sim |
| Exigir maiúscula | Não |
| Exigir minúscula | Não |
| Exigir caractere especial | Não |
| Lockout | 5 tentativas → 5 minutos |

---

## Componentes Envolvidos

| Componente | Localização | Papel |
|------------|-------------|-------|
| `IdentityConfig` | `agilum.mvc.web/Configuration/IdentityConfig.cs` | Configura Identity Core + Cookie Auth |
| `dbIdentityContext` | `agilum.mvc.web/Data/dbIdentityContext.cs` | DbContext do Identity (MySQL) |
| `CaUsuarioIdentity` | `agilium-manager-azure-business/Models/CaUsuarioIdentity.cs` | Entidade de usuário (extends `IdentityUser`) |
| `AspNetUser` | `agilum.mvc.web/Extensions/AspNetUser.cs` | Implementação de `IUser` |
| `CustomAuthorization` | `agilum.mvc.web/Extensions/CustomAuth.cs` | `ClaimsAuthorizeAttribute` + `RequisitoClaimFilter` |
| `RequisitoClaimFilter` | `agilum.mvc.web/Extensions/CustomAuth.cs` | `IAuthorizationFilter` — valida permissão por idTag |
| `EmpresaSelecionadaMiddleware` | `agilum.mvc.web/Extensions/EmpresaSelecionadaMiddleware.cs` | Bloqueia acesso sem empresa na sessão |
| `AuthService` | `agilum.mvc.web/Services/AuthService.cs` | Autenticação customizada |
| `CaService` | `agilium-manager-azure-business/Services/CaService.cs` | `UsuarioTemPermissao(idUsuario, idTag)` |
| `SignInManager` | Identity Core | Gerenciamento de login (`PasswordSignInAsync`) |
| `UserManager` | Identity Core | Gerenciamento de usuários |

---

## Permissões (ClaimsAuthorize)

| idTag | Funcionalidade | Controller |
|-------|---------------|------------|
| 2066 | Listar Compras | `CompraController.IndexCompra` |
| 2067 | Criar Compra | `CompraController.Create` |
| 2068 | Cancelar Compra | `CompraController.Cancelar` |
| 2070 | Editar / Importar XML | `CompraController.Edit` |
| 2072 | Efetivar Compra | `CompraController.Efetivar` |

---

## Pontos de Atenção

- Cookie expira em **3 horas** com sliding expiration
- Cookie: `HttpOnly = true`, `IsEssential = true`, `SecurePolicy = SameAsRequest`
- Identity usa **MySQL** (`UseMySql`) com connection string `dbIdentityContextConnection`
- Empresa deve ser selecionada após login (middleware `EmpresaSelecionadaMiddleware`)
- Permissões são validadas a **cada requisição** via `ICaService.UsuarioTemPermissao()`
- Lockout é de **5 minutos** após 5 tentativas (`AllowedForNewUsers = true`)
- Rotas de login/logout são públicas (excluídas do middleware de empresa)
- `RequireUniqueEmail = false` (e-mails podem se repetir)
