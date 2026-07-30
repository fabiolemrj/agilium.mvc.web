# Diagrama: Persistência

## Objetivo

Representar a arquitetura da camada de persistência do Agilium Manager, mostrando os três mecanismos de acesso a dados: EF Core, Dapper e MongoDB.

---

## Visão Geral

```mermaid
graph TD
    subgraph "Business Layer"
        Service["Service<br/>Ex: CompraService"]
    end

    subgraph "Repository Layer"
        EFRepo["Repository<TEntity><br/>EF Core"]
        DapperRepo["*DapperRepository<br/>Dapper"]
        MongoRepo["RepositoryMongo<T><br/>MongoDB"]
    end

    subgraph "Data Access"
        EF["Entity Framework Core 3.1<br/>Pomelo MySQL 3.2.7"]
        DapperConn["Dapper + ConnectionFactory<br/>MySqlConnection"]
        MongoDriver["MongoDB.Driver 2.22.0<br/>IMongoDatabase"]
    end

    subgraph "Databases"
        MySQL[("MySQL 8.0<br/>~100+ tabelas")]
        Mongo[("MongoDB<br/>Fotos de usuário")]
    end

    Service --> EFRepo
    Service --> DapperRepo
    Service --> MongoRepo

    EFRepo --> EF
    DapperRepo --> DapperConn
    MongoRepo --> MongoDriver

    EF --> MySQL
    DapperConn --> MySQL
    MongoDriver --> Mongo
```

---

## DbContexts

```mermaid
graph TD
    subgraph "DbContexts"
        Agilium["AgiliumContext<br/>agilium-manager-git-azure-infra<br/>~100+ tabelas de negócio"]
        Identity["dbIdentityContext<br/>agilum.mvc.web<br/>Tabelas Identity"]
    end

    subgraph "Tabelas"
        Agilium --> Produto["produto"]
        Agilium --> Compra["compra"]
        Agilium --> Venda["venda"]
        Agilium --> Cliente["cliente"]
        Agilium --> Estoque["estoque"]
        Agilium --> Caixa["caixa"]
        Agilium --> Turno["turno"]
        Agilium --> Conta["conta_pagar / conta_receber"]
        Agilium --> Plano["plano_conta"]
        Agilium --> Fiscal["cfop / cst / ncm / cest"]
        Agilium --> Empresa["empresa"]
        Agilium --> "... 90+ outras tabelas"

        Identity --> Users["aspnetusers"]
        Identity --> Roles["aspnetroles"]
        Identity --> UserRoles["aspnetuserroles"]
        Identity --> Claims["aspnetuserclaims"]
        Identity --> Logins["aspnetuserlogins"]
        Identity --> Tokens["aspnetusertokens"]
    end
```

---

## Estratégia de Acesso

```mermaid
graph LR
    Query{Qual operação?}

    Query -->|"CRUD Simples<br/>1-2 joins"| EF["EF Core<br/>Repository<T>"]
    Query -->|"Consultas Complexas<br/>3+ joins, relatórios"| Dapper["Dapper<br/>SQL otimizado"]
    Query -->|"Documentos<br/>Fotos de usuário"| MongoDB["MongoDB<br/>GridFS"]

    EF -->|"SaveChanges()"| MySQL[("MySQL")]
    Dapper -->|"QueryAsync()"| MySQL
    MongoDB -->|"InsertOneAsync()"| Mongo[("MongoDB")]
```

---

## Repository Pattern

```mermaid
classDiagram
    class IRepository~TEntity~ {
        <<interface>>
        +Adicionar(TEntity)
        +AdicionarSemSalvar(TEntity)
        +Atualizar(TEntity)
        +AtualizarSemSalvar(TEntity)
        +Remover(long)
        +ObterPorId(long)
        +Buscar(predicate)
        +Existe(predicate)
        +SaveChanges()
        +Dispose()
    }

    class Repository~TEntity~ {
        <<abstract>>
        #Db: AgiliumContext
        #DbSet: DbSet~TEntity~
        +Adicionar() → AddAsync + SaveChanges
        +Atualizar() → trata detached
        +SaveChanges() → captura concorrência
    }

    class ProdutoReposiotry {
        +ProdutoReposiotry(AgiliumContext)
    }

    class CompraRepository {
        +ObterPorFornecedor(long)
    }

    IRepository~TEntity~ <|.. Repository~TEntity~
    Repository~TEntity~ <|-- ProdutoReposiotry
    Repository~TEntity~ <|-- CompraRepository
```

---

## Para Preencher

> **TODO:** Adicionar diagrama ER completo com principais entidades e relacionamentos.
