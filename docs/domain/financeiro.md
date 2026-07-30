# Módulo Financeiro

## Objetivo

O módulo **Financeiro** gerencia as contas a pagar e a receber, plano de contas, categorias financeiras, moedas e fluxo de caixa da empresa.

---

# Responsabilidades

- Cadastro de contas a pagar (ContaPagar)
- Cadastro de contas a receber (ContaReceber)
- Plano de contas (PlanoConta)
- Lançamentos contábeis (PlanoContaLancamento)
- Saldos por plano de conta (PlanoContaSaldo)
- Categorias financeiras (CategoriaFinanceira)
- Cadastro de moedas e cotações (Moeda)

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| ContaPagar | Contas a pagar |
| ContaReceber | Contas a receber |
| PlanoConta | Plano de contas contábil |
| PlanoContaLancamento | Lançamentos no plano de contas |
| PlanoContaSaldo | Saldos por conta contábil |
| CategoriaFinanceira | Categorias de receita/despesa |
| Moeda | Moedas e cotações |
| MoedaSiteMercado | Cotações de marketplace |
| FormaPagamento | Formas de pagamento |

---

# Dependências

- Empresa
- Venda (origem de contas a receber)
- Compra (origem de contas a pagar)
- Fornecedor
- Cliente

---

# Regras de Negócio

## Contas a Pagar/Receber

- Data de vencimento obrigatória
- Valor deve ser maior que zero
- Situação controla baixa

## Plano de Contas

- Estrutura hierárquica
- Contas analíticas e sintéticas
- Saldos atualizados por lançamento

---

# Situações

| Conta | Situações |
|-------|-----------|
| ContaPagar | Aberta, Paga, Vencida, Cancelada |
| ContaReceber | Aberta, Recebida, Vencida, Cancelada |

---

# Serviços Envolvidos

- ContaService
- PlanoContaService
- CategoriaFinanceiraService
- MoedaService
- FormaPagamentoService

---

# Controllers Relacionados

- ContaController
- PlanoContaController
- CategoriaFinanceiraController
- MoedaController
- FormaPagamentoController

---

# Checklist

☐ Data de vencimento definida

☐ Valor correto

☐ Categoria financeira vinculada

☐ Plano de contas configurado

☐ Baixas registradas corretamente

---

# Conclusão

O módulo **Financeiro** fornece a base para o controle contábil da empresa. A integração automática com Vendas e Compras reduz o trabalho manual e garante a consistência dos lançamentos.
