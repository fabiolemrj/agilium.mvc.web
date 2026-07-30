# Padrão Services

## Objetivo

Documentar o padrão da **Camada de Serviços** (Service Layer) no projeto Agilium Manager, que concentra toda a lógica de negócio, orquestração de operações, validações e integração entre camadas.

---

## Visão Geral

A camada de serviços é o **coração da lógica de negócio**. Ela fica entre os Controllers (entrada) e os Repositories (persistência), orquestrando operações, aplicando regras de negócio e validando dados.

```
Controller  →  Service  →  Repository  →  Banco
   (entrada)    (negócio)    (dados)
```

---

## Estrutura

```
agilium-manager-azure-business/
├── Interfaces/
│   ├── IService/
│   │   ├── IProdutoService.cs
│   │   ├── ICompraService.cs
│   │   └── ...                         # 40+ interfaces
│   └── INotificador.cs
│
├── Services/
│   ├── BaseService.cs                  # Classe base abstrata
│   ├── ProdutoService.cs
│   ├── CompraService.cs
│   └── ...                             # 40+ serviços
│
└── Validations/
    ├── ProdutoValidation.cs
    ├── CompraValidation.cs
    └── CaUsuarioValidation.cs          # FluentValidation validators
```

---

## Responsabilidades

Cada serviço é responsável por:

- **Validação de negócio** — regras que vão além da simples validação de entrada
- **Orquestração** — coordenar múltiplos repositórios e serviços
- **Transformação de dados** — quando necessária antes/depois da persistência
- **Integração** — chamadas a APIs externas, importação de arquivos
- **Geração de códigos** — `GerarCodigoCompra()`, `GerarUUID()`

> Os serviços **NÃO** devem: acessar `HttpContext`, retornar ViewModels, ou renderizar views.

---

## Interfaces

Cada serviço expõe uma interface na camada Business:

```csharp
// agilium-manager-azure-business/Interfaces/IService/IProdutoService.cs
public interface IProdutoService : IDisposable
{
    Task Adicionar(Produto produto);
    Task Atualizar(Produto produto);
    Task<Produto> ObterPorId(long id);
    Task<IEnumerable<Produto>> ObterTodas(long idEmpresa);
    Task Salvar();
}
```

> Interfaces herdam de `IDisposable` — o serviço gerencia o ciclo de vida dos repositórios.

---

## Implementação

### Classe Base (`BaseService`)

Local: `agilium-manager-azure-business/Services/BaseService.cs`

```csharp
public abstract class BaseService
{
    private readonly INotificador _notificador;

    protected BaseService(INotificador notificador)
    {
        _notificador = notificador;
    }

    // ===== NOTIFICAÇÕES =====

    protected void Notificar(string mensagem)
    {
        _notificador.Handle(new Notificacao(mensagem));
    }

    protected void Notificar(ValidationResult validationResult)
    {
        foreach (var error in validationResult.Errors)
            Notificar(error.ErrorMessage);
    }

    // ===== FLUENT VALIDATION =====

    protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade)
        where TV : AbstractValidator<TE> where TE : Entity
    {
        var validator = validacao.Validate(entidade);
        if (validator.IsValid) return true;
        Notificar(validator);
        return false;
    }

    // ===== CONSULTA =====

    protected bool TemNotificacao()
    {
        return _notificador.TemNotificacao();
    }

    protected List<Notificacao> ObterNotificacao()
    {
        return _notificador.ObterNotificacoes();
    }
}
```

### Serviço Específico

