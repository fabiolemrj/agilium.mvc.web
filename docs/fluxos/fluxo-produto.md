# Fluxo de Produto

## Objetivo

Documentar o fluxo de cadastro e manutenção de **Produtos** no Agilium Manager, incluindo classificação fiscal, preços, códigos de barras e fotos.

---

## Fluxo de Cadastro

```
[View] /produto/novo
      │
      ▼
[GET] ProdutoController.Create()
      │
      ├── Verificar empresa selecionada
      │     └── Sem empresa? → Redirect Home
      │
      ├── Criar ProdutoViewModel inicial
      │     ├── IDEMPRESA = empresa selecionada
      │     ├── DataCadastro = DateTime.Now
      │     ├── Situacao = Ativo
      │     └── Código gerado automaticamente
      │
      ├── Popular listas auxiliares:
      │     ├── Grupos, SubGrupos, Departamentos, Marcas
      │     ├── Unidades de medida
      │     ├── Tabelas fiscais (NCM, CEST, CFOP, CST, CSOSN)
      │     └── Estoques disponíveis
      │
      ▼
Return View("CreateEdit", model)
      │
      ▼
[POST] ProdutoController.Create(ProdutoViewModel model)
      │
      ├── ModelState.IsValid?
      │     └── Não → return View(model)
      │
      ├── Mapear ViewModel → Produto (AutoMapper)
      │
      ├── ProdutoService.Adicionar(produto)
      │     │
      │     ├── ExecutarValidacao(new ProdutoValidation(), produto)
      │     │     └── Inválido → Notificar() → return
      │     │
      │     ├── Validar código único por empresa
      │     │     └── Duplicado → Notificar() → return
      │     │
      │     └── Repository.AdicionarSemSalvar(produto)
      │
      ├── Se tem códigos de barras:
      │     └── ProdutoCodigoBarraRepository.AdicionarSemSalvar(cb)
      │
      ├── Se tem preços:
      │     └── ProdutoPrecoRepository.AdicionarSemSalvar(preco)
      │
      ├── Se tem foto:
      │     └── ProdutoFotoRepository.AdicionarSemSalvar(foto)
      │
      ├── Controller.OperacaoValida()?
      │     ├── Sim → ProdutoService.Salvar() → Redirect Index
      │     └── Não → return View(model) com erros
```

---

## Estrutura do Produto

```
Produto
  │
  ├── Classificação
  │     ├── GrupoProduto
  │     ├── SubGrupoProduto
  │     ├── ProdutoDepartamento
  │     └── ProdutoMarca
  │
  ├── Informações Fiscais
  │     ├── NCM (classificação fiscal)
  │     ├── CEST (substituição tributária)
  │     ├── CFOP (natureza da operação)
  │     ├── CST / CSOSN (tributação)
  │     ├── OrigemProduto
  │     └── IBPT (alíquotas)
  │
  ├── Comercial
  │     ├── ProdutoPreco (preço de venda)
  │     ├── ProdutoCodigoBarra (múltiplos)
  │     ├── Unidade (KG, UN, L...)
  │     └── ProdutoComposicao (produtos compostos)
  │
  ├── Estoque
  │     └── EstoqueProduto (saldo por estoque)
  │
  └── Outros
        ├── ProdutoFoto (imagens)
        ├── ProdutoSiteMercado (marketplace)
        └── ClientePreco (preço diferenciado)
```

---

## Fluxo de Edição

```
[GET] /produto/editar?id=X
      │
      ▼
Obter produto por ID (incluindo códigos de barras, preços)
      │
      ├── Mapear Produto → ProdutoViewModel
      │
      ├── Popular listas auxiliares
      │
      ▼
Return View("CreateEdit", model)
      │
      ▼
[POST] ProdutoController.Edit(ProdutoViewModel model)
      │
      ├── ModelState.IsValid?
      ├── Mapear ViewModel → Produto
      │
      ├── ProdutoService.Atualizar(produto)
      │     ├── ExecutarValidacao(new ProdutoValidation(), produto)
      │     └── Repository.AtualizarSemSalvar(produto)
      │
      ├── Atualizar códigos de barras
      │     ├── Remover removidos
      │     ├── Atualizar existentes
      │     └── Adicionar novos
      │
      ├── Atualizar preços
      │
      ├── Controller.OperacaoValida()?
      │     ├── Sim → ProdutoService.Salvar() → Redirect
      │     └── Não → return View(model)
```

---

## Integração com Cardápio Digital

```
[Produto Criado/Atualizado]
      │
      ▼
IntegracaoCardapioService.SincronizarProduto(produto)
      │
      ├── Mapear para formato do cardápio
      ├── Chamar API externa (CardapioDigital.ApiBaseUrl)
      └── Registrar log de sincronização
```

---

## Regras de Negócio

- Código do produto **único por empresa**
- Nome é **obrigatório**
- Unidade de medida é **obrigatória**
- NCM é obrigatório para produtos fiscais
- Pelo menos **um código de barras**
- Preço de venda **> 0**
- Produto pode ser **composto** (ProdutoComposicao)
- Produto pode ter **múltiplos códigos de barras**
- Produto pode ter **fotos** (ProdutoFoto)
- Produto possui classificação hierárquica: Grupo → SubGrupo → Departamento → Marca

---

## Serviços Envolvidos

- `ProdutoService`
- `TabelaAuxiliarFiscalService`
- `EstoqueService`
- `UnidadeService`
- `IntegracaoCardapioService`
- `ProdutoDapper` (consultas otimizadas)
