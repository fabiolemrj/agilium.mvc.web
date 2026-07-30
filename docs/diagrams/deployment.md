# Diagrama: Deployment

## Objetivo

Representar a arquitetura de deployment do Agilium Manager, incluindo ambientes, contêineres Docker e plataforma cloud (Render).

---

## Arquitetura de Deployment

```mermaid
graph TD
    subgraph "Render Cloud"
        subgraph "Web Service"
            MVC["agilum.mvc.web<br/>Docker Container<br/>PORT=5000"]
        end

        subgraph "Web Service"
            API["agilium-manager-azure-api<br/>Docker Container<br/>PORT=5000"]
        end

        subgraph "Web Service"
            PDV["agilium-pdv-azure-api<br/>Docker Container<br/>PORT=5000"]
        end
    end

    subgraph "Banco de Dados (externo)"
        MySQL[("MySQL 8.0<br/>Serviço gerenciado")]
        MongoDB[("MongoDB<br/>Serviço gerenciado")]
    end

    subgraph "Externo"
        CDN["CDN / Proxy<br/>HTTPS Termination"]
        DNS["DNS<br/>domínio.com"]
    end

    DNS --> CDN
    CDN --> MVC
    CDN --> API
    CDN --> PDV

    MVC --> MySQL
    API --> MySQL
    PDV --> MySQL

    API --> MongoDB
```

---

## Docker

### Dockerfile (exemplo MVC)

```dockerfile
FROM mcr.microsoft.com/dotnet/core/aspnet:3.1 AS base
WORKDIR /app
EXPOSE 5000
ENV ASPNETCORE_URLS=http://+:5000

FROM mcr.microsoft.com/dotnet/core/sdk:3.1 AS build
WORKDIR /src
COPY . .
RUN dotnet restore
RUN dotnet publish -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=build /app .
ENTRYPOINT ["dotnet", "agilum.mvc.web.dll"]
```

### Projetos com Dockerfile

| Projeto | Dockerfile |
|---------|------------|
| `agilum.mvc.web` | ✅ `Dockerfile` |
| `agilium-manager-azure-api` | ✅ `Dockerfile` |
| `agilium-pdv-azure-api` | ❌ (não localizado) |
| Raiz | ✅ `Dockerfile` |

---

## Configuração para Render

```csharp
// Program.cs
var port = Environment.GetEnvironmentVariable("PORT") ?? "5000";
webBuilder.UseUrls($"http://0.0.0.0:{port}");

// Startup.cs
var isRender = Environment.GetEnvironmentVariable("RENDER") != null;

if (!isRender)
{
    app.UseHttpsRedirection();  // Render gerencia HTTPS no proxy
}
```

---

## Variáveis de Ambiente

```mermaid
graph LR
    subgraph "Render Dashboard"
        Env1["ConnectionStrings__ConnectionDb"]
        Env2["ConnectionStrings__dbIdentityContextConnection"]
        Env3["PORT"]
        Env4["RENDER"]
        Env5["EmailSettings__*"]
        Env6["CardapioDigital__*"]
    end

    subgraph "Aplicação"
        Startup["Startup.ObterConnectionString()"]
    end

    Env1 --> Startup
    Env2 --> Startup
    Env3 --> Startup
    Env4 --> Startup
```

---

## Ambientes

```mermaid
graph LR
    Dev["Development<br/>localhost:5000<br/>User Secrets"]
    Staging["Staging<br/>Render (branch staging)<br/>Variáveis de Ambiente"]
    Prod["Production<br/>Render (branch main)<br/>Variáveis de Ambiente"]

    Dev -->|"dotnet run"| LocalDB[("MySQL Local")]
    Staging -->|"Docker + Render"| StagingDB[("MySQL Staging")]
    Prod -->|"Docker + Render"| ProdDB[("MySQL Production")]
```

---

## Para Preencher

> **TODO:** Adicionar diagrama de CI/CD pipeline e estratégia de backup.
