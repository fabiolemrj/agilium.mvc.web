# ADR-0016 - Estratégia de Exclusão Lógica (Soft Delete)

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é uma plataforma de gestão empresarial responsável por armazenar informações críticas relacionadas a:

- Clientes
- Produtos
- Usuários
- Empresas
- Estoque
- Pedidos
- Vendas
- Financeiro
- Configurações
- Integrações

Grande parte desses dados possui relacionamento com registros históricos, documentos fiscais, auditorias e integrações externas.

A remoção física de registros poderia comprometer a integridade histórica da plataforma e gerar inconsistências em relatórios e referências entre entidades.

Era necessário definir uma estratégia oficial para exclusão de registros.

---

# Problema

A exclusão física (`DELETE`) apresenta diversos riscos:

- Perda definitiva de dados;
- Quebra de relacionamentos;
- Inconsistências históricas;
- Problemas em auditorias;
- Dificuldade para recuperação de informações;
- Falhas em integrações que referenciam registros removidos.

Era necessário preservar o histórico sem impedir que registros deixassem de ser utilizados pela aplicação.

---

# Alternativas Consideradas

## Alternativa 1 — Exclusão Física

```sql
DELETE FROM Produto
WHERE Id = 10;
```

### Vantagens

- Banco menor.
- Simplicidade.

### Desvantagens

- Perda permanente dos dados.
- Quebra de referências.
- Sem possibilidade de recuperação.

---

## Alternativa 2 — Backup antes do DELETE

### Vantagens

- Mantém histórico externo.

### Desvantagens

- Complexidade elevada.
- Recuperação difícil.
- Dados espalhados.

---

## Alternativa 3 — Soft Delete (Escolhida)

Atualizar o registro ao invés de removê-lo.

### Vantagens

- Preserva histórico.
- Permite recuperação.
- Mantém integridade referencial.
- Facilita auditoria.

### Desvantagens

- Consultas devem ignorar registros excluídos.
- Banco cresce ao longo do tempo.

---

# Decisão

Foi adotado o **Soft Delete** como estratégia oficial de exclusão do Agilium Manager.

Registros não deverão ser removidos fisicamente da base de dados, salvo em situações excepcionais previamente aprovadas.

A exclusão deverá ser representada através de campos específicos indicando o estado do registro.

---

# Objetivos

Esta decisão possui os seguintes objetivos:

- Preservar histórico.
- Garantir rastreabilidade.
- Evitar perda de dados.
- Facilitar auditorias.
- Permitir recuperação de registros.
- Manter integridade referencial.

---

# Estrutura

Toda entidade que utilizar Soft Delete deverá possuir, no mínimo, os seguintes campos:

```text
Id

Ativo

DataExclusao

UsuarioExclusao
```

Quando necessário, poderão existir campos adicionais como:

```text
MotivoExclusao

EmpresaExclusao
```

---

# Fluxo

```text
Solicitação de Exclusão

↓

Validação

↓

Atualização do Registro

↓

Ativo = false

↓

DataExclusao = Agora

↓

UsuarioExclusao = Usuário Atual

↓

Registro permanece no banco
```

---

# Exemplo

Ao invés de:

```sql
DELETE FROM Produto
WHERE Id = 10;
```

Executar:

```sql
UPDATE Produto

SET
    Ativo = 0,
    DataExclusao = CURRENT_TIMESTAMP,
    UsuarioExclusao = @Usuario

WHERE Id = @Id;
```

---

# Consultas

Todas as consultas da aplicação deverão ignorar registros excluídos.

Exemplo:

```sql
SELECT *

FROM Produto

WHERE Ativo = 1;
```

---

# Entity Framework Core

Sempre que possível deverão ser utilizados **Global Query Filters**.

Exemplo:

```csharp
builder.Entity<Produto>()
       .HasQueryFilter(p => p.Ativo);
```

Com isso, registros inativos deixam de ser retornados automaticamente.

---

# Recuperação

Registros poderão ser restaurados quando permitido pela regra de negócio.

Fluxo:

```text
Registro Inativo

↓

Solicitação

↓

Validação

↓

Ativo = true

↓

Remoção da DataExclusao
```

---

# Exclusão Física

A exclusão física somente poderá ocorrer em situações específicas, como:

- Dados temporários;
- Cache;
- Logs descartáveis;
- Filas de processamento;
- Dados de testes;
- Processos de limpeza previamente aprovados.

Toda exclusão física deverá ser documentada.

---

# Auditoria

Toda exclusão lógica deverá registrar:

- Usuário responsável;
- Data;
- Hora;
- Empresa;
- Motivo (quando aplicável).

Essas informações poderão ser utilizadas por mecanismos de auditoria.

---

# Integrações

Integrações externas deverão respeitar o estado do registro.

Registros inativos:

- Não deverão ser utilizados em novas operações;
- Poderão permanecer disponíveis para consultas históricas quando aplicável.

---

# Benefícios

- Preservação do histórico.
- Recuperação simples.
- Integridade referencial.
- Melhor suporte à auditoria.
- Maior segurança dos dados.
- Redução de perda acidental de informações.

---

# Desvantagens

- Crescimento da base de dados.
- Necessidade de filtros em consultas.
- Rotinas periódicas de manutenção podem ser necessárias.

---

# Riscos

Caso esta estratégia não seja seguida:

- Perda permanente de dados.
- Inconsistências históricas.
- Quebra de relacionamentos.
- Problemas em auditorias.
- Dificuldade para recuperação.

---

# Impacto

Esta decisão impacta:

- Banco de Dados
- Entity Framework Core
- Repositories
- Services
- APIs
- MVC
- Auditoria
- Relatórios
- Integrações

---

# Plano de Implementação

1. Adicionar campos de Soft Delete às entidades aplicáveis.
2. Configurar Global Query Filters no Entity Framework Core.
3. Atualizar Repositories para utilizar exclusão lógica.
4. Criar funcionalidade de restauração quando aplicável.
5. Revisar consultas SQL para ignorar registros inativos.
6. Registrar informações de auditoria.
7. Atualizar documentação técnica.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Nenhuma entidade de negócio utiliza exclusão física por padrão.
- Todas as consultas ignoram registros inativos.
- O Entity Framework utiliza Global Query Filters quando possível.
- Exclusões registram usuário e data.
- Existe possibilidade de restauração quando permitido pela regra de negócio.
- Exclusões físicas são exceções documentadas.

---

# ADRs Relacionados

- ADR-0002 — Repository Pattern
- ADR-0004 — Entity Framework Core
- ADR-0007 — Estratégia de Validação
- ADR-0011 — Service Layer
- ADR-0014 — Tratamento Global de Exceções
- ADR-0017 — Estratégia de Auditoria
- ADR-0019 — Estratégia de Migrations

---

# Referências

- Microsoft — Global Query Filters (EF Core)
- Microsoft — Entity Framework Core Documentation
- Martin Fowler — Patterns of Enterprise Application Architecture
- Clean Architecture — Robert C. Martin
- Domain-Driven Design — Eric Evans

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | **2026-07-29** | Criação da ADR definindo o Soft Delete como estratégia oficial de exclusão do Agilium Manager, estabelecendo diretrizes para preservação de histórico, utilização de filtros globais no Entity Framework Core, recuperação de registros e auditoria das operações de exclusão. |