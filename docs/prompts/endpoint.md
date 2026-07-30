# Prompt: Endpoint

# Objetivo

Template para criação, alteração ou documentação de endpoints do Agilium Manager.

Este prompt deve ser utilizado para garantir que novos endpoints sigam a arquitetura da solução e os padrões já estabelecidos.

---

# Quando utilizar

Utilize este prompt para:

- criar novos endpoints;
- alterar endpoints existentes;
- revisar endpoints;
- documentar APIs;
- refatorar endpoints.

---

# Prompt

```text
Crie ou altere o endpoint abaixo:

Projeto:

[NOME_API]

Endpoint:

[MÉTODO] [ROTA]

Antes de implementar qualquer alteração, faça um levantamento da funcionalidade relacionada.

---

## 1. Especificação

Definir:

- método HTTP;
- rota;
- finalidade;
- Controller responsável;
- Action;
- autenticação necessária;
- autorização necessária;
- integração com outros módulos.

---

## 2. Fluxo Arquitetural

Garantir que o endpoint siga o fluxo:

Controller

↓

Service

↓

Repository

↓

Entity Framework Core / Dapper

↓

Banco de Dados

Não implementar regras de negócio diretamente no Controller.

---

## 3. Request

Documentar:

Headers

Path Parameters

Query Parameters

Request Body

ViewModel utilizado

Validações

---

## 4. Response

Documentar:

Status HTTP

Model/ViewModel retornado

Mensagens de erro

Notificações de negócio

Exemplos de resposta

---

## 5. Implementação

Verificar:

Controller

Service

Repository

ViewModels

AutoMapper

Notification Pattern

FluentValidation

Dependency Injection

Unit of Work

---

## 6. Segurança

Verificar:

Authorize

ClaimsAuthorize

Identity

Validação dos dados

Proteção de informações sensíveis

---

## 7. Persistência

Verificar:

Entity Framework Core

Dapper

Repository Pattern

Transações (quando aplicável)

---

## 8. Impacto

Identificar impacto em:

Controllers

Services

Repositories

Banco de Dados

ViewModels

Integrações

Outros módulos

---

## 9. Documentação

Atualizar a documentação da API.

Quando aplicável incluir:

Objetivo

Fluxo

Request

Response

Status HTTP

Regras de Negócio

Dependências

Limitações

---

## 10. Resultado

Apresentar:

Arquivos alterados

Classes alteradas

Métodos alterados

Fluxo da funcionalidade

Impactos

Riscos

Recomendações
```

---

# Parâmetros

| Parâmetro | Descrição | Exemplo |
|-----------|-----------|---------|
| `NOME_API` | Projeto da API | `agilium-manager-azure-api` |
| `MÉTODO` | Método HTTP | `GET`, `POST`, `PUT`, `DELETE` |
| `ROTA` | Endpoint | `/api/produtos/{id}/historico-precos` |

---

# Resultado Esperado

O endpoint deve:

- seguir a arquitetura em camadas do Agilium Manager;
- manter Controllers responsáveis apenas pela orquestração da requisição;
- utilizar Services para regras de negócio;
- utilizar Repositories para acesso aos dados;
- utilizar AutoMapper para conversão entre Models e ViewModels;
- utilizar FluentValidation e Notification Pattern para validações de domínio;
- respeitar os padrões de autenticação, autorização e tratamento de exceções da solução;
- manter a documentação da API atualizada e consistente com os demais documentos do projeto.