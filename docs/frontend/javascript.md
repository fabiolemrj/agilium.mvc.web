# Arquitetura JavaScript

# Objetivo

Documentar a arquitetura JavaScript utilizada no Agilium Manager, descrevendo as bibliotecas empregadas, a organização dos scripts, os padrões de comunicação entre cliente e servidor e as convenções adotadas para desenvolvimento front-end.

---

# Escopo

Este documento contempla:

- Arquitetura JavaScript
- Bibliotecas Utilizadas
- Organização dos Scripts
- Comunicação Cliente-Servidor
- AJAX
- Convenções
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura JavaScript
- Bibliotecas Utilizadas
- Organização dos Scripts
- Comunicação Cliente-Servidor
- Padrões AJAX
- Convenções
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

A camada JavaScript do Agilium Manager complementa a renderização realizada pelo ASP.NET Core MVC, adicionando recursos de interação, validação, comunicação assíncrona e componentes ricos de interface.

A maior parte da lógica de apresentação permanece nas Views Razor, enquanto o JavaScript é utilizado para comportamentos dinâmicos da interface.

---

# Arquitetura JavaScript

A arquitetura da interface segue o fluxo:

```text
Razor View

↓

HTML

↓

Bootstrap

↓

AdminLTE

↓

JavaScript

↓

jQuery

↓

AJAX

↓

Controller MVC
```

Essa arquitetura privilegia renderização no servidor, utilizando JavaScript para enriquecer a experiência do usuário.

---

# Bibliotecas Utilizadas

O levantamento identificou as seguintes bibliotecas:

| Biblioteca | Finalidade |
|------------|------------|
| jQuery | Manipulação do DOM e AJAX |
| Bootstrap 4 | Componentes da interface |
| AdminLTE 3.x | Template administrativo |
| DataTables | Tabelas interativas |
| Select2 | Combos avançados |
| Toastr | Notificações |
| Inputmask | Máscaras de entrada |
| Chart.js | Gráficos |
| DK Notus Tour (v1.2) | Tour guiado / sistema de ajuda (`btnAjuda`) |

Essas bibliotecas compõem a base do frontend da aplicação. :contentReference[oaicite:1]{index=1}

---

# Organização dos Scripts

Os scripts da aplicação encontram-se na estrutura de recursos estáticos.

Exemplo:

```text
wwwroot/

js/

lib/

dist/

css/
```

Os arquivos JavaScript próprios da aplicação devem permanecer separados das bibliotecas de terceiros.

---

# Comunicação Cliente-Servidor

A comunicação da interface ocorre principalmente entre:

```text
View Razor

↓

JavaScript

↓

AJAX

↓

Controller MVC

↓

Business Service

↓

Repository

↓

Banco de Dados
```

O Controller permanece como ponto central de entrada das requisições da interface.

---

# Padrões AJAX

Sempre que possível, chamadas assíncronas devem:

- utilizar endpoints MVC apropriados;
- tratar erros retornados pelo servidor;
- atualizar apenas os elementos necessários da interface;
- evitar recarga completa da página quando não necessária.

As respostas devem seguir o padrão definido pelos Controllers da aplicação.

---

# Convenções

Os scripts devem seguir as seguintes diretrizes:

- organizar código por funcionalidade;
- evitar scripts excessivamente grandes;
- separar lógica de negócio da lógica de interface;
- utilizar nomes descritivos para funções;
- minimizar dependências globais.

---

# Boas Práticas

Sempre:

- reutilizar funções existentes;
- manter scripts organizados;
- documentar comportamentos complexos;
- utilizar bibliotecas já adotadas pela solução;
- tratar erros de chamadas AJAX.

Evitar:

- JavaScript inline nas Views;
- duplicação de funções;
- manipulação excessiva do DOM;
- dependência entre scripts não documentada;
- criação de variáveis globais desnecessárias.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de jQuery;
- utilização de AJAX;
- AdminLTE 3.x;
- Bootstrap 4;
- DataTables;
- Select2;
- Toastr;
- Inputmask;
- Chart.js;
- existência da pasta `wwwroot/js`.

Ainda deverão ser documentados em maior detalhe:

- organização interna dos scripts customizados;
- padrões específicos de nomenclatura;
- módulos JavaScript reutilizáveis;
- convenções para eventos e inicialização de componentes.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- adoção de novas bibliotecas JavaScript;
- alteração da arquitetura da interface;
- reorganização dos scripts;
- inclusão de novos padrões de comunicação cliente-servidor.

---

# Documentação Relacionada

## Interface

- ui/mvc.md
- ui/components.md
- ui/layouts.md
- ui/css.md- [tour-ajuda.md](tour-ajuda.md) — Sistema de ajuda guiada (Tour / btnAjuda)
## Arquitetura

- architecture/overview.md
- architecture/layers.md

## Desenvolvimento

- development/coding-standards.md