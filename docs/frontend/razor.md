# Razor Views

## Objetivo

Documentar a arquitetura das **Razor Views** do Agilium Manager, descrevendo a organização, convenções de desenvolvimento, composição das páginas, sintaxe Razor utilizada e práticas adotadas na camada de apresentação.

Este documento serve como referência para criação, manutenção e evolução das Views da aplicação.

---

## Escopo

Este documento contempla:

- Organização das Views (28 pastas por domínio)
- Layout AdminLTE 3.x (`_main.cshtml`)
- Partial Views compartilhadas
- View Components (`PaginacaoViewComponent`, `SummaryViewComponent`)
- Tag Helpers personalizados (`MoneyInputTagHelper`)
- Html Helpers (`GetEnumValueSelectList`)
- Sintaxe Razor utilizada no projeto
- Padrões de formulários, tabelas e grids
- ViewModels e AutoMapper
- Boas práticas e checklist

---

## Índice

- [Visão Geral](#visão-geral)
- [Arquitetura das Razor Views](#arquitetura-das-razor-views)
- [Organização das Views](#organização-das-views)
- [_ViewImports e _ViewStart](#_viewimports-e-_viewstart)
- [Layout Principal (_main.cshtml)](#layout-principal-_maincshtml)
- [Sintaxe Razor Utilizada](#sintaxe-razor-utilizada)
- [Partial Views](#partial-views)
- [View Components](#view-components)
- [Tag Helpers Personalizados](#tag-helpers-personalizados)
- [Html Helpers](#html-helpers)
- [Padrões de Tela](#padrões-de-tela)
- [ViewModels e AutoMapper](#viewmodels-e-automapper)
- [Convenções](#convenções)
- [Boas Práticas](#boas-práticas)
- [Limitações Conhecidas](#limitações-conhecidas)
- [Checklist](#checklist)

---

## Visão Geral

O Agilium Manager utiliza o mecanismo **ASP.NET Core Razor Views (.NET Core 3.1)** para construção da interface da aplicação MVC.

As Views são organizadas por Controller e utilizam o template **AdminLTE 3.x** (Bootstrap 4) como base visual.

A composição da interface é baseada em:

- **Razor Views** tipadas com `@model ViewModel`
- **Layout principal** `_main.cshtml` (AdminLTE 3.x)
- **Partial Views** para componentes reutilizáveis
- **View Components** para componentes com lógica
- **Tag Helpers** personalizados e nativos
- **AutoMapper** para Model → ViewModel

---

## Arquitetura das Razor Views

```
Controller
      │
      ▼
AutoMapper: Model → ViewModel
      │
      ▼
return View(viewModel)
      │
      ▼
┌──────────────────────────────────────────────┐
│  _ViewStart.cshtml                            │
│  Layout = "_main"                             │
└──────────────────────────────────────────────┘
      │
      ▼
┌──────────────────────────────────────────────┐
│  _main.cshtml (AdminLTE 3.x)                  │
│  ├── <head> CSS + Fonts + RenderSection(Head) │
│  ├── <body class="hold-transition...">        │
│  ├── Navbar (_LoginPartial)                   │
│  ├── Sidebar (_ASideMenu)                     │
│  ├── Content: @RenderBody()                   │
│  ├── Footer (_rodape)                         │
│  └── Scripts: jQuery, Bootstrap, DataTables,  │
│       Select2, Toastr, Chart.js               │
│       @RenderSection("Scripts")               │
└──────────────────────────────────────────────┘
      │
      ▼
@RenderBody() → View específica
      │
      ├── Partial Views (_indexItem, etc.)
      ├── View Components (Paginacao, Summary)
      └── Tag Helpers (<form>, <input>, <select>)
```

---

## Organização das Views

```
Views/
├── _ViewImports.cshtml             # @using + @addTagHelper
├── _ViewStart.cshtml                # Layout = "_main"
│
├── Shared/
│   ├── _main.cshtml                 # Layout AdminLTE principal
│   ├── _Layout.cshtml               # Layout alternativo
│   ├── _ASideMenu.cshtml            # Menu lateral (nav-sidebar)
│   ├── _LoginPartial.cshtml         # Header: nome usuário + empresa + logout
│   ├── _rodape.cshtml               # Rodapé
│   ├── _ValidationScriptsPartial.cshtml  # jQuery validation
│   └── Components/
│       ├── Paginacao/default.cshtml
│       └── Summary/default.cshtml
│
├── Home/          Index.cshtml, Licenca.cshtml
├── Produto/       Index.cshtml, CreateEdit.cshtml, ...
├── Compra/        IndexCompra.cshtml, CreateEdit.cshtml,
│                  Cancelar.cshtml, ListaItemCompra.cshtml,
│                  RetornoXmlNfeImportada.cshtml, ...
├── Venda/         Index.cshtml, ...
├── Caixa/         ...
├── Cliente/       ...
├── Fornecedor/    ...
├── Funcionario/   ...
├── Empresa/       ...
├── Usuario/       ...
├── Estoque/       ...
├── Turno/         ...
├── Conta/         ...
├── PlanoConta/    ...
├── CategoriaFinanceira/ ...
├── FormaPagamento/ ...
├── Moeda/         ...
├── Devolucao/     ...
├── Inventario/    ...
├── Perda/         ...
├── Vale/          ...
├── PontoVenda/    ...
├── Unidade/       ...
├── Config/        ...
├── Log/           ...
└── Endereco/      ...
```

---

## _ViewImports e _ViewStart

### _ViewImports.cshtml

```razor
@using agilum.mvc.web
@using agilum.mvc.web.ViewModels
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, agilum.mvc.web
```

> Disponibiliza namespaces e Tag Helpers para **todas** as Views.

### _ViewStart.cshtml

```razor
@{
    Layout = "_main";
}
```

> Define o layout padrão (`_main.cshtml`) para todas as Views.

---

## Layout Principal (_main.cshtml)

### Estrutura

```razor
@using agilium.api.business.Models
@inject IConfiguration Configuration
@inject IWebHostEnvironment HostingEnvironment

<!DOCTYPE html>
<html lang="pt-br">
<head>
    <title>Agilium Manager</title>
    <!-- Fonts: Source Sans Pro -->
    <!-- CSS: Font Awesome, Ionicons, iCheck, Bootstrap 4.5.3 -->
    <!-- CSS: AdminLTE, Chart.js, Toastr, SweetAlert2, DateRangePicker -->
    <!-- CSS: DataTables (BS4, Responsive, Buttons), Select2 (BS4 theme) -->
    @RenderSection("Head", required: false)
</head>

<body class="hold-transition sidebar-mini text-md accent-navy">
    <!-- Overlay de loading -->
    <div id="overlay" style="display:none;">
        <div class="loader">
            <p>Carregando...</p>
            <img src="~/Images/loading-gif-png-5.gif" />
        </div>
    </div>

    <div class="wrapper">
        <!-- Navbar -->
        <nav class="main-header navbar navbar-expand navbar-light navbar-warning">
            <partial name="_LoginPartial" />
        </nav>

        <!-- Sidebar -->
        <aside class="main-sidebar sidebar-light-navy elevation-4">
            <partial name="_ASideMenu" />
        </aside>

        <!-- Content -->
        <div class="content-wrapper">
            @RenderBody()
        </div>

        <!-- Footer -->
        <partial name="_rodape" />
    </div>

    <!-- Scripts: jQuery, Bootstrap, AdminLTE, DataTables, Select2, Toastr,
         Chart.js, Inputmask, SweetAlert2 -->
    @RenderSection("Scripts", required: false)
</body>
</html>
```

### Bibliotecas CSS/JS Carregadas

| Biblioteca | Uso |
|------------|-----|
| Bootstrap 4.5.3 | Framework CSS base |
| AdminLTE 3.x | Template administrativo |
| Font Awesome | Ícones |
| DataTables + BS4 | Tabelas paginadas, ordenáveis, exportáveis |
| Select2 + BS4 Theme | Dropdowns com busca |
| Toastr | Notificações toast |
| SweetAlert2 | Diálogos modais bonitos |
| Chart.js | Gráficos no dashboard |
| DateRangePicker | Seleção de intervalo de datas |
| Inputmask | Máscaras (CPF, CNPJ, telefone, moeda) |
| iCheck | Checkboxes e radios estilizados |

### RenderSection

| Section | Uso |
|---------|-----|
| `Head` | CSS adicional por página |
| `Scripts` | JavaScript adicional por página |

---

## Sintaxe Razor Utilizada

### Diretivas Básicas

```razor
@model agilum.mvc.web.ViewModels.PagedViewModel<ProdutoViewModel>  {{/* Modelo tipado */}}
@using agilium.api.business.Enums                                     {{/* Namespace */}}
@inject IConfiguration Configuration                                  {{/* Injeção de dependência */}}
@{
    ViewData["Title"] = "Produto";                                    {{/* Bloco de código */}}
    Layout = "_main";                                                 {{/* Layout (no _ViewStart) */}}
}
```

### Blocos de Código Condicionais

```razor
@if (Model.TotalResults == 0)
{
    <div class="alert alert-warning">Nenhum registro encontrado.</div>
}
else
{
    <table class="table table-hover">...</table>
}
```

### Loops (foreach)

```razor
@foreach (var item in Model.List)
{
    <tr>
        <td>@item.Codigo</td>
        <td>@item.NomeFornecedor</td>
    </tr>
}
```

### Funções Locais (Raro)

```razor
@{
    double ConverterStringParaDecimal(string valor, double resultado)
    {
        resultado = 0;
        if (!string.IsNullOrEmpty(valor))
            Double.TryParse(valor, out resultado);
        return resultado;
    }
}
```

### Tag Helpers Nativos

```razor
{{/* Link/Button */}}
<a asp-action="Create" asp-controller="Home" class="btn btn-primary">
    Novo
</a>

{{/* Form */}}
<form asp-action="Index" method="get">
    <input asp-for="PageSize" type="hidden" />
    <input name="q" class="form-control" placeholder="Pesquisar..." />
    <button type="submit"><i class="fa fa-search"></i></button>
</form>

{{/* Validation */}}
<span asp-validation-for="Nome" class="text-danger"></span>

{{/* Partial */}}
<partial name="_LoginPartial" />
<partial name="_indexItem" model="item" />

{{/* AntiForgery (automático com [AutoValidateAntiforgeryToken]) */}}
<form asp-action="Create" method="post">
    @Html.AntiForgeryToken()
</form>
```

### TempData e ViewBag

```razor
@if (TempData["Mensagem"] != null)
{
    <script>
        toastr.success('@TempData["Mensagem"]');
    </script>
}
```

---

## Partial Views

### Catálogo de Partial Views

| Partial View | Localização | Finalidade |
|--------------|-------------|------------|
| `_LoginPartial` | `Views/Shared/` | Header: nome do usuário, empresa, logout |
| `_ASideMenu` | `Views/Shared/` | Menu lateral AdminLTE com ícones e submenus |
| `_rodape` | `Views/Shared/` | Rodapé da aplicação |
| `_ValidationScriptsPartial` | `Views/Shared/` | Scripts jQuery Validation + Unobtrusive |
| `_indexItem` | `Views/Compra/` | Linha da tabela de itens de compra |
| `_createEditItemCompra` | `Views/Compra/` | Formulário de item de compra |
| `_editarItemCompra` | `Views/Compra/` | Modal de edição de item |
| `RetornoXmlNfeImportada` | `Views/Compra/` | Resultado da importação de NFe |

### Uso

```razor
{{/* Inclusão simples */}}
<partial name="_LoginPartial" />

{{/* Com modelo */}}
<partial name="_indexItem" model="item" />
```

---

## View Components

### PaginacaoViewComponent

```csharp
// Extensions/PaginacaoViewComponent.cs
public class PaginacaoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPagedList modeloPaginado)
    {
        return View(modeloPaginado);
    }
}
```

```razor
{{/* Uso na View */}}
@await Component.InvokeAsync("Paginacao", new { modeloPaginado = Model })
```

### SummaryViewComponent

```csharp
// Extensions/SummaryViewComponent.cs
public class SummaryViewComponent : ViewComponent
{
    private readonly INotificador _notificador;

    public SummaryViewComponent(INotificador notificador)
    {
        _notificador = notificador;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());
        notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Mensagem));
        return View();
    }
}
```

> Exibe notificações de validação acumuladas no `INotificador`, adicionando-as ao `ModelState`.

---

## Tag Helpers Personalizados

### MoneyInputTagHelper

```csharp
// Extensions/TagHelpers.cs
[HtmlTargetElement("input", Attributes = "asp-for")]
public class MoneyInputTagHelper : TagHelper
{
    public ModelExpression For { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var type = For.ModelExplorer.ModelType;
        if (type == typeof(double) || type == typeof(double?) ||
            type == typeof(decimal) || type == typeof(decimal?))
        {
            output.Attributes.SetAttribute("class", existingClass + " money");
        }
    }
}
```

> Adiciona automaticamente a classe CSS `money` a inputs `double`/`decimal` para aplicar máscara monetária JavaScript.

---

## Html Helpers

### GetEnumValueSelectList

```csharp
// Extensions/HtmlExtensions.cs
public static IEnumerable<SelectListItem> GetEnumValueSelectList<TEnum>(
    this IHtmlHelper htmlHelper) where TEnum : struct
{
    return new SelectList(Enum.GetValues(typeof(TEnum)).OfType<Enum>()
        .Select(x => new SelectListItem
        {
            Text = x.GetType().GetField(x.ToString())
                     .GetCustomAttribute<DisplayAttribute>()?.Name,
            Value = x.ToString()
        }), "Value", "Text");
}
```

Uso na View:
```razor
<select asp-for="Situacao"
        asp-items="Html.GetEnumValueSelectList<ESituacaoCompra>()"
        class="form-control">
</select>
```

---

## Padrões de Tela

### Página de Listagem (Index)

```razor
@model PagedViewModel<ProdutoViewModel>

<h5>@ViewData["Title"]</h5>
<hr />

{{/* Barra de ferramentas */}}
<section class="barra-de-menu-principal">
    <div class="barra-de-botoes-menu-principal">
        <a asp-action="Create"><span class="fa fa-plus-square"></span></a>
        <a href="#" id="btnAjuda"><span class="fa fa-question"></span></a>
    </div>
    <article>
        <div class="barra-de-posicao-atual" id="breadcrumb">
            <a asp-action="Index" asp-controller="Home">Início</a> / Produto
        </div>
    </article>
</section>

{{/* Filtro de pesquisa */}}
<form asp-action="Index" method="get">
    <input type="hidden" asp-for="PageSize" />
    <input type="hidden" asp-for="PageIndex" />
    <input name="q" class="form-control" placeholder="Pesquisar..." />
    <button type="submit"><i class="fa fa-search"></i></button>
</form>

{{/* Grid de resultados */}}
<table class="table table-hover">
    <thead class="thead-dark">
        <tr>
            <th>Código</th>
            <th>Nome</th>
            <th>Ações</th>
        </tr>
    </thead>
    <tbody>
        @foreach (var item in Model.List)
        {
            <tr>
                <td>@item.Codigo</td>
                <td>@item.Nome</td>
                <td>
                    <a asp-action="Edit" asp-route-id="@item.Id">Editar</a>
                </td>
            </tr>
        }
    </tbody>
</table>

{{/* Paginação */}}
@await Component.InvokeAsync("Paginacao", new { modeloPaginado = Model })
```

### Página de Cadastro/Edição (CreateEdit)

```razor
@model ProdutoViewModel

{{/* Reutiliza mesma View para Create e Edit */}}
@{
    ViewBag.acao = "Create";  {{/* ou "Edit" */}}
    ViewBag.operacao = "I";   {{/* "I"=Insert, "E"=Edit */}}
}

<form asp-action="@ViewBag.acao" method="post">
    @Html.AntiForgeryToken()

    <div class="form-group">
        <label asp-for="Nome"></label>
        <input asp-for="Nome" class="form-control" />
        <span asp-validation-for="Nome" class="text-danger"></span>
    </div>

    <div class="form-group">
        <label asp-for="Preco"></label>
        <input asp-for="Preco" class="form-control money" />
        <span asp-validation-for="Preco" class="text-danger"></span>
    </div>

    <button type="submit" class="btn btn-primary">Salvar</button>
    <a asp-action="Index" class="btn btn-secondary">Cancelar</a>
</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

---

## ViewModels e AutoMapper

- ViewModels organizados por domínio em `ViewModels/{Dominio}/`
- Sufixo `ViewModel` em todos os nomes
- Data Annotations para validação estrutural
- AutoMapper converte **Model → ViewModel** (leitura) e **ViewModel → Model** (escrita)
- Controller nunca recebe Model diretamente na View — sempre via ViewModel

---

## Convenções

| Convenção | Exemplo |
|-----------|---------|
| Pasta = nome do Controller | `Views/Produto/` para `ProdutoController` |
| `@model` tipado (nunca `dynamic`) | `@model PagedViewModel<ProdutoViewModel>` |
| `CreateEdit.cshtml` reutilizado | View de criar e editar usando `ViewBag.operacao` |
| Layout padrão `_main` | Definido em `_ViewStart.cshtml` |
| Partial Views com prefixo `_` | `_LoginPartial.cshtml`, `_indexItem.cshtml` |
| `ViewData["Title"]` em toda View | Define título da página e breadcrumb |
| Seções `Head` e `Scripts` opcionais | Para CSS/JS específicos da página |
| Formulários sempre com AntiForgery | `@Html.AntiForgeryToken()` ou automático |
| Mensagens via `TempData` | `TempData["Mensagem"]` exibido como toast |

---

## Boas Práticas

| Fazer | Evitar |
|-------|--------|
| `@model ViewModel` fortemente tipado | `ViewBag`/`ViewData` para dados complexos |
| Partial Views para repetição | Duplicar HTML entre Views |
| Tag Helpers (`asp-for`, `asp-action`) | HTML manual com strings mágicas |
| `@section Scripts` para JS da página | `<script>` inline no meio do HTML |
| `CreateEdit.cshtml` compartilhado | Views separadas idênticas para Create e Edit |
| `TempData` para mensagens pós-redirect | `ViewBag` entre redirects (perde) |
| View Components para lógica | Lógica complexa na Razor View |
| Data Annotations no ViewModel | Validação inline na View |

---

## Limitações Conhecidas

- **AdminLTE 3.x com Bootstrap 4** — versão anterior ao Bootstrap 5
- **Algumas Views têm funções C# inline** — `ConverterStringParaDecimal()` no `IndexCompra.cshtml`
- **Arquivos `.cshtml` grandes** — `CreateEdit.cshtml` de Compra e Produto são extensos
- **_ViewImports** poderia incluir mais namespaces globais para reduzir `@using` por View
- **Sem componentes Blazor ou SPA** — arquitetura tradicional server-rendered

---

## Checklist

Antes de criar/alterar uma View:

☐ `@model ViewModel` tipado definido

☐ `ViewData["Title"]` preenchido

☐ Breadcrumb consistente com o módulo

☐ Barra de ferramentas com ações principais (Novo, Voltar, Ajuda)

☐ Filtro de pesquisa para páginas de listagem

☐ Tabela com `thead-dark` e classes consistentes

☐ Paginação via `PaginacaoViewComponent` para listas

☐ Formulários com `@Html.AntiForgeryToken()`

☐ `asp-validation-for` em todos os campos

☐ `_ValidationScriptsPartial` na `@section Scripts`

☐ Partial Views para trechos reutilizáveis (não duplicar HTML)

☐ Sem lógica de negócio ou acesso a serviços na View

☐ CSS/JS adicional em `@section Head` ou `@section Scripts`