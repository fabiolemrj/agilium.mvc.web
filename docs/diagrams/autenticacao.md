# Diagrama: Autenticação

## Objetivo

Representar o fluxo de autenticação e autorização do Agilium Manager, incluindo login, Cookie Auth, Claims e `ClaimsAuthorizeAttribute`.

---

## Fluxo de Login

```mermaid
sequenceDiagram
    participant Browser
    participant LoginPage
    participant SignInManager
    participant IdentityDB
    participant Cookie
    participant EmpresaPage

    Browser->>LoginPage: GET /Identity/Account/Login
    LoginPage-->>Browser: Form (email + senha)

    Browser->>LoginPage: POST Login
    LoginPage->>SignInManager: PasswordSignInAsync(email, senha)
    SignInManager->>IdentityDB: Validar credenciais
    IdentityDB-->>SignInManager: Usuário válido

    alt Sucesso
        SignInManager->>Cookie: Criar Cookie (Claims)
        Note over Cookie: UserId, Email, Roles<br/>HttpOnly, 3h expiry
        Cookie-->>Browser: Set-Cookie
        Browser->>EmpresaPage: Redirect seleção empresa
    else Falha
        SignInManager-->>Browser: Retorna View com erro
    else Lockout
        SignInManager-->>Browser: Redirect /Account/Lockout
    end
```

---

## Fluxo de Autorização (ClaimsAuthorizeAttribute)

```mermaid
sequenceDiagram
    participant Browser
    participant Middleware
    participant Controller
    participant RequisitoClaimFilter
    participant CaService
    participant DB

    Browser->>Middleware: GET /compra/novo
    Middleware->>Controller: CompraController.Create()
    Controller->>RequisitoClaimFilter: [ClaimsAuthorize(2067)]
    
    RequisitoClaimFilter->>RequisitoClaimFilter: IsAuthenticated?
    alt Não autenticado
        RequisitoClaimFilter-->>Browser: Redirect /Identity/Account/Login
    end

    RequisitoClaimFilter->>RequisitoClaimFilter: Obter UserId da Claim
    RequisitoClaimFilter->>CaService: UsuarioTemPermissao(userId, 2067)
    CaService->>DB: SELECT permissao...
    DB-->>CaService: Resultado

    alt Tem permissão
        CaService-->>RequisitoClaimFilter: true
        RequisitoClaimFilter->>Controller: Executa Action
        Controller-->>Browser: View Create
    else Sem permissão
        CaService-->>RequisitoClaimFilter: false
        RequisitoClaimFilter-->>Browser: Redirect /Home/Error/403
    end
```

---

## Configuração de Cookie e Sessão

```mermaid
graph TD
    subgraph "Identity"
        IdentityCore["AddIdentityCore<br/>CaUsuarioIdentity"]
        Roles["AddRoles<br/>IdentityRole"]
        SignIn["SignInManager"]
        UserMgr["UserManager"]
    end

    subgraph "Cookie Auth"
        Cookie["CookieAuthenticationDefaults"]
        Cookie --> Login["LoginPath: /Identity/Account/Login"]
        Cookie --> Logout["LogoutPath: /Identity/Account/Logout"]
        Cookie --> Denied["AccessDeniedPath: /Identity/Account/AccessDenied"]
        Cookie --> HttpOnly["HttpOnly: true"]
        Cookie --> Expire["ExpireTimeSpan: 3 horas"]
        Cookie --> Sliding["SlidingExpiration: true"]
    end

    subgraph "Session"
        Session["AddSession"]
        Session --> Timeout["IdleTimeout: 3 horas"]
        Session --> Essential["IsEssential: true"]
    end

    IdentityCore --> Cookie
    Roles --> IdentityCore
    SignIn --> IdentityCore
    UserMgr --> IdentityCore
```

---

## Para Preencher

> **TODO:** Adicionar diagrama de refresh token e fluxo de logout.
