# Glossary

## Objetivo

Este documento reúne os principais termos técnicos, arquiteturais e de negócio utilizados no **Agilium Manager**.

Seu objetivo é facilitar o entendimento da documentação e padronizar a terminologia utilizada entre desenvolvedores, arquitetos e agentes de IA.

Sempre que um termo não estiver claro durante uma implementação, consulte este glossário antes de prosseguir.

---

# Como Utilizar

Durante a leitura da documentação:

- Consulte este glossário sempre que encontrar um termo desconhecido.
- Utilize os termos padronizados ao escrever documentação.
- Evite criar novos termos para conceitos já existentes.
- Mantenha o vocabulário consistente em toda a solução.

---

# Termos de Arquitetura

## ADR (Architecture Decision Record)

Documento que registra uma decisão arquitetural importante do projeto.

Cada ADR descreve:

- Contexto
- Problema
- Decisão
- Consequências
- Alternativas

Documentação:

```text
docs/decisions/
```

---

## Layered Architecture

Arquitetura organizada em camadas independentes.

Fluxo:

```text
Presentation

↓

Application

↓

Domain

↓

Repository

↓

Persistence
```

---

## Domain

Camada responsável pelas regras de negócio.

Contém:

- Entidades
- Value Objects
- Domain Services
- Aggregates
- Domain Events

---

## Application Service

Camada responsável por orquestrar casos de uso.

Não deve conter regras de negócio complexas.

---

## Repository

Responsável pela persistência das entidades do domínio.

Não implementa regras de negócio.

---

## Persistence

Camada responsável por:

- DbContext
- Fluent API
- Migrations
- Configuração do banco

---

## Infrastructure

Camada que implementa integrações externas.

Exemplos:

- E-mail
- Cache
- Storage
- APIs externas
- Mensageria

---

# Termos de Domínio

## Entity

Objeto do domínio que possui identidade própria.

Exemplos:

- Cliente
- Produto
- Pedido
- Venda

---

## Value Object

Objeto imutável identificado pelo seu valor.

Exemplos:

- Endereço
- Documento
- Dinheiro

---

## Aggregate

Conjunto de entidades relacionadas que devem manter consistência.

Possui um **Aggregate Root** responsável pelas alterações.

---

## Aggregate Root

Entidade principal de um Aggregate.

Toda alteração deve ocorrer através dela.

---

## Domain Service

Serviço responsável por regras que envolvem múltiplas entidades.

---

## Domain Event

Evento importante ocorrido dentro do domínio.

Exemplos:

- VendaRealizada
- PedidoCancelado

---

## Specification

Classe responsável por encapsular regras reutilizáveis.

---

# Termos de Persistência

## Entity Framework Core

ORM principal utilizado pelo projeto.

---

## Dapper

Micro ORM utilizado para consultas específicas e de alta performance.

---

## Migration

Histórico versionado das alterações estruturais do banco de dados.

---

## Fluent API

Forma oficial de configurar entidades e relacionamentos utilizando o Entity Framework Core.

---

## Soft Delete

Estratégia onde registros são marcados como excluídos sem remoção física do banco.

---

## Auditoria

Registro automático das operações realizadas sobre os dados.

Exemplos:

- DataCadastro
- UsuarioCadastro
- DataAlteracao
- UsuarioAlteracao

---

# Termos de API

## DTO (Data Transfer Object)

Objeto utilizado para comunicação entre clientes e APIs.

---

## Endpoint

Recurso disponibilizado por uma API.

Exemplo:

```text
GET /api/clientes
```

---

## Request

Dados enviados pelo cliente para a API.

---

## Response

Dados retornados pela API.

---

## Middleware

Componente responsável por interceptar o processamento das requisições HTTP.

---

## Versionamento

Estratégia para manter compatibilidade entre diferentes versões da API.

---

# Termos de Desenvolvimento

## Dependency Injection (DI)

Técnica utilizada para fornecer dependências entre componentes sem acoplamento direto.

---

## SOLID

Conjunto de princípios para desenvolvimento orientado a objetos:

- Single Responsibility
- Open/Closed
- Liskov Substitution
- Interface Segregation
- Dependency Inversion

---

## Clean Code

Práticas voltadas para tornar o código:

- Simples
- Legível
- Manutenível
- Testável

---

## Refatoração

Processo de melhoria da estrutura do código sem alterar seu comportamento funcional.

---

## Code Review

Processo de revisão do código antes da integração ao repositório principal.

---

# Termos de Testes

## Teste Unitário

Valida o comportamento isolado de uma unidade de código.

---

## Teste de Integração

Valida a interação entre componentes da aplicação.

---

## Teste End-to-End (E2E)

Valida um fluxo completo do sistema do ponto de vista do usuário.

---

## Cobertura de Testes

Métrica que indica quanto do código é exercitado pelos testes automatizados.

---

# Termos de Infraestrutura

## Docker

Tecnologia utilizada para empacotar aplicações em contêineres.

---

## Container

Ambiente isolado onde uma aplicação é executada.

---

## Ambiente

Contexto de execução da aplicação.

Exemplos:

- Desenvolvimento
- Homologação
- Produção

---

## CI/CD

Processo automatizado de integração, testes e implantação contínua.

---

# Termos de Negócio

Os conceitos específicos do domínio (como Cliente, Empresa, Pedido, Venda, Caixa, Licenciamento etc.) devem ser documentados nos respectivos módulos em:

```text
docs/business/
```

e

```text
docs/business-rules/
```

---

# Siglas Utilizadas

| Sigla | Significado |
|--------|-------------|
| ADR | Architecture Decision Record |
| API | Application Programming Interface |
| CI | Continuous Integration |
| CD | Continuous Delivery / Deployment |
| CRUD | Create, Read, Update, Delete |
| DI | Dependency Injection |
| DTO | Data Transfer Object |
| E2E | End-to-End |
| EF Core | Entity Framework Core |
| HTTP | HyperText Transfer Protocol |
| JSON | JavaScript Object Notation |
| JWT | JSON Web Token |
| ORM | Object-Relational Mapping |
| REST | Representational State Transfer |
| SOLID | Princípios de Design Orientado a Objetos |
| SQL | Structured Query Language |
| UI | User Interface |
| UUID | Universally Unique Identifier |

---

# Documentação Relacionada

| Assunto | Documento |
|----------|-----------|
| Arquitetura | knowledge/architecture.md |
| APIs | knowledge/api.md |
| Banco de Dados | knowledge/database.md |
| Domínio | knowledge/domain.md |
| Desenvolvimento | knowledge/development.md |
| Padrões | knowledge/patterns.md |
| Regras de Negócio | knowledge/business-rules.md |
| Decisões Arquiteturais | knowledge/decisions.md |

---

# Documentação Oficial

Para detalhes sobre cada conceito, consulte a documentação correspondente em:

```text
docs/
```

Cada diretório contém a definição completa dos conceitos aplicáveis ao seu contexto.

---

# Fluxo Recomendado para Agentes de IA

```text
Encontrar um termo desconhecido

↓

Consultar glossary.md

↓

Localizar a documentação relacionada

↓

Compreender o contexto

↓

Prosseguir com a implementação
```

---

# Resumo

Este documento centraliza a terminologia utilizada no Agilium Manager.

Seu objetivo é garantir uma linguagem comum entre documentação, código e desenvolvimento, reduzindo ambiguidades e facilitando a navegação pelos demais documentos da base de conhecimento.