# Decisions

## Objetivo

Este documento fornece uma visão geral das **Architecture Decision Records (ADRs)** adotadas pelo **Agilium Manager**.

Seu objetivo é permitir que desenvolvedores e agentes de IA identifiquem rapidamente quais decisões arquiteturais devem ser consideradas antes de modificar ou implementar qualquer funcionalidade.

A documentação oficial encontra-se em:

```text
docs/decisions/
```

Este documento é um resumo. Em caso de dúvida, **sempre prevalece o conteúdo dos ADRs oficiais**.

---

# O que são ADRs?

**Architecture Decision Records (ADRs)** documentam decisões arquiteturais importantes do projeto.

Cada ADR registra:

- Contexto
- Problema
- Decisão adotada
- Justificativa
- Consequências
- Alternativas consideradas
- ADRs relacionados

Toda decisão arquitetural permanente deve possuir uma ADR correspondente.

---

# Quando consultar

Consulte os ADRs sempre que:

- Criar uma nova funcionalidade.
- Alterar a arquitetura.
- Criar novos projetos.
- Alterar persistência.
- Modificar autenticação.
- Alterar regras de negócio.
- Criar APIs.
- Refatorar código.
- Definir novos padrões.

---

# Organização

As ADRs encontram-se organizadas em:

```text
docs/

decisions/

ADR-0001-...

ADR-0002-...

...

ADR-0020-...
```

Cada documento descreve uma única decisão arquitetural.

---

# Índice das ADRs

| ADR | Tema | Área |
|------|------|------|
| ADR-0001 | Arquitetura em Camadas | Arquitetura |
| ADR-0002 | Repository Pattern | Persistência |
| ADR-0003 | Notification Pattern | Domínio |
| ADR-0004 | Entity Framework Core | Persistência |
| ADR-0005 | Estratégia de Autenticação | Segurança |
| ADR-0006 | Estratégia de Autorização | Segurança |
| ADR-0007 | Estratégia de Validação | Domínio |
| ADR-0008 | Versionamento de APIs | APIs |
| ADR-0009 | Dependency Injection | Arquitetura |
| ADR-0010 | Dapper para Consultas | Persistência |
| ADR-0011 | Service Layer | Aplicação |
| ADR-0012 | Docker e Deploy | Infraestrutura |
| ADR-0013 | Logging | Observabilidade |
| ADR-0014 | Tratamento Global de Exceções | APIs |
| ADR-0015 | Padronização das Respostas | APIs |
| ADR-0016 | Soft Delete | Persistência |
| ADR-0017 | Auditoria | Persistência |
| ADR-0018 | Configuration Management | Infraestrutura |
| ADR-0019 | Database Migrations | Persistência |
| ADR-0020 | Estratégia de Testes | Qualidade |

---

# Principais Decisões

## Arquitetura

**ADR-0001**

Define:

- Arquitetura em Camadas
- Fluxo de dependências
- Responsabilidade das camadas
- Organização da solução

Consulte antes de alterar qualquer componente estrutural.

---

## Persistência

Relacionadas à camada de dados:

- ADR-0002 — Repository Pattern
- ADR-0004 — Entity Framework Core
- ADR-0010 — Dapper
- ADR-0016 — Soft Delete
- ADR-0017 — Auditoria
- ADR-0019 — Migrations

---

## Segurança

Relacionadas ao controle de acesso:

- ADR-0005 — Autenticação
- ADR-0006 — Autorização

Consulte sempre antes de alterar:

- Login
- JWT
- Permissões
- Claims
- Controle de acesso

---

## Domínio

Relacionadas às regras de negócio:

- ADR-0003 — Notification Pattern
- ADR-0007 — Estratégia de Validação
- ADR-0011 — Service Layer

Essas ADRs definem como implementar regras de negócio e organizar os casos de uso.

---

## APIs

Relacionadas à camada de integração:

- ADR-0008 — Versionamento
- ADR-0014 — Tratamento Global de Exceções
- ADR-0015 — Padronização das Respostas

Devem ser consideradas ao criar ou alterar endpoints.

---

## Infraestrutura

Relacionadas à operação da aplicação:

- ADR-0009 — Dependency Injection
- ADR-0012 — Docker
- ADR-0013 — Logging
- ADR-0018 — Configuration Management

---

## Qualidade

Relacionada aos testes:

- ADR-0020 — Estratégia de Testes

Define:

- Pirâmide de testes
- Ferramentas
- Cobertura
- Critérios mínimos

---

# Como Utilizar

Fluxo recomendado:

```text
Receber solicitação

↓

Identificar o módulo

↓

Identificar a camada

↓

Consultar ADRs relacionados

↓

Ler documentação oficial

↓

Planejar implementação

↓

Executar alterações

↓

Executar testes

↓

Atualizar documentação
```

---

# Relação entre Documentos

| Documento | Quando consultar |
|------------|------------------|
| architecture.md | Entender a arquitetura da solução |
| api.md | Criar ou alterar APIs |
| business-rules.md | Implementar regras de negócio |
| database.md | Alterar persistência |
| development.md | Seguir padrões de desenvolvimento |
| patterns.md | Aplicar padrões arquiteturais |

---

# Antes de Implementar

Verifique:

- Existe um ADR relacionado?
- A implementação respeita a arquitetura?
- Existe impacto em outras camadas?
- A decisão arquitetural continua válida?
- Será necessário criar uma nova ADR?

---

# Quando Criar uma Nova ADR

Uma nova ADR deve ser criada quando houver:

- Novo padrão arquitetural.
- Alteração permanente na arquitetura.
- Mudança de tecnologia principal.
- Nova estratégia de autenticação.
- Nova estratégia de persistência.
- Mudança significativa na infraestrutura.
- Alteração estrutural que afete vários módulos.

---

# Regras para ADRs

- Uma decisão por ADR.
- ADRs nunca devem ser excluídas.
- ADRs substituídas devem ser marcadas como **Superseded**.
- A numeração deve ser sequencial.
- Toda ADR deve possuir contexto, decisão e consequências.

---

# Documentação Relacionada

| Assunto | Documento |
|----------|-----------|
| Arquitetura | knowledge/architecture.md |
| APIs | knowledge/api.md |
| Banco de Dados | knowledge/database.md |
| Regras de Negócio | knowledge/business-rules.md |
| Desenvolvimento | knowledge/development.md |
| Padrões | knowledge/patterns.md |

---

# Documentação Oficial

Para detalhes completos consulte:

```text
docs/decisions/
```

Cada ADR contém:

- Contexto
- Decisão
- Motivação
- Consequências
- Alternativas
- Referências
- ADRs relacionadas

---

# Fluxo Recomendado para Agentes de IA

```text
Ler decisions.md

↓

Identificar a alteração

↓

Localizar ADRs relacionadas

↓

Ler os documentos oficiais

↓

Planejar implementação

↓

Executar alterações

↓

Atualizar documentação

↓

Verificar necessidade de nova ADR
```

---

# Resumo

Este documento é um **índice resumido das decisões arquiteturais** do Agilium Manager.

Antes de qualquer implementação:

- identifique os ADRs aplicáveis;
- siga as decisões arquiteturais existentes;
- consulte a documentação oficial em `docs/decisions/`;
- proponha uma nova ADR quando uma decisão arquitetural permanente for introduzida.