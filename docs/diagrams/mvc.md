# Diagrama: MVC

## Objetivo

Representar a arquitetura MVC do projeto `agilum.mvc.web`, mostrando a interação entre Controllers, Views, ViewModels e as camadas inferiores.

---

## Diagrama MVC

```mermaid
graph TD
    subgraph "Browser"
        User["Usuário<br/>AdminLTE + jQuery"]
    end

    subgraph "ASP.NET Core MVC"
        Middleware["Middleware Pipeline<br/>Auth → Empresa → Exception"]
        Controller["Controller<br/>Herda de MainController"]
        View["View<br/>Razor + _main.cshtml"]
        VM["ViewModel<br/>DTO + Data Annotations"]
    end

    subgraph "Business"
        Service["Service<br/>BaseService → *Service"]
        Notifier["INotificador<br/>Notification Pattern"]
        Fluent["FluentValidation<br/>AbstractValidator"]
    end

    subgraph "Infrastructure"
        Repo["Repository<br/>Repository<T>"]
        EF["EF Core 3.1<br/>MySQL via Pomelo"]
        DapperRepo["Dapper Repository<br/>Consultas SQL"]
    end

    User -->|"HTTP Request"| Middleware
    Middleware --> Controller
    Controller -->|"ViewModel"| View
    Controller --> Service
    Service --> Notifier
    Service --> Fluent
    Service --> Repo
    Repo --> EF
    Repo --> DapperRepo
    View -->|"HTML Response"| User
```

---

## Fluxo de uma Action

```mermaid
sequenceDiagram
    participant Browser
    participant Middleware
    participant Controller
    participant AutoMapper
    participant Service
    participant Repository
    participant DB

    Browser->>Middleware: GET /produto/novo
    Middleware->>Controller: Create()
    Controller->>Service: ObterListasAuxiliares()
    Controller->>AutoMapper: Model → ViewModel
    Controller-->>Browser: View("CreateEdit", viewModel)

    Browser->>Middleware: POST /produto/novo
    Middleware->>Controller: Create(ProdutoViewModel)
    Controller->>Controller: ModelState.IsValid?
    Controller->>AutoMapper: ViewModel → Produto
    Controller->>Service: Adicionar(produto)
    Service->>Service: ExecutarValidacao()
    Service->>Repository: AdicionarSemSalvar(produto)
    Controller->>Service: Salvar()
    Service->>Repository: SaveChanges()
    Repository->>DB: INSERT INTO produto...
    Controller-->>Browser: Redirect /produto
```

---

## Para Preencher

> **TODO:** Adicionar diagrama detalhado do MainController e seus 8 serviços injetados.
