# Funcionários

## Objetivo

Módulo responsável pelo cadastro e gestão de funcionários, incluindo vínculo com usuários do sistema, operações de PDV/caixa e controle de acesso.

---

# Visão Geral

O módulo de Funcionários gerencia o cadastro de pessoas que operam o sistema, especialmente no PDV. Cada funcionário pode ser vinculado a um usuário do sistema (`CaUsuarioIdentity`) e é referenciado nas operações de Caixa (`IDFUNC`) e Venda (`RealizarVenda` obtém `idFuncionario` via `ObterIdFuncionarioPorUsuarioEmpresa`).

---

# Responsabilidades

- Cadastro e edição de funcionários (CRUD)
- Vínculo funcionário × usuário do sistema
- Vínculo funcionário × empresa
- Identificação do funcionário para abertura de caixa
- Identificação do funcionário para realização de venda
- Controle de permissões operacionais (PDV, caixa)

---

# Principais Entidades

- `Funcionario` — Cadastro principal (nome, CPF, cargo, situação)
- Relacionamento com `Caixa` — `IDFUNC` (operador do caixa)
- Relacionamento com `Usuario` — vinculação para login

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-caixa.md` — Funcionário ao abrir caixa
- `docs/fluxos/fluxo-venda.md` — Funcionário na realização de venda
- `docs/fluxos/fluxo-autenticacao.md` — Vínculo usuário × funcionário

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/FuncionarioController.cs`
- `agilium-manager-azure-api/V1/`

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

- `knowledge/business/usuarios.md` — Vínculo com usuário do sistema
- `knowledge/business/caixa.md` — Funcionário como operador de caixa
- `knowledge/business/vendas.md` — Funcionário na venda

---

# Documentação Oficial

`docs/business/funcionarios/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `FuncionarioController.cs` para operações CRUD
2. Verificar `Funcionario` model em `agilium-manager-azure-business/Models/`
3. Verificar `Caixa` model — campo `IDFUNC`
4. Verificar `VendaService.RealizarVenda()` — `ObterIdFuncionarioPorUsuarioEmpresa()`
5. Verificar `ICaixaDapperRepository.ObterIdFuncionarioPorUsuarioEmpresa()` para obtenção do vínculo
6. Verificar `ICaixaService.AbrirCaixa()` — parâmetro `idUsuario`

---

# Resumo

Funcionários são a ponte entre usuários do sistema e operações de PDV/caixa. Cada funcionário é vinculado a um usuário e referenciado no Caixa (`IDFUNC`) e nas operações de venda.
