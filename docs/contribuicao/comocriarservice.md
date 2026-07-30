# Como Criar um Service

## Objetivo

Guia passo a passo para criar um novo **Service** na camada de negócio do Agilium Manager.

---

## Pré-requisitos

- Model da entidade criado em `agilium-manager-azure-business/Models/`
- Repository já implementado
- FluentValidation criado em `Models/Validations/`

---

## Passo a Passo

### 1. Criar Interface

**Local:** `agilium-manager-azure-business/Interfaces/IService/I{Nome}Service.cs`

```csharp
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface I{Nome}Service : IDisposable
    {
        Task Adicionar({Nome} entity);
        Task Atualizar({Nome} entity);
        Task Remover(long id);
        Task<{Nome}> ObterPorId(long id);
        Task<IEnumerable<{Nome}>> ObterTodas(long idEmpresa);
        Task Salvar();
    }
}
```

### 2. Criar Implementação

**Local:** `agilium-manager-azure-business/Services/{Nome}Service.cs`

```csharp
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class {Nome}Service : BaseService, I{Nome}Service
    {
        private readonly I{Nome}Repository _{nome}Repository;

        #region Construtor
        public {Nome}Service(
            I{Nome}Repository {nome}Repository,
            INotificador notificador) : base(notificador)
        {
            _{nome}Repository = {nome}Repository;
        }
        #endregion

        #region CRUD
        public async Task Adicionar({Nome} entity)
        {
            // 1. FluentValidation
            if (!ExecutarValidacao(new {Nome}Validation(), entity))
                return;

            // 2. Regras de negócio adicionais
            // Exemplo: verificar unicidade
            // if (await _{nome}Repository.Existe(x => x.Codigo == entity.Codigo))
            // {
            //     Notificar("Código já cadastrado.");
            //     return;
            // }

            // 3. Persistência (sem salvar)
            await _{nome}Repository.AdicionarSemSalvar(entity);
        }

        public async Task Atualizar({Nome} entity)
        {
            if (!ExecutarValidacao(new {Nome}Validation(), entity))
                return;

            await _{nome}Repository.AtualizarSemSalvar(entity);
        }

        public async Task Remover(long id)
        {
            // Validar se pode remover
            // if (await PodeRemover(id))
            // {
            //     Notificar("Registro em uso. Não é possível remover.");
            //     return;
            // }

            await _{nome}Repository.RemoverSemSalvar(id);
        }

        public async Task<{Nome}> ObterPorId(long id)
        {
            return await _{nome}Repository.ObterPorId(id);
        }

        public async Task<IEnumerable<{Nome}>> ObterTodas(long idEmpresa)
        {
            return await _{nome}Repository.Buscar(x => x.IDEMPRESA == idEmpresa);
        }
        #endregion

        #region Persistência
        public async Task Salvar()
        {
            await _{nome}Repository.SaveChanges();
        }

        public void Dispose()
        {
            _{nome}Repository?.Dispose();
        }
        #endregion
    }
}
```

### 3. Criar FluentValidation

**Local:** `agilium-manager-azure-business/Models/Validations/{Nome}Validation.cs`

```csharp
using agilium.api.business.Models;
using FluentValidation;

namespace agilium.api.business.Models.Validations
{
    public class {Nome}Validation : AbstractValidator<{Nome}>
    {
        public {Nome}Validation()
        {
            RuleFor(x => x.IDEMPRESA)
                .GreaterThan(0).WithMessage("Empresa é obrigatória.");

            // Adicionar regras específicas
            // RuleFor(x => x.Nome)
            //     .NotEmpty().WithMessage("Nome é obrigatório.")
            //     .MaximumLength(100).WithMessage("Máximo 100 caracteres.");
        }
    }
}
```

### 4. Registrar no DI

**Local:** `agilum.mvc.web/Configuration/ResolveDependencyConfig.cs`

```csharp
services.AddScoped<I{Nome}Service, {Nome}Service>();
```

---

## Padrão de Métodos

### Adicionar

```csharp
public async Task Adicionar(Entity entity)
{
    if (!ExecutarValidacao(new EntityValidation(), entity))
        return;

    // Regras de negócio
    // ...

    await _repository.AdicionarSemSalvar(entity);
    // Salvar() é chamado pelo Controller
}
```

### Atualizar

```csharp
public async Task Atualizar(Entity entity)
{
    if (!ExecutarValidacao(new EntityValidation(), entity))
        return;

    await _repository.AtualizarSemSalvar(entity);
}
```

### Orquestração (Ex: Efetivar)

```csharp
public async Task Efetivar(long idCompra)
{
    var compra = await _compraRepository.ObterPorId(idCompra);
    if (compra == null)
    {
        Notificar("Compra não encontrada.");
        return;
    }

    if (compra.Situacao != ESituacaoCompra.Aberta)
    {
        Notificar("Compra não está aberta.");
        return;
    }

    // Orquestrar múltiplas operações
    foreach (var item in compra.Itens)
    {
        await _estoqueService.EntradaEstoque(item.IDPRODUTO, item.Quantidade);
    }

    compra.Situacao = ESituacaoCompra.Efetivada;
    await _compraRepository.AtualizarSemSalvar(compra);

    await _contaService.GerarContaPagar(compra);
}
```

---

## Checklist do Service

☐ Interface em `Interfaces/IService/I{Nome}Service.cs`

☐ Implementação herda de `BaseService`

☐ Construtor recebe `INotificador` e passa para `base(notificador)`

☐ `Adicionar()` chama `ExecutarValidacao()` antes de persistir

☐ `Atualizar()` chama `ExecutarValidacao()` antes de persistir

☐ Usa `AdicionarSemSalvar` / `AtualizarSemSalvar`

☐ Método `Salvar()` delega para o repositório principal

☐ Método `Dispose()` libera todos os repositórios

☐ Implementa `IDisposable`

☐ Registrado como **Scoped** no DI

☐ Sem dependência circular entre serviços

---

## Exemplo Real

Veja o `ProdutoService` como referência completa:

- **Local:** `agilium-manager-azure-business/Services/ProdutoService.cs`
- **Características:** 11 dependências, FluentValidation, CRUD, Dispose
