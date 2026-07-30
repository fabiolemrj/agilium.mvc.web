# Fluxo de Compra

## Objetivo

Documentar o fluxo completo de uma **Compra** no Agilium Manager, desde a criação até a efetivação com entrada no estoque e impacto contábil, incluindo importação de XML NF-e, cadastro automático de produtos e cancelamento.

---

## Pré-condições

- Empresa selecionada
- Fornecedor cadastrado e ativo
- Usuário com permissão de compra
- Turno aberto (opcional, vinculado via `IDTURNO`)
- Configuração `CONTA_REALIZARCONTROLE` e `CONTA_IDCONTAESTOQUE` definidas para lançamento contábil

---

## Estados da Compra

| Situação (`ESituacaoCompra`) | Código | Descrição |
|---|---|---|
| `Aberta` | 1 | Compra criada, itens podem ser editados |
| `Efetivada` | 2 | Compra finalizada — estoque atualizado, lançamentos contábeis gerados |
| `Cancelada` | 3 | Compra cancelada — se estava Efetivada, estoque e contas são revertidos |

> ⚠️ **Não existe** situação "Aprovada" nem "Parcialmente Efetivada". A efetivação é **atômica** (todos os itens de uma vez).

---

## Fluxo Principal: Criação e Efetivação

```
[Início] /compra/novo
      │
      ▼
CompraService.Adicionar(compra)
      │
      ├── Validar (CompraValidation)
      │
      ├── Gerar código da compra (CDCOMPRA) via _compraService.GerarCodigoCompra()
      │
      ├── Compra criada com:
      │     ├── IDEMPRESA (sessão)
      │     ├── IDFORN (fornecedor)
      │     ├── IDTURNO (turno aberto, opcional)
      │     ├── DTCOMPRA (data da compra)
      │     ├── DTCAD (data de cadastro)
      │     ├── CDCOMPRA (código único)
      │     ├── STCOMPRA = Aberta (1)
      │     └── DSOBS (observações)
      │
      ▼
Situação: Aberta
      │
      ▼
┌──────────────────────────────────────────────────┐
│          ADICIONAR ITENS (CompraItem)              │
│                                                    │
│  Cada item contém:                                 │
│    ├── IDPRODUTO (pode ser null — item pendente)   │
│    ├── IDESTOQUE (estoque de destino)              │
│    ├── DSPRODUTO (descrição do produto)            │
│    ├── CDEAN (código de barras)                    │
│    ├── CDNCM / CDCEST (classificação fiscal)       │
│    ├── SGUN (unidade de medida)                    │
│    ├── NUQTD (quantidade)                          │
│    ├── NURELACAO (relação unid. compra × venda)    │
│    ├── VLUNIT (valor unitário)                     │
│    ├── VLTOTAL (valor total)                       │
│    ├── VLNOVOPRECOVENDA (novo preço de venda)      │
│    ├── NUCFOP (CFOP)                               │
│    ├── Dados fiscais: CST, alíquotas e bases de    │
│    │   cálculo de ICMS, PIS, COFINS, IPI           │
│    ├── VLIPI, VLPIS, VLCOFINS, VLICMS              │
│    ├── VLBSRET / PCICMSRET (base e % ICMS retido)  │
│    ├── VLOUTROS (outras despesas)                  │
│    ├── CDPRODFORN (código do produto no fornecedor) │
│    └── DTVALIDADE (validade, opcional)             │
│                                                    │
│  Atualizar totais da Compra:                       │
│    ├── VLTOTPROD (soma dos totais dos itens)       │
│    ├── VLFRETE, VLSEGURO, VLDESCONTO, VLOUTROS     │
│    ├── VLIPI (IPI total)                           │
│    └── VLTOTAL (total geral)                       │
│                                                    │
└──────────────────────────────────────────────────┘
      │
      ▼
┌──────────────────────────────────────────────────┐
│        IMPORTAÇÃO DE XML NF-e (opcional)           │
│                                                    │
│  /compra/importar ou /compra/ImportarXML           │
│                                                    │
│  CompraService.ImportarCompraDeXmlNfe(nfe, id)     │
│      │                                             │
│      ├── Desserializa XML da NF-e (NFeProc)        │
│      ├── Preenche dados fiscais da Compra:         │
│      │     ├── NUNF, DSSERIENF, DSCHAVENFE         │
│      │     ├── DTNF (data da nota)                 │
│      │     ├── TPCOMPROVANTE (tipo do comprovante) │
│      │     ├── NUCFOP                              │
│      │     └── Valores de ICMS, IPI, frete, etc.   │
│      │                                             │
│      ├── Cria CompraItem para cada produto da NF   │
│      │     └── Se IDPRODUTO não corresponde a      │
│      │         nenhum produto cadastrado → fica     │
│      │         como item "pendente"                 │
│      │                                             │
│      ├── Cria CompraFiscal (armazena XML):         │
│      │     ├── STMANIFESTO (manifesto)             │
│      │     └── DSXML (conteúdo do XML)             │
│      │                                             │
│      └── SalvarArquivoXmlNFE(): salva XML em disco │
│                                                    │
│  Tipos de Comprovante (ETipoCompravanteCompra):     │
│    NFE=1, NFCE=2, NFSE=3, CupomFiscal=4,           │
│    Recibo=5, NotaFiscalPapel=6, Outros=7            │
│                                                    │
└──────────────────────────────────────────────────┘
      │
      ▼
┌──────────────────────────────────────────────────┐
│      CADASTRO AUTOMÁTICO DE PRODUTO (opcional)     │
│                                                    │
│  /compra/cadastro-produto-automatico               │
│                                                    │
│  CompraService.RealizarCadastroProdutoAutomatico()  │
│      │                                             │
│      ├── Só funciona se STCOMPRA == Aberta         │
│      ├── Só processa itens SEM IDPRODUTO           │
│      │                                             │
│      ├── Para cada item pendente:                  │
│      │     ├── Cria Produto com status "Pendente"  │
│      │     ├── Usa DSPRODUTO (truncado em 50 char) │
│      │     ├── Herda SGUN, CDNCM, CDCEST,          │
│      │     │   NURELACAO, VLNOVOPRECOVENDA         │
│      │     └── Vincula IDPRODUTO ao CompraItem     │
│      │                                             │
│      └── Produto fica marcado para revisão manual  │
│                                                    │
└──────────────────────────────────────────────────┘
      │
      ▼
┌──────────────────────────────────────────────────┐
│               EFETIVAR COMPRA                      │
│                                                    │
│  /compra/efetivar                                  │
│                                                    │
│  CompraService.EfetivarCompra(idCompra, usuario)    │
│      │                                             │
│      ├── Validações:                               │
│      │     ├── Compra existe?                      │
│      │     ├── Itens associados a produtos?        │
│      │     │     └── Se não: "Não foram            │
│      │     │       encontrados itens associados"    │
│      │     └── Conta de estoque configurada?       │
│      │           (se CONTA_REALIZARCONTROLE=1)     │
│      │                                             │
│      ├── BEGIN TRANSACTION                         │
│      │                                             │
│      ├── Para cada CompraItem:                     │
│      │     │                                       │
│      │     ├── 1. Calcular quantidade de entrada:  │
│      │     │     qtd = NUQTD × NURELACAO           │
│      │     │     (converte unid. compra → venda)   │
│      │     │                                       │
│      │     ├── 2. Calcular valor unitário venda:   │
│      │     │     vlUnitVenda = VLUNIT ÷ NURELACAO  │
│      │     │                                       │
│      │     ├── 3. Atualizar custo médio do produto │
│      │     │     _produtoDapperRepository          │
│      │     │       .AtualizarCustoMedio()          │
│      │     │                                       │
│      │     ├── 4. Atualizar último valor de compra │
│      │     │     _produtoDapperRepository          │
│      │     │       .AtualizarUltimoValorCompra()   │
│      │     │                                       │
│      │     ├── 5. Se VLNOVOPRECOVENDA > 0:        │
│      │     │     Atualizar preço de venda          │
│      │     │     _produtoDapperRepository          │
│      │     │       .AtualizarPrecoVenda()          │
│      │     │                                       │
│      │     ├── 6. Entrada no estoque:              │
│      │     │     _estoqueDapperRepository          │
│      │     │       .RealizaEntradaRetornaId-       │
│      │     │        HistoricoGerado()              │
│      │     │     └── Gera EstoqueHistorico com     │
│      │     │         referência ao CompraItem      │
│      │     │                                       │
│      │     ├── 7. Cadastrar EAN (se não existir):  │
│      │     │     _produtoDapperRepository          │
│      │     │       .InsereProdutoCodigoBarra()     │
│      │     │                                       │
│      │     └── 8. Lançamento contábil:             │
│      │           _planoContaDapperRepository       │
│      │             .RealizarLancamento()           │
│      │           ├── Tipo: Débito                  │
│      │           ├── Valor: VLTOTAL do item        │
│      │           └── Vincula ao EstoqueHistorico   │
│      │                                             │
│      ├── Atualizar saldo da conta contábil         │
│      │     _planoContaDapperRepository             │
│      │       .AtualizarSaldoContaESubConta()       │
│      │                                             │
│      ├── Atualizar STCOMPRA → Efetivada (2)        │
│      │                                             │
│      └── COMMIT (ou ROLLBACK se houve erro)        │
│                                                    │
└──────────────────────────────────────────────────┘
      │
      ▼
Compra Efetivada
```

