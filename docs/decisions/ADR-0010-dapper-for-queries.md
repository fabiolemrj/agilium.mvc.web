# ADR-0010 - Adoção do Dapper para Consultas de Alta Performance

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager utiliza o **Entity Framework Core** como ORM oficial (ADR-0004), sendo responsável pela persistência da aplicação, gerenciamento de entidades, relacionamentos e controle de transações.

Entretanto, alguns módulos da plataforma realizam consultas de alta complexidade e grande volume de dados, como:

- Dashboards;
- Relatórios fiscais;
- Relatórios financeiros;
- Painéis gerenciais;
- Consultas estatísticas;
- Exportações;
- Pesquisas paginadas com muitos filtros;
- Integrações.

Nesses cenários, o Entity Framework Core pode gerar consultas SQL mais complexas do que o necessário, impactando a performance.

Era necessário definir uma estratégia para consultas de leitura de alta performance sem comprometer a arquitetura da solução.

---

# Problema

Utilizar exclusivamente o Entity Framework Core para todas as consultas pode ocasionar:

- SQL desnecessariamente complexo;
- Maior consumo de memória;
- Tracking de entidades desnecessário;
- Baixo desempenho em consultas analíticas;
- Consultas lentas em grandes volumes de dados.

Ao mesmo tempo, utilizar apenas SQL manual comprometeria a produtividade e a padronização da aplicação.

Era necessário equilibrar produtividade e desempenho.

---

# Alternativas Consideradas

## Alternativa 1 — Utilizar apenas Entity Framework Core

### Vantagens

- Padronização.
- Menor quantidade de tecnologias.
- Facilidade de manutenção.

### Desvantagens

- Menor desempenho em consultas complexas.
- SQL gerado nem sempre otimizado.
- Tracking desnecessário em leituras.

---

## Alternativa 2 — Utilizar apenas Dapper

### Vantagens

- Excelente desempenho.
- SQL totalmente controlado.
- Baixo consumo de memória.

### Desvantagens

- Grande quantidade de código SQL.
- Ausência de Change Tracking.
- Ausência de Migrations.
- Baixa produtividade para operações CRUD.

---

## Alternativa 3 — EF Core + Dapper (Escolhida)

### Vantagens

- Produtividade para operações CRUD.
- Alta performance em consultas específicas.
- Melhor equilíbrio entre manutenção e desempenho.
- Menor duplicação de código.
- Aproveitamento das vantagens de cada tecnologia.

### Desvantagens

- Necessidade de definir quando utilizar cada tecnologia.
- Equipe deve conhecer ambas as ferramentas.

---

# Decisão

Foi adotada uma estratégia híbrida de persistência.

O **Entity Framework Core** permanece como ORM oficial da aplicação.

O **Dapper** será utilizado exclusivamente para consultas de leitura com necessidade comprovada de alta performance.

O Dapper **não deverá ser utilizado para operações de escrita (INSERT, UPDATE ou DELETE)**, salvo casos excepcionais devidamente justificados e aprovados em revisão arquitetural.

---

# Objetivos

Esta decisão possui os seguintes objetivos:

- Melhorar desempenho de consultas críticas.
- Reduzir tempo de resposta.
- Minimizar consumo de memória.
- Manter a produtividade do EF Core.
- Evitar SQL complexo gerado automaticamente quando necessário.

---

# Fluxo

```text
Controller

↓

Service

↓

Repository

↓

Consulta

↓

├── EF Core (CRUD)
│
└── Dapper (Read Only)

↓

Banco de Dados
```

---

# Quando utilizar Entity Framework Core

O EF Core deverá ser utilizado para:

- Inclusão de registros;
- Alteração de registros;
- Exclusão de registros;
- Consultas simples;
- Relacionamentos;
- Controle transacional;
- Migrations;
- Change Tracking.

---

# Quando utilizar Dapper

O Dapper deverá ser utilizado para:

- Dashboards;
- Relatórios;
- Consultas estatísticas;
- Grandes volumes de leitura;
- Pesquisas complexas;
- Consultas paginadas;
- Agregações;
- Views materializadas;
- Procedimentos armazenados (quando existentes).

---

# Quando NÃO utilizar Dapper

Evitar utilizar Dapper para:

