# Módulo Pedidos

## Objetivo

O módulo **Pedidos** gerencia pedidos de vendas, incluindo pedidos integrados com marketplace (Site Mercado), controle de itens, pagamentos e conversão em vendas.

---

# Responsabilidades

- Registro de pedidos
- Registro de itens do pedido
- Registro de pagamentos do pedido
- Integração com marketplace (PedidoSitemercado)
- Conversão de pedido em venda (PedidoVenda)
- Controle de situação do pedido

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Pedido | Registro principal |
| PedidoItem | Itens do pedido |
| PedidoPagamento | Pagamentos do pedido |
| PedidoSitemercado | Pedido originado do marketplace |
| PedidoItemSiteMercado | Itens do pedido marketplace |
| PedidoPagamentoSitemercado | Pagamento do pedido marketplace |
| PedidoVenda | Pedido convertido em venda |
| PedidoVendaItem | Itens do pedido convertido |

---

# Dependências

- Empresa
- Cliente
- Produto
- FormaPagamento

---

# Situações do Pedido

| Situação | Descrição |
|----------|-----------|
| Aberto | Pedido em andamento |
| Confirmado | Pedido confirmado |
| Cancelado | Pedido cancelado |
| Convertido | Transformado em Venda |

---

# Serviços Envolvidos

- PedidoService
- SiteMercadoService
- VendaService (conversão)

---

# Checklist

☐ Cliente vinculado

☐ Itens com preço e quantidade

☐ Pagamento definido

☐ Marketplace sincronizado (se aplicável)

---

# Conclusão

O módulo **Pedidos** atua como etapa anterior à venda, permitindo reserva e confirmação antes da efetivação. A integração com marketplace permite centralizar pedidos de múltiplos canais.
