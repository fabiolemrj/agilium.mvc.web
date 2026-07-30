# Endpoint Template

# Objetivo

Template padrão para documentação de endpoints do Agilium Manager.

Cada documento deve representar exclusivamente um endpoint existente na solução e documentar seu funcionamento, fluxo arquitetural, regras de negócio e integrações.

Não documentar endpoints planejados ou ainda não implementados.

---

# [MÉTODO HTTP] [ROTA]

| Campo | Valor |
|--------|-------|
| **Projeto/API** | |
| **Controller** | |
| **Action** | |
| **Versão** | |
| **Autenticação** | |
| **Autorização** | |
| **Status** | Ativo / Obsoleto |

---

# Objetivo

Descrever a finalidade do endpoint.

---

# Contexto

Informar:

- módulo ao qual pertence;
- funcionalidade;
- fluxo em que é utilizado;
- integrações relacionadas.

---

# Fluxo Arquitetural

Documentar o fluxo completo da requisição.

Exemplo:

```
Cliente

↓

Controller

↓

Service

↓

Repository

↓

Entity Framework Core / Dapper

↓

Banco de Dados

↓

Response
```

---

# Componentes Envolvidos

| Camada | Componente |
|---------|------------|
| Controller | |
| Service | |
| Repository | |
| ViewModel | |
| Entity | |
| AutoMapper | |
| FluentValidation | |
| Notification Pattern | |

---

# Request

## Headers

| Header | Obrigatório | Descrição |
|---------|-------------|-----------|
| | | |

---

## Path Parameters

| Nome | Tipo | Obrigatório | Descrição |
|------|------|-------------|-----------|
| | | | |

---

## Query Parameters

| Nome | Tipo | Obrigatório | Valor Padrão | Descrição |
|------|------|-------------|--------------|-----------|
| | | | | |

---

## Request Body

Documentar o ViewModel utilizado.

```json
{
}
```

---

# Processamento

Descrever:

- validações;
- regras de negócio;
- consultas realizadas;
- persistência;
- integrações;
- notificações geradas.

---

# Response

## Sucesso

Documentar:

- Status HTTP;
- estrutura da resposta;
- ViewModel retornado.

```json
{
}
```

---

## Erros

| Status | Cenário | Comportamento |
|---------|----------|---------------|
| | | |

Documentar também:

- notificações;
- mensagens;
- validações.

---

# Regras de Negócio

Relacionar todas as regras aplicadas pelo endpoint.

Caso existam documentos específicos de regras de negócio, referenciá-los.

---

# Segurança

Documentar:

- autenticação;
- autorização;
- permissões necessárias;
- proteção de dados sensíveis.

---

# Dependências

Relacionar:

- Services;
- Repositories;
- AutoMapper;
- FluentValidation;
- Notification Pattern;
- Entity Framework Core;
- Dapper;
- APIs externas;
- outros módulos.

---

# Impacto

Informar quais componentes podem ser afetados por alterações neste endpoint.

Exemplos:

- Front-end;
- Aplicativos móveis;
- Integrações;
- Banco de Dados;
- Outros serviços.

---

# Limitações Conhecidas

Registrar:

- comportamentos ainda não confirmados;
- dependências externas;
- limitações da implementação.

---

# Documentação Relacionada

Relacionar documentos relevantes.

Exemplos:

- Arquitetura
- Regras de Negócio
- Fluxo da Funcionalidade
- Banco de Dados
- Endpoint relacionado

---

# Histórico

| Versão | Data | Alteração |
|---------|------|-----------|
| 1.0 | | Criação |