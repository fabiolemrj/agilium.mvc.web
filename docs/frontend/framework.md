# Framework AdminLTE

## Objetivo

Documentar o framework **AdminLTE 3.x** utilizado como template base da interface do Agilium Manager, incluindo sua estrutura, dependências, plugins integrados, customizações e convenções de uso.

---

## Visão Geral

O AdminLTE é um template administrativo open-source construído sobre **Bootstrap 4**, que fornece uma interface responsiva com sidebar, navbar, dashboard e dezenas de plugins integrados.

O Agilium Manager utiliza:

- **AdminLTE 3.x** (versão estável, comunidade ativa)
- **Bootstrap 4.5.3 / 4.6.2** (framework CSS)
- **jQuery 3.6.0** (manipulação DOM e plugins)

---

## Estrutura de Arquivos (wwwroot)

```
wwwroot/
├── dist/
│   ├── css/
│   │   ├── adminlte.css            # AdminLTE CSS completo
│   │   └── adminlte.min.css        # AdminLTE CSS minificado
│   │
│   ├── js/
│   │   ├── adminlte.min.js         # AdminLTE JS (sidebar, push-menu, treeview)
│   │   └── adminlte.min.js.map
│   │
│   ├── img/                        # Imagens do template (avatar, logo padrão, bg)
│   │
│   └── plugins/                    # 60+ plugins oficiais do AdminLTE
│       ├── bootstrap/              # Bootstrap JS
│       ├── bootstrap-colorpicker/
│       ├── bootstrap-slider/
│       ├── bootstrap-switch/
│       ├── bs-custom-file-input/
│       ├── bs-stepper/
│       ├── chart.js/               # Gráficos
│       ├── codemirror/             # Editor de código
│       ├── datatables/             # Tabelas (core)
│       ├── datatables-bs4/         # DataTables Bootstrap 4
│       ├── datatables-responsive/  # Tabelas responsivas
│       ├── datatables-buttons/     # Botões (exportar, imprimir)
│       ├── daterangepicker/        # Seleção de intervalo de datas
│       ├── dropzone/               # Upload de arquivos
│       ├── ekko-lightbox/          # Lightbox para imagens
│       ├── fastclick/              # Elimina delay de touch
│       ├── filterizr/              # Filtros de galeria
│       ├── flag-icon-css/          # Bandeiras de países
│       ├── flot/                   # Gráficos (alternativo)
│       ├── fontawesome-free/       # Ícones Font Awesome
│       ├── fullcalendar/           # Calendário
│       ├── icheck-bootstrap/       # Checkboxes/radios estilizados
│       ├── inputmask/              # Máscaras de input
│       ├── ion-rangeslider/        # Slider de intervalo
│       ├── jquery/                 # jQuery (bundled)
│       ├── jquery-knob/            # Knob circular
│       ├── jquery-ui/              # jQuery UI
│       ├── jquery-validation/      # Validação de formulários
│       ├── jqvmap/                 # Mapas vetoriais
│       ├── jsgrid/                 # Grid editável
│       ├── moment/                 # Manipulação de datas
│       ├── overlayScrollbars/      # Scrollbar customizada
│       ├── pace-progress/          # Barra de progresso no topo
│       ├── popper/                 # Posicionamento de tooltips/popovers
│       ├── raphael/                # SVG (dependência de gráficos)
│       ├── select2/                # Dropdowns com busca
│       ├── select2-bootstrap4-theme/ # Tema Bootstrap 4 para Select2
│       ├── sparklines/             # Mini gráficos inline
│       ├── summernote/             # Editor WYSIWYG
│       ├── sweetalert2/            # Diálogos modais
│       ├── sweetalert2-theme-bootstrap-4/
│       ├── tempusdominus-bootstrap-4/ # DateTime picker
│       ├── toastr/                 # Notificações toast
│       └── uplot/                  # Gráficos (leve)
│
├── lib/
│   ├── bootstrap/                  # Bootstrap (alternativo)
│   ├── jquery/                     # jQuery (alternativo)
│   ├── jquery-validation/          # jQuery Validation
│   └── jquery-validation-unobtrusive/ # Unobtrusive Validation
│
├── css/
│   ├── site.css                    # Estilos customizados do projeto
│   └── toastr.css                  # Toastr (alternativo)
│
├── js/
│   ├── site.js                     # JavaScript customizado
│   ├── toastr.min.js               # Toastr (alternativo)
│   ├── chartJs.js                  # Configuração de gráficos
│   └── dknotus-tour.min.js         # Tour guiado
│
├── Images/                         # Imagens do sistema
├── imagens-cardapio/               # Imagens do cardápio digital
├── font/                           # Fontes customizadas (NightMachine)
└── local/                          # Assets localizados
```

