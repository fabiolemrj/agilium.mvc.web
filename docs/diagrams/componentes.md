# Diagrama: Componentes

## Objetivo

Representar os principais componentes do sistema Agilium Manager, seus relacionamentos e a comunicação entre eles.

---

## Componentes por Projeto

```mermaid
graph TD
    subgraph "agilum.mvc.web"
        direction TB
        Ctrls["28 Controllers"]
        Vws["Views Razor"]
        VMs["ViewModels"]
        Config["Configuration<br/>Identity, AutoMapper, DI, MVC"]
        Ext["Extensions<br/>Middleware, TagHelpers, ViewComponents"]
        SvcMVC["Services MVC<br/>Auth, Email, Crypto"]
        DataID["Data<br/>dbIdentityContext"]
    end

    subgraph "agilium-manager-azure-business"
        direction TB
        SvcBiz["40+ Services"]
        Models["100+ Models"]
        Ifaces["Interfaces<br/>IService, IRepository"]
        EnumsBiz["33 Enums"]
        Notif["Notificações<br/>Notification Pattern"]
        ValidBiz["Validations<br/>FluentValidation"]
    end

    subgraph "agilium-manager-git-azure-infra"
        direction TB
        Repos["40+ Repositories"]
        DapperRepos["Dapper Repositories"]
        MongoRepo["MongoDB Repository"]
        Ctx["AgiliumContext"]
        ConnFactory["ConnectionFactory"]
        Mappings["Entity Mappings"]
    end

    Ctrls --> SvcBiz
    VMs --> Models
    SvcBiz --> Ifaces
    SvcBiz --> Notif
    SvcBiz --> ValidBiz
    Repos --> Ctx
    DapperRepos --> ConnFactory
    Ctx --> Models
```

---

## Comunicação entre Componentes

```mermaid
sequenceDiagram
    participant View
    participant Controller
    participant AutoMapper
    participant Service
    participant Validation
    participant Repository
    participant DB

    View->>Controller: HTTP Request + ViewModel
    Controller->>AutoMapper: ViewModel → Model
    Controller->>Service: Adicionar(model)
    Service->>Validation: ExecutarValidacao()
    Validation-->>Service: Válido / Inválido
    Service->>Repository: AdicionarSemSalvar(model)
    Controller->>Service: Salvar()
    Service->>Repository: SaveChanges()
    Repository->>DB: INSERT/UPDATE
    DB-->>Repository: Resultado
    Repository-->>Service: OK
    Service-->>Controller: OK
    Controller->>AutoMapper: Model → ViewModel
    Controller-->>View: View(viewModel)
```

---

## Para Preencher

> **TODO:** Adicionar diagrama de componentes detalhado por módulo de negócio.
