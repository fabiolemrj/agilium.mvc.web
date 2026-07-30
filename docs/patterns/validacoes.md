# Validações

## Objetivo

Este documento descreve a arquitetura, responsabilidades e boas práticas relacionadas ao mecanismo de **Validações** utilizado no projeto **Agilium Manager**.

As validações têm como objetivo garantir que todas as operações da aplicação sejam executadas com dados consistentes, respeitando tanto as regras estruturais quanto as regras de negócio antes da persistência ou processamento das informações.

A arquitetura separa claramente:

- Validações de entrada (ViewModels/DTOs)
- Validações de domínio (FluentValidation)
- Validações de negócio (Services + Notification Pattern)
- Validações de persistência (EF Core + banco de dados)

Essa separação melhora a manutenção, reutilização e previsibilidade da aplicação.

---

# Visão Geral

Fluxo simplificado:

```
Browser (jQuery Validation)

↓

ViewModel (Data Annotations)

↓

Controller (ModelState.IsValid)

↓

Service (FluentValidation + Regras de Negócio)

↓

Notificador (Notification Pattern)

↓

Repository (EF Core)

↓

Banco de Dados (Constraints)
```

Cada camada é responsável apenas pelas validações que lhe competem.

---

# Objetivos

A arquitetura de validações possui os seguintes objetivos:

- Garantir integridade dos dados
- Evitar persistência de dados inválidos
- Centralizar regras de validação
- Reutilizar validações
- Reduzir duplicação de código
- Melhorar legibilidade
- Facilitar manutenção
- Padronizar mensagens de erro

---

# Arquitetura

```
[Client]       jQuery Validation + Data Annotations (unobtrusive)
     │
     ▼
[Controller]   ModelState.IsValid + Data Annotations + AntiForgery
     │
     ▼
[Service]      FluentValidation (via ExecutarValidacao) + Regras Manuais
     │
     ▼
[Notificador]  INotificador → Notificacao (acumula erros)
     │
     ▼
[Repository]   EF Core SaveChanges → DbUpdateConcurrencyException
```

Cada tipo de validação ocorre em sua respectiva camada.

---

# Tipos de Validação

A aplicação trabalha com quatro grupos de validação:

## Validação Estrutural (Client + Controller)

Responsável por validar:

- Campos obrigatórios
- Tamanho máximo / mínimo
- Formatos (e-mail, CPF/CNPJ, telefone)
- Datas válidas
- Valores numéricos (range, positivos)
- Tipos de dados

Essa validação ocorre **antes** da execução das regras de negócio.

### Implementação

```csharp
// ViewModel com Data Annotations
public class ProdutoViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, MinimumLength = 3,
        ErrorMessage = "O nome deve ter entre {2} e {1} caracteres")]
    public string Nome { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Preco { get; set; }
}

// Controller
[HttpPost]
public async Task<IActionResult> Create(ProdutoViewModel model)
{
    if (!ModelState.IsValid)      // ← Validação estrutural
        return View(model);

    // Prossegue para validação de negócio...
}
```

---

## Validação de Domínio (FluentValidation)

Responsável por validar:

- Estado da entidade
- Consistência interna
- Regras do modelo
- Invariantes

### Implementação

```csharp
// agilium-manager-azure-business/Models/Validations/ProdutoValidation.cs
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

        RuleFor(p => p.VLPRECO)
            .GreaterThan(0).WithMessage("Preço de venda deve ser maior que zero.");
    }
}
```

---

## Validação de Negócio (Service)

Executada pelos Services. Exemplos reais do projeto:

- Caixa aberto antes da venda
- Estoque com saldo disponível
- Compra em situação válida para cancelamento
- Produto com código único por empresa
- Cliente ativo para receber pedido
- Empresa ativa para operações
- Turno aberto antes de abrir caixa

### Implementação

