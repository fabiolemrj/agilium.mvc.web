# Produto

## Objetivo

Documentar o fluxo de ciclo de vida do produto: cadastro, classificação fiscal, preços, códigos de barras, composições (kits) e integração com cardápio digital.

---

## Visão Geral

Produtos formam o catálogo central do ERP. Cada produto possui classificação hierárquica (Grupo → SubGrupo → Marca + Departamento), informações fiscais (NCM, CEST, CST), múltiplos códigos de barras e preço de venda. O custo médio é atualizado automaticamente via efetivação de compra.

---

## Fluxo Principal

### Cadastro

```
GET: /produto/novo
      │
      ▼
ProdutoController.Create()
      ├── IDEMPRESA (sessão)
      ├── CDPRODUTO gerado automaticamente
      ├── Situação = Ativo
      └── Listas auxiliares (grupos, NCM, CFOP, etc.)
      │
      ▼
POST: ProdutoController.Create(ProdutoViewModel)
      │
      ├── ModelState.IsValid?
      ├── AutoMapper: ViewModel → Produto
      ├── ProdutoService.Adicionar(produto)
      │     ├── ProdutoValidation (FluentValidation)
      │     ├── Código único por empresa?
      │     └── Repository.AdicionarSemSalvar()
      ├── Códigos de barras → AdicionarSemSalvar()
      ├── Preços → AdicionarSemSalvar()
      ├── Foto (se houver) → AdicionarSemSalvar()
      └── ProdutoService.Salvar()
```

### Edição

```
GET: /produto/editar?id=X → ObterCompletoPorId()
POST: ProdutoController.Edit(ProdutoViewModel)
      ├── Atualizar produto
      ├── Atualizar/remover/adicionar códigos de barras
      └── Atualizar preços
```

### Atualização Automática (via Compra)

```
EfetivarCompra() → p/ cada item:
      ├── AtualizarCustoMedio()
      ├── AtualizarUltimoValorCompra()
      └── Se VLNOVOPRECOVENDA > 0 → AtualizarPrecoVenda()
```

---

## Classificação Fiscal

- **NCM**: Classificação fiscal do produto
- **CEST**: Substituição tributária
- **CST / CSOSN**: Regime de tributação
- **CFOP**: Natureza da operação

---

## Integração

- **Cardápio Digital**: `IntegracaoCardapioService.SincronizarProduto()`
- **Marketplace**: `ProdutoSiteMercado`

---

## Regras de Negócio

Consultar:

`docs/business-rules/produtos.md`

---

## Módulos Envolvidos

- `knowledge/business/produtos.md`
- `knowledge/business/categorias.md`
- `knowledge/business/fiscal.md`

---

## APIs Relacionadas

- `agilum.mvc.web/Controllers/ProdutoController.cs`
- `IProdutoService` — CRUD + Grupos, Marcas, Departamentos
- `IProdutoDapper` — `AtualizarCustoMedio()`, `AtualizarPrecoVenda()`

---

## Banco de Dados

- `produto` — CDPRODUTO, NMPRODUTO, NUPRECO, NURELACAO, dados fiscais
- `produto_codigo_barra` — Múltiplos EANs
- `produto_preco` — Preço de venda
- `produto_composicao` — Kits
- `produto_foto` — Imagens

---

## ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

## Documentação Relacionada

- `docs/fluxos/fluxo-produto.md` — Documentação oficial detalhada
- `docs/fluxos/fluxo-compra.md` — Atualização de custo via compra

---

## Documentação Oficial

`docs/fluxos/fluxo-produto.md`

---

## Fluxo Recomendado para Agentes de IA

1. Verificar `ProdutoValidation` para regras de validação
2. Verificar `IProdutoService` para CRUD completo
3. Verificar `IProdutoDapper` para consultas otimizadas
4. Verificar `IntegracaoCardapioService` para sincronização
5. Consultar `docs/fluxos/fluxo-produto.md` para detalhes

---

## Resumo

Cadastro com validação FluentValidation + código único por empresa. Classificação fiscal completa. Custo médio atualizado automaticamente na efetivação de compra. Integração com cardápio digital.
