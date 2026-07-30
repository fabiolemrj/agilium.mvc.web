# Arquitetura de Persistência

## Objetivo

Documentar a arquitetura da camada de persistência utilizada pelo ecossistema Agilium Manager, descrevendo os componentes responsáveis pelo acesso aos dados, os padrões arquiteturais adotados e as tecnologias utilizadas.

Este documento fornece uma visão geral da infraestrutura de persistência da solução e serve como referência para o desenvolvimento de novas funcionalidades.

---

# Escopo

Este documento contempla:

- Arquitetura de Persistência
- Bancos de Dados
- Entity Framework Core
- Dapper
- MongoDB
- DbContexts
- Repository Pattern
- Unit of Work
- Fluent API
- Configuração
- Gerenciamento de Conexões
- Performance
- Segurança

---

# Índice

- Visão Geral
- Arquitetura da Persistência
- Tecnologias Utilizadas
- Bancos de Dados
- Entity Framework Core
- MongoDB
- Dapper
- Repository Pattern
- Unit of Work
- DbContexts
- Configuração
- Gerenciamento de Conexões
- Estratégias de Performance
- Segurança
- Boas Práticas
- Limitações Conhecidas
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager utiliza uma arquitetura de persistência em camadas, separando claramente a lógica de negócio do acesso aos dados.

A comunicação com os bancos de dados ocorre através da camada de Repositories, utilizando Entity Framework Core como ORM principal e Dapper em cenários específicos de consultas otimizadas.

O levantamento técnico também identificou referências ao uso de persistência documental (MongoDB), cuja utilização deverá ser confirmada durante a análise dos projetos de API.

---

# Arquitetura da Persistência

```text
Controllers

      │

Services

      │

Repositories

      │

────────────────────────────

Entity Framework Core

Dapper

MongoDB Driver

────────────────────────────

Banco Relacional

MongoDB
```

Toda operação de persistência deve ocorrer através da camada de infraestrutura, mantendo o desacoplamento entre domínio e banco de dados.

---

# Tecnologias Utilizadas

| Tecnologia | Finalidade |
|------------|------------|
| Entity Framework Core | ORM principal para persistência relacional |
| Dapper | Consultas otimizadas e alto desempenho |
| MongoDB.Driver | Persistência documental (quando aplicável) |
| Fluent API | Configuração e mapeamento das entidades |
| Dependency Injection | Gerenciamento dos componentes de persistência |

A utilização efetiva de cada tecnologia deverá refletir a implementação existente na solução.

---

# Bancos de Dados

A solução pode utilizar diferentes mecanismos de persistência, conforme a responsabilidade de cada módulo.

## Banco Relacional

Responsável por armazenar os dados transacionais da aplicação, como:

- usuários;
- clientes;
- produtos;
- vendas;
- financeiro;
- estoque;
- configurações.

> A tecnologia do banco relacional (SQL Server, MySQL ou outra) deve ser documentada conforme identificado na solução.

## MongoDB

Quando presente, o MongoDB é utilizado para dados documentais ou cenários que exigem maior flexibilidade estrutural.

Sua utilização deve ser detalhada em documentação específica.

---

# Entity Framework Core

O Entity Framework Core é o principal mecanismo de persistência relacional.

Responsabilidades:

- gerenciamento de entidades;
- rastreamento de alterações;
- persistência;
- relacionamentos;
- migrations.

Principais componentes:

- DbContext;
- DbSet;
- IEntityTypeConfiguration;
- Fluent API.

---

# Fluent API

O mapeamento das entidades deve permanecer centralizado em classes específicas.

Cada entidade deve possuir sua própria configuração contendo:

- chave primária;
- relacionamentos;
- índices;
- restrições;
- tipos de dados.

---

# MongoDB

Quando utilizado, o acesso deve ocorrer através do MongoDB.Driver e de componentes específicos da infraestrutura.

A documentação das coleções, índices e estratégias de persistência deverá ser mantida em um documento próprio.

---

# Dapper

O Dapper deve ser utilizado apenas quando houver necessidade de consultas otimizadas ou de maior desempenho.

Cenários comuns:

- relatórios;
- dashboards;
- consultas complexas;
- projeções específicas.

---

# Repository Pattern

O acesso aos dados deve ocorrer exclusivamente através de Repositories.

Responsabilidades:

- encapsular consultas;
- isolar a infraestrutura;
- facilitar testes;
- reutilizar regras de persistência.

Os Controllers nunca devem acessar diretamente o banco de dados.

---

# Unit of Work

Quando utilizado, o Unit of Work deve coordenar transações envolvendo múltiplos repositórios, garantindo consistência nas operações de escrita.

---

# DbContexts

Os DbContexts representam os pontos de acesso ao banco relacional.

Exemplos de contextos:

- ApplicationDbContext;
- IdentityDbContext;
- Contextos específicos de módulos.

A estrutura definitiva deve refletir a implementação existente.

---

# Configuração

As configurações de persistência normalmente encontram-se em:

- appsettings.json;
- appsettings.{Environment}.json;
- variáveis de ambiente;
- User Secrets.

Nunca armazenar credenciais diretamente no código-fonte.

---

# Gerenciamento de Conexões

As conexões devem ser gerenciadas pelo mecanismo de injeção de dependências e configuradas de forma centralizada.

Boas práticas:

- pooling de conexões;
- timeout adequado;
- retry policies (quando aplicável);
- segregação por ambiente.

---

# Estratégias de Performance

Para manter o desempenho da camada de persistência:

- utilizar índices adequados;
- evitar consultas N+1;
- aplicar paginação;
- projetar apenas os campos necessários;
- utilizar Dapper quando houver ganho comprovado de desempenho.

---

# Segurança

Boas práticas:

- utilizar consultas parametrizadas;
- proteger connection strings;
- restringir permissões do banco;
- registrar operações críticas quando necessário.

---

# Boas Práticas

Sempre:

- utilizar Repository Pattern;
- centralizar mapeamentos;
- documentar novas entidades;
- revisar índices;
- revisar migrations;
- utilizar Entity Framework Core para persistência padrão;
- utilizar Dapper apenas quando necessário.

Evitar:

- acesso direto ao banco em Controllers;
- SQL concatenado;
- duplicação de consultas;
- lógica de negócio na camada de persistência.

---

# Limitações Conhecidas

O levantamento técnico identificou:

- utilização de Entity Framework Core;
- utilização de Fluent API;
- utilização de Dapper em cenários específicos;
- referências à persistência documental.

Entretanto, ainda devem ser confirmados durante a análise dos projetos:

- tecnologia do banco relacional utilizada;
- estrutura dos DbContexts;
- uso efetivo do MongoDB;
- estratégia de migrations;
- política de transações.

---

# Documentação Relacionada

- overview.md
- entities.md
- relationships.md
- migrations.md
- repositories.md
- entity-framework.md
- dapper.md
- mongodb.md
- connection-management.md