```csharp
public async Task Adicionar(Produto produto)
{
    // 1. FluentValidation
    if (!ExecutarValidacao(new ProdutoValidation(), produto))
        return;

    // 2. Regras de negócio manuais
    var existente = await _produtoRepository
        .Buscar(p => p.CDPRODUTO == produto.CDPRODUTO
                  && p.IDEMPRESA == produto.IDEMPRESA);

    if (existente.Any())
    {
        Notificar("Já existe um produto com este código.");
        return;
    }

    // 3. Persistência
    await _produtoRepository.AdicionarSemSalvar(produto);
}
```

---

## Validação de Persistência (Repository + Banco)

Executada durante a gravação dos dados:

- Chave duplicada (UNIQUE constraint)
- Chave estrangeira (FOREIGN KEY)
- Concorrência (`DbUpdateConcurrencyException`)
- Integridade referencial

São garantidas pelo banco de dados e pelo `Repository.SaveChanges()`:

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
            "Erro de concorrência ao salvar. Dados podem ter sido alterados.", ex);
    }
}
```

---

# Fluxo de Validação

```
[Browser] jQuery Validation + Unobtrusive
      │
      ✗ Falhou? → Mensagem no campo, formulário não enviado
      │
      ▼
[Controller] ModelState.IsValid + AntiForgery
      │
      ✗ Falhou? → return View(model) com erros
      │
      ▼
[Service] ExecutarValidacao(new XValidation(), entity)
      │
      ✗ Falhou? → Notificar(validationResult) → return
      │
      ▼
[Service] Regras de negócio manuais
      │
      ✗ Falhou? → Notificar("mensagem") → return
      │
      ▼
[Repository] AdicionarSemSalvar / AtualizarSemSalvar
      │
      ▼
[Controller] OperacaoValida()?
      │
      ├── false → Adiciona erros ao ModelState → return View(model)
      │
      └── true → Service.Salvar() → Redirect
```

---

# Integração com FluentValidation

### BaseService — Ponte entre FluentValidation e Notification

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

protected void Notificar(ValidationResult validationResult)
{
    foreach (var error in validationResult.Errors)
        Notificar(error.ErrorMessage);
}
```

### Uso Padrão

```csharp
public async Task Adicionar(Compra compra)
{
    if (!ExecutarValidacao(new CompraValidation(), compra))
        return;

    await _compraRepository.AdicionarSemSalvar(compra);
}
```

---

# Organização

Estrutura real do projeto:

```
agilium-manager-azure-business/
├── Models/
│   └── Validations/
│       ├── ProdutoValidation.cs
│       ├── CompraValidation.cs
│       └── CaUsuarioValidation.cs
│
├── Services/
│   └── BaseService.cs          # ExecutarValidacao<TV, TE>()
│
└── Notificacoes/
    ├── Notificacao.cs           # Model da notificação
    └── Notificador.cs           # INotificador
```

Cada entidade que requer validação possui seu próprio `AbstractValidator<T>`.

---

# Integração com Notificador

Quando uma validação falha:

```
Service.Adicionar()
  │
  ├── FluentValidation falhou
  │     └── ExecutarValidacao() → Notificar(validationResult)
  │
  ├── Regra de negócio falhou
  │     └── Notificar("Mensagem")
  │
  ▼
Controller.OperacaoValida()
  │
  └── false → ObterNotificacoes() → ModelState.AddModelError()
```

As mensagens são centralizadas no `INotificador` (Scoped por requisição).

---

# Mensagens

As mensagens de validação devem ser:

- **Claras** — o usuário entende o que está errado
- **Objetivas** — diz exatamente qual campo e qual problema
- **Padronizadas** — mesmo estilo em todo o sistema
- **Orientadas ao usuário** — em português, sem jargão técnico

Exemplos reais do projeto:

- `"O nome do produto é obrigatório."`
- `"Já existe um produto com este código."`
- `"Erro ao criar nova compra."`
- `"Selecione uma empresa para acessar Compras."`
- `"Compra não localizada."`

