# Diagrama: Frontend

## Objetivo

Representar a arquitetura do frontend do Agilium Manager, incluindo a estrutura de Views, layout AdminLTE, assets estáticos e bibliotecas JavaScript.

---

## Composição da Página

```mermaid
graph TD
    subgraph "_main.cshtml (Layout)"
        direction TB
        Head["<head><br/>CSS + Fonts + RenderSection(Head)"]
        Body["<body class='hold-transition sidebar-mini'>"]
        Overlay["Overlay Loading<br/>#overlay"]
        Wrapper["div.wrapper"]
        Nav["Navbar<br/>_LoginPartial"]
        Sidebar["Sidebar<br/>_ASideMenu"]
        Content["Content<br/>@RenderBody()"]
        Footer["Footer<br/>_rodape"]
        Modal["Modal Global<br/>#myModal"]
        Scripts["Scripts<br/>jQuery + Bootstrap + Plugins<br/>@RenderSection(Scripts)"]
    end

    Head --> Body
    Body --> Overlay
    Body --> Wrapper
    Wrapper --> Nav
    Wrapper --> Sidebar
    Wrapper --> Content
    Wrapper --> Footer
    Body --> Modal
    Body --> Scripts
```

---

## Assets Estáticos

```mermaid
graph TD
    subgraph "wwwroot/"
        Dist["dist/<br/>AdminLTE + 60+ plugins"]
        CSS["css/<br/>site.css, toastr.css"]
        JS["js/<br/>site.js, chartJs.js, toastr.min.js"]
        Lib["lib/<br/>bootstrap, jquery, jquery-validation"]
        Images["Images/<br/>logos, loading GIF"]
        CardapioImgs["imagens-cardapio/"]
        Font["font/<br/>NightMachine.ttf"]
        Local["local/"]
    end

    subgraph "CDN Externo"
        jQueryCDN["jQuery 3.6.0<br/>code.jquery.com"]
        BootstrapCDN["Bootstrap 4.6.2<br/>cdn.jsdelivr.net"]
        MaskCDN["jQuery Mask 1.14.16<br/>cdnjs.cloudflare.com"]
        IoniconsCDN["Ionicons 2.0.1<br/>code.ionicframework.com"]
        FontsCDN["Source Sans Pro<br/>fonts.googleapis.com"]
    end
```

---

## Bibliotecas JavaScript

```mermaid
graph LR
    subgraph "Core"
        jQuery["jQuery 3.6.0"]
        Bootstrap["Bootstrap 4.6.2"]
        AdminLTE["AdminLTE 3.x"]
    end

    subgraph "Tabelas e Dados"
        DataTables["DataTables"]
        DataTablesResp["DataTables Responsive"]
        DataTablesBtn["DataTables Buttons"]
    end

    subgraph "Formulários"
        Select2["Select2"]
        Inputmask["Inputmask"]
        jQueryMask["jQuery Mask"]
        jQueryVal["jQuery Validation"]
        Unobtrusive["Unobtrusive Validation"]
        iCheck["iCheck"]
    end

    subgraph "UI e Notificações"
        Toastr["Toastr"]
        SweetAlert2["SweetAlert2"]
        DateRange["DateRangePicker"]
        OverlayScroll["OverlayScrollbars"]
    end

    subgraph "Gráficos"
        ChartJS["Chart.js"]
    end

    subgraph "Customizado"
        SiteJS["site.js"]
        ChartJsJS["chartJs.js"]
        Tour["dknotus-tour"]
    end

    jQuery --> Bootstrap
    jQuery --> AdminLTE
    jQuery --> DataTables
    jQuery --> Select2
    jQuery --> Inputmask
    jQuery --> Toastr
    jQuery --> SweetAlert2
```

---

## Para Preencher

> **TODO:** Adicionar wireframes das principais telas (Dashboard, Listagem de Produtos, PDV).
