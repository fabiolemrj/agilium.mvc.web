# Formulários

## Objetivo

Documentar os padrões de formulários, validação e componentes de entrada utilizados no Agilium Manager.

---

# Visão Geral

Os formulários do Agilium Manager usam **Razor Tag Helpers** do ASP.NET Core com **jQuery Unobtrusive Validation** no cliente e **FluentValidation + Notification Pattern** no servidor. Os formulários são submetidos via POST tradicional ou via AJAX para modais.

---

# Organização

- **Criação/Edição**: Views `CreateEdit.cshtml` com formulários tipados
- **Validação client-side**: jQuery Validation + Unobtrusive (Data Annotations)
- **Validação server-side**: FluentValidation + `ModelState.IsValid`
- **Modais**: Formulários carregados via AJAX no `#myModal`

---

# Principais Conceitos

### Estrutura de Formulário

```razor
@model ProdutoViewModel

<form asp-action="Create" method="post">
    <div asp-validation-summary="ModelOnly" class="text-danger"></div>
    
    <div class="form-group">
        <label asp-for="Nome"></label>
        <input asp-for="Nome" class="form-control" />
        <span asp-validation-for="Nome" class="text-danger"></span>
    </div>
    
    <button type="submit" class="btn btn-primary">Salvar</button>
</form>
```

### Validação Client-Side
- **jQuery Unobtrusive Validation**: Baseada em Data Annotations do ViewModel
- Scripts: `jquery.validate.js` + `jquery.validate.unobtrusive.js`
- Carregados via `_ValidationScriptsPartial.cshtml`

### Validação Server-Side
- **Controller**: `if (!ModelState.IsValid) return View(model)`
- **Service**: `ExecutarValidacao(new ProdutoValidation(), produto)`
- **Notification Pattern**: Erros acumulados e exibidos na View

### Plugins de Formulário
- **Select2**: Dropdowns com busca
- **Inputmask**: Máscaras (CPF, CNPJ, telefone, CEP)
- **DateRangePicker**: Seleção de intervalo de datas
- **iCheck**: Checkboxes e radios estilizados
- **Summernote**: Editor WYSIWYG

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-produto.md` — Cadastro de produto

---

# Componentes Relacionados

- `_ValidationScriptsPartial.cshtml` — Scripts de validação
- `#myModal` — Modal para formulários AJAX
- Select2, Inputmask, DateRangePicker — Plugins

---

# APIs Relacionadas

- N/A — POST para actions MVC

---

# Boas Práticas

- Sempre usar `asp-validation-summary` e `asp-validation-for`
- Validar no cliente (Data Annotations) E no servidor (FluentValidation)
- Usar `ModelState.AddModelError()` para erros de negócio
- Usar Select2 para dropdowns com muitas opções
- Submeter formulários de modal via AJAX e fechar/fechar com refresh

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/padroes/validacoes.md` — Arquitetura de validações
- `docs/frontend/razor.md` — Sintaxe Razor

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `_ValidationScriptsPartial.cshtml` para scripts de validação
2. Usar `asp-for` Tag Helpers para binding automático
3. Incluir `asp-validation-for` para mensagens de erro
4. Verificar Data Annotations no ViewModel para validação client-side
5. Verificar FluentValidation no Service para validação server-side

---

# Resumo

Formulários com Razor Tag Helpers + jQuery Unobtrusive Validation + FluentValidation. Dupla validação (cliente e servidor). Plugins: Select2, Inputmask, DateRangePicker. Modais carregados via AJAX.
