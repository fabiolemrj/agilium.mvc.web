# Project Overview

## Objetivo

Este documento fornece uma visão geral do **Agilium Manager**, apresentando sua arquitetura, objetivos, tecnologias, módulos e organização da documentação.

Seu propósito é permitir que novos desenvolvedores e agentes de IA compreendam rapidamente o projeto antes de iniciar qualquer análise ou implementação.

Este é o **primeiro documento** que deve ser consultado por qualquer pessoa ou agente de IA.

---

# O que é o Agilium Manager

O **Agilium Manager** é uma plataforma de gestão composta por aplicações Web, APIs e serviços responsáveis por administrar processos de negócio utilizados pelos sistemas da suíte Agilium.

A solução foi projetada utilizando uma arquitetura em camadas, com forte separação entre domínio, aplicação, infraestrutura e persistência, priorizando:

- Manutenibilidade
- Escalabilidade
- Reutilização
- Testabilidade
- Baixo acoplamento
- Alta coesão

---

# Objetivos da Solução

Os principais objetivos do projeto são:

- Centralizar regras de negócio.
- Disponibilizar APIs para integração.
- Suportar múltiplos módulos de negócio.
- Facilitar evolução contínua.
- Garantir consistência arquitetural.
- Manter documentação integrada ao código.

---

# Arquitetura

A solução segue uma **Arquitetura em Camadas (Layered Architecture)**.

```text
Presentation
(MVC / APIs)

↓

Application

↓

Domain

↓

Repository

↓

Persistence

↓

Database
```

Cada camada possui responsabilidades específicas e bem definidas.

Consulte:

```text
knowledge/architecture.md
```

---

# Tecnologias

As tecnologias utilizadas podem variar entre projetos da solução, porém normalmente incluem:

## Backend

- ASP.NET Core
- C#
- Entity Framework Core
- Dapper

## Banco de Dados

- MySQL
- SQL Server (quando aplicável)

## Frontend

- ASP.NET MVC
- Razor
- JavaScript
- TypeScript
- Bootstrap

## Infraestrutura

- Docker
- GitHub
- CI/CD
- Logging
- Monitoramento

Consulte:

```text
docs/architecture/
```

---

# Estrutura da Solução

A organização da solução segue aproximadamente a estrutura abaixo.

```text
src/

MVC/

API/

Application/

Domain/

Repository/

Persistence/

Infrastructure/

tests/

docs/

.ai/
```

A estrutura detalhada encontra-se na documentação oficial.

---

# Organização da Documentação

A documentação oficial está organizada em:

```text
docs/

api/
architecture/
business/
business-rules/
contribuicao/
database/
decisions/
development/
diagrams/
domain/
fluxos/
frontend/
patterns/
prompts/
templates/
```

Cada diretório documenta um aspecto específico da solução.

---

# Base de Conhecimento (.ai)

A pasta `.ai/knowledge/` contém uma versão resumida da documentação, otimizada para desenvolvedores e agentes de IA.

Seu objetivo é:

- Facilitar navegação.
- Reduzir contexto necessário.
- Direcionar para a documentação oficial.
- Resumir conceitos importantes.

A documentação oficial continua sendo a fonte de verdade.

---

# Módulos de Negócio

Os módulos funcionais são documentados em:

```text
docs/business/
```

Exemplos:

- Clientes
- Empresas
- Usuários
- Produtos
- Estoque
- Caixa
- Pedidos
- Vendas
- Financeiro
- Licenciamento

Cada módulo possui documentação própria.

---

# Regras de Negócio

As regras funcionais encontram-se em:

```text
docs/business-rules/
```

Toda implementação deve consultar as regras correspondentes antes de alterar o comportamento do sistema.

Consulte também:

```text
knowledge/business-rules.md
```

---

# Fluxos

Os principais processos de negócio encontram-se documentados em:

```text
docs/fluxos/
```

Exemplos:

- Login
- Venda
- Pedido
- Caixa
- Estoque
- Pagamentos
- Licenciamento

Consulte:

```text
knowledge/fluxos.md
```

---

# Arquitetura e ADRs

As decisões arquiteturais são registradas por meio de **Architecture Decision Records (ADRs)**.

Localização:

```text
docs/decisions/
```

Antes de qualquer alteração estrutural, consulte:

```text
knowledge/decisions.md
```

---

# Processo de Desenvolvimento

Todo desenvolvimento deve seguir o fluxo abaixo.

```text
Receber Requisito

↓

Analisar Contexto

↓

Consultar Documentação

↓

Consultar ADRs

↓

Planejar

↓

Implementar

↓

Criar Testes

↓

Atualizar Documentação

↓

Code Review

↓

Merge
```

Consulte:

```text
knowledge/development.md
```

---

# Padrões Utilizados

A solução utiliza diversos padrões arquiteturais e de projeto, incluindo:

- Layered Architecture
- Repository Pattern
- Service Layer
- Dependency Injection
- Notification Pattern
- Specification Pattern
- Soft Delete
- Auditoria
- Clean Code
- SOLID

Consulte:

```text
knowledge/patterns.md
```

---

# Como Navegar pela Documentação

Dependendo da atividade, consulte os documentos correspondentes.

| Objetivo | Documento |
|----------|-----------|
| Entender a arquitetura | `knowledge/architecture.md` |
| Criar APIs | `knowledge/api.md` |
| Alterar banco de dados | `knowledge/database.md` |
| Implementar regras de negócio | `knowledge/business-rules.md` |
| Compreender o domínio | `knowledge/domain.md` |
| Seguir padrões | `knowledge/patterns.md` |
| Desenvolver funcionalidades | `knowledge/development.md` |
| Consultar decisões arquiteturais | `knowledge/decisions.md` |
| Entender fluxos | `knowledge/fluxos.md` |
| Utilizar templates | `knowledge/templates.md` |
| Utilizar prompts | `knowledge/prompts.md` |
| Contribuir com o projeto | `knowledge/contribuicao.md` |

---

# Fluxo Recomendado para Novos Desenvolvedores

```text
Project Overview

↓

Architecture

↓

Domain

↓

Business Rules

↓

Patterns

↓

Development

↓

Decisions

↓

Fluxos

↓

Módulo específico
```

---

# Fluxo Recomendado para Agentes de IA

```text
Ler project-overview.md

↓

Identificar o objetivo da tarefa

↓

Consultar architecture.md

↓

Consultar decisions.md

↓

Consultar documentação específica

↓

Planejar implementação

↓

Executar alterações

↓

Criar ou atualizar testes

↓

Atualizar documentação
```

---

# Documentação Oficial

A documentação completa encontra-se em:

```text
docs/
```

Sempre utilize os documentos oficiais como fonte principal para:

- Arquitetura
- APIs
- Banco de Dados
- Domínio
- Fluxos
- Regras de Negócio
- ADRs
- Diagramas
- Templates
- Prompts

---

# Resumo

O **Agilium Manager** é uma plataforma modular construída sobre uma arquitetura em camadas, com forte foco em organização, reutilização e manutenção.

Este documento é o ponto de entrada da base de conhecimento e deve ser consultado antes de qualquer atividade técnica. A partir dele, desenvolvedores e agentes de IA podem navegar para os documentos específicos de arquitetura, domínio, regras de negócio, padrões, desenvolvimento e demais áreas da solução.