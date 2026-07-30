# Agilium Manager — Coding Standards

> Este documento define os padrões obrigatórios de desenvolvimento do projeto **Agilium Manager**.
> Todas as implementações devem seguir estas diretrizes para manter consistência arquitetural, qualidade e facilidade de manutenção.

---

# 1. Princípios Gerais

## 1.1 Clean Code

Todo código deve priorizar:

- Clareza
- Simplicidade
- Legibilidade
- Baixo acoplamento
- Alta coesão

Evite:

- Métodos grandes
- Classes com muitas responsabilidades
- Código duplicado
- Comentários desnecessários
- Código morto

Sempre prefira código autoexplicativo.

---

## 1.2 SOLID

Todo desenvolvimento deve respeitar os princípios SOLID.

Especialmente:

- Single Responsibility
- Dependency Inversion
- Open/Closed

---

# 2. Arquitetura

O projeto segue arquitetura em camadas.

```
Controller
↓

Application Service

↓

Business Service

↓

Repository

↓

Entity Framework Core

↓

Database
```

Controllers nunca acessam Repository diretamente.

Controllers nunca possuem regra de negócio.

---

# 3. Organização do Código

## Namespaces

```csharp
Agilium.Manager.Web.Controllers

Agilium.Manager.Api.V1.Controllers

Agilium.Manager.Business.Interfaces

Agilium.Manager.Business.Services

Agilium.Manager.Business.Models

Agilium.Manager.Business.Validations

Agilium.Manager.Business.Notifications

Agilium.Manager.Data.Context

Agilium.Manager.Data.Repository
```

---

## Ordem dos membros

```text
Campos privados

Constantes

Construtor

Propriedades

Métodos Públicos

Métodos Protegidos

Métodos Privados
```

---

## Regions

❌ Não utilizar #region.

Caso seja necessário utilizar regions, provavelmente a classe está fazendo mais do que deveria.

---

# 4. Convenções de Nomenclatura

## Classes

PascalCase

```
ProdutoService
ClienteRepository
Usuario
Pedido
```

---

## Interfaces

Sempre iniciar com I

```
IProdutoRepository

IProdutoService

INotificador
```

---

## Métodos

PascalCase

```
ObterPorIdAsync()

Cadastrar()

Atualizar()

Excluir()
```

---

## Variáveis

camelCase

```
produto

cliente

listaPedidos
```

---

## Campos privados

Sempre iniciar com "_"

```
private readonly IProdutoRepository _produtoRepository;
```

---

## Constantes

PascalCase

```
public const int QuantidadeMaximaItens = 100;
```

---

# 5. Entity Framework Core

## DbContext

Sempre utilizar:

```csharp
builder.ApplyConfigurationsFromAssembly(typeof(ApplicationDbContext).Assembly);
```

Nunca mapear entidades diretamente no DbContext.

Todo mapeamento deve ficar em:

```
Data/Mappings
```

---

## Queries

Consultas sempre utilizam:

```csharp
AsNoTracking()
```

quando os dados forem somente leitura.

Exemplo:

```csharp
var produtos = await _context.Produtos
    .AsNoTracking()
    .Where(x => x.Ativo)
    .ToListAsync();
```

---

## Include

Sempre explícito.

Nunca depender de Lazy Loading.

```csharp
.Include(x => x.Itens)
.Include(x => x.Cliente)
```

---

## SaveChanges

Nunca chamar diretamente dentro de Repository.

A persistência deve ser centralizada através da Unit of Work (quando existente) ou da abstração definida pelo projeto.

---

# 6. Repository Pattern

Repositories possuem apenas acesso a dados.

Nunca implementar:

- regra de negócio
- validação
- autorização

Repositories retornam entidades.

Nunca ViewModels.

---

# 7. Services

Toda regra de negócio pertence aos Services.

Services:

- validam
- notificam erros
- chamam repositories
- executam transações

Nunca retornar IActionResult.

Nunca acessar HttpContext.

---

# 8. Controllers

Controllers possuem apenas responsabilidade HTTP.

Devem:

- receber requisição
- validar ModelState
- chamar Service
- retornar resposta

Nunca implementar regra de negócio.

---

Exemplo:

```csharp
[HttpPost]

public async Task<IActionResult> Post(ProdutoViewModel vm)
{
    if (!ModelState.IsValid)
        return CustomResponse(ModelState);

    await _produtoService.Adicionar(vm);

    return CustomResponse(vm);
}
```

---

# 9. Notification Pattern

Todo erro de domínio deve utilizar Notification Pattern.

Nunca lançar Exception para erro de negócio.

Exemplo:

```csharp
Notificar("Produto já cadastrado.");
```

