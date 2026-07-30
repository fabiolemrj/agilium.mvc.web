# Exemplos da API

## Objetivo

Documentar exemplos práticos de utilização das APIs do Agilium Manager, demonstrando como consumir os endpoints, interpretar as respostas e tratar situações de erro.

Todos os exemplos devem refletir fielmente a implementação existente no código-fonte e permanecer sincronizados com a documentação dos endpoints.

---

# Escopo

Este documento contempla:

- Exemplos de autenticação
- Exemplos de consultas (GET)
- Exemplos de inclusão (POST)
- Exemplos de atualização (PUT)
- Exemplos de exclusão (DELETE)
- Exemplos de paginação
- Exemplos de filtros
- Exemplos de ordenação
- Exemplos de erros
- Exemplos utilizando diferentes ferramentas

---

# Fontes para Análise

Antes de atualizar este documento, analisar:

- Controllers
- Endpoints
- Swagger/OpenAPI
- DTOs
- Models
- Services
- Responses
- Middleware
- Autenticação
- Autorização

---

# Índice

- Autenticação
- Consultas
- Inclusão
- Atualização
- Exclusão
- Paginação
- Filtros
- Ordenação
- Tratamento de Erros
- Ferramentas
- Boas Práticas
- Documentos Relacionados

---

# Autenticação

## Objetivo

Demonstrar como autenticar e consumir endpoints protegidos.

### Exemplo de Login

> **TODO**
>
> Documentar utilizando o endpoint real identificado durante a análise da API.

### Exemplo de Requisição Autenticada

```http
GET /api/v1/recurso
Authorization: Bearer {token}
```

> Atualizar conforme o mecanismo de autenticação implementado.

---

# Consultas (GET)

Cada exemplo deve conter:

- Endpoint
- Headers
- Query String
- Resposta
- Observações

Exemplo:

```http
GET /api/v1/produtos
```

```json
{
    "data": []
}
```

> Substituir pelo endpoint real.

---

# Inclusão (POST)

Cada exemplo deve conter:

- Endpoint
- Body
- Resposta
- Código HTTP

Exemplo:

```http
POST /api/v1/produtos
```

```json
{
    "nome": "Produto Exemplo",
    "valor": 10.00
}
```

Resposta:

```json
{
    "id": 1
}
```

---

# Atualização (PUT)

Exemplo:

```http
PUT /api/v1/produtos/1
```

```json
{
    "nome": "Produto Alterado"
}
```

---

# Exclusão (DELETE)

Exemplo:

```http
DELETE /api/v1/produtos/1
```

Resposta esperada:

```http
204 No Content
```

---

# Paginação

Quando suportada pela API, documentar exemplos.

Requisição:

```http
GET /api/v1/produtos?page=1&pageSize=20
```

Resposta:

```json
{
    "page": 1,
    "pageSize": 20,
    "total": 100,
    "items": []
}
```

Caso a API utilize outro formato, documentá-lo.

---

# Filtros

Exemplo:

```http
GET /api/v1/produtos?ativo=true&categoria=10
```

---

# Ordenação

Exemplo:

```http
GET /api/v1/produtos?sort=nome&direction=asc
```

---

# Tratamento de Erros

Cada exemplo deve conter:

- cenário;
- requisição;
- resposta;
- código HTTP.

### Recurso não encontrado

```http
404 Not Found
```

```json
{
    "errors": [
        "Recurso não encontrado."
    ]
}
```

---

### Dados inválidos

```http
400 Bad Request
```

```json
{
    "errors": [
        "Campo obrigatório."
    ]
}
```

---

### Não autenticado

```http
401 Unauthorized
```

---

### Sem permissão

```http
403 Forbidden
```

---

### Erro interno

```http
500 Internal Server Error
```

---

# Ferramentas

Sempre que possível, fornecer exemplos utilizando:

## cURL

```bash
curl -X GET \
https://api.exemplo.com/api/v1/produtos
```

---

## HTTPie

```bash
http GET https://api.exemplo.com/api/v1/produtos
```

---

## Postman

Sempre informar:

- Método
- URL
- Headers
- Body
- Variáveis de ambiente

---

## Swagger

Quando disponível, utilizar exemplos gerados pelo Swagger como referência.

---

# Boas Práticas

Os exemplos devem:

- utilizar endpoints reais;
- utilizar DTOs reais;
- conter respostas reais;
- apresentar códigos HTTP corretos;
- utilizar autenticação quando necessária;
- estar sincronizados com a documentação dos endpoints.

Evitar exemplos fictícios que não representem a implementação da API.

---

# Atualização

Sempre que um endpoint for criado ou alterado:

- revisar este documento;
- atualizar exemplos;
- validar respostas;
- revisar códigos HTTP;
- sincronizar com a documentação dos endpoints.

---

# Limitações Conhecidas

O levantamento técnico atualmente disponível concentrou-se no projeto **agilum.mvc.web**.

Os exemplos definitivos dependem da análise dos projetos:

- agilium-manager-azure-api
- agilium-pdv-azure-api

Até que essa análise seja concluída, os exemplos apresentados neste documento devem ser considerados apenas como modelos de documentação.

---

# Documentos Relacionados

- endpoints.md
- authentication.md
- authorization.md
- conventions.md
- errors.md
- overview.md
- ../templates/endpoint-template.md