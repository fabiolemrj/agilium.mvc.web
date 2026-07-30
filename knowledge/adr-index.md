# ADR Index

Este documento fornece uma visão rápida das **Architecture Decision Records (ADRs)** adotadas pelo Agilium Manager.

> **Objetivo**
>
> Permitir que desenvolvedores e agentes de IA identifiquem rapidamente quais decisões arquiteturais devem ser consideradas antes de implementar qualquer alteração.

---

# Como utilizar

Antes de iniciar qualquer implementação:

1. Identifique o módulo que será alterado.
2. Localize os ADRs relacionados.
3. Leia os ADRs completos em `docs/decisions/`.
4. Certifique-se de que a implementação está aderente às decisões arquiteturais.

A documentação completa de cada decisão encontra-se em:

```text
docs/
└── decisions/
```

---

# Índice das ADRs

| ADR | Decisão | Quando consultar |
|------|----------|------------------|
| ADR-0001 | Arquitetura em Camadas | Sempre |
| ADR-0002 | Repository Pattern | Persistência |
| ADR-0003 | Notification Pattern | Regras de Negócio |
| ADR-0004 | Entity Framework Core | Banco de Dados |
| ADR-0005 | Estratégia de Autenticação | Login e JWT |
| ADR-0006 | Estratégia de Autorização | Permissões |
| ADR-0007 | Estratégia de Validação | Validações |
| ADR-0008 | Versionamento de APIs | Endpoints REST |
| ADR-0009 | Dependency Injection | Serviços |
| ADR-0010 | Dapper para Consultas | Consultas complexas |
| ADR-0011 | Service Layer | Casos de Uso |
| ADR-0012 | Docker e Deploy | Infraestrutura |
| ADR-0013 | Logging | Observabilidade |
| ADR-0014 | Tratamento Global de Exceções | Erros |
| ADR-0015 | Padronização das Respostas | APIs |
| ADR-0016 | Soft Delete | Exclusão de Dados |
| ADR-0017 | Auditoria | Rastreabilidade |
| ADR-0018 | Configuration Management | Configurações |
| ADR-0019 | Migrations | Evolução do Banco |
| ADR-0020 | Estratégia de Testes | Qualidade |

---

# Arquitetura

## ADR-0001 — Arquitetura em Camadas

**Objetivo**

Define a arquitetura oficial da solução.

**Resumo**

```text
MVC

↓

API

↓

Application / Services

↓

Domain

↓

Repository

↓

Persistence

↓

Database
```

**Consultar quando**

- Criar funcionalidades
- Refatorar código
- Criar novos projetos

---

## ADR-0002 — Repository Pattern

**Objetivo**

Define a camada responsável pela persistência.

**Resumo**

```text
Controller

↓

Service

↓

Repository

↓

EF Core

↓

Database
```

**Regras**

- Sem regra de negócio
- Apenas persistência
- Interface obrigatória

---

## ADR-0003 — Notification Pattern

**Objetivo**

Padronizar validações de negócio.

**Resumo**

- Regras retornam notificações.
- Exceptions apenas para falhas inesperadas.

---

## ADR-0004 — Entity Framework Core

**Objetivo**

Padronizar o ORM da aplicação.

**Resumo**

- EF Core para CRUD.
- Fluent API.
- Migrations.
- Dapper apenas para leitura otimizada.

---

# Segurança

## ADR-0005 — Autenticação

**Resumo**

- JWT
- Refresh Token
- Usuário do domínio
- Claims
- Tokens expirados automaticamente

Consultar sempre que alterar:

- Login
- Token
- Sessão
- Refresh Token

---

## ADR-0006 — Autorização

**Resumo**

Autorização baseada em:

- Policies
- Claims
- Permissões

Exemplo:

```text
Produto.Visualizar

Produto.Alterar

Venda.Realizar

Financeiro.Alterar
```

---

# Regras de Negócio