---

## Layout Principal (_main.cshtml)

### Estrutura HTML

```html
<!DOCTYPE html>
<html lang="pt-br">
<head>
    <!-- Fontes e CSS -->
    @RenderSection("Head", required: false)
</head>

<body class="hold-transition sidebar-mini text-md accent-navy">
    <!-- Overlay de loading -->
    <div id="overlay" style="display:none;">
        <div class="loader">
            <p>Carregando...</p>
            <img src="~/Images/loading-gif-png-5.gif" />
        </div>
    </div>

    <div class="wrapper">
        <!-- Navbar superior -->
        <nav class="main-header navbar navbar-expand navbar-light navbar-warning">
            <partial name="_LoginPartial" />
        </nav>

        <!-- Sidebar esquerda -->
        <aside class="main-sidebar sidebar-light-navy elevation-4">
            <!-- Logo -->
            <a class="brand-link navbar-warning">
                <img src="~/Images/logo_v3c_SIMBOLO.png"
                     class="brand-image img-circle elevation-3" />
                <span class="fonteLogoAgilium">AGILIUM MANAGER</span>
            </a>
            <partial name="_AsideMenu" />
        </aside>

        <!-- Conteúdo principal -->
        <div class="content-wrapper">
            <div class="content-header"></div>
            <div class="conteudo content">
                <div class="container-fluid">
                    @RenderBody()
                </div>
            </div>
        </div>

        <!-- Rodapé -->
        <partial name="_rodape" />
    </div>

    <!-- Modal global para carregar partials via AJAX -->
    <div id="myModal" class="modal fade" tabindex="-1">
        <div class="modal-dialog modal-lg">
            <div class="modal-content" id="myModalContent"></div>
        </div>
    </div>

    <!-- Scripts -->
    <script src="https://code.jquery.com/jquery-3.6.0.min.js"></script>
    <script src="https://cdn.jsdelivr.net/npm/bootstrap@4.6.2/dist/js/bootstrap.bundle.min.js"></script>
    <script src="~/dist/js/adminlte.min.js"></script>
    <!-- ... plugins ... -->
    @RenderSection("Scripts", required: false)
</body>
</html>
```

### Classes CSS do AdminLTE Utilizadas

| Classe | Descrição |
|--------|-----------|
| `hold-transition` | Animações de transição |
| `sidebar-mini` | Sidebar colapsada por padrão |
| `text-md` | Tamanho de fonte médio |
| `accent-navy` | Cor de destaque azul marinho |
| `wrapper` | Container principal do layout |
| `main-header` | Barra superior (navbar) |
| `navbar-warning` | Navbar com tema laranja/amarelo |
| `main-sidebar` | Sidebar principal |
| `sidebar-light-navy` | Sidebar clara com destaque azul |
| `elevation-4` | Sombra nível 4 |
| `brand-link` | Área do logo na sidebar |
| `brand-image img-circle` | Logo circular |
| `content-wrapper` | Área de conteúdo |
| `content-header` | Cabeçalho do conteúdo |
| `container-fluid` | Container fluido |
| `modal fade` | Modal Bootstrap |
| `modal-dialog modal-lg` | Modal tamanho grande |

---

## Plugins Ativamente Utilizados

