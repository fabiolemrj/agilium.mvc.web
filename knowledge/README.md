# Agilium Manager - AI Knowledge Base

Bem-vindo à **Knowledge Base** do Agilium Manager.

Esta pasta contém uma base de conhecimento otimizada para consumo por agentes de Inteligência Artificial (IA), como GitHub Copilot, ChatGPT, Claude e outros assistentes de desenvolvimento.

> **Importante**
>
> A pasta **knowledge** **não substitui** a documentação oficial localizada em `docs/`.
>
> Ela funciona como um **índice inteligente**, contendo resumos, mapas de navegação e referências para que agentes de IA consigam localizar rapidamente as informações necessárias durante análises, implementações, revisões e geração de documentação.

---

# Objetivo

A Knowledge Base foi criada para:

- reduzir o tempo de entendimento do projeto;
- facilitar a navegação pela documentação;
- fornecer contexto arquitetural rapidamente;
- evitar respostas inconsistentes;
- padronizar implementações realizadas por IA;
- servir como ponto inicial de consulta para agentes especializados.

---

# Como utilizar

Sempre que um agente iniciar uma nova tarefa, recomenda-se a seguinte sequência:

```text
knowledge/

↓

Architecture

↓

ADR Index

↓

Business

↓

Database

↓

API

↓

Frontend

↓

Documentação Oficial (docs/)
```

A **Knowledge Base** responde perguntas como:

- Onde está determinada informação?
- Qual ADR devo seguir?
- Em qual camada implementar?
- Onde estão as regras de negócio?
- Quais arquivos normalmente serão alterados?

---

# Estrutura

```text
knowledge/

README.md

architecture.md

technology-stack.md

solution-structure.md

adr-index.md

coding-standards.md

development.md

glossary.md

troubleshooting.md

workflows.md

patterns.md

business-rules.md

business/

database/

api/

frontend/

integrations/

deployment/

testing/
```

---

# Descrição dos Arquivos

## architecture.md

Resumo da arquitetura da solução.

Contém:

- Arquitetura em Camadas
- Fluxo de dependências
- Componentes
- Responsabilidades das camadas
- Links para ADRs

---

## technology-stack.md

Resumo das tecnologias utilizadas.

Exemplo:

- ASP.NET Core
- MVC
- Entity Framework Core
- Dapper
- MySQL
- Docker
- Bootstrap
- JavaScript

---

## solution-structure.md

Resumo da organização da Solution.

Exemplo:

```text
src/

tests/

docs/

.ai/
```

Também descreve onde localizar:

- APIs
- MVC
- Banco
- Serviços
- Repositórios
- Documentação

---

## adr-index.md

Índice resumido dos ADRs.

Não substitui os ADRs completos.

Exemplo:

```text
ADR-0001

Arquitetura em Camadas

↓

MVC

↓

Service

↓

Repository

↓

EF Core

----------------------------

ADR-0016

Soft Delete

↓

Ativo

↓

DataExclusao

↓

UsuarioExclusao
```

---

## coding-standards.md

Resumo das convenções utilizadas.

Inclui:

- Naming
- Estrutura de Classes
- Convenções de Código
- SOLID
- Clean Code

---

## development.md

Resumo do fluxo de desenvolvimento.

Inclui:

- Implementação
- Revisão
- Testes
- Documentação
- Deploy

---

## glossary.md

Glossário do domínio.

Exemplo:

- Cliente
- Empresa
- Caixa
- Pedido
- Venda
- Licenciamento
- Produto

---

## troubleshooting.md

Problemas conhecidos.

Exemplo:

- Entity Framework
- Migrations
- Docker
- JWT
- Build
- Dependências
- Banco de Dados

---

## workflows.md

Fluxos resumidos do sistema.

Exemplo:

- Login
- Venda
- Cancelamento
- Caixa
- Licenciamento

---

## patterns.md

Resumo dos padrões arquiteturais.

Exemplo:

- Repository Pattern
- Service Layer
- Notification Pattern
- Dependency Injection

---

## business-rules.md

Índice das regras de negócio.

Exemplo:

```text
Venda

↓

docs/business-rules/vendas.md

-----------------------

Caixa

↓

docs/business-rules/caixa.md
```

---

# Diretórios

## business/

Resumo dos módulos de negócio.

Cada documento deve responder:

- O que o módulo faz?
- Quais entidades utiliza?
- Quais serviços participam?
- Quais APIs existem?
- Quais ADRs se aplicam?
- Onde está a documentação oficial?

---

## database/

Resumo da camada de persistência.

Exemplo:

- Entidades
- Mappings
- Índices
- Auditoria
- Soft Delete
- Migrations

---

## api/

Resumo das APIs.

Inclui:

- Endpoints
- DTOs
- Versionamento
- Segurança
- Responses

---

## frontend/

Resumo da camada MVC.

Inclui:

- Layouts
- Controllers
- Views
- JavaScript
- Bootstrap

---

## integrations/

Resumo das integrações.

Exemplos:

- PDV
- Mobile
- Cardápio Digital
- APIs externas

---

## deployment/

Resumo do processo de implantação.

Inclui:

- Docker
- Variáveis de Ambiente
- Configuração
- CI/CD

---

## testing/

Resumo da estratégia de testes.

Inclui:

- Testes Unitários
- Integração
- E2E
- Mock
- Cobertura

---

# Relação com a documentação oficial

A Knowledge Base **não deve duplicar** informações existentes na pasta `docs`.

Sempre que possível, os documentos devem conter:

- resumo;
- contexto;
- localização da documentação oficial;
- ADRs relacionados;
- links internos para os documentos completos.

Exemplo:

```text
Regra de Negócio

Resumo:
Cliente deve estar ativo para realizar vendas.

Documentação completa:

docs/business-rules/vendas.md
```

---

# Fluxo recomendado para Agentes de IA

```text
Receber Solicitação

↓

Ler README.md

↓

Ler architecture.md

↓

Ler adr-index.md

↓

Identificar o módulo

↓

Consultar o resumo correspondente

↓

Abrir a documentação oficial em docs/

↓

Planejar

↓

Implementar

↓

Validar ADRs

↓

Documentar
```

---

# Princípios

A Knowledge Base deve seguir os seguintes princípios:

- Não duplicar documentação.
- Ser objetiva e resumida.
- Facilitar navegação.
- Referenciar sempre a documentação oficial.
- Evoluir junto com o projeto.
- Permanecer consistente com os ADRs.

---

# Público-Alvo

Esta base de conhecimento é destinada a:

- GitHub Copilot
- ChatGPT
- Claude
- Agentes especializados
- Ferramentas de geração automática de código
- Desenvolvedores que desejam localizar rapidamente informações do projeto

---

# Manutenção

Sempre que houver alteração em:

- arquitetura;
- regras de negócio;
- APIs;
- banco de dados;
- ADRs;
- padrões de desenvolvimento;

a documentação correspondente em `docs/` deverá ser atualizada primeiro.

Em seguida, a **Knowledge Base** deverá ser revisada para manter seus resumos, índices e referências sincronizados.

---

# Filosofia

A documentação oficial responde **"como o sistema funciona"**.

A Knowledge Base responde **"onde encontrar a informação e quais padrões seguir"**.

Essa separação mantém a documentação consistente, reduz redundâncias e fornece aos agentes de IA um ponto de partida eficiente para compreender o contexto do projeto.