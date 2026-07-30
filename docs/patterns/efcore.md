# Padrão Entity Framework Core

## Objetivo

Documentar o uso do **Entity Framework Core 3.1** como ORM principal do projeto Agilium Manager, incluindo configuração de DbContext, mapeamentos, migrations, consultas e boas práticas.

---

## Visão Geral

O EF Core é o ORM padrão para **operações CRUD e consultas de baixa/média complexidade**. Ele trabalha em conjunto com o Dapper, que é reservado para consultas complexas.

---

## DbContexts

O projeto utiliza **dois DbContexts** separados:

| DbContext | Projeto | Banco | Finalidade |
|-----------|---------|-------|------------|
| `AgiliumContext` | `agilium-manager-git-azure-infra` | MySQL | Dados de negócio (~100+ tabelas) |
| `dbIdentityContext` | `agilum.mvc.web` | MySQL | Tabelas Identity (aspnetusers, roles, claims) |

---

## Configuração

### Registro no Startup

```csharp
// Startup.cs — ConfigureServices
services.AddDbContext<AgiliumContext>(options =>
{
    options.UseMySql(
        ObterConnectionString("ConnectionDb"),
        b => b.MigrationsAssembly("agilium.mvc.web"));
    options.EnableSensitiveDataLogging(true);
    options.EnableDetailedErrors(true);
});
```

### Provider MySQL

- **Pomelo.EntityFrameworkCore.MySql 3.2.7**
- Suporta MySQL 8.0
- Migrations via `Microsoft.EntityFrameworkCore.Tools`

---

## Mapeamento de Entidades

### Fluent API (recomendado)

```csharp
// agilium-manager-git-azure-infra/Mappings/
public class ProdutoMapping : IEntityTypeConfiguration<Produto>
{
    public void Configure(EntityTypeBuilder<Produto> builder)
    {
        builder.ToTable("produto");
        builder.HasKey(p => p.Id);
        builder.Property(p => p.NMPRODUTO).IsRequired().HasMaxLength(100);
        builder.HasOne(p => p.Empresa).WithMany().HasForeignKey(p => p.IDEMPRESA);
    }
}

// No OnModelCreating:
modelBuilder.ApplyConfiguration(new ProdutoMapping());
// ou
modelBuilder.ApplyConfigurationsFromAssembly(typeof(AgiliumContext).Assembly);
```

### Data Annotations (alternativo)

```csharp
[Table("produto")]
public class Produto
{
    [Key]
    public long Id { get; set; }

    [Required, MaxLength(100)]
    public string NMPRODUTO { get; set; }
}
```

---

## Padrões de Consulta

### Leitura (Comum)

```csharp
// ⚠️ Sempre usar AsNoTracking() em consultas somente leitura
var produtos = await _context.Produtos
    .AsNoTracking()
    .Where(p => p.IDEMPRESA == idEmpresa && p.Ativo)
    .OrderBy(p => p.NMPRODUTO)
    .ToListAsync();
```

### Com Includes (Eager Loading)

```csharp
var compra = await _context.Compras
    .Include(c => c.Itens)
        .ThenInclude(i => i.Produto)
    .Include(c => c.Fornecedor)
    .FirstOrDefaultAsync(c => c.Id == id);
```

### Escrita

```csharp
// Adicionar
_context.Produtos.Add(produto);
await _context.SaveChangesAsync();

// Atualizar
_context.Produtos.Update(produto);
await _context.SaveChangesAsync();
```

---

## Repository Pattern com EF Core

```csharp
// Repository<T> genérico
public abstract class Repository<T> : IRepository<T> where T : Entity
{
    protected readonly AgiliumContext Db;
    protected readonly DbSet<T> DbSet;

    public Repository(AgiliumContext db)
    {
        Db = db;
        DbSet = db.Set<T>();
    }

    public virtual async Task Adicionar(T entity) => DbSet.Add(entity);
    public virtual async Task Atualizar(T entity) => DbSet.Update(entity);
    public virtual async Task<T> ObterPorId(long id) => await DbSet.FindAsync(id);
    public virtual async Task<IEnumerable<T>> ObterTodos() => await DbSet.ToListAsync();
    public async Task Salvar() => await Db.SaveChangesAsync();
}
```

---

## Migrations

### Gerar Migration

```bash
dotnet ef migrations add NomeDaMigration --context AgiliumContext \
    --project agilium-manager-git-azure-infra \
    --startup-project agilum.mvc.web
```

### Aplicar

```bash
dotnet ef database update --context AgiliumContext \
    --project agilium-manager-git-azure-infra \
    --startup-project agilum.mvc.web
```

---

## Boas Práticas

