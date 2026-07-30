# Produtos

## Objetivo

Módulo responsável pelo cadastro e manutenção do catálogo de produtos, incluindo classificação fiscal, preços, códigos de barras, fotos, composições e integração com cardápio digital.

---

# Visão Geral

O módulo de Produtos é um dos pilares do ERP. Cada produto possui classificação hierárquica (Grupo → SubGrupo → Departamento → Marca), informações fiscais (NCM, CEST, CST, CFOP, IBPT), múltiplos códigos de barras, preço de venda e pode ser composto (kits). A interface utiliza `CreateEdit.cshtml` para criar e editar.

---

# Responsabilidades

- Cadastro e edição de produtos (CRUD)
- Classificação hierárquica em Grupo, SubGrupo, Departamento e Marca
- Configuração fiscal (NCM, CEST, CST, CSOSN, CFOP, OrigemProduto)
- Gestão de preços (`ProdutoPreco`)
- Gestão de códigos de barras (`ProdutoCodigoBarra`)
- Produtos compostos (kits) via `ProdutoComposicao`
- Fotos do produto (`ProdutoFoto`)
- Integração com cardápio digital (`IntegracaoCardapioService`)
- Cálculo de custo médio (atualizado via efetivação de compra)

---

# Principais Entidades

- `Produto` — Cadastro principal (CDPRODUTO, NMPRODUTO, NUPRECO, NURELACAO)
- `ProdutoCodigoBarra` — Múltiplos EANs por produto
- `ProdutoPreco` — Preço de venda
- `ProdutoComposicao` — Kits (produto pai → produtos filhos)
- `ProdutoFoto` — Imagens do produto
- `GrupoProduto`, `SubGrupoProduto` — Categorização
- `ProdutoDepartamento`, `ProdutoMarca` — Classificação
- `ProdutoSiteMercado` — Integração com marketplace

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-produto.md` — Cadastro e edição de produtos
- `docs/fluxos/fluxo-compra.md` — Atualização de custo médio na efetivação
- `docs/fluxos/fluxo-venda.md` — Busca de produto no PDV

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/ProdutoController.cs`
- `agilium-manager-azure-api/V1/ProdutoController.cs`

---

# Regras de Negócio

Consultar:

`docs/business-rules/produtos.md`

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

- `docs/padroes/validacoes.md` — `ProdutoValidation`
- `docs/padroes/services.md` — `ProdutoService`
- `docs/frontend/razor.md` — Views de Produto

---

# Documentação Oficial

`docs/business/produtos/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `ProdutoController.cs` para endpoints MVC
2. Verificar `Produto` model em `agilium-manager-azure-business/Models/`
3. Verificar `ProdutoValidation` para regras de validação
4. Verificar `IProdutoService` para interface de serviços
5. Consultar `IProdutoDapper` para consultas otimizadas
6. Verificar `IntegracaoCardapioService` para sincronização

---

# Resumo

Produtos formam o catálogo central do ERP, com classificação fiscal completa, suporte a kits, múltiplos códigos de barras e integração com cardápio digital. O custo médio é recalculado a cada efetivação de compra.
