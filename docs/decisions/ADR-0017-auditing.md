# ADR-0017 - Estratégia de Auditoria (Auditing Strategy)

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é uma plataforma de gestão empresarial utilizada para controlar operações críticas de negócio, incluindo:

- Usuários
- Empresas
- Produtos
- Estoque
- Vendas
- Financeiro
- Caixa
- Licenciamento
- Configurações
- Integrações

Grande parte dessas operações altera informações que precisam ser rastreadas por motivos de:

- Segurança;
- Auditoria;
- Conformidade;
- Suporte técnico;
- Investigação de incidentes;
- Recuperação de informações.

Era necessário definir uma estratégia única de auditoria para toda a plataforma.

---

# Problema

Sem uma política padronizada de auditoria surgem diversos problemas:

- Não é possível identificar quem realizou uma alteração;
- Não existe histórico das modificações;
- Dificuldade para investigar problemas;
- Falta de rastreabilidade;
- Baixa confiabilidade dos dados;
- Dificuldade para atender auditorias externas.

Era necessário registrar todas as alterações relevantes da aplicação.

---

# Alternativas Consideradas

## Alternativa 1 — Não realizar auditoria

### Vantagens

- Implementação simples.
- Nenhum impacto adicional.

### Desvantagens

- Ausência de histórico.
- Sem rastreabilidade.
- Alto risco operacional.

---

## Alternativa 2 — Auditoria Manual

Cada Service grava seus próprios logs.

### Vantagens

- Flexibilidade.

### Desvantagens

- Código duplicado.
- Alto risco de inconsistência.
- Difícil manutenção.

---

## Alternativa 3 — Auditoria Centralizada (Escolhida)

A auditoria é executada automaticamente pela infraestrutura da aplicação.

### Vantagens

- Padronização.
- Baixo acoplamento.
- Fácil manutenção.
- Maior confiabilidade.

### Desvantagens

- Configuração inicial mais elaborada.

---

# Decisão

Foi adotada uma estratégia de **Auditoria Centralizada** para registrar automaticamente todas as operações relevantes realizadas pelos usuários.

Toda alteração em entidades de negócio deverá possuir informações mínimas de auditoria.

Sempre que possível, a auditoria será implementada de forma transparente através da camada de Persistência (Entity Framework Core) ou infraestrutura compartilhada.

---

# Objetivos

Esta estratégia possui os seguintes objetivos:

- Garantir rastreabilidade.
- Identificar responsáveis pelas alterações.
- Facilitar investigações.
- Apoiar auditorias internas e externas.
- Melhorar segurança.
- Preservar histórico operacional.

---

# Informações Auditadas

Toda entidade de negócio deverá possuir, sempre que aplicável, os seguintes campos:

```text
DataCadastro

UsuarioCadastro

DataAlteracao

UsuarioAlteracao

DataExclusao

UsuarioExclusao
```

Quando necessário:

```text
EmpresaCadastro

MotivoAlteracao

VersaoRegistro
```

---

# Operações Auditadas

As seguintes operações deverão ser registradas:

- Inclusão;
- Alteração;
- Exclusão lógica;
- Restauração;
- Login;
- Logout;
- Alteração de senha;
- Alteração de permissões;
- Alteração de configurações;
- Operações administrativas.

---

# Fluxo

```text
Request

↓

Controller

↓

Service

↓

Repository

↓

Persistência

↓

Auditoria

↓

Banco de Dados
```

---

# Entity Framework Core

A auditoria deverá ocorrer automaticamente durante o `SaveChanges()` ou `SaveChangesAsync()`.

Exemplo:

```text
Entity

↓

Change Tracker

↓

Preenchimento dos Campos

↓

SaveChanges()
```

O desenvolvedor não deverá preencher manualmente os campos de auditoria.

---

# Responsabilidades

## Service

Responsável apenas por executar o caso de uso.

Não deve preencher:

- DataCadastro;
- UsuarioCadastro;
- DataAlteracao;
- UsuarioAlteracao.

---

## Persistência

