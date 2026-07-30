# ADR-0004 - Adoção do Entity Framework Core como ORM Oficial

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é composto por diversos módulos responsáveis pelo gerenciamento de processos comerciais, financeiros, fiscais, estoque, usuários, licenciamento e integrações.

A solução necessita de um mecanismo de persistência que ofereça:

- Produtividade no desenvolvimento;
- Facilidade de manutenção;
- Controle de transações;
- Mapeamento objeto-relacional (ORM);
- Suporte a migrations;
- Integração nativa com ASP.NET Core;
- Facilidade para testes.

Além disso, a equipe precisava de uma tecnologia consolidada, amplamente documentada e suportada pela Microsoft.

---

# Problema

Realizar toda a persistência utilizando SQL manual aumenta significativamente a complexidade da aplicação.

Os principais problemas identificados foram:

- Grande quantidade de código repetitivo;
- Alto custo de manutenção;
- Mapeamentos manuais;
- Maior possibilidade de erros;
- Baixa produtividade;
- Dificuldade para evoluir o modelo de dados.

Era necessário definir um ORM oficial para a solução.

---

# Alternativas Consideradas

## Alternativa 1 — SQL puro (ADO.NET)

### Vantagens

- Máximo controle sobre SQL.
- Excelente desempenho.

### Desvantagens

- Muito código repetitivo.
- Alto custo de manutenção.
- Mapeamento manual.
- Baixa produtividade.

---

## Alternativa 2 — Dapper como ORM principal

### Vantagens

- Excelente desempenho.
- SQL explícito.
- Baixo overhead.

### Desvantagens

- Não possui Change Tracking.
- Não possui Migrations.
- Não possui relacionamento automático.
- CRUDs exigem mais código.

---

## Alternativa 3 — Entity Framework Core (Escolhida)

### Vantagens

- Alta produtividade.
- Mapeamento objeto-relacional completo.
- Change Tracking.
- LINQ.
- Migrations.
- Lazy/Eager Loading.
- Integração nativa com ASP.NET Core.
- Grande comunidade.

### Desvantagens

- Pequeno overhead em relação ao SQL puro.
- Consultas complexas podem exigir otimização.

---

# Decisão

Foi adotado o **Entity Framework Core** como ORM oficial do Agilium Manager.

O EF Core será utilizado para todas as operações padrão de persistência, incluindo:

- Inclusão;
- Alteração;
- Exclusão;
- Consultas comuns;
- Controle de transações;
- Relacionamentos;
- Migrations.

O Dapper será utilizado apenas em cenários específicos de leitura com necessidade comprovada de alta performance.

---

# Objetivos

A utilização do EF Core visa:

- Padronizar o acesso ao banco de dados.
- Reduzir código repetitivo.
- Melhorar a produtividade.
- Centralizar configurações de persistência.
- Facilitar manutenção.
- Garantir evolução segura do banco de dados.

---

# Fluxo

```text
Controller

↓

Service

↓

Repository

↓

DbContext

↓

Entity Framework Core

↓

Banco de Dados
```

---

# Responsabilidades

## Entity Framework Core

Responsável por:

- Mapeamento ORM;
- Tracking de entidades;
- Persistência;
- Relacionamentos;
- Transações;
- Conversão entre objetos e tabelas.

---

## DbContext

Responsável por:

- Configuração das entidades;
- Gerenciamento do ciclo de vida das entidades;
- Controle de SaveChanges();
- Controle de transações.

---

## EntityTypeConfiguration

Toda configuração das entidades deverá ser realizada utilizando classes que implementem:

```csharp
IEntityTypeConfiguration<TEntity>
```

Não é permitido realizar configurações diretamente dentro das entidades.

---

# Estrutura recomendada

```text
Persistence/

├── Context/

│   └── ApplicationDbContext.cs

│

├── Mapping/

│   ├── ProdutoMap.cs

│   ├── ClienteMap.cs

│   ├── UsuarioMap.cs

│   └── ...

│

├── Migrations/

└── Repository/
```

---

# Convenções

As entidades devem permanecer limpas.

Não devem possuir:

- Configurações do EF Core;
- Atributos de persistência desnecessários;
- Código SQL;
- Dependências da infraestrutura.

Todo mapeamento deve estar nas classes de configuração.

---

# Relacionamentos

Os relacionamentos deverão ser configurados utilizando Fluent API.

Exemplos:

- One-to-One
- One-to-Many
- Many-to-Many

Evitar Data Annotations quando a configuração puder ser realizada via Fluent API.

---

# Migrations

Toda alteração estrutural do banco deverá ser realizada através de Migrations.

Não é permitido modificar manualmente a estrutura do banco em ambientes controlados.

As Migrations deverão:

- Ser versionadas;
- Ser revisadas;
- Possuir nomenclatura descritiva;
- Ser executadas em ordem cronológica.

---

# Quando utilizar Dapper

O Dapper poderá ser utilizado somente quando:

- Houver necessidade comprovada de desempenho;
- Consultas complexas;
- Dashboards;
- Relatórios;
- Grandes volumes de leitura.

Sempre encapsulado na camada Repository.

---

# Quando NÃO utilizar EF Core

Evitar EF Core para:

- Consultas extremamente complexas;
- Processamentos massivos de leitura;
- Relatórios analíticos pesados.

Nestes casos utilizar Dapper.

---

# Benefícios

- Maior produtividade.
- Redução de código repetitivo.
- Melhor manutenção.
- Padronização.
- Integração com LINQ.
- Controle automático de mudanças.
- Suporte a migrations.
- Evolução segura do banco.

---

# Desvantagens

- Pequeno overhead.
- Necessidade de conhecer boas práticas de performance.
- Consultas mal elaboradas podem gerar SQL ineficiente.

---

# Riscos

Caso o padrão não seja seguido:

- Mistura de tecnologias de persistência.
- SQL espalhado pela solução.
- Código inconsistente.
- Problemas de manutenção.
- Dificuldade para evoluir o banco.

---

# Impacto

Esta decisão impacta diretamente:

- Persistência
- Repository
- Services
- Banco de Dados
- Migrations
- Testes
- Documentação

---

# Plano de Implementação

1. Definir o EF Core como ORM oficial.
2. Centralizar os DbContexts.
3. Criar classes de mapeamento utilizando Fluent API.
4. Migrar configurações existentes para `IEntityTypeConfiguration`.
5. Utilizar Migrations para evolução do banco.
6. Reservar Dapper apenas para consultas especializadas.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Toda persistência padrão utiliza Entity Framework Core.
- Os mapeamentos utilizam Fluent API.
- As entidades permanecem desacopladas da infraestrutura.
- Alterações estruturais são realizadas via Migrations.
- Dapper é utilizado apenas quando tecnicamente justificado.
- Nenhuma camada acessa diretamente o banco sem passar pelos Repositories.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0002 — Repository Pattern
- ADR-0009 — Dependency Injection
- ADR-0010 — Dapper para Consultas de Alta Performance
- ADR-0019 — Estratégia de Migrations

---

# Referências

- Microsoft — *Entity Framework Core Documentation*
- Microsoft — *EF Core Fluent API*
- Microsoft — *EF Core Performance Best Practices*
- Martin Fowler — *Patterns of Enterprise Application Architecture*
- Julie Lerman — *Programming Entity Framework Core*

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| 1.0 | 2026-07-29 | Criação da ADR definindo o Entity Framework Core como ORM oficial do Agilium Manager e estabelecendo as diretrizes para utilização de Fluent API, Migrations e integração com o Repository Pattern. |