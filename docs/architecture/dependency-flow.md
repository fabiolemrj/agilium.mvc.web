# Fluxo de Dependências

## Objetivo

Documentar a direção das dependências entre os projetos do ecossistema Agilium Manager, estabelecendo as regras arquiteturais que garantem baixo acoplamento, alta coesão e manutenção da arquitetura em camadas.

Este documento define como os projetos devem se relacionar e quais dependências são permitidas.

---

# Escopo

Este documento contempla:

- Arquitetura em Camadas
- Fluxo de Dependências
- Referências entre Projetos
- Dependency Injection
- Inversão de Dependência
- Regras Arquiteturais
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura de Dependências
- Direção das Dependências
- Grafo de Referências
- Dependency Injection
- Regras de Dependência
- Dependências Permitidas
- Dependências Proibidas
- Inclusão de Novos Projetos
- Boas Práticas
- Limitações
- Documentação Relacionada

---

# Visão Geral

A arquitetura do Agilium Manager segue o princípio da **Dependency Inversion**, onde as dependências sempre apontam para camadas mais internas da aplicação.

Nenhuma camada de infraestrutura deve conhecer detalhes das camadas superiores.

---

# Arquitetura de Dependências

```text
                Presentation

          MVC        API

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

As dependências sempre devem seguir essa direção lógica.

---

# Direção das Dependências

Fluxo esperado:

```text
MVC

↓

Application

↓

Domain

↑

Infrastructure
```

A camada Domain permanece independente.

A Infrastructure implementa contratos definidos pelo Domain ou pela Application.

---

# Grafo de Referências

Estrutura conceitual:

```text
Agilium.Mvc.Web
        │
        ▼
Agilium.Application
        │
        ▼
Agilium.Domain
        ▲
        │
Agilium.Infrastructure
```

A estrutura definitiva deverá ser validada a partir da Solution (`.sln`) e dos arquivos `.csproj`.

---

# Dependency Injection

Toda comunicação entre camadas deve ocorrer através de interfaces registradas no contêiner de Injeção de Dependência.

Fluxo:

```text
Controller

↓

Interface

↓

Service

↓

Repository

↓

DbContext
```

As implementações concretas devem permanecer ocultas das camadas consumidoras.

---

# Configuração da Injeção de Dependência

A solução deve centralizar os registros de serviços em classes específicas de configuração.

Exemplos:

- DependencyInjectionConfig
- NativeInjectorBootStrapper
- ServiceCollectionExtensions

Os nomes reais devem refletir a implementação encontrada na solução.

---

# Regras de Dependência

## A camada MVC

Pode depender de:

- Application
- Interfaces
- Shared

Não deve depender diretamente de:

- Infrastructure
- DbContext
- Repositories concretos

---

## A camada API

Pode depender de:

- Application
- Interfaces
- Shared

Não deve acessar diretamente o banco de dados.

---

## A camada Application

Pode depender de:

- Domain
- Interfaces

Não deve conhecer detalhes da camada Presentation.

---

## A camada Domain

Deve permanecer independente.

Não deve depender de:

- Entity Framework Core
- ASP.NET Core
- MVC
- API
- Infrastructure

---

## A camada Infrastructure

Pode depender de:

- Domain
- Application (quando necessário para implementar contratos)

É responsável por:

- Entity Framework Core
- Dapper
- MongoDB
- Serviços externos
- Persistência

---

# Dependências Permitidas

| Origem | Destino |
|---------|----------|
| MVC | Application |
| API | Application |
| Application | Domain |
| Infrastructure | Domain |
| Infrastructure | Interfaces |

---

# Dependências Proibidas

Nunca permitir:

- MVC → Infrastructure
- MVC → DbContext
- Controller → Repository
- Controller → Entity Framework
- Domain → Infrastructure
- Domain → ASP.NET Core
- Domain → Entity Framework Core

---

# Inclusão de Novos Projetos

Antes de criar uma nova referência entre projetos, verificar:

- A responsabilidade pertence realmente ao projeto?
- Existe uma interface que possa ser reutilizada?
- A dependência respeita a arquitetura em camadas?
- O novo projeto introduz acoplamento desnecessário?

Toda nova referência deve ser justificada tecnicamente.

---

# Boas Práticas

Sempre:

- depender de abstrações;
- utilizar Dependency Injection;
- manter o Domain isolado;
- centralizar registros de DI;
- evitar dependências circulares;
- utilizar interfaces para comunicação entre camadas.

Evitar:

- referências cruzadas;
- acesso direto ao banco pela camada de apresentação;
- uso de classes concretas quando houver abstrações disponíveis;
- lógica de negócio em Controllers.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de Dependency Injection;
- arquitetura em camadas;
- uso de Services;
- uso de Repositories;
- AutoMapper;
- Notification Pattern;
- Middleware Pipeline.

Entretanto, o grafo definitivo de dependências entre projetos deverá ser confirmado pela análise da estrutura da Solution (`.sln`) e dos arquivos de projeto (`.csproj`).

---

# Atualização

Sempre que houver:

- criação de um novo projeto;
- alteração de referências;
- mudança de arquitetura;
- criação de uma nova camada;

este documento deverá ser revisado.

---

# Documentação Relacionada

- layers.md
- solution-structure.md
- patterns/dependency-injection.md
- patterns/repository.md
- patterns/unit-of-work.md
- architecture/overview.md