Responsável por:

- Preencher automaticamente os campos;
- Identificar o usuário autenticado;
- Registrar datas em UTC;
- Atualizar somente os campos necessários.

---

# Data e Hora

Todas as datas deverão ser armazenadas em **UTC**.

Exemplo:

```
2026-07-29T18:45:31Z
```

A conversão para horário local será responsabilidade da camada de apresentação.

---

# Usuário

Sempre que existir autenticação, deverá ser registrado:

```text
UsuarioId

EmpresaId
```

Caso a operação seja automática:

```text
Sistema
```

---

# Exclusão

A auditoria deverá ser integrada ao Soft Delete (ADR-0016).

Ao excluir um registro:

```text
Ativo = false

↓

DataExclusao

↓

UsuarioExclusao
```

---

# Histórico Completo

Quando necessário manter histórico detalhado das alterações, poderão ser utilizadas tabelas específicas de auditoria.

Exemplo:

```text
Produto

↓

ProdutoHistorico

↓

ProdutoAuditoria
```

Cada registro poderá armazenar:

- Valor anterior;
- Valor atual;
- Campo alterado;
- Usuário;
- Data.

---

# Segurança

Os registros de auditoria:

- Não poderão ser alterados manualmente;
- Não poderão ser excluídos pela aplicação;
- Deverão possuir acesso restrito.

---

# Integração com Logging

Auditoria não substitui Logging.

| Auditoria | Logging |
|-----------|---------|
| Histórico permanente | Diagnóstico |
| Alterações de dados | Eventos técnicos |
| Quem alterou | Como ocorreu |
| Persistente | Operacional |

---

# Benefícios

- Rastreabilidade.
- Histórico permanente.
- Maior segurança.
- Apoio à auditoria.
- Facilidade de investigação.
- Melhor governança.

---

# Desvantagens

- Pequeno aumento do armazenamento.
- Maior quantidade de dados persistidos.
- Necessidade de manutenção dos históricos.

---

# Riscos

Caso esta estratégia não seja seguida:

- Impossibilidade de identificar alterações.
- Perda de histórico.
- Dificuldade para auditorias.
- Baixa confiabilidade.
- Investigações demoradas.

---

# Impacto

Esta decisão impacta:

- Banco de Dados
- Entity Framework Core
- Services
- Repositories
- APIs
- MVC
- Segurança
- Relatórios
- Administração

---

# Plano de Implementação

1. Criar interface base para entidades auditáveis.
2. Adicionar campos padrão de auditoria.
3. Implementar preenchimento automático no `DbContext`.
4. Integrar com autenticação para identificação do usuário.
5. Integrar com Soft Delete.
6. Criar mecanismos de histórico quando necessário.
7. Atualizar documentação técnica.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Todas as entidades auditáveis possuem os campos padrão.
- Os campos são preenchidos automaticamente.
- As datas são armazenadas em UTC.
- O usuário autenticado é registrado nas operações.
- Exclusões utilizam Soft Delete integrado à auditoria.
- Os registros de auditoria não podem ser alterados pela aplicação.

---

# ADRs Relacionados

- ADR-0004 — Entity Framework Core
- ADR-0005 — Estratégia de Autenticação
- ADR-0009 — Dependency Injection
- ADR-0011 — Service Layer
- ADR-0013 — Estratégia de Logging
- ADR-0016 — Estratégia de Soft Delete
- ADR-0018 — Gerenciamento de Configurações

---

# Referências

- Microsoft — Entity Framework Core Change Tracking
- Microsoft — SaveChanges Interceptors
- Microsoft — Audit Logging Guidance
- Martin Fowler — Patterns of Enterprise Application Architecture
- OWASP Logging Cheat Sheet

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | **2026-07-29** | Criação da ADR definindo a estratégia oficial de auditoria do Agilium Manager, estabelecendo auditoria centralizada, preenchimento automático dos campos de rastreabilidade, integração com Entity Framework Core, Soft Delete e autenticação, garantindo histórico e governança das alterações realizadas na plataforma. |