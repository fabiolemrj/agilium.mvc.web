# ADR-0014 - Estratégia de Tratamento Global de Exceções

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é composto por diversos módulos responsáveis por operações críticas de negócio, incluindo APIs REST, aplicações MVC, integrações externas e processamento de dados.

Durante a evolução da plataforma foram identificadas diferentes formas de tratamento de exceções, como:

- `try/catch` em Controllers;
- tratamento diretamente nos Services;
- exceções não tratadas chegando ao usuário;
- mensagens inconsistentes;
- respostas HTTP diferentes para o mesmo tipo de erro.

Essa abordagem dificultava a manutenção, aumentava a duplicação de código e gerava uma experiência inconsistente para os consumidores da API.

Era necessário definir uma estratégia única para tratamento de exceções em toda a plataforma.

---

# Problema

Sem uma política padronizada de tratamento de exceções ocorrem diversos problemas:

- Código duplicado;
- Respostas inconsistentes;
- Exposição de detalhes internos da aplicação;
- Dificuldade de manutenção;
- Logs incompletos;
- Experiência inconsistente para clientes da API.

Também foi identificado o uso excessivo de exceções para representar erros de negócio, contrariando a estratégia definida na ADR-0003 (Notification Pattern).

---

# Alternativas Consideradas

## Alternativa 1 — Try/Catch em todas as Controllers

### Vantagens

- Simples.
- Fácil compreensão inicial.

### Desvantagens

- Código repetitivo.
- Difícil manutenção.
- Alto acoplamento.
- Tratamento inconsistente.

---

## Alternativa 2 — Try/Catch em Services

### Vantagens

- Centraliza parte da lógica.

### Desvantagens

- Continua gerando duplicação.
- Mistura tratamento de erro com regras da aplicação.

---

## Alternativa 3 — Middleware Global (Escolhida)

### Vantagens

- Tratamento centralizado.
- Código limpo.
- Padronização.
- Fácil manutenção.
- Integração com Logging.

### Desvantagens

- Exige configuração inicial.
- Necessidade de mapear corretamente as exceções.

---

# Decisão

Foi adotado um **Middleware Global de Tratamento de Exceções** como mecanismo oficial para captura e tratamento de erros não previstos.

Toda exceção não tratada deverá ser interceptada pelo middleware, registrada através do sistema oficial de Logging e convertida em uma resposta HTTP padronizada.

As exceções **não deverão ser utilizadas para representar erros de negócio**.

Erros de negócio deverão utilizar o **Notification Pattern** (ADR-0003).

---

# Objetivos

Esta estratégia possui os seguintes objetivos:

- Padronizar respostas de erro.
- Reduzir duplicação de código.
- Melhorar observabilidade.
- Evitar exposição de detalhes internos.
- Facilitar manutenção.
- Melhorar experiência dos consumidores da API.

---

# Fluxo

```text
Request

↓

Controller

↓

Service

↓

Repository

↓

Exception

↓

Exception Middleware

↓

Logging

↓

Resposta HTTP Padronizada
```

---

# Responsabilidades

## Middleware

Responsável por:

- Capturar exceções não tratadas;
- Registrar logs;
- Mapear exceções para códigos HTTP;
- Gerar resposta padronizada;
- Adicionar CorrelationId quando disponível.

---

## Controllers

Não deverão possuir `try/catch` genéricos.

Responsáveis apenas por:

- Receber requisições;
- Chamar Services;
- Retornar respostas.

---

## Services

Responsáveis por:

- Lançar exceções apenas quando ocorrerem falhas inesperadas;
- Utilizar Notification Pattern para erros de negócio.

---

## Repository

Responsável apenas por propagar exceções de infraestrutura.

---

# Exceções Esperadas

Exemplos:

- ValidationException
- UnauthorizedAccessException
- KeyNotFoundException
- ArgumentException
- InvalidOperationException

Cada uma deverá possuir um tratamento específico.

---

# Exceções de Negócio

Não utilizar:

```csharp
throw new Exception("Produto indisponível");
```

Utilizar:

```text
Notification

↓

Produto indisponível para venda.
```

