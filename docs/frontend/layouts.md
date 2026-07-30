# Layouts

# Objetivo

Documentar a arquitetura de layouts do Agilium Manager MVC, descrevendo a organização das páginas, layouts compartilhados, componentes reutilizáveis e o fluxo de composição das Views Razor.

Este documento estabelece as diretrizes para construção e manutenção da interface da aplicação.

---

# Escopo

Este documento contempla:

- Arquitetura dos Layouts
- Layout Principal
- Layouts Alternativos
- Shared Views
- Partial Views
- View Components
- Sections Razor
- Organização da Interface

---

# Índice

- Visão Geral
- Arquitetura dos Layouts
- Layout Principal
- Layouts Compartilhados
- Componentes Compartilhados
- Sections
- Organização das Views
- Convenções
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

A interface do Agilium Manager é construída utilizando **ASP.NET Core MVC** e **Razor Views**, tendo como base o template **AdminLTE 3.x** sobre **Bootstrap 4**.

A composição visual é centralizada em layouts compartilhados localizados em `Views/Shared`, promovendo padronização entre todas as páginas da aplicação. :contentReference[oaicite:1]{index=1}

---

# Arquitetura dos Layouts

A composição das páginas segue o fluxo:

```text
_ViewStart.cshtml

↓

_main.cshtml

↓

Partial Views

↓

View Components

↓

View Razor

↓

JavaScript

↓

CSS
```

A responsabilidade de cada camada é claramente separada para facilitar reutilização e manutenção.

---

# Layout Principal

O layout principal da aplicação é:

```text
Views/
└── Shared/
    └── _main.cshtml
```

Este layout concentra a estrutura comum das páginas, incluindo:

- cabeçalho;
- menu lateral;
- área principal de conteúdo;
- rodapé;
- carregamento de estilos;
- carregamento de scripts globais.

O `_main.cshtml` utiliza o template **AdminLTE 3.x** como base da interface. :contentReference[oaicite:2]{index=2}

---

# Layouts Compartilhados

Além do layout principal, o projeto possui:

```text
Views/
└── Shared/
    ├── _Layout.cshtml
    └── _main.cshtml
```

Cada layout deve possuir uma responsabilidade claramente definida.

A utilização de layouts específicos deve ocorrer apenas quando houver necessidade funcional.

---

# Componentes Compartilhados

Os seguintes componentes compartilhados foram identificados:

| Componente | Finalidade |
|------------|------------|
| `_ASideMenu.cshtml` | Menu lateral |
| `_LoginPartial.cshtml` | Informações do usuário autenticado |
| `_rodape.cshtml` | Rodapé da aplicação |
| `_ValidationScriptsPartial.cshtml` | Scripts de validação |
| `Views/Shared/Components` | View Components reutilizáveis |

Esses componentes são utilizados para reduzir duplicação e manter consistência visual. :contentReference[oaicite:3]{index=3}

---

# Sections

As Views Razor podem definir seções específicas para complementar o layout principal.

Exemplos comuns incluem:

- scripts adicionais;
- estilos específicos;
- conteúdo opcional do cabeçalho.

As Sections disponíveis devem ser mantidas consistentes entre os layouts.

---

# Organização das Views

A estrutura da camada de apresentação segue a organização:

```text
Views/

Shared/

Home/

Produto/

Cliente/

Fornecedor/

Venda/

...

_ViewImports.cshtml

_ViewStart.cshtml
```

Cada Controller possui sua própria pasta de Views.

Os layouts compartilhados permanecem centralizados em `Views/Shared`. :contentReference[oaicite:4]{index=4}

---

# Convenções

Os layouts devem seguir as seguintes diretrizes:

- concentrar apenas estrutura visual comum;
- reutilizar Partial Views;
- reutilizar View Components;
- evitar lógica de negócio;
- manter consistência entre módulos.

---

# Boas Práticas

Sempre:

- utilizar o layout principal da aplicação;
- reutilizar componentes existentes;
- manter layouts simples;
- concentrar scripts globais no layout;
- manter organização em `Views/Shared`.

Evitar:

- duplicação de layouts;
- lógica de negócio nas Views;
- código JavaScript excessivo diretamente nos layouts;
- criação de layouts específicos sem necessidade.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de `_main.cshtml` como layout principal;
- existência de `_Layout.cshtml`;
- utilização de `_ViewStart.cshtml`;
- utilização de `_ViewImports.cshtml`;
- utilização de Partial Views compartilhadas;
- existência de `Views/Shared/Components`;
- utilização de AdminLTE 3.x;
- Bootstrap 4.

Ainda deverão ser documentados em maior detalhe:

- todas as Sections disponíveis;
- layouts específicos da área Identity;
- catálogo completo de View Components;
- fluxo completo de composição das páginas.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- criação de novos layouts;
- alteração da estrutura visual;
- inclusão de novos componentes compartilhados;
- atualização do template AdminLTE;
- reorganização da camada de apresentação.

---

# Documentação Relacionada

## Interface

- ui/mvc.md
- ui/razor.md
- ui/components.md
- ui/css.md
- ui/javascript.md

## Arquitetura

- architecture/overview.md
- architecture/layers.md

## Desenvolvimento

- development/coding-standards.md