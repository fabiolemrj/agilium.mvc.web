# Empresas

## Objetivo

Módulo responsável pelo gerenciamento multi-empresa do Agilium Manager, permitindo que um mesmo sistema atenda múltiplas empresas com configurações, dados e usuários independentes.

---

# Visão Geral

O Agilium Manager suporta múltiplas empresas no mesmo banco de dados. Cada registro de entidade principal (Produto, Venda, Compra, Caixa, etc.) é escopado por `IDEMPRESA`. A empresa é selecionada após o login via `EmpresaSelecionadaMiddleware` e armazenada na sessão.

---

# Responsabilidades

- Cadastro de empresas (razão social, CNPJ, IE, regime tributário)
- Seleção de empresa pós-login (middleware)
- Configurações por empresa (fiscal, financeiro, PDV, e-mail)
- Vínculo de usuários a empresas (`EmpresaAuth`)
- Isolamento de dados entre empresas

---

# Principais Entidades

- `Empresa` — Cadastro principal (IDEMPRESA, razão social, CNPJ, IE, CNAE, regime)
- `EmpresaAuth` — Vínculo usuário × empresa (quais empresas o usuário pode acessar)
- `EmpresaUsuarioViewModel` — Armazenado na sessão após seleção

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-autenticacao.md` — Login → seleção de empresa
- `docs/fluxos/fluxo-configuracao.md` — Seleção de empresa

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/EmpresaController.cs` — CRUD e seleção
- `agilium-manager-azure-api/V1/EmpresaController.cs`

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

- `knowledge/domain.md` — Modelo de domínio
- `knowledge/architecture.md` — `EmpresaSelecionadaMiddleware`
- `docs/fluxos/fluxo-configuracao.md` — Configurações por empresa

---

# Documentação Oficial

`docs/business/empresas/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `EmpresaController.cs` para CRUD e seleção
2. Verificar `EmpresaSelecionadaMiddleware.cs` no pipeline
3. Verificar `IDEMPRESA` como chave estrangeira em TODAS as entidades principais
4. Consultar `_utilDapperRepository.ConfigRetornaValor("CHAVE", idEmpresa)` para configurações por empresa

---

# Resumo

Multi-empresa é um conceito transversal no Agilium Manager. Toda operação é escopada por empresa, isolada via `IDEMPRESA` e controlada pelo middleware `EmpresaSelecionadaMiddleware`.
