# Caixa

## Objetivo

Documentar o fluxo operacional do módulo Caixa: abertura, fechamento, sangria, suprimento e integração com Venda.

---

## Visão Geral

O Caixa controla o fluxo financeiro do PDV com apenas 2 estados: `Aberto (1)` e `Fechado (0)`. É pré-requisito para vendas. A abertura vincula PDV, funcionário e opcionalmente turno. O fechamento compara valor declarado × calculado.

---

## Fluxo Principal

```
Abertura: AbrirCaixa(idEmpresa, idUsuario, idPdv) → int (ID do caixa)
      │
      ▼
Caixa Aberto (STCAIXA = 1)
      │
      ├── Venda → CaixaMovimento (entrada)
      ├── Sangria → RealizarSangria(idCaixa, idUsuario, valor, msg)
      └── Suprimento → RealizarSuprimento(idCaixa, idUsuario, valor, msg)
      │
      ▼
Fechamento: FecharCaixa(idCaixa, idUsuario, valorFechamento, msgFechamento)
      │
      ├── ObterCaixaParaFechamento() → dados calculados
      ├── Confere valor declarado × calculado
      └── STCAIXA = Fechado (0)
```

---

## Abertura

- `ICaixaService.AbrirCaixa(idEmpresa, idUsuario, idPdv)`
- Cria `Caixa` com `STCAIXA = Aberto`, `DTHRABT`, `IDPDV`, `IDFUNC`
- Retorna ID do caixa criado

## Sangria

- `ICaixaService.RealizarSangria(idCaixa, idUsuario, valor, msg)`
- Registra `CaixaMovimento` de saída
- Só funciona com caixa Aberto

## Suprimento

- `ICaixaService.RealizarSuprimento(idCaixa, idUsuario, valor, msg)`
- Registra `CaixaMovimento` de entrada
- Só funciona com caixa Aberto

## Fechamento

- `ICaixaService.FecharCaixa(idCaixa, idUsuario, valorFechamento, msgFechamento)`
- Compara valor declarado com calculado
- Atualiza `STCAIXA = Fechado`, `DTHRFECH`, `VLFECH`

---

## Integração com Venda

- `VendaService.RealizarVenda()` verifica caixa aberto via `ICaixaDapperRepository.ObterCaixaAberto()`
- Venda registra `CaixaMovimento` automaticamente

---

## Regras de Negócio

Consultar:

`docs/business-rules/caixa.md`

---

## Módulos Envolvidos

- `knowledge/business/caixa.md`
- `knowledge/business/vendas.md`

---

## APIs Relacionadas

- `ICaixaService` — `AbrirCaixa`, `FecharCaixa`, `RealizarSangria`, `RealizarSuprimento`, `RealizarCorrecaoValor`
- `agilum.mvc.web/Controllers/CaixaController.cs`

---

## Banco de Dados

- `caixa` — IDEMPRESA, IDPDV, IDFUNC, IDTURNO, STCAIXA, SQCAIXA
- `caixa_movimento` — IDCAIXA, TPMOV, VLMOV, DSMOV
- `caixa_moeda` — IDCAIXA, IDMOEDA, VLMOEDAORIGINAL, VLMOEDACORRECAO

---

## ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

## Documentação Relacionada

- `docs/fluxos/fluxo-caixa.md` — Documentação oficial detalhada

---

## Documentação Oficial

`docs/fluxos/fluxo-caixa.md`

---

## Fluxo Recomendado para Agentes de IA

1. Verificar `ICaixaService` para todos os métodos
2. Verificar `Caixa` model com `ESituacaoCaixa` (Aberto=1, Fechado=0)
3. Verificar `VendaService.RealizarVenda()` — dependência de caixa aberto
4. Verificar `ICaixaDapperRepository` para persistência
5. Consultar `docs/fluxos/fluxo-caixa.md` para detalhes

---

## Resumo

Caixa com 2 estados. Abertura vincula PDV + funcionário. Sangria e suprimento registram movimentações. Fechamento confere saldo. Venda depende de caixa aberto.
