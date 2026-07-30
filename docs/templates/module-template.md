# Module Template

# Objetivo

Template padrão para documentação dos módulos de negócio do Agilium Manager.

Cada documento deve descrever a responsabilidade do módulo, sua arquitetura, componentes, regras de negócio, integrações e dependências.

A documentação deve refletir exclusivamente a implementação existente ou, quando aplicável, a especificação aprovada da funcionalidade.

---

# [Nome do Módulo]

| Campo | Valor |
|--------|-------|
| **Status** | Ativo / Obsoleto / Planejado |
| **Projetos** | |
| **Responsável** | |
| **Versão** | |

---

# Objetivo

Descrever qual capacidade de negócio este módulo fornece.

---

# Escopo

## Este módulo cobre

- ...

## Este módulo NÃO cobre

- ...

---

# Contexto

Explicar:

- objetivo do módulo;
- processos atendidos;
- relacionamento com outros módulos;
- importância para a solução.

---

# Arquitetura

Descrever a posição do módulo na arquitetura.

Exemplo:

```
Interface (MVC)

↓

Business

↓

Infrastructure

↓

Banco de Dados
```

---

# Componentes

## Controllers

| Controller | Responsabilidade |
|------------|------------------|
| | |

---

## Services

| Service | Interface | Responsabilidade |
|---------|-----------|------------------|
| | | |

---

## Repositories

| Repository | Interface | Responsabilidade |
|------------|-----------|------------------|
| | | |

---

## ViewModels

| ViewModel | Finalidade |
|-----------|------------|
| | |

---

## Entities

| Entidade | Tabela | Finalidade |
|----------|--------|------------|
| | | |

---

# Fluxo Funcional

Documentar o fluxo principal do módulo.

Exemplo:

```
Usuário

↓

Controller

↓

Service

↓

Repository

↓

Banco de Dados

↓

Resposta
```

---

# Regras de Negócio

Relacionar as principais regras implementadas pelo módulo.

Referenciar documentos específicos de regras de negócio quando existirem.

---

# Persistência

Documentar:

- entidades;
- tabelas;
- relacionamentos;
- consultas relevantes;
- uso de Entity Framework Core;
- uso de Dapper (quando aplicável).

---

# Integrações

Relacionar integrações com:

- outros módulos;
- APIs;
- serviços externos;
- mensageria;
- processos automáticos.

---

# Dependências

Documentar dependências internas e externas.

Exemplos:

- AutoMapper;
- FluentValidation;
- Notification Pattern;
- Unit of Work;
- bibliotecas compartilhadas;
- módulos relacionados.

---

# Segurança

Documentar:

- autenticação;
- autorização;
- permissões;
- auditoria;
- proteção de dados.

---

# Impacto

Identificar quais áreas da solução dependem deste módulo.

Exemplos:

- outros módulos;
- APIs;
- interface;
- banco de dados;
- integrações;
- jobs.

---

# Limitações Conhecidas

Registrar:

- funcionalidades não implementadas;
- comportamentos ainda não analisados;
- restrições técnicas;
- pontos dependentes de investigação adicional.

---

# Boas Práticas

Documentar padrões específicos adotados pelo módulo.

Registrar apenas práticas efetivamente observadas na implementação.

---

# Documentação Relacionada

Relacionar documentos associados.

Exemplos:

- Arquitetura
- APIs
- Regras de Negócio
- Banco de Dados
- Fluxos
- Funcionalidades
- Módulos relacionados

---

# Histórico

| Versão | Data | Alteração |
|---------|------|-----------|
| 1.0 | | Criação |