# Views Razor

## Objetivo

Documentar a organização das Views Razor do Agilium Manager, padrões de desenvolvimento e convenções específicas do ASP.NET Core MVC.

---

# Visão Geral

As Views Razor são o mecanismo principal de renderização da interface. Organizadas em `Views/{Controller}/`, usam layout AdminLTE 3.x, são tipadas com `@model ViewModel` e utilizam Tag Helpers, Partial Views e View Components para composição da interface.

---

# Organização

```
Views/
├── _ViewImports.cshtml             # @using + @addTagHelper globais
├── _ViewStart.cshtml                # Layout = "_main"
├── Shared/
│   ├── _main.cshtml                 # Layout AdminLTE
│   ├── _Layout.cshtml               # Layout alternativo
│   ├── _ASideMenu.cshtml            # Menu lateral
│   ├── _LoginPartial.cshtml         # Header
│   ├── _rodape.cshtml               # Rodapé
│   ├── _ValidationScriptsPartial.cshtml
│   └── Components/
│       ├── Paginacao/default.cshtml
│       └── Summary/default.cshtml
└── {Controller}/
    ├── Index.cshtml                 # Listagem
    ├── CreateEdit.cshtml            # Criação/edição (compartilhada)
    └── *.cshtml                     # Views específicas
```

---

# Principais Conceitos

### _ViewImports.cshtml

```razor
@using agilum.mvc.web
@using agilum.mvc.web.ViewModels
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *,agilum.mvc.web
```

### _ViewStart.cshtml

```razor
@{
    Layout = "_main";
}
```

### Estrutura de View Típica

```razor
@model ProdutoViewModel
@{
    ViewData["Title"] = "Novo Produto";
}

<form asp-action="Create" method="post">
    <div asp-validation-summary="ModelOnly"></div>
    <!-- campos -->
    <button type="submit">Salvar</button>
</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

---

# Fluxos Relacionados

- `docs/fluxos/` — Cada fluxo tem Views correspondentes

---

# Componentes Relacionados

- `_ViewImports.cshtml` — Namespaces globais
- `_ViewStart.cshtml` — Layout padrão
- `_main.cshtml` — Layout AdminLTE

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Toda View deve ser tipada com `@model ViewModel`
- Criar e Editar compartilham `CreateEdit.cshtml`
- Scripts específicos em `@section Scripts`
- Estilos específicos em `@section Head`
- Não colocar lógica de negócio nas Views
- Usar Partial Views para trechos reutilizáveis

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/razor.md` — Documentação detalhada de Razor Views
- `docs/frontend/mvc.md` — Arquitetura MVC

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `_ViewImports.cshtml` para namespaces disponíveis
2. Navegar para `Views/{Controller}/` para views do domínio
3. Usar `CreateEdit.cshtml` como padrão para formulários
4. Incluir `_ValidationScriptsPartial` em views com formulário

---

# Resumo

Views Razor organizadas por Controller, tipadas com ViewModel, layout AdminLTE via `_ViewStart.cshtml`. Criação e edição compartilham `CreateEdit.cshtml`. Scripts e estilos via `@section`.
