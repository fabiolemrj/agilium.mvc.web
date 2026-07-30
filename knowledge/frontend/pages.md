# Páginas (Views)

## Objetivo

Documentar a estrutura das páginas (Views Razor) do Agilium Manager, organização por controller e responsabilidades de cada tipo de View.

---

# Visão Geral

As Views são organizadas em pastas por Controller dentro de `Views/{Controller}/`. Cada Controller possui Views para operações CRUD: `Index.cshtml` (listagem), `CreateEdit.cshtml` (criação/edição — compartilhada), views específicas de domínio e partials auxiliares.

---

# Organização

```
Views/
├── Home/          Index.cshtml, Licenca.cshtml
├── Produto/       Index.cshtml, CreateEdit.cshtml
├── Compra/        IndexCompra.cshtml, CreateEdit.cshtml,
│                  Cancelar.cshtml, ListaItemCompra.cshtml,
│                  RetornoXmlNfeImportada.cshtml
├── Venda/         Index.cshtml, ...
├── Caixa/         ...
├── Cliente/       ...
├── Fornecedor/    ...
├── Funcionario/   ...
├── Empresa/       Index.cshtml, SelecionarEmpresa.cshtml
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

# Principais Conceitos

- **View tipada**: Toda View usa `@model ViewModel` — nunca `ViewBag` para dados principais
- **CreateEdit compartilhado**: Criação e edição usam a mesma View (`CreateEdit.cshtml`)
- **Partial Views**: Trechos reutilizáveis como `_indexItem.cshtml`
- **Sections**: `@section Scripts { }` para scripts específicos da página
- **Modais**: Carregados via AJAX no `#myModal` global

---

# Padrão de Página CRUD

```
Index.cshtml        → Listagem com DataTables, filtros, paginação
CreateEdit.cshtml   → Formulário de criação/edição com validação
_*.cshtml           → Partials auxiliares (itens, modais, scripts)
```

---

# Fluxos Relacionados

- `docs/fluxos/` — Fluxos de negócio (cada fluxo tem Views correspondentes)

---

# Componentes Relacionados

- `_main.cshtml` — Layout que envolve todas as páginas
- `PaginacaoViewComponent` — Usado nas listagens
- DataTables, Select2 — Plugins usados nas páginas

---

# APIs Relacionadas

- N/A — Views consomem Models via Controller

---

# Boas Práticas

- Views tipadas com `@model ViewModel`
- Formulários com `asp-action`, `asp-controller`, `asp-route-*`
- Validação client-side com jQuery Unobtrusive Validation
- Scripts específicos em `@section Scripts`
- Não colocar lógica de negócio nas Views

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/razor.md` — Padrões de Razor Views
- `docs/frontend/mvc.md` — Controllers e rotas

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Identificar o Controller relacionado à funcionalidade
2. Navegar para `Views/{Controller}/` 
3. Verificar `Index.cshtml` para listagem
4. Verificar `CreateEdit.cshtml` para formulários
5. Consultar `_ViewImports.cshtml` para namespaces disponíveis

---

# Resumo

Views organizadas por Controller, tipadas com ViewModel, usando layout AdminLTE. Criação e edição compartilham `CreateEdit.cshtml`. Scripts e estilos específicos via `@section`.
