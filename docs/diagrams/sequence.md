# Diagramas de Sequência

## Objetivo

Agrupar diagramas de sequência para os principais fluxos do Agilium Manager, complementando os diagramas específicos de venda, login e caixa.

---

## Fluxo: Criar Produto

```mermaid
sequenceDiagram
    actor User
    participant View
    participant Controller as ProdutoController
    participant Mapper as AutoMapper
    participant Service as ProdutoService
    participant Validation as ProdutoValidation
    participant Repo as ProdutoRepository
    participant DB

    User->>View: GET /produto/novo
    View->>Controller: Create()
    Controller-->>View: View("CreateEdit", viewModel)

    User->>View: Preencher formulário
    View->>Controller: POST Create(ProdutoViewModel)
    Controller->>Controller: ModelState.IsValid?
    Controller->>Mapper: Map<Produto>(viewModel)
    Controller->>Service: Adicionar(produto)

    Service->>Validation: ExecutarValidacao()
    Validation->>Validation: Validate(produto)
    
    alt Inválido
        Validation-->>Service: Erros
        Service->>Service: Notificar(validationResult)
        Service-->>Controller: return
        Controller-->>View: View(model) com erros
    end

    Service->>Repo: Buscar(c => c.CDPRODUTO == codigo)
    Repo->>DB: SELECT
    DB-->>Repo: Resultado

    alt Já existe
        Service->>Service: Notificar("Código duplicado")
        Service-->>Controller: return
        Controller-->>View: View(model) com erro
    end

    Service->>Repo: AdicionarSemSalvar(produto)
    Controller->>Service: Salvar()
    Service->>Repo: SaveChanges()
    Repo->>DB: INSERT INTO produto
    Controller-->>View: Redirect /produto
```

---

## Fluxo: Efetivar Compra

```mermaid
sequenceDiagram
    actor User
    participant Controller as CompraController
    participant CompraSvc as CompraService
    participant EstoqueSvc as EstoqueService
    participant ContaSvc as ContaService
    participant Repo as Repository
    participant DB

    User->>Controller: POST Efetivar(id)
    Controller->>CompraSvc: EfetivarCompra(id, usuario)

    CompraSvc->>Repo: ObterPorId(compra)
    Repo->>DB: SELECT
    DB-->>Repo: Compra + Itens
    Repo-->>CompraSvc: Compra

    alt Situação != Aberta
        CompraSvc->>CompraSvc: Notificar("Compra não está aberta")
        CompraSvc-->>Controller: return
    end

    loop Cada Item
        CompraSvc->>EstoqueSvc: EntradaEstoque(produto, qtd, valor)
        EstoqueSvc->>Repo: AtualizarSemSalvar(EstoqueProduto)
        EstoqueSvc->>Repo: AdicionarSemSalvar(EstoqueHistorico)
    end

    CompraSvc->>CompraSvc: Situação → Efetivada
    CompraSvc->>Repo: AtualizarSemSalvar(Compra)

    CompraSvc->>ContaSvc: GerarContaPagar(compra)
    ContaSvc->>Repo: AdicionarSemSalvar(ContaPagar)

    CompraSvc->>Repo: SaveChanges()
    Repo->>DB: COMMIT

    CompraSvc-->>Controller: OK
    Controller-->>User: Redirect IndexCompra
```

---

## Fluxo: Sincronizar Cardápio Digital

```mermaid
sequenceDiagram
    actor User
    participant Controller as ProdutoController
    participant Service as IntegracaoCardapioService
    participant Repo as Repository
    participant DB
    participant CardapioAPI as Cardápio Digital API

    User->>Controller: ExportarParaCardapio()
    
    Controller->>Service: SincronizarProdutos()

    Service->>Repo: Buscar(p => p.STEXPORTARPEDIDO == Sim)
    Repo->>DB: SELECT
    DB-->>Repo: List<Produto>
    Repo-->>Service: Produtos marcados

    loop Cada Produto
        Service->>CardapioAPI: POST /api/produtos
        Note over CardapioAPI: ConnectionString + ApiBaseUrl<br/>do appsettings.json
        
        alt Sucesso
            CardapioAPI-->>Service: 200 OK
        else Falha
            CardapioAPI-->>Service: Erro
            Service->>Service: Notificar("Falha na sincronização")
        end
    end

    Service-->>Controller: Resultado
    Controller-->>User: Mensagem de conclusão
```

---

## Para Preencher

> **TODO:** Adicionar diagramas de sequência para: inventário, devolução, fechamento de turno, conciliação financeira.
