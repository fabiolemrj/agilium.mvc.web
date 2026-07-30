# JavaScript

## Objetivo

Documentar a organização dos scripts JavaScript no Agilium Manager, bibliotecas utilizadas, padrões de código e comunicação com o servidor.

---

# Visão Geral

O JavaScript complementa a renderização server-side. A principal biblioteca é **jQuery 3.6.0**, usada para manipulação DOM, AJAX e inicialização de plugins. Scripts customizados estão em `wwwroot/js/site.js` e `wwwroot/local/` organizados por funcionalidade.

---

# Organização

```
wwwroot/
├── js/
│   ├── site.js                 # JavaScript customizado principal
│   ├── chartJs.js              # Configuração de gráficos
│   ├── toastr.min.js           # Notificações
│   └── dknotus-tour.min.js     # Tour guiado
├── local/
│   ├── global.js               # Funções globais (loading, modais)
│   ├── empresa.js              # Seleção de empresa
│   ├── empresaCreate.js        # Cadastro de empresa
│   ├── Processo/
│   │   └── compra.js           # Lógica de compras
│   ├── Financeiro/
│   ├── cadastros/
│   ├── usuario/
│   └── Ferramentas/
└── lib/
    ├── jquery/
    ├── jquery-validation/
    └── jquery-validation-unobtrusive/
```

---

# Principais Conceitos

### Bibliotecas Carregadas

| Biblioteca | Versão | Origem |
|-----------|--------|--------|
| jQuery | 3.6.0 | CDN (`code.jquery.com`) |
| jQuery Mask | 1.14.16 | CDN (`cdnjs.cloudflare.com`) |
| Bootstrap JS | 4.6.2 | CDN (`cdn.jsdelivr.net`) |
| AdminLTE JS | — | Local (`dist/js/adminlte.min.js`) |
| DataTables | — | Local (`dist/plugins/datatables/`) |
| Select2 | — | Local (`dist/plugins/select2/`) |
| SweetAlert2 | — | Local (`dist/plugins/sweetalert2/`) |
| Chart.js | — | Local (`dist/plugins/chart.js/`) |
| Toastr | — | Local (`js/toastr.min.js`) |
| DK Notus Tour | 1.2 | Local (`js/dknotus-tour.min.js`) |

### Padrão AJAX

```javascript
$.ajax({
    url: '/compra/importar',
    type: 'POST',
    data: formData,
    beforeSend: function () {
        $('#overlay').show();
    },
    success: function (result) {
        $('#myModalContent').html(result);
        $('#myModal').modal('show');
    },
    error: function (xhr) {
        toastr.error('Erro ao processar requisição');
    },
    complete: function () {
        $('#overlay').hide();
    }
});
```

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-compra.md` — `compra.js`
- `docs/fluxos/fluxo-venda.md` — Scripts do PDV

---

# Componentes Relacionados

- `_main.cshtml` — Carregamento de scripts globais
- `#myModal` — Modal carregado via AJAX
- `#overlay` — Loading durante AJAX

---

# APIs Relacionadas

- Actions MVC (chamadas via AJAX)

---

# Boas Práticas

- Scripts específicos em `wwwroot/local/` por funcionalidade
- AJAX com `beforeSend` (overlay) e `complete` (esconder overlay)
- Tratar erros com `toastr.error()`
- Usar `$(document).ready()` para inicialização
- Evitar scripts inline — usar arquivos `.js` separados

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/javascript.md` — Arquitetura JavaScript
- `docs/frontend/tour-ajuda.md` — Sistema de ajuda guiada (Tour / btnAjuda)
- `knowledge/frontend/ui-components.md` — Plugins

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `_main.cshtml` para ordem de carregamento de scripts
2. Verificar `wwwroot/local/` para scripts existentes por funcionalidade
3. Seguir padrão AJAX com overlay + toastr
4. Para novas funcionalidades, criar script em `wwwroot/local/{Modulo}/`

---

# Resumo

JavaScript complementar com jQuery 3.6.0 + plugins. Scripts organizados em `wwwroot/local/` por funcionalidade. AJAX com overlay de loading e feedback Toastr.
