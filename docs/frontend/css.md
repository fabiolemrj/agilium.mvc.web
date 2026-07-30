# Arquitetura CSS

# Objetivo

Documentar a arquitetura de estilos do Agilium Manager, descrevendo os frameworks utilizados, a organização dos arquivos CSS, convenções de desenvolvimento e diretrizes para evolução da interface.

---

# Escopo

Este documento contempla:

- Arquitetura CSS
- Frameworks
- Organização dos Arquivos
- Estilos Customizados
- Responsividade
- Boas Práticas
- Convenções

---

# Índice

- Visão Geral
- Arquitetura CSS
- Frameworks Utilizados
- Organização dos Arquivos
- Estilos Customizados
- Responsividade
- Convenções
- Boas Práticas
- Estrutura dos Assets
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

A camada de apresentação do Agilium Manager utiliza como base o template **AdminLTE 3.x**, construído sobre o **Bootstrap 4**, complementado por folhas de estilo customizadas para atender às necessidades específicas da aplicação.

Os estilos são organizados dentro da pasta `wwwroot`, juntamente com bibliotecas de terceiros e demais recursos estáticos. :contentReference[oaicite:1]{index=1}

---

# Arquitetura CSS

A estrutura visual da aplicação segue a arquitetura:

```text
Views Razor

↓

Layout (_main.cshtml)

↓

AdminLTE

↓

Bootstrap

↓

CSS Customizado

↓

Componentes da Interface
```

Essa abordagem permite reutilização dos componentes visuais e centralização das customizações.

---

# Frameworks Utilizados

A interface utiliza os seguintes frameworks e bibliotecas:

| Framework | Finalidade |
|------------|------------|
| AdminLTE 3.x | Template administrativo |
| Bootstrap 4 | Sistema de grid, componentes e responsividade |
| Toastr | Estilos para notificações |
| DataTables | Estilização de tabelas interativas |
| Select2 | Componentes avançados de seleção |
| Inputmask | Campos com máscara |
| Chart.js | Componentes gráficos (integração visual) |

Essas bibliotecas compõem a base visual da aplicação. :contentReference[oaicite:2]{index=2}

---

# Organização dos Arquivos

Os recursos estáticos estão organizados em `wwwroot`.

Exemplo:

```text
wwwroot/
│
├── css/
│   ├── site.css
│   └── toastr.css
│
├── dist/
│   └── AdminLTE
│
├── js/
│
├── lib/
│
├── Images/
│
├── imagens-cardapio/
│
├── font/
│
└── favicon.ico
```

A organização favorece a separação entre bibliotecas de terceiros e estilos específicos da aplicação. :contentReference[oaicite:3]{index=3}

---

# Estilos Customizados

Os estilos próprios da aplicação concentram-se principalmente em:

## site.css

Responsável pelas customizações da interface sobre o AdminLTE e Bootstrap.

Deve conter:

- ajustes de layout;
- estilos específicos da aplicação;
- customizações de componentes;
- adequações visuais.

---

## toastr.css

Responsável pela apresentação das notificações exibidas pela biblioteca Toastr.

---

# Responsividade

A responsividade da aplicação é baseada no sistema de grid do Bootstrap 4.

Os layouts devem:

- adaptar-se a diferentes resoluções;
- reutilizar componentes responsivos do Bootstrap;
- evitar estilos fixos sempre que possível;
- preservar a usabilidade em diferentes tamanhos de tela.

---

# Convenções

Os estilos devem seguir as seguintes diretrizes:

- reutilizar classes do Bootstrap sempre que possível;
- utilizar classes customizadas apenas quando necessário;
- evitar sobrescrever diretamente estilos do AdminLTE;
- centralizar customizações em arquivos próprios;
- manter nomenclatura consistente.

---

# Boas Práticas

Sempre:

- reutilizar componentes do Bootstrap;
- manter estilos organizados por responsabilidade;
- documentar customizações significativas;
- remover estilos não utilizados;
- evitar duplicação de regras CSS.

Evitar:

- estilos inline;
- uso excessivo de `!important`;
- sobrescrever arquivos da biblioteca AdminLTE;
- duplicação de regras existentes no Bootstrap.

---

# Estrutura dos Assets

A camada visual é composta pelos seguintes recursos:

| Pasta | Responsabilidade |
|--------|------------------|
| css | Estilos customizados |
| dist | Recursos do AdminLTE |
| js | Scripts da aplicação |
| lib | Bibliotecas externas |
| Images | Imagens do sistema |
| imagens-cardapio | Imagens do módulo de cardápio |
| font | Fontes utilizadas |

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização do AdminLTE 3.x;
- Bootstrap 4;
- estrutura de assets em `wwwroot`;
- utilização de `site.css`;
- utilização de `toastr.css`.

Ainda deverão ser documentados em maior detalhe:

- organização interna de `site.css`;
- convenções de nomenclatura CSS;
- possíveis temas personalizados;
- padrões de reutilização de estilos entre módulos.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- adoção de novo framework CSS;
- alteração do template administrativo;
- reorganização da estrutura de assets;
- criação de novos estilos globais;
- atualização do Bootstrap ou AdminLTE.

---

# Documentação Relacionada

## Interface

- ui/layouts.md
- ui/components.md
- ui/razor.md
- ui/mvc.md

## Front-end

- ui/javascript.md
- ui/assets.md

## Arquitetura

- architecture/overview.md