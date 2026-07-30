# API Conventions

## Objective
Document the coding conventions and standards for developing and maintaining the Agilium Manager APIs.

## Scope
Covers controller patterns, response formats, error handling, and best practices.

## Index
- [Controller Conventions](#controller-conventions)
- [Request/Response Format](#requestresponse-format)
- [Error Handling](#error-handling)
- [Pagination](#pagination)
- [Filtering and Sorting](#filtering-and-sorting)
- [Related Documents](#related-documents)# Convenções da API

## Objetivo

Documentar os padrões de desenvolvimento utilizados nas APIs do Agilium Manager, garantindo consistência entre os projetos, facilidade de manutenção e conformidade com a arquitetura da solução.

Este documento deve servir como referência para implementação de novos endpoints e manutenção dos existentes.

---

# Escopo

Este documento contempla:

- Convenções de Controllers
- Convenções de Rotas
- Convenções de Endpoints
- Padrões de Request
- Padrões de Response
- Tratamento de Erros
- Paginação
- Filtros
- Ordenação
- Versionamento
- Boas práticas

---

# Fontes para Análise

Antes de atualizar este documento analisar:

- Controllers
- MainController
- Startup.cs
- Program.cs
- ExceptionMiddleware
- AutoMapper
- Notification Pattern
- Models
- DTOs
- ViewModels
- Middleware
- Swagger
- Configuração de Versionamento

---

# Índice

- Convenções Gerais
- Controllers
- Rotas
- Endpoints
- Request
- Response
- Tratamento de Erros
- Paginação
- Filtros
- Ordenação
- Versionamento
- Boas Práticas
- Documentos Relacionados

---

# Convenções Gerais

As APIs devem seguir os padrões do ecossistema ASP.NET Core e manter consistência com a arquitetura do Agilium Manager.

Sempre:

- utilizar Controllers enxutos;
- concentrar regras de negócio nos Services;
- utilizar DTOs para comunicação externa;
- manter nomenclatura consistente;
- utilizar Dependency Injection.

---

# Controllers

## Convenções

Os Controllers devem:

- possuir sufixo `Controller`;
- possuir responsabilidade única;
- apenas orquestrar chamadas;
- não conter regras de negócio.

Exemplo:

```csharp
ProdutoController
ClienteController
UsuarioController
```

Sempre que possível:

```csharp
[ApiController]
[Route("api/v{version:apiVersion}/[controller]")]
```

As Actions devem possuir nomes claros e objetivos.

Exemplos:

- Get()
- GetById()
- Post()
- Put()
- Delete()

---

# Rotas

As rotas devem seguir o padrão REST.

Exemplos:

```text
GET    /api/v1/produtos
GET    /api/v1/produtos/{id}
POST   /api/v1/produtos
PUT    /api/v1/produtos/{id}
DELETE /api/v1/produtos/{id}
```

Evitar verbos na URL.

---

# Endpoints

Cada endpoint deve:

- possuir responsabilidade única;
- validar entrada;
- retornar códigos HTTP apropriados;
- documentar parâmetros;
- documentar respostas.

---

# Request

Os Requests devem utilizar DTOs específicos.

Não expor diretamente entidades do domínio.

Utilizar:

- DataAnnotations
- FluentValidation (quando aplicável)

Sempre validar os dados recebidos.

---

# Response

As respostas devem utilizar modelos padronizados.

Sempre retornar objetos consistentes.

Exemplo:

```json
{
  "success": true,
  "data": {},
  "errors": []
}
```

Caso o projeto utilize outro padrão, documentá-lo.

---

# Tratamento de Erros

Os erros devem ser tratados através do middleware global.

Sempre utilizar:

- ExceptionMiddleware
- Notification Pattern

Mapeamento sugerido:

| Código HTTP | Situação |
|--------------|----------|
| 200 | Sucesso |
| 201 | Recurso criado |
| 204 | Sem conteúdo |
| 400 | Requisição inválida |
| 401 | Não autenticado |
| 403 | Acesso negado |
| 404 | Recurso não encontrado |
| 409 | Conflito |
| 422 | Erro de validação |
| 500 | Erro interno |

A implementação real deve prevalecer sobre esta referência.

---

# Paginação

Quando suportada, utilizar parâmetros padronizados.

Exemplo:

```text
?page=1
&pageSize=20
```

A resposta deve informar:

- página atual;
- quantidade por página;
- total de registros;
- total de páginas.

Caso exista outro padrão, documentá-lo.

---

# Filtros

Os filtros devem ser enviados por Query String.

Exemplo:

```text
?nome=produto
&ativo=true
&categoria=5
```

Filtros devem ser opcionais sempre que possível.

---

# Ordenação

A ordenação deve utilizar parâmetros específicos.

Exemplo:

```text
?sort=nome
&direction=asc
```

ou

```text
?orderBy=nome
&descending=false
```

Documentar o padrão efetivamente utilizado.

---

# Versionamento

Sempre utilizar versionamento da API.

Exemplo:

```text
/api/v1/
```

Caso exista configuração via ApiVersion, documentar:

- versões disponíveis;
- estratégia de versionamento;
- compatibilidade.

---

# Boas Práticas

Sempre:

- utilizar métodos assíncronos;
- retornar ActionResult<T>;
- utilizar DTOs;
- validar ModelState;
- documentar via Swagger;
- manter Controllers pequenos;
- utilizar Dependency Injection.

Evitar:

- regras de negócio em Controllers;
- acesso direto ao banco;
- exposição de entidades;
- tratamento manual de exceções repetitivo.

---

# Limitações Conhecidas

Este documento foi elaborado com base na arquitetura identificada no projeto.

As convenções específicas das APIs `agilium-manager-azure-api` e `agilium-pdv-azure-api` deverão ser revisadas e complementadas após análise detalhada de seus Controllers e configurações.

---

# Documentos Relacionados

- overview.md
- authentication.md
- authorization.md
- errors.md
- examples.md
- ../architecture/layers.md
- ../architecture/dependency-flow.md
- ../patterns/notification-pattern.md
- ../patterns/dependency-injection.md

---

## Controller Conventions

> **TODO:** Document controller naming, route attributes, action naming, and parameter conventions.

---

## Request/Response Format

> **TODO:** Document the standard request and response JSON format, including envelope patterns if used.

---

## Error Handling

> **TODO:** Document the standard error response format, HTTP status codes mapping, and `ExceptionMiddleware` integration.

---

## Pagination

> **TODO:** Document pagination conventions — query parameters, response headers, and metadata.

---

## Filtering and Sorting

> **TODO:** Document filtering and sorting conventions.

---

## Related Documents
- [API Overview](./overview.md)
- [Errors](./errors.md)
- [Examples](./examples.md)