```csharp
public class ProdutoService : BaseService, IProdutoService
{
    private readonly IProdutoRepository _produtoRepository;
    private readonly IProdutoDepartamentoRepository _produtoDepartamentoRepository;
    private readonly IDapperRepository _dapperRepository;
    private readonly IProdutoDapper _produtoDapperRepository;

    public ProdutoService(
        INotificador notificador,
        IProdutoRepository produtoRepository,
        IProdutoDepartamentoRepository produtoDepartamentoRepository,
        IDapperRepository dapperRepository,
        IProdutoDapper produtoDapper) : base(notificador)
    {
        _produtoRepository = produtoRepository;
        _produtoDepartamentoRepository = produtoDepartamentoRepository;
        _dapperRepository = dapperRepository;
        _produtoDapperRepository = produtoDapper;
    }

    public async Task Adicionar(Produto produto)
    {
        // 1. Validação via FluentValidation
        if (!ExecutarValidacao(new ProdutoValidation(), produto))
            return;

        // 2. Persistência (sem salvar — caller controla)
        await _produtoRepository.AdicionarSemSalvar(produto);
    }

    public async Task Atualizar(Produto produto)
    {
        if (!ExecutarValidacao(new ProdutoValidation(), produto))
            return;

        await _produtoRepository.AtualizarSemSalvar(produto);
    }

    public async Task Salvar()
    {
        await _produtoRepository.SaveChanges();
    }

    public void Dispose()
    {
        _produtoRepository?.Dispose();
        _produtoDepartamentoRepository?.Dispose();
    }
}
```

---

## Regras de Negócio

As regras de negócio são implementadas nos serviços. Exemplos reais do projeto:

### Validações de Existência

```csharp
public async Task Adicionar(Produto produto)
{
    var existente = await _produtoRepository
        .Buscar(p => p.CDPRODUTO == produto.CDPRODUTO
                  && p.IDEMPRESA == produto.IDEMPRESA);

    if (existente.Any())
    {
        Notificar("Já existe um produto com este código.");
        return;
    }

    await _produtoRepository.AdicionarSemSalvar(produto);
}
```

### Regras de Integridade

```csharp
public async Task CancelarCompra(long idCompra, string nomeUsuario)
{
    var compra = await _compraRepository.ObterPorId(idCompra);

    if (compra.Situacao == ESituacaoCompra.Cancelada)
    {
        Notificar("Compra já está cancelada.");
        return;
    }

    if (compra.Situacao == ESituacaoCompra.Efetivada)
    {
        // Validar se há itens já vendidos
        // Reverter estoque se necessário
    }

    compra.Situacao = ESituacaoCompra.Cancelada;
    await _compraRepository.AtualizarSemSalvar(compra);
}
```

### Orquestração Multi-Step

```csharp
public async Task EfetivarCompra(long idCompra, string nomeUsuario)
{
    // 1. Validar compra
    var compra = await _compraRepository.ObterPorId(idCompra);
    if (compra == null) { Notificar("Compra não encontrada."); return; }

    // 2. Atualizar estoque (via EstoqueService)
    foreach (var item in compra.Itens)
    {
        await _estoqueService.EntradaEstoque(item.IDPRODUTO, item.Quantidade);
    }

    // 3. Atualizar situação
    compra.Situacao = ESituacaoCompra.Efetivada;
    await _compraRepository.AtualizarSemSalvar(compra);

    // 4. Registrar financeiro (via ContaService)
    await _contaService.GerarContaPagar(compra);
}
```

---

## Integração com FluentValidation

### Como Funciona

O `BaseService.ExecutarValidacao<TV, TE>()` é a ponte entre FluentValidation e o Notification Pattern:

```csharp
protected bool ExecutarValidacao<TV, TE>(TV validacao, TE entidade)
    where TV : AbstractValidator<TE> where TE : Entity
{
    var validator = validacao.Validate(entidade);
    if (validator.IsValid) return true;

    // Converte falhas do FluentValidation em notificações
    Notificar(validator);
    return false;
}
```

### Classe Validator

Local: `agilium-manager-azure-business/Models/Validations/`

```csharp
public class ProdutoValidation : AbstractValidator<Produto>
{
    public ProdutoValidation()
    {
        RuleFor(p => p.NMPRODUTO)
            .NotEmpty().WithMessage("O nome do produto é obrigatório.")
            .MaximumLength(100).WithMessage("Nome deve ter no máximo 100 caracteres.");

        RuleFor(p => p.CDPRODUTO)
            .NotEmpty().WithMessage("O código do produto é obrigatório.");

        RuleFor(p => p.IDEMPRESA)
            .GreaterThan(0).WithMessage("Empresa é obrigatória.");
    }
}
```

### Uso no Serviço

