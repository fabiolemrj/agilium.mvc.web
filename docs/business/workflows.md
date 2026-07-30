# Workflows de Negócio

## Objetivo

Documentar os principais processos de negócio implementados no Agilium Manager, descrevendo seus fluxos, participantes, regras, entidades envolvidas e interações entre módulos.

Este documento funciona como o catálogo oficial dos workflows da plataforma.

---

# Escopo

Este documento contempla:

- Catálogo de Workflows
- Fluxos de Negócio
- Participantes
- Entidades
- Casos de Uso
- Dependências
- Fluxos Alternativos
- Exceções
- Integrações

---

# Índice

- Visão Geral
- Organização dos Workflows
- Catálogo
- Estrutura de Documentação
- Fluxo Padrão
- Estados
- Tratamento de Exceções
- Convenções
- Atualização
- Documentação Relacionada

---

# Visão Geral

Os workflows representam os processos executados pelos usuários e pelo sistema durante a operação da plataforma.

Cada workflow descreve:

- objetivo;
- participantes;
- entidades envolvidas;
- sequência de execução;
- regras de negócio;
- exceções;
- integrações.

Os workflows não substituem as regras de negócio; eles demonstram **como** essas regras são aplicadas durante um processo.

---

# Organização dos Workflows

Cada módulo funcional deve possuir seus próprios workflows.

Exemplo:

```text
Produtos
Clientes
Pedidos
Vendas
Caixa
Estoque
Financeiro
Usuários
```

---

# Catálogo de Workflows

> O catálogo definitivo deverá ser consolidado após o levantamento funcional da solução.

| Workflow | Objetivo | Módulo | Documento |
|-----------|----------|--------|-----------|
| Cadastro de Produto | Manter produtos | Produtos | modules/products/workflows.md |
| Cadastro de Cliente | Manter clientes | Clientes | modules/customers/workflows.md |
| Realização de Venda | Registrar venda | Vendas | modules/sales/workflows.md |
| Abertura de Caixa | Iniciar operação | Caixa | modules/cash-register/workflows.md |
| Fechamento de Caixa | Encerrar operação | Caixa | modules/cash-register/workflows.md |
| Autenticação | Acesso ao sistema | Segurança | modules/security/workflows.md |

---

# Estrutura de Documentação

Todo workflow deve seguir um padrão.

## Identificação

```text
WF-001
```

---

## Nome

Nome do processo.

---

## Objetivo

Qual problema resolve.

---

## Módulo

Módulo responsável.

---

## Participantes

Exemplo:

- Usuário
- Sistema
- API
- Serviço Externo

---

## Pré-condições

Condições necessárias para iniciar o processo.

---

## Fluxo Principal

Descrição sequencial das etapas.

---

## Fluxos Alternativos

Caminhos opcionais.

---

## Exceções

Situações de erro previstas.

---

## Entidades Envolvidas

Exemplo:

- Cliente
- Produto
- Pedido
- Venda

---

## Serviços Utilizados

Exemplo:

- VendaService
- ProdutoService

---

## Regras Relacionadas

Referência para Business Rules.

---

## Integrações

APIs ou serviços externos envolvidos.

---

## Resultado Esperado

Estado final do processo.

---

# Fluxo Padrão

Representação genérica de um workflow.

```text
Usuário

↓

Controller

↓

Application Service

↓

Validação

↓

Business Rules

↓

Repository

↓

Persistência

↓

Resposta
```

---

# Estados

Sempre que aplicável, um workflow deve documentar os estados possíveis.

Exemplo:

```text
Novo

↓

Em Processamento

↓

Concluído
```

ou

```text
Aberto

↓

Aprovado

↓

Finalizado

↓

Cancelado
```

A máquina de estados deve refletir a implementação do módulo.

---

# Tratamento de Exceções

Todo workflow deve documentar:

- falhas de validação;
- regras impeditivas;
- erros de integração;
- erros de autorização;
- erros de persistência.

As regras de negócio devem utilizar o Notification Pattern quando aplicável.

---

# Convenções

Todo workflow deve:

- possuir identificador único;
- possuir objetivo claro;
- referenciar regras de negócio;
- referenciar entidades;
- documentar exceções;
- documentar integrações.

Sempre utilizar a mesma nomenclatura adotada pelo Glossário de Negócio.

---

# Atualização

Este documento deve ser atualizado sempre que:

- um novo processo de negócio for criado;
- um fluxo existente for alterado;
- novas integrações forem adicionadas;
- novas regras modificarem o comportamento do processo.

---

# Documentação Relacionada

## Negócio

- business/overview.md
- business/modules.md
- business/business-rules.md
- business/glossary.md

## Arquitetura

- architecture/layers.md
- architecture/overview.md

## Segurança

- security/authorization.md
- security/permissions.md