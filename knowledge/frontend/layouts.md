# Layouts

## Objetivo

Documentar os layouts da aplicação Agilium Manager, estrutura visual, templates e composição das páginas.

---

# Visão Geral

O layout principal é `_main.cshtml` (AdminLTE 3.x), definido em `_ViewStart.cshtml`. A estrutura visual inclui: navbar superior (`_LoginPartial`), sidebar esquerda (`_ASideMenu`), área de conteúdo (`@RenderBody`) e rodapé (`_rodape`). Um layout alternativo `_Layout.cshtml` está disponível.

---

# Organização

```
Views/Shared/
├── _main.cshtml                 # Layout AdminLTE principal
├── _Layout.cshtml               # Layout alternativo
├── _ASideMenu.cshtml            # Menu lateral
├── _LoginPartial.cshtml         # Header: usuário + empresa + logout
├── _rodape.cshtml               # Rodapé
└── _ValidationScriptsPartial.cshtml
```

### _ViewStart.cshtml

```razor
@{
    Layout = "_main";
}
```

---

# Estrutura do _main.cshtml

```html
<!DOCTYPE html>
<html lang="pt-br">
<head>
    <!-- Fonts: Source Sans Pro -->
    <!-- CSS: Font Awesome, Ionicons, iCheck -->
    <!-- CSS: Bootstrap 4.5.3, AdminLTE -->
    <!-- CSS: Chart.js, Toastr, SweetAlert2, DateRangePicker -->
    <!-- CSS: DataTables (BS4, Responsive, Buttons), Select2 -->
    @RenderSection("Head", required: false)
</head>
<body class="hold-transition sidebar-mini text-md accent-navy">
    <div id="overlay"><!-- loading --></div>
    <div class="wrapper">
        <nav class="main-header navbar navbar-expand navbar-light navbar-warning">
            <partial name="_LoginPartial" />
        </nav>
        <aside class="main-sidebar sidebar-light-navy elevation-4">
            <partial name="_ASideMenu" />
        </aside>
        <div class="content-wrapper">
            @RenderBody()
        </div>
        <partial name="_rodape" />
    </div>
    <div id="myModal" class="modal fade"><!-- Modal global --></div>
    <!-- Scripts: jQuery 3.6.0, Bootstrap 4.6.2, AdminLTE -->
    <!-- Plugins: OverlayScrollbars, Select2, DateRangePicker, jQuery Mask -->
    <!-- Custom: site.js, toastr.min.js, dknotus-tour.min.js -->
    @RenderSection("Scripts", required: false)
</body>
</html>
```

---

# Principais Conceitos

- **Sections**: `Head` (estilos adicionais), `Scripts` (scripts específicos da página)
- **Overlay de loading**: Exibido durante chamadas AJAX
- **Modal global**: `#myModal` para carregar partials dinamicamente
- **Sidebar**: Colapsável (`sidebar-mini`), tema `sidebar-light-navy`
- **Navbar**: Tema `navbar-warning` (laranja/amarelo)
- **Logo**: `Images/logo_v3c_SIMBOLO.png` com texto "AGILIUM MANAGER"

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-autenticacao.md` — Login redireciona para Home

---

# Componentes Relacionados

- `_main.cshtml` — Layout principal
- `_ASideMenu.cshtml` — Navegação lateral
- `_LoginPartial.cshtml` — Informações do usuário
- `_rodape.cshtml` — Rodapé

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Adicionar estilos extras via `@section Head`
- Adicionar scripts extras via `@section Scripts`
- Manter o layout enxuto — apenas estrutura visual comum
- Usar Partial Views para componentes do layout

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/layouts.md` — Documentação de layouts
- `docs/frontend/framework.md` — AdminLTE

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `_ViewStart.cshtml` — define Layout = "_main"
2. Verificar `_main.cshtml` — estrutura completa do layout
3. Verificar `_ASideMenu.cshtml` — links de navegação
4. Verificar `_LoginPartial.cshtml` — header do usuário

---

# Resumo

Layout AdminLTE 3.x com navbar, sidebar colapsável, modal global AJAX e overlay de loading. Sections `Head` e `Scripts` permitem customização por página. jQuery e Bootstrap carregados via CDN.