```csharp
public async Task Adicionar(Produto produto)
{
    // Executa validação FluentValidation
    if (!ExecutarValidacao(new ProdutoValidation(), produto))
        return;  // Notificações já foram registradas

    await _produtoRepository.AdicionarSemSalvar(produto);
}
```

---

## Integração com Notificador

O `INotificador` é injetado via `BaseService` e está disponível para todos os serviços:

```csharp
// Notificar um erro simples
Notificar("Produto não encontrado.");

// Notificar resultado do FluentValidation
Notificar(validationResult);  // Itera sobre todos os erros

// Verificar se há erros antes de prosseguir
if (TemNotificacao()) return;

// Controller verifica após chamar o serviço
if (!OperacaoValida())
{
    var erros = ObterNotificacoes();
    // ...
}
```

---

## Integração com AutoMapper

Os serviços **NÃO** usam AutoMapper diretamente — o mapeamento ocorre no Controller:

```
Controller: ViewModel → (AutoMapper) → Model → Service
Controller: Service → Model → (AutoMapper) → ViewModel
```

```csharp
// ✅ Controller — mapeia antes/depois de chamar o serviço
var produto = _mapper.Map<Produto>(viewModel);
await _produtoService.Adicionar(produto);

// ❌ Service NÃO mapeia — trabalha apenas com Models
```

---

## Validações

O sistema aplica validações em dois momentos:

### 1. FluentValidation (Serviço)

```csharp
// Antes de Adicionar/Atualizar
if (!ExecutarValidacao(new ProdutoValidation(), produto))
    return;
```

### 2. Validações Manuais (Serviço)

```csharp
// Regras que vão além do FluentValidation
if (await _produtoRepository.Existe(p => p.CDPRODUTO == codigo))
{
    Notificar("Código já existe.");
    return;
}
```

### Fluxo Completo

```
Controller
  │
  ├── ModelState.IsValid (Data Annotations)
  │
  ▼
Service.Adicionar()
  │
  ├── ExecutarValidacao() → FluentValidation
  │     └── Falhou → Notificar() → return
  │
  ├── Regras de negócio manuais
  │     └── Falhou → Notificar() → return
  │
  └── Repository.AdicionarSemSalvar()
```

---

## Fluxo de Execução

### Operação de Criação (ex: Criar Produto)

```
[Controller]
  │ POST: model (ViewModel)
  │ ModelState.IsValid?
  │ Mapeia ViewModel → Model (AutoMapper)
  ▼
[Service] Adicionar(produto)
  │
  ├── 1. Validação FluentValidation
  │      ExecutarValidacao(new ProdutoValidation(), produto)
  │      └── Inválido → Notificar() → return
  │
  ├── 2. Regras de negócio
  │      Existe código duplicado? → Notificar()
  │      Empresa ativa? → Notificar()
  │
  └── 3. Persistência
         Repository.AdicionarSemSalvar(produto)
         │
         ▼
[Controller]
  OperacaoValida()?
  ├── true  → Service.Salvar() → Redirect
  └── false → Adiciona erros ao ModelState → return View()
```

### Operação de Orquestração (ex: Efetivar Compra)

```
[Service] EfetivarCompra(id)
  │
  ├── 1. Obter compra (Repository)
  ├── 2. Validar situação (não cancelada, não já efetivada)
  ├── 3. Para cada item:
  │      EstoqueService.EntradaEstoque(produto, qtd)
  ├── 4. Atualizar situação (Repository)
  ├── 5. ContaService.GerarContaPagar(compra) → Financeiro
  │
  └── Controller chama Service.Salvar()
```

---

## Transações

### Controle pelo Caller

O padrão do projeto delega o `SaveChanges()` ao caller (Controller ou método orquestrador):

```csharp
// Serviço: Adiciona sem salvar
await _produtoRepository.AdicionarSemSalvar(produto);
await _produtoRepository.AdicionarSemSalvar(codigoBarra);

// Controller: Salva tudo junto (atômico)
await _produtoService.Salvar();
```

### Vantagem

- Múltiplas operações em um único `SaveChanges()` → **atomicidade**
- O serviço não decide quando persistir — o orquestrador decide

---

## Tratamento de Exceções