Evitar mensagens técnicas ou detalhes internos da aplicação.

---

# Reutilização

Validações comuns devem ser reutilizadas entre módulos:

- CPF / CNPJ
- CEP
- Datas (formato, intervalo)
- Telefones
- E-mails
- Documentos fiscais (chave NFe)

Evitar duplicação de regras entre diferentes módulos. Regras compartilhadas podem ser extraídas para validators base ou extension methods.

---

# Dependências

As validações no projeto dependem de:

| Camada | Dependência |
|--------|-------------|
| Client | jQuery Validation, Unobtrusive, Data Annotations |
| Controller | `ModelState`, `AutoValidateAntiforgeryToken` |
| Service | `FluentValidation`, `INotificador`, `IRepository` (para `Existe()`) |
| Repository | EF Core `SaveChanges`, constraints do banco |

Sempre que possível, manter as validações independentes de infraestrutura externa.

---

# Tratamento de Erros

Situações comuns e como são tratadas:

| Situação | Mecanismo | Camada |
|----------|-----------|--------|
| Campo obrigatório | Data Annotations / FluentValidation | Controller / Service |
| Formato inválido | Data Annotations / FluentValidation | Controller / Service |
| Registro inexistente | `Notificar("não localizado")` | Service |
| Regra de negócio violada | `Notificar("mensagem")` | Service |
| Permissão insuficiente | `ClaimsAuthorizeAttribute` → 403 | Controller |
| Falha de banco | `DbUpdateConcurrencyException` → middleware | Repository |
| Timeout | Exceção → `ExceptionMiddleware` | Repository |

---

# Segurança

Boas práticas aplicadas no projeto:

- **Nunca confiar apenas no client-side** — jQuery Validation é UX, não segurança
- **AntiForgery global** — `[AutoValidateAntiforgeryToken]` em todo POST
- **Validar todos os dados recebidos** — ModelState sempre verificado
- **Não expor detalhes internos** — mensagens genéricas para o usuário
- **Validar permissões** — `ClaimsAuthorizeAttribute` antes da ação
- **Connection strings** — variáveis de ambiente, nunca hardcoded

---

# Performance

Boas práticas de performance nas validações:

- **Validar primeiro o mais simples** — Data Annotations antes de FluentValidation
- **Evitar consultas desnecessárias** — usar `Existe()` (bool) em vez de `ObterPorId()` (entidade completa)
- **Interromper cedo** — `return` imediato após `Notificar()`
- **Evitar validações duplicadas** — não repetir FluentValidation no Controller
- **Dapper para verificações em lote** — validações que exigem ir ao banco

```csharp
// ✅ Performance: Existe() retorna apenas true/false
if (await _produtoRepository.Existe(p => p.CDPRODUTO == codigo))
{
    Notificar("Código já cadastrado.");
    return;
}

// ❌ Evitar: trazer entidade completa só para verificar existência
var existente = await _produtoRepository.ObterPorId(id);
if (existente != null) { ... }
```

---

# Dependency Injection

Os Validators são instanciados diretamente nos serviços — **não são injetados**:

```csharp
// ✅ Padrão do projeto: instância new no ponto de uso
if (!ExecutarValidacao(new ProdutoValidation(), produto))
    return;
```

O `INotificador` é injetado via `BaseService` (Scoped):

```csharp
// ResolveDependencyConfig.cs
services.AddScoped<INotificador, Notificador>();
```

---

# Convenções

A implementação segue as convenções:

| Convenção | Exemplo |
|-----------|---------|
| Um Validator por entidade | `ProdutoValidation`, `CompraValidation` |
| Herda de `AbstractValidator<T>` | `class ProdutoValidation : AbstractValidator<Produto>` |
| Localizado em `Models/Validations/` | `ProdutoValidation.cs` |
| Chamado via `ExecutarValidacao()` | `ExecutarValidacao(new XValidation(), entity)` |
| Mensagens em português | `"O nome do produto é obrigatório."` |
| Integração com Notificador | `Notificar(validationResult)` |
| Validação antes de `Adicionar`/`Atualizar` | `if (!ExecutarValidacao(...)) return;` |

