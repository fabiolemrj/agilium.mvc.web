# Diagrama: Fluxo de Login

## Objetivo

Representar o fluxo completo de autenticação do Agilium Manager, incluindo login, validação de credenciais, criação de cookie, seleção de empresa e verificação de permissões.

---

## Fluxo Completo

```mermaid
sequenceDiagram
    actor User
    participant Browser
    participant LoginPage as /Identity/Account/Login
    participant SignInMgr as SignInManager
    participant UserMgr as UserManager
    participant IdentityDB as MySQL (Identity)
    participant Cookie as Cookie Auth
    participant Session as Session
    participant EmpresaPage as Seleção Empresa
    participant HomePage as Home/Index

    User->>Browser: Acessa qualquer página
    Browser->>Browser: UseAuthentication()
    
    alt Não autenticado
        Browser->>LoginPage: Redirect /Identity/Account/Login
        LoginPage-->>User: Form (email + senha)
        
        User->>LoginPage: Submit credenciais
        LoginPage->>SignInMgr: PasswordSignInAsync(email, senha)
        SignInMgr->>UserMgr: FindByEmailAsync(email)
        UserMgr->>IdentityDB: SELECT * FROM aspnetusers
        IdentityDB-->>UserMgr: CaUsuarioIdentity
        
        alt Senha correta
            UserMgr->>UserMgr: VerifyPassword()
            SignInMgr->>Cookie: Criar ClaimsPrincipal
            Note over Cookie: Claims: UserId, Email, Roles
            Cookie->>Cookie: HttpOnly, 3h, Sliding
            Cookie-->>Browser: Set-Cookie
            Browser->>EmpresaPage: Redirect seleção empresa
        else Senha incorreta
            SignInMgr-->>LoginPage: Falha
            LoginPage-->>User: "Login inválido"
        else Lockout (5 tentativas)
            SignInMgr-->>Browser: Redirect Lockout
        end
    else Autenticado
        Browser->>Session: EmpresaSelecionadaMiddleware
        alt Sem empresa na sessão
            Session->>EmpresaPage: Redirect seleção
            User->>EmpresaPage: Selecionar Empresa
            EmpresaPage->>Session: Armazenar EmpresaUsuarioViewModel
        end
        Session->>HomePage: Home/Index
        HomePage-->>User: Dashboard
    end
```

---

## Verificação de Permissão

```mermaid
sequenceDiagram
    participant Controller
    participant Filter as RequisitoClaimFilter
    participant CaService
    participant DB

    Controller->>Filter: [ClaimsAuthorize(2067)]
    Filter->>Filter: IsAuthenticated?
    
    alt Não
        Filter-->>Controller: Redirect Login
    end

    Filter->>Filter: Obter UserId da Claim
    Filter->>CaService: UsuarioTemPermissao(userId, 2067)
    CaService->>DB: Consultar permissões
    DB-->>CaService: Resultado

    alt Tem permissão
        CaService-->>Filter: true
        Filter-->>Controller: Prossegue
    else Sem permissão
        CaService-->>Filter: false
        Filter-->>Controller: Redirect /Home/Error/403
    end
```

---

## Para Preencher

> **TODO:** Adicionar fluxo de refresh token, logout e troca de empresa.
