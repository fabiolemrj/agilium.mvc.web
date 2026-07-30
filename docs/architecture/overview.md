# Arquitetura do Agilium Manager

## Objetivo

Apresentar uma visão geral da arquitetura do ecossistema Agilium Manager, descrevendo seus componentes, princípios arquiteturais, organização da solução e a interação entre as principais camadas da aplicação.

Este documento funciona como o ponto de entrada para toda a documentação técnica da plataforma.

---

# Escopo

Este documento contempla:

- Visão Geral da Plataforma
- Objetivos Arquiteturais
- Arquitetura em Camadas
- Componentes
- Tecnologias
- Fluxo Geral
- Padrões Arquiteturais
- Organização da Documentação

Os detalhes de cada área encontram-se em documentos específicos.

---

# Índice

- Visão Geral
- Objetivos Arquiteturais
- Arquitetura da Plataforma
- Componentes
- Fluxo Geral
- Princípios Arquiteturais
- Tecnologias
- Organização da Solução
- Organização da Documentação
- ADRs Relacionados
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager é uma plataforma composta por aplicações Web, APIs e componentes de infraestrutura organizados em uma arquitetura em camadas.

Cada camada possui responsabilidades bem definidas e comunica-se apenas através de contratos e abstrações, reduzindo acoplamento e facilitando manutenção, testes e evolução da solução.

---

# Objetivos Arquiteturais

A arquitetura foi construída para atender aos seguintes objetivos:

- separação de responsabilidades;
- baixo acoplamento;
- alta coesão;
- reutilização de componentes;
- facilidade de testes;
- escalabilidade;
- manutenção simplificada;
- padronização entre projetos.

---

# Arquitetura da Plataforma

```text
                 Usuários

                      │

         ┌──────────────────────┐
         │     MVC Web          │
         └──────────────────────┘

                      │

         ┌──────────────────────┐
         │      APIs REST       │
         └──────────────────────┘

                      │

         ┌──────────────────────┐
         │  Application Layer   │
         └──────────────────────┘

                      │

         ┌──────────────────────┐
         │    Domain Layer      │
         └──────────────────────┘

                      │

         ┌──────────────────────┐
         │ Infrastructure Layer │
         └──────────────────────┘

                      │

               Bancos de Dados
```

---

# Componentes

Os principais componentes identificados são:

- MVC Web
- APIs REST
- Application Services
- Domain
- Infrastructure
- Persistência
- Middlewares
- Serviços Compartilhados

Cada componente possui documentação própria.

---

# Fluxo Geral

```text
Cliente

↓

Middleware

↓

Autenticação

↓

Autorização

↓

Controller

↓

Application Service

↓

Domain

↓

Repository

↓

Banco

↓

Resposta
```

---

# Princípios Arquiteturais

O levantamento técnico identificou os seguintes padrões utilizados pela solução:

## Layered Architecture

Separação clara entre apresentação, aplicação, domínio e infraestrutura.

## Dependency Injection

Toda comunicação ocorre através de interfaces registradas no contêiner de DI.

## Repository Pattern

Persistência encapsulada na camada de infraestrutura.

## Service Layer

As regras de negócio são centralizadas na camada de aplicação.

## Notification Pattern

Validações de negócio são tratadas através de notificações em vez de exceções para fluxos esperados.

## Middleware Pipeline

Processamento transversal centralizado em middlewares.

## AutoMapper

Conversão padronizada entre entidades, DTOs e ViewModels.

---

# Tecnologias

As tecnologias efetivamente identificadas no levantamento incluem:

| Categoria | Tecnologia |
|------------|------------|
| Plataforma | .NET Core 3.1 |
| Web | ASP.NET Core MVC |
| ORM | Entity Framework Core |
| Persistência | Dapper (em cenários específicos) |
| Autenticação | ASP.NET Core Identity |
| Sessão Web | Cookie Authentication |
| Mapeamento | AutoMapper |

Outras tecnologias deverão ser adicionadas conforme forem confirmadas na análise completa da solução.

---

# Organização da Solução

A arquitetura está organizada em projetos especializados:

- Presentation
- APIs
- Application
- Domain
- Infrastructure

A estrutura física é documentada em `solution-structure.md`.

---

# Organização da Documentação

```text
docs/

architecture/
api/
database/
patterns/
security/
deployment/
integrations/
development/
decisions/
```

Cada pasta documenta uma área específica da arquitetura.

---

# ADRs Relacionados

Este documento está diretamente relacionado aos seguintes ADRs:

- ADR-001 – Arquitetura em Camadas
- ADR-002 – Dependency Injection
- ADR-003 – Repository Pattern
- ADR-004 – Service Layer
- ADR-005 – Notification Pattern
- ADR-006 – Middleware Pipeline

---

# Documentação Relacionada

- architecture/layers.md
- architecture/dependency-flow.md
- architecture/solution-structure.md
- architecture/security.md
- database/overview.md
- deployment/overview.md
- integrations/overview.md