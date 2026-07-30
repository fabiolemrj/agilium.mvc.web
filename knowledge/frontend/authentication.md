# Autenticação

## Objetivo

Documentar o fluxo de autenticação no frontend do Agilium Manager: login, armazenamento de credenciais, cookie e logout.

---

# Visão Geral

A autenticação usa **ASP.NET Core Identity** com **Cookie Authentication**. O usuário faz login via `/Identity/Account/Login` (Razor Pages). Após autenticação bem-sucedida, um cookie HttpOnly é criado com expiração de 3 horas e sliding expiration. O logout limpa o cookie e a sessão.

---

# Organização

### Fluxo de Login

```
[GET] /Identity/Account/Login → Formulário (e-mail + senha)
      │
      ▼
[POST] /Identity/Account/Login
      │
      ▼
SignInManager.PasswordSignInAsync(email, password)
      ├── Falha: lockout (5 tentativas, 5 min) ou erro de senha
      └── Sucesso: criar ClaimsPrincipal + Cookie
      │
      ▼
Redireciona para seleção de empresa
      │
      ▼
[POST] /Empresa/SelecionarEmpresa → Session
      │
      ▼
Redirect /Home/Index
```

### Configuração do Cookie

```csharp
// IdentityConfig.cs
options.Cookie.HttpOnly = true;
options.Cookie.IsEssential = true;
options.Cookie.SecurePolicy = CookieSecurePolicy.SameAsRequest;
options.SlidingExpiration = true;
options.ExpireTimeSpan = TimeSpan.FromHours(3);
```

---

# Principais Conceitos

- **Cookie Authentication**: HttpOnly, IsEssential, 3h sliding
- **Password Policy**: 6 caracteres, exige dígito (sem maiúscula/minúscula/especial)
- **Lockout**: 5 tentativas → 5 minutos bloqueado
- **RequireUniqueEmail**: false (e-mails podem repetir)
- **dbIdentityContext**: MySQL para tabelas Identity (aspnetusers, roles, claims)

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-autenticacao.md` — Fluxo completo

---

# Componentes Relacionados

- `IdentityConfig.cs` — Configuração
- `dbIdentityContext` — DbContext Identity
- `CaUsuarioIdentity` — Entidade de usuário
- `_LoginPartial.cshtml` — Exibição do usuário logado

---

# APIs Relacionadas

- N/A — Autenticação via Razor Pages Identity

---

# Boas Práticas

- Cookie HttpOnly para prevenir XSS
- Sliding expiration para renovar cookie em atividade
- Sempre verificar `User.Identity.IsAuthenticated` antes de ações protegidas
- Redirecionar para `/Identity/Account/Login` quando não autenticado

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/fluxos/fluxo-autenticacao.md` — Fluxo detalhado
- `docs/frontend/mvc.md` — Pipeline de autenticação
- `knowledge/business/usuarios.md` — Gestão de usuários

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `IdentityConfig.cs` para configuração completa
2. Verificar `Areas/Identity/` para páginas de login
3. Verificar `CaUsuarioIdentity` model
4. Verificar `EmpresaController.SelecionarEmpresa` para pós-login

---

# Resumo

Login via Identity Core + Cookie Auth (3h sliding, HttpOnly, IsEssential). Lockout de 5 tentativas/5 min. Pós-login obriga seleção de empresa. Logout limpa cookie + sessão.
