# Componentes de Interface

# Objetivo

Documentar a arquitetura, organização e reutilização dos componentes de interface do Agilium Manager.

Este documento estabelece as diretrizes para criação, organização e utilização de componentes reutilizáveis da camada de apresentação.

---

# Escopo

Este documento contempla:

- Arquitetura de Componentes
- Organização da Interface
- Componentes Reutilizáveis
- Partial Views
- View Components
- Tag Helpers
- Convenções
- Catálogo de Componentes

---

# Índice

- Visão Geral
- Estratégia de Componentização
- Organização dos Componentes
- Partial Views
- View Components
- Tag Helpers
- Catálogo de Componentes
- Convenções
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

A camada de apresentação deve priorizar a reutilização de componentes de interface para reduzir duplicação, facilitar manutenção e promover consistência visual e funcional.

Sempre que possível, elementos compartilhados devem ser implementados como componentes reutilizáveis.

---

# Estratégia de Componentização

A reutilização pode ocorrer por diferentes mecanismos da plataforma MVC.

Conceitualmente:

```text
Layout

↓

Página Razor

↓

Componentes Compartilhados

↓

Elementos Visuais
```

A estratégia efetivamente adotada deverá refletir a implementação existente na solução.

---

# Organização dos Componentes

Os componentes deverão ser organizados por responsabilidade funcional.

Exemplo:

```text
Views/

Shared/

Components/

Layouts/

Partials/
```

A estrutura definitiva deverá refletir a organização real do projeto.

---

# Partial Views

As Partial Views são recomendadas para reutilização de trechos de interface que:

- não possuem lógica complexa;
- dependem apenas do modelo recebido;
- são utilizadas por múltiplas páginas.

Cada Partial View deverá documentar:

- objetivo;
- parâmetros esperados;
- páginas que a utilizam;
- dependências.

---

# View Components

Quando utilizados, os View Components devem encapsular componentes que possuam lógica própria de obtenção ou preparação de dados.

Cada componente deverá documentar:

- finalidade;
- parâmetros de entrada;
- resultado produzido;
- dependências;
- localização do código.

---

# Tag Helpers

Quando existirem Tag Helpers personalizados, cada um deverá possuir documentação contendo:

- objetivo;
- atributos suportados;
- exemplos de utilização;
- restrições;
- dependências.

---

# Catálogo de Componentes

O catálogo deverá ser mantido atualizado durante a evolução da aplicação.

Exemplo:

| Componente | Tipo | Finalidade | Localização |
|------------|------|------------|-------------|
| Menu Principal | Partial View | Navegação principal | Views/Shared |
| Cabeçalho | Partial View | Cabeçalho padrão | Views/Shared |
| Rodapé | Partial View | Rodapé padrão | Views/Shared |

*A tabela acima representa apenas um modelo de organização.*

---

# Convenções

Todo componente reutilizável deve:

- possuir responsabilidade única;
- ser reutilizável;
- possuir documentação quando relevante;
- evitar dependências desnecessárias;
- manter consistência visual com a aplicação.

---

# Boas Práticas

Sempre:

- reutilizar componentes existentes antes de criar novos;
- manter componentes pequenos e especializados;
- separar lógica de apresentação da lógica de negócio;
- documentar componentes compartilhados;
- revisar impactos antes de alterar componentes amplamente utilizados.

Evitar:

- duplicação de interface;
- lógica de negócio em componentes visuais;
- componentes excessivamente grandes;
- dependências circulares entre componentes.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de ASP.NET Core MVC;
- utilização de Razor Views;
- existência de uma camada de apresentação estruturada.

Ainda deverão ser confirmados durante a análise do projeto `agilium.mvc.web` e dos demais componentes da solução:

- catálogo completo de Partial Views;
- utilização de View Components;
- existência de Tag Helpers personalizados;
- organização definitiva dos componentes reutilizáveis;
- padrões específicos de reutilização da interface.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- criação de novo componente reutilizável;
- alteração da arquitetura da interface;
- inclusão de novos padrões de reutilização;
- reorganização da estrutura das Views.

---

# Documentação Relacionada

## Interface

- ui/mvc.md
- ui/razor.md
- ui/layouts.md

## Arquitetura

- architecture/overview.md
- architecture/layers.md

## Desenvolvimento

- development/coding-standards.md