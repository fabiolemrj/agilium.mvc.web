# ADR-0019 - Estratégia de Versionamento e Migrações de Banco de Dados

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager possui uma base de dados em constante evolução devido à inclusão de novos módulos, funcionalidades e integrações.

Ao longo da vida útil do sistema serão realizadas alterações como:

- Criação de tabelas;
- Inclusão de colunas;
- Alteração de tipos;
- Criação de índices;
- Ajustes de relacionamentos;
- Correções estruturais;
- Melhorias de performance.

Historicamente, alterações de banco realizadas manualmente geraram divergências entre ambientes, dificultando implantações, suporte e rastreabilidade.

Era necessário definir uma estratégia oficial para versionamento e evolução do banco de dados.

---

# Problema

Sem uma política de versionamento do banco de dados surgem diversos problemas:

- Bancos diferentes entre ambientes;
- Scripts executados fora de ordem;
- Dificuldade para reproduzir ambientes;
- Baixa rastreabilidade;
- Erros durante deploy;
- Falta de histórico das alterações.

Era necessário tornar toda alteração estrutural do banco reproduzível e controlada.

---

# Alternativas Consideradas

## Alternativa 1 — Alterações Manuais

Executar comandos SQL diretamente no banco.

### Vantagens

- Simplicidade.
- Rapidez para pequenas alterações.

### Desvantagens

- Sem histórico.
- Alto risco.
- Ambientes inconsistentes.
- Difícil automação.

---

## Alternativa 2 — Scripts SQL Versionados

Manter scripts SQL numerados.

### Vantagens

- Histórico parcial.
- Independente de tecnologia.

### Desvantagens

- Controle manual.
- Alto risco de execução incorreta.
- Difícil sincronização.

---

## Alternativa 3 — Entity Framework Core Migrations (Escolhida)

Utilizar o mecanismo oficial de Migrations do Entity Framework Core.

### Vantagens

- Histórico completo.
- Controle de versão.
- Integração com CI/CD.
- Automatização.
- Reprodutibilidade.

### Desvantagens

- Necessidade de disciplina no processo.
- Dependência do modelo EF Core.

---

# Decisão

Foi adotado o **Entity Framework Core Migrations** como mecanismo oficial para versionamento da estrutura do banco de dados.

Toda alteração estrutural deverá ser realizada através de uma Migration.

É proibido realizar alterações manuais diretamente em ambientes controlados (Homologação e Produção), exceto em situações emergenciais previamente aprovadas e posteriormente refletidas em uma Migration correspondente.

---

# Objetivos

Esta decisão possui os seguintes objetivos:

- Versionar a estrutura do banco.
- Garantir consistência entre ambientes.
- Facilitar deploy.
- Automatizar evolução do banco.
- Preservar histórico das alterações.
- Integrar com pipelines de CI/CD.

---

# Fluxo

```text
Alteração na Entidade

↓

Atualização do Mapping

↓

Criação da Migration

↓

Revisão

↓

Commit

↓

Pipeline

↓

Banco Atualizado
```

---

# Estrutura

Organização recomendada:

```text
Persistence/

├── Context/

│   └── ApplicationDbContext.cs

│

├── Mapping/

│   ├── ProdutoMap.cs

│   ├── ClienteMap.cs

│   └── ...

│

├── Migrations/

│   ├── 202607290001_CreateProduto.cs

│   ├── 202607300001_AddIndiceVenda.cs

│   └── ...

│

└── Seeds/
```

---

# Processo de Alteração

Toda alteração deverá seguir o fluxo:

1. Alterar a entidade.
2. Atualizar o Mapping.
3. Gerar Migration.
4. Revisar Migration.
5. Executar testes locais.
6. Publicar junto com a aplicação.

---

# Nome das Migrations

As Migrations deverão possuir nomes descritivos.

Exemplos:

```text
CreateProduto

CreateCliente

AddIndiceVenda

AddEmpresaIdPedido

AlterTamanhoDescricaoProduto

CreateTabelaAuditoria
```

Evitar nomes genéricos como:

