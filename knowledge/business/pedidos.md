# Pedidos

## Objetivo

Módulo responsável pela gestão de pedidos de venda, incluindo criação, conversão para venda definitiva e rastreamento de status.

---

# Visão Geral

Pedidos de venda representam vendas futuras ou programadas. Podem ser convertidos em vendas definitivas. O modelo `PedidoVenda` existe como entidade separada de `Venda`, permitindo rastreamento de origem (`EOrigemVenda.PEDIDO`).

---

# Responsabilidades

- Criação e gestão de pedidos de venda
- Conversão de pedido em venda definitiva
- Rastreamento de origem da venda (DIRETA vs PEDIDO)
- Integração com site mercado (marketplace)

---

# Principais Entidades

- `PedidoVenda` — Pedido de venda
- `PedidoVendaItem` — Itens do pedido
- `PedidoSitemercado` — Pedidos originados de marketplace

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-venda.md` — Conversão pedido → venda

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/VendaController.cs`
- `agilium-manager-azure-api/V1/`

---

# Regras de Negócio

Consultar:

`docs/business-rules/pedidos.md`

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

- `knowledge/business/vendas.md` — Destino da conversão
- `knowledge/business/integracoes.md` — Marketplace

---

# Documentação Oficial

`docs/business/pedidos/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `PedidoVenda` model em `agilium-manager-azure-business/Models/`
2. Verificar `PedidoService` para lógica de conversão
3. Verificar `IPedidoDapperRepository` para consultas
4. Verificar `EOrigemVenda.PEDIDO` no modelo de Venda

---

# Resumo

Pedidos representam vendas programadas que podem ser convertidas em vendas definitivas. O rastreamento de origem é feito via `EOrigemVenda` (DIRETA ou PEDIDO).