---

## Fluxo: Cancelamento de Compra

```
/compra/cancelar

CompraService.CancelarCompra(idCompra, usuarioNome)
      │
      ├── BEGIN TRANSACTION
      │
      ├── Se STCOMPRA == Aberta:
      │     └── Apenas muda status → Cancelada (3)
      │
      ├── Se STCOMPRA == Efetivada:
      │     │
      │     ├── Obtém itens da compra com seus
      │     │   EstoquesHistoricos (ObterItemCompraEfetivada)
      │     │
      │     └── Para cada item:
      │           │
      │           ├── Desvincula lançamento contábil do
      │           │   histórico de estoque
      │           │   _estoqueDapperRepository
      │           │     .DesvincularHistoricoDoLancamento()
      │           │
      │           ├── Exclui lançamento contábil
      │           │   _planoContaDapperRepository
      │           │     .ExcluirLancamento()
      │           │
      │           └── Retirada do estoque
      │               _estoqueDapperRepository
      │                 .RealizaRetiradaRetornaIdHistoricoGerado()
      │               └── Gera EstoqueHistorico negativo
      │                   com referência ao cancelamento
      │
      ├── Atualizar saldo da conta contábil
      │     _planoContaDapperRepository
      │       .AtualizarSaldoContaESubConta()
      │
      ├── Atualizar STCOMPRA → Cancelada (3)
      │
      └── COMMIT (ou ROLLBACK se houve erro)
      │
      ▼
Compra Cancelada
```