```text
Migration1

Teste

NovaMigration
```

---

# Conteúdo da Migration

Cada Migration deverá conter apenas alterações relacionadas a um único contexto funcional.

Evitar:

- Alterações não relacionadas;
- Ajustes de múltiplos módulos;
- Grandes refatorações em uma única Migration.

---

# Seeds

Dados obrigatórios poderão ser inseridos através de mecanismos controlados.

Exemplos:

- Perfis padrão;
- Permissões;
- Configurações iniciais;
- Países;
- Estados;
- Moedas.

Dados operacionais nunca deverão ser inseridos automaticamente.

---

# Rollback

Toda Migration deverá suportar reversão através do método:

```text
Down()
```

Sempre que tecnicamente possível.

---

# Deploy

Fluxo recomendado:

```text
Publicação

↓

Backup

↓

Migration

↓

Validação

↓

Liberação
```

Nunca executar Migrations sem backup em produção.

---

# Ambientes

Todos os ambientes deverão utilizar exatamente o mesmo conjunto de Migrations.

Não é permitido criar estruturas específicas para um ambiente.

---

# Alterações Manuais

Caso uma alteração emergencial seja realizada diretamente no banco:

1. Documentar a alteração.
2. Criar Migration equivalente.
3. Versionar no repositório.
4. Validar todos os ambientes.

---

# Índices

Toda criação de índice deverá ser documentada.

Antes da criação verificar:

- Volume esperado;
- Consultas afetadas;
- Impacto em INSERT/UPDATE.

---

# Integridade

Toda alteração estrutural deverá preservar:

- Chaves primárias;
- Chaves estrangeiras;
- Constraints;
- Integridade referencial.

---

# Benefícios

- Histórico completo.
- Banco reproduzível.
- Deploy automatizado.
- Ambientes consistentes.
- Facilidade para novos desenvolvedores.
- Integração com CI/CD.

---

# Desvantagens

- Necessidade de revisão das Migrations.
- Disciplina na geração e versionamento.

---

# Riscos

Caso esta estratégia não seja seguida:

- Bancos inconsistentes.
- Erros em produção.
- Diferenças entre ambientes.
- Perda de rastreabilidade.
- Deploys falhos.

---

# Impacto

Esta decisão impacta:

- Entity Framework Core
- Banco de Dados
- APIs
- MVC
- DevOps
- CI/CD
- Docker
- Deploy
- Infraestrutura

---

# Plano de Implementação

1. Centralizar todas as Migrations na camada de Persistência.
2. Atualizar o processo de desenvolvimento para incluir geração de Migrations.
3. Revisar Migrations antes do merge.
4. Automatizar execução em pipelines quando aplicável.
5. Definir política de Seeds.
6. Documentar alterações relevantes.
7. Treinar a equipe sobre o processo.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Toda alteração estrutural possui Migration correspondente.
- Todas as Migrations possuem nomes descritivos.
- O método `Down()` está implementado quando possível.
- Não existem alterações manuais não documentadas.
- Todos os ambientes utilizam o mesmo histórico de Migrations.
- O processo de deploy considera backup e validação da base.

---

# ADRs Relacionados

- ADR-0004 — Entity Framework Core
- ADR-0009 — Dependency Injection
- ADR-0012 — Estratégia de Containerização e Deploy
- ADR-0016 — Estratégia de Soft Delete
- ADR-0017 — Estratégia de Auditoria
- ADR-0018 — Gerenciamento de Configurações
- ADR-0020 — Estratégia de Testes

---

# Referências

- Microsoft — Entity Framework Core Migrations
- Microsoft — Applying Migrations
- Microsoft — Data Seeding
- Martin Fowler — Evolutionary Database Design
- Continuous Delivery — Jez Humble

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | **2026-07-29** | Criação da ADR definindo o Entity Framework Core Migrations como estratégia oficial para versionamento e evolução da estrutura do banco de dados do Agilium Manager, estabelecendo diretrizes para criação, revisão, rollback, deploy e sincronização entre ambientes. |