## ADR-0007 — Estratégia de Validação

Resumo:

- Controllers validam entrada.
- Services executam casos de uso.
- Domain valida regras.
- Notification Pattern para erros esperados.

---

## ADR-0011 — Service Layer

Resumo:

```text
Controller

↓

Application Service

↓

Domain

↓

Repository
```

Services:

- Orquestram
- Coordenam transações
- Chamam múltiplos repositórios
- Nunca acessam Views

---

# APIs

## ADR-0008 — Versionamento

Resumo

```text
/api/v1/

/api/v2/
```

Criar nova versão apenas para Breaking Changes.

---

## ADR-0015 — Resposta Padrão

Todas as APIs retornam:

```json
{
    "success": true,
    "status": 200,
    "message": "",
    "data": {},
    "errors": []
}
```

---

# Persistência

## ADR-0010 — Dapper

Resumo

Utilizar Dapper apenas para:

- Dashboards
- Relatórios
- Consultas grandes
- Leitura

Nunca para regras de negócio.

---

## ADR-0016 — Soft Delete

Resumo

Nunca remover registros.

Utilizar:

```text
Ativo

DataExclusao

UsuarioExclusao
```

---

## ADR-0017 — Auditoria

Toda alteração registra:

- Usuário
- Data
- Empresa
- Operação

Preenchimento automático pelo DbContext.

---

## ADR-0019 — Migrations

Toda alteração estrutural do banco deve possuir Migration.

Fluxo:

```text
Entidade

↓

Mapping

↓

Migration

↓

Review

↓

Deploy
```

---

# Infraestrutura

## ADR-0009 — Dependency Injection

Resumo

Todos os componentes devem utilizar DI nativa do ASP.NET Core.

Nunca utilizar:

- Service Locator
- new em Controllers

---

## ADR-0012 — Docker

Resumo

- Multi-stage Build
- Docker Compose
- Containers Stateless
- Variáveis de Ambiente

---

## ADR-0013 — Logging

Resumo

Utilizar:

```text
ILogger<T>
```

Nunca utilizar:

```text
Console.WriteLine()

Debug.WriteLine()
```

---

## ADR-0018 — Configuration Management

Resumo

Hierarquia:

```text
appsettings.json

↓

appsettings.Environment.json

↓

Environment Variables

↓

Secret Manager
```

Nunca armazenar:

- Senhas
- JWT Secret
- API Keys

no repositório.

---

# Qualidade

## ADR-0020 — Estratégia de Testes

Resumo

Prioridade:

```text
Unit Tests

↓

Integration Tests

↓

E2E
```

Ferramentas:

- xUnit
- Moq
- FluentAssertions
- Coverlet

---

# Consulta Rápida

| Se você vai... | Consulte primeiro |
|----------------|-------------------|
| Criar uma API | ADR-0008, ADR-0015 |
| Criar um Service | ADR-0011 |
| Alterar regras de negócio | ADR-0003, ADR-0007 |
| Criar Repository | ADR-0002 |
| Alterar Banco | ADR-0004, ADR-0016, ADR-0017, ADR-0019 |
| Alterar Login | ADR-0005, ADR-0006 |
| Configurar Docker | ADR-0012, ADR-0018 |
| Criar Logs | ADR-0013 |
| Escrever Testes | ADR-0020 |

---

# Fluxo recomendado para IA

```text
Solicitação

↓

Identificar módulo

↓

Consultar este índice

↓

Abrir ADRs relacionados

↓

Planejar implementação

↓

Executar alterações

↓

Validar aderência aos ADRs

↓

Documentar
```

---

# Documentação Oficial

Este documento é apenas um **índice resumido**.

As decisões completas estão em:

```text
docs/
└── decisions/
    ├── ADR-0001-...
    ├── ADR-0002-...
    ├── ...
    └── ADR-0020-...
```

Em caso de divergência, **sempre prevalece o conteúdo dos ADRs oficiais**.