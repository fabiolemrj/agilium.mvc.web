# Diagrama: Request Pipeline

## Objetivo

Representar o pipeline de processamento de requisições HTTP no projeto `agilum.mvc.web`, da entrada até a resposta.

---

## Pipeline Completo

```mermaid
graph TD
    Request["HTTP Request"] --> StaticFiles["UseStaticFiles<br/>wwwroot/"]
    StaticFiles --> Routing["UseRouting<br/>Rotas MVC + Areas"]
    Routing --> Session["UseSession<br/>3h timeout, HttpOnly"]
    Session --> Auth["UseAuthentication<br/>Cookie Auth + Identity"]
    Auth --> Authorization["UseAuthorization<br/>Claims + Roles"]
    Authorization --> Empresa["EmpresaSelecionadaMiddleware<br/>Bloqueia sem empresa"]
    Empresa --> Exception["ExceptionMiddleware<br/>Captura exceções"]
    Exception --> Cultura["Cultura pt-BR<br/>dd/MM/yyyy, decimal ','"]
    Cultura --> Endpoints["UseEndpoints"]
    
    Endpoints --> RazorPages["Razor Pages<br/>/Identity/Account/*"]
    Endpoints --> Controllers["Controllers<br/>{controller}/{action}"]
    Endpoints --> Areas["Areas<br/>{area}/{controller}/{action}"]
    
    Controllers --> Response["HTTP Response"]
    RazorPages --> Response
    Areas --> Response
```

---

## Sequência de Execução

```mermaid
sequenceDiagram
    participant Browser
    participant StaticFiles
    participant Session
    participant Auth
    participant Company
    participant Controller
    participant Service
    participant DB

    Browser->>StaticFiles: GET /produto
    Note over StaticFiles: Arquivo estático?<br/>Não → Próximo middleware
    StaticFiles->>Session: Carregar sessão
    Session->>Auth: Validar Cookie
    Note over Auth: Identity.IsAuthenticated?
    Auth->>Company: EmpresaSelecionadaMiddleware
    Note over Company: Empresa na Session?
    Company->>Controller: ProdutoController.Index()
    Controller->>Service: ObterTodas(idEmpresa)
    Service->>DB: SELECT * FROM produto...
    DB-->>Service: ResultSet
    Service-->>Controller: List<Produto>
    Controller-->>Browser: View(produtos)
```

---

## Middlewares Customizados

### EmpresaSelecionadaMiddleware

```mermaid
graph TD
    Request["Requisição"] --> Check{Autenticado?}
    Check -->|Não| Skip["Skip middleware"]
    Check -->|Sim| PathCheck{Rota permitida?<br/>Login, Logout, CSS, JS...}
    PathCheck -->|Sim| Skip
    PathCheck -->|Não| EmpresaCheck{Empresa na Session?}
    EmpresaCheck -->|Sim| Next["next() → Próximo middleware"]
    EmpresaCheck -->|Não| Block["Redireciona para<br/>seleção de empresa"]
```

### ExceptionMiddleware

```mermaid
graph TD
    Request["Requisição"] --> Try["try { await next() }"]
    Try -->|Sucesso| Return["Retorna resposta"]
    Try -->|CustomHttpRequestException| HandleCustom["Tratar erro HTTP<br/>StatusCode definido"]
    Try -->|Outra Exception| HandleGeneric["Log erro<br/>Retornar 500"]
```

---

## Para Preencher

> **TODO:** Adicionar diagrama de sequência detalhado com tempos de cada middleware.