Apenas falhas inesperadas deverão lançar exceções.

---

# Mapeamento HTTP

| Exceção | HTTP |
|----------|------|
| ValidationException | 400 |
| UnauthorizedAccessException | 401 |
| ForbiddenException | 403 |
| KeyNotFoundException | 404 |
| ConflictException | 409 |
| Exception | 500 |

---

# Formato da Resposta

Todas as respostas de erro deverão seguir um padrão único.

Exemplo:

```json
{
    "success": false,
    "status": 500,
    "message": "Ocorreu um erro interno.",
    "correlationId": "9c1d8f5b-2a4b-4f42-b1fd-6d54f4f80b98",
    "errors": []
}
```

Para erros de validação:

```json
{
    "success": false,
    "status": 400,
    "message": "Falha de validação.",
    "errors": [
        "Descrição é obrigatória.",
        "Empresa inválida."
    ]
}
```

---

# Logging

Toda exceção deverá ser registrada utilizando:

```csharp
ILogger<T>
```

Sempre registrar:

- Exceção original;
- StackTrace;
- CorrelationId;
- Usuário autenticado (quando disponível);
- Empresa;
- Endpoint.

---

# Dados Sensíveis

Nunca retornar ao cliente:

- StackTrace;
- SQL;
- Connection String;
- Caminhos físicos;
- Senhas;
- Tokens;
- Informações internas.

Esses dados deverão permanecer apenas nos logs.

---

# Organização

Estrutura sugerida:

```text
Infrastructure/

├── Middleware/

│   ├── ExceptionMiddleware.cs

│   └── ExceptionMiddlewareExtensions.cs

│

├── Exceptions/

│   ├── BusinessException.cs

│   ├── ConflictException.cs

│   ├── ForbiddenException.cs

│   └── ...
```

---

# Benefícios

- Código limpo.
- Tratamento centralizado.
- Respostas consistentes.
- Melhor experiência para APIs.
- Facilidade de manutenção.
- Integração com Logging.
- Melhor rastreabilidade.

---

# Desvantagens

- Necessidade de manutenção do mapeamento de exceções.
- Configuração inicial adicional.

---

# Riscos

Caso esta estratégia não seja seguida:

- Controllers com excesso de `try/catch`;
- Respostas inconsistentes;
- Exposição de detalhes internos;
- Logs incompletos;
- Dificuldade para suporte.

---

# Impacto

Esta decisão impacta:

- APIs
- MVC
- Middleware
- Services
- Repositories
- Logging
- Observabilidade
- Integrações

---

# Plano de Implementação

1. Criar Exception Middleware.
2. Registrar middleware no pipeline HTTP.
3. Mapear exceções conhecidas.
4. Padronizar respostas de erro.
5. Integrar com ILogger.
6. Remover `try/catch` desnecessários das Controllers.
7. Atualizar documentação técnica.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Existe um Middleware Global de Tratamento de Exceções.
- Controllers não possuem `try/catch` genéricos.
- Todas as exceções são registradas via `ILogger`.
- As respostas seguem um formato único.
- Erros de negócio utilizam Notification Pattern.
- Informações sensíveis não são retornadas ao cliente.
- O CorrelationId é incluído quando disponível.

---

# ADRs Relacionados

- ADR-0003 — Notification Pattern
- ADR-0005 — Estratégia de Autenticação
- ADR-0007 — Estratégia de Validação
- ADR-0009 — Dependency Injection
- ADR-0011 — Service Layer
- ADR-0013 — Estratégia de Logging
- ADR-0015 — Padronização das Respostas da API

---

# Referências

- Microsoft — Error Handling in ASP.NET Core
- Microsoft — Middleware Fundamentals
- Microsoft — Problem Details for HTTP APIs (RFC 7807)
- Martin Fowler — Patterns of Enterprise Application Architecture
- Clean Architecture — Robert C. Martin

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | 2026-07-29 | Criação da ADR definindo o Middleware Global como mecanismo oficial para tratamento de exceções do Agilium Manager, padronizando respostas HTTP, integração com logging, proteção de informações sensíveis e utilização do Notification Pattern para erros de negócio. |