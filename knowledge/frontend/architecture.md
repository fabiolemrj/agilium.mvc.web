# Arquitetura do Frontend

## Objetivo

Documentar a arquitetura da camada de apresentação do Agilium Manager, incluindo organização das camadas, módulos, responsabilidades e fluxo de renderização.

---

# Visão Geral

O frontend segue o padrão **ASP.NET Core MVC (.NET Core 3.1)** com renderização server-side. A arquitetura é composta por: Controllers (28), Views Razor organizadas por domínio, ViewModels tipados, Layout AdminLTE 3.x, Partial Views, View Components, Tag Helpers e JavaScript complementar (jQuery + plugins).

---

# Organização

```
agilum.mvc.web/
├── Controllers/            # 28 controllers MVC
│   └── MainController.cs   # Classe base (INotificador, IMapper, IUser)
├── Views/                  # Views Razor
│   ├── _ViewImports.cshtml # @using + @addTagHelper globais
│   ├── _ViewStart.cshtml   # Layout = "_main"
│   ├── Shared/             # Layouts, partials, componentes
│   │   ├── _main.cshtml    # Layout AdminLTE principal
│   │   ├── _Layout.cshtml  # Layout alternativo
│   │   └── Components/     # View Components
│   └── {Controller}/       # Uma pasta por controller
├── ViewModels/             # DTOs da camada de apresentação
├── wwwroot/                # Assets estáticos
│   ├── dist/               # AdminLTE (css, js, plugins)
│   ├── lib/                # Bibliotecas (bootstrap, jquery)
│   ├── css/                # site.css, toastr.css
│   ├── js/                 # site.js, chartJs.js, toastr.min.js
│   ├── local/              # Scripts organizados por funcionalidade
│   └── Images/             # Imagens do sistema
├── Configuration/          # Identity, AutoMapper, DI, MVC
├── Extensions/             # Middlewares, HtmlHelpers, TagHelpers, ViewComponents
├── Services/               # AuthService, ServiceEmail, CryptoService
└── Areas/Identity/         # Razor Pages (login, logout, registro)
```

---

# Camadas do Frontend

```
Browser (AdminLTE + jQuery + AJAX)
      │
      ▼
Middleware Pipeline (Startup.Configure)
      │
      ▼
Controller (MainController)
      │  └── IMapper: Model → ViewModel
      │
      ▼
View Razor (_main.cshtml + @RenderBody)
      │  ├── Partial Views
      │  ├── View Components
      │  └── Tag Helpers
      │
      ▼
HTML + CSS + JavaScript → Browser
```

---

# Principais Conceitos

- **Server-side rendering**: A maior parte da lógica de apresentação está nas Views Razor
- **AdminLTE 3.x**: Template administrativo responsivo com sidebar, navbar e plugins
- **jQuery + AJAX**: Comunicação assíncrona para modais, buscas e carregamento parcial
- **AutoMapper**: Converte Model (negócio) → ViewModel (apresentação)
- **Notification Pattern**: Erros de negócio acumulados e exibidos na View
- **Cookie Authentication**: 3h com sliding expiration

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-autenticacao.md` — Pipeline de autenticação
- `docs/fluxos/fluxo-configuracao.md` — Seleção de empresa

---

# Componentes Relacionados

- `MainController` — Classe base de todos os controllers
- `_main.cshtml` — Layout AdminLTE
- `EmpresaSelecionadaMiddleware` — Bloqueia acesso sem empresa
- `RequisitoClaimFilter` — Valida permissões por idTag

---

# APIs Relacionadas

- N/A — Frontend MVC consome serviços internos, não APIs REST diretamente

---

# Boas Práticas

- Views tipadas com `@model ViewModel` (nunca usar `ViewBag` para dados principais)
- Partial Views para reutilização de trechos de interface
- View Components para componentes com lógica própria
- Scripts em `wwwroot/local/` organizados por funcionalidade
- AJAX via jQuery com tratamento de erros do servidor

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/mvc.md` — Arquitetura MVC detalhada
- `docs/frontend/razor.md` — Padrões de Razor Views
- `docs/frontend/framework.md` — AdminLTE e plugins
- `knowledge/architecture.md` — Arquitetura geral do sistema

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `Startup.cs` — pipeline de middleware
2. Verificar `_main.cshtml` — estrutura do layout
3. Verificar `MainController.cs` — classe base
4. Verificar `_ViewImports.cshtml` — namespaces e Tag Helpers globais
5. Verificar `AutomapperConfig.cs` — mapeamentos Model → ViewModel

---

# Resumo

Frontend server-side MVC com 28 controllers, Views Razor tipadas, AdminLTE 3.x + Bootstrap 4, jQuery para AJAX e plugins. A renderização é server-side com JavaScript complementar para interatividade.
