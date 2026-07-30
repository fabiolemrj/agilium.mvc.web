# Como Criar um Repository

## Objetivo

Guia passo a passo para criar um novo **Repository** seguindo os padrões do projeto Agilium Manager.

---

## Decisão: EF Core ou Dapper?

```
A entidade precisa de CRUD simples? ──Sim──→ EF Core (Repository<T>)
A entidade tem consultas com 3+ joins? ──Sim──→ Dapper (*DapperRepository)
A entidade é um documento (foto, JSON)? ──Sim──→ MongoDB (RepositoryMongo<T>)
```

---

## Passo a Passo — EF Core

### 1. Criar Interface

**Local:** `agilium-manager-azure-business/Interfaces/IRepository/I{Nome}Repository.cs`

```csharp
using agilium.api.business.Interfaces;
using agilium.api.business.Models;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface I{Nome}Repository : IRepository<{Nome}>
    {
        // Métodos específicos (se necessário)
        // Task<IEnumerable<Nome>> ObterPorEmpresa(long idEmpresa);
    }
}
```

### 2. Criar Implementação

**Local:** `agilium-manager-git-azure-infra/Repository/{Nome}Repository.cs`

```csharp
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Context;

namespace agilium.api.infra.Repository
{
    public class {Nome}Repository : Repository<{Nome}>, I{Nome}Repository
    {
        public {Nome}Repository(AgiliumContext db) : base(db) { }

        // Métodos específicos (se necessário)
        // public async Task<IEnumerable<Nome>> ObterPorEmpresa(long idEmpresa)
        // {
        //     return await Buscar(x => x.IDEMPRESA == idEmpresa);
        // }
    }
}
```

### 3. Registrar no DI

**Local:** `agilum.mvc.web/Configuration/ResolveDependencyConfig.cs`

```csharp
services.AddScoped<I{Nome}Repository, {Nome}Repository>();
```

---

## Passo a Passo — Dapper

### 1. Criar Interface

**Local:** `agilium-manager-azure-business/Interfaces/IRepository/I{Nome}DapperRepository.cs`

```csharp
using agilium.api.business.Models;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface I{Nome}DapperRepository
    {
        Task<IEnumerable<Nome>> ObterListaPaginada(long idEmpresa, int page, int pageSize);
    }
}
```

### 2. Criar Implementação

**Local:** `agilium-manager-git-azure-infra/Repository/Dapper/{Nome}DapperRepository.cs`

```csharp
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.infra.Interfaces;
using Dapper;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.infra.Repository.Dapper
{
    public class {Nome}DapperRepository : I{Nome}DapperRepository
    {
        private readonly IConnectionFactory _connectionFactory;

        public {Nome}DapperRepository(IConnectionFactory connectionFactory)
        {
            _connectionFactory = connectionFactory;
        }

        public async Task<IEnumerable<Nome>> ObterListaPaginada(
            long idEmpresa, int page, int pageSize)
        {
            using var connection = _connectionFactory.CreateConnection();
            var sql = @"
                SELECT * FROM nome
                WHERE IDEMPRESA = @idEmpresa
                ORDER BY Id
                LIMIT @pageSize OFFSET @offset";

            return await connection.QueryAsync<Nome>(sql, new
            {
                idEmpresa,
                pageSize,
                offset = (page - 1) * pageSize
            });
        }
    }
}
```

### 3. Registrar no DI

```csharp
services.AddScoped<I{Nome}DapperRepository, {Nome}DapperRepository>();
```

---

## Métodos Herdados do Repository\<T\>

| Método | Tracking | Descrição |
|--------|----------|-----------|
| `Adicionar(entity)` | ✅ | Adiciona e salva |
| `AdicionarSemSalvar(entity)` | ✅ | Adiciona sem salvar |
| `Atualizar(entity)` | ✅ | Atualiza (trata detached) e salva |
| `AtualizarSemSalvar(entity)` | ✅ | Atualiza sem salvar |
| `Remover(id)` | ✅ | Remove por ID e salva |
| `RemoverSemSalvar(id)` | ✅ | Remove sem salvar |
| `ObterPorId(id)` | ✅ | FindAsync (com tracking) |
| `ObterTodos()` | ❌ | Lista completa (AsNoTracking) |
| `Buscar(predicate)` | ❌ | Filtro sem tracking |
| `Buscar(predicate, includes)` | ❌ | Filtro + includes sem tracking |
| `Obter(predicate, includes)` | ✅ | Filtro + includes com tracking |
| `ObterCompletoPorId(id, includes)` | ❌ | Por ID + includes sem tracking |
| `Existe(predicate)` | ❌ | Verifica existência |
| `SaveChanges()` | — | Persiste alterações |
| `Dispose()` | — | Libera recursos |

---

## Checklist do Repository

☐ Interface em `Interfaces/IRepository/`

☐ Implementação herda de `Repository<T>`

☐ Construtor recebe `AgiliumContext` e passa para `base(db)`

☐ Registrado como **Scoped** no `ResolveDependencyConfig.cs`

☐ Consultas somente leitura usam `Buscar()` (sem tracking)

☐ Consultas para atualização usam `ObterPorId()` ou `Obter()`

☐ Sem lógica de negócio — apenas acesso a dados

---

## Exemplos Reais

- **EF Core simples:** `ProdutoReposiotry` — apenas herda de `Repository<Produto>`, sem métodos extras
- **EF Core com métodos:** `CompraRepository` — adiciona `ObterPorFornecedor()`
- **Dapper:** `UtilDapperRepository` — `GerarUUID()`, `GerarIdInt()`
