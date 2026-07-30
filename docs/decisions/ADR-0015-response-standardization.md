# ADR-0015 - Padronização das Respostas das APIs (API Response Standardization)

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager disponibiliza diversas APIs REST consumidas por diferentes aplicações da plataforma:

- Agilium Manager MVC
- Agilium PDV
- Agilium Mobile
- Cardápio Digital
- Serviços de Licenciamento
- Integrações externas

Durante a evolução da plataforma foram identificados diferentes formatos de resposta entre endpoints.

Exemplos encontrados:

- Objetos retornados diretamente;
- Listas simples;
- Objetos anônimos;
- Strings;
- Mensagens sem padronização;
- Respostas de erro inconsistentes.

Essa diversidade aumenta a complexidade dos clientes consumidores e dificulta a manutenção das APIs.

Era necessário definir um contrato único para todas as respostas públicas.

---

# Problema

Sem uma padronização de respostas:

- Cada endpoint retorna um formato diferente;
- O front-end precisa tratar diversos modelos;
- Integrações tornam-se mais complexas;
- Erros são inconsistentes;
- Documentação fica difícil de manter;
- Evolução da API torna-se arriscada.

Era necessário estabelecer um padrão único para respostas de sucesso e erro.

---

# Alternativas Consideradas

## Alternativa 1 — Retornar apenas o objeto solicitado

Exemplo:

```json
{
    "id": 10,
    "descricao": "Produto"
}
```

### Vantagens

- Simples.
- Pouco código.

### Desvantagens

- Não informa sucesso.
- Não possui mensagens.
- Não possui metadados.
- Não padroniza erros.

---

## Alternativa 2 — Respostas específicas por Controller

Cada Controller define seu próprio formato.

### Vantagens

- Flexibilidade.

### Desvantagens

- Inconsistência.
- Difícil manutenção.
- Alto acoplamento.

---

## Alternativa 3 — Envelope Padrão (Escolhida)

Toda resposta segue um contrato único.

### Vantagens

- Padronização.
- Facilidade para clientes.
- Melhor documentação.
- Evolução controlada.
- Melhor experiência para integrações.

### Desvantagens

- Pequeno aumento do tamanho da resposta.

---

# Decisão

Foi adotado um **Envelope de Resposta Padronizado** para todas as APIs do Agilium Manager.

Independentemente do recurso retornado, todas as respostas deverão seguir o mesmo contrato.

Esse padrão será utilizado para:

- APIs REST;
- Integrações;
- Serviços internos;
- Novos módulos da plataforma.

---

# Objetivos

Esta estratégia possui os seguintes objetivos:

- Padronizar respostas.
- Facilitar consumo da API.
- Simplificar documentação.
- Melhorar consistência.
- Facilitar evolução do contrato.
- Padronizar erros.

---

# Estrutura da Resposta

Toda resposta deverá seguir a seguinte estrutura:

```json
{
    "success": true,
    "status": 200,
    "message": null,
    "data": {},
    "errors": [],
    "timestamp": "2026-07-29T14:30:00Z",
    "correlationId": "4b8f6a9d-5cb5-4a6d-a6cb-0c3e0e7f4f6d"
}
```

---

# Campos

| Campo | Descrição |
|--------|-----------|
| success | Indica sucesso ou falha |
| status | Código HTTP |
| message | Mensagem amigável |
| data | Dados retornados |
| errors | Lista de erros |
| timestamp | Data/Hora UTC da resposta |
| correlationId | Identificador da requisição |

---

# Resposta de Sucesso

Exemplo:

```json
{
    "success": true,
    "status": 200,
    "message": "Produto encontrado.",
    "data": {
        "id": 10,
        "descricao": "Notebook"
    },
    "errors": [],
    "timestamp": "2026-07-29T14:30:00Z",
    "correlationId": "7f61c8c0-bb9f-47b5-9b87-57db07cf72df"
}
```

---

# Lista de Dados

```json
{
    "success": true,
    "status": 200,
    "message": null,
    "data": [
        {
            "id": 1,
            "descricao": "Produto A"
        },
        {
            "id": 2,
            "descricao": "Produto B"
        }
    ],
    "errors": []
}
```

