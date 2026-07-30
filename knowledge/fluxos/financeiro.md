# Financeiro

## Objetivo

Documentar o fluxo financeiro: contas a pagar, contas a receber, plano de contas, lançamentos contábeis e categorias financeiras.

---

## Visão Geral

Contas a Pagar e Receber são **cadastradas manualmente** — não há geração automática por compra/venda. O impacto contábil de compras é feito via `PlanoContaLancamento` (débito na conta de estoque). O plano de contas é hierárquico com saldo atualizado em cascata.

---

## Fluxo Principal

### Contas a Pagar (Manual)

```
Usuário → ContaService.Adicionar(ContaPagar)
      ├── IDEMPRESA, IDFORNEC
      ├── IDCONTAPAI (parcelamento)
      ├── IDCATEG_FINANC (categoria)
      ├── IDCONTA (plano de contas)
      └── Valor, Vencimento, Situação
```

### Contas a Receber (Manual)

```
Usuário → ContaService.Adicionar(ContaReceber)
      ├── IDEMPRESA, IDCLIENTE
      ├── IDCONTAPAI (parcelamento)
      ├── IDCATEG_FINANC (categoria)
      └── Valor, Vencimento, Situação
```

### Lançamentos Contábeis (Compra)

```
EfetivarCompra() → PlanoContaDapperRepository.RealizarLancamento()
      ├── Tipo: Débito
      ├── Valor: VLTOTAL do item
      ├── IDCONTA: conta de estoque (CONTA_IDCONTAESTOQUE)
      └── Vinculado ao EstoqueHistorico

CancelarCompra() → PlanoContaDapperRepository.ExcluirLancamento()
      └── Reverte lançamento
```

### Consolidação

```
ContaService.ConsolidarContaPorId(id)    → fecha contabilmente
ContaService.DesconsolidarContaPorId(id) → reabre
```

---

## Regras de Negócio

- ContaPagar referencia Fornecedor (`IDFORNEC`), **não** Compra
- ContaReceber referencia Cliente (`IDCLIENTE`), **não** Venda
- Impacto contábil de compras é via `PlanoContaLancamento`, não `ContaPagar`
- Saldo do plano de contas atualizado em cascata (`AtualizarSaldoContaESubConta`)

---

## Módulos Envolvidos

- `knowledge/business/financeiro.md`
- `knowledge/business/compras.md`
- `knowledge/business/vendas.md`

---

## APIs Relacionadas

- `IContaService` — CRUD + `ConsolidarContaPorId`, `DesconsolidarContaPorId`
- `IPlanoContaDapperRepository` — `RealizarLancamento()`, `ExcluirLancamento()`, `AtualizarSaldoContaESubConta()`
- `agilum.mvc.web/Controllers/ContaController.cs`
- `agilum.mvc.web/Controllers/PlanoContaController.cs`

---

## Banco de Dados

- `conta_pagar` — IDFORNEC, IDCONTAPAI, IDCONTA
- `conta_receber` — IDCLIENTE, IDCONTAPAI, IDLANC
- `plano_conta` — Estrutura hierárquica
- `plano_conta_lancamento` — Débito/Crédito

---

## ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

## Documentação Relacionada

- `docs/fluxos/fluxo-financeiro.md` — Documentação oficial detalhada

---

## Documentação Oficial

`docs/fluxos/fluxo-financeiro.md`

---

## Fluxo Recomendado para Agentes de IA

1. Verificar `IContaService` — CRUD de contas
2. Verificar `ContaPagar` e `ContaReceber` models
3. Verificar `PlanoContaDapperRepository` — lançamentos
4. Notar que NÃO há geração automática por compra/venda
5. Consultar `docs/fluxos/fluxo-financeiro.md` para detalhes

---

## Resumo

Contas manuais (sem automação por compra/venda). Impacto contábil de compras via `PlanoContaLancamento`. Plano de contas hierárquico com saldo em cascata. Consolidação periódica.
