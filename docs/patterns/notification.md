# Padrão Notification (Notificação)

## Objetivo

Documentar o **Notification Pattern** utilizado no projeto Agilium Manager para tratamento de erros de negócio, substituindo exceções por notificações acumulativas.

---

## Visão Geral

O Notification Pattern evita que exceções sejam usadas para controle de fluxo de negócio. Em vez de lançar exceções para cada validação, o sistema acumula notificações e as retorna de forma estruturada.

```
Validação falha → Notificar("mensagem") → Controller verifica OperacaoValida()
                                                      ├── false → Retorna erros para view
                                                      └── true  → Prossegue
```

---

## Localização no Projeto

```
agilium-manager-azure-business/
└── Notificacoes/
    ├── Notificacao.cs      # Model da notificação
    └── Notificador.cs      # Implementação do INotificador
```

Interfaces em:
```
agilium-manager-azure-business/Interfaces/
├── INotificador.cs
```

---

## Implementação

### Model

```csharp
public class Notificacao
{
    public string Mensagem { get; }

    public Notificacao(string mensagem)
    {
        Mensagem = mensagem;
    }
}
```

### Interface

```csharp
public interface INotificador
{
    bool TemNotificacao();
    List<Notificacao> ObterNotificacoes();
    void Handle(Notificacao notificacao);
}
```

### Implementação

```csharp
public class Notificador : INotificador
{
    private readonly List<Notificacao> _notificacoes;

    public Notificador() => _notificacoes = new List<Notificacao>();

    public void Handle(Notificacao notificacao) => _notificacoes.Add(notificacao);

    public List<Notificacao> ObterNotificacoes() => _notificacoes;

    public bool TemNotificacao() => _notificacoes.Any();
}
```

---

## Uso no Projeto

### Na Camada de Negócio (BaseService)

```csharp
public abstract class BaseService
{
    protected readonly INotificador _notificador;

    protected BaseService(INotificador notificador)
    {
        _notificador = notificador;
    }

    protected void Notificar(string mensagem)
    {
        _notificador.Handle(new Notificacao(mensagem));
    }
}
```

### No Serviço Específico

```csharp
public class ProdutoService : BaseService, IProdutoService
{
    public async Task Adicionar(Produto produto)
    {
        // Validação de negócio
        if (string.IsNullOrEmpty(produto.NMPRODUTO))
        {
            Notificar("O nome do produto é obrigatório.");
            return;
        }

        var existente = await _repo.BuscarPorCodigo(produto.CDPRODUTO);
        if (existente != null)
        {
            Notificar("Já existe um produto com este código.");
            return;
        }

        await _repo.Adicionar(produto);
    }
}
```

### No Controller (MainController)

```csharp
public abstract class MainController : Controller
{
    protected bool OperacaoValida()
    {
        return !_notificador.TemNotificacao();
    }

    protected string[] ObterNotificacoes()
    {
        return _notificador.ObterNotificacoes().Select(n => n.Mensagem).ToArray();
    }

    protected void NotificarErro(string mensagem)
    {
        _notificador.Handle(new Notificacao(mensagem));
    }
}
```

### Exemplo no Fluxo de Cadastro

```csharp
[HttpPost]
public async Task<IActionResult> Create(ProdutoViewModel model)
{
    if (!ModelState.IsValid) return View(model);

    var produto = _mapper.Map<Produto>(model);
    await _produtoService.Adicionar(produto);

    if (!OperacaoValida())
    {
        var erros = ObterNotificacoes();
        foreach (var erro in erros)
            ModelState.AddModelError(string.Empty, erro);
        return View(model);
    }

    await _produtoService.Salvar();
    TempData["Mensagem"] = "Produto criado com sucesso!";
    return RedirectToAction("Index");
}
```

---

## Ciclo de Vida

```
Requisição HTTP
      │
      ▼
Controller (MainController)
  → INotificador injetado (Scoped)
      │
      ▼
Service.Adicionar(objeto)
  → Notificar("erro 1")
  → Notificar("erro 2")
      │
      ▼
Controller.OperacaoValida()
  → false → retorna erros acumulados para a View
```

> O `INotificador` é registrado como **Scoped** — uma instância por requisição, zerando automaticamente.

---

## Boas Práticas

- **Acumular erros** — validar tudo antes de retornar (não parar no primeiro erro)
- **Mensagens em português** — o sistema é voltado para usuários brasileiros
- **Não usar para erros de infraestrutura** — exceções de banco/rede ainda devem ser tratadas com try/catch
- **Separar validação de entrada** (ModelState) da validação de negócio (Notification)

---

## Vantagens sobre Exceções

| Exceções | Notification Pattern |
|----------|---------------------|
| Interrompe o fluxo | Acumula múltiplos erros |
| Custo de performance | Leve (lista em memória) |
| Stack trace desnecessário | Apenas a mensagem |
| Difícil retornar múltiplos erros | Retorna todos de uma vez |
