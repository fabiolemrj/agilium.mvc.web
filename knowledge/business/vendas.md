# Vendas

## Objetivo

Módulo responsável pela realização de vendas no PDV, incluindo itens, formas de pagamento, dados fiscais (NFC-e), venda temporária (pré-venda) e cancelamento.

---

# Visão Geral

O módulo de Vendas é o coração do PDV. A venda é criada via `VendaService.RealizarVenda()` em uma transação atômica. Interage com Caixa (deve estar aberto), Estoque (baixa de itens), Vales (consumo) e fiscal (IBPT). Suporta pré-venda (VendaTemporaria) e cancelamento (VendaCancelada).

---

# Responsabilidades

- Realização de venda (atômica, transacional)
- Criação de venda temporária (pré-venda)
- Conversão de pré-venda em venda definitiva
- Cálculo de IBPT (tributos aproximados)
- Lançamento de moedas no cupom
- Cancelamento de venda com reversão de estoque
- Emissão fiscal (NFC-e)
- Geração de espelho de segurança (VendaEspelho)

---

# Principais Entidades

- `Venda` — Registro principal (STVENDA, VLVENDA, VLTOTAL)
- `VendaItem` — Itens vendidos
- `VendaMoeda` — Formas de pagamento
- `VendaFiscal` — Dados fiscais
- `VendaCancelada` — Registro de cancelamento
- `VendaEspelho` — Cópia de segurança
- `VendaTemporaria` — Pré-venda (VendaTemporariaItem, VendaTemporariaMoeda)

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-venda.md` — Fluxo completo
- `docs/fluxos/fluxo-caixa.md` — Interação com caixa
- `docs/fluxos/fluxo-estoque.md` — Baixa de estoque

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/VendaController.cs`
- `agilium-manager-azure-api/V1/VendaController.cs`
- `agilium-pdv-azure-api/` — PDV

---

# Regras de Negócio

Consultar:

`docs/business-rules/vendas.md`

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

- `docs/padroes/services.md` — `VendaService`
- `knowledge/business/caixa.md` — Pré-requisito de caixa aberto
- `knowledge/business/fiscal.md` — Emissão NFC-e

---

# Documentação Oficial

`docs/business/vendas/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `VendaService.RealizarVenda()` para lógica completa
2. Verificar `IVendaDapperRepository` para persistência
3. Verificar `Venda` model com `ESituacaoVenda` (Ativo=1, Inativo=0) e `ETipoEmissaoVenda`
4. Verificar `ICaixaDapperRepository.ObterCaixaAberto()` como pré-condição
5. Verificar `IValeDapperRepository.UtilizarValePorVenda()`
6. Consultar `docs/fluxos/fluxo-venda.md` para fluxo detalhado

---

# Resumo

Vendas são o principal fluxo de receita. `STVENDA` é `Ativo (1)` ou `Inativo (0)`. A realização é atômica e depende de caixa aberto. Pré-venda é suportada via `VendaTemporaria`.