| Plugin | Versão | Uso no Projeto |
|--------|--------|----------------|
| **Bootstrap** | 4.5.3 / 4.6.2 | Framework CSS base |
| **jQuery** | 3.6.0 | Manipulação DOM, AJAX, base para plugins |
| **DataTables** | (BS4) | Tabelas paginadas, ordenáveis, com busca |
| **DataTables Responsive** | — | Tabelas adaptáveis a mobile |
| **DataTables Buttons** | — | Exportar (Excel, PDF, CSV), imprimir |
| **Select2** | 4.x (BS4 theme) | Dropdowns com busca e multi-select |
| **Toastr** | 2.x | Notificações toast (sucesso, erro, info) |
| **SweetAlert2** | — | Diálogos modais de confirmação |
| **Chart.js** | — | Gráficos no dashboard |
| **DateRangePicker** | — | Seleção de intervalo de datas nos filtros |
| **Inputmask** | — | Máscaras de input (CPF, CNPJ, telefone, moeda) |
| **jQuery Mask** | 1.14.16 (CDN) | Máscaras complementares |
| **jQuery Validation** | — | Validação client-side de formulários |
| **jQuery Unobtrusive** | — | Integração Data Annotations → validação |
| **iCheck** | (BS4) | Checkboxes e radios estilizados |
| **OverlayScrollbars** | — | Scrollbar customizada |
| **Font Awesome** | 5.x (free) | Ícones (`fas fa-*`, `fab fa-*`, `far fa-*`) |
| **Ionicons** | 2.0.1 (CDN) | Ícones complementares |
| **dknotus-tour** | — | Tour guiado pela interface |

---

## Plugins Disponíveis mas Não Utilizados Ativamente

| Plugin | Potencial Uso Futuro |
|--------|---------------------|
| `summernote` | Editor WYSIWYG para campos de texto rico |
| `dropzone` | Upload de arquivos com drag & drop |
| `fullcalendar` | Calendário de eventos/agendamentos |
| `flot` / `uplot` | Gráficos alternativos |
| `jqvmap` | Mapa de regiões (vendas por estado) |
| `sparklines` | Mini gráficos em tabelas |
| `codemirror` | Editor de código (configurações avançadas) |
| `bs-stepper` | Wizard multi-etapas |
| `filterizr` | Filtros de galeria |
| `jsgrid` | Grid editável inline |
| `pace-progress` | Barra de progresso no topo da página |
| `ekko-lightbox` | Visualizador de imagens |

---

## Customizações do Projeto

### CSS Customizado (`wwwroot/css/site.css`)

```css
/* Fonte customizada NightMachine */
@font-face {
    font-family: 'NightMachine';
    src: url('../font/NightMachine-rwMB.ttf') format('truetype');
}

/* Tamanho de fonte base */
html { font-size: 14px; }
@media (min-width: 768px) { html { font-size: 16px; } }

/* Logo na home */
.home-logo {
    max-width: 420px;
    margin: 20px auto;
    display: block;
}

/* Cores do tema */
.btn-primary { background-color: #1b6ec2; }
.nav-pills .nav-link.active { background-color: #1b6ec2; }
a { color: #0366d6; }
```

### JavaScript Customizado (`wwwroot/js/site.js`)

```javascript
// Overlay de loading
function on() {
    document.getElementById("overlay").style.display = "block";
}
function off() {
    document.getElementById("overlay").style.display = "none";
}
```

### Configuração de Gráficos (`wwwroot/js/chartJs.js`)

Arquivo separado para configuração de gráficos Chart.js usados no dashboard.

### Tour Guiado (`wwwroot/js/dknotus-tour.min.js`)

Biblioteca de tour guiado pela interface para novos usuários.

---

## Modal Global

O `_main.cshtml` define um modal reutilizável para carregar partial views via AJAX:

```html
<div id="myModal" class="modal fade" tabindex="-1">
    <div class="modal-dialog modal-lg">
        <div class="modal-content" id="myModalContent"></div>
    </div>
</div>
```

Uso típico (JavaScript):
```javascript
$.get('/Compra/EditarItemModal?id=' + id, function(data) {
    $('#myModalContent').html(data);
    $('#myModal').modal('show');
});
```

---

## Overlay de Loading

Todas as páginas têm um overlay de loading controlado por JavaScript:

```html
<div id="overlay" style="display:none;">
    <div class="loader">
        <p class="mt-3 text-white font-weight-bold">Carregando...</p>
        <img id="imgLoader" src="~/Images/loading-gif-png-5.gif" />
    </div>
</div>
```

Ativado nos links de ação:
```razor
<a asp-action="Create" onclick="on()">Novo</a>
```

Desativado após carregamento da página.

---

## Convenções de Uso

### Ícones

| Prefixo | Fonte | Exemplo |
|---------|-------|---------|
| `fas fa-*` | Font Awesome Solid | `fas fa-plus-square`, `fas fa-search` |
| `fab fa-*` | Font Awesome Brands | `fab fa-whatsapp` |
| `far fa-*` | Font Awesome Regular | `far fa-edit` |
| `fa fa-*` | Font Awesome (v4 legado) | `fa fa-question`, `fa fa-dashboard` |
| `sns-tool-action` | Classe customizada | Ícones da barra de ferramentas |

