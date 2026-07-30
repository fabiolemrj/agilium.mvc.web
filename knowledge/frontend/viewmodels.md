# ViewModels

## Objetivo

Documentar a organização e padrões dos ViewModels e DTOs da camada MVC do Agilium Manager.

---

# Visão Geral

ViewModels são DTOs (Data Transfer Objects) da camada de apresentação. Ficam em `agilum.mvc.web/ViewModels/`, organizados por domínio. São mapeados a partir dos Models de negócio via AutoMapper e utilizam Data Annotations para validação client-side.

---

# Organização

```
agilum.mvc.web/ViewModels/
├── Compra/
│   ├── CompraViewModel.cs
│   └── NFeProc.cs
├── Produtos/
│   └── ProdutoViewModel.cs
├── Venda/
│   └── VendaViewModel.cs
├── Empresa/
│   └── EmpresaViewModel.cs
├── Estoque/
├── Fornecedor/
├── Caixa/
├── Cliente/
├── Funcionario/
├── Usuario/
├── Turno/
├── UnidadeViewModel/
├── Impostos/
├── Conta/
├── PlanoConta/
├── Moeda/
├── Vale/
├── Config/
└── ...
```

---

# Principais Conceitos

### ViewModel vs Model

| Camada | Localização | Responsabilidade |
|--------|-------------|------------------|
| **Model** | `agilium-manager-azure-business/Models/` | Entidade de negócio, regras, persistência |
| **ViewModel** | `agilum.mvc.web/ViewModels/` | DTO para View, validação client-side, exibição |

### Data Annotations

```csharp
public class ProdutoViewModel
{
    public long Id { get; set; }

    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100)]
    public string Nome { get; set; }

    [Range(0.01, double.MaxValue)]
    public decimal Preco { get; set; }
}
```

### Mapeamento AutoMapper

```csharp
// AutomapperConfig.cs
CreateMap<Produto, ProdutoViewModel>().ReverseMap();
// Controller
var viewModel = _mapper.Map<ProdutoViewModel>(produto);
```

---

# Fluxos Relacionados

- `docs/fluxos/` — Cada fluxo mapeia Model → ViewModel

---

# Componentes Relacionados

- `AutomapperConfig.cs` — Mapeamentos centralizados
- `_ViewImports.cshtml` — Namespace `@using agilum.mvc.web.ViewModels`

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- ViewModel deve conter apenas dados necessários para a View
- Data Annotations para validação client-side
- Não referenciar entidades de negócio diretamente nas Views
- Usar AutoMapper para conversão (evitar mapeamento manual)
- ViewModel NÃO deve ter lógica de negócio

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/padroes/automapper.md` — Padrão AutoMapper
- `docs/frontend/razor.md` — Views tipadas

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `ViewModels/{Dominio}/` para modelos existentes
2. Criar ViewModel com Data Annotations para validação
3. Adicionar mapeamento em `AutomapperConfig.cs`
4. Usar `_mapper.Map<T>(model)` no Controller

---

# Resumo

ViewModels são DTOs organizados por domínio em `ViewModels/`, com Data Annotations para validação client-side. Mapeados via AutoMapper no Controller.
