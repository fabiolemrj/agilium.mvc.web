# Frontend

## Objetivo

Índice da documentação de Frontend do Agilium Manager, cobrindo a arquitetura da camada de apresentação MVC, organização das Views Razor, componentes visuais (AdminLTE 3.x + Bootstrap 4), JavaScript e convenções de desenvolvimento.

---

# Visão Geral

O frontend do Agilium Manager é construído com **ASP.NET Core MVC (.NET Core 3.1)** + **Razor Views** + **AdminLTE 3.x** (Bootstrap 4) + **jQuery 3.6.0**. A comunicação com o servidor é feita via AJAX e a autenticação usa Cookie Authentication.

---

# Organização

```
knowledge/frontend/
├── README.md              # Este índice
├── architecture.md        # Arquitetura do frontend
├── routing.md             # Rotas MVC e navegação
├── components.md          # Componentes reutilizáveis
├── pages.md               # Estrutura das páginas (Views)
├── layouts.md             # Layouts e templates
├── forms.md               # Formulários e validação
├── state-management.md    # Gerenciamento de estado
├── services.md            # Serviços frontend
├── authentication.md      # Login e autenticação
├── authorization.md       # Permissões e ClaimsAuthorize
├── navigation.md          # Menus, breadcrumbs, sidebar
├── ui-components.md       # Componentes visuais (AdminLTE)
├── styling.md             # CSS, temas e design
├── responsiveness.md      # Responsividade e breakpoints
├── validation.md          # Validação no cliente
├── localization.md        # Localização e formatação
├── performance.md         # Otimização e boas práticas
├── accessibility.md       # Acessibilidade
├── testing.md             # Testes de frontend
├── deployment.md          # Build e publicação
├── troubleshooting.md     # Problemas comuns
├── views.md               # Organização das Views Razor
├── viewmodels.md          # ViewModels e DTOs MVC
├── tag-helpers.md         # Tag Helpers e HTML Helpers
├── javascript.md          # Scripts JavaScript
└── bundling.md            # Bundling e assets estáticos
```

---

# Principais Conceitos

- **Server-side rendering** com Razor Views (principal)
- **AdminLTE 3.x** como template base administrativo
- **Bootstrap 4.5.3/4.6.2** como framework CSS
- **jQuery 3.6.0** para manipulação DOM e AJAX
- **DataTables, Select2, Toastr, SweetAlert2, Chart.js** como plugins
- **DK Notus Tour (v1.2)** para tour guiado / sistema de ajuda (`btnAjuda`)
- Cookie Authentication com expiração de 3 horas (sliding)
- `ClaimsAuthorizeAttribute` para controle de permissões por ação

---

# Fluxos Relacionados

- `docs/fluxos/` — Fluxos de negócio documentados

---

# Componentes Relacionados

- `agilum.mvc.web/` — Projeto MVC principal
- `agilum.mvc.web/Views/Shared/_main.cshtml` — Layout principal
- `agilum.mvc.web/wwwroot/` — Assets estáticos

---

# APIs Relacionadas

- `agilium-manager-azure-api/` — API REST
- `agilium-pdv-azure-api/` — API do PDV

---

# Boas Práticas

- Views Razor tipadas com `@model ViewModel`
- Partial Views para reutilização de trechos de interface
- View Components para componentes com lógica
- Scripts organizados por funcionalidade em `wwwroot/local/`
- AJAX para carregamento dinâmico de modais e partials

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `knowledge/architecture.md` — Arquitetura do sistema
- `knowledge/development.md` — Desenvolvimento
- `knowledge/patterns.md` — Padrões
- `docs/frontend/` — Documentação oficial
- `docs/frontend/tour-ajuda.md` — Sistema de ajuda guiada (Tour / btnAjuda)

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Ler este README para visão geral
2. Consultar `architecture.md` para entender a estrutura
3. Verificar `docs/frontend/mvc.md` para detalhes da camada MVC
4. Verificar `docs/frontend/razor.md` para padrões de Views
5. Verificar `docs/frontend/framework.md` para AdminLTE e plugins

---

# Resumo

O frontend do Agilium Manager é server-side MVC com Razor Views + AdminLTE + jQuery. A documentação cobre desde arquitetura até troubleshooting, com foco em agentes de IA que precisam navegar e modificar a camada de apresentação.