### Cores de Tema

| Classe | Cor | Uso |
|--------|-----|-----|
| `navbar-warning` | Laranja/Amarelo | Navbar e logo |
| `sidebar-light-navy` | Claro + Azul Marinho | Sidebar |
| `accent-navy` | Azul Marinho | Cor de destaque |
| `btn-info` | Azul claro | Botões de ação |
| `thead-dark` | Escuro | Cabeçalho de tabelas |
| `btn-primary` | Azul (#1b6ec2) | Botão principal |

### Tabelas

```html
<table class="table table-hover">
    <thead class="thead-dark">
        <tr>...</tr>
    </thead>
    <tbody>...</tbody>
</table>
```

### Formulários

```html
<div class="form-group">
    <label asp-for="Nome"></label>
    <input asp-for="Nome" class="form-control" />
    <span asp-validation-for="Nome" class="text-danger"></span>
</div>
```

### Barra de Ferramentas

```html
<section class="barra-de-menu-principal">
    <div class="barra-de-botoes-menu-principal">
        <a asp-action="Create" onclick="on()"
           title="Cadastrar Novo Registro">
            <span class="fa fa-plus-square sns-tool-action"></span>
        </a>
        <a href="#" title="Voltar" id="btnVoltar">
            <span class="fas fa-reply sns-tool-action"></span>
        </a>
    </div>
    <article>
        <div class="barra-de-posicao-atual" id="breadcrumb">
            <a asp-action="Index" asp-controller="Home">Início</a> /
            <a href="#">Módulo</a>
        </div>
    </article>
</section>
```

---

## Responsividade

O AdminLTE é responsivo por padrão:

- **Desktop**: sidebar visível, tabelas completas
- **Tablet**: sidebar colapsável, tabelas com scroll horizontal
- **Mobile**: sidebar oculta (toggle), tabelas responsivas (DataTables Responsive)

Classes Bootstrap utilizadas para grid:
```html
<div class="row">
    <div class="col-lg-6 col-md-6 col-sm-6 col-xs-12">...</div>
    <div class="col-lg-3 col-md-3 col-sm-3">...</div>
</div>
```

---

## Dependências

| Recurso | Origem | Versão |
|---------|--------|--------|
| Bootstrap CSS | CDN (`cdn.jsdelivr.net`) | 4.5.3 |
| Bootstrap JS Bundle | CDN | 4.6.2 |
| jQuery | CDN (`code.jquery.com`) | 3.6.0 |
| jQuery Mask | CDN (`cdnjs.cloudflare.com`) | 1.14.16 |
| Ionicons | CDN (`code.ionicframework.com`) | 2.0.1 |
| Google Fonts | CDN (`fonts.googleapis.com`) | Source Sans Pro |
| AdminLTE CSS/JS | Local (`wwwroot/dist/`) | 3.x |
| Plugins | Local (`wwwroot/dist/plugins/`) | — |

---

## Boas Práticas

- Manter AdminLTE e plugins **locais** em `wwwroot/dist/` (não depender de CDN)
- CSS customizado em `wwwroot/css/site.css` (não modificar `adminlte.css`)
- JS customizado em `wwwroot/js/site.js` (não modificar `adminlte.min.js`)
- Usar classes do AdminLTE/Bootstrap sempre que possível (evitar CSS inline)
- Ícones via Font Awesome (consistência visual)
- Modal global `#myModal` para partials via AJAX
- Overlay de loading em ações que demoram
- Scripts no final do `<body>` (antes de `</body>`)

---

## Checklist

Antes de adicionar novas dependências front-end:

☐ Verificar se o plugin já existe em `wwwroot/dist/plugins/`

☐ Preferir plugin já incluso no AdminLTE a adicionar nova lib

☐ CSS customizado em `site.css`, não no `adminlte.css`

☐ JS customizado em `site.js`, não inline no HTML

☐ Usar classes Bootstrap/AdminLTE existentes (grid, cores, componentes)

☐ Ícones via Font Awesome (`fas fa-*`)

☐ Testar responsividade (mobile, tablet, desktop)

☐ Overlay de loading para ações com delay perceptível

☐ Scripts no `@section Scripts` (página) ou no `_main.cshtml` (global)
