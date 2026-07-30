# Como Criar Views

## Objetivo

Guia passo a passo para criar **Views Razor** seguindo os padrões do projeto Agilium Manager (AdminLTE 3.x + Bootstrap 4).

---

## Estrutura de Arquivos

```
Views/{Nome}/
├── Index.cshtml          # Página de listagem
├── CreateEdit.cshtml     # Formulário de cadastro/edição (reutilizado)
└── _partial.cshtml       # Partials específicas (se necessário)
```

---

## Passo a Passo: Index (Listagem)

### 1. Criar o arquivo

**Local:** `Views/{Nome}/Index.cshtml`

### 2. Template

```razor
@using agilium.api.business.Enums
@using agilum.mvc.web.ViewModels.{Dominio}
@model agilum.mvc.web.ViewModels.PagedViewModel<{Nome}ViewModel>
@using agilum.mvc.web.Extensions

@{
    ViewData["Title"] = "Nome da Entidade";
}

<h5>@ViewData["Title"]</h5>
<hr />

@* ===== BARRA DE FERRAMENTAS ===== *@
<section class="barra-de-menu-principal">
    <div class="barra-de-botoes-menu-principal">
        <a href="#" title="Voltar" id="btnVoltar">
            <span class="fas fa-reply sns-tool-action"></span>
        </a>
        <a href="#" title="Precisa de Ajuda?" id="btnAjuda">
            <span class="fa fa-question sns-tool-action"></span>
        </a>
        <a asp-action="Create" onclick="on()" title="Cadastrar Novo Registro">
            <span class="fa fa-plus-square sns-tool-action" id="btnNovoCadastro"></span>
        </a>
    </div>

    <article>
        <div class="barra-de-posicao-atual" id="breadcrumb">
            <a asp-action="Index" asp-controller="Home"> Início</a> /
            <a href="#"> Nome da Entidade </a>
        </div>
    </article>
</section>

<br />

@* ===== FILTRO ===== *@
<div class="row">
    <div class="col-12" id="areaFiltro">
        <form asp-action="Index" class="sidebar-form" method="get">
            <input type="hidden" asp-for="PageSize" />
            <input type="hidden" asp-for="PageIndex" />
            <div class="row">
                <div class="col-lg-6">
                    <input name="q" class="form-control"
                           placeholder="Pesquisar por descrição..."
                           value="@ViewBag.Pesquisa" />
                </div>
                <div>
                    <button type="submit" class="btn btn-info btn-flat">
                        <i class="fa fa-search"></i>
                    </button>
                </div>
            </div>
        </form>
    </div>
</div>

@* ===== GRID ===== *@
<div id="divGridResultado">
    <table class="table table-hover">
        <thead class="thead-dark">
            <tr>
                <th>Código</th>
                <th>Descrição</th>
                <th>Situação</th>
                <th>Ações</th>
            </tr>
        </thead>
        <tbody>
            @if (Model.List.Any())
            {
                @foreach (var item in Model.List)
                {
                    <tr>
                        <td>@item.Codigo</td>
                        <td>@item.Nome</td>
                        <td>@item.Situacao</td>
                        <td>
                            <a asp-action="Edit" asp-route-id="@item.Id"
                               title="Editar" onclick="on()">
                                <span class="fa fa-pencil-alt"></span>
                            </a>
                        </td>
                    </tr>
                }
            }
            else
            {
                <tr>
                    <td colspan="4" class="text-center">
                        Nenhum registro encontrado.
                    </td>
                </tr>
            }
        </tbody>
    </table>

    @* Paginação *@
    @await Component.InvokeAsync("Paginacao", new { modeloPaginado = Model })
</div>

@section Scripts {
    <script>
        $(document).ready(function () {
            // Configurações específicas da página
        });
    </script>
}
```

---

## Passo a Passo: CreateEdit (Formulário)

### 1. Criar o arquivo

**Local:** `Views/{Nome}/CreateEdit.cshtml`

> **Reutilizado para Create e Edit** — diferenciado por `ViewBag.operacao`.

### 2. Template