- Inserções;
- Atualizações;
- Exclusões;
- Controle de transações da aplicação;
- Regras de negócio;
- Operações simples de CRUD.

Essas operações permanecem sob responsabilidade do Entity Framework Core.

---

# Organização

As consultas utilizando Dapper deverão permanecer na camada de Repository.

Estrutura sugerida:

```text
Persistence/

├── Repository/

│   ├── ProdutoRepository.cs
│   ├── ClienteRepository.cs
│   └── ...

│
├── Queries/

│   ├── ProdutoQueryRepository.cs
│   ├── VendaQueryRepository.cs
│   ├── DashboardQueryRepository.cs
│   └── ...

│
└── Context/
```

---

# Interfaces

Toda consulta deverá possuir interface.

Exemplo:

```csharp
public interface IDashboardQueryRepository
{
    Task<DashboardResumoDto> ObterResumoAsync();
}
```

---

# SQL

As consultas deverão utilizar SQL parametrizado.

Exemplo:

```sql
SELECT *

FROM Produto

WHERE EmpresaId = @EmpresaId
```

Nunca concatenar parâmetros diretamente na instrução SQL.

---

# Segurança

Todas as consultas Dapper deverão:

- Utilizar parâmetros;
- Evitar SQL Injection;
- Validar filtros;
- Respeitar permissões do usuário;
- Respeitar isolamento por empresa (Tenant).

---

# Mapeamento

Os resultados deverão ser projetados diretamente para DTOs ou ViewModels.

Evitar carregar entidades completas quando não necessário.

Exemplo:

```text
ProdutoResumoDto

VendaDashboardDto

FinanceiroResumoDto
```

---

# Performance

Sempre que possível:

- Selecionar apenas colunas necessárias;
- Evitar `SELECT *`;
- Utilizar índices existentes;
- Implementar paginação;
- Limitar quantidade de registros;
- Utilizar consultas assíncronas.

---

# Benefícios

- Alto desempenho.
- Baixo consumo de memória.
- SQL otimizado.
- Melhor experiência do usuário.
- Maior escalabilidade.
- Menor tempo de resposta.

---

# Desvantagens

- Introdução de uma segunda tecnologia de acesso a dados.
- Necessidade de manter SQL manual.
- Maior responsabilidade na escrita das consultas.

---

# Riscos

Caso esta estratégia não seja seguida:

- Consultas lentas.
- Alto consumo de recursos.
- SQL ineficiente.
- Perda de desempenho em módulos analíticos.
- Uso inadequado do Dapper para operações de escrita.

---

# Impacto

Esta decisão impacta:

- Repositories
- Relatórios
- Dashboards
- APIs
- MVC
- Integrações
- Banco de Dados
- Performance
- Monitoramento

---

# Plano de Implementação

1. Manter o Entity Framework Core como ORM principal.
2. Criar repositórios específicos para consultas Dapper.
3. Centralizar SQL em classes de consulta.
4. Utilizar apenas consultas parametrizadas.
5. Monitorar desempenho das consultas.
6. Revisar periodicamente consultas críticas.
7. Atualizar documentação técnica.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- O Entity Framework Core continua responsável pelas operações CRUD.
- O Dapper é utilizado apenas para consultas de leitura com necessidade comprovada de desempenho.
- Todas as consultas Dapper utilizam SQL parametrizado.
- Os resultados são projetados diretamente para DTOs ou ViewModels.
- Não existem regras de negócio implementadas nas consultas Dapper.
- Consultas críticas apresentam ganhos mensuráveis de desempenho em relação à implementação equivalente utilizando apenas EF Core.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0002 — Repository Pattern
- ADR-0004 — Entity Framework Core como ORM Oficial
- ADR-0009 — Estratégia de Dependency Injection
- ADR-0011 — Service Layer
- ADR-0019 — Estratégia de Migrations

---

# Referências

- Dapper Documentation
- Microsoft — Performance Best Practices with Entity Framework Core
- Microsoft — EF Core Efficient Querying
- Stack Overflow Engineering Blog — Why Dapper?
- Martin Fowler — Patterns of Enterprise Application Architecture

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | 2026-07-29 | Criação da ADR definindo o Dapper como tecnologia oficial para consultas de leitura de alta performance no Agilium Manager, mantendo o Entity Framework Core como ORM principal para operações CRUD e gerenciamento de persistência. |