# Categorias

## Objetivo

Módulo responsável pela organização hierárquica do catálogo de produtos através de Grupos, SubGrupos, Departamentos e Marcas.

---

# Visão Geral

O Agilium Manager organiza produtos em uma hierarquia de 4 níveis: Grupo → SubGrupo → Marca + Departamento. Essa classificação é utilizada para organização do catálogo, relatórios e filtros.

---

# Responsabilidades

- Cadastro de Grupos de produtos (`GrupoProduto`)
- Cadastro de SubGrupos vinculados a Grupos (`SubGrupoProduto`)
- Cadastro de Marcas (`ProdutoMarca`)
- Cadastro de Departamentos (`ProdutoDepartamento`)
- Vínculo com produtos via chaves estrangeiras

---

# Principais Entidades

- `GrupoProduto` — Agrupamento principal (ex: Bebidas, Alimentos)
- `SubGrupoProduto` — Subdivisão do grupo (ex: Refrigerantes, Sucos)
- `ProdutoMarca` — Marca do produto (ex: Coca-Cola, Nestlé)
- `ProdutoDepartamento` — Departamento (ex: Mercearia, Hortifruti)

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-produto.md` — Cadastro de produto referencia categorias

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/ProdutoController.cs` — CRUD de categorias via ProdutoService

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

- `docs/padroes/services.md` — `IProdutoService` (métodos de Grupo, SubGrupo, Marca, Departamento)
- `knowledge/business/produtos.md` — Módulo de Produtos

---

# Documentação Oficial

`docs/business/categorias/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `IProdutoService` — métodos `ObterTodosGrupos()`, `ObterTodosSubGrupos()`, `ObterTodosDepartamento()`, `ObterTodosProdutoMarca()`
2. Verificar models em `agilium-manager-azure-business/Models/`
3. Verificar mappings em `agilium-manager-git-azure-infra/Mappings/ProdutoMapping.cs`

---

# Resumo

A hierarquia de categorias (Grupo → SubGrupo + Marca + Departamento) organiza o catálogo de produtos e é gerenciada pelo `ProdutoService`. SubGrupos são vinculados a Grupos via chave estrangeira.
