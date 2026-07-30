# Padrão Repository

## Objetivo

Documentar o **Repository Pattern** implementado no projeto Agilium Manager, que abstrai o acesso a dados e desacopla a camada de negócio da infraestrutura de persistência.

---

## Visão Geral

O padrão Repository isola a lógica de acesso a dados em uma camada dedicada (`agilium-manager-git-azure-infra`), permitindo que a camada de negócio trabalhe com abstrações (interfaces) sem conhecer os detalhes de EF Core, Dapper ou MongoDB.

---

## Arquitetura

```
Business (Service)
      │
      ▼  (depende de)
Interfaces/IRepository/IEntidadeRepository.cs
      ▲
      │  (implementa)
Infra/Repository/EntidadeRepository.cs
      │
      ▼  (usa)
EF Core (Repository<TEntity>)  ou  Dapper  ou  MongoDB
```

---

## Estrutura de Pastas

```
agilium-manager-azure-business/
└── Interfaces/
    ├── IRepository.cs              # Genérico
    ├── IRepositoryMongo.cs         # MongoDB
    └── IRepository/
        ├── IProdutoRepository.cs
        ├── ICompraRepository.cs
        └── ...                     # 30+ interfaces específicas

agilium-manager-git-azure-infra/
└── Repository/
    ├── Repository.cs               # Genérico EF Core (classe abstrata)
    ├── RepositoryMongo.cs          # MongoDB
    ├── Dapper/
    │   ├── ProdutoDapper.cs
    │   └── UtilDapperRepository.cs
    ├── ProdutoReposiotry.cs        # EF Core (contém múltiplas classes)
    ├── CompraRepository.cs
    └── ...                         # 40+ repositórios específicos
```

---

## Interface Genérica (`IRepository<TEntity>`)

Local: `agilium-manager-azure-business/Interfaces/IRepository.cs`

```csharp
public interface IRepository<TEntity> : IDisposable where TEntity : Entity
{
    // ========== CONSULTAS ==========
    Task<TEntity> ObterPorId(long id);
    Task<TEntity> ObterCompletoPorId(long id, params string[] includes);
    Task<List<TEntity>> ObterTodos();
    Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate);
    Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate,
                                      params string[] includes);
    Task<IEnumerable<TEntity>> Obter(Expression<Func<TEntity, bool>> predicate,
                                     params string[] includes);
    Task<bool> Existe(Expression<Func<TEntity, bool>> predicate);
    Task<string> GerarCodigo(string sql);
    Task<TEntity> GerarCodigoPorSql(string sql);

    // ========== COMANDOS ==========
    Task Adicionar(TEntity entity);
    Task AdicionarSemSalvar(TEntity entity);
    Task AdicionarLista(IEnumerable<TEntity> entity);
    void AdicionarSincrona(TEntity entity);

    Task Atualizar(TEntity entity);
    Task AtualizarSemSalvar(TEntity entity);
    Task AtualizarLista(IEnumerable<TEntity> entity);
    Task AtualizarComSetValues(TEntity entity, object model);
    Task Atualizar2(TEntity entity, object key);
    void AtualizarSincrona(TEntity entity);

    Task Remover(long id);
    Task RemoverSemSalvar(TEntity entity);
    Task RemoverSemSalvar(long id);
    Task RemoverSemSalvar(IEnumerable<TEntity> entity);
    void RemoverSincrona(TEntity entity);

    // ========== PERSISTÊNCIA ==========
    Task<int> SaveChanges();
}
```

### Interface `IUtilDapperRepository`

```csharp
public interface IUtilDapperRepository
{
    Task<long> GerarUUID();
    Task<string> ConfigRetornaValor(string valor, long? idEmpresa);
    Task<string> GerarCodigo(string sql);
    Task<int> GerarIdInt(string generator);
    Task<DateTime> ObterDataAtual();
}
```

---

## Repository Genérico (`Repository<TEntity>`)

Local: `agilium-manager-git-azure-infra/Repository/Repository.cs`

### Consultas

