# Pagamentos

## Objetivo

Módulo responsável pelas formas de pagamento, moedas, vales e gestão de recebimentos no PDV e nas vendas.

---

# Visão Geral

O módulo de Pagamentos gerencia as formas de pagamento disponíveis (dinheiro, cartão, vale), moedas (BRL, USD, etc.) e vales (alimentação, refeição, presente, troca). As moedas são usadas em `VendaMoeda` (pagamento da venda) e `CaixaMoeda` (saldo do caixa). Vales são consumidos via `UtilizarValePorVenda`.

---

# Responsabilidades

- Cadastro de formas de pagamento
- Cadastro de moedas e cotações
- Gestão de vales (criação, consumo, saldo)
- Registro de `VendaMoeda` (pagamento por moeda na venda)
- Registro de `CaixaMoeda` (saldo do caixa por moeda)

---

# Principais Entidades

- `FormaPagamento` — Meios de pagamento
- `Moeda` — Moedas (BRL, USD, EUR)
- `Vale` — Vales (alimentação, presente, troca)
- `VendaMoeda` — Pagamento registrado na venda
- `CaixaMoeda` — Saldo do caixa por moeda

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-venda.md` — VendaMoeda no pagamento
- `docs/fluxos/fluxo-caixa.md` — CaixaMoeda na conferência

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/FormaPagamentoController.cs`
- `agilum.mvc.web/Controllers/MoedaController.cs`
- `agilum.mvc.web/Controllers/ValeController.cs`
- `agilium-manager-azure-api/V1/`

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

- `knowledge/business/vendas.md` — Pagamento na venda
- `knowledge/business/caixa.md` — Saldo por moeda no caixa

---

# Documentação Oficial

`docs/business/pagamentos/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `VendaMoeda` model — como o pagamento é registrado na venda
2. Verificar `CaixaMoeda` model — saldo por moeda no caixa
3. Verificar `Vale` model e `IValeDapperRepository.UtilizarValePorVenda()`
4. Verificar `FormaPagamento` e `Moeda` models

---

# Resumo

Pagamentos conectam vendas e caixa através de `VendaMoeda` e `CaixaMoeda`. Vales são gerenciados com saldo e consumidos automaticamente na venda. Múltiplas formas de pagamento são suportadas por venda.
