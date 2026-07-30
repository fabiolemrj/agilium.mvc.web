# Diagrama: Fluxo de Venda

## Objetivo

Representar o fluxo completo de uma venda no PDV, desde a abertura até a finalização, incluindo validações, atualização de estoque e integração com caixa.

---

## Fluxo Completo

```mermaid
sequenceDiagram
    actor Operador
    participant PDV as View PDV
    participant Controller as VendaController
    participant VendaSvc as VendaService
    participant ProdutoSvc as ProdutoService
    participant EstoqueSvc as EstoqueService
    participant CaixaSvc as CaixaService
    participant Repo as Repository
    participant DB as MySQL

    Note over Operador,DB: Pré-condições: Turno aberto, Caixa aberto

    Operador->>PDV: Nova Venda
    PDV->>Controller: Create()
    Controller-->>PDV: View Venda (Situação: Aberta)

    loop Adicionar Itens
        Operador->>PDV: Buscar Produto (código)
        PDV->>Controller: BuscarProduto(codigo)
        Controller->>ProdutoSvc: ObterPorCodigo(codigo)
        ProdutoSvc->>Repo: Buscar()
        Repo->>DB: SELECT
        DB-->>Repo: Produto
        Repo-->>ProdutoSvc: Produto
        ProdutoSvc-->>Controller: Produto
        
        alt Produto ativo e com estoque
            Controller-->>PDV: Dados do produto
            Operador->>PDV: Confirmar quantidade
            PDV->>Controller: AdicionarItem(produto, qtd)
            Controller->>VendaSvc: AdicionarItem()
            VendaSvc->>Repo: AdicionarSemSalvar(VendaItem)
            Controller-->>PDV: Item adicionado
        else Produto inativo ou sem estoque
            Controller-->>PDV: Notificar("Produto indisponível")
        end
    end

    Operador->>PDV: Finalizar Venda
    PDV->>Controller: Finalizar(formaPagamento, valorPago)
    
    Controller->>VendaSvc: FinalizarVenda()
    VendaSvc->>VendaSvc: Validar pagamento >= total
    VendaSvc->>VendaSvc: Gerar VendaFiscal
    VendaSvc->>VendaSvc: Situação → Finalizada

    loop Cada Item
        VendaSvc->>EstoqueSvc: SaidaEstoque(produto, qtd)
        EstoqueSvc->>Repo: AtualizarSemSalvar(EstoqueProduto)
        EstoqueSvc->>Repo: AdicionarSemSalvar(EstoqueHistorico)
    end

    VendaSvc->>VendaSvc: Gerar VendaEspelho (backup)
    VendaSvc->>CaixaSvc: RegistrarMovimentoVenda(valor)
    CaixaSvc->>Repo: AdicionarSemSalvar(CaixaMovimento)

    VendaSvc->>Repo: SaveChanges()
    Repo->>DB: COMMIT

    Controller-->>PDV: Venda Finalizada
    PDV-->>Operador: Comprovante
```

---

## Cancelamento de Venda

```mermaid
sequenceDiagram
    actor Operador
    participant Controller
    participant VendaSvc
    participant EstoqueSvc
    participant CaixaSvc
    participant Repo
    participant DB

    Operador->>Controller: CancelarVenda(id, motivo)
    Controller->>VendaSvc: CancelarVenda()

    VendaSvc->>VendaSvc: Validar situação (Finalizada?)
    VendaSvc->>Repo: AdicionarSemSalvar(VendaCancelada)

    loop Cada Item
        VendaSvc->>EstoqueSvc: EntradaEstoque(produto, qtd)
        EstoqueSvc->>Repo: AtualizarSemSalvar(EstoqueProduto)
    end

    VendaSvc->>CaixaSvc: RegistrarMovimentoCancelamento(valor)
    VendaSvc->>VendaSvc: Situação → Cancelada
    VendaSvc->>Repo: SaveChanges()
    Repo->>DB: COMMIT

    Controller-->>Operador: Venda cancelada
```

---

## Para Preencher

> **TODO:** Adicionar fluxo de venda temporária (pré-venda) e conversão de pedido em venda.