### Exceções de Negócio → Notification Pattern

```csharp
// ❌ NÃO usar exceções para regras de negócio
if (produto == null)
    throw new Exception("Produto não encontrado");

// ✅ Usar Notification Pattern
if (produto == null)
{
    Notificar("Produto não encontrado.");
    return;
}
```

### Exceções de Infraestrutura → Stack

```csharp
// Exceções de banco/rede sobem para o ExceptionMiddleware
// O Repository já captura DbUpdateConcurrencyException
// e lança com mensagem amigável
```

---

## Métodos Assíncronos

### Padrão do Projeto

```csharp
// ✅ async/await de ponta a ponta
public async Task Adicionar(Produto produto)
{
    await _produtoRepository.AdicionarSemSalvar(produto);
}

// ⚠️ Padrão encontrado no projeto (evitar em novos códigos)
public async Task<Compra> ObterPorId(long id)
{
    return _compraRepository.ObterPorId(id).Result;  // Bloqueante!
}
```

> **Atenção:** Alguns serviços usam `.Result` (bloqueante). Para novos desenvolvimentos, usar `await`.

---

## Segurança

| Prática | Como é Aplicado |
|---------|-----------------|
| Validação de entrada | FluentValidation + Notification Pattern |
| Autorização | `ClaimsAuthorizeAttribute` no Controller (antes do Service) |
| SQL Injection | Prevenido pelo EF Core (LINQ parametrizado) |
| Dados sensíveis | Connection strings em variáveis de ambiente |
| Exceções | Mensagens internas nunca expostas ao cliente |

> A segurança de **autenticação/autorização** é aplicada no Controller, não no Service. O Service presume que o caller já validou permissões.

---

## Performance

| Técnica | Descrição |
|---------|-----------|
| **Dapper para consultas pesadas** | Serviços injetam `ICompraDapperRepository`, `IProdutoDapper` |
| **Projeção** | Dapper retorna DTOs enxutos, não entidades completas |
| **`AdicionarSemSalvar`** | Evita múltiplos `SaveChanges` em loops |
| **Pagination no Dapper** | `LIMIT/OFFSET` nativo, não `Skip/Take` em memória |

### Exemplo: Paginação com Dapper (via Service)

```csharp
public async Task<PagedResult<Compra>> ObterCompraPorPaginacaoDapper(
    long idEmpresa, DateTime dtIni, DateTime dtFim, int page, int pageSize)
{
    // Serviço delega ao DapperRepository (SQL otimizado)
    return await _compraDapperRepository.ObterCompraPaginada(
        idEmpresa, dtIni, dtFim, page, pageSize);
}
```

---

## Dependency Injection

### Registro

```csharp
// ResolveDependencyConfig.cs
services.AddScoped<INotificador, Notificador>();
services.AddScoped<IProdutoService, ProdutoService>();
services.AddScoped<ICompraService, CompraService>();
services.AddScoped<IVendaService, VendaService>();
services.AddScoped<IEstoqueService, EstoqueService>();
// ... 40+ serviços
```

> Todos os serviços são **Scoped** — uma instância por requisição HTTP.

### Injeção no Controller

```csharp
public class ProdutoController : MainController
{
    private readonly IProdutoService _produtoService;

    public ProdutoController(..., IProdutoService produtoService)
        : base(...)
    {
        _produtoService = produtoService;
    }
}
```

---

## Dependências

### Serviços Dependem de

```
Service
  ├── INotificador (BaseService)
  ├── I*Repository (EF Core)
  ├── I*DapperRepository (Dapper)
  ├── IDapperRepository (genérico Dapper)
  ├── IUtilDapperRepository (UUID, códigos)
  └── I*Service (outros serviços — orquestração)
```

### Exemplo Real: `CompraService`

```
CompraService
  ├── ICompraRepository (EF Core)
  ├── ICompraItemRepository (EF Core)
  ├── ICompraFiscalRepository (EF Core)
  ├── ICompraDapperRepository (Dapper)
  ├── IFornecedorDapperRepository (Dapper)
  ├── IProdutoDapper (Dapper)
  ├── IEstoqueDapperRepository (Dapper)
  ├── IPlanoContaDapperRepository (Dapper)
  ├── IUtilDapperRepository (Dapper - UUID)
  └── IDapperRepository (Dapper genérico)
```

