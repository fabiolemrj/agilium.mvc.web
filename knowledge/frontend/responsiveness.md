# Responsividade

## Objetivo

Documentar o comportamento responsivo da interface do Agilium Manager, breakpoints e adaptação para diferentes dispositivos.

---

# Visão Geral

A responsividade é herdada do **AdminLTE 3.x** e **Bootstrap 4**, que utilizam um sistema de grid flexível com breakpoints padrão. A sidebar é colapsável (`sidebar-mini`) e se adapta automaticamente em dispositivos móveis.

---

# Organização

### Breakpoints Bootstrap 4

| Breakpoint | Largura | Comportamento |
|-----------|---------|---------------|
| **xs** | < 576px | Sidebar oculta, toggle manual |
| **sm** | ≥ 576px | Sidebar colapsada |
| **md** | ≥ 768px | Sidebar parcialmente visível |
| **lg** | ≥ 992px | Sidebar expandida |
| **xl** | ≥ 1200px | Layout completo |

### Classes Responsivas

- Grid: `col-md-*`, `col-lg-*`
- Visibilidade: `d-none d-md-block`
- Sidebar: `sidebar-mini` (colapsa automaticamente)
- Tabelas: DataTables com `responsive` plugin
- Modais: `modal-dialog modal-lg` adaptável

---

# Principais Conceitos

- **AdminLTE sidebar-mini**: Sidebar colapsa em telas menores
- **Bootstrap Grid**: Layout flexível com 12 colunas
- **DataTables Responsive**: Colunas ocultas em telas pequenas com expand
- **Modal Bootstrap**: Redimensiona automaticamente
- **Push menu**: Toggle da sidebar no mobile

---

# Fluxos Relacionados

- N/A (responsividade é transversal)

---

# Componentes Relacionados

- `_main.cshtml` — Classes `hold-transition sidebar-mini`
- AdminLTE 3.x — Comportamento responsivo nativo
- DataTables — Plugin responsive
- Bootstrap 4 — Grid system

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Usar classes Bootstrap para grid (`col-md-*`, `col-lg-*`)
- Testar em resoluções mobile (375px) e tablet (768px)
- Tabelas com DataTables + plugin responsive
- Modais com `modal-lg` para conteúdo extenso
- Evitar larguras fixas — usar `container-fluid`

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/framework.md` — AdminLTE
- `docs/frontend/css.md` — Grid Bootstrap

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Usar classes Bootstrap para responsividade
2. DataTables com `responsive: true` para tabelas
3. Sidebar AdminLTE já é responsiva nativamente
4. Testar novos componentes em múltiplas resoluções

---

# Resumo

Responsividade via AdminLTE 3.x + Bootstrap 4 grid. Sidebar colapsável, DataTables responsive, modais adaptáveis. Breakpoints padrão Bootstrap.
