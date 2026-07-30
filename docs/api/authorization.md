# Autorização da API

## Objetivo

Documentar como o controle de autorização é implementado no ecossistema Agilium Manager, descrevendo os mecanismos de autorização utilizados, a validação de permissões e a forma como o acesso aos recursos é protegido.

Este documento deve permanecer sincronizado com a implementação existente no código-fonte.

---

# Escopo

Este documento contempla:

- Arquitetura da autorização
- Fluxo de autorização
- Controle de acesso
- Claims
- Papéis e permissões
- Policies
- Atributos de autorização
- Validação de permissões
- Endpoints públicos
- Configuração da autorização

---

# Fontes para Análise

Antes de atualizar este documento, analisar:

- Startup.cs
- IdentityConfig.cs
- CustomAuth.cs
- ClaimsAuthorizeAttribute
- Controllers
- MainController
- ICaService
- IUser
- AuthService
- EmpresaSelecionadaMiddleware
- Policies registradas
- Atributos `[Authorize]`
- Atributos `[AllowAnonymous]`

---

# Índice

- Visão Geral
- Fluxo de Autorização
- Controle por Claims
- Papéis e Permissões
- Policies
- Atributos de Autorização
- Proteção dos Controllers
- Endpoints Públicos
- Componentes Envolvidos
- Considerações de Segurança
- Limitações Conhecidas
- Documentos Relacionados

---

# Visão Geral

Após a autenticação do usuário, toda requisição protegida passa pelo mecanismo de autorização do ASP.NET Core.

A autorização utiliza:

- `[Authorize]`
- `ClaimsAuthorizeAttribute`
- `ICaService.UsuarioTemPermissao()`

As permissões são avaliadas antes da execução das Actions dos Controllers.

---

# Fluxo de Autorização

O fluxo de autorização segue a sequência abaixo:

```text
Requisição

      │

UseAuthentication()

      │

UseAuthorization()

      │

[Authorize]

      │

ClaimsAuthorizeAttribute

      │

ICaService.UsuarioTemPermissao()

      │

Action do Controller
```

---

# Controle por Claims

O principal mecanismo de autorização é baseado em Claims.

Após a autenticação, as Claims do usuário são carregadas e utilizadas para validar o acesso às funcionalidades.

As principais informações utilizadas são:

- Identificador do usuário
- Perfil
- Permissões
- Empresa selecionada

---

# Papéis e Permissões

A autorização do sistema é baseada principalmente nas permissões cadastradas para o usuário.

A validação é realizada através do serviço:

```
ICaService.UsuarioTemPermissao()
```

As permissões são verificadas antes da execução das funcionalidades protegidas.

Caso o projeto utilize papéis (Roles), estes devem ser documentados conforme identificados no código.

---

# Policies

Até o momento do levantamento não foram identificadas Policies customizadas registradas explicitamente.

Caso novas Policies sejam adicionadas, documentar:

- Nome
- Objetivo
- Regras
- Onde são utilizadas

---

# Atributos de Autorização

Os principais atributos utilizados são:

## Authorize

Protege Controllers e Actions contra acesso não autenticado.

Exemplo:

```csharp
[Authorize]
public class ProdutoController : MainController
{
}
```

---

## ClaimsAuthorizeAttribute

Implementa o controle de permissões granulares do sistema. É o **principal mecanismo de autorização por ação** no projeto.

### Definição

Localizado em `agilum.mvc.web/Extensions/CustomAuth.cs`:

```csharp
public class ClaimsAuthorizeAttribute : TypeFilterAttribute
{
    public ClaimsAuthorizeAttribute(int idTag) : base(typeof(RequisitoClaimFilter))
    {
        Arguments = new object[] { idTag };
    }
}
```

- Herda de `TypeFilterAttribute`, permitindo **injeção de dependência** no filtro
- Recebe um `int idTag` — identificador único da permissão (tag) a ser verificada
- Delega a execução para `RequisitoClaimFilter`

### Filtro de Execução

```csharp
public class RequisitoClaimFilter : IAuthorizationFilter
{
    private readonly int _idTag;
    private readonly ICaService _caService;

    public RequisitoClaimFilter(int idTag, ICaService caService)
    {
        _idTag = idTag;
        _caService = caService;
    }

    public void OnAuthorization(AuthorizationFilterContext context)
    {
        // 1. Verifica se o usuário está autenticado
        if (!context.HttpContext.User.Identity.IsAuthenticated)
        {
            context.Result = new RedirectToRouteResult(new RouteValueDictionary(new {
                area = "Identity",
                page = "/Account/Login",
                ReturnUrl = context.HttpContext.Request.Path.ToString()
            }));
            return;
        }

        // 2. Obtém o ID do usuário das claims
        var id = context.HttpContext.User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

        // 3. Valida permissão via serviço de negócio
        if (!CustomAuthorization.ValidarUsuario(_caService, id, _idTag))
        {
            context.Result = Error(context, 403); // → /Home/Error/403
        }
    }
}
```

