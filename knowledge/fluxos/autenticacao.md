# Autenticação

## Objetivo

Documentar o fluxo de autenticação e autorização do Agilium Manager: login, Identity Core, Cookie Authentication, Claims e permissões.

---

## Visão Geral

Autenticação via ASP.NET Core Identity com Cookie Authentication (3h sliding, HttpOnly). Pós-login exige seleção de empresa. Autorização por Claims com validação a cada requisição via `ClaimsAuthorizeAttribute(idTag)` + `CaService.UsuarioTemPermissao()`.

---

## Fluxo Principal

```
[Browser] → /Identity/Account/Login
      │
      ▼
POST: SignInManager.PasswordSignInAsync(email, password)
      ├── Falha: lockout (5 tentativas, 5 min) ou erro de senha
      └── Sucesso: ClaimsPrincipal + Cookie HttpOnly (3h sliding)
      │
      ▼
Redireciona → Seleção de Empresa
      │
      ▼
POST: /Empresa/SelecionarEmpresa → Session
      │
      ▼
Redirect /Home/Index
```

---

## Pré-condições

- Usuário cadastrado (`CaUsuarioIdentity`)
- Empresa vinculada ao usuário (`EmpresaAuth`)
- Senha: 6+ caracteres, exige dígito

---

## Autorização

- **ClaimsAuthorizeAttribute(idTag)**: Cada action protegida
- **RequisitoClaimFilter**: `IAuthorizationFilter` que valida permissão
- **CaService.UsuarioTemPermissao(userId, idTag)**: Consulta no banco
- Sem permissão → Redirect `/Home/Error/403`

---

## APIs Envolvidas

- Identity Razor Pages (`Areas/Identity/`)
- `EmpresaController.SelecionarEmpresa`

---

## Banco de Dados

- `dbIdentityContext` (MySQL): tabelas Identity (`aspnetusers`, `aspnetroles`, `aspnetuserclaims`)
- Tabelas de permissão (idTags)

---

## Regras de Negócio

Consultar:

`docs/business-rules/usuario.md`
`docs/business-rules/autorizacao.md`

---

## ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

## Documentação Relacionada

- `docs/fluxos/fluxo-autenticacao.md` — Documentação oficial detalhada
- `knowledge/business/usuarios.md` — Módulo de usuários
- `knowledge/frontend/authentication.md` — Frontend auth

---

## Documentação Oficial

`docs/fluxos/fluxo-autenticacao.md`

---

## Fluxo Recomendado para Agentes de IA

1. Verificar `IdentityConfig.cs` para configuração de senha e cookie
2. Verificar `CustomAuth.cs` para ClaimsAuthorize e RequisitoClaimFilter
3. Verificar `CaService.UsuarioTemPermissao()` para validação
4. Verificar `EmpresaSelecionadaMiddleware` para middleware de empresa
5. Consultar `docs/fluxos/fluxo-autenticacao.md` para detalhes

---

## Resumo

Login via Identity Core + Cookie (3h, sliding, HttpOnly). Pós-login força seleção de empresa. Autorização por idTag validada a cada requisição. Lockout de 5 tentativas/5 min.
