# Diagrama: Integrações

## Objetivo

Representar as integrações externas do Agilium Manager com outros sistemas e serviços.

---

## Mapa de Integrações

```mermaid
graph TD
    subgraph "Agilium Manager"
        MVC["agilum.mvc.web<br/>MVC Web"]
        API["agilium-manager-azure-api<br/>REST API"]
        PDV["agilium-pdv-azure-api<br/>PDV API"]
    end

    subgraph "Bancos de Dados"
        MySQL[("MySQL 8.0<br/>Pomelo EF Core")]
        MongoDB[("MongoDB<br/>Fotos")]
    end

    subgraph "Integrações Externas"
        Cardapio["Cardápio Digital<br/>API REST"]
        Email["Servidor SMTP<br/>E-mail"]
        NFeXML["NF-e<br/>Importação XML"]
        Marketplace["Site Mercado<br/>Marketplace"]
        AD["Active Directory<br/>System.DirectoryServices"]
        Render["Render Cloud<br/>PaaS"]
        WhatsApp["WhatsApp<br/>Mensageria"]
    end

    MVC --> MySQL
    MVC --> MongoDB
    API --> MySQL
    PDV --> MySQL

    MVC -->|"IntegracaoCardapioService"| Cardapio
    MVC -->|"ServiceEmail"| Email
    MVC -->|"ImportarXML NFe"| NFeXML
    MVC -->|"SiteMercadoService"| Marketplace
    API -->|"Autenticação"| AD
    MVC -->|"Deploy"| Render
    API -->|"Deploy"| Render
    MVC -->|"Notificações"| WhatsApp
```

---

## Cardápio Digital

```mermaid
sequenceDiagram
    participant MVC
    participant CardapioService
    participant CardapioAPI

    MVC->>CardapioService: ExportarParaCardapio()
    CardapioService->>CardapioService: Obter produtos marcados
    CardapioService->>CardapioAPI: POST /api/produtos
    Note over CardapioAPI: ConnectionString + ApiBaseUrl<br/>do appsettings.json
    CardapioAPI-->>CardapioService: OK
    CardapioService-->>MVC: Sincronização concluída
```

## Importação NFe

```mermaid
sequenceDiagram
    participant MVC
    participant CompraService
    participant FileSystem
    participant XMLParser

    MVC->>CompraService: ImportarXML(arquivo)
    CompraService->>FileSystem: Ler arquivo XML
    FileSystem-->>CompraService: byte[]
    CompraService->>XMLParser: XmlSerializer.Deserialize()
    XMLParser-->>CompraService: NFeProc (dados da nota)
    CompraService->>CompraService: Popular Compra + Itens
    CompraService->>CompraService: Cadastro automático de produtos
    CompraService-->>MVC: Compra preenchida
```

---

## E-mail

```mermaid
graph LR
    MVC["agilum.mvc.web"] --> ServiceEmail["ServiceEmail"]
    ServiceEmail --> Config["EmailSettings<br/>appsettings.json"]
    Config --> SMTP["Servidor SMTP<br/>PrimaryDomain:Port"]
    SMTP --> Dest["Destinatários<br/>FromEmail, ToEmail, CcEmail"]
```

---

## Para Preencher

> **TODO:** Adicionar diagramas de integração com WhatsApp, Active Directory e Marketplace.