| Prática | Motivo |
|---------|--------|
| `.AsNoTracking()` em leituras | Evita overhead do change tracker |
| `.Include()` explícito | Previne lazy loading surpresa |
| `SaveChanges()` único por operação | Atomicidade |
| DbContext Scoped | Uma instância por request |
| Fluent API em classes separadas | Organização e reuso |

---

## Anti-Padrões

| Evitar | Por que | Alternativa |
|--------|---------|-------------|
| `.Result` / `.Wait()` | Deadlock | Usar `async/await` |
| Lazy Loading | N+1 queries | Eager loading com `.Include()` |
| `SaveChanges()` em loop | Performance | Salvar em lote |
| DbContext Singleton | Concorrência | Usar Scoped |
| SQL raw para CRUD | Perde rastreamento | Usar DbSet/LINQ |

---

## Segurança

### Proteção contra SQL Injection

O EF Core com **LINQ parametrizado** previne SQL injection automaticamente:

```csharp
// ✅ Seguro — LINQ gera query parametrizada
var produto = await _context.Produtos
    .Where(p => p.NMPRODUTO == nome)
    .FirstOrDefaultAsync();

// ⚠️ Use com cautela — apenas para cenários onde LINQ não é viável
var produto = await _context.Produtos
    .FromSqlRaw("SELECT * FROM produto WHERE NMPRODUTO = {0}", nome)
    .FirstOrDefaultAsync();
```

### Connection Strings

```csharp
// ✅ Em produção: variáveis de ambiente (nunca hardcoded)
var connStr = Environment.GetEnvironmentVariable("ConnectionStrings__ConnectionDb");

// ❌ NUNCA: hardcoded no código ou appsettings commitado
var connStr = "Server=localhost;Database=agilium;Uid=root;Pwd=123456;";
```

### Dados Sensíveis

- `EnableSensitiveDataLogging(true)` **deve ser desabilitado em produção** — expõe valores de parâmetros nos logs
- Usar `.Ignore()` no mapeamento para propriedades que não devem ser persistidas (senhas em texto plano, tokens)
- Propriedades de entidades mapeadas para o banco não devem expor dados sensíveis em `ToString()` ou serialização

---

## Performance

### Estratégias de Otimização

| Técnica | Impacto | Quando Usar |
|---------|---------|-------------|
| `.AsNoTracking()` | Reduz ~30% overhead | Todas as consultas somente leitura |
| `.Include()` explícito | Evita N+1 queries | Quando precisa de dados relacionados |
| `.Select()` com projeção | Traz só colunas necessárias | Listagens e grids |
| Paginação (`Skip/Take`) | Limita dados trafegados | Toda listagem com muitos registros |
| `SaveChanges()` em lote | Reduz round-trips ao banco | Múltiplas inserções/atualizações |

### Projeção (Select)

```csharp
// ✅ Performance: traz só as colunas necessárias
var lista = await _context.Produtos
    .AsNoTracking()
    .Where(p => p.Ativo)
    .Select(p => new ProdutoListaDto
    {
        Id = p.Id,
        Nome = p.NMPRODUTO,
        Preco = p.VLPRECO
    })
    .ToListAsync();

// ❌ Evitar: trazer entidade completa para listagens
var lista = await _context.Produtos
    .Where(p => p.Ativo)
    .ToListAsync(); // Traz TODAS as colunas + tracking
```

### Paginação Eficiente

```csharp
public async Task<PagedResult<Produto>> ObterPaginado(int page, int pageSize)
{
    var query = _context.Produtos.AsNoTracking().Where(p => p.Ativo);

    var total = await query.CountAsync();
    var items = await query
        .OrderBy(p => p.NMPRODUTO)
        .Skip((page - 1) * pageSize)
        .Take(pageSize)
        .ToListAsync();

    return new PagedResult<Produto> { List = items, TotalResults = total };
}
```

### Quando Delegar ao Dapper

| Cenário | Ferramenta |
|---------|------------|
| Consulta com 3+ joins e agregações | **Dapper** |
| Relatórios com muitas colunas de tabelas diferentes | **Dapper** |
| Stored Procedures | **Dapper** |
| Bulk insert/update | **Dapper** |

---

## Auditoria

### Log de Operações

O projeto integra EF Core com `ILogService` para registrar operações:

```csharp
// No Controller (MainController)
LogInformacao($"Produto criado: {Deserializar(produto)}", "Produto", "Adicionar", null);
LogErro(ex.Message, "Produto", "Adicionar", null, "Web");
```

### Rastreamento de Alterações

```csharp
// EF Core Change Tracker pode ser usado para auditoria
var entidadesModificadas = _context.ChangeTracker.Entries()
    .Where(e => e.State == EntityState.Modified || e.State == EntityState.Added);

foreach (var entry in entidadesModificadas)
{
    // Registrar: entidade, estado, propriedades alteradas, usuário, data
    _logService.RegistrarAuditoria(entry);
}
```