```csharp
public abstract class Repository<TEntity> : IRepository<TEntity>
    where TEntity : Entity, new()
{
    protected readonly AgiliumContext Db;
    protected readonly DbSet<TEntity> DbSet;

    protected Repository(AgiliumContext db)
    {
        Db = db;
        DbSet = db.Set<TEntity>();
    }

    /// <summary>Consulta sem tracking: listagens e leitura.</summary>
    public async Task<IEnumerable<TEntity>> Buscar(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.AsNoTracking().Where(predicate).ToListAsync();
    }

    /// <summary>Consulta com tracking: ideal para atualização.</summary>
    public async Task<TEntity> ObterPorId(long id)
    {
        return await DbSet.FindAsync(id);  // FindAsync usa cache do change tracker
    }

    /// <summary>Consulta com includes (sem tracking).</summary>
    public async Task<IEnumerable<TEntity>> Buscar(
        Expression<Func<TEntity, bool>> predicate, params string[] includes)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking().Where(predicate);
        foreach (var inc in includes)
            query = query.Include(inc);
        return await query.ToListAsync();
    }

    /// <summary>Entidade completa por ID (sem tracking + includes).</summary>
    public async Task<TEntity> ObterCompletoPorId(long id, params string[] includes)
    {
        IQueryable<TEntity> query = DbSet.AsNoTracking().Where(x => x.Id == id);
        foreach (var inc in includes)
            query = query.Include(inc);
        return await query.FirstOrDefaultAsync();
    }

    /// <summary>Consulta com tracking + includes (para atualização).</summary>
    public async Task<IEnumerable<TEntity>> Obter(
        Expression<Func<TEntity, bool>> predicate, params string[] includes)
    {
        IQueryable<TEntity> query = DbSet.Where(predicate);
        foreach (var inc in includes)
            query = query.Include(inc);
        return await query.ToListAsync();
    }

    public async Task<bool> Existe(Expression<Func<TEntity, bool>> predicate)
    {
        return await DbSet.AsNoTracking().AnyAsync(predicate);
    }

    public virtual async Task<List<TEntity>> ObterTodos()
    {
        return await DbSet.AsNoTracking().ToListAsync();
    }
```

### Comandos

```csharp
    /// <summary>Adiciona E salva imediatamente.</summary>
    public async Task Adicionar(TEntity entity)
    {
        await DbSet.AddAsync(entity);
        await SaveChanges();                 // ← Salva imediatamente!
    }

    /// <summary>Adiciona sem salvar (para operações em lote).</summary>
    public async Task AdicionarSemSalvar(TEntity entity)
    {
        await DbSet.AddAsync(entity);
    }

    public async Task AdicionarLista(IEnumerable<TEntity> entities)
    {
        await DbSet.AddRangeAsync(entities);
    }
```

### Atualização com Entidade Detached

```csharp
    /// <summary>Atualiza E salva. Trata entidades detached automaticamente.</summary>
    public async Task Atualizar(TEntity entity)
    {
        await AtualizarSemSalvar(entity);
        await SaveChanges();
    }

    /// <summary>
    /// Atualização segura: resolve entidades detached.
    /// Se a entidade não está sendo rastreada, busca a versão do banco
    /// e aplica os novos valores sobre ela.
    /// </summary>
    public async Task AtualizarSemSalvar(TEntity entity)
    {
        var entry = Db.Entry(entity);

        if (entry.State == EntityState.Detached)
        {
            var noBanco = await DbSet.FindAsync(entity.Id);

            if (noBanco != null)
            {
                // Copia valores da entidade recebida para a rastreada
                var entryBanco = Db.Entry(noBanco);
                entryBanco.CurrentValues.SetValues(entity);

                // Força Modified quando AutoDetectChangesEnabled = false
                if (entryBanco.State == EntityState.Unchanged)
                    entryBanco.State = EntityState.Modified;
            }
            else
            {
                // Entidade nova: attach e marca como modified
                DbSet.Attach(entity);
                entry.State = EntityState.Modified;
            }
        }
    }
```

### Remoção

```csharp
    /// <summary>Remove por ID E salva.</summary>
    public async Task Remover(long id)
    {
        DbSet.Remove(new TEntity { Id = id });
        await SaveChanges();
    }

    /// <summary>Remove sem salvar (para operações em lote).</summary>
    public async Task RemoverSemSalvar(long id)
    {
        DbSet.Remove(new TEntity { Id = id });
    }

    public async Task RemoverSemSalvar(TEntity entity)
    {
        DbSet.Remove(entity);
    }

    public async Task RemoverSemSalvar(IEnumerable<TEntity> entities)
    {
        DbSet.RemoveRange(entities);
    }
```

