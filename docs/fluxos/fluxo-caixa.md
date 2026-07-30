# Fluxo de Caixa

## Objetivo

Documentar o fluxo operacional do módulo **Caixa** no Agilium Manager, desde a abertura até o fechamento, incluindo movimentações (sangrias, suprimentos) e correção de valores.

---

## Pré-condições

- Empresa selecionada na sessão
- PDV configurado
- Usuário com permissão de abertura de caixa
- Funcionário vinculado ao usuário na empresa

---

## Estados do Caixa

| Situação (`ESituacaoCaixa`) | Código | Descrição |
|---|---|---|
| `Aberto` | 1 | Aceita movimentações |
| `Fechado` | 0 | Encerrado |

> ⚠️ **Não existe** situação "Em conferência". Apenas `Aberto (1)` e `Fechado (0)`.

---

## Modelo de Dados

```
Caixa
  ├── IDEMPRESA
  ├── IDTURNO (turno vinculado, opcional)
  ├── IDPDV (ponto de venda)
  ├── IDFUNC (funcionário operador)
  ├── SQCAIXA (sequencial do caixa)
  ├── STCAIXA (ESituacaoCaixa: Aberto=1, Fechado=0)
  ├── DTHRABT (data/hora abertura)
  ├── VLABT (valor de abertura)
  ├── DTHRFECH (data/hora fechamento)
  ├── VLFECH (valor de fechamento)
  ├── CaixaMoeda[] (saldos por moeda)
  └── CaixaMovimento[] (histórico de movimentações)

CaixaMovimento
  ├── IDCAIXA
  ├── TPMOV (ETipoMovCaixa)
  ├── DSMOV (descrição)
  ├── VLMOV (valor)
  └── STMOV (ESituacaoMovCaixa)

CaixaMoeda
  ├── IDCAIXA
  ├── IDMOEDA
  ├── VLMOEDAORIGINAL (valor original)
  ├── VLMOEDACORRECAO (valor após correção)
  ├── IDUSUARIOCORRECAO
  └── DTHRCORRECAO
```

---

## Fluxo: Abertura de Caixa

```
ICaixaService.AbrirCaixa(idEmpresa, idUsuario, idPdv)
      │
      ├── Validações internas (Dapper)
      ├── Cria registro Caixa com:
      │     ├── STCAIXA = Aberto (1)
      │     ├── DTHRABT = DateTime.Now
      │     ├── IDPDV, IDFUNC, IDTURNO
      │     └── SQCAIXA (sequencial gerado)
      │
      └── Retorna: int (ID do caixa criado)
      │
      ▼
Caixa Aberto
```

---

## Fluxo: Sangria (Retirada)

```
ICaixaService.RealizarSangria(idCaixa, idUsuario, valor, msg)
      │
      ├── Valida: caixa está Aberto?
      │     └── Não → Notificar
      │
      ├── Registra movimentação de saída
      │     └── Cria CaixaMovimento (TPMOV = Saída)
      │
      └── Retorna: bool
```

---

## Fluxo: Suprimento (Entrada)

```
ICaixaService.RealizarSuprimento(idCaixa, idUsuario, valor, msg)
      │
      ├── Valida: caixa está Aberto?
      │     └── Não → Notificar
      │
      ├── Registra movimentação de entrada
      │     └── Cria CaixaMovimento (TPMOV = Entrada)
      │
      └── Retorna: bool
```

---

## Fluxo: Fechamento de Caixa

```
ICaixaService.FecharCaixa(idCaixa, idUsuario, valorFechamento, msgFechamento)
      │
      ├── Valida: caixa está Aberto?
      │     └── Não → Notificar
      │
      ├── Obter dados para fechamento:
      │     └── ObterCaixaParaFechamento(idCaixa)
      │           └── Retorna FecharCaixaInicializarViewModel
      │                 com saldos calculados
      │
      ├── Conferência:
      │     ├── Compara valorFechamento (declarado)
      │     │   com saldo calculado pelo sistema
      │     ├── Confere? → Fechamento OK
      │     └── Não confere? → Registra divergência
      │
      ├── Atualiza:
      │     ├── STCAIXA = Fechado (0)
      │     ├── DTHRFECH = DateTime.Now
      │     └── VLFECH = valorFechamento
      │
      └── Retorna: bool
```

---

## Fluxo: Correção de Valor de Moeda

```
ICaixaService.RealizarCorrecaoValor(CaixaMoeda)
      │
      ├── Atualiza VLMOEDACORRECAO e DTHRCORRECAO
      │
      └── Usado para ajustar divergências por moeda
          durante a conferência de fechamento
```

---

## Fluxo: Obter Caixa Aberto

```
ICaixaService.ObterCaixaAbertoPorEmpresa(idEmpresa, idUsuario)
      │
      └── Retorna o Caixa com STCAIXA = Aberto (1)
          para o usuário na empresa, ou null
```

---

## Entidades Envolvidas

| Entidade | Papel |
|----------|-------|
| `Caixa` | Registro principal de abertura/fechamento |
| `CaixaMovimento` | Cada movimentação (TPMOV, VLMOV, DSMOV) |
| `CaixaMoeda` | Saldo por tipo de moeda (VLMOEDAORIGINAL, VLMOEDACORRECAO) |
| `PontoVenda` (PDV) | Ponto de venda vinculado ao caixa |
| `Funcionario` | Operador do caixa |
| `Turno` | Turno vinculado (opcional) |
| `Empresa` | Contexto da operação |

---

## Regras de Negócio

- Apenas **um caixa aberto** por empresa/usuário (`ObterCaixaAbertoPorEmpresa`)
- Sangria e Suprimento só funcionam com caixa **Aberto**
- Fechamento recebe `valorFechamento` declarado e compara com o calculado
- Se houver divergência, o fechamento ainda ocorre — a diferença fica registrada
- Correção de valores por moeda via `RealizarCorrecaoValor`
- Caixa fechado **não aceita** novas movimentações
- `ObterCaixaParaFechamento` fornece os dados calculados para conferência
- A venda no PDV interage com o caixa via `_caixaDapperRepository.ObterCaixaAberto()` dentro de `RealizarVenda`

---

## Serviços Envolvidos

- `CaixaService` — `AbrirCaixa`, `FecharCaixa`, `RealizarSangria`, `RealizarSuprimento`, `RealizarCorrecaoValor`
- `ICaixaDapperRepository` — persistência das operações
- `VendaService` — `RealizarVenda` (interage com caixa)

