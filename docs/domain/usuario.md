# Módulo Usuários

## Objetivo

O módulo **Usuários** gerencia as contas de acesso ao sistema, autenticação, perfis de permissão, vínculo com empresas e controle de acesso às funcionalidades via tags de permissão.

---

# Responsabilidades

- Cadastro de usuários
- Autenticação (login/logout)
- Gerenciamento de perfis (CaPerfil)
- Gerenciamento de permissões (CaPermissaoItem, CaPermissaoManager)
- Vínculo com empresas (EmpresaAuth)
- Controle de acesso via ClaimsAuthorizeAttribute
- Cadastro de fotos de usuário (MongoDB)
- Refresh Token

---

# Fluxo de Autenticação

```
Login (/Identity/Account/Login)

↓

SignInManager.PasswordSignInAsync()

↓

Cookie Authentication

↓

Carregar Claims (UserId, Email, Roles)

↓

Selecionar Empresa (EmpresaSelecionadaMiddleware)

↓

Acessar Funcionalidades (ClaimsAuthorizeAttribute → ICaService.UsuarioTemPermissao)
```

---

# Principais Entidades

| Entidade | Descrição |
|----------|-----------|
| Usuario | Cadastro do usuário no sistema |
| CaUsuarioIdentity | Entidade Identity Core (aspnetusers) |
| CaPerfil | Perfil de acesso |
| CaPerfiManager | Vínculo usuário-perfil |
| CaPermissaoItem | Item de permissão (tag) |
| CaPermissaoManager | Vínculo perfil-permissão |
| CaAreaManager | Áreas do sistema |
| CaModelo | Modelos de permissão |
| EmpresaAuth | Vínculo usuário-empresa |
| UsuarioFoto | Foto do usuário (MongoDB) |
| RefreshToken | Token de renovação de sessão |

---

# Dependências

- Empresa
- Identity Core (Microsoft.AspNetCore.Identity)
- MongoDB (fotos)

---

# Regras de Negócio

## Cadastro

- Nome e CPF obrigatórios
- Senha: mínimo 6 caracteres, deve conter dígito
- Email não precisa ser único
- Usuário vinculado a pelo menos uma empresa

## Segurança

- Lockout após 5 tentativas (5 minutos)
- Senha não pode conter caracteres especiais obrigatoriamente
- Sessão expira em 3 horas (cookie + session)

## Permissões

- Cada ação de controller tem um idTag
- `ICaService.UsuarioTemPermissao(userId, idTag)` valida acesso
- Perfis agrupam permissões

---

# Serviços Envolvidos

- UsuarioService
- CaService (controle de acesso)
- CaUsuarioService
- UserClaimsManagerService
- AuthService (autenticação customizada)
- UsuarioFotoService / UsuarioFotoEntityService

---

# Controllers Relacionados

- UsuarioController (`agilum.mvc.web/Controllers/UsuarioController.cs`)

---

# Boas Práticas

- Não armazenar senhas em texto plano
- Validar permissões a cada requisição
- Não expor dados de outros usuários
- Manter log de acessos e alterações de permissão

---

# Checklist

☐ Nome e CPF informados

☐ Senha atende requisitos mínimos

☐ Empresa vinculada

☐ Perfil atribuído

☐ Permissões coerentes com o cargo

☐ Foto cadastrada (se necessário)

---

# Conclusão

O módulo **Usuários** é responsável por toda a segurança de acesso ao sistema. O modelo de permissões baseado em tags numéricas permite controle granular sobre cada funcionalidade.
