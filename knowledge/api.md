# API

## Objetivo

Este documento fornece uma visão resumida da arquitetura e dos padrões das APIs do **Agilium Manager**.

A documentação oficial encontra-se em:

```text
docs/api/
```

Este documento serve como um guia rápido para desenvolvedores e agentes de IA, indicando como as APIs são organizadas, quais padrões devem ser seguidos e onde encontrar informações detalhadas.

---

# Visão Geral

As APIs do Agilium Manager são responsáveis por:

- Expor funcionalidades do sistema.
- Permitir integração entre aplicações.
- Disponibilizar serviços para aplicações Web, Mobile e sistemas terceiros.
- Centralizar regras de autenticação e autorização.
- Padronizar a comunicação entre clientes e servidor.

As APIs seguem arquitetura REST e utilizam JSON como formato padrão para troca de informações.

---

# Arquitetura

Fluxo simplificado de uma requisição.

```text
Cliente

↓

API

↓

Controller

↓

Application Service

↓

Domain

↓

Repository

↓

Persistence

↓

Database

↓

Response
```

Toda regra de negócio deve ser executada na camada de domínio ou de aplicação.

---

# Organização

As APIs normalmente são organizadas por módulos de negócio.

Exemplo:

```text
/api/

clientes/

produtos/

usuarios/

pedidos/

vendas/

caixa/

financeiro/

licenciamento/
```

A estrutura detalhada encontra-se em:

```text
docs/api/
```

---

# Responsabilidades

A camada de API deve ser responsável apenas por:

- Receber requisições.
- Validar dados de entrada.
- Autenticar usuários.
- Autorizar operações.
- Invocar Application Services.
- Retornar respostas padronizadas.

A camada de API **não deve**:

- Implementar regras de negócio.
- Acessar diretamente o banco de dados.
- Manipular entidades de persistência.

---

# Padrões

As APIs seguem os seguintes padrões:

- REST
- JSON
- Versionamento
- DTOs
- Dependency Injection
- Validação de entrada
- Tratamento global de exceções
- Resposta padronizada

Consulte:

```text
docs/patterns/
```

---

# Versionamento

As APIs utilizam versionamento para garantir compatibilidade entre clientes.

Exemplo:

```text
/api/v1/

/api/v2/
```

Novas versões devem ser criadas apenas quando houver alterações incompatíveis (breaking changes).

Consulte:

```text
docs/api/versioning.md
```

---

# Autenticação

O acesso às APIs é protegido por autenticação baseada em token.

Resumo:

- Login do usuário.
- Emissão de JWT.
- Validação do token em cada requisição.
- Expiração automática.
- Refresh Token (quando aplicável).

Consulte:

```text
docs/api/authentication.md
```

e

```text
docs/decisions/ADR-0005-*.md
```

---

# Autorização

O acesso aos recursos é controlado por permissões e políticas.

Exemplos:

```text
Produto.Visualizar

Produto.Alterar

Venda.Realizar

Financeiro.Alterar
```

Consulte:

```text
docs/api/authorization.md
```

---

# DTOs

As APIs utilizam DTOs para comunicação.

Objetivos:

- Não expor entidades do domínio.
- Isolar contratos da API.
- Facilitar versionamento.
- Reduzir acoplamento.

---

# Responses

As respostas devem seguir um padrão único.

Exemplo:

```json
{
    "success": true,
    "status": 200,
    "message": "",
    "data": {},
    "errors": []
}
```

Consulte:

```text
docs/decisions/ADR-0015-*.md
```

---

# Tratamento de Erros

As exceções são tratadas globalmente.

Os Controllers não devem implementar tratamento específico para erros de negócio.

Utilizar:

- Middleware global.
- Notification Pattern.
- Responses padronizadas.

Consulte:

```text
docs/decisions/ADR-0014-*.md
```

---

# Validações

As validações são distribuídas entre as camadas.

| Camada | Responsabilidade |
|---------|------------------|
| Controller | Validação de entrada |
| Application | Casos de uso |
| Domain | Regras de negócio |

---

# Integrações

As APIs podem ser consumidas por:

- Aplicação MVC
- Aplicações Mobile
- PDV
- Cardápio Digital
- Sistemas externos
- Integrações internas

Consulte:

```text
docs/api/

docs/integrations/
```

---

# Testes

As APIs devem possuir:

- Testes unitários.
- Testes de integração.
- Testes de autenticação.
- Testes de autorização.
- Testes de contratos.

Consulte:

```text
docs/testing/
```

---

# ADRs Relacionados

| Tema | ADR |
|------|-----|
| Arquitetura | ADR-0001 |
| Repository | ADR-0002 |
| Notification Pattern | ADR-0003 |
| Entity Framework | ADR-0004 |
| Autenticação | ADR-0005 |
| Autorização | ADR-0006 |
| Validação | ADR-0007 |
| Versionamento | ADR-0008 |
| Dependency Injection | ADR-0009 |
| Service Layer | ADR-0011 |
| Exception Handling | ADR-0014 |
| API Response | ADR-0015 |

Consulte:

```text
knowledge/decisions.md
```

---

# Antes de Criar um Endpoint

Verifique:

- O endpoint pertence ao módulo correto?
- Existe documentação do módulo?
- Existe regra de negócio documentada?
- Existe um DTO apropriado?
- Existe um Service responsável pelo caso de uso?
- Existe um ADR relacionado?
- A resposta segue o padrão oficial?

---

# Documentação Relacionada

| Assunto | Documento |
|----------|-----------|
| Arquitetura | knowledge/architecture.md |
| Domínio | knowledge/domain.md |
| Banco de Dados | knowledge/database.md |
| Regras de Negócio | knowledge/business-rules.md |
| Desenvolvimento | knowledge/development.md |
| Padrões | knowledge/patterns.md |
| ADRs | knowledge/decisions.md |

---

# Documentação Oficial

Para detalhes completos, consulte:

```text
docs/api/

authentication.md

authorization.md

endpoints.md

dtos.md

responses.md

versioning.md

swagger.md

security.md
```

---

# Fluxo Recomendado para Agentes de IA

```text
Ler api.md

↓

Consultar decisions.md

↓

Identificar o módulo

↓

Consultar as regras de negócio

↓

Consultar a documentação oficial

↓

Implementar o endpoint

↓

Criar ou atualizar testes

↓

Atualizar documentação
```

---

# Resumo

Este documento fornece uma visão geral da camada de API do Agilium Manager.

Antes de implementar qualquer endpoint:

- siga os padrões REST adotados pelo projeto;
- utilize DTOs para comunicação;
- respeite a arquitetura em camadas;
- consulte os ADRs relacionados;
- utilize a documentação oficial em `docs/api/` como fonte de verdade.