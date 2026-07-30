# Arquitetura em Camadas

## Objetivo

Documentar a arquitetura em camadas utilizada pelo ecossistema Agilium Manager, definindo as responsabilidades, os limites e o fluxo de comunicação entre os componentes da solução.

Este documento representa a principal referência arquitetural para o desenvolvimento de novas funcionalidades.

---

# Escopo

Este documento contempla:

- Arquitetura em Camadas
- Responsabilidades
- Fluxo entre Camadas
- Regras de Dependência
- Componentes
- Comunicação
- Cross-Cutting Concerns
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura
- Fluxo da Requisição
- Presentation Layer
- API Layer
- Application / Business Layer
- Domain Layer
- Infrastructure Layer
- Cross-Cutting Concerns
- Fluxo de Dependências
- Regras Arquiteturais
- Boas Práticas
- Limitações
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager adota uma arquitetura em camadas, onde cada camada possui responsabilidades bem definidas e depende apenas das abstrações das camadas inferiores.

Os principais objetivos são:

- baixo acoplamento;
- alta coesão;
- reutilização de código;
- facilidade de testes;
- escalabilidade;
- manutenção simplificada.

---

# Arquitetura

```text
                Presentation

        MVC             APIs

             │

             ▼

        Application

             │

             ▼

           Domain

             ▲

             │

      Infrastructure

             │

      Banco de Dados
```

As dependências sempre apontam para o centro da arquitetura.

---

# Fluxo da Requisição

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

Entity Framework / Dapper

↓

Banco de Dados

↓

Resposta
```

---

# Presentation Layer

Responsável pela interação com o usuário.

Inclui:

- Controllers
- Views
- ViewModels
- Filtros
- Model Binding
- Validação de entrada

Responsabilidades:

- receber requisições;
- validar modelos;
- chamar Services;
- retornar respostas.

Não deve conter regras de negócio.

---

# API Layer

Responsável pela exposição dos serviços REST.

Inclui:

- Controllers
- DTOs
- Versionamento
- Swagger
- Autenticação
- Autorização

Responsabilidades:

- expor endpoints;
- validar contratos;
- controlar autenticação;
- delegar processamento à camada de Application.

Os detalhes de implementação deverão ser confirmados nos projetos de API.

---

# Application / Business Layer

Representa o núcleo da aplicação.

Responsabilidades:

- orquestrar casos de uso;
- aplicar regras de negócio;
- validar operações;
- coordenar repositórios;
- publicar notificações.

Componentes típicos:

- Services
- Interfaces
- DTOs
- Validators

Esta camada concentra a lógica de aplicação.

---

# Domain Layer

Representa o domínio do negócio.

Responsabilidades:

- entidades;
- objetos de valor;
- contratos;
- regras fundamentais;
- abstrações.

O domínio deve permanecer independente de tecnologias externas.

Não deve conhecer:

- ASP.NET Core;
- Entity Framework Core;
- Dapper;
- MVC;
- APIs.

---

# Infrastructure Layer

Responsável pela implementação dos serviços técnicos.

Inclui:

- Repositories
- Entity Framework Core
- Dapper
- MongoDB (quando aplicável)
- Integrações externas
- Persistência

Responsabilidades:

- acesso a dados;
- integrações;
- configuração técnica;
- implementação de interfaces.

---

# Cross-Cutting Concerns

O levantamento técnico identificou componentes transversais utilizados por múltiplas camadas.

## Dependency Injection

Centraliza a criação de objetos e reduz o acoplamento.

## AutoMapper

Responsável pelo mapeamento entre entidades, DTOs e ViewModels.

## Notification Pattern

Centraliza erros e validações de negócio sem lançar exceções para fluxos esperados.

## ExceptionMiddleware

Padroniza o tratamento global de exceções.

## EmpresaSelecionadaMiddleware

Garante o contexto correto da empresa durante o processamento da requisição.

## Configuração

Centraliza:

- appsettings;
- variáveis de ambiente;
- serviços compartilhados.

---

# Fluxo de Dependências

```text
Presentation
        │
        ▼
Application
        │
        ▼
Domain
        ▲
        │
Infrastructure
```

A camada Domain nunca deve depender das demais.

---

# Regras Arquiteturais

## Presentation

Pode depender de:

- Application
- Interfaces
- Shared

Não deve acessar:

- Repositories
- DbContext
- Entity Framework Core

---

## API

Pode depender de:

- Application
- Shared

Não deve acessar diretamente:

- Infrastructure
- Banco de Dados

---

## Application

Pode depender de:

- Domain
- Interfaces

Não deve conhecer Presentation.

---

## Domain

Não deve depender de:

- ASP.NET Core
- Entity Framework Core
- MVC
- APIs
- Infrastructure

---

## Infrastructure

Implementa contratos definidos pelas camadas superiores e concentra as dependências tecnológicas.

---

# Comunicação entre Camadas

Toda comunicação deve ocorrer através de abstrações.

Fluxo recomendado:

```text
Controller

↓

IService

↓

Service

↓

IRepository

↓

Repository

↓

DbContext
```

Nunca utilizar implementações concretas diretamente nas camadas superiores.

---

# Boas Práticas

Sempre:

- respeitar os limites entre camadas;
- depender de interfaces;
- manter Controllers enxutos;
- centralizar regras de negócio na Application;
- manter o Domain independente;
- encapsular persistência na Infrastructure.

Evitar:

- lógica de negócio em Controllers;
- acesso direto ao banco pela camada de apresentação;
- dependências circulares;
- comunicação direta entre camadas não adjacentes.

---

# Limitações Conhecidas

O levantamento técnico confirmou a utilização de:

- Layered Architecture;
- Dependency Injection;
- Service Layer;
- Repository Pattern;
- Notification Pattern;
- AutoMapper;
- ExceptionMiddleware;
- EmpresaSelecionadaMiddleware.

Ainda deverão ser confirmados, durante a análise dos projetos `agilium-manager-azure-api` e `agilium-pdv-azure-api`:

- estrutura completa da camada Application;
- separação entre Domain e Business;
- organização física dos projetos;
- responsabilidades específicas de cada assembly.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- criação de uma nova camada;
- alteração nas dependências entre projetos;
- inclusão de novos componentes transversais;
- mudança na arquitetura da solução.

---

# Documentação Relacionada

- overview.md
- solution-structure.md
- dependency-flow.md
- database/overview.md
- patterns/dependency-injection.md
- patterns/repository.md
- patterns/unit-of-work.md
- architecture/security.md