### Persistência

```csharp
    public async Task<int> SaveChanges()
    {
        try
        {
            return await Db.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException ex)
        {
            throw new DbUpdateConcurrencyException(
                "Erro de concorrência ao salvar a entidade. " +
                "Dados podem ter sido alterados por outro processo.", ex);
        }
    }

    public void Dispose()
    {
        Db?.Dispose();
    }
```

---

## Repositórios Específicos

### Padrão 1: Herança simples (mais comum)

A maioria dos repositórios **não adiciona métodos customizados** — apenas herdam de `Repository<T>`:

```csharp
// agilium-manager-git-azure-infra/Repository/ProdutoReposiotry.cs

public class ProdutoReposiotry : Repository<Produto>, IProdutoRepository
{
    public ProdutoReposiotry(AgiliumContext db) : base(db) { }
}

public class ProdutoDepartamentoRepository
    : Repository<ProdutoDepartamento>, IProdutoDepartamentoRepository
{
    public ProdutoDepartamentoRepository(AgiliumContext db) : base(db) { }
}

public class ProdutoMarcaRepository
    : Repository<ProdutoMarca>, IProdutoMarcaRepository
{
    public ProdutoMarcaRepository(AgiliumContext db) : base(db) { }
}

public class GrupoProdutoRepository
    : Repository<GrupoProduto>, IGrupoProdutoRepository
{
    public GrupoProdutoRepository(AgiliumContext db) : base(db) { }
}
```

> 📁 **Múltiplas classes no mesmo arquivo**: `ProdutoReposiotry.cs` contém 8 classes de repositório relacionadas a produtos.

### Padrão 2: Com métodos customizados (específicos)

Quando necessário, o repositório adiciona consultas específicas:

```csharp
public class CompraRepository : Repository<Compra>, ICompraRepository
{
    public CompraRepository(AgiliumContext db) : base(db) { }

    public async Task<IEnumerable<Compra>> ObterPorFornecedor(long idFornecedor)
    {
        return await Buscar(c => c.IDFORN == idFornecedor);
    }
}
```

---

## `IUtilDapperRepository` — Utilitários

```csharp
// agilium-manager-git-azure-infra/Repository/Dapper/UtilDapperRepository.cs
public class UtilDapperRepository : IUtilDapperRepository
{
    private readonly IConnectionFactory _connectionFactory;

    public UtilDapperRepository(IConnectionFactory connectionFactory)
    {
        _connectionFactory = connectionFactory;
    }

    public async Task<long> GerarUUID()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<long>("SELECT UUID_SHORT()");
    }

    public async Task<int> GerarIdInt(string generator)
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<int>(
            "SELECT NEXT VALUE FOR @generator", new { generator });
    }

    public async Task<DateTime> ObterDataAtual()
    {
        using var connection = _connectionFactory.CreateConnection();
        return await connection.ExecuteScalarAsync<DateTime>("SELECT NOW()");
    }
}
```

---

## MongoDB Repository

```csharp
public class RepositoryMongo<T> : IRepositoryMongo<T>
{
    protected readonly IMongoCollection<T> _collection;

    public RepositoryMongo(IMongoDatabase database, string collectionName)
    {
        _collection = database.GetCollection<T>(collectionName);
    }
}
```

> Usado para `UsuarioFotoRepositoryMongo` — fotos de perfil armazenadas no MongoDB.

---

## Registro no DI

```csharp
// agilum.mvc.web/Configuration/ResolveDependencyConfig.cs
services.AddScoped<AgiliumContext>();
services.AddScoped<DbSession>();
services.AddScoped<CardapioDigitalDbSession>();

// Repositórios EF Core
services.AddScoped<IProdutoRepository, ProdutoReposiotry>();
services.AddScoped<ICompraRepository, CompraRepository>();
services.AddScoped<IEmpresaRepository, EmpresaRepository>();
// ... 40+ registros

// Dapper
services.AddScoped<IUtilDapperRepository, UtilDapperRepository>();
services.AddScoped<IProdutoDapper, ProdutoDapper>();
```

> Todos os repositórios são **Scoped** — uma instância por requisição HTTP.

---

## Uso nos Serviços