---

## Fluxo: Importação de Arquivo XML

```
/compra/ImportarXML (POST, multipart)

CompraService.ImportarArquivoXmlNFE(idCompra, xmlString)
      │
      ├── Desserializa arquivo .xml para NFeProc
      │
      ├── Chama ImportarCompraDeXmlNfe(nfe, idCompra)
      │     (ver fluxo na seção principal)
      │
      └── Se sucesso → redireciona para edição da compra

Variação: ImportarArquivoXmlNFESemGravar()
      └── Apenas lê e retorna NFeProc, sem persistir
          (útil para preview antes de importar)
```

---

## Entidades Envolvidas

| Entidade | Papel |
|----------|-------|
| `Compra` | Registro principal: fornecedor, totais, dados da NF |
| `CompraItem` | Itens da compra com dados fiscais individuais |
| `CompraFiscal` | XML da NF-e e status de manifesto (`STMANIFESTO`) |
| `Fornecedor` | Fornecedor da mercadoria |
| `Produto` | Produto associado (ou null se pendente) |
| `Estoque` | Local de armazenamento de destino |
| `EstoqueHistorico` | Registro da entrada/saída no estoque |
| `PlanoContaLancamento` | Lançamento contábil (débito na conta de estoque) |
| `Empresa` | Contexto da operação |
| `Turno` | Turno opcional vinculado à compra |

---

## Regras de Negócio

- A compra inicia sempre com `STCOMPRA = Aberta (1)`
- Itens podem ser adicionados/editados/removidos enquanto Aberta
- **Efetivação é atômica**: processa todos os itens em uma transação; ou todos entram no estoque, ou nenhum
- A quantidade de entrada no estoque é `NUQTD × NURELACAO` (conversão unidade compra → unidade venda)
- O custo médio é recalculado com base no **valor unitário de venda** (`VLUNIT ÷ NURELACAO`)
- Se `VLNOVOPRECOVENDA > 0`, o preço de venda do produto é atualizado automaticamente
- Se o EAN (código de barras) do item não existir, é cadastrado automaticamente no produto
- O lançamento contábil é do tipo **Débito** na conta de estoque configurada
- Cancelamento de compra **Efetivada** reverte: lançamentos contábeis + entrada no estoque
- Cancelamento de compra **Aberta** apenas muda o status
- Cadastro automático de produto só funciona com compra Aberta e itens sem `IDPRODUTO`
- A importação de XML NF-e preenche automaticamente dados fiscais e cria `CompraFiscal`

---

## Serviços Envolvidos

- `CompraService` — CRUD, efetivação, cancelamento, importação XML
- `ICompraRepository` / `ICompraDapperRepository` — persistência
- `IProdutoDapper` — atualização de custo médio, preço, código de barras
- `IEstoqueDapperRepository` — entrada e retirada de estoque
- `IPlanoContaDapperRepository` — lançamentos e saldos contábeis
- `IUtilDapperRepository` — configurações (`CONTA_REALIZARCONTROLE`, etc.)

---

## Endpoints (MVC)

| Rota | Método | Ação |
|------|--------|------|
| `/compra/lista` | GET | Listagem paginada de compras |
| `/compra/novo` | GET/POST | Criar nova compra |
| `/compra/editar` | GET/POST | Editar compra existente |
| `/compra/cancelar` | GET/POST | Cancelar compra |
| `/compra/importar` | GET/POST | Importar dados de NF-e (objeto) |
| `/compra/ImportarXML` | POST | Upload de arquivo XML NF-e |
| `/compra/efetivar` | GET/POST | Efetivar compra |
| `/compra/cadastro-produto-automatico` | GET/POST | Criar produtos a partir de itens pendentes |

- `EstoqueService`
- `FinanceiroService` / `ContaPagarService`
- `ProdutoService` (recalcular custo médio)
- `SugestaoCompraService`
