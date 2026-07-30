# Usuários

## Objetivo

Módulo responsável pela gestão de usuários do sistema, incluindo cadastro, autenticação (Identity Core), perfis de acesso (ClaimsAuthorize) e vínculo com funcionários e empresas.

---

# Visão Geral

O Agilium Manager utiliza ASP.NET Core Identity com Cookie Authentication. O modelo de usuário é `CaUsuarioIdentity` (herda de `IdentityUser`). As permissões são validadas via `ClaimsAuthorizeAttribute` (idTag) + `CaService.UsuarioTemPermissao()`. O lockout é de 5 minutos após 5 tentativas.

---

# Responsabilidades

- Cadastro e gestão de usuários (CRUD)
- Autenticação via Identity Core + Cookie
- Autorização por Claims (idTag) com validação a cada requisição
- Vínculo usuário × empresa (`EmpresaAuth`)
- Vínculo usuário × funcionário (para operações de PDV e caixa)
- Política de senha (6 caracteres, exige dígito)
- Logout e expiração de sessão (3 horas)

---

# Principais Entidades

- `CaUsuarioIdentity` — Usuário Identity (herda `IdentityUser`)
- `Usuario` — Usuário de negócio (link com Identity via `IDUSUARIO_ASPNET`)
- `EmpresaAuth` — Vínculo usuário × empresa
- `Funcionario` — Vínculo com operações de PDV/Caixa

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-autenticacao.md` — Login, logout, claims, permissões
- `docs/fluxos/fluxo-caixa.md` — Funcionário vinculado ao caixa
- `docs/fluxos/fluxo-venda.md` — Usuário na realização de venda

---

# APIs Relacionadas

- `agilum.mvc.web/Controllers/UsuarioController.cs`
- `agilium-manager-azure-api/V1/UsuarioController.cs`
- `agilium-pdv-azure-api/` — autenticação no PDV

---

# Regras de Negócio

Consultar:

`docs/business-rules/usuario.md`

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

- `knowledge/architecture.md` — Pipeline de autenticação
- `docs/padroes/notification.md` — Notification Pattern
- `docs/frontend/mvc.md` — ClaimsAuthorize nos controllers

---

# Documentação Oficial

`docs/business/usuarios/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `IdentityConfig.cs` para configuração de senha e cookie
2. Verificar `CustomAuth.cs` para `ClaimsAuthorizeAttribute` e `RequisitoClaimFilter`
3. Verificar `CaService.UsuarioTemPermissao()` para validação de permissões
4. Consultar `CaUsuarioIdentity` em `agilium-manager-azure-business/Models/`
5. Verificar `dbIdentityContext` em `agilum.mvc.web/Data/`

---

# Resumo

Usuários são gerenciados via ASP.NET Core Identity com autenticação por Cookie (3h, sliding). Permissões são validadas por idTag via `ClaimsAuthorizeAttribute`. O usuário deve estar vinculado a uma empresa e, para operações de PDV, a um funcionário.
