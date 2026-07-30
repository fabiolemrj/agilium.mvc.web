# Diagrama: Backend

## Objetivo

Representar a arquitetura do backend do Agilium Manager, incluindo APIs REST, camada de serviços e comunicação entre projetos.

---

## Projetos Backend

```mermaid
graph TD
    subgraph "APIs"
        API["agilium-manager-azure-api<br/>Core REST API<br/>.NET Core 3.1"]
        PDV["agilium-pdv-azure-api<br/>PDV REST API<br/>.NET Core 3.1"]
    end

    subgraph "Camada de Negócio"
        Business["agilium-manager-azure-business<br/>Models + Services + Validations"]
    end

    subgraph "Camada de Infra"
        Infra["agilium-manager-git-azure-infra<br/>EF Core + Dapper + MongoDB"]
    end

    API --> Business
    PDV --> Business
    Business --> Infra
```

---

## Estrutura da API

```mermaid
graph TD
    subgraph "agilium-manager-azure-api"
        direction TB
        ApiProgram["Program.cs"]
        ApiStartup["Startup.cs"]
        ApiConfig["Configuration/<br/>Swagger, Identity, DI, JWT"]
        ApiControllers["Controllers/<br/>MainController"]
        ApiV1["V1/<br/>Versionamento"]
        ApiExt["Extension/<br/>AppSettings, CustomAuthorization"]
        ApiServices["Services/"]
    end

    ApiProgram --> ApiStartup
    ApiStartup --> ApiConfig
    ApiStartup --> ApiControllers
    ApiStartup --> ApiV1
    ApiControllers --> ApiExt
    ApiControllers --> ApiServices
```

---

## Versionamento da API

```mermaid
graph LR
    Client["Cliente HTTP"] --> V1["/api/v1/produtos"]
    Client --> V2["/api/v2/produtos (futuro)"]

    V1 --> ApiVersion["[ApiVersion('1.0')]"]
    V1 --> Route["[Route('api/v{version:apiVersion}/[controller]')]"]

    ApiVersion --> Explorer["ApiVersioning + ApiExplorer"]
    Explorer --> Swagger["Swagger<br/>Docs por versão"]
```

---

## Comunicação MVC ↔ API

```mermaid
sequenceDiagram
    participant MVC
    participant HttpClient
    participant API
    participant Business
    participant DB

    MVC->>HttpClient: GET /api/v1/produtos
    Note over HttpClient: Polly Resilience<br/>Retry + Circuit Breaker
    HttpClient->>API: HTTP Request
    API->>Business: ProdutoService.ObterTodas()
    Business->>DB: SELECT...
    DB-->>Business: ResultSet
    Business-->>API: List<Produto>
    API-->>HttpClient: JSON Response
    HttpClient-->>MVC: Desserializar
```

---

## Para Preencher

> **TODO:** Adicionar diagrama detalhado dos endpoints da API com versões e autenticação.
