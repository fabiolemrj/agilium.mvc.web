# Financeiro

## Objetivo

Módulo responsável pela gestão financeira: contas a pagar, contas a receber, plano de contas, categorias financeiras e moedas.

---

# Visão Geral

O módulo financeiro gerencia obrigações (ContaPagar) e direitos (ContaReceber) de forma manual. Diferente do que se poderia esperar, compras e vendas NÃO geram automaticamente contas a pagar/receber — o impacto contábil das compras é feito via `PlanoContaLancamento`. O plano de contas é hierárquico com atualização de saldo em cascata.

---

# Responsabilidades

- CRUD de Contas a Pagar (manual)
- CRUD de Contas a Receber (manual)
- Consolidação/Desconsolidação de contas
- Plano de contas hierárquico
- Lançamentos contábeis (débito/crédito)
- Atualização de saldo em cascata (`AtualizarSaldoContaESubConta`)
- Categorias financeiras
- Gestão de moedas

---

# Principais Entidades

- `ContaPagar` — Obrigações (IDFORNEC, IDCONTAPAI, IDCATEG_FINANC)
- `ContaReceber` — Direitos (IDCLIENTE, IDCONTAPAI, IDLANC)
- `PlanoConta` — Estrutura contábil hierárquica
- `PlanoContaLancamento` — Lançamentos (tipo, valor, conta)
- `PlanoContaSaldo` — Saldo por conta
- `CategoriaFinanceira` — Classificação
- `Moeda` — Moedas e cotações

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-financeiro.md` — Fluxos financeiros
- `docs/fluxos/fluxo-compra.md` — Lançamentos contábeis na efetivação

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/ContaController.cs`
- `agilum.mvc.web/Controllers/PlanoContaController.cs`
- `agilum.mvc.web/Controllers/MoedaController.cs`
- `agilium-manager-azure-api/V1/`

---

# Regras de Negócio

Consultar:

`docs/business-rules/financeiro.md`

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

- `docs/padroes/services.md` — `ContaService`
- `docs/padroes/dapper.md` — `PlanoContaDapperRepository`
- `knowledge/business/compras.md` — Origem de lançamentos contábeis

---

# Documentação Oficial

`docs/business/financeiro/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `IContaService` para interface (Adicionar, ConsolidarContaPorId, etc.)
2. Verificar `ContaPagar` e `ContaReceber` models
3. Verificar `PlanoContaDapperRepository` para lançamentos e saldos
4. Verificar `CategoriaFinanceira` para classificação
5. Notar que NÃO há geração automática de contas por compra/venda
6. Consultar `docs/fluxos/fluxo-financeiro.md`

---

# Resumo

Contas a Pagar e Receber são criadas manualmente. O impacto contábil de compras é via `PlanoContaLancamento` (não `ContaPagar`). O plano de contas é hierárquico com saldo atualizado em cascata.
