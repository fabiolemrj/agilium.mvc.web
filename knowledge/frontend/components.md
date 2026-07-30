# Componentes

## Objetivo

Documentar a organização e padrões para componentes reutilizáveis da camada de apresentação do Agilium Manager: Partial Views, View Components e Tag Helpers.

---

# Visão Geral

O frontend MVC utiliza três mecanismos de componentização: **Partial Views** (trechos de HTML reutilizáveis sem lógica), **View Components** (componentes com lógica própria) e **Tag Helpers** (extensões da sintaxe Razor). A reutilização reduz duplicação e promove consistência visual.

---

# Organização

```
Views/Shared/
├── _main.cshtml                 # Layout principal (AdminLTE)
├── _Layout.cshtml               # Layout alternativo
├── _ASideMenu.cshtml            # Menu lateral
├── _LoginPartial.cshtml         # Header: usuário + empresa + logout
├── _rodape.cshtml               # Rodapé
├── _ValidationScriptsPartial.cshtml  # Scripts de validação
└── Components/
    ├── Paginacao/default.cshtml
    └── Summary/default.cshtml
```

---

# Principais Conceitos

### Partial Views
- Trechos de HTML reutilizáveis sem lógica complexa
- Recebem apenas o modelo passado pelo caller
- Usadas para: menus, cabeçalhos, rodapés, scripts compartilhados
- Exemplos: `_ASideMenu.cshtml`, `_LoginPartial.cshtml`, `_rodape.cshtml`

### View Components
- Componentes com lógica própria de obtenção de dados
- Localizados em `Extensions/` como classes C# + Views em `Shared/Components/`
- Exemplos:
  - `PaginacaoViewComponent` — Renderiza controles de paginação
  - `SummaryViewComponent` — Renderiza resumos/seções

### Tag Helpers
- Extensões da sintaxe Razor processadas no servidor
- Exemplo: `MoneyInputTagHelper` — Formata campos monetários
- Tag Helpers nativos do ASP.NET Core: `<form>`, `<input>`, `<select>`, `<a>`, `<label>`

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-venda.md` — Uso de modais via AJAX

---

# Componentes Relacionados

- `PaginacaoViewComponent.cs` — `Extensions/`
- `SummaryViewComponent.cs` — `Extensions/`
- `MoneyInputTagHelper` — Tag Helper customizado
- `#myModal` — Modal global para carregar partials via AJAX

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Partial Views para reutilização de trechos de HTML sem lógica
- View Components quando o componente precisa consultar dados
- Tag Helpers para estender a sintaxe Razor
- Manter componentes pequenos e com responsabilidade única
- Documentar parâmetros esperados por cada Partial View

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/components.md` — Guia de componentes
- `docs/frontend/razor.md` — Razor Views

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `Views/Shared/` para Partial Views existentes
2. Verificar `Extensions/` para View Components e Tag Helpers
3. Reutilizar componentes existentes antes de criar novos
4. Seguir o padrão de nomenclatura: `_NomePartial.cshtml`

---

# Resumo

Componentização via Partial Views (HTML reutilizável), View Components (lógica + view) e Tag Helpers (extensões Razor). Centralizados em `Views/Shared/` e `Extensions/`.