### Campos de Auditoria Recomendados

Toda entidade que requer auditoria deve possuir:

- `DataCadastro` — data de criação
- `DataAlteracao` — data da última modificação
- `UsuarioCadastro` — quem criou
- `UsuarioAlteracao` — quem modificou

---

## Tratamento de Erros

### Exceções Comuns do EF Core

| Exceção | Causa | Tratamento |
|---------|-------|------------|
| `DbUpdateException` | Violação de constraint, FK inválida | Validar antes de salvar, capturar e logar |
| `DbUpdateConcurrencyException` | Conflito de concorrência | Implementar retry ou resolver merge |
| `SqlException` (via Pomelo) | Timeout, conexão recusada | Polly retry, notificar usuário |
| `InvalidOperationException` | Query inválida, disposed context | Revisar ciclo de vida do DbContext |

### Padrão de Tratamento

```csharp
try
{
    await _context.SaveChangesAsync();
}
catch (DbUpdateException ex)
{
    _logger.Error(ex, "Erro ao salvar alterações no banco");
    Notificar("Erro ao salvar os dados. Verifique as informações e tente novamente.");
    // NÃO repassar a exceção para o cliente com detalhes internos
}
```

### Resiliência com Polly

```csharp
// Configurado no projeto para chamadas HTTP — pode ser estendido para EF Core
services.AddHttpClient("MyClient")
    .AddPolicyHandler(HttpPolicyExtensions
        .HandleTransientHttpError()
        .WaitAndRetryAsync(3, retryAttempt =>
            TimeSpan.FromSeconds(Math.Pow(2, retryAttempt))));
```

---

## Dependências

### Pacotes NuGet — EF Core

| Pacote | Versão | Projeto |
|--------|--------|---------|
| `Microsoft.EntityFrameworkCore` | 3.1.32 | Business + Infra |
| `Microsoft.EntityFrameworkCore.Relational` | 3.1.32 | Business + Infra |
| `Microsoft.EntityFrameworkCore.Tools` | 3.1.32 | Infra (migrations) |
| `Microsoft.EntityFrameworkCore.Design` | 3.1.32 | Infra (design-time) |
| `Pomelo.EntityFrameworkCore.MySql` | 3.2.7 | MVC + Infra |
| `Microsoft.EntityFrameworkCore.SqlServer` | 3.1.32 | (referenciado, pouco usado) |
| `Microsoft.EntityFrameworkCore.Sqlite` | 3.1.32 | (referenciado, pouco usado) |

### Dependências entre Projetos

```
agilum.mvc.web
  ├── Pomelo.EntityFrameworkCore.MySql (provider)
  │
  └── agilium-manager-git-azure-infra
        └── Microsoft.EntityFrameworkCore (ORM)
```

---

## Configuração

### Connection String com Fallback

```csharp
// Startup.cs — suporta múltiplas fontes de connection string
private string ObterConnectionString(string name)
{
    // 1. Tenta appsettings.json
    var connStr = Configuration.GetConnectionString(name);
    if (!string.IsNullOrEmpty(connStr)) return connStr;

    // 2. Tenta variável de ambiente direta
    connStr = Environment.GetEnvironmentVariable(name);
    if (!string.IsNullOrEmpty(connStr)) return connStr;

    // 3. Tenta prefixo ConnectionStrings__
    connStr = Environment.GetEnvironmentVariable($"ConnectionStrings__{name}");
    if (!string.IsNullOrEmpty(connStr)) return connStr;

    throw new InvalidOperationException(
        $"Connection string '{name}' não encontrada.");
}
```

### Configurações de Log

```csharp
// Development — Log detalhado
options.EnableSensitiveDataLogging(true);
options.EnableDetailedErrors(true);

// Production — Log mínimo
options.EnableSensitiveDataLogging(false);
options.EnableDetailedErrors(false);
```

### Configuração de Timeout

```csharp
// No connection string
"Server=...;Connection Timeout=30;Default Command Timeout=60;"
```

---

## Convenções

### Nomenclatura de Tabelas e Colunas

| Elemento | Convenção | Exemplo |
|----------|-----------|---------|
| Tabela | Nome minúsculo, sem prefixo | `produto`, `compra`, `venda` |
| Coluna | UPPERCASE com underscore (legado) | `NMPRODUTO`, `IDEMPRESA` |
| Chave Primária | `Id` (long) | `Id` |
| Chave Estrangeira | `ID` + nome da tabela | `IDEMPRESA`, `IDFORN` |
| Identity | `aspnetusers`, `aspnetroles` | Padrão Microsoft |

### Mapeamento

- Usar **Fluent API** (`IEntityTypeConfiguration<T>`) como padrão
- Centralizar mapeamentos em `agilium-manager-git-azure-infra/Mappings/`
- Registrar via `ApplyConfigurationsFromAssembly()`
- Tabelas do Identity mapeadas explicitamente em `dbIdentityContext.OnModelCreating()`