```csharp
public class ProdutoService : BaseService, IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;

    public ProdutoService(IProdutoRepository produtoRepository,
                          INotificador notificador) : base(notificador)
    {
        _produtoRepository = produtoRepository;
    }

    public async Task Adicionar(Produto produto)
    {
        // Validações...
        await _produtoRepository.Adicionar(produto);  // Salva imediatamente
    }
}
```

---

## Resumo dos Métodos por Categoria

### Consultas

| Método | Tracking | Includes | Uso Típico |
|--------|----------|----------|------------|
| `ObterPorId(id)` | ✅ Sim | ❌ | Atualização de entidade única |
| `ObterTodos()` | ❌ `AsNoTracking` | ❌ | Listagem completa |
| `Buscar(predicate)` | ❌ `AsNoTracking` | ❌ | Filtro simples, leitura |
| `Buscar(predicate, includes)` | ❌ `AsNoTracking` | ✅ | Leitura com relacionamentos |
| `Obter(predicate, includes)` | ✅ Sim | ✅ | Atualização com relacionamentos |
| `ObterCompletoPorId(id, includes)` | ❌ `AsNoTracking` | ✅ | Detalhe completo para tela |
| `Existe(predicate)` | ❌ `AsNoTracking` | ❌ | Validação de unicidade |

### Comandos

| Método | Salva? | Uso Típico |
|--------|--------|------------|
| `Adicionar(entity)` | ✅ Sim | Inserção única |
| `AdicionarSemSalvar(entity)` | ❌ Não | Parte de lote |
| `AdicionarLista(entities)` | ❌ Não | Inserção em massa |
| `Atualizar(entity)` | ✅ Sim | Atualização única (trata detached) |
| `AtualizarSemSalvar(entity)` | ❌ Não | Parte de lote |
| `AtualizarLista(entities)` | ❌ Não | Atualização em massa |
| `Remover(id)` | ✅ Sim | Exclusão única |
| `RemoverSemSalvar(...)` | ❌ Não | Parte de lote |

---

## Boas Práticas

| Prática | Motivo |
|---------|--------|
| Interface na camada Business | Inversão de dependência |
| Implementação na camada Infra | Isolamento do provider de dados |
| `Repository<TEntity>` genérico | Evita duplicação de CRUD |
| Scoped no DI | DbContext por requisição |
| `AdicionarSemSalvar` + `AtualizarSemSalvar` | Controle de transação em lote |
| `Atualizar` trata entidade detached | Evita `InvalidOperationException` |
| `SaveChanges` captura concorrência | Mensagem amigável ao usuário |
| `ObterTodos` usa `AsNoTracking` | Performance em listagens |

---

## Anti-Padrões

| Evitar | Por que | Alternativa |
|--------|---------|-------------|
| Chamar `Adicionar` em loop | Cada chamada faz `SaveChanges` | Usar `AdicionarSemSalvar` + `SaveChanges` único |
| Acessar Repository direto do Controller | Viola separação de camadas | Passar pelo Service |
| Lógica de negócio no Repository | Repository é só acesso a dados | Mover para o Service |
| Usar `Obter` (com tracking) para listagem | Overhead desnecessário | Usar `Buscar` (sem tracking) |
| Esquecer `Dispose` do DbContext | Vazamento de conexão | DI Scoped gerencia ciclo de vida |

---

## Checklist

Antes de criar/alterar um Repository:

☐ Interface definida em `Interfaces/IRepository/`

☐ Classe herda de `Repository<TEntity>`

☐ Construtor recebe `AgiliumContext` e passa para `base(db)`

☐ Consultas somente leitura usam `Buscar()` (sem tracking)

☐ Consultas para atualização usam `ObterPorId()` ou `Obter()` (com tracking)

☐ Operações em lote usam `...SemSalvar` + `SaveChanges()` único

☐ Registrado como **Scoped** no `ResolveDependencyConfig.cs`

☐ Sem lógica de negócio — apenas acesso a dados
| Dapper separado do EF Core | Clareza de propósito |

---

## Anti-Padrões

| Evitar | Por que |
|--------|---------|
| Acessar Repository direto do Controller | Viola separação de camadas |
| Lógica de negócio no Repository | Repository é só acesso a dados |
| Retornar IQueryable | Vaza abstração do banco |
| Múltiplos SaveChanges | Perde atomicidade |
