# Prompt: Migration

# Objetivo

Template para criação, revisão ou manutenção de alterações estruturais no banco de dados do Agilium Manager.

Este prompt deve ser utilizado sempre que houver necessidade de evoluir o modelo de dados da aplicação, preservando a compatibilidade com a arquitetura da solução e a integridade dos dados existentes.

---

# Quando utilizar

Utilize este prompt para:

- criar migrations;
- revisar migrations existentes;
- alterar entidades;
- alterar o modelo do Entity Framework Core;
- evoluir o banco de dados;
- revisar impactos estruturais.

---

# Prompt

```text
Realize o gerenciamento da alteração estrutural abaixo:

[MIGRATION_OU_ALTERAÇÃO]

Antes de qualquer alteração, analisar completamente a estrutura atual do banco e os impactos na aplicação.

---

## 1. Contexto

Identificar:

- projeto afetado;
- DbContext envolvido;
- entidades relacionadas;
- tabelas afetadas;
- relacionamentos existentes;
- impacto em outros módulos.

---

## 2. Arquitetura

Verificar integração com:

Entity Framework Core

Repository Pattern

Unit of Work

Services

AutoMapper

ViewModels

Notification Pattern

---

## 3. Alterações

Documentar todas as modificações propostas:

- novas tabelas;
- novas colunas;
- alterações de colunas;
- índices;
- chaves primárias;
- chaves estrangeiras;
- relacionamentos;
- restrições;
- remoções.

Explicar o objetivo de cada alteração.

---

## 4. Implementação

Quando aplicável, revisar:

- código da Migration;
- método Up();
- método Down();
- impacto nas entidades;
- mapeamentos do Entity Framework Core;
- atualizações nos Repositories.

---

## 5. Integridade

Verificar:

- compatibilidade com dados existentes;
- necessidade de migração de dados;
- impacto em integrações;
- impacto em consultas existentes;
- compatibilidade com Dapper.

---

## 6. Performance

Avaliar:

- índices;
- tipos de dados;
- relacionamentos;
- consultas afetadas;
- impacto em operações críticas.

---

## 7. Segurança

Verificar:

- impacto em permissões;
- exposição de dados;
- auditoria (quando aplicável);
- consistência das restrições.

---

## 8. Impacto

Identificar impacto em:

Entities

Repositories

Services

Controllers

ViewModels

AutoMapper

Consultas Dapper

Integrações

Documentação

---

## 9. Validação

Confirmar:

- consistência do modelo;
- reversibilidade da alteração (quando aplicável);
- compatibilidade entre entidades e banco;
- atualização dos mapeamentos.

---

## 10. Resultado

Apresentar:

Resumo Executivo

Arquivos Alterados

Entidades Alteradas

Tabelas Alteradas

Relacionamentos Alterados

Impactos

Riscos

Plano de Implantação

Plano de Rollback

Recomendações
```

---

# Parâmetros

| Parâmetro | Descrição | Exemplo |
|-----------|-----------|---------|
| `MIGRATION_OU_ALTERAÇÃO` | Nome da migration ou descrição da alteração | `AddHistoricoPrecoProduto`, `Adicionar coluna DataCancelamento em Pedido`, `Criar tabela LicencaCliente` |

---

# Resultado Esperado

A análise deve:

- preservar a integridade dos dados;
- manter compatibilidade com a arquitetura do Agilium Manager;
- considerar os impactos em Entity Framework Core, Dapper, Repository Pattern e Unit of Work;
- identificar riscos e dependências antes da implementação;
- documentar claramente as alterações estruturais e seus impactos.

Caso o projeto ou a funcionalidade analisada não utilize EF Core Migrations para evolução do banco de dados, o relatório deve registrar explicitamente a estratégia de atualização identificada (scripts SQL, processo manual, ferramenta externa ou outro mecanismo), em vez de presumir o uso de migrations.