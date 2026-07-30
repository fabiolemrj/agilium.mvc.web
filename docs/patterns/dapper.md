# Padrão Dapper

## Objetivo

Documentar o uso do **Dapper** como micro-ORM complementar ao Entity Framework Core no projeto Agilium Manager, incluindo quando utilizá-lo, como configurá-lo e padrões de consulta.

---

## Visão Geral

O Dapper é utilizado para **consultas complexas e de alta performance** onde o EF Core seria ineficiente ou geraria queries excessivamente complexas (múltiplos joins, agregações, relatórios).

---

## Quando Usar Dapper vs EF Core

| Cenário | Ferramenta |
|---------|------------|
| CRUD simples (1 entidade) | EF Core |
| Consulta com 1-2 joins | EF Core |
| Consulta com 3+ joins | **Dapper** |
| Relatórios e agregações | **Dapper** |
| Paginação complexa | **Dapper** |
| Stored Procedures | **Dapper** |
| Operações em lote (bulk) | **Dapper** |

---

## Localização no Projeto

```
agilium-manager-git-azure-infra/
├── Repository/
│   └── Dapper/
│       ├── ProdutoDapper.cs
│       ├── UtilDapperRepository.cs
│       └── ...
├── ViewModelDapper/          # DTOs específicos para queries Dapper
└── Interfaces/
    └── IDapperRepository.cs
```

---

## Configuração

### Connection Factory

```csharp
// agilium-manager-git-azure-infra/Context/ConnectionFactory.cs
public class ConnectionFactory : IConnectionFactory
{
    private readonly string _connectionString;

    public ConnectionFactory(string connectionString)
    {
        _connectionString = connectionString;
    }

    public IDbConnection CreateConnection()
        => new MySqlConnection(_connectionString);
}
```

### Registro no DI

```csharp
// ResolveDependencyConfig.cs
services.AddScoped<IUtilDapperRepository, UtilDapperRepository>();
services.AddScoped<IProdutoDapper, ProdutoDapper>();
services.AddScoped<DbSession>();
```

---

## Padrões de Consulta

### Query Simples

```csharp
public async Task<IEnumerable<Produto>> ObterTodos(long idEmpresa)
{
    using var connection = _connectionFactory.CreateConnection();
    var sql = "SELECT * FROM produto WHERE IDEMPRESA = @idEmpresa";
    return await connection.QueryAsync<Produto>(sql, new { idEmpresa });
}
```

### Query com Múltiplos Joins (Multi-Mapping)

```csharp
var sql = @"
    SELECT p.*, c.*
    FROM produto p
    INNER JOIN categoria c ON p.IDCATEGORIA = c.Id
    WHERE p.IDEMPRESA = @idEmpresa";

var produtos = await connection.QueryAsync<Produto, Categoria, Produto>(
    sql,
    (produto, categoria) =>
    {
        produto.Categoria = categoria;
        return produto;
    },
    new { idEmpresa },
    splitOn: "Id"
);
```

### Paginação

```csharp
public async Task<PagedResult<CompraViewModel>> ObterCompraPaginado(
    long idEmpresa, DateTime dtIni, DateTime dtFim, int page, int pageSize)
{
    using var connection = _connectionFactory.CreateConnection();

    var sqlCount = "SELECT COUNT(*) FROM compra WHERE IDEMPRESA = @idEmpresa ...";
    var total = await connection.ExecuteScalarAsync<int>(sqlCount, new { idEmpresa, dtIni, dtFim });

    var sqlData = @"SELECT ... FROM compra ... LIMIT @pageSize OFFSET @offset";
    var items = await connection.QueryAsync<CompraViewModel>(sqlData,
        new { idEmpresa, dtIni, dtFim, pageSize, offset = (page - 1) * pageSize });

    return new PagedResult<CompraViewModel>
    {
        List = items.ToList(),
        TotalResults = total,
        PageIndex = page,
        PageSize = pageSize
    };
}
```

---

## Interfaces Dapper no Projeto

| Interface | Responsabilidade |
|-----------|------------------|
| `IUtilDapperRepository` | Utilitários (GerarUUID, GerarIdInt, consultas genéricas) |
| `IProdutoDapper` | Consultas otimizadas de produtos |
| `IDapperRepository<T>` | Repositório Dapper genérico |

---

## Boas Práticas

- **Sempre usar `using`** para a conexão — o Dapper não gerencia ciclo de vida
- Usar **DTOs específicos** (`ViewModelDapper/`) em vez de entidades do domínio
- Parametrizar queries com **objetos anônimos** (`new { id }`) — previne SQL injection
- Usar `splitOn` no multi-mapping para indicar onde começa o próximo objeto
- Para operações de escrita, usar transações:
  ```csharp
  using var transaction = connection.BeginTransaction();
  // ... operações ...
  transaction.Commit();
  ```

---

## Pontos de Atenção

- Dapper **não substitui** o EF Core — são complementares
- Queries Dapper são **strings SQL** — sem validação em tempo de compilação
- Mudanças no schema do banco podem quebrar queries Dapper silenciosamente
- Manter queries Dapper em classes dedicadas (`*Dapper.cs`), não nos serviços
