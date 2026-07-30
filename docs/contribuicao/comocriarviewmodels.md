# Como Criar ViewModels

## Objetivo

Guia passo a passo para criar **ViewModels** seguindo os padrões do projeto Agilium Manager.

---

## O que é um ViewModel

ViewModel é um **DTO da camada de apresentação**. Ele transporta dados entre o Controller e a View, isolando as entidades de domínio da interface.

---

## Tipos de ViewModel

| Tipo | Sufixo | Uso |
|------|--------|-----|
| Listagem | `{Nome}ViewModel` ou `{Nome}IndexViewModel` | Grid/lista no Index |
| Cadastro/Edição | `{Nome}ViewModel` | Formulário Create/Edit |
| Item | `{Nome}ItemViewModel` | Linha de tabela ou partial |
| Dropdown | `{Nome}ListaViewModel` | Select/combobox |

---

## Passo a Passo

### 1. Criar o arquivo

**Local:** `agilum.mvc.web/ViewModels/{Dominio}/{Nome}ViewModel.cs`

### 2. Estrutura Base

```csharp
using System.ComponentModel.DataAnnotations;

namespace agilum.mvc.web.ViewModels.{Dominio}
{
    public class {Nome}ViewModel
    {
        [Key]
        public long Id { get; set; }

        // ===== IDENTIFICAÇÃO =====

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [Display(Name = "Nome")]
        [StringLength(100, MinimumLength = 3,
            ErrorMessage = "O campo {0} deve ter entre {2} e {1} caracteres")]
        public string Nome { get; set; }

        [Display(Name = "Código")]
        public string Codigo { get; set; }

        // ===== VALORES (string para formatação pt-BR) =====

        [Display(Name = "Valor")]
        public string Valor { get; set; }  // string para aceitar "1.234,56"

        [Display(Name = "Quantidade")]
        public double? Quantidade { get; set; }

        // ===== RELACIONAMENTOS (IDs) =====

        [Required(ErrorMessage = "Empresa é obrigatória")]
        [Display(Name = "Empresa")]
        public long? IDEMPRESA { get; set; }

        // ===== DADOS DE EXIBIÇÃO (não persiste) =====

        [Display(Name = "Nome da Empresa")]
        public string NomeEmpresa { get; set; }

        // ===== LISTAS AUXILIARES (dropdowns) =====

        public List<SelectListItemViewModel> Empresas { get; set; }
            = new List<SelectListItemViewModel>();

        // ===== SITUAÇÃO =====

        [Display(Name = "Situação")]
        public int? Situacao { get; set; }

        [Display(Name = "Data de Cadastro")]
        public DateTime? DataCadastro { get; set; }
    }
}
```

---

## Convenções

### Campos Monetários

Valores monetários são tratados como `string` nos ViewModels para compatibilidade com o formato brasileiro (R$ 1.234,56):

```csharp
// ViewModel
public string ValorTotal { get; set; }     // "1.234,56"
public string ValorUnitario { get; set; }  // "99,90"

// Conversão no Controller (via método auxiliar)
private async Task<double> ConverterStringParaDecimal(string valor, double resultado)
{
    resultado = 0;
    if (!string.IsNullOrEmpty(valor))
        Double.TryParse(valor, out resultado);
    return await Task.FromResult(resultado);
}
```

### Listas Auxiliares

Todo ViewModel de formulário deve ter listas para dropdowns:

```csharp
public List<SelectListItemViewModel> Empresas { get; set; } = new List<SelectListItemViewModel>();
public List<SelectListItemViewModel> Categorias { get; set; } = new List<SelectListItemViewModel>();
```

Populadas no Controller:
```csharp
model.Empresas = _mapper.Map<List<SelectListItemViewModel>>(
    await _empresaService.ObterTodas());
```

### Data Annotations Comuns

| Atributo | Uso |
|----------|-----|
| `[Required]` | Campo obrigatório |
| `[StringLength(100, MinimumLength = 3)]` | Tamanho mínimo/máximo |
| `[Range(0.01, double.MaxValue)]` | Intervalo numérico |
| `[Display(Name = "...")]` | Nome amigável |
| `[Key]` | Chave primária |
| `[EmailAddress]` | Validação de e-mail |
| `[DataType(DataType.Date)]` | Campo de data |
| `[Compare("OutraPropriedade")]` | Confirmação (senha) |

---

## AutoMapper

### Registro

**Local:** `agilum.mvc.web/Configuration/AutomapperConfig.cs`

```csharp
// Model → ViewModel (leitura)
CreateMap<{Nome}, {Nome}ViewModel>()
    .ForMember(dest => dest.NomeEmpresa,
               opt => opt.MapFrom(src => src.Empresa != null ? src.Empresa.NMRZSOCIAL : ""));

// ViewModel → Model (escrita)
CreateMap<{Nome}ViewModel, {Nome}>();

// Bidirecional (quando simétrico)
CreateMap<{Nome}, {Nome}ViewModel>().ReverseMap();

// PagedResult
CreateMap<PagedResult<{Nome}>, PagedResult<{Nome}ViewModel>>().ReverseMap();
```

### Uso no Controller

```csharp
// Leitura: Model → ViewModel
var viewModel = _mapper.Map<NomeViewModel>(entidade);

// Escrita: ViewModel → Model
var entidade = _mapper.Map<Nome>(viewModel);
```

---

## Checklist do ViewModel

☐ Arquivo em `ViewModels/{Dominio}/`

☐ Sufixo `ViewModel` no nome da classe

☐ Data Annotations nos campos obrigatórios

☐ `[Display(Name = "...")]` para nomes amigáveis

☐ Listas auxiliares para dropdowns

☐ Campos monetários como `string` (formato pt-BR)

☐ Sem referências a Services ou Repositories

☐ Mapeamento AutoMapper registrado

☐ AutoMapper: `.ReverseMap()` apenas quando simétrico

---

## Exemplos Reais

- **Cadastro/Edição:** `ViewModels/Compra/CompraViewModel.cs`
- **Listagem:** `ViewModels/Compra/CompraIndexViewModel.cs`
- **Item:** `ViewModels/Compra/CompraItemViewModel.cs`
- **Edição Modal:** `ViewModels/Compra/CompraItemEditViewModel.cs`
