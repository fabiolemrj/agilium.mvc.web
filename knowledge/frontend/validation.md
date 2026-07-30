# Validação no Cliente

## Objetivo

Documentar as validações realizadas no lado do cliente (browser) do Agilium Manager, mensagens de erro e UX de formulários.

---

# Visão Geral

A validação client-side usa **jQuery Unobtrusive Validation**, que lê os Data Annotations dos ViewModels e gera regras de validação JavaScript automaticamente. Complementada por validação server-side com FluentValidation + Notification Pattern.

---

# Organização

### Camadas de Validação

```
[Cliente]  jQuery Unobtrusive Validation
              ↓ (Data Annotations do ViewModel)
[Controller] ModelState.IsValid
              ↓
[Service]   FluentValidation + Regras de Negócio
              ↓
[Notificador] Notification Pattern (acumula erros)
```

### Scripts

- `jquery.validate.js` — Core da validação
- `jquery.validate.unobtrusive.js` — Adaptador para Data Annotations
- Carregados via `_ValidationScriptsPartial.cshtml`

---

# Principais Conceitos

### Data Annotations → Validação Client-Side

```csharp
public class ProdutoViewModel
{
    [Required(ErrorMessage = "O nome é obrigatório")]
    [StringLength(100, MinimumLength = 3)]
    public string Nome { get; set; }

    [Range(0.01, double.MaxValue, ErrorMessage = "Preço deve ser maior que zero")]
    public decimal Preco { get; set; }
}
```

### HTML Gerado

```html
<input asp-for="Nome" class="form-control" />
<span asp-validation-for="Nome" class="text-danger"></span>
```

### Exibição de Erros do Servidor

```csharp
if (!OperacaoValida())
{
    foreach (var erro in ObterNotificacoes())
        ModelState.AddModelError(string.Empty, erro);
    return View(model);
}
```

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-produto.md` — Validação no cadastro

---

# Componentes Relacionados

- `_ValidationScriptsPartial.cshtml` — Scripts de validação
- `jquery.validate.js` + `jquery.validate.unobtrusive.js`
- Inputmask — Validação de formato (CPF, CNPJ, telefone)

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Data Annotations no ViewModel para validação client-side
- FluentValidation no Service para validação server-side
- `asp-validation-summary` para resumo de erros
- `asp-validation-for` para erros por campo
- Inputmask para validação de formato em tempo real
- Mensagens de erro claras e em português

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/padroes/validacoes.md` — Arquitetura completa de validações
- `knowledge/frontend/forms.md` — Formulários

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Adicionar Data Annotations no ViewModel
2. Usar `asp-validation-for` nos campos do formulário
3. Incluir `_ValidationScriptsPartial` na View
4. Para validações complexas, usar FluentValidation no Service
5. Tratar erros do servidor com `ModelState.AddModelError()`

---

# Resumo

Validação em 3 camadas: jQuery Unobtrusive (client-side, Data Annotations), ModelState.IsValid (controller) e FluentValidation (service). Erros de negócio retornam via Notification Pattern.
