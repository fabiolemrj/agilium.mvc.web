# Bundling e Assets Estáticos

## Objetivo

Documentar a estratégia de bundling, minificação e carregamento de assets estáticos (CSS, JS, imagens) no Agilium Manager.

---

# Visão Geral

O Agilium Manager **não utiliza** bundling/minificação em tempo de build (como Webpack ou Gulp). As bibliotecas já vêm minificadas (AdminLTE, plugins) e são servidas via `UseStaticFiles`. jQuery e Bootstrap são carregados via CDN para performance.

---

# Organização

### wwwroot (servido via UseStaticFiles)

```
wwwroot/
├── dist/               # AdminLTE + plugins (minificados)
├── lib/                # Bibliotecas alternativas
├── css/                # site.css, toastr.css
├── js/                 # site.js, chartJs.js, toastr.min.js
├── local/              # Scripts organizados por funcionalidade
├── Images/             # Imagens do sistema
├── imagens-cardapio/   # Imagens do cardápio digital
├── font/               # Fontes customizadas (NightMachine)
└── favicon.ico
```

### CDN vs Local

| Recurso | Origem | Versão |
|---------|--------|--------|
| jQuery | `code.jquery.com` | 3.6.0 |
| Bootstrap CSS | `cdn.jsdelivr.net` | 4.5.3 |
| Bootstrap JS | `cdn.jsdelivr.net` | 4.6.2 |
| jQuery Mask | `cdnjs.cloudflare.com` | 1.14.16 |
| Ionicons | `code.ionicframework.com` | 2.0.1 |
| Google Fonts | `fonts.googleapis.com` | Source Sans Pro |
| AdminLTE CSS/JS | Local (`dist/`) | 3.x |
| Plugins | Local (`dist/plugins/`) | — |
| site.css / site.js | Local | Custom |

---

# Principais Conceitos

### Cache Busting

Scripts locais usam `asp-append-version="true"`:

```html
<script src="~/js/site.js" asp-append-version="true"></script>
```

Isso adiciona um hash ao nome do arquivo, forçando o browser a baixar a nova versão quando o arquivo muda.

### Ordem de Carregamento

1. CSS no `<head>` (Bootstrap CDN, AdminLTE, plugins, site.css)
2. HTML no `<body>`
3. Scripts no final do `<body>` (jQuery CDN, Bootstrap CDN, AdminLTE, plugins, site.js)

---

# Fluxos Relacionados

- N/A (assets são transversais)

---

# Componentes Relacionados

- `_main.cshtml` — Carregamento de CSS e JS
- `Startup.cs` — `UseStaticFiles()`
- `wwwroot/` — Todos os assets

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Bibliotecas grandes via CDN (cache cross-site)
- AdminLTE e plugins locais para controle de versão
- `asp-append-version="true"` para cache busting em assets locais
- Imagens otimizadas (PNG para logo, JPEG para fotos)
- Evitar duplicação de bibliotecas (CDN + local)

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/frontend/framework.md` — Estrutura do wwwroot
- `knowledge/frontend/performance.md` — Performance

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `_main.cshtml` para ordem de carregamento
2. Adicionar novos assets em `wwwroot/` na pasta apropriada
3. Usar `asp-append-version` para scripts locais
4. Preferir CDN para bibliotecas grandes e comuns

---

# Resumo

Sem bundling em tempo de build. Bibliotecas já minificadas. jQuery/Bootstrap via CDN. AdminLTE/plugins locais. Cache busting com `asp-append-version`. Assets servidos via `UseStaticFiles`.