---

# Paginação

Para consultas paginadas:

```json
{
    "success": true,
    "status": 200,
    "data": {
        "items": [],
        "page": 1,
        "pageSize": 20,
        "totalItems": 250,
        "totalPages": 13
    },
    "errors": []
}
```

---

# Resposta de Erro

```json
{
    "success": false,
    "status": 400,
    "message": "Falha de validação.",
    "data": null,
    "errors": [
        "Descrição é obrigatória.",
        "Empresa inválida."
    ],
    "timestamp": "2026-07-29T14:30:00Z",
    "correlationId": "5b1c6415-67af-40e0-a2d8-30c4e32b98dd"
}
```

---

# Códigos HTTP

| HTTP | Utilização |
|------|------------|
| 200 | Consulta realizada |
| 201 | Registro criado |
| 204 | Operação sem conteúdo |
| 400 | Erro de validação |
| 401 | Não autenticado |
| 403 | Acesso negado |
| 404 | Recurso não encontrado |
| 409 | Conflito |
| 422 | Regra de negócio |
| 500 | Erro interno |

---

# Regras

Toda resposta deverá:

- Utilizar o envelope padrão;
- Informar código HTTP correto;
- Nunca retornar exceções internas;
- Possuir mensagens amigáveis;
- Informar CorrelationId quando disponível.

---

# Integração com Notification Pattern

Quando houver erros de negócio:

```text
Notification

↓

Errors[]

↓

API Response
```

Não lançar exceções para erros de negócio.

---

# Integração com Exception Middleware

Exceções inesperadas serão convertidas automaticamente pelo Middleware Global.

Fluxo:

```text
Exception

↓

Exception Middleware

↓

API Response
```

---

# Organização

Criar um modelo único.

Exemplo:

```text
Application/

├── Responses/

│   ├── ApiResponse.cs

│   ├── ApiResponse<T>.cs

│   ├── PagedResponse.cs

│   └── ErrorResponse.cs
```

---

# Benefícios

- Contrato único.
- Melhor documentação.
- APIs previsíveis.
- Front-end simplificado.
- Integrações facilitadas.
- Evolução segura.

---

# Desvantagens

- Pequeno aumento do payload.
- Necessidade de adaptação de APIs existentes.

---

# Riscos

Caso esta estratégia não seja seguida:

- APIs inconsistentes.
- Clientes complexos.
- Documentação difícil.
- Erros diferentes entre endpoints.
- Maior custo de manutenção.

---

# Impacto

Esta decisão impacta:

- APIs
- MVC
- Mobile
- Cardápio Digital
- Integrações
- Swagger
- Front-end
- Middleware
- Notification Pattern

---

# Plano de Implementação

1. Criar `ApiResponse<T>`.
2. Criar `PagedResponse<T>`.
3. Atualizar Controllers.
4. Integrar com Notification Pattern.
5. Integrar com Exception Middleware.
6. Atualizar Swagger.
7. Atualizar documentação.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Todas as APIs retornam o envelope padrão.
- Erros seguem o mesmo contrato.
- O campo `success` está presente em todas as respostas.
- O código HTTP corresponde ao resultado da operação.
- `CorrelationId` é retornado quando disponível.
- As respostas são documentadas no Swagger.

---

# ADRs Relacionados

- ADR-0003 — Notification Pattern
- ADR-0007 — Estratégia de Validação
- ADR-0008 — Versionamento de APIs
- ADR-0011 — Service Layer
- ADR-0013 — Estratégia de Logging
- ADR-0014 — Tratamento Global de Exceções
- ADR-0017 — Estratégia de Auditoria

---

# Referências

- RFC 7807 — Problem Details for HTTP APIs
- Microsoft REST API Guidelines
- Microsoft — ASP.NET Core Web API Best Practices
- RESTful Web APIs — Leonard Richardson
- JSON:API Specification

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | **2026-07-29** | Criação da ADR definindo um envelope padronizado para todas as respostas das APIs do Agilium Manager, estabelecendo um contrato único para respostas de sucesso, erro, paginação e integração com o Notification Pattern e o Middleware Global de Exceções. |