# Relatórios

## Objetivo

Módulo responsável pelos relatórios gerenciais do Agilium Manager: vendas, financeiro, estoque, ranking e análises.

---

# Visão Geral

Os relatórios do Agilium Manager são gerados via Dapper para consultas de alta performance com agregações e múltiplos joins. Estão concentrados nos métodos `ObterRelatorio*` das interfaces de serviço e em ViewModels específicos de relatório (`VendaReportViewModel`, etc.).

---

# Responsabilidades

- Relatório de venda detalhada (`VendasReportViewModel`)
- Relatório de venda por fornecedor (`VendasFornecedorViewModel`)
- Relatório de venda por moeda (`VendaMoedaReport`)
- Ranking de vendas (`VendaRankingReport`)
- Diferença de caixa (`VendaDiferencaCaixaReport`)
- Relatórios financeiros (contas a pagar/receber)
- Relatórios de estoque e inventário

---

# Principais Entidades

- `VendasReportViewModel` — Venda detalhada
- `VendasFornecedorViewModel` — Venda por fornecedor
- `VendaMoedaReport` — Venda por moeda
- `VendaRankingReport` — Ranking de produtos/clientes
- `VendaDiferencaCaixaReport` — Diferenças de caixa

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-venda.md` — Dados de origem
- `docs/fluxos/fluxo-financeiro.md` — Relatórios financeiros

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/` — Controllers com actions de relatório
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

- `docs/padroes/dapper.md` — Dapper para consultas de relatório
- `knowledge/business/vendas.md` — Dados de origem dos relatórios

---

# Documentação Oficial

`docs/business/relatorios/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `IVendaService` — métodos `ObterRelatorio*`
2. Verificar `VendaReportViewModel` models em `agilium-manager-azure-business/Models/CustomReturn/ReportViewModel/`
3. Verificar consultas Dapper associadas nos repositórios
4. Verificar `EResultadoFiltroRanking` e `EOrdenacaoFiltroRanking` enums

---

# Resumo

Relatórios são gerados via Dapper com ViewModels específicos. Os principais são: venda detalhada, ranking, por fornecedor, por moeda e diferença de caixa.
