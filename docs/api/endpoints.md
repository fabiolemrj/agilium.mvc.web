# Endpoints da API

## Objetivo

Catalogar todos os endpoints REST disponibilizados pelas APIs do Agilium Manager, documentando sua finalidade, parâmetros, autenticação, respostas e regras de negócio.

Este documento deve ser atualizado automaticamente sempre que novos endpoints forem adicionados ou alterados.

---

# Escopo

Este documento contempla:

- Controllers
- Endpoints
- Métodos HTTP
- Rotas
- Parâmetros
- DTOs de Entrada
- DTOs de Saída
- Códigos HTTP
- Regras de Autorização
- Observações de Negócio

---

# Fontes para Análise

Antes de atualizar este documento, analisar:

- Todos os Controllers da API
- Atributos `[HttpGet]`
- `[HttpPost]`
- `[HttpPut]`
- `[HttpDelete]`
- `[Route]`
- `[ApiController]`
- DTOs
- Models
- Services
- Swagger
- Versionamento
- Autorização

---

# Índice

- Catálogo de Endpoints
- Estrutura de Documentação
- Convenções
- Organização por Controller
- Boas Práticas
- Documentos Relacionados

---

# Catálogo de Endpoints

Cada Controller deve possuir sua própria seção.

Exemplo:

## ProdutoController

| Método | Rota | Descrição | Autenticação |
|---------|------|-----------|--------------|
| GET | /api/v1/produtos | Lista produtos | Sim |
| GET | /api/v1/produtos/{id} | Consulta produto | Sim |
| POST | /api/v1/produtos | Cadastra produto | Sim |
| PUT | /api/v1/produtos/{id} | Atualiza produto | Sim |
| DELETE | /api/v1/produtos/{id} | Remove produto | Sim |

---

## ClienteController

| Método | Rota | Descrição | Autenticação |
|---------|------|-----------|--------------|
| GET | /api/v1/clientes | Lista clientes | Sim |
| POST | /api/v1/clientes | Cadastra cliente | Sim |

---

> **Importante**
>
> Os exemplos acima são ilustrativos.
> O agente deve substituir pelas rotas reais encontradas durante a análise do código-fonte.

---

# Estrutura de Documentação

Cada endpoint deve conter, no mínimo, as seguintes informações.

## Identificação

- Controller
- Action
- Método HTTP
- Rota
- Versão da API

---

## Objetivo

Descrição funcional do endpoint.

---

## Autenticação

Informar:

- Requer autenticação
- Tipo de autenticação
- Permissões necessárias
- Policies
- Claims
- Roles

---

## Parâmetros

### Path Parameters

| Nome | Tipo | Obrigatório | Descrição |
|------|------|-------------|-----------|

---

### Query Parameters

| Nome | Tipo | Obrigatório | Descrição |
|------|------|-------------|-----------|

---

### Headers

| Header | Obrigatório | Descrição |
|---------|-------------|-----------|

---

## Corpo da Requisição

Informar o DTO utilizado.

Exemplo:

```json
{
  "nome": "",
  "descricao": "",
  "ativo": true
}
```

---

## Respostas

Documentar todas as respostas possíveis.

| Código | Situação |
|----------|----------|
| 200 | Sucesso |
| 201 | Criado |
| 204 | Sem conteúdo |
| 400 | Requisição inválida |
| 401 | Não autenticado |
| 403 | Acesso negado |
| 404 | Não encontrado |
| 409 | Conflito |
| 422 | Erro de validação |
| 500 | Erro interno |

---

## Exemplo de Resposta

```json
{
  "success": true,
  "data": {}
}
```

Caso a API utilize outro padrão, documentá-lo.

---

## Regras de Negócio

Descrever:

- validações;
- dependências;
- restrições;
- comportamento esperado.

---

## Serviços Utilizados

Relacionar os Services chamados pelo endpoint.

Exemplo:

- ProdutoService
- ClienteService

---

## DTOs Utilizados

Relacionar:

- DTO de Entrada
- DTO de Saída

---

## Observações

Registrar particularidades do endpoint.

---

# Convenções

Durante a documentação:

- utilizar sempre a rota completa;
- identificar o Controller responsável;
- documentar todos os parâmetros;
- documentar todos os códigos HTTP;
- informar se exige autenticação;
- informar permissões necessárias;
- incluir exemplos de Request e Response.

---

# Organização

Os endpoints devem ser agrupados por Controller.

Exemplo:

```
ProdutoController
    GET
    POST
    PUT
    DELETE

ClienteController
    GET
    POST

VendaController
...
```

---

# Boas Práticas

Sempre documentar:

- rota completa;
- método HTTP;
- autenticação;
- autorização;
- parâmetros;
- DTOs;
- exemplos;
- regras de negócio;
- respostas.

Evitar documentação incompleta.

---

# Atualização

Sempre que um novo endpoint for criado:

- adicionar ao catálogo;
- documentar Request;
- documentar Response;
- atualizar exemplos;
- revisar documentação relacionada.

---

# Limitações Conhecidas

O levantamento técnico atualmente disponível foi realizado sobre o projeto **agilum.mvc.web**.

A catalogação completa dos endpoints depende da análise dos projetos:

- agilium-manager-azure-api
- agilium-pdv-azure-api

Após essa análise, este documento deverá ser atualizado automaticamente com todas as rotas implementadas.

---

# Documentos Relacionados

- overview.md
- authentication.md
- authorization.md
- conventions.md
- errors.md
- examples.md
- ../templates/endpoint-template.md