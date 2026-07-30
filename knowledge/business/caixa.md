# Caixa

## Objetivo

Módulo responsável pela gestão de caixa no PDV, incluindo abertura, fechamento, sangria, suprimento, movimentações e correção de valores por moeda.

---

# Visão Geral

O módulo de Caixa controla o fluxo de dinheiro no PDV. Cada caixa é vinculado a um PDV (`IDPDV`), um funcionário (`IDFUNC`) e opcionalmente a um turno (`IDTURNO`). Os estados são apenas `Aberto (1)` e `Fechado (0)`. As operações principais são: AbrirCaixa, FecharCaixa, RealizarSangria e RealizarSuprimento.

---

# Responsabilidades

- Abertura de caixa (`AbrirCaixa`) — retorna ID do caixa
- Fechamento de caixa (`FecharCaixa`) — com valor declarado e conferência
- Sangria (`RealizarSangria`) — retirada de valor
- Suprimento (`RealizarSuprimento`) — entrada de valor
- Correção de valor por moeda (`RealizarCorrecaoValor`)
- Consulta de caixa aberto (`ObterCaixaAbertoPorEmpresa`)
- Dados para fechamento (`ObterCaixaParaFechamento`)

---

# Principais Entidades

- `Caixa` — Registro principal (STCAIXA, DTHRABT, VLABT, DTHRFECH, VLFECH)
- `CaixaMovimento` — Histórico de movimentações (TPMOV, VLMOV, DSMOV)
- `CaixaMoeda` — Saldo por moeda (VLMOEDAORIGINAL, VLMOEDACORRECAO)

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-caixa.md` — Fluxo completo
- `docs/fluxos/fluxo-venda.md` — Caixa como pré-condição da venda

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/CaixaController.cs`
- `agilium-manager-azure-api/V1/CaixaController.cs`
- `agilium-pdv-azure-api/` — PDV

---

# Regras de Negócio

Consultar:

`docs/business-rules/caixa.md`

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

- `docs/padroes/services.md` — `CaixaService`
- `knowledge/business/vendas.md` — Venda depende de caixa aberto

---

# Documentação Oficial

`docs/business/caixa/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `ICaixaService` para interface completa
2. Verificar `CaixaService.AbrirCaixa()` — parâmetros e retorno
3. Verificar `CaixaService.FecharCaixa()` — conferência de saldo
4. Verificar `Caixa` model com `ESituacaoCaixa` (Aberto=1, Fechado=0)
5. Verificar `ICaixaDapperRepository` para persistência
6. Consultar `docs/fluxos/fluxo-caixa.md` para fluxo detalhado

---

# Resumo

Caixa controla o fluxo financeiro do PDV com apenas 2 estados (Aberto/Fechado). Abertura e fechamento são operações Dapper. O caixa deve estar aberto para vendas, sangrias e suprimentos. O fechamento compara valor declarado × calculado.
