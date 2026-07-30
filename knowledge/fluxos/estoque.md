# Estoque

## Objetivo

Documentar o fluxo de controle de estoque: 5 origens de movimentação (Compra, Venda, Devolução, Perda, Inventário), rastreabilidade e cálculo de custo médio.

---

## Visão Geral

O estoque é atualizado por 5 fluxos distintos. Toda movimentação gera `EstoqueHistorico` com rastreabilidade completa. O saldo é por produto × estoque (`EstoqueProduto`). O custo médio é recalculado automaticamente via `IProdutoDapper.AtualizarCustoMedio()`.

---

## Fluxo Principal

```
         ┌──────────────────────────────────────┐
         │            ESTOQUE                    │
         │  (EstoqueProduto — Saldo por produto) │
         └──────────────────────────────────────┘
              ▲         ▲         ▲         ▲         ▲
              │         │         │         │         │
         ┌────┴───┐ ┌───┴────┐ ┌──┴───┐ ┌───┴───┐ ┌──┴─────┐
         │ Compra │ │Devolução│ │Venda │ │ Perda │ │Invent. │
         │(entrada)│ │(entrada)│ │(saída)│ │(saída)│ │(ajuste)│
         └────────┘ └────────┘ └──────┘ └───────┘ └────────┘
```

---

## Entrada por Compra

```
EfetivarCompra() → RealizaEntrada()
      ├── Atualiza EstoqueProduto (+)
      ├── EstoqueHistorico (Tipo=Entrada, IDCOMPRA)
      └── Recalcula custo médio
```

## Saída por Venda

```
RealizarVenda() → baixa automática
      ├── EstoqueProduto (-)
      ├── EstoqueHistorico (Tipo=Saída, IDVENDA)
      └── Valida saldo >= quantidade
```

## Entrada por Devolução

```
Devolução finalizada → EntradaEstoque()
      ├── EstoqueProduto (+)
      └── EstoqueHistorico (Tipo=Entrada, IDDEVOLUCAO)
```

## Saída por Perda

```
Perda registrada → SaidaEstoque()
      ├── EstoqueProduto (-)
      └── EstoqueHistorico (Tipo=Saída, IDPERDA, motivo)
```

## Ajuste por Inventário

```
Inventário finalizado → AjustarSaldo()
      ├── Calcula diferença (contado - sistema)
      ├── EstoqueProduto = novaQuantidade
      └── EstoqueHistorico (Tipo=Ajuste, IDINVENTARIO)
```

---

## Regras de Negócio

- Saldo não pode ficar negativo (salvo configuração)
- Toda movimentação gera `EstoqueHistorico`
- Estoque é por empresa
- Custo médio recalculado a cada entrada

---

## Módulos Envolvidos

- `knowledge/business/estoque.md`
- `knowledge/business/compras.md`
- `knowledge/business/vendas.md`

---

## APIs Relacionadas

- `agilum.mvc.web/Controllers/EstoqueController.cs`
- `IEstoqueDapperRepository` — `RealizaEntrada()`, `RealizaRetirada()`
- `IProdutoDapper` — `AtualizarCustoMedio()`

---

## Banco de Dados

- `estoque` — Locais de armazenamento
- `estoque_produto` — Saldo por produto × estoque
- `estoque_historico` — Rastreabilidade completa

---

## ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

## Documentação Relacionada

- `docs/fluxos/fluxo-estoque.md` — Documentação oficial detalhada

---

## Documentação Oficial

`docs/fluxos/fluxo-estoque.md`

---

## Fluxo Recomendado para Agentes de IA

1. Verificar `EstoqueService` para operações de entrada/saída/ajuste
2. Verificar `EstoqueHistorico` para rastreabilidade
3. Verificar `IProdutoDapper.AtualizarCustoMedio()` para custo médio
4. Consultar `docs/fluxos/fluxo-estoque.md` para fluxos detalhados

---

## Resumo

5 origens de movimentação com rastreabilidade completa via `EstoqueHistorico`. Saldo por produto × estoque. Custo médio recalculado automaticamente. Venda valida saldo antes da baixa.
