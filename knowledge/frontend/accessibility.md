# Acessibilidade

## Objetivo

Documentar as práticas de acessibilidade no frontend do Agilium Manager: ARIA, navegação por teclado, contraste e boas práticas.

---

# Visão Geral

A acessibilidade é parcialmente herdada do **Bootstrap 4** e **AdminLTE 3.x**, que fornecem atributos ARIA básicos e suporte a navegação por teclado. O projeto não possui um nível formal de conformidade (WCAG), mas segue práticas recomendadas pelo framework.

---

# Principais Conceitos

### ARIA e Semântica

- **Roles**: AdminLTE usa `role="navigation"`, `role="button"`, etc.
- **Labels**: `aria-label` em links e botões da sidebar
- **Landmarks**: `main`, `nav`, `aside`, `footer`

### Navegação por Teclado

- **Tab**: Navega entre elementos focáveis
- **Enter/Space**: Ativa botões e links
- **Escape**: Fecha modais
- **Push menu**: Toggle da sidebar por teclado

### Contraste

- Tema `sidebar-light-navy`: contraste adequado (texto escuro em fundo claro)
- `navbar-warning`: fundo laranja com texto escuro

---

# Fluxos Relacionados

- N/A (acessibilidade é transversal)

---

# Componentes Relacionados

- AdminLTE 3.x — ARIA básico
- Bootstrap 4 — Foco e teclado
- `_main.cshtml` — Estrutura semântica

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Usar elementos HTML semânticos (`<nav>`, `<main>`, `<aside>`)
- Adicionar `aria-label` em ícones sem texto
- Garantir contraste suficiente em customizações
- Manter ordem lógica de tabulação
- Fornecer feedback visual para foco

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/framework.md` — AdminLTE

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Usar elementos HTML semânticos
2. Adicionar `aria-label` em elementos interativos
3. Testar navegação por teclado em novos componentes
4. Verificar contraste em customizações de cor

---

# Resumo

Acessibilidade baseada em Bootstrap 4 + AdminLTE 3.x com ARIA básico e navegação por teclado. Sem conformidade WCAG formal, mas com práticas recomendadas pelos frameworks.