### DbContext

- **Scoped** (uma instância por requisição)
- `SaveChanges()` chamado uma vez ao final da operação
- Nunca compartilhar DbContext entre threads

---

## Impacto de Alterações

### O que Pode Ser Impactado ao Alterar o EF Core

| Alteração | Impacto | Precaução |
|-----------|---------|-----------|
| Mudar versão do EF Core | Breaking changes na API | Testar todas as queries e migrations |
| Alterar entidade (propriedade) | Migration necessária | Gerar migration, testar Up/Down |
| Alterar relacionamento | FK constraints, queries com Include | Revisar todas as queries que usam a entidade |
| Mudar provider (Pomelo → outro) | Comportamento diferente | Testar exaustivamente |
| Alterar connection string | Ambiente errado | Verificar variáveis de ambiente |
| Adicionar/remover índice | Performance de queries | Analisar plano de execução |

### Projetos Afetados

```
Alteração no EF Core impacta:
├── agilium-manager-git-azure-infra (DbContext, Repositories)
├── agilium-manager-azure-business (Models, Interfaces)
├── agilum.mvc.web (Startup, Migrations)
└── agilum.mvc.web.tests (Testes de integração)
```

---

## Comparação: Entity Framework Core x Dapper

| Critério | EF Core | Dapper |
|----------|---------|--------|
| **Produtividade** | Alta — LINQ, migrations, change tracking | Média — SQL manual |
| **Performance** | Boa para CRUD; overhead em queries complexas | Excelente — SQL puro, sem overhead |
| **Curva de Aprendizado** | Média — LINQ, Fluent API | Baixa — SQL |
| **Manutenção** | Fácil — refactoring seguro com LINQ | Difícil — strings SQL sem validação |
| **Migrations** | Nativo — `dotnet ef migrations` | Manual — scripts SQL |
| **Type Safety** | ✅ LINQ é type-safe em compilação | ❌ Strings SQL não são validadas |
| **Joins Complexos** | Verboso com `.Include()`/`.ThenInclude()` | Simples — SQL com joins |
| **Stored Procedures** | Limitado | ✅ Suporte nativo |
| **Uso no Projeto** | CRUD e queries simples/médias | Relatórios, paginação complexa, UUID |

### Regra de Decisão

```
A consulta tem 3+ joins? ──Sim──→ Use Dapper
A consulta é CRUD simples? ──Sim──→ Use EF Core
Precisa de migration? ──Sim──→ Use EF Core
É relatório/agregação? ──Sim──→ Use Dapper
```

---

## Boas Práticas

| Prática | Motivo |
|---------|--------|
| `.AsNoTracking()` em leituras | Evita overhead do change tracker (~30% mais rápido) |
| `.Include()` explícito | Previne lazy loading surpresa e N+1 queries |
| `SaveChanges()` único por operação | Atomicidade — tudo ou nada |
| DbContext **Scoped** | Uma instância por request; evita concorrência |
| Fluent API em classes separadas | Organização; `ApplyConfigurationsFromAssembly()` |
| Projeção com `.Select()` | Trafega apenas colunas necessárias |
| Paginação com `Skip()/Take()` | Limita dados em listas grandes |
| Transações explícitas quando necessário | `BeginTransaction()` para múltiplos `SaveChanges()` |
| Migration testada em dev antes de produção | Evita surpresas com dados reais |
| Connection strings em variáveis de ambiente | Segurança — nunca no código fonte |

---

## Checklist

Antes de alterar código que usa EF Core:

☐ `.AsNoTracking()` em todas as consultas somente leitura

☐ `.Include()` usado em vez de Lazy Loading

☐ `SaveChanges()` chamado uma vez ao final da operação

☐ Projeção com `.Select()` em listagens (não traz entidade completa)

☐ Paginação (`Skip/Take`) em consultas que retornam muitos registros

☐ `EnableSensitiveDataLogging` desabilitado em produção

☐ Connection string via variável de ambiente (não hardcoded)

☐ Migration gerada com nome descritivo e revisada (`Up`/`Down`)

☐ Fluent API em classe separada no padrão `IEntityTypeConfiguration<T>`

☐ DbContext registrado como **Scoped**

☐ Sem `.Result`/`.Wait()` — usar `async/await` de ponta a ponta

☐ Dapper usado apenas para consultas complexas (3+ joins, relatórios)

---

## Pontos de Atenção

- EF Core 3.1 — versão LTS, mas fora de suporte desde dez/2022
- `EnableSensitiveDataLogging(true)` ativo — expõe dados em logs (desabilitar em produção)
- Migrations assembly está no projeto MVC (`agilium.mvc.web`), não no Infra
