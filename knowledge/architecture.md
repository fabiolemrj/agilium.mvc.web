# Architecture

## Objetivo

Este documento fornece uma visão resumida da arquitetura do **Agilium Manager**, servindo como ponto de entrada para desenvolvedores e agentes de IA.

A documentação oficial e detalhada encontra-se em:

```text
docs/architecture/
```

Este documento **não substitui** a documentação oficial. Seu objetivo é orientar rapidamente onde encontrar as informações e quais decisões arquiteturais devem ser consideradas durante o desenvolvimento.

---

# Visão Geral

O Agilium Manager foi desenvolvido utilizando uma arquitetura em camadas, baseada nos princípios de:

- Clean Architecture
- Domain-Driven Design (DDD)
- SOLID
- Separation of Concerns (SoC)
- Dependency Injection
- Repository Pattern
- Service Layer
- Notification Pattern

Toda implementação deve respeitar essa organização.

---

# Arquitetura da Solução

```text
               Interface (MVC / API)
                        │
                        ▼
                  Controllers
                        │
                        ▼
              Application Services
                        │
                        ▼
                     Domain
                        │
                        ▼
                 Repository Layer
                        │
                        ▼
                  Persistence
            (EF Core / Dapper)
                        │
                        ▼
                    MySQL
```

---

# Fluxo das Dependências

As dependências sempre seguem o mesmo sentido.

```text
MVC / API

↓

Controller

↓

Application Service

↓

Domain

↓

Repository

↓

Persistence

↓

Database
```

Nenhuma camada inferior pode depender de uma camada superior.

---

# Responsabilidades

| Camada | Responsabilidade |
|---------|------------------|
| MVC | Interface Web |
| API | Exposição dos serviços REST |
| Controllers | Entrada da aplicação |
| Application | Casos de uso e orquestração |
| Domain | Regras de negócio |
| Repository | Persistência |
| Persistence | EF Core, Dapper, Mappings |
| Database | Armazenamento |

---

# Tecnologias Principais

| Tecnologia | Utilização |
|------------|------------|
| ASP.NET Core | Backend |
| ASP.NET MVC | Interface Web |
| Entity Framework Core | ORM |
| Dapper | Consultas otimizadas |
| MySQL | Banco de Dados |
| JWT | Autenticação |
| Docker | Containers |
| xUnit | Testes |
| FluentAssertions | Assertivas |
| Moq | Mocks |

Consulte:

```text
docs/architecture/
docs/development/
```

---

# Padrões Arquiteturais

A solução adota os seguintes padrões:

- Layered Architecture
- Repository Pattern
- Service Layer
- Dependency Injection
- Notification Pattern
- Options Pattern
- Soft Delete
- Auditoria Automática
- Global Exception Handling

Consulte:

```text
docs/patterns/
```

---

# Princípios

Toda implementação deve seguir:

- Baixo acoplamento
- Alta coesão
- SOLID
- Clean Code
- DRY
- KISS
- Separation of Concerns
- Código testável
- Código reutilizável

---

# Organização da Solução

Resumo da estrutura da Solution.

```text
Solution

src/
    MVC
    API
    Application
    Domain
    Repository
    Persistence
    Infrastructure

tests/

docs/

.ai/
```

A estrutura detalhada encontra-se em:

```text
docs/architecture/
```

---

# Fluxo de uma Requisição

```text
Usuário

↓

MVC / API

↓

Controller

↓

Application Service

↓

Domain

↓

Repository

↓

Entity Framework

↓

Banco de Dados

↓

Response
```

---

# Decisões Arquiteturais

Antes de implementar qualquer funcionalidade, consulte os ADRs correspondentes.

| Tema | ADR |
|------|-----|
| Arquitetura em Camadas | ADR-0001 |
| Repository Pattern | ADR-0002 |
| Notification Pattern | ADR-0003 |
| Entity Framework Core | ADR-0004 |
| Autenticação | ADR-0005 |
| Autorização | ADR-0006 |
| Estratégia de Validação | ADR-0007 |
| Dependency Injection | ADR-0009 |
| Service Layer | ADR-0011 |
| Docker | ADR-0012 |
| Logging | ADR-0013 |
| Exception Handling | ADR-0014 |
| API Response | ADR-0015 |
| Soft Delete | ADR-0016 |
| Auditoria | ADR-0017 |
| Configuration | ADR-0018 |
| Migrations | ADR-0019 |
| Testes | ADR-0020 |

Consulte:

```text
knowledge/decisions.md
```

ou

```text
docs/decisions/
```

---

# Antes de Implementar

Sempre responda às seguintes perguntas:

- Qual camada será alterada?
- Existe um ADR relacionado?
- Existe uma regra de negócio envolvida?
- Existe documentação específica do módulo?
- O padrão arquitetural está sendo respeitado?

---

# Documentação Relacionada

| Assunto | Documento |
|----------|-----------|
| APIs | knowledge/api.md |
| Banco de Dados | knowledge/database.md |
| Domínio | knowledge/domain.md |
| Regras de Negócio | knowledge/business-rules.md |
| Padrões | knowledge/patterns.md |
| Desenvolvimento | knowledge/development.md |
| Diagramas | knowledge/diagrams.md |
| ADRs | knowledge/decisions.md |

---

# Documentação Oficial

Para informações completas, consulte:

```text
docs/

architecture/
decisions/
patterns/
database/
domain/
business-rules/
development/
diagrams/
```

---

# Fluxo Recomendado para Agentes de IA

```text
Ler architecture.md

↓

Ler decisions.md

↓

Identificar o módulo

↓

Consultar a documentação oficial

↓

Planejar a implementação

↓

Executar alterações

↓

Atualizar documentação

↓

Validar aderência aos ADRs
```

---

# Resumo

Este documento serve como um **guia de navegação arquitetural**.

Para qualquer alteração na solução:

- respeite a arquitetura em camadas;
- siga os padrões definidos;
- consulte os ADRs aplicáveis;
- utilize a documentação oficial em `docs/` como fonte de verdade.