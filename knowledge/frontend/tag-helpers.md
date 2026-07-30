# Tag Helpers e HTML Helpers

## Objetivo

Documentar o uso de Tag Helpers e HTML Helpers no Agilium Manager, incluindo os nativos do ASP.NET Core e os personalizados do projeto.

---

# Visão Geral

Tag Helpers são a principal forma de gerar HTML no servidor. O projeto utiliza os Tag Helpers nativos do ASP.NET Core (`<form>`, `<input>`, `<select>`, `<a>`, `<label>`, `<partial>`) e pelo menos um Tag Helper personalizado: `MoneyInputTagHelper`. HTML Helpers (`@Html.*`) são usados pontualmente.

---

# Organização

### Tag Helpers Nativos (Microsoft.AspNetCore.Mvc.TagHelpers)

| Tag Helper | Uso |
|------------|-----|
| `asp-action` | Define a action do formulário/link |
| `asp-controller` | Define o controller |
| `asp-route-*` | Parâmetros de rota |
| `asp-for` | Binding com propriedade do Model |
| `asp-validation-for` | Mensagem de erro por campo |
| `asp-validation-summary` | Resumo de erros |
| `asp-items` | Itens de `<select>` |
| `asp-append-version` | Cache busting em scripts/links |

### Tag Helpers Personalizados

| Tag Helper | Localização | Função |
|------------|-------------|--------|
| `MoneyInputTagHelper` | `Extensions/` | Formata campos monetários |

### HTML Helpers

```csharp
@Html.GetEnumValueSelectList<ESituacaoCompra>()  // Dropdown de enum
@Html.Raw()                                       // HTML não escapado
```

---

# Principais Conceitos

### Exemplo de Formulário com Tag Helpers

```razor
<form asp-action="Create" asp-controller="Produto" method="post">
    <div asp-validation-summary="ModelOnly"></div>
    
    <label asp-for="Nome"></label>
    <input asp-for="Nome" class="form-control" />
    <span asp-validation-for="Nome" class="text-danger"></span>
    
    <select asp-for="CategoriaId" asp-items="Model.Categorias"></select>
    
    <button type="submit">Salvar</button>
</form>

@section Scripts {
    <partial name="_ValidationScriptsPartial" />
}
```

### Registro de Tag Helpers

```razor
@* _ViewImports.cshtml *@
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *,agilum.mvc.web
```

---

# Fluxos Relacionados

- `docs/fluxos/` — Todo formulário usa Tag Helpers

---

# Componentes Relacionados

- `_ViewImports.cshtml` — Registro de Tag Helpers
- `MoneyInputTagHelper` — Tag Helper customizado
- HtmlHelpers em `Extensions/`

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Sempre usar `asp-for` em vez de `name` manual
- Sempre usar `asp-validation-for` para erros
- Partial views com `<partial name="..." />`
- Links com `asp-action` e `asp-controller` (nunca hardcoded)
- Scripts com `asp-append-version="true"`

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/razor.md` — Sintaxe Razor
- `knowledge/frontend/forms.md` — Formulários

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Usar Tag Helpers nativos para formulários e links
2. Verificar `_ViewImports.cshtml` para Tag Helpers registrados
3. Verificar `Extensions/` para Tag Helpers e HTML Helpers customizados
4. Seguir o padrão `asp-*` para todos os atributos dinâmicos

---

# Resumo

Tag Helpers nativos do ASP.NET Core são o padrão para formulários, links e validação. Tag Helpers personalizados registrados em `_ViewImports.cshtml`. HTML Helpers usados pontualmente.
