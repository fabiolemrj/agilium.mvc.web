# Repository Pattern

# Objetivo

Documentar a arquitetura do Repository Pattern utilizada pelo Agilium Manager, descrevendo como o acesso aos dados é abstraído, organizado e integrado às camadas de negócio e infraestrutura da aplicação.

---

# Escopo

Este documento contempla:

- Repository Pattern
- Interfaces
- Implementações
- Integração com Services
- Unit of Work
- Convenções
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura do Repository Pattern
- Organização dos Repositórios
- Interfaces
- Implementações
- Integração com Services
- Fluxo de Persistência
- Convenções
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager utiliza o **Repository Pattern** para abstrair o acesso aos dados, desacoplando as regras de negócio da tecnologia de persistência.

A camada Business interage exclusivamente com interfaces de repositório, enquanto a camada Infrastructure concentra as implementações responsáveis pela comunicação com os bancos de dados. :contentReference[oaicite:1]{index=1}

---

# Arquitetura do Repository Pattern

O fluxo de persistência segue a arquitetura:

```text
Controller

↓

Service

↓

Repository Interface

↓

Repository

↓

Entity Framework Core / Dapper

↓

MySQL
```

Essa arquitetura favorece a separação de responsabilidades, facilita testes e reduz o acoplamento entre as camadas. :contentReference[oaicite:2]{index=2}

---

# Organização dos Repositórios

O levantamento técnico identificou a seguinte organização na camada de infraestrutura:

```text
Infra/

├── Interfaces/

└── Repository/
```

As interfaces definem os contratos de acesso aos dados, enquanto as implementações concretizam esses contratos utilizando as tecnologias de persistência adotadas pela solução. :contentReference[oaicite:3]{index=3}

---

# Interfaces

As interfaces de repositório são consumidas pela camada Business por meio da Injeção de Dependência.

Essa abordagem permite que os Services dependam de abstrações, preservando o desacoplamento em relação às implementações concretas da infraestrutura. :contentReference[oaicite:4]{index=4}

---

# Implementações

As implementações dos repositórios estão localizadas na camada Infrastructure e utilizam:

- Entity Framework Core;
- Dapper.

A escolha entre essas tecnologias depende das necessidades de persistência e consulta de cada funcionalidade. :contentReference[oaicite:5]{index=5}

---

# Integração com Services

Os Services concentram as regras de negócio e utilizam os repositórios para realizar operações de persistência.

O acesso aos dados ocorre sempre por meio das interfaces registradas no container de Injeção de Dependência, mantendo a independência entre as camadas Business e Infrastructure. :contentReference[oaicite:6]{index=6}

---

# Fluxo de Persistência

O fluxo típico de uma operação é:

```text
Controller

↓

Service

↓

Repository Interface

↓

Repository

↓

Entity Framework Core / Dapper

↓

Banco de Dados
```

Quando a operação envolve alterações de dados, o Repository Pattern atua em conjunto com o **Unit of Work**, garantindo o gerenciamento consistente das transações. :contentReference[oaicite:7]{index=7}

---

# Convenções

A utilização do Repository Pattern segue as seguintes diretrizes:

- definir contratos por interfaces;
- manter as implementações na camada Infrastructure;
- concentrar regras de negócio nos Services;
- utilizar o Repository apenas para acesso aos dados;
- integrar operações transacionais ao Unit of Work.

---

# Boas Práticas

Sempre:

- depender de interfaces;
- manter os repositórios focados em persistência;
- utilizar Services para orquestração das regras de negócio;
- reutilizar os mecanismos de Entity Framework Core e Dapper conforme apropriado;
- registrar os repositórios por meio da Injeção de Dependência.

Evitar:

- implementar regras de negócio nos repositórios;
- acessar diretamente o banco de dados a partir dos Controllers;
- duplicar consultas em diferentes repositórios;
- acoplar a camada Business às tecnologias de persistência.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização do Repository Pattern;
- separação entre Business e Infrastructure;
- existência das pastas `Infra/Interfaces` e `Infra/Repository`;
- utilização de Entity Framework Core;
- utilização de Dapper;
- utilização de Unit of Work;
- consumo dos repositórios pelos Services;
- registro das dependências via Injeção de Dependência. :contentReference[oaicite:8]{index=8}

Ainda deverão ser documentados mediante análise do código-fonte:

- inventário completo dos repositórios;
- interfaces disponíveis;
- métodos expostos por cada contrato;
- implementação de repositórios genéricos, se existente;
- estratégias específicas para consultas complexas;
- convenções de nomenclatura dos repositórios.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- criação de novos repositórios;
- alteração da arquitetura de persistência;
- inclusão de novos bancos de dados;
- evolução do Unit of Work;
- mudanças na organização da camada Infrastructure.

---

# Documentação Relacionada

## Arquitetura

- architecture/layers.md
- architecture/patterns.md

## Desenvolvimento

- development/dependency-injection.md

## Banco de Dados

- database/overview.md
- database/entity-framework.md
- database/dapper.md