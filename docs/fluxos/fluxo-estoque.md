# Fluxo de Estoque

## Objetivo

Documentar o fluxo de movimentação de **Estoque** no Agilium Manager, cobrindo entradas (compras, devoluções), saídas (vendas, perdas) e ajustes (inventário).

---

## Visão Geral

O estoque é atualizado por **5 origens** diferentes:

```
         ┌──────────────────────────────────────┐
         │            ESTOQUE                    │
         │  (EstoqueProduto — Saldo por produto) │
         └──────────────────────────────────────┘
              ▲         ▲         ▲         ▲         ▲
              │         │         │         │         │
         ┌────┴───┐ ┌───┴────┐ ┌──┴───┐ ┌───┴───┐ ┌──┴─────┐
         │ Compra │ │Devoluç.│ │Venda │ │ Perda │ │Invent. │
         │(entrada)│ │(entrada)│ │(saída)│ │(saída)│ │(ajuste)│
         └────────┘ └────────┘ └──────┘ └───────┘ └────────┘
```

---

## Fluxo: Entrada por Compra

```
[Compra Efetivada]
      │
      ▼
CompraService.EfetivarCompra(id)
      │
      ├── Validar situação da compra (Aberta?)
      ├── Validar itens
      │
      ▼
Para cada CompraItem:
      │
      ▼
EstoqueService.EntradaEstoque(idProduto, idEstoque, quantidade, valorUnitario)
      │
      ├── Obter EstoqueProduto (produto × estoque)
      │     │
      │     ├── Existe? → Atualizar quantidade (+)
      │     └── Não existe? → Criar EstoqueProduto
      │
      ├── Criar EstoqueHistorico
      │     ├── Tipo = Entrada (Compra)
      │     ├── Quantidade
      │     ├── IDCOMPRA (referência)
      │     └── DataHora
      │
      ▼
EstoqueService.Salvar()
```

---

## Fluxo: Saída por Venda

```
[Venda Finalizada]
      │
      ▼
VendaService.FinalizarVenda(id)
      │
      ▼
Para cada VendaItem:
      │
      ▼
EstoqueService.SaidaEstoque(idProduto, idEstoque, quantidade)
      │
      ├── Obter EstoqueProduto
      │     │
      │     ├── Saldo >= quantidade?
      │     │     ├── Sim → Atualizar quantidade (-)
      │     │     └── Não → Notificar("Estoque insuficiente")
      │     │
      │     └── Não existe? → Notificar("Produto sem estoque")
      │
      ├── Criar EstoqueHistorico
      │     ├── Tipo = Saída (Venda)
      │     ├── Quantidade
      │     ├── IDVENDA (referência)
      │     └── DataHora
      │
      ▼
EstoqueService.Salvar()
```

---

## Fluxo: Entrada por Devolução

```
[Devolução Finalizada]
      │
      ▼
DevolucaoService.FinalizarDevolucao(id)
      │
      ▼
Para cada DevolucaoItem:
      │
      ▼
EstoqueService.EntradaEstoque(idProduto, idEstoque, quantidade, 0)
      │
      ├── Atualizar EstoqueProduto (+)
      │
      ├── Criar EstoqueHistorico
      │     ├── Tipo = Entrada (Devolução)
      │     └── IDDEVOLUCAO (referência)
      │
      ▼
EstoqueService.Salvar()
```

---

## Fluxo: Saída por Perda

```
[Perda Registrada]
      │
      ▼
PerdaService.RegistrarPerda(perda)
      │
      ▼
EstoqueService.SaidaEstoque(idProduto, idEstoque, quantidade)
      │
      ├── Atualizar EstoqueProduto (-)
      │
      ├── Criar EstoqueHistorico
      │     ├── Tipo = Saída (Perda)
      │     ├── Motivo (quebra, validade, furto)
      │     └── IDPERDA (referência)
      │
      ▼
EstoqueService.Salvar()
```

---

## Fluxo: Ajuste por Inventário

```
[Inventário Finalizado]
      │
      ▼
InventarioService.FinalizarInventario(id)
      │
      ▼
Para cada InventarioItem com divergência:
      │
      ▼
EstoqueService.AjustarSaldo(idProduto, idEstoque, novaQuantidade)
      │
      ├── Calcular diferença (quantidade contada - quantidade sistema)
      │
      ├── Atualizar EstoqueProduto (= novaQuantidade)
      │
      ├── Criar EstoqueHistorico
      │     ├── Tipo = Ajuste (Inventário)
      │     ├── Quantidade (positiva ou negativa)
      │     └── IDINVENTARIO (referência)
      │
      ▼
EstoqueService.Salvar()
```

---

## Entidades Envolvidas

| Entidade | Papel |
|----------|-------|
| `Estoque` | Local de armazenamento |
| `EstoqueProduto` | Saldo de um produto em um estoque |
| `EstoqueHistorico` | Todas as movimentações (rastreabilidade) |
| `Produto` | Produto movimentado |
| `Empresa` | Contexto da operação |

---

## Regras de Negócio

- **Saldo não pode ficar negativo** (salvo configuração específica)
- **Toda** movimentação gera `EstoqueHistorico`
- Estoque é **por empresa**
- Venda só finaliza se **todos** os itens têm saldo
- Compra efetivada atualiza estoque **item a item**
- Inventário gera ajuste pela **diferença** (pode ser positivo ou negativo)

---

## Serviços Envolvidos

- `EstoqueService`
- `CompraService` (entrada)
- `VendaService` (saída)
- `DevolucaoService` (entrada)
- `PerdaService` (saída)
- `InventarioService` (ajuste)
