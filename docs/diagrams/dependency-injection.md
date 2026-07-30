# Diagrama: Dependency Injection

## Objetivo

Representar o grafo de injeção de dependências do Agilium Manager, mostrando como os serviços, repositórios e componentes são registrados e resolvidos.

---

## Ciclo de Vida

```mermaid
graph LR
    subgraph "Scoped (por requisição)"
        DbContext["AgiliumContext"]
        Notifier["INotificador"]
        Services["40+ Services"]
        Repos["40+ Repositories"]
        DapperRepos["Dapper Repositories"]
        DbSession["DbSession"]
        User["IUser (AspNetUser)"]
    end

    subgraph "Singleton"
        HttpAccessor["IHttpContextAccessor"]
    end

    subgraph "Transient"
        Validators["FluentValidation Validators"]
    end

    HttpAccessor --> User
    Services --> Notifier
    Services --> Repos
    Services --> DapperRepos
    Repos --> DbContext
    DapperRepos --> DbSession
```

---

## Grafo de Dependências — Compra

```mermaid
graph TD
    CC["CompraController"] --> CS["CompraService"]
    CC --> MC["MainController (base)"]

    MC --> Notifier["INotificador"]
    MC --> Config["IConfiguration"]
    MC --> User["IUser"]
    MC --> UtilDapper["IUtilDapperRepository"]
    MC --> Log["ILogService"]
    MC --> Mapper["IMapper"]
    MC --> Licenca["ILicencaService"]
    MC --> Auth["IAuthService"]

    CS --> CR["ICompraRepository"]
    CS --> CIR["ICompraItemRepository"]
    CS --> CFR["ICompraFiscalRepository"]
    CS --> CDR["ICompraDapperRepository"]
    CS --> FDR["IFornecedorDapperRepository"]
    CS --> PDR["IProdutoDapper"]
    CS --> EDR["IEstoqueDapperRepository"]
    CS --> PCDR["IPlanoContaDapperRepository"]
    CS --> DR["IDapperRepository"]
    CS --> UDR["IUtilDapperRepository"]
    CS --> Notifier

    CR --> EF["AgiliumContext (EF Core)"]
    CDR --> DB["ConnectionFactory (Dapper)"]
```

---

## Registro no Contêiner

```mermaid
graph TD
    Startup["Startup.ConfigureServices()"]
    Startup --> Resolve["ResolveDependencies()"]
    
    Resolve --> Geral["#region geral"]
    Geral --> HttpClient["AddHttpClient"]
    Geral --> NotifierReg["AddScoped<INotificador, Notificador>"]
    Geral --> HttpAccessorReg["AddSingleton<IHttpContextAccessor>"]
    Geral --> AuthServiceReg["AddScoped<IAuthService, AuthService>"]
    Geral --> AutenticacaoReg["AddScoped<IAutenticacaoService>"]
    Geral --> AgiliumReg["AddScoped<AgiliumContext>"]
    Geral --> UtilDapperReg["AddScoped<IUtilDapperRepository>"]
    Geral --> DbSessionReg["AddScoped<DbSession>"]
    
    Resolve --> Repos["#region repositories"]
    Resolve --> Services["#region services"]
    Resolve --> Dapper["#region dapper"]
```

---

## Para Preencher

> **TODO:** Adicionar lista completa de todos os registros por categoria.