> Serviços complexos injetam **ambos** EF Core e Dapper. Consultas de listagem usam Dapper; CRUD usa EF Core.

---

## Convenções

| Convenção | Exemplo |
|-----------|---------|
| Sufixo `Service` | `ProdutoService`, `CompraService` |
| Interface com prefixo `I` | `IProdutoService` |
| Interface em `Interfaces/IService/` | `IProdutoService.cs` |
| Implementação em `Services/` | `ProdutoService.cs` |
| Herda de `BaseService` | `class ProdutoService : BaseService` |
| Um serviço por agregado | `ProdutoService` cobre produto + departamento + marca |
| Validação no método `Adicionar`/`Atualizar` | `ExecutarValidacao(new XValidation(), entity)` |
| `AdicionarSemSalvar`/`AtualizarSemSalvar` | Serviço não controla transação |

---

## Impactos de Alterações

| Alteração | Impacto | Precaução |
|-----------|---------|-----------|
| Mudar assinatura de método do Service | Quebra Controllers e Services dependentes | Atualizar todas as referências |
| Adicionar validação FluentValidation | Pode rejeitar dados antes aceitos | Verificar dados existentes no banco |
| Alterar regra de negócio | Efeito cascata (estoque, financeiro, fiscal) | Testar fluxo ponta a ponta |
| Mudar de `AdicionarSemSalvar` para `Adicionar` | Muda controle transacional | Revisar todos os callers |
| Adicionar dependência de outro Service | Risco de dependência circular | Usar injeção de interface |

### Projetos Afetados

```
Alteração no Service impacta:
├── agilium-manager-azure-business (Interfaces, outros Services)
├── agilum.mvc.web (Controllers)
├── agilium-manager-azure-api (API Controllers)
└── agilium-pdv-azure-api (PDV Controllers)
```

---

## Boas Práticas

| Prática | Motivo |
|---------|--------|
| Herdar de `BaseService` | Acesso a `INotificador` e `ExecutarValidacao` |
| Interface para cada serviço | Desacoplamento e testabilidade |
| `ExecutarValidacao()` antes de persistir | FluentValidation integrado com Notification Pattern |
| `AdicionarSemSalvar` / `AtualizarSemSalvar` | Controle de transação pelo orquestrador |
| Validar antes de persistir | Não persiste dados inválidos |
| `Dispose()` nos repositórios | Libera recursos do DbContext |
| Dapper para consultas complexas | Performance em listagens e relatórios |
| Não mapear ViewModel no Service | Separação de responsabilidades |
| Scoped no DI | Uma instância por requisição |
| Um serviço por agregado | Coesão |

---

## Anti-Padrões

| Evitar | Alternativa |
|--------|-------------|
| Lógica de negócio no Controller | Mover para o Service |
| Service acessando `HttpContext` | Injetar `IUser` se precisar do usuário |
| Service retornando ViewModel | Retornar Model; Controller mapeia |
| `.Result` / `.Wait()` | Usar `await` |
| Service chamando outro Service em loop | Refatorar para único método orquestrador |
| Service com `SaveChanges` em cada método | Usar `...SemSalvar` + `Salvar()` no caller |

---

## Checklist

Antes de criar/alterar um Service:

☐ Interface definida em `Interfaces/IService/I{Nome}Service.cs`

☐ Classe herda de `BaseService`

☐ Construtor recebe `INotificador` e passa para `base(notificador)`

☐ Validação via `ExecutarValidacao(new XValidation(), entity)` antes de persistir

☐ Métodos de escrita usam `...SemSalvar` (caller controla transação)

☐ Método `Salvar()` delega para um repositório principal

☐ Método `Dispose()` libera todos os repositórios injetados

☐ Consultas complexas delegam para repositórios Dapper

☐ Sem regras de negócio no Controller

☐ Registrado como **Scoped** no `ResolveDependencyConfig.cs`

☐ Sem dependência circular entre serviços
| Métodos com muitos parâmetros | Usar objetos de request |