```razor
@using agilum.mvc.web.ViewModels.{Dominio}
@model {Nome}ViewModel

@{
    ViewData["Title"] = ViewBag.operacao == "I" ? "Novo Registro" : "Editar Registro";
}

<h5>@ViewData["Title"]</h5>
<hr />

@* ===== BARRA DE FERRAMENTAS ===== *@
<section class="barra-de-menu-principal">
    <div class="barra-de-botoes-menu-principal">
        <a href="#" title="Voltar" id="btnVoltar">
            <span class="fas fa-reply sns-tool-action"></span>
        </a>
        <a href="#" title="Precisa de Ajuda?" id="btnAjuda">
            <span class="fa fa-question sns-tool-action"></span>
        </a>
    </div>

    <article>
        <div class="barra-de-posicao-atual" id="breadcrumb">
            <a asp-action="Index" asp-controller="Home"> Início</a> /
            <a asp-action="Index">Nome da Entidade</a> /
            <a href="#">@ViewData["Title"]</a>
        </div>
    </article>
</section>

<br />

@* ===== FORMULÁRIO ===== *@
<form asp-action="@ViewBag.acao" method="post">
    @Html.AntiForgeryToken()
    <input type="hidden" asp-for="Id" />

    <div class="row">
        <div class="col-md-8">
            <div class="card card-primary">
                <div class="card-header">
                    <h3 class="card-title">Dados Principais</h3>
                </div>
                <div class="card-body">
                    @* Campo: Nome *@
                    <div class="form-group">
                        <label asp-for="Nome"></label>
                        <input asp-for="Nome" class="form-control" />
                        <span asp-validation-for="Nome" class="text-danger"></span>
                    </div>

                    @* Campo: Empresa (dropdown) *@
                    <div class="form-group">
                        <label asp-for="IDEMPRESA"></label>
                        <select asp-for="IDEMPRESA" class="form-control select2"
                                asp-items="new SelectList(Model.Empresas, nameof(SelectListItemViewModel.Value), nameof(SelectListItemViewModel.Text))">
                            <option value="">Selecione...</option>
                        </select>
                        <span asp-validation-for="IDEMPRESA" class="text-danger"></span>
                    </div>

                    @* Campo: Valor (monetário) *@
                    <div class="form-group">
                        <label asp-for="Valor"></label>
                        <input asp-for="Valor" class="form-control money" />
                        <span asp-validation-for="Valor" class="text-danger"></span>
                    </div>
                </div>
            </div>
        </div>

        <div class="col-md-4">
            <div class="card card-secondary">
                <div class="card-header">
                    <h3 class="card-title">Ações</h3>
                </div>
                <div class="card-body">
                    <button type="submit" class="btn btn-primary btn-block"
                            onclick="on()">
                        <i class="fas fa-save"></i> Salvar
                    </button>
                    <a asp-action="Index" class="btn btn-secondary btn-block">
                        <i class="fas fa-times"></i> Cancelar
                    </a>
                </div>
            </div>
        </div>
    </div>
</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
    <script>
        $(document).ready(function () {
            // Inicializar Select2
            $('.select2').select2({
                theme: 'bootstrap4'
            });

            // Inicializar máscaras
            $('.money').mask('#.##0,00', { reverse: true });
        });
    </script>
}
```

---

## Componentes Disponíveis

### Tabelas

```razor
<table class="table table-hover">
    <thead class="thead-dark">...</thead>
    <tbody>...</tbody>
</table>
```

### Cards

```razor
<div class="card card-primary">
    <div class="card-header">
        <h3 class="card-title">Título</h3>
    </div>
    <div class="card-body">
        Conteúdo
    </div>
</div>
```

### Grid (Bootstrap)

```razor
<div class="row">
    <div class="col-lg-6 col-md-6 col-sm-12">Coluna 1</div>
    <div class="col-lg-6 col-md-6 col-sm-12">Coluna 2</div>
</div>
```

### Alertas

```razor
@if (!ViewData.ModelState.IsValid)
{
    <div class="alert alert-danger">
        <ul>
            @foreach (var error in ViewData.ModelState.Values
                .SelectMany(v => v.Errors))
            {
                <li>@error.ErrorMessage</li>
            }
        </ul>
    </div>
}
```

### Dropdown (Select2)

```razor
<select asp-for="IDEMPRESA" class="form-control select2"
        asp-items="new SelectList(Model.Empresas, 'Value', 'Text')">
    <option value="">Selecione...</option>
</select>
```

---

## Checklist da View

☐ `@model` tipado com ViewModel (nunca `dynamic`)

☐ `ViewData["Title"]` definido

☐ Barra de ferramentas com breadcrumb

☐ Filtro de pesquisa para Index (se listagem)

☐ Tabela com `thead-dark` e `table-hover`

☐ Paginação via `PaginacaoViewComponent`

☐ CreateEdit reutilizado (ViewBag.operacao)

☐ Form POST com `@Html.AntiForgeryToken()`

☐ `asp-validation-for` em todos os campos

☐ `_ValidationScriptsPartial` na `@section Scripts`

☐ Select2 inicializado para dropdowns com busca

☐ Máscaras monetárias (`$('.money').mask(...)`)

☐ Overlay de loading no submit (`onclick="on()"`)

☐ Sem lógica de negócio na View

---

## Exemplos Reais

- **Listagem:** `Views/Produto/Index.cshtml`
- **Create/Edit:** `Views/Compra/CreateEdit.cshtml`
- **Partial:** `Views/Compra/_indexItem.cshtml`
- **Modal:** `Views/Compra/_editarItemCompra.cshtml`
