# Business Rules

## Objetivo

Este documento fornece uma visão geral das **regras de negócio** do Agilium Manager, indicando onde elas estão documentadas e como devem ser utilizadas durante o desenvolvimento.

A documentação oficial encontra-se em:

```text
docs/business-rules/
```

Este documento serve como um índice para desenvolvedores e agentes de IA, permitindo localizar rapidamente as regras aplicáveis a cada módulo do sistema.

---

# O que são Regras de Negócio

Regras de negócio representam o comportamento funcional esperado do sistema.

Elas definem:

- O que pode ser feito.
- O que não pode ser feito.
- Restrições.
- Validações.
- Fluxos.
- Exceções.
- Impactos em outros módulos.

As regras de negócio pertencem ao **Domínio** da aplicação.

---

# Onde Implementar

As regras de negócio devem ser implementadas exclusivamente na camada de domínio ou nos serviços de domínio/aplicação quando apropriado.

Fluxo recomendado:

```text
Controller

↓

Application Service

↓

Domain

↓

Repository
```

Nunca implementar regras de negócio em:

- Controllers
- Repositories
- Mappings
- DbContext
- Views

---

# Organização

Cada módulo do sistema possui seu próprio documento de regras de negócio.

Exemplo:

```text
docs/business-rules/

clientes.md

usuarios.md

produtos.md

estoque.md

caixa.md

pedidos.md

vendas.md

financeiro.md

licenciamento.md
```

---

# Estrutura Recomendada

Cada documento de regra de negócio deve conter:

- Objetivo
- Escopo
- Pré-condições
- Fluxo principal
- Validações
- Regras
- Exceções
- Pós-condições
- Impactos
- Casos especiais
- ADRs relacionados

---

# Módulos

## Clientes

Documenta regras relacionadas a:

- Cadastro
- Alteração
- Exclusão lógica
- Ativação
- Inativação
- Validações cadastrais

Documentação:

```text
docs/business-rules/clientes.md
```

---

## Usuários

Regras relacionadas a:

- Login
- Permissões
- Perfis
- Alteração de senha
- Bloqueios
- Auditoria

Documentação:

```text
docs/business-rules/usuarios.md
```

---

## Produtos

Inclui regras sobre:

- Cadastro
- Preço
- Unidade
- Estoque
- Situação
- Disponibilidade

Documentação:

```text
docs/business-rules/produtos.md
```

---

## Estoque

Responsável pelas regras de:

- Entrada
- Saída
- Transferência
- Ajuste
- Inventário
- Saldo

Documentação:

```text
docs/business-rules/estoque.md
```

---

## Pedidos

Regras relacionadas a:

- Criação
- Alteração
- Cancelamento
- Situações
- Itens
- Pagamentos

Documentação:

```text
docs/business-rules/pedidos.md
```

---

## Vendas

Inclui regras como:

- Abertura
- Inclusão de itens
- Descontos
- Acréscimos
- Pagamentos
- Finalização
- Cancelamento

Documentação:

```text
docs/business-rules/vendas.md
```

---

## Caixa

Regras referentes a:

- Abertura
- Fechamento
- Movimentações
- Sangrias
- Suprimentos
- Conferência

Documentação:

```text
docs/business-rules/caixa.md
```

---

## Financeiro

Documenta regras de:

- Contas
- Recebimentos
- Pagamentos
- Baixas
- Conciliação

Documentação:

```text
docs/business-rules/financeiro.md
```

---

## Licenciamento

Regras relacionadas a:

- Liberação de clientes
- Validade
- Bloqueios
- Renovação
- Consulta por APIs
- Ativação de aplicações

Documentação:

```text
docs/business-rules/licenciamento.md
```

---

# Fluxo de Aplicação

Sempre que uma funcionalidade for implementada:

```text
Identificar o módulo

↓

Consultar as regras de negócio

↓

Consultar ADRs

↓

Implementar

↓

Testar

↓

Atualizar documentação
```

---

# Relação com o Domínio

As regras de negócio fazem parte do domínio da aplicação.

Relacionamento:

```text
Domain

├── Entities

├── Value Objects

├── Domain Services

├── Domain Events

└── Business Rules
```

---

# ADRs Relacionados

As regras de negócio estão diretamente relacionadas às seguintes decisões arquiteturais:

| Tema | ADR |
|------|-----|
| Arquitetura em Camadas | ADR-0001 |
| Notification Pattern | ADR-0003 |
| Estratégia de Validação | ADR-0007 |
| Service Layer | ADR-0011 |
| Soft Delete | ADR-0016 |
| Auditoria | ADR-0017 |
| Estratégia de Testes | ADR-0020 |

Consulte:

```text
knowledge/decisions.md
```

---

# Antes de Implementar

Verifique:

- Existe documentação para o módulo?
- A regra já existe?
- Existe impacto em outros módulos?
- Existe um ADR relacionado?
- A implementação respeita a arquitetura?
- Foram considerados casos excepcionais?

---

# Documentação Relacionada

| Assunto | Documento |
|----------|-----------|
| Arquitetura | knowledge/architecture.md |
| Domínio | knowledge/domain.md |
| Banco de Dados | knowledge/database.md |
| APIs | knowledge/api.md |
| Desenvolvimento | knowledge/development.md |
| Padrões | knowledge/patterns.md |
| ADRs | knowledge/decisions.md |

---

# Documentação Oficial

Consulte sempre a documentação completa em:

```text
docs/business-rules/
```

Cada arquivo desse diretório contém a descrição detalhada das regras de negócio de um módulo específico, incluindo fluxos, validações, exceções e cenários de uso.

---

# Fluxo Recomendado para Agentes de IA

```text
Ler business-rules.md

↓

Identificar o módulo

↓

Consultar o documento específico

↓

Consultar ADRs relacionados

↓

Planejar implementação

↓

Executar alterações

↓

Criar ou atualizar testes

↓

Atualizar documentação
```

---

# Resumo

Este documento é um **índice das regras de negócio** do Agilium Manager.

Antes de implementar qualquer funcionalidade:

- identifique o módulo envolvido;
- consulte a documentação oficial das regras de negócio;
- implemente as validações na camada de domínio;
- siga os ADRs aplicáveis;
- mantenha a documentação sincronizada com a implementação.