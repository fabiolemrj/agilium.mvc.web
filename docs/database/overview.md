# Visão Geral da Persistência

## Objetivo

Documentar a arquitetura da camada de persistência do Agilium Manager, apresentando sua organização, responsabilidades, estratégias de acesso aos dados e princípios adotados para armazenamento das informações da aplicação.

Este documento fornece uma visão geral da persistência e referencia os documentos específicos relacionados às entidades, mapeamentos, relacionamentos, índices e migrações.

---

# Escopo

Este documento contempla:

- Arquitetura da Persistência
- Organização da Camada de Dados
- Estratégias de Acesso
- Modelo de Persistência
- Convenções
- Princípios
- Responsabilidades
- Limitações

---

# Índice

- Visão Geral
- Arquitetura da Persistência
- Organização da Camada
- Estratégias de Acesso aos Dados
- Modelo de Persistência
- Princípios de Modelagem
- Organização da Documentação
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

A camada de persistência é responsável pelo armazenamento, recuperação e atualização das informações utilizadas pela aplicação.

Sua implementação deve garantir:

- isolamento da lógica de acesso aos dados;
- consistência das informações;
- manutenção da integridade dos dados;
- reutilização dos componentes de persistência;
- desacoplamento entre domínio e infraestrutura.

---

# Arquitetura da Persistência

A persistência integra a arquitetura em camadas da aplicação.

Fluxo simplificado:

```text
Controller

↓

Application Service

↓

Repository

↓

ORM / Camada de Persistência

↓

Banco de Dados
```

As regras de negócio não devem depender diretamente do mecanismo de persistência.

---

# Organização da Camada

A camada de persistência é composta, conceitualmente, pelos seguintes elementos:

```text
Entidades

↓

Mapeamentos

↓

Repositórios

↓

Contexto de Persistência

↓

Banco de Dados
```

Cada componente possui responsabilidade específica e deve permanecer desacoplado das demais camadas da aplicação.

---

# Estratégias de Acesso aos Dados

O levantamento técnico confirmou a utilização de uma camada de persistência baseada em **Entity Framework Core** para acesso às entidades persistidas. :contentReference[oaicite:1]{index=1}

Outras estratégias de acesso aos dados (como micro-ORMs, acesso direto ao banco ou bancos não relacionais) deverão ser documentadas apenas após confirmação durante o levantamento dos projetos de infraestrutura e APIs.

---

# Modelo de Persistência

O modelo de persistência deve observar os seguintes princípios:

- cada entidade representa um único conceito de domínio;
- relacionamentos devem ser explicitamente definidos;
- mapeamentos devem permanecer centralizados na camada de infraestrutura;
- índices devem ser documentados;
- alterações estruturais devem ser versionadas.

---

# Princípios de Modelagem

Sempre que possível, a modelagem da persistência deve:

- refletir o modelo de domínio;
- minimizar redundâncias;
- preservar integridade referencial;
- manter nomenclatura consistente;
- favorecer a evolução controlada do esquema de dados.

As convenções específicas deverão refletir a implementação existente.

---

# Organização da Documentação

A documentação da persistência está organizada nos seguintes documentos:

```text
database/

overview.md
entities.md
relationships.md
mappings.md
indexes.md
constraints.md
migrations.md
performance.md
```

Cada documento trata um aspecto específico da camada de persistência.

---

# Boas Práticas

Sempre:

- centralizar o acesso aos dados na camada de persistência;
- documentar entidades e relacionamentos;
- manter os mapeamentos consistentes com o modelo de domínio;
- utilizar versionamento para alterações estruturais;
- manter a documentação sincronizada com a implementação.

Evitar:

- acesso direto ao banco fora da camada de persistência;
- duplicação de lógica de acesso aos dados;
- acoplamento entre regras de negócio e infraestrutura;
- alterações estruturais sem documentação.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de Entity Framework Core na camada de persistência;
- utilização de entidades e mapeamentos;
- existência de uma camada de repositórios.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-git-azure-infra`;
- `agilium-manager-azure-business`;
- `agilium-manager-azure-api`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- sistema gerenciador de banco de dados utilizado;
- utilização de Dapper ou outras estratégias de acesso;
- utilização de bancos NoSQL;
- estratégia de versionamento do banco;
- arquitetura completa da infraestrutura de persistência.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração da arquitetura da persistência;
- adoção de novas tecnologias de acesso aos dados;
- mudança no modelo de armazenamento;
- evolução significativa da infraestrutura.

---

# Documentação Relacionada

## Banco de Dados

- database/entities.md
- database/relationships.md
- database/mappings.md
- database/indexes.md
- database/migrations.md
- database/constraints.md
- database/performance.md

## Arquitetura

- architecture/layers.md
- architecture/overview.md
- architecture/dependency-flow.md

## Desenvolvimento

- development/coding-standards.md
- development/versioning.md