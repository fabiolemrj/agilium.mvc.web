# Architecture Decision Records (ADR)

## Objetivo

Documentar todas as decisões arquiteturais relevantes do ecossistema Agilium Manager, preservando o contexto, as alternativas avaliadas, a decisão adotada e seus impactos na arquitetura.

Os ADRs servem como histórico técnico da evolução da plataforma e auxiliam novos desenvolvedores na compreensão das escolhas realizadas ao longo do projeto.

---

# Escopo

Este documento contempla:

- Catálogo de ADRs
- Processo de criação
- Status dos ADRs
- Estrutura dos documentos
- Convenções
- Governança
- Organização da documentação

---

# Índice

- O que é um ADR
- Quando criar um ADR
- Estrutura dos ADRs
- Status
- Catálogo
- Convenções
- Processo de Atualização
- ADRs Identificados
- Documentação Relacionada

---

# O que é um ADR

Architecture Decision Record (ADR) é um documento que registra uma decisão arquitetural importante tomada durante o desenvolvimento da plataforma.

Cada ADR responde às seguintes perguntas:

- Qual problema existia?
- Qual decisão foi tomada?
- Por que essa decisão foi escolhida?
- Quais alternativas foram avaliadas?
- Quais são as consequências?

---

# Quando criar um ADR

Um novo ADR deve ser criado sempre que houver decisões que afetem significativamente a arquitetura.

Exemplos:

- adoção de uma nova tecnologia;
- alteração do padrão arquitetural;
- mudança no mecanismo de autenticação;
- adoção de novo banco de dados;
- mudança de estratégia de versionamento;
- substituição de frameworks;
- alterações estruturais permanentes.

Não devem ser criados ADRs para decisões puramente funcionais ou correções de bugs.

---

# Estrutura dos ADRs

Cada ADR deve conter, no mínimo:

- Identificador
- Título
- Data
- Status
- Contexto
- Problema
- Alternativas Avaliadas
- Decisão
- Consequências
- Impactos
- Referências

Modelo:

```
ADR-001

Título

Status

Contexto

Problema

Alternativas

Decisão

Consequências

Referências
```

---

# Status

Os ADRs podem assumir os seguintes estados.

| Status | Descrição |
|----------|-----------|
| Proposed | Em análise |
| Accepted | Aprovado |
| Superseded | Substituído por outro ADR |
| Deprecated | Não recomendado |
| Rejected | Rejeitado |

---

# Catálogo de ADRs

A tabela abaixo representa o índice oficial das decisões arquiteturais.

| ADR | Título | Status | Área |
|------|---------|--------|------|
| ADR-001 | Arquitetura em Camadas | Identificado | Arquitetura |
| ADR-002 | Repository Pattern | Identificado | Persistência |
| ADR-003 | Service Layer | Identificado | Arquitetura |
| ADR-004 | Notification Pattern | Identificado | Negócio |
| ADR-005 | Dependency Injection | Identificado | Infraestrutura |
| ADR-006 | AutoMapper | Identificado | Infraestrutura |
| ADR-007 | Entity Framework Core | Identificado | Persistência |
| ADR-008 | Dapper para Consultas | Identificado | Persistência |
| ADR-009 | ASP.NET Core Identity | Identificado | Segurança |
| ADR-010 | Cookie Authentication | Identificado | Segurança |
| ADR-011 | Middleware Pipeline | Identificado | Infraestrutura |
| ADR-012 | Fluent API | Identificado | Persistência |
| ADR-013 | Versionamento de API | Pendente de validação | API |
| ADR-014 | Swagger / OpenAPI | Pendente de validação | API |

> O status **Identificado** indica que a decisão foi observada durante o levantamento técnico, mas ainda deve ser formalizada em um ADR individual.

---

# Convenções

Todos os ADRs devem:

- utilizar numeração sequencial;
- possuir título objetivo;
- registrar o contexto da decisão;
- documentar alternativas descartadas;
- indicar impactos técnicos;
- conter referências para documentação relacionada.

---

# Processo de Atualização

Sempre que uma decisão arquitetural for tomada:

1. Criar um novo ADR.
2. Atualizar este índice.
3. Relacionar o ADR aos documentos afetados.
4. Revisar ADRs substituídos, quando aplicável.

---

# ADRs Identificados

Com base no levantamento inicial da solução, foram identificadas as seguintes decisões arquiteturais que deverão ser documentadas individualmente:

## Arquitetura

- Layered Architecture
- Service Layer
- Repository Pattern
- Dependency Injection

## Persistência

- Entity Framework Core
- Fluent API
- Dapper
- Repository Pattern

## Segurança

- ASP.NET Core Identity
- Cookie Authentication
- Claims Authorization

## APIs

- Versionamento
- Swagger
- OpenAPI

## Infraestrutura

- Middleware Pipeline
- AutoMapper
- Notification Pattern

Cada um desses itens deverá originar um ADR específico.

---

# Organização

```
docs/
└── decisions/
    ├── README.md
    ├── adr-index.md
    ├── adr-template.md
    ├── ADR-001-layered-architecture.md
    ├── ADR-002-repository-pattern.md
    ├── ADR-003-service-layer.md
    ├── ADR-004-notification-pattern.md
    ├── ADR-005-dependency-injection.md
    ├── ADR-006-automapper.md
    ├── ADR-007-entity-framework-core.md
    ├── ADR-008-dapper.md
    ├── ADR-009-identity.md
    ├── ADR-010-cookie-authentication.md
    ├── ADR-011-middleware-pipeline.md
    ├── ADR-012-fluent-api.md
    ├── ADR-013-api-versioning.md
    └── ADR-014-swagger.md
```

---

# Documentação Relacionada

- architecture/overview.md
- architecture/patterns.md
- architecture/security.md
- api/overview.md
- database/overview.md
- decisions/adr-template.md