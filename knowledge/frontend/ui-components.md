# Componentes Visuais (UI)

## Objetivo

Documentar a biblioteca de componentes visuais utilizados no Agilium Manager: AdminLTE 3.x, Bootstrap 4, plugins e padrões de interface.

---

# Visão Geral

A interface é construída sobre **AdminLTE 3.x** (template administrativo) + **Bootstrap 4.5.3/4.6.2** (framework CSS). Dezenas de plugins são integrados para componentes ricos: DataTables, Select2, SweetAlert2, Toastr, Chart.js, DateRangePicker, etc.

---

# Principais Conceitos

### Componentes AdminLTE

| Componente | Classe CSS | Uso |
|-----------|------------|-----|
| Sidebar | `main-sidebar sidebar-light-navy` | Menu lateral colapsável |
| Navbar | `main-header navbar-warning` | Barra superior |
| Content Wrapper | `content-wrapper` | Área de conteúdo |
| Cards | `card card-*` | Agrupamento visual |
| Info Boxes | `info-box` | Indicadores numéricos |

### Plugins Ativos

| Plugin | Uso |
|--------|-----|
| **DataTables** | Tabelas com paginação, busca, ordenação, exportação |
| **Select2** | Dropdowns com busca e multiseleção |
| **SweetAlert2** | Diálogos de confirmação (excluir, cancelar) |
| **Toastr** | Notificações toast (sucesso, erro, aviso) |
| **Chart.js** | Gráficos (dashboard) |
| **DateRangePicker** | Seleção de intervalo de datas |
| **Inputmask** | Máscaras de input (CPF, CNPJ, telefone, CEP) |
| **iCheck** | Checkboxes e radios estilizados |
| **Summernote** | Editor WYSIWYG |
| **OverlayScrollbars** | Scrollbar customizada |
| **TempusDominus** | DateTime picker |

### Modais

- **Modal Bootstrap**: `#myModal` global para carregar partials via AJAX
- **SweetAlert2**: Confirmações (ex: "Deseja cancelar esta compra?")

---

# Fluxos Relacionados

- `docs/fluxos/` — Cada fluxo usa componentes específicos

---

# Componentes Relacionados

- `wwwroot/dist/plugins/` — 60+ plugins AdminLTE
- `wwwroot/css/site.css` — Estilos customizados
- `wwwroot/js/site.js` — Inicialização de plugins

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Usar classes Bootstrap/AdminLTE para consistência visual
- DataTables para qualquer listagem com +10 registros
- SweetAlert2 para confirmações destrutivas
- Toastr para feedback de operações (sucesso/erro)
- Select2 para dropdowns com muitas opções
- Manter plugins atualizados nas versões empacotadas

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/framework.md` — AdminLTE e estrutura de plugins
- `docs/frontend/css.md` — Arquitetura CSS

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `docs/frontend/framework.md` para lista completa de plugins
2. Verificar `wwwroot/dist/plugins/` para plugins disponíveis
3. Usar classes Bootstrap/AdminLTE para novos componentes
4. Consultar documentação do AdminLTE 3.x para padrões

---

# Resumo

UI baseada em AdminLTE 3.x + Bootstrap 4 + 60+ plugins. Componentes principais: DataTables (tabelas), Select2 (dropdowns), SweetAlert2 (confirmações), Toastr (notificações), Chart.js (gráficos).
