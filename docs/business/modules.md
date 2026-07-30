# Módulos de Negócio

## Objetivo

Documentar os módulos funcionais que compõem o ecossistema Agilium Manager, descrevendo seus objetivos, responsabilidades, limites, entidades, serviços e integrações.

Este documento representa o mapa funcional da plataforma.

---

# Escopo

Este documento contempla:

- Catálogo de Módulos
- Responsabilidades
- Limites Funcionais
- Entidades
- Serviços
- Fluxos
- Dependências
- Integrações

---

# Índice

- Visão Geral
- Arquitetura Funcional
- Catálogo de Módulos
- Estrutura dos Módulos
- Dependências
- Integrações
- Convenções
- Atualização
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager é organizado em módulos de negócio independentes, cada um responsável por um conjunto específico de funcionalidades.

Essa divisão facilita:

- manutenção;
- evolução da plataforma;
- reutilização;
- organização da documentação.

Cada módulo possui documentação própria.

---

# Arquitetura Funcional

```text
                Plataforma

                     │

 ┌────────────────────────────────────┐
 │          Módulos de Negócio        │
 └────────────────────────────────────┘

     │        │        │        │

 Produtos  Clientes  Vendas  Financeiro

     │        │        │        │

     └────────┴────────┴────────┘

            Serviços Compartilhados
```

---

# Catálogo de Módulos

> O catálogo abaixo representa a organização funcional da plataforma e deverá ser validado durante o levantamento completo da solução.

| Módulo | Objetivo | Documentação |
|---------|----------|--------------|
| Usuários | Gestão de usuários | modules/users.md |
| Empresas | Gestão de empresas | modules/companies.md |
| Clientes | Gestão de clientes | modules/customers.md |
| Produtos | Cadastro de produtos | modules/products.md |
| Pedidos | Gestão de pedidos | modules/orders.md |
| Vendas | Processo de venda | modules/sales.md |
| Caixa | Controle operacional do caixa | modules/cash-register.md |
| Financeiro | Operações financeiras | modules/financial.md |
| Estoque | Controle de estoque | modules/inventory.md |
| Configurações | Configurações do sistema | modules/settings.md |
| Segurança | Autenticação e autorização | modules/security.md |

---

# Estrutura dos Módulos

Cada módulo deve seguir uma documentação padronizada.

## Objetivo

Qual problema o módulo resolve.

---

## Responsabilidades

O que pertence ao módulo.

---

## Limites

O que não pertence ao módulo.

---

## Principais Entidades

Exemplo:

- Produto
- Categoria
- Marca

---

## Serviços

Exemplo:

- ProdutoService
- ProdutoRepository

---

## Casos de Uso

- Cadastrar
- Alterar
- Consultar
- Excluir

---

## Regras de Negócio

Referência para o documento de regras do módulo.

---

## Fluxos

Referência para workflows específicos.

---

## Integrações

Sistemas externos utilizados.

---

## Dependências

Quais módulos são utilizados.

---

# Exemplo

## Produtos

### Objetivo

Gerenciar o catálogo de produtos.

### Entidades

- Produto
- Categoria
- Unidade

### Serviços

- ProdutoService

### Casos de Uso

- Cadastro
- Consulta
- Atualização
- Exclusão

### Dependências

- Estoque
- Financeiro

---

# Dependências entre Módulos

```text
Vendas

├── Clientes
├── Produtos
├── Caixa
└── Financeiro
```

As dependências devem ser unidirecionais sempre que possível.

---

# Integrações

Cada módulo deve documentar:

- APIs utilizadas;
- eventos publicados;
- eventos consumidos;
- integrações externas.

---

# Convenções

Todo módulo deve possuir:

- documentação própria;
- regras de negócio;
- workflows;
- casos de uso;
- entidades;
- integrações.

---

# Atualização

Sempre que:

- um novo módulo for criado;
- um módulo for descontinuado;
- responsabilidades forem alteradas;
- novas integrações forem adicionadas;

este documento deverá ser atualizado.

---

# Documentação Relacionada

- overview.md
- business-rules.md
- workflows.md
- glossary.md
- architecture/layers.md