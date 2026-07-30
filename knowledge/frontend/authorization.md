# Autorização

## Objetivo

Documentar o controle de permissões e políticas de acesso no frontend do Agilium Manager.

---

# Visão Geral

A autorização é baseada em **Claims** com validação por **idTag**. O atributo `[ClaimsAuthorize(idTag)]` em cada action do Controller dispara `RequisitoClaimFilter` que verifica se o usuário tem permissão via `CaService.UsuarioTemPermissao()`. Sem permissão → redirect para `/Home/Error/403`.

---

# Organização

### Fluxo de Autorização

```
[Browser] Acessa action protegida
      │
      ▼
[Middleware] UseAuthorization
      │
      ▼
[Action] [ClaimsAuthorize(2066)]
      │
      ▼
RequisitoClaimFilter.OnAuthorization()
      ├── User.Identity.IsAuthenticated?
      │     └── Não → Redirect /Identity/Account/Login
      │
      ├── Obtém UserId da Claim
      │
      └── CaService.UsuarioTemPermissao(userId, idTag)
            ├── True → Executa Action
            └── False → Redirect /Home/Error/403
```

---

# Principais Conceitos

- **ClaimsAuthorizeAttribute(idTag)**: Atributo em cada action protegida
- **RequisitoClaimFilter**: `IAuthorizationFilter` que executa a validação
- **idTag**: Identificador numérico da permissão (ex: 2066 = Listar Compras)
- **CaService.UsuarioTemPermissao()**: Consulta permissão no banco

### Exemplos de idTags

| idTag | Funcionalidade | Controller |
|-------|---------------|------------|
| 2066 | Listar Compras | `CompraController.IndexCompra` |
| 2067 | Criar Compra | `CompraController.Create` |
| 2068 | Cancelar Compra | `CompraController.Cancelar` |
| 2070 | Editar / Importar XML | `CompraController.Edit` |
| 2072 | Efetivar Compra | `CompraController.Efetivar` |

---

# Fluxos Relacionados

- `docs/fluxos/fluxo-autenticacao.md` — Autenticação precede autorização

---

# Componentes Relacionados

- `CustomAuth.cs` — `ClaimsAuthorizeAttribute` + `RequisitoClaimFilter`
- `CaService.UsuarioTemPermissao()` — Validação da permissão
- `_ASideMenu.cshtml` — Menu deve respeitar permissões

---

# APIs Relacionadas

- N/A

---

# Boas Práticas

- Toda action protegida deve ter `[Authorize]` + `[ClaimsAuthorize(idTag)]`
- idTags são numéricos e mapeados a funcionalidades específicas
- Menu lateral deve esconder itens sem permissão
- Erro 403 deve ter página amigável

---

# ADRs Relacionadas

Consultar:

`knowledge/decisions.md`

---

# Documentação Relacionada

- `docs/fluxos/fluxo-autenticacao.md` — Fluxo completo de auth
- `knowledge/business/usuarios.md` — Permissões de usuário
- `docs/frontend/mvc.md` — Controllers com ClaimsAuthorize

---

# Documentação Oficial

`docs/frontend/`

---

# Fluxo Recomendado para Agentes de IA

1. Verificar `CustomAuth.cs` — `ClaimsAuthorizeAttribute` e `RequisitoClaimFilter`
2. Verificar `CaService.UsuarioTemPermissao()` — lógica de validação
3. Verificar `ICaRepository.UsuarioTemPermissaoAcesso()` — consulta no banco
4. Ao criar novas actions, adicionar `[ClaimsAuthorize(idTag)]` apropriado

---

# Resumo

Autorização por Claims com idTag numérico. `RequisitoClaimFilter` valida a cada requisição. Sem permissão = 403. idTags mapeados a funcionalidades específicas.
