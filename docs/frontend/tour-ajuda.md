# Sistema de Ajuda Guiada (Tour / btnAjuda)

## Objetivo

Documentar o funcionamento do recurso de ajuda guiada do Agilium Manager, implementado via botões `id="btnAjuda"` que acionam tours passo-a-passo com a biblioteca `dknotus-tour.js`. Este documento serve como referência para desenvolvimento e manutenção do sistema de ajuda por agentes de IA e desenvolvedores.

---

## Escopo

Este documento contempla:

- Arquitetura do sistema de ajuda
- O botão `btnAjuda` no HTML
- O manipulador JavaScript (`Tour.run`)
- A biblioteca `dknotus-tour.js` v1.2
- Como adicionar ajuda a uma nova página
- Distribuição atual por módulo
- Problemas conhecidos e boas práticas

---

## Índice

- [Visão Geral](#visão-geral)
- [Arquitetura de 3 Camadas](#arquitetura-de-3-camadas)
- [1. O Botão HTML (`btnAjuda`)](#1-o-botão-html-btnajuda)
- [2. O Manipulador JavaScript](#2-o-manipulador-javascript)
- [3. A Biblioteca `dknotus-tour.js`](#3-a-biblioteca-dknotus-tourjs)
- [Como Adicionar Ajuda a uma Nova Página](#como-adicionar-ajuda-a-uma-nova-página)
- [Checklist de Criação de Tour](#checklist-de-criação-de-tour)
- [Distribuição por Módulo](#distribuição-por-módulo)
- [Fluxo de Execução](#fluxo-de-execução)
- [Problemas Conhecidos](#problemas-conhecidos)
- [Boas Práticas para Desenvolvimento](#boas-práticas-para-desenvolvimento)
- [Referência Rápida da API do Tour](#referência-rápida-da-api-do-tour)

---

## Visão Geral

O Agilium Manager utiliza um sistema de **tour guiado interativo** para ajudar o usuário a entender os elementos de cada tela. Ao clicar no ícone de interrogação (`?`), um tour passo-a-passo é iniciado, destacando elementos-chave da interface (botões, filtros, grids) com balões explicativos.

**Estatísticas atuais:**
- **287** views com botão `btnAjuda`
- **69** arquivos JavaScript com manipuladores de clique
- **2** aplicações: `agilium-manager-azure-web` e `agilum.mvc.web`

---

## Arquitetura de 3 Camadas

```
┌─────────────────────────────────────────────────┐
│  1. HTML (Razor View)                           │
│     <a href="#" id="btnAjuda">                  │
│     └─ Ícone "?" (Font Awesome) na barra        │
├─────────────────────────────────────────────────┤
│  2. JS Handler (wwwroot/local/**/*.js)          │
│     $('#btnAjuda').click(function() {           │
│         Tour.run([...])                         │
│     })                                          │
│     └─ Define os elementos e textos do tour     │
├─────────────────────────────────────────────────┤
│  3. Biblioteca (dknotus-tour.js v1.2)           │
│     └─ Engine de renderização de popovers       │
│     └─ Navegação: Previous / Next / Close       │
│     └─ Spotlight com overlay escuro             │
└─────────────────────────────────────────────────┘
```

---

## 1. O Botão HTML (`btnAjuda`)

### Marcação Padrão

O botão está presente em praticamente todas as views do sistema. Existem duas variações:

**Variação A — Com `type="button"` (em formulários/telas de Create/Edit):**
```html
<a href="#" type="button" title="Precisa de Ajuda?" id="btnAjuda">
    <span class="fa fa-question sns-tool-action"></span>
</a>
```

**Variação B — Sem `type` (em telas de listagem/Index):**
```html
<a href="#" title="Precisa de Ajuda?" id="btnAjuda">
    <span class="fa fa-question sns-tool-action"></span>
</a>
```

### Localização na Página

O botão sempre aparece na **barra de botões superior** (`barra-de-botoes-menu-principal`), tipicamente ao lado de:
- `#btnNovoCadastro` — ícone de adicionar (`fa-plus-square`)
- `#btnSalvar` — ícone de salvar (`fa-save`) (em formulários)
- `#btnReturn` — botão voltar

### Exceção

No arquivo `Views/Shared/ExibirImagemUsuario.cshtml`, o `btnAjuda` é usado com finalidade diferente — editar foto do usuário:
```html
<a href="#" type="button" title="Editar foto?" id="btnAjuda">
    <span class="fal fa-camera sns-tool-action"></span>
</a>
```

---

## 2. O Manipulador JavaScript

### Estrutura Padrão

Cada view que oferece ajuda carrega um arquivo JS local via `@section Scripts`:

```html
@section Scripts {
    <partial name="_ValidationScriptsPartial" />
    <script src="~/local/cadastros/principal/produto.js"></script>
}
```

### Padrão do Tour

```javascript
$(function () {
    $('#btnAjuda').click(function () {
        Tour.run([
            {
                element: $('#btnNovoCadastro'),
                content: '<strong><div align="center" class="text-info">Botão adicionar</div></strong><p><div align="center">Incluir novo registro.</div></p>',
                position: 'top'
            },
            {
                element: $('#breadcrumb'),
                content: '<strong><div align="center" class="text-info">Breadcrumb</div></strong><p><div align="center">Area de Breadcrumb para navegação.</div></p>',
                position: 'top'
            },
            {
                element: $('#areaFiltro'),
                content: '<strong><div align="center" class="text-info">Filtro</div></strong><p><div align="center">Area de filtro para lista.</div></p>',
                position: 'left'
            },
            {
                element: $('#search-btn'),
                content: '<strong><div align="center" class="text-info">Botão de filtro</div></strong><p><div align="center">Faz a pesquisa de acordo com o filtro.</div></p>',
                position: 'bottom'
            },
            {
                element: $('#divGridResultado'),
                content: '<strong><div align="center" class="text-info">Resultado</div></strong><p><div align="center">Retorna os dados da consulta.</div></p>',
                position: 'top'
            },
        ]);
    });
});
```

### Elementos Comuns Mapeados nos Tours

| ID do Elemento | Descrição | Presente em |
|----------------|-----------|-------------|
| `#btnNovoCadastro` | Botão de adicionar novo registro | Index, Listas |
| `#btnSalvar` | Botão de salvar formulário | Create/Edit |
| `#btnReturn` | Botão voltar | Create/Edit |
| `#breadcrumb` | Navegação breadcrumb | Praticamente todas |
| `#areaFiltro` | Área de filtro | Index, Listas |
| `#search-btn` | Botão de executar pesquisa | Index, Listas |
| `#divGridResultado` | Grid/tabela de resultados | Index, Listas |

---

## 3. A Biblioteca `dknotus-tour.js`

### Origem

- **Nome:** DK Notus Tour JavaScript Library
- **Versão:** 1.2
- **Licença:** MIT
- **Data:** 2018-03-17
- **GitHub:** https://github.com/DKNotusIT/DKNotus-Tour/
- **Arquivos:** `wwwroot/js/dknotus-tour.js` (dev) e `wwwroot/js/dknotus-tour.min.js` (produção)

### Carregamento

A biblioteca é carregada no **layout principal** (`_main.cshtml`) e em algumas páginas individuais:

| Local | Arquivo |
|-------|---------|
| Layout principal | `Views/Shared/_main.cshtml` (linha ~763) |
| Layout alternativo | **NÃO** incluso em `Views/Shared/_Layout.cshtml` |
| Páginas avulsas | `Config/Index`, `ControleAcesso/*`, `Empresa/Index`, `Identidade/*`, `Usuario/*` |

**Atenção:** Views que usam `_Layout.cshtml` precisam incluir manualmente `<script src="~/js/dknotus-tour.min.js"></script>`, caso contrário o `btnAjuda` existirá mas o clique lançará erro `Tour is not defined`.

### API do Objeto `Tour`

#### `Tour.run(tourArray, options)`

Inicia o tour guiado. Filtra automaticamente steps cujo `element` não existe no DOM.

**Parâmetro `tourArray`:** Array de objetos, cada um representando um passo.

**Propriedades de cada step:**

| Propriedade | Tipo | Padrão | Descrição |
|-------------|------|--------|-----------|
| `element` | jQuery | **obrigatório** | Elemento(s) alvo do balão |
| `content` | string | `''` | Conteúdo HTML do balão |
| `position` | string | `'right'` | Posição: `'top'`, `'right'`, `'left'`, `'bottom'` |
| `spotlight` | bool | `true` | Ativa overlay escuro com destaque no elemento |
| `scroll` | bool | `true` | Rolagem automática para o step |
| `padding` | number | `5` | Padding em px do spotlight ao redor do elemento |
| `close` | bool | `true` | Exibe botão X para fechar |
| `language` | string | `'en'` | Idioma (suporta 27 idiomas) |
| `onstep` | function | `null` | Callback executado ao exibir o step |
| `forceCorrectionLeft` | number | `0` | Correção de posição horizontal (px) |
| `forceCorrectionTop` | number | `0` | Correção de posição vertical (px) |
| `forceCorrectionWidth` | number | `0` | Correção de largura (px) |
| `forceCorrectionHeight` | number | `0` | Correção de altura (px) |

**Parâmetro `options`:** Opções globais que servem como fallback para steps que não definem a propriedade.

#### `Tour.next()`

Avança para o próximo step.

#### `Tour.prev()`

Retrocede para o step anterior.

#### `Tour.close()`

Fecha o tour e remove todos os elementos do DOM.

#### Callbacks Globais

| Callback | Descrição |
|----------|-----------|
| `Tour.onstart` | Executado ao iniciar o tour |
| `Tour.onstep` | Executado a cada step (pode ser sobrescrito por step) |
| `Tour.onresize` | Executado no redimensionamento da janela |

### Comportamento Interno

1. Ao iniciar, filtra steps cujo `element` existe no DOM
2. Cria `<div class="tourStep popover">` com:
   - Seta direcional (`.arrow`)
   - Conteúdo HTML (`.panel-body`)
   - Rodapé com Previous / Next / "Step X de N"
   - Botão fechar (`.tourClose`)
3. Se `spotlight: true`, cria 4 `<div class="tourBg">` como overlay escuro (z-index: 1000, opacidade: 0.3) ao redor do elemento alvo
4. Posiciona o popover absolutamente baseado no offset do elemento alvo
5. Se `scroll: true`, anima a rolagem para centralizar o step

### Idiomas Suportados

pl, en, be, ca, cs, da, de, el, es, et, fi, fr, hu, it, lt, lv, mk, nl, no, pt, ru, sk, sl, sq, sv, tr, uk

O idioma padrão usado no sistema é **inglês (en)** — todos os tours usam o padrão, sem sobrescrever `language`.

---

## Como Adicionar Ajuda a uma Nova Página

### Passo 1: Adicionar o botão na View (.cshtml)

Na barra de botões superior, adicione:

```html
<a href="#" title="Precisa de Ajuda?" id="btnAjuda">
    <span class="fa fa-question sns-tool-action"></span>
</a>
```

### Passo 2: Garantir que o dknotus-tour está carregado

Se a view usa `_main.cshtml`, a biblioteca já está incluída. Caso use `_Layout.cshtml`, adicione na `@section Scripts`:

```html
<script src="~/js/dknotus-tour.min.js"></script>
```

### Passo 3: Criar/editar o arquivo JS local

Crie ou edite o arquivo JS correspondente em `wwwroot/local/...` e adicione:

```javascript
$(function () {
    $('#btnAjuda').click(function () {
        Tour.run([
            {
                element: $('#btnNovoCadastro'),
                content: '<strong><div align="center" class="text-info">Título</div></strong><p><div align="center">Descrição do elemento.</div></p>',
                position: 'top'
            },
            // ... mais steps
        ]);
    });
});
```

### Passo 4: Referenciar o JS na View

```html
@section Scripts {
    <script src="~/local/caminho/para/seu-arquivo.js"></script>
}
```

---

## Checklist de Criação de Tour

- [ ] O botão `btnAjuda` existe na view
- [ ] A biblioteca `dknotus-tour.min.js` está carregada (via layout ou manualmente)
- [ ] O arquivo JS local existe e está referenciado na `@section Scripts`
- [ ] Cada step referencia um `element` que existe no DOM com `id` único
- [ ] O conteúdo de cada step é descritivo e útil para o usuário
- [ ] As posições (`top`, `right`, `left`, `bottom`) são adequadas ao layout
- [ ] Testou o tour completo (Previous, Next, Close, spotlight)
- [ ] O tour não duplica steps genéricos sem necessidade

---

## Distribuição por Módulo

| Módulo | Views com btnAjuda | Arquivos JS | Localização dos JS |
|--------|-------------------|-------------|---------------------|
| **Cadastros/Auxiliar** | ~30 | 7 | `local/cadastros/auxiliar/` |
| **Cadastros/Principal** | ~50 | 8 | `local/cadastros/principal/` |
| **Cadastros/PontoVenda** | ~15 | 3 | `local/cadastros/pontoVenda/` |
| **Financeiro** | ~25 | 5 | `local/Financeiro/` |
| **Processos** | ~40 | 7 | `local/Processo/` |
| **Configurações** | ~40 | 6 | `local/`, `local/usuario/` |
| **SiteMercado** | ~6 | 1 | `local/cadastros/principal/` |
| **Ferramentas** | ~1 | 1 | `local/Ferramentas/` |

### Lista completa de arquivos JS com Tour

**Azure Web:**
```
wwwroot/local/cadastros/auxiliar/DepartamentoProduto.js
wwwroot/local/cadastros/auxiliar/Estoque.js
wwwroot/local/cadastros/auxiliar/GrupoProduto.js
wwwroot/local/cadastros/auxiliar/motivoDevolucao.js
wwwroot/local/cadastros/auxiliar/ProdutoMarca.js
wwwroot/local/cadastros/auxiliar/SubGrupo.js
wwwroot/local/cadastros/auxiliar/unidade.js
wwwroot/local/cadastros/pontoVenda/formaPagamento.js
wwwroot/local/cadastros/pontoVenda/moeda.js
wwwroot/local/cadastros/pontoVenda/pontoVenda.js
wwwroot/local/cadastros/principal/cliente.js
wwwroot/local/cadastros/principal/Fornecedor.js
wwwroot/local/cadastros/principal/funcionario.js
wwwroot/local/cadastros/principal/produto.js
wwwroot/local/cadastros/principal/produtoClientePreco.js
wwwroot/local/cadastros/principal/produtoFoto.js
wwwroot/local/cadastros/principal/produtoTurnoPreco.js
wwwroot/local/cadastros/principal/siteMercado.js
wwwroot/local/empresa.js
wwwroot/local/empresaCreate.js
wwwroot/local/Ferramentas/log.js
wwwroot/local/Financeiro/caixaMoeda.js
wwwroot/local/Financeiro/CategFinanc.js
wwwroot/local/Financeiro/contaPagar.js
wwwroot/local/Financeiro/contaReceber.js
wwwroot/local/Financeiro/notaFiscalInutil.js
wwwroot/local/Financeiro/planoConta.js
wwwroot/local/Processo/caixa.js
wwwroot/local/Processo/devolucao.js
wwwroot/local/Processo/inventario.js
wwwroot/local/Processo/perda.js
wwwroot/local/Processo/turno.js
wwwroot/local/Processo/vale.js
wwwroot/local/Processo/venda.js
```

**MVC Web:** (espelho em `agilum.mvc.web/wwwroot/local/`)

---

## Fluxo de Execução

```
Usuário clica no ícone "?"
        │
        ▼
$('#btnAjuda').click() dispara
        │
        ▼
Tour.run([ {element, content, position}, ... ])
        │
        ├─ Filtra steps com elemento existente no DOM
        │
        ▼
step(0) — primeiro step
        │
        ├─ Cria <div class="tourStep popover">
        ├─ Cria <div class="tourBg"> (x4, overlay spotlight)
        ├─ Posiciona popover relativo ao elemento alvo
        ├─ Scroll animado (se scroll: true)
        └─ Dispara onstep callback
        │
        ▼
Usuário clica "Next" ──► step(cur + 1)
Usuário clica "Previous" ──► step(cur - 1)
Usuário clica "X" ──► Tour.close()
Usuário clica "Finish" ──► Tour.close()
        │
        ▼
Tour.close(): remove .tourStep e .tourBg do DOM
```

---

## Problemas Conhecidos

### 1. ID duplicado `btnAjuda`

O `id="btnAjuda"` é repetido em todas as views. Como cada página carrega seu próprio JS, funciona na prática, mas viola a especificação HTML (IDs devem ser únicos no documento).

**Risco:** Se dois JS diferentes forem carregados na mesma página (ex.: partial + view pai), o segundo handler sobrescreve o primeiro.

### 2. Conteúdo genérico (copy-paste)

Muitos tours têm textos idênticos, indicando que foram copiados sem personalização. Exemplo: "Botão adicionar — Incluir novo registro" aparece em dezenas de tours.

### 3. Biblioteca legada e sem manutenção

`dknotus-tour.js` é de 2018, sem atualizações. Não há garantia de compatibilidade com versões futuras do jQuery ou Bootstrap.

### 4. Carregamento inconsistente

Views que usam `_Layout.cshtml` (em vez de `_main.cshtml`) **não** têm o `dknotus-tour.min.js` carregado automaticamente. Nestes casos, o botão `btnAjuda` existe mas o clique falha com `Uncaught ReferenceError: Tour is not defined`.

**Views afetadas conhecidas:** Nenhuma mapeada atualmente, mas o `_Layout.cshtml` não referencia o tour.

### 5. Sem fallback visual

Se um `element` não existe no DOM, o step é silenciosamente ignorado (`if (v.element && !!v.element.length)`). O tour simplesmente pula para o próximo step, sem alertar o usuário.

### 6. Sem CSS customizado

Não existe arquivo CSS específico para o tour. O estilo depende inteiramente das classes Bootstrap (`.popover`, `.panel-default`, etc.) e estilos inline aplicados pela biblioteca.

---

## Boas Práticas para Desenvolvimento

### Ao criar um novo tour:

1. **Personalize os textos** — não faça copy-paste de outras telas
2. **Mapeie apenas elementos relevantes** — nem todo botão precisa de step
3. **Use posições adequadas** — `top`/`bottom` para botões, `right`/`left` para sidebars
4. **Teste com spotlight** — garanta que o overlay não cubra elementos importantes
5. **Considere responsividade** — tours podem quebrar em telas menores

### Ao modificar uma view existente:

1. **Se adicionar/remover elementos**, atualize o tour correspondente
2. **Se renomear um `id`**, atualize o `element` no step do tour
3. **Se mudar de layout** (`_main` → `_Layout`), garanta que o `dknotus-tour` está carregado

### Padrão recomendado para o futuro:

```javascript
// Prefira encapsular o tour em uma função nomeada
function iniciarTourProduto() {
    Tour.run([
        {
            element: $('#btnNovoCadastro'),
            content: '...',
            position: 'top'
        },
    ]);
}

$(function () {
    $('#btnAjuda').click(iniciarTourProduto);
});
```

---

## Referência Rápida da API do Tour

```javascript
// Iniciar tour
Tour.run([
    {
        element: $('#meuElemento'),   // jQuery object (obrigatório)
        content: '<b>Título</b><p>Descrição</p>',
        position: 'top',              // 'top' | 'right' | 'left' | 'bottom'
        spotlight: true,              // overlay escuro
        scroll: true,                 // rolagem automática
        padding: 5,                   // padding do spotlight
        close: true,                  // botão fechar
        language: 'en',               // idioma (padrão: 'en')
        onstep: function(step) {},    // callback
        forceCorrectionLeft: 0,       // correções de posição
        forceCorrectionTop: 0,
        forceCorrectionWidth: 0,
        forceCorrectionHeight: 0
    },
    // ... mais steps
]);

// Navegação programática
Tour.next();   // próximo step
Tour.prev();   // step anterior
Tour.close();  // fechar tour

// Callbacks globais
Tour.onstart = function() { /* ao iniciar */ };
Tour.onstep = function(step) { /* a cada step */ };
Tour.onresize = function() { /* ao redimensionar */ };
```

---

## Documentação Relacionada

- `docs/frontend/javascript.md` — Arquitetura JavaScript geral
- `knowledge/frontend/javascript.md` — Organização dos scripts
- `knowledge/frontend/components.md` — Componentes reutilizáveis