### Fluxo de Validação

```
[ClaimsAuthorizeAttribute(2066)]     ← idTag da permissão
        │
        ▼
RequisitoClaimFilter.OnAuthorization()
        │
        ├── Usuário autenticado? ──Não──→ Redireciona para /Identity/Account/Login
        │
        └── Sim
              │
              ▼
        ICaService.UsuarioTemPermissao(userId, idTag)
              │
              ├── True  → Executa a Action
              └── False → Redireciona para /Home/Error/403
```

### Exemplo de Uso

No `CompraController`, cada ação tem sua própria tag de permissão:

```csharp
[Route("compra")]
[Authorize]
public class CompraController : MainController
{
    [Route("lista")]
    [ClaimsAuthorizeAttribute(2066)]   // Permissão: Listar Compras
    public async Task<IActionResult> IndexCompra(...) { }

    [Route("novo")]
    [ClaimsAuthorizeAttribute(2067)]   // Permissão: Criar Compra
    public async Task<IActionResult> Create() { }

    [Route("cancelar")]
    [ClaimsAuthorizeAttribute(2068)]   // Permissão: Cancelar Compra
    public async Task<IActionResult> Cancelar(long id) { }

    [Route("editar")]
    [ClaimsAuthorizeAttribute(2070)]   // Permissão: Editar Compra
    public async Task<IActionResult> Edit(long id) { }

    [Route("efetivar")]
    [ClaimsAuthorizeAttribute(2072)]   // Permissão: Efetivar Compra
    public async Task<IActionResult> Efetivar(long id) { }
}
```

### Classe Auxiliar CustomAuthorization

```csharp
public class CustomAuthorization
{
    // Validação simplificada de claims (atualmente sempre retorna true)
    public static bool ValidarClaimsUsuario(HttpContext context, string claimName, string claimValue)
    {
        return true;
    }

    // Validação real: consulta o banco via ICaService
    public static bool ValidarUsuario(ICaService caService, string idUsuario, int idTag)
    {
        return caService.UsuarioTemPermissao(idUsuario, idTag).Result;
    }
}
```

### Comportamento em Caso de Falha

| Cenário | Resultado |
|---------|-----------|
| Usuário **não autenticado** | Redireciona para `/Identity/Account/Login` |
| Usuário autenticado **sem permissão** | Redireciona para `/Home/Error/403` |
| Usuário autenticado **com permissão** | Executa a Action normalmente |

---

## AllowAnonymous

Utilizado apenas em endpoints públicos.

Sempre justificar sua utilização.

---

# Proteção dos Controllers

O padrão identificado no projeto é:

- Controllers protegidos com `[Authorize]`
- Actions protegidas por `ClaimsAuthorizeAttribute`
- Validação realizada antes da execução da Action

A maior parte dos Controllers herda de `MainController`, centralizando comportamentos comuns de autenticação e autorização.

---

# Componentes Envolvidos

| Componente | Responsabilidade |
|------------|------------------|
| Authorize | Exige autenticação |
| ClaimsAuthorizeAttribute | Valida permissões específicas |
| IUser | Usuário autenticado |
| ICaService | Verificação de permissões |
| MainController | Base dos Controllers |
| EmpresaSelecionadaMiddleware | Validação da empresa ativa |

---

# Endpoints Públicos

Os endpoints públicos devem utilizar explicitamente:

```csharp
[AllowAnonymous]
```

Seu uso deve ser restrito apenas às funcionalidades que realmente não exigem autenticação.

---

# Considerações de Segurança

A implementação atual utiliza:

- ASP.NET Core Identity
- Cookie Authentication
- Claims
- Autorização baseada em permissões
- Middleware de autenticação
- Middleware de autorização
- Empresa ativa validada por Middleware

---

# Limitações Conhecidas

O levantamento técnico foi realizado sobre o projeto **agilum.mvc.web**.

As APIs REST (`agilium-manager-azure-api` e `agilium-pdv-azure-api`) deverão possuir documentação específica caso utilizem um fluxo de autorização diferente do MVC.

---

# Boas Práticas

Sempre:

- proteger Controllers com `[Authorize]`;
- utilizar `ClaimsAuthorizeAttribute` para permissões específicas;
- evitar duplicação de regras de autorização;
- centralizar validações na camada de negócio;
- manter o controle de acesso desacoplado da lógica de negócio.

---

# Documentos Relacionados

- ./authentication.md
- ../architecture/authentication.md
- ../architecture/authorization.md
- ../business/permissions.md
- ./conventions.md
- ./errors.md