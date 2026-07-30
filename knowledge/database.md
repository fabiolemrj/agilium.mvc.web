# Database

## Objetivo

Este documento fornece uma visão geral da camada de persistência do **Agilium Manager**, apresentando os principais padrões, responsabilidades e convenções relacionadas ao banco de dados.

A documentação oficial encontra-se em:

```text
docs/database/
```

Este documento serve como um guia rápido para desenvolvedores e agentes de IA, indicando como a persistência está organizada e quais padrões devem ser respeitados durante o desenvolvimento.

---

# Visão Geral

A camada de banco de dados é responsável por:

- Persistência das informações.
- Integridade dos dados.
- Auditoria.
- Soft Delete.
- Versionamento do esquema.
- Performance das consultas.
- Integridade referencial.

A persistência é implementada utilizando:

- Entity Framework Core
- Dapper (consultas específicas)
- MySQL

---

# Arquitetura

Fluxo simplificado.

```text
Application Service

↓

Domain

↓

Repository

↓

Persistence

↓

Entity Framework Core

↓

Database
```

Nenhuma outra camada deve acessar diretamente o banco de dados.

---

# Responsabilidades

## Repository

Responsável por:

- Consultas
- Inclusões
- Atualizações
- Exclusões lógicas

Não implementa regras de negócio.

---

## Persistence

Responsável por:

- DbContext
- Entity Configurations
- Fluent API
- Migrations
- Configuração do banco
- Transações
- Auditoria automática

---

## Database

Responsável apenas pela persistência dos dados.

Não deve conter regras de negócio implementadas através de procedures, triggers ou funções, salvo exceções devidamente documentadas.

---

# Organização

A documentação oficial normalmente está organizada da seguinte forma:

```text
docs/database/

README.md

entities.md

relationships.md

mappings.md

indexes.md

constraints.md

migrations.md

audit.md

soft-delete.md

performance.md
```

---

# ORM

O ORM oficial do projeto é o **Entity Framework Core**.

Utilizado para:

- CRUD
- Relacionamentos
- Tracking
- Migrations
- Configurações via Fluent API

Consultar:

```text
docs/database/mappings.md
```

---

# Dapper

O Dapper é utilizado apenas para consultas específicas.

Exemplos:

- Dashboards
- Relatórios
- Consultas de alta performance
- Grandes volumes de leitura

Nunca utilizar Dapper para implementar regras de negócio.

---

# Modelagem

Toda entidade deve possuir:

- Chave primária.
- Configuração via Fluent API.
- Relacionamentos explícitos.
- Índices quando necessários.
- Convenções padronizadas.

Consultar:

```text
docs/database/entities.md
```

---

# Relacionamentos

Todos os relacionamentos devem ser configurados através do Fluent API.

Exemplos:

- One-to-One
- One-to-Many
- Many-to-Many

Evitar configurações implícitas quando houver necessidade de regras específicas.

---

# Soft Delete

O projeto utiliza **Soft Delete** como estratégia oficial de exclusão.

Campos normalmente utilizados:

```text
Ativo

DataExclusao

UsuarioExclusao
```

Nunca remover registros de negócio fisicamente.

Consultar:

```text
docs/database/soft-delete.md
```

e

```text
ADR-0016
```

---

# Auditoria

As entidades auditáveis registram automaticamente:

- DataCadastro
- UsuarioCadastro
- DataAlteracao
- UsuarioAlteracao
- DataExclusao
- UsuarioExclusao

O preenchimento ocorre automaticamente durante a persistência.

Consultar:

```text
docs/database/audit.md
```

---

# Migrations

Toda alteração estrutural do banco deve possuir uma Migration.

Fluxo recomendado:

```text
Alterar Entidade

↓

Alterar Mapping

↓

Gerar Migration

↓

Revisar

↓

Aplicar

↓

Testar
```

Nunca alterar o banco manualmente em ambientes controlados.

Consultar:

```text
docs/database/migrations.md
```

---

# Índices

Criar índices quando:

- Houver consultas frequentes.
- Existirem filtros recorrentes.
- Campos forem utilizados em ordenações.
- Chaves estrangeiras exigirem otimização.

Toda criação de índice deve ser documentada.

---

# Convenções

Toda entidade deve seguir as convenções definidas pelo projeto.

Exemplos:

- Chaves primárias padronizadas.
- Convenções de nomenclatura.
- Tipos consistentes.
- Relacionamentos explícitos.
- Configuração centralizada.

Consultar:

```text
docs/database/conventions.md
```

---

# Performance

Boas práticas:

- Evitar N+1 Queries.
- Utilizar Include apenas quando necessário.
- Utilizar projeções.
- Utilizar paginação.
- Utilizar índices.
- Utilizar Dapper apenas para consultas específicas.

Consultar:

```text
docs/database/performance.md
```

---

# Integridade

Garantir sempre:

- Integridade referencial.
- Consistência transacional.
- Chaves estrangeiras.
- Restrições de unicidade.
- Dados auditáveis.

---

# ADRs Relacionados

| Tema | ADR |
|------|-----|
| Repository Pattern | ADR-0002 |
| Entity Framework Core | ADR-0004 |
| Dependency Injection | ADR-0009 |
| Soft Delete | ADR-0016 |
| Auditoria | ADR-0017 |
| Configuration Management | ADR-0018 |
| Migrations | ADR-0019 |
| Estratégia de Testes | ADR-0020 |

Consulte:

```text
knowledge/decisions.md
```

---

# Antes de Alterar o Banco

Verifique:

- Existe documentação da entidade?
- Existe Migration correspondente?
- O relacionamento está documentado?
- Existe impacto em outros módulos?
- O Soft Delete foi considerado?
- A Auditoria foi mantida?
- Existe índice necessário?
- Há impacto de performance?

---

# Documentação Relacionada

| Assunto | Documento |
|----------|-----------|
| Arquitetura | knowledge/architecture.md |
| APIs | knowledge/api.md |
| Domínio | knowledge/domain.md |
| Regras de Negócio | knowledge/business-rules.md |
| Desenvolvimento | knowledge/development.md |
| Padrões | knowledge/patterns.md |
| ADRs | knowledge/decisions.md |

---

# Documentação Oficial

Para informações detalhadas, consulte:

```text
docs/database/
```

A documentação oficial contém informações completas sobre:

- Modelo de dados.
- Entidades.
- Relacionamentos.
- Mapeamentos.
- Índices.
- Constraints.
- Auditoria.
- Soft Delete.
- Migrations.
- Performance.

---

# Fluxo Recomendado para Agentes de IA

```text
Ler database.md

↓

Consultar decisions.md

↓

Identificar a entidade

↓

Consultar documentação oficial

↓

Implementar alterações

↓

Criar Migration

↓

Executar testes

↓

Atualizar documentação
```

---

# Resumo

Este documento fornece uma visão geral da camada de persistência do Agilium Manager.

Antes de alterar qualquer estrutura de banco de dados:

- consulte a documentação oficial;
- respeite os padrões de persistência;
- utilize Entity Framework Core como ORM principal;
- utilize Dapper apenas para consultas especializadas;
- mantenha Soft Delete, Auditoria e Migrations conforme os ADRs;
- documente toda alteração estrutural realizada.