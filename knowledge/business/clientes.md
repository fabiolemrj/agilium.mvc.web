# Clientes

## Objetivo

Módulo responsável pelo cadastro e gestão de clientes (pessoas físicas e jurídicas), incluindo limites de crédito, histórico de compras e vínculo com vendas e contas a receber.

---

# Visão Geral

O módulo de Clientes gerencia o cadastro completo de clientes da empresa. Cada cliente pode ter endereços, contatos, limite de crédito e histórico de transações. O cliente é referenciado nos módulos de Venda (IDCLIENTE) e Contas a Receber (IDCLIENTE).

---

# Responsabilidades

- Cadastro de clientes (CPF/CNPJ, nome, dados cadastrais)
- Gestão de endereços e contatos
- Definição de limite de crédito
- Vínculo com vendas e pedidos
- Histórico de compras do cliente
- Suporte a cliente como consumidor final (não identificado)

---

# Principais Entidades

- `Cliente` — Cadastro principal (IDCLIENTE, CPF/CNPJ, nome, situação)
- `ClienteEndereco` — Endereços do cliente
- `ClienteContato` — Contatos (telefone, e-mail)
- `ClientePreco` — Preços diferenciados por cliente (se configurado)

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-venda.md` — Venda vincula cliente opcional
- `docs/fluxos/fluxo-financeiro.md` — Contas a receber vinculam cliente

---

# APIs Relacionadas

- `agilium-manager-azure-api/V1/ClienteController.cs`
- `agilium-pdv-azure-api/` — consulta de cliente no PDV

---

# Regras de Negócio

Consultar:

`docs/business-rules/`

---

# Banco de Dados

Consultar:

`docs/database/`

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `knowledge/domain.md` — Modelo de domínio
- `docs/business-rules/usuario.md` — Regras de cliente/usuário

---

# Documentação Oficial

`docs/business/clientes/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `ClienteController.cs` no MVC para operações CRUD
2. Consultar `Cliente` model em `agilium-manager-azure-business/Models/`
3. Verificar validações de CPF/CNPJ e unicidade
4. Consultar regras de negócio em `docs/business-rules/`
5. Verificar relacionamento com Venda e ContaReceber

---

# Resumo

Clientes são a base para vendas, pedidos e contas a receber. O cadastro suporta pessoa física e jurídica, com limite de crédito configurável e vínculo a endereços e contatos.
