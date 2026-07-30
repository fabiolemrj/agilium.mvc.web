# Visão Geral das APIs

## Objetivo

Apresentar uma visão geral da arquitetura das APIs do ecossistema **Agilium Manager**, descrevendo sua finalidade, organização, princípios de desenvolvimento e integração com os demais projetos da solução.

Este documento serve como ponto de entrada para compreender como as APIs estão organizadas e como elas se relacionam com as demais camadas da aplicação.

---

# Escopo

Este documento contempla:

- Projetos de API
- Arquitetura Geral
- Objetivos das APIs
- Organização da Solução
- Fluxo de Requisição
- Princípios REST
- Estrutura das URLs
- Versionamento
- Ambientes
- Integrações
- Documentação Relacionada

---

# Fontes para Análise

Antes de atualizar este documento analisar:

- agilium-manager-azure-api
- agilium-pdv-azure-api
- Startup.cs
- Program.cs
- Controllers
- Middleware
- Swagger
- Configuração de Versionamento
- Dependency Injection
- Arquitetura da Solução

---

# Índice

- Projetos
- Arquitetura
- Fluxo de Requisição
- Princípios REST
- Estrutura das URLs
- Versionamento
- Ambientes
- Integrações
- Tecnologias
- Documentação Relacionada

---

# Projetos

Atualmente o ecossistema Agilium possui duas APIs principais.

| Projeto | Finalidade | Consumidores |
|----------|------------|--------------|
| **agilium-manager-azure-api** | API principal de gestão do sistema | Aplicações Web, Mobile e integrações |
| **agilium-pdv-azure-api** | API específica para operações de PDV | Aplicações de Frente de Caixa e integrações do PDV |

> A documentação detalhada de cada projeto deve ser mantida individualmente.

---

# Arquitetura

As APIs fazem parte da arquitetura em camadas do Agilium Manager.

```text
Clientes

    │

HTTP / HTTPS

    │

API REST

    │

Controllers

    │

Services

    │

Repositories

    │

Banco de Dados
```

As APIs seguem a separação entre:

- Apresentação
- Negócio
- Infraestrutura
- Persistência

---

# Fluxo de Requisição

De forma geral, uma requisição percorre as seguintes etapas:

```text
Cliente

      │

Middleware

      │

Autenticação

      │

Autorização

      │

Controller

      │

Service

      │

Repository

      │

Banco de Dados

      │

Resposta HTTP
```

---

# Princípios REST

As APIs devem seguir os princípios REST.

Principais diretrizes:

- utilização de recursos ao invés de ações;
- uso correto dos métodos HTTP;
- respostas padronizadas;
- utilização adequada dos códigos HTTP;
- operações sem estado (Stateless);
- comunicação utilizando JSON.

---

# Métodos HTTP

| Método | Finalidade |
|----------|------------|
| GET | Consultar recursos |
| POST | Criar recursos |
| PUT | Atualizar recursos |
| PATCH | Atualizações parciais (quando aplicável) |
| DELETE | Remover recursos |

---

# Estrutura das URLs

As rotas devem seguir uma estrutura consistente.

Exemplo:

```text
/api/v1/produtos

/api/v1/clientes

/api/v1/vendas
```

Caso exista outro padrão implementado, este documento deverá ser atualizado.

---

# Versionamento

As APIs devem utilizar versionamento para garantir compatibilidade entre versões.

Exemplo:

```text
/api/v1/

/api/v2/
```

A estratégia efetivamente utilizada deve ser documentada após análise da configuração da API.

---

# Ambientes

As URLs variam conforme o ambiente.

Exemplo:

| Ambiente | URL |
|-----------|-----|
| Desenvolvimento | https://localhost:{porta}/api |
| Homologação | Definir conforme ambiente |
| Produção | Definir conforme ambiente |

Os endereços oficiais devem ser documentados após validação da infraestrutura.

---

# Tecnologias

As APIs utilizam tecnologias compatíveis com o ecossistema Agilium Manager.

Principais componentes:

- ASP.NET Core
- Entity Framework Core
- Dependency Injection
- AutoMapper
- Notification Pattern
- Middleware Pipeline
- MySQL
- Dapper (quando aplicável)

A lista definitiva deve ser confirmada durante a análise dos projetos de API.

---

# Integrações

As APIs podem ser consumidas por:

- Aplicação Web
- Aplicações Mobile
- PDV
- Sistemas parceiros
- Serviços internos

Cada integração deve possuir documentação própria.

---

# Documentação Complementar

A documentação detalhada encontra-se distribuída nos seguintes documentos:

- Endpoints
- Autenticação
- Autorização
- Convenções
- Tratamento de Erros
- Exemplos
- Versionamento

---

# Limitações Conhecidas

O levantamento técnico atualmente disponível foi realizado principalmente sobre o projeto **agilum.mvc.web**, identificando a arquitetura geral da solução e seus componentes compartilhados.

As informações específicas das APIs **agilium-manager-azure-api** e **agilium-pdv-azure-api** deverão ser refinadas após a análise detalhada de seus Controllers, Middlewares, configurações e documentação Swagger.

---

# Atualização

Sempre que houver alterações nas APIs:

- atualizar este documento;
- revisar a documentação relacionada;
- validar exemplos;
- revisar endpoints;
- revisar autenticação e autorização;
- atualizar diagramas arquiteturais.

---

# Documentos Relacionados

- endpoints.md
- authentication.md
- authorization.md
- conventions.md
- errors.md
- examples.md
- versioning.md
- ../architecture/overview.md
- ../architecture/request-pipeline.md
- ../architecture/layers.md