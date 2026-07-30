# Diagrama: Infraestrutura

## Objetivo

Representar a infraestrutura de servidores, serviços e rede que suportam o Agilium Manager.

---

## Visão Geral da Infraestrutura

```mermaid
graph TD
    subgraph "Internet"
        User["Usuário<br/>Browser"]
        APIClient["Cliente API<br/>Mobile / Integração"]
    end

    subgraph "Render Cloud"
        Proxy["Proxy / Load Balancer<br/>HTTPS Termination"]
        
        subgraph "Web Services"
            MVC["agilum.mvc.web<br/>.NET Core 3.1"]
            API["agilium-manager-azure-api<br/>.NET Core 3.1"]
            PDV["agilium-pdv-azure-api<br/>.NET Core 3.1"]
        end
    end

    subgraph "Banco de Dados"
        MySQL[("MySQL 8.0<br/>3306")]
        MongoDB[("MongoDB<br/>27017")]
    end

    subgraph "Serviços Externos"
        SMTP["Servidor SMTP<br/>E-mail"]
        Cardapio["Cardápio Digital<br/>API REST"]
    end

    User -->|"HTTPS :443"| Proxy
    APIClient -->|"HTTPS :443"| Proxy
    
    Proxy -->|"HTTP :5000"| MVC
    Proxy -->|"HTTP :5000"| API
    Proxy -->|"HTTP :5000"| PDV

    MVC -->|"3306"| MySQL
    API -->|"3306"| MySQL
    PDV -->|"3306"| MySQL

    API -->|"27017"| MongoDB

    MVC -->|"SMTP"| SMTP
    MVC -->|"HTTPS"| Cardapio
```

---

## Serviços e Portas

| Serviço | Porta | Protocolo | Origem |
|---------|-------|-----------|--------|
| MVC Web | 5000 | HTTP | Proxy Render |
| API | 5000 | HTTP | Proxy Render |
| PDV API | 5000 | HTTP | Proxy Render |
| MySQL | 3306 | TCP | Web Services |
| MongoDB | 27017 | TCP | API |
| SMTP | 587 | TCP | MVC |
| HTTPS (Proxy) | 443 | HTTPS | Internet |

---

## Recursos Computacionais

```mermaid
graph LR
    subgraph "Web Service (Render)"
        CPU["CPU<br/>Shared"]
        RAM["RAM<br/>512 MB - 1 GB"]
        Disk["Disk<br/>Ephemeral"]
    end

    subgraph "MySQL"
        DBCPU["CPU<br/>Dedicated"]
        DBRAM["RAM<br/>1-4 GB"]
        DBStorage["Storage<br/>SSD 10-100 GB"]
        Backup["Backup<br/>Automático"]
    end
```

---

## Monitoramento e Logs

```mermaid
graph TD
    App["Aplicação"] --> KissLog["KissLog<br/>Logging"]
    App --> LogService["ILogService<br/>LogSistema / LogErro"]
    App --> Console["Console Logging<br/>stdout/stderr"]

    KissLog --> Dashboard["KissLog Dashboard"]
    LogService --> DB[("MySQL<br/>Tabelas de log")]
    Console --> RenderLogs["Render Logs<br/>Web dashboard"]
```

---

## Para Preencher

> **TODO:** Adicionar diagrama de rede com subnets, firewall e políticas de segurança.
