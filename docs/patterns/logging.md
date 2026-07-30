# Logging

# Objetivo

Documentar a arquitetura de logging utilizada pelo Agilium Manager, descrevendo como eventos são registrados, quais componentes participam do processo e as convenções adotadas para diagnóstico e auditoria da aplicação.

---

# Escopo

Este documento contempla:

- Arquitetura de Logging
- Configuração
- Fluxo de Registro
- ILogService
- ExceptionMiddleware
- Controllers
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura de Logging
- Configuração
- Componentes
- Fluxo de Registro
- Utilização
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager utiliza os mecanismos de logging do ASP.NET Core juntamente com uma abstração própria (`ILogService`) para registrar eventos relevantes da aplicação.

A estratégia de logging atua em diferentes níveis da arquitetura, permitindo registrar erros, exceções e eventos operacionais durante o processamento das requisições. :contentReference[oaicite:1]{index=1}

---

# Arquitetura de Logging

O fluxo de registro segue a arquitetura:

```text
Controller

↓

ILogService

↓

Logging ASP.NET Core

↓

Console / Debug

↓

Persistência de Logs
```

Além do fluxo iniciado pelos Controllers, exceções não tratadas são capturadas pelo middleware global de exceções. :contentReference[oaicite:2]{index=2}

---

# Configuração

O levantamento identificou que a configuração do logging ocorre durante o `Startup`.

Fluxo simplificado:

```text
Startup

↓

ConfigureServices

↓

AddLogging()

↓

Dependency Injection
```

O `AddLogging()` integra os mecanismos de logging do ASP.NET Core ao ciclo de vida da aplicação. :contentReference[oaicite:3]{index=3}

---

# Componentes

## ILogService

O `MainController` recebe uma instância de `ILogService` por Injeção de Dependência.

Esse serviço é disponibilizado para todos os Controllers derivados, padronizando o registro de eventos da camada de apresentação. :contentReference[oaicite:4]{index=4}

---

## ExceptionMiddleware

O projeto possui um middleware dedicado ao tratamento global de exceções:

```text
ExceptionMiddleware
```

Sua responsabilidade é capturar exceções não tratadas durante o processamento das requisições e integrá-las à estratégia de logging da aplicação. :contentReference[oaicite:5]{index=5}

---

## LogController

O levantamento identificou a existência de um `LogController`, indicando a presença de funcionalidades relacionadas à consulta ou gerenciamento de registros de log na aplicação. O detalhamento dessas funcionalidades deve ser documentado após análise específica desse módulo. :contentReference[oaicite:6]{index=6}

---

# Fluxo de Registro

O fluxo típico para registro de erros segue a sequência:

```text
Controller

↓

try/catch

↓

ILogService

↓

ExceptionMiddleware (quando aplicável)

↓

Registro do Evento
```

Erros não tratados percorrem o pipeline até o `ExceptionMiddleware`, responsável pelo tratamento global. :contentReference[oaicite:7]{index=7}

---

# Utilização

O levantamento identificou os seguintes pontos de utilização do mecanismo de logging:

- Controllers;
- `MainController`;
- `ExceptionMiddleware`;
- `ILogService`.

Esses componentes colaboram para registrar informações relevantes durante a execução da aplicação. :contentReference[oaicite:8]{index=8}

---

# Boas Práticas

Sempre:

- registrar exceções relevantes;
- utilizar `ILogService` para centralizar registros;
- manter mensagens de log claras e objetivas;
- evitar duplicação de registros para o mesmo evento;
- preservar a separação entre tratamento de exceções e regras de negócio.

Evitar:

- registrar informações sensíveis (senhas, tokens ou dados pessoais);
- utilizar logging para controlar fluxo de negócio;
- capturar exceções sem registrá-las ou tratá-las adequadamente.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de `AddLogging()`;
- utilização dos provedores Console e Debug;
- existência de `ILogService`;
- utilização de `ExceptionMiddleware`;
- existência de `LogController`;
- tratamento de exceções por meio de `try/catch` e middleware global. :contentReference[oaicite:9]{index=9}

Ainda deverão ser documentados mediante análise do código-fonte:

- implementação de `ILogService`;
- destino definitivo dos registros;
- estratégia de persistência dos logs;
- níveis de severidade utilizados;
- políticas de retenção;
- integração com provedores externos de logging, caso existam.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração da estratégia de logging;
- adoção de novos provedores;
- mudanças na implementação do `ILogService`;
- alterações no `ExceptionMiddleware`.

---

# Documentação Relacionada

## Desenvolvimento

- development/debugging.md

## Arquitetura

- architecture/overview.md
- architecture/layers.md

## API

- api/errors.md