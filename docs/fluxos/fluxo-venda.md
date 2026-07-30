# Fluxo de Venda

## Objetivo

Documentar o fluxo completo de uma **Venda** no Agilium Manager, desde a abertura até a realização, incluindo itens, formas de pagamento, dados fiscais, impacto no estoque, venda temporária (pré-venda) e cancelamento.

---

## Pré-condições

- Empresa selecionada
- **Caixa aberto** (`ObterCaixaAbertoPorEmpresa`)
- Funcionário vinculado ao usuário na empresa
- Estoque configurado para o caixa (`ObterEstoquePorIdCaixa`)
- Usuário com permissão de venda

---

## Estados da Venda

| Situação (`ESituacaoVenda`) | Código | Descrição |
|---|---|---|
| `Ativo` | 1 | Venda realizada e vigente |
| `Inativo` | 0 | Venda inativa/cancelada |

> ⚠️ **Não existe** situação "Aberta" ou "Finalizada". A venda é criada diretamente como `Ativo` ou `Inativo`.

---

## Estados de Emissão Fiscal

| Situação (`ETipoEmissaoVenda`) | Código | Descrição |
|---|---|---|
| `NaoEmitido` | 0 | Documento fiscal não emitido |
| `Emitido` | 1 | NFC-e / NF-e emitida |
| `Contigencia` | 2 | Emitido em contingência |
| `Cancelada` | 3 | Documento fiscal cancelado |

---

## Fluxo Principal: RealizarVenda

```
[Início] PDV → Nova Venda
      │
      ▼
VendaService.RealizarVenda(venda, idUsuario, idEmpresa)
      │
      ├── Validar (VendaValidation)
      │
      ├── Validar: VendaItem.Any()?
      │     └── Não → Notificar("Não existem itens da venda.")
      │
      ├── Validar: VendaMoeda.Any()?
      │     └── Não → Notificar("Não existem formas de pagamento")
      │
      ├── Validar: VLVENDA > 0 && VLTOTAL > 0
      │
      ├── Validar cada item:
      │     ├── VLTOTAL > 0 && VLITEM > 0
      │     └── NUQTD > 0
      │
      ├── Validar cada moeda:
      │     └── VLPAGO > 0
      │
      ├── Obter usuário e funcionário vinculado
      │     └── Se não encontrado → Notificar()
      │
      ├── Obter caixa aberto:
      │     │
      │     ├── _caixaDapperRepository.ObterCaixaAberto(
      │     │       idEmpresa, idFuncionario)
      │     │
      │     └── Se não encontrado → Notificar(
      │           "Não existe caixa aberto para o usuario.")
      │
      ├── Obter estoque do caixa:
      │     └── _caixaDapperRepository
      │           .ObterEstoquePorIdCaixa(caixaAberto.Id)
      │
      ├── BEGIN TRANSACTION
      │
      ├── Gerar código sequencial da venda (SQVENDA)
      │
      ├── Obter CPF/CNPJ do cliente (se informado)
      │
      ├── Obter config VENDAS_DOC_FISCAL_PADRAO
      │     └── Define tipo de documento (NFCE por padrão)
      │
      ├── venda.AdicionarOrigemVenda(EOrigemVenda.DIRETA)
      ├── venda.MudarSituacaoAtivo()  → STVENDA = Ativo (1)
      │
      ├── Calcular IBPT (impostos aproximados):
      │     └── venda.AdicionarIbpt(...)
      │
      ├── Adicionar informações complementares
      │     (tributos federais, estaduais, municipais)
      │
      ├── Verificar config PDV_PREVENDA e PREVENDA_ATIVO
      │
      ├── Criar VendaTemporaria:
      │     └── _vendaDapperRepository
      │           .AdicionarVendaTemporaria(venda, idEstoque,
      │             seqCaixa, nomeUsuario, cpf)
      │
      ├── Se pré-venda NÃO ativa e NÃO MEI sem cupom:
      │     │
      │     ├── Criar Venda definitiva:
      │     │     _vendaDapperRepository.AdicionarVenda(...)
      │     │
      │     ├── Lançar moedas no cupom (LancarMoedasCupom)
      │     │
      │     └── Apagar venda temporária
      │
      ├── Utilizar vales vinculados (se houver):
      │     └── _valeDapperRepository.UtilizarValePorVenda(idVenda)
      │
      ├── COMMIT (ou ROLLBACK se houve erro)
      │
      ▼
Venda Realizada (STVENDA = Ativo)
```

---

## Fluxo: Venda Temporária (Pré-Venda)

```
Configurações relevantes:
  ├── PDV_PREVENDA: "S" para MEI sem cupom fiscal
  └── PREVENDA_ATIVO: "S" para pré-venda ativa

[Nova Venda — Pré-venda ativa]
      │
      ▼
VendaTemporaria é criada e mantida
  ├── VendaTemporariaItem
  ├── VendaTemporariaMoeda
  └── VendaTemporariaEspelho
      │
      ▼
[Posteriormente]
      │
      ├── Converter para Venda definitiva
      │     └── _vendaDapperRepository.AdicionarVenda()
      │
      └── Ou excluir (abandono)
```

---

## Fluxo: Cancelamento de Venda

```
[Venda Ativa] Solicitar Cancelamento
      │
      ▼
O cancelamento de venda é tratado via Dapper:
      │
      ├── Cria registro em VendaCancelada
      │     ├── IDVENDA
      │     ├── Motivo
      │     └── UsuarioCancelamento
      │
      ├── Reverte itens ao estoque
      │     └── _estoqueDapperRepository (entrada)
      │
      ├── Atualiza ETipoEmissaoVenda → Cancelada (3)
      │
      └── Atualiza STVENDA → Inativo (0) ou
          mantém Ativo com emissão Cancelada
```

---

## Entidades Envolvidas

| Entidade | Papel |
|----------|-------|
| `Venda` | Registro principal: STVENDA, VLVENDA, VLTOTAL, STVENDA |
| `VendaItem` | Itens vendidos (IDPRODUTO, NUQTD, VLITEM, VLTOTAL) |
| `VendaMoeda` | Formas de pagamento (IDMOEDA, VLPAGO, TROCO) |
| `VendaFiscal` | Dados fiscais da venda |
| `VendaCancelada` | Registro de cancelamento |
| `VendaEspelho` | Cópia de segurança |
| `VendaTemporaria` | Pré-venda |
| `VendaTemporariaItem` | Itens da pré-venda |
| `VendaTemporariaMoeda` | Moedas da pré-venda |
| `VendaTemporariaEspelho` | Espelho da pré-venda |
| `PedidoVenda` | Pedido convertido em venda |
| `Caixa` | Caixa deve estar aberto (`ESituacaoCaixa.Aberto`) |
| `Cliente` | Cliente opcional (IDCLIENTE) |

---

## Regras de Negócio

- **Caixa deve estar aberto** (`ESituacaoCaixa.Aberto = 1`)
- `STVENDA` é `Ativo (1)` para venda vigente, `Inativo (0)` caso contrário
- A venda é **atômica** — criada em uma única transação
- A venda temporária é criada **antes** da definitiva e apagada após
- O tipo de documento fiscal padrão é **NFC-e** (`ETipoDocVenda.NFCE`)
- `EOrigemVenda.DIRETA` para vendas no PDV; `EOrigemVenda.PEDIDO` para pedidos convertidos
- IBPT é calculado para exibição de tributos aproximados na NFC-e
- Vales são consumidos após a venda (`UtilizarValePorVenda`)
- Histórico é preservado no `VendaEspelho`

---

## Serviços Envolvidos

- `VendaService` — `RealizarVenda()`
- `IVendaDapperRepository` — `AdicionarVenda()`, `AdicionarVendaTemporaria()`, `ApagarVendaTemporaria()`
- `ICaixaDapperRepository` — `ObterCaixaAberto()`, `ObterEstoquePorIdCaixa()`
- `IPedidoDapperRepository` — `GerarCodigoVenda()`, `ObterUsuarioPorId()`, `ObterCpfCnpjPorCliente()`
- `IValeDapperRepository` — `UtilizarValePorVenda()`
- `IConfigDapperRepository` — `ObterConfig()`
- `ProdutoService`, `EstoqueService`