Exceptions ficam reservadas apenas para erros inesperados.

---

# 10. FluentValidation

Toda entidade deve possuir sua Validation.

```
ProdutoValidation

ClienteValidation

PedidoValidation
```

Nunca colocar regra de domínio dentro da entidade.

---

# 11. AutoMapper

Todo mapeamento deve ficar em Profiles.

Nunca utilizar Mapper estático.

Sempre:

```csharp
_mapper.Map<Destino>(origem)
```

---

# 12. Async/Await

Todo acesso ao banco deve ser Async.

Proibido:

```
.Result

.Wait()

Task.Run()
```

Métodos assíncronos recebem sufixo Async.

```
ObterPorIdAsync()

AdicionarAsync()
```

Exceção:

Actions do Controller.

---

# 13. Dependency Injection

Sempre utilizar interfaces.

Correto:

```csharp
services.AddScoped<IProdutoRepository, ProdutoRepository>();

services.AddScoped<IProdutoService, ProdutoService>();
```

Nunca registrar Service que utiliza DbContext como Singleton.

---

# 14. Logging

Todo erro deve possuir contexto.

Correto:

```csharp
_logger.LogError(ex,
    "Erro ao consultar produto {ProdutoId}",
    id);
```

Nunca:

```csharp
_logger.LogError(ex, "Erro");
```

---

# 15. Tratamento de Exceções

Não utilizar try/catch desnecessariamente.

Exceptions devem subir até Middleware global.

Capturar exceções apenas quando houver recuperação ou tratamento específico.

---

# 16. Segurança

Nunca armazenar:

- Senhas
- JWT Secret
- Connection String

hardcoded.

Utilizar:

```
appsettings.json

appsettings.Development.json

Environment Variables

User Secrets
```

---

## Autenticação

O projeto utiliza autenticação baseada na entidade **Usuario**.

Não utilizar ASP.NET Identity padrão.

Não criar tabelas:

```
AspNetUsers

AspNetRoles

AspNetUserClaims
```

A autenticação deve utilizar exclusivamente as entidades existentes do domínio.

---

## Autorização

Utilizar:

```
[Authorize]

[AllowAnonymous]
```

somente quando necessário.

---

# 17. ViewModels

ViewModels servem apenas para transporte de dados.

Nunca implementar regra de negócio.

Utilizar DataAnnotations apenas para validações de entrada.

---

# 18. Testes

Nome:

```
Metodo_Condicao_ResultadoEsperado()
```

Exemplo:

```
Adicionar_ProdutoDuplicado_DeveNotificarErro()
```

Preferir:

- Arrange
- Act
- Assert

---

# 19. Formatação

Indentação:

4 espaços

Máximo:

120 caracteres

Chaves:

Allman

Exemplo:

```csharp
if (condicao)
{
    Executar();
}
```

Usings:

- System primeiro
- Ordem alfabética
- Remover não utilizados

Utilizar:

```
nameof()
```

ao invés de strings mágicas.

---

# 20. Boas Práticas

Sempre:

✔ Métodos pequenos

✔ Uma responsabilidade por classe

✔ Um nível de abstração por método

✔ Evitar comentários desnecessários

✔ Preferir composição à herança

✔ Utilizar Dependency Injection

✔ Utilizar CancellationToken em operações longas

✔ Utilizar ConfigureAwait(false) apenas em bibliotecas quando apropriado

✔ Evitar código duplicado

✔ Utilizar expressões LINQ legíveis

✔ Sempre validar argumentos públicos

✔ Sempre utilizar tipos fortes ao invés de string quando possível

---

# 21. O que NÃO fazer

❌ Utilizar Repository dentro do Controller

❌ Utilizar DbContext dentro do Controller

❌ Utilizar ViewModel dentro do Repository

❌ Utilizar Exception para validação

❌ Utilizar Mapper.Map estático

❌ Utilizar .Result ou .Wait()

❌ Utilizar #region

❌ Utilizar métodos com mais de aproximadamente 50 linhas sem justificativa

❌ Utilizar números mágicos

❌ Duplicar regras de negócio

❌ Escrever SQL inline quando houver suporte do EF Core

❌ Criar dependências circulares entre projetos

---

# 22. Checklist antes do Commit

- Código compila sem warnings relevantes
- Não existem TODOs esquecidos
- Não existem Console.WriteLine
- Não existem comentários temporários
- Não existem credenciais no código
- Todos os métodos Async possuem await
- Controllers sem regra de negócio
- Services com regras centralizadas
- Repositories apenas acesso a dados
- Validações implementadas
- Logs adicionados quando necessário
- Código revisado
- Testes executados (quando aplicável)
