# Estilização (CSS)

## Objetivo

Documentar a organização de CSS/SCSS, temas, design system e convenções visuais do Agilium Manager.

---

# Visão Geral

O sistema visual é baseado em **AdminLTE 3.x** sobre **Bootstrap 4.5.3**, com customizações em `wwwroot/css/site.css`. Fontes incluem Source Sans Pro (Google Fonts), Font Awesome 5 (ícones), Ionicons e uma fonte customizada "NightMachine" para o logo.

---

# Organização

```
wwwroot/
├── css/
│   ├── site.css          # Estilos customizados do projeto
│   ├── toastr.css        # Notificações
│   └── toastr.min.css
├── dist/
│   ├── css/
│   │   └── adminlte.min.css   # AdminLTE
│   └── plugins/
│       ├── fontawesome-free/  # Ícones
│       ├── icheck-bootstrap/  # Checkboxes
│       ├── datatables-bs4/    # DataTables Bootstrap 4
│       ├── select2/           # Dropdowns
│       ├── select2-bootstrap4-theme/
│       ├── sweetalert2/       # Modais
│       ├── sweetalert2-theme-bootstrap-4/
│       ├── daterangepicker/   # Datas
│       ├── overlayScrollbars/ # Scrollbar
│       ├── chart.js/          # Gráficos
│       └── tempusdominus-bootstrap-4/ # DateTime
├── font/
│   └── NightMachine/        # Fonte do logo
└── Images/
    └── logo_v3c_SIMBOLO.png # Logo do sistema
```

---

# Principais Conceitos

### Tema de Cores

| Elemento | Classe | Cor |
|----------|--------|-----|
| Sidebar | `sidebar-light-navy` | Fundo claro, destaque azul marinho |
| Navbar | `navbar-warning` | Laranja/amarelo |
| Acento | `accent-navy` | Azul marinho |
| Body | `hold-transition sidebar-mini text-md` | Transições, sidebar colapsada |

### Fontes

- **Source Sans Pro**: Fonte principal (Google Fonts)
- **Font Awesome 5**: Ícones (`fas fa-*`)
- **Ionicons**: Ícones alternativos
- **NightMachine**: Fonte customizada do logo "AGILIUM MANAGER"

### CDN vs Local

| Recurso | Origem |
|---------|--------|
| Bootstrap CSS 4.5.3 | CDN (`cdn.jsdelivr.net`) |
| jQuery 3.6.0 | CDN (`code.jquery.com`) |
| Bootstrap JS 4.6.2 | CDN (`cdn.jsdelivr.net`) |
| Ionicons | CDN (`code.ionicframework.com`) |
| AdminLTE CSS/JS | Local (`wwwroot/dist/`) |
| Plugins | Local (`wwwroot/dist/plugins/`) |

---

# Fluxos Relacionados

- N/A (estilização é transversal)

---

# Componentes Relacionados

- `_main.cshtml` — Carregamento de CSS
- `site.css` — Customizações
- AdminLTE 3.x — Framework base

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Customizações em `site.css` (não editar `adminlte.css`)
- Usar classes Bootstrap/AdminLTE sempre que possível
- Manter consistência de cores (navy + warning)
- Font Awesome para ícones (não usar imagens para ícones)
- Responsividade via classes Bootstrap (`col-md-*`, `d-none d-md-block`)

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/css.md` — Arquitetura CSS
- `docs/frontend/framework.md` — AdminLTE

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `_main.cshtml` — carregamento de CSS/CDN
2. Verificar `site.css` — customizações existentes
3. Para novas estilizações, adicionar em `site.css`
4. Seguir tema `navy + warning` para consistência

---

# Resumo

AdminLTE 3.x + Bootstrap 4.5.3 + customizações em site.css. Tema navy (sidebar) + warning (navbar). Font Awesome para ícones. Bootstrap CSS do CDN, AdminLTE local.
