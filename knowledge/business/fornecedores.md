# Fornecedores

## Objetivo

Módulo responsável pelo cadastro e gestão de fornecedores, incluindo dados cadastrais, endereços, contatos e vínculo com compras.

---

# Visão Geral

Fornecedores são cadastrados para serem vinculados a compras (IDFORN). Cada fornecedor possui dados fiscais (CNPJ, IE), endereços e contatos. O fornecedor é referenciado em Compras e Contas a Pagar.

---

# Responsabilidades

- Cadastro e edição de fornecedores (CRUD)
- Gestão de endereços e contatos do fornecedor
- Vínculo com compras
- Vínculo com contas a pagar

---

# Principais Entidades

- `Fornecedor` — Cadastro principal (CNPJ, razão social, IE)
- `FornecedorEndereco` — Endereços
- `FornecedorContato` — Contatos

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-compra.md` — Fornecedor na criação/edição de compra

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/FornecedorController.cs`
- `agilium-manager-azure-api/V1/FornecedorController.cs`

---

# Regras de Negócio

Consultar:

`docs/business-rules/`

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

- `knowledge/business/compras.md` — Vínculo com compras
- `knowledge/business/financeiro.md` — Vínculo com contas a pagar

---

# Documentação Oficial

`docs/business/fornecedores/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `FornecedorController.cs` para operações CRUD
2. Verificar `Fornecedor` model em `agilium-manager-azure-business/Models/`
3. Verificar `IFornecedorService` para interface de serviços
4. Verificar `IFornecedorDapperRepository` para consultas otimizadas

---

# Resumo

Fornecedores são a contraparte das compras. O cadastro inclui dados fiscais, endereços e contatos. São referenciados em Compras (IDFORN) e Contas a Pagar (IDFORNEC).
