# Fiscal

## Objetivo

Módulo responsável pela configuração e aplicação de regras fiscais: CFOP, NCM, CEST, CST, CSOSN, IBPT, regimes tributários e emissão de documentos fiscais (NFC-e, NF-e).

---

# Visão Geral

O módulo Fiscal permeia todo o sistema. Produtos têm classificação fiscal (NCM, CEST, CST). Compras e Vendas usam CFOP e alíquotas. A venda calcula IBPT para exibição na NFC-e. O tipo de documento fiscal padrão é NFC-e (`ETipoDocVenda.NFCE`), configurável por empresa (`VENDAS_DOC_FISCAL_PADRAO`).

---

# Responsabilidades

- Cadastro de CFOP (natureza da operação)
- Classificação fiscal de produtos (NCM, CEST, CST, CSOSN)
- Cálculo de IBPT (tributos aproximados na NFC-e)
- Configuração de alíquotas (ICMS, PIS, COFINS, IPI)
- Tabelas auxiliares fiscais (`TabelaAuxiliarFiscalService`)
- Emissão de NFC-e / NF-e
- Regime de Substituição Tributária (ST)
- Importação de NF-e de compras (XML)

---

# Principais Entidades

- `VendaFiscal` — Dados fiscais da venda
- `CompraFiscal` — XML da NF-e da compra
- `TabelaAuxiliarFiscal` — Tabelas de CFOP, NCM, CEST, CST, CSOSN
- `Produto` — Contém campos fiscais (CDNCM, CDCEST, etc.)
- `CompraItem` — Contém campos fiscais individuais

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-venda.md` — Cálculo de IBPT na venda
- `docs/fluxos/fluxo-compra.md` — Importação de XML NF-e

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/` — Tabelas fiscais
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

- `knowledge/business/vendas.md` — NFC-e na venda
- `knowledge/business/compras.md` — Importação NF-e
- `knowledge/business/produtos.md` — Classificação fiscal

---

# Documentação Oficial

`docs/business/fiscal/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `TabelaAuxiliarFiscalService` para tabelas fiscais
2. Verificar `VendaFiscal` e `CompraFiscal` models
3. Verificar `ETipoDocVenda` e `ETipoEmissaoVenda` enums
4. Verificar `NFeProc` para desserialização de XML NF-e
5. Verificar config `VENDAS_DOC_FISCAL_PADRAO`

---

# Resumo

O módulo Fiscal é transversal: classificação de produtos, CFOP em compras/vendas, IBPT na NFC-e e importação de NF-e. O tipo de documento padrão é NFC-e, configurável por empresa.