---

# Impactos de Alterações

Alterações nas validações podem impactar:

- **Controllers** — mudanças no ModelState/ViewModel
- **Services** — novas regras podem rejeitar dados antes aceitos
- **APIs** — consumidores externos precisam ser notificados
- **Integrações** — importação de NFe, marketplace
- **Front-end** — mensagens de erro exibidas ao usuário
- **Banco de dados** — dados existentes podem violar novas regras

Toda alteração deve ser acompanhada por testes e verificação de dados existentes.

---

# Comparação: Validação Estrutural x Validação de Negócio

| Validação Estrutural | Validação de Negócio |
|----------------------|----------------------|
| Campos obrigatórios | Código de produto único por empresa |
| Formato de e-mail | Caixa aberto para venda |
| CPF/CNPJ válido | Estoque com saldo disponível |
| Tamanho máximo do texto | Cliente ativo para pedido |
| Range de valores (preço > 0) | Empresa ativa para operações |
| Datas no formato correto | Turno aberto antes de caixa |
| **Onde**: Data Annotations / FluentValidation | **Onde**: Service (manual) |
| **Quando**: Antes do Service | **Quando**: Dentro do Service |

---

# Comparação: Validação x Exceção

| Situação | Validação (Notification) | Exceção |
|----------|--------------------------|----------|
| Campo obrigatório não preenchido | ✅ | ❌ |
| Produto inativo para venda | ✅ | ❌ |
| Cliente não encontrado | ✅ | ❌ |
| Estoque insuficiente | ✅ | ❌ |
| Código duplicado | ✅ | ❌ |
| Falha de conexão com banco | ❌ | ✅ |
| Timeout de operação | ❌ | ✅ |
| Erro de infraestrutura | ❌ | ✅ |
| Violação de FK/UNIQUE | ❌ | ✅ (`DbUpdateException`) |

---

# Boas Práticas

- Centralizar validações estruturais no FluentValidation
- Implementar regras de negócio exclusivamente nos Services
- Utilizar o `INotificador` para comunicar erros previstos
- Não duplicar validações entre camadas
- Manter mensagens padronizadas e em português
- Validar todos os dados recebidos (client + server)
- Criar Validators específicos para cada entidade
- Usar `Existe()` em vez de `ObterPorId()` para verificações de existência
- Interromper o fluxo ao encontrar erros (`return` após `Notificar()`)
- Não usar exceções para controle de fluxo de negócio

---

# Checklist

Antes de alterar uma validação:

☐ Validator atualizado (FluentValidation)

☐ Regras de negócio revisadas no Service

☐ Mensagens padronizadas e em português

☐ Integração com `INotificador` validada

☐ Controller verifica `OperacaoValida()` após chamar Service

☐ `ExecutarValidacao()` chamado antes de `Adicionar`/`Atualizar`

☐ Dados existentes no banco compatíveis com a nova regra

☐ Testes executados nos fluxos afetados

☐ Performance avaliada (sem consultas desnecessárias)

☐ Impactos em APIs e integrações analisados

---

# Conclusão

A arquitetura de **Validações** do **Agilium Manager** é responsável por garantir a integridade dos dados e o cumprimento das regras do domínio em todas as etapas do processamento.

Ao separar **validações estruturais** (jQuery + Data Annotations), **validações de domínio** (FluentValidation), **validações de negócio** (Services + Notification Pattern) e **validações de persistência** (EF Core + Constraints), a aplicação mantém uma arquitetura organizada, reutilizável e de fácil manutenção, reduzindo inconsistências e aumentando a confiabilidade das operações.

