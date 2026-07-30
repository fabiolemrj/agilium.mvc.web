# Estoque

## Objetivo

Módulo responsável pelo controle de saldo, movimentações (entrada/saída), inventário e rastreabilidade de produtos nos estoques da empresa.

---

# Visão Geral

O estoque é atualizado por 5 origens: Compra (entrada), Devolução (entrada), Venda (saída), Perda (saída) e Inventário (ajuste). Toda movimentação gera um registro em `EstoqueHistorico`. O saldo é mantido em `EstoqueProduto` (produto × estoque). O custo médio é recalculado via `IProdutoDapper.AtualizarCustoMedio()`.

---

# Responsabilidades

- Controle de saldo por produto × estoque (`EstoqueProduto`)
- Registro de todas as movimentações (`EstoqueHistorico`)
- Entrada por compra efetivada
- Entrada por devolução de venda
- Saída por venda realizada
- Saída por perda registrada
- Ajuste por inventário
- Cálculo e atualização de custo médio
- Rastreabilidade completa via histórico

---

# Principais Entidades

- `Estoque` — Local de armazenamento (depósito, loja)
- `EstoqueProduto` — Saldo de um produto em um estoque
- `EstoqueHistorico` — Histórico completo de movimentações

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-estoque.md` — 5 fluxos de movimentação
- `docs/fluxos/fluxo-compra.md` — Entrada por efetivação de compra
- `docs/fluxos/fluxo-venda.md` — Saída por venda

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/EstoqueController.cs`
- `agilium-manager-azure-api/V1/EstoqueController.cs`

---

# Regras de Negócio

Consultar:

`docs/business-rules/estoque.md`

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

- `docs/padroes/dapper.md` — Dapper para consultas de estoque
- `docs/padroes/services.md` — `EstoqueService`
- `knowledge/business/compras.md` — Origem de entrada

---

# Documentação Oficial

`docs/business/estoque/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `EstoqueService` para operações de entrada/saída/ajuste
2. Verificar `IEstoqueDapperRepository` para persistência
3. Verificar `EstoqueHistorico` para rastreabilidade
4. Consultar `docs/fluxos/fluxo-estoque.md` para fluxos detalhados
5. Verificar `IProdutoDapper.AtualizarCustoMedio()` para custo médio

---

# Resumo

O estoque é atualizado por 5 fluxos distintos, com rastreabilidade completa via `EstoqueHistorico`. O saldo é sempre por produto × estoque e o custo médio é recalculado automaticamente a cada entrada.
