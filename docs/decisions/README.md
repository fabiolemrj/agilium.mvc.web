# Architecture Decisions (ADRs)

# Objetivo

Esta pasta contém os **Architecture Decision Records (ADRs)** do Agilium Manager.

Os ADRs registram as principais decisões arquiteturais tomadas durante a evolução da plataforma, preservando seu contexto, motivação, alternativas avaliadas, justificativas e impactos.

Cada ADR representa um registro histórico da arquitetura e deve ser mantido mesmo quando a decisão for substituída ou descontinuada.

---

# Objetivos dos ADRs

Os ADRs possuem como finalidade:

- documentar decisões arquiteturais importantes;
- registrar o contexto em que cada decisão foi tomada;
- preservar o histórico de evolução da arquitetura;
- facilitar futuras manutenções;
- reduzir perda de conhecimento técnico;
- apoiar novos desenvolvedores no entendimento da solução;
- servir como referência para futuras decisões.

---

# Quando Criar um ADR

Um ADR deve ser criado sempre que houver uma decisão arquitetural relevante.

Exemplos:

- adoção de um padrão arquitetural;
- alteração da arquitetura da solução;
- mudança na estratégia de autenticação;
- alteração da estratégia de autorização;
- adoção de um novo mecanismo de persistência;
- alteração da estratégia de integração;
- definição de padrões de validação;
- adoção de uma nova tecnologia com impacto arquitetural;
- mudanças estruturais que afetem diversos módulos.

Alterações pequenas ou exclusivamente relacionadas à implementação normalmente não justificam um ADR.

---

# Organização

Cada decisão deve possuir um arquivo próprio.

Estrutura recomendada:

```text
architecture/
└── decisions/
    ├── README.md
    ├── ADR-template.md
    ├── ADR-0001-layered-architecture.md
    ├── ADR-0002-repository-pattern.md
    ├── ADR-0003-notification-pattern.md
    ├── ADR-0004-entity-framework-core.md
    ├── ADR-0005-authentication.md
    ├── ADR-0006-authorization.md
    ├── ADR-0007-validation-strategy.md
    └── ADR-0008-api-versioning.md
```

---

# Convenção de Nomenclatura

Todos os ADRs devem seguir o padrão:

```text
ADR-XXXX-descricao.md
```

Exemplos:

```text
ADR-0001-layered-architecture.md

ADR-0002-repository-pattern.md

ADR-0003-notification-pattern.md

ADR-0004-authentication.md

ADR-0005-validation-strategy.md
```

Regras:

- utilizar numeração sequencial;
- nunca reutilizar um número;
- utilizar nomes curtos;
- utilizar apenas letras minúsculas;
- separar palavras com hífen.

---

# Ciclo de Vida

Todo ADR possui um status.

| Status | Descrição |
|---------|-----------|
| Proposed | Decisão proposta, ainda em avaliação |
| Accepted | Decisão aprovada e adotada |
| Deprecated | Decisão não deve mais ser utilizada |
| Superseded | Decisão substituída por outro ADR |

Mesmo ADRs obsoletos devem permanecer na documentação para preservar o histórico arquitetural.

---

# Estrutura Recomendada

Todos os ADRs devem seguir o template oficial (`ADR-template.md`) contendo, no mínimo:

- Identificação
- Contexto
- Problema
- Objetivos
- Premissas
- Restrições
- Alternativas Avaliadas
- Decisão
- Justificativa
- Impactos Arquiteturais
- Consequências
- Compatibilidade
- Plano de Implementação (quando aplicável)
- Critérios de Aceitação
- Critérios para Revisão
- Documentação Relacionada
- Referências
- Histórico

---

# Boas Práticas

Sempre:

- registrar apenas decisões arquiteturais relevantes;
- justificar claramente a decisão adotada;
- documentar alternativas consideradas;
- registrar impactos positivos e negativos;
- manter os ADRs atualizados;
- referenciar documentos relacionados da arquitetura.

Evitar:

- utilizar ADRs para registrar tarefas de desenvolvimento;
- documentar detalhes exclusivos de implementação;
- alterar ADRs antigos sem registrar o histórico;
- remover ADRs substituídos.

---

# Processo de Criação

Sempre que uma nova decisão arquitetural for necessária:

1. Identificar o problema.
2. Levantar alternativas.
3. Avaliar impactos.
4. Escolher a solução.
5. Registrar a decisão utilizando o template oficial.
6. Revisar tecnicamente.
7. Aprovar.
8. Publicar o ADR.
9. Atualizar a documentação relacionada.

---

# Relação com a Documentação

Os ADRs complementam a documentação arquitetural da solução.

Sempre que um ADR alterar a arquitetura, os seguintes documentos devem ser revisados quando aplicável:

## Arquitetura

- architecture/overview.md
- architecture/layers.md
- architecture/dependency-flow.md
- architecture/solution-structure.md

## Padrões

- patterns/notification-pattern.md
- patterns/repository.md
- patterns/validation.md

## Persistência

- database/overview.md
- database/entities.md
- database/relationships.md
- database/mappings.md

## Segurança

- security/authentication.md
- security/authorization.md
- security/permissions.md

---

# Histórico das Decisões

A tabela abaixo deve ser mantida atualizada à medida que novos ADRs forem criados.

| ADR | Título | Status | Data |
|-----|--------|--------|------|
| ADR-0001 | Arquitetura em Camadas | Accepted | YYYY-MM-DD |
| ADR-0002 | Repository Pattern | Accepted | YYYY-MM-DD |
| ADR-0003 | Notification Pattern | Accepted | YYYY-MM-DD |
| ADR-0004 | Entity Framework Core | Proposed | YYYY-MM-DD |

---

# Atualização

Este README deve ser revisado sempre que:

- o processo de criação de ADRs for alterado;
- novos padrões de documentação forem adotados;
- houver mudança na organização da pasta `decisions`;
- o template oficial de ADR for atualizado.

---

# Documentação Relacionada

## Arquitetura

- architecture/overview.md
- architecture/layers.md
- architecture/dependency-flow.md
- architecture/solution-structure.md

## Template

- ADR-template.md

## Desenvolvimento

- development/coding-standards.md
- development/versioning.md