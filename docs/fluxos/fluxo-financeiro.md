# Fluxo Financeiro

## Objetivo

Documentar os fluxos financeiros do Agilium Manager: contas a pagar, contas a receber, plano de contas, categorias financeiras, moedas e consolidação contábil.

> ⚠️ **Importante:** Contas a Pagar e Contas a Receber são criadas **manualmente** pelo usuário. Diferente do que se poderia esperar, a efetivação de uma compra **não** gera automaticamente uma `ContaPagar`, nem a realização de uma venda gera automaticamente uma `ContaReceber`. O impacto contábil da compra é feito via `PlanoContaLancamento` (débito na conta de estoque), e não via `ContaPagar`.

---

## Fluxo: Contas a Pagar (CRUD Manual)

```
[Usuário] /conta/contapagar
      │
      ▼
ContaService.Adicionar(ContaPagar)
      │
      ├── Validar (ContaPagarValidation)
      │
      ├── ContaPagar criada com:
      │     ├── IDEMPRESA
      │     ├── IDCONTAPAI (opcional — para parcelamento)
      │     ├── IDCATEG_FINANC (categoria financeira)
      │     ├── IDUSUARIO (quem cadastrou)
      │     ├── IDFORNEC (fornecedor vinculado)
      │     ├── IDCONTA (plano de contas)
      │     ├── Descrição, Valor, DataVencimento
      │     └── Situação
      │
      ▼
ContaService.Salvar()
```

---

## Fluxo: Contas a Receber (CRUD Manual)

```
[Usuário] /conta/contareceber
      │
      ▼
ContaService.Adicionar(ContaReceber)
      │
      ├── Validar (ContaReceberValidation)
      │
      ├── ContaReceber criada com:
      │     ├── IDEMPRESA
      │     ├── IDCONTAPAI (opcional — para parcelamento)
      │     ├── IDCLIENTE
      │     ├── IDCATEG_FINANC (categoria financeira)
      │     ├── IDUSUARIO (quem cadastrou)
      │     ├── IDLANC (lançamento contábil vinculado)
      │     ├── Descrição, Valor, DataVencimento
      │     └── Situação
      │
      ▼
ContaService.Salvar()
```

---

## Fluxo: Consolidação de Conta

```
ContaService.ConsolidarContaPorId(id)
      │
      ├── Marca conta como consolidada
      │     └── Usado para fechamento contábil periódico
      │
ContaService.DesconsolidarContaPorId(id)
      │
      └── Reverte a consolidação
```

---

## Fluxo: Plano de Contas e Lançamentos

```
[Estrutura Hierárquica]

PlanoConta (Nível 1: Sintético)
  │
  ├── Conta de Estoque (configurada via CONTA_IDCONTAESTOQUE)
  ├── Contas de Receita
  ├── Contas de Despesa
  └── ...

PlanoContaLancamento
  ├── Cada movimentação gera um lançamento
  ├── Tipos: Débito / Crédito (ETipoContaLancacmento)
  ├── Vinculado a PlanoConta
  └── Pode referenciar EstoqueHistorico (quando de compra/venda)

PlanoContaSaldo
  └── Saldo acumulado por conta (atualizado via
      AtualizarSaldoContaESubConta)
```

### Origem dos Lançamentos

| Origem | Método | Tipo |
|--------|--------|------|
| Efetivação de Compra | `PlanoContaDapperRepository.RealizarLancamento()` | Débito |
| Cancelamento de Compra | `PlanoContaDapperRepository.ExcluirLancamento()` | Exclusão |
| Outras movimentações | Manual / via serviço | Débito ou Crédito |

---

## Fluxo: Categorias Financeiras

```
CategoriaFinanceira
      │
      └── Usada em ContaPagar (IDCATEG_FINANC) e
          ContaReceber (IDCATEG_FINANC) para
          classificação contábil

Exemplos:
  ├── Fornecedores
  ├── Clientes
  ├── Despesas Operacionais (aluguel, água, luz)
  └── Impostos (ICMS, PIS, COFINS, ISS)
```

---

## Fluxo: Moedas

```
[Configuração] Moeda
      │
      ├── Moeda Nacional (BRL — Real)
      ├── Moedas Estrangeiras (USD, EUR...)
      │
      ▼
Moeda é usada em:
  ├── VendaMoeda (pagamentos na venda)
  ├── CaixaMoeda (saldo do caixa por moeda)
  └── MoedaSiteMercado (cotações de marketplace)
```

---

## Entidades Envolvidas

| Entidade | Papel |
|----------|-------|
| `ContaPagar` | Obrigações a pagar (CRUD manual) |
| `ContaReceber` | Direitos a receber (CRUD manual) |
| `PlanoConta` | Estrutura contábil hierárquica |
| `PlanoContaLancamento` | Cada lançamento contábil (débito/crédito) |
| `PlanoContaSaldo` | Saldo acumulado por conta |
| `CategoriaFinanceira` | Classificação de contas |
| `Moeda` | Moedas e cotações |

---

## Regras de Negócio

- `ContaPagar` e `ContaReceber` são **cadastradas manualmente** — não há geração automática por compra/venda
- `ContaPagar` referencia `Fornecedor` (IDFORNEC), não `Compra`
- `ContaReceber` referencia `Cliente` (IDCLIENTE), não `Venda`
- O impacto contábil de compras é feito via `PlanoContaLancamento` (não via `ContaPagar`)
- `ConsolidarContaPorId` / `DesconsolidarContaPorId` para fechamento contábil
- `RealizarCorrecaoValor` em `CaixaMoeda` para ajustes de conferência
- Plano de Contas é hierárquico com atualização de saldo em cascata (`AtualizarSaldoContaESubConta`)

---

## Serviços Envolvidos

- `ContaService` — CRUD de ContaPagar e ContaReceber + consolidação
- `PlanoContaDapperRepository` — lançamentos e saldos contábeis
- `CategoriaFinanceiraService` — classificação
- `MoedaService` — moedas e cotações

