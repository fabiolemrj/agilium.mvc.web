# Autenticação da API

## Objetivo

Documentar completamente como a autenticação e a autorização são implementadas nas APIs do **Agilium Manager**.

Este documento deve descrever o fluxo completo de autenticação, geração e validação de tokens, configuração de segurança e todas as regras relacionadas ao controle de acesso.

> **Importante**
>
> Este documento deve refletir sempre a implementação existente no código-fonte.
> Não documente comportamentos esperados ou hipotéticos. Documente apenas o que foi identificado durante a análise da solução.

---

# Escopo

Este documento contempla:

- Arquitetura da autenticação
- Fluxo de autenticação
- Processo de login
- Configuração do JWT
- Geração de Tokens
- Validação de Tokens
- Claims
- Papéis e Permissões
- Políticas de Autorização
- Refresh Token (quando existir)
- Middleware de autenticação
- Configuração de segurança
- Ciclo de vida do Token
- Logout e revogação
- Provedores externos (quando aplicável)

---

# Fontes para Análise

Antes de preencher este documento, analisar:

- Startup.cs
- Program.cs
- Configuração da autenticação
- Configuração da autorização
- Dependency Injection
- Configuração do JwtBearer
- Serviços de autenticação
- Serviços de geração de Token
- Middleware de autenticação
- Controllers responsáveis pelo Login
- Classe AppTokenSettings
- appsettings.json
- appsettings.Development.json
- Variáveis de ambiente
- Atributos `[Authorize]`
- Atributos `[AllowAnonymous]`
- Policies
- Claims
- Roles (quando utilizadas)

---

# Índice

- Visão Geral
- Fluxo de Autenticação
- Processo de Login
- JWT Bearer
- Estrutura do Token
- Claims
- Autorização
- Papéis e Permissões
- Refresh Token
- Ciclo de Vida do Token
- Configuração
- Diagrama do Fluxo
- Considerações de Segurança
- Limitações Conhecidas
- Documentos Relacionados

---

# Visão Geral

## Objetivo

Descrever:

- como a autenticação funciona;
- qual mecanismo é utilizado;
- onde está configurada;
- quais projetos participam do processo.

> **TODO:** Documentar após análise do código.

---

# Fluxo de Autenticação

Documentar todo o fluxo de autenticação.

Exemplo:

1. O cliente envia as credenciais.
2. A API valida o usuário.
3. O usuário é carregado.
4. As Claims são geradas.
5. O Token JWT é criado.
6. O Token é retornado ao cliente.
7. As requisições protegidas validam o Token.

> **TODO:** Descrever o fluxo real implementado.

---

# Processo de Login

Documentar:

- endpoint responsável;
- modelo da requisição;
- validação das credenciais;
- validação da senha;
- notificações de erro;
- resposta retornada;
- códigos HTTP utilizados.

> **TODO:** Documentar após análise.

---

# JWT Bearer

Descrever:

- esquema de autenticação;
- configuração do JwtBearer;
- parâmetros de validação;
- Issuer;
- Audience;
- Chave de assinatura;
- Tempo de expiração.

> **TODO:** Documentar conforme implementação.

---

# Estrutura do Token

Documentar a estrutura do Token JWT.

| Claim | Descrição | Obrigatória |
|--------|-----------|-------------|
| sub | Identificador do usuário | Sim |
| name | Nome do usuário | Sim |
| role | Perfil do usuário | Quando existir |
| exp | Data de expiração | Sim |
| iss | Emissor | Sim |

> **TODO:** Atualizar conforme as Claims reais.

---

# Claims

Documentar:

- todas as Claims geradas;
- origem de cada Claim;
- finalidade;
- impacto na autorização.

> **TODO:** Documentar após análise.

---

# Autorização

A autorização no projeto `agilum.mvc.web` (MVC) utiliza **dois níveis** de controle:

| Nível | Mecanismo | Escopo |
|-------|-----------|--------|
| Controller | `[Authorize]` | Bloqueia acesso não autenticado |
| Action | `ClaimsAuthorizeAttribute(int idTag)` | Verifica permissão específica via `ICaService.UsuarioTemPermissao()` |

### [Authorize]

Aplicado em **todos os controllers**. Exemplo:

```csharp
[Route("compra")]
[Authorize]
public class CompraController : MainController { }
```

### ClaimsAuthorizeAttribute

Mecanismo customizado de autorização por ação, definido em `Extensions/CustomAuth.cs`:

```csharp
[Route("novo")]
[ClaimsAuthorizeAttribute(2067)]   // tag da permissão
public async Task<IActionResult> Create() { }
```

> Veja a documentação completa em **[./authorization.md#claimsauthorizeattribute](./authorization.md#claimsauthorizeattribute)** — incluindo fluxo de validação, implementação do `RequisitoClaimFilter`, e exemplos de tags por ação.

### [AllowAnonymous]

Utilizado apenas em endpoints públicos (login, logout, recuperação de senha) nas Razor Pages da área Identity.

### Policies customizadas

Não foram identificadas `AuthorizationPolicy` customizadas. O controle é feito via `ClaimsAuthorizeAttribute` + `ICaService`.

---

# Papéis e Permissões

O sistema adota um modelo de **permissões baseado em tags numéricas**, não em roles tradicionais.

### Como funciona

1. Cada ação de controller é decorada com `[ClaimsAuthorizeAttribute(N)]`, onde `N` é o **idTag** da permissão
2. O filtro `RequisitoClaimFilter` intercepta a requisição
3. Chama `ICaService.UsuarioTemPermissao(userId, idTag)` que consulta as permissões do usuário
4. Se o usuário **não** tem a permissão → HTTP 403 (`/Home/Error/403`)

### Exemplos de Tags

| idTag | Funcionalidade | Controller |
|-------|---------------|------------|
| 2066 | Listar Compras | `CompraController.IndexCompra` |
| 2067 | Criar Compra | `CompraController.Create` |
| 2068 | Cancelar Compra | `CompraController.Cancelar` |
| 2070 | Editar Compra / Importar XML | `CompraController.Edit` |
| 2072 | Efetivar Compra | `CompraController.Efetivar` |

### Identity Roles

O ASP.NET Core Identity com `AddIdentityCore` + `.AddRoles<IdentityRole>()` está configurado, mas o controle de acesso em tempo de execução é feito via `ICaService` (tabelas customizadas `ca_permissao_item`, `ca_permissao_manager`), não via `[Authorize(Roles = "...")]`.

> Detalhes completos em **[./authorization.md](./authorization.md)**.

---

# Refresh Token

Caso exista:

Documentar:

- geração;
- armazenamento;
- expiração;
- renovação;
- revogação.

Caso não exista, informar explicitamente.

---

# Ciclo de Vida do Token

Documentar:

- geração;
- validação;
- expiração;
- renovação;
- invalidação;
- logout.

> **TODO:** Documentar conforme implementação.

---

# Configuração

Documentar todas as configurações relacionadas à autenticação.

Exemplos:

- AppTokenSettings
- appsettings.json
- appsettings.Development.json
- Variáveis de Ambiente

Explicar a finalidade de cada configuração.

---

# Diagrama do Fluxo

Adicionar um diagrama representando o fluxo de autenticação.

Exemplo:

```text
Cliente
   │
   │ Login
   ▼
Controller
   │
Service
   │
Repository
   │
Banco de Dados
   │
Serviço JWT
   │
Retorna Token
```

> **TODO:** Atualizar com o fluxo real.

---

# Considerações de Segurança

Documentar:

- armazenamento das senhas;
- algoritmo de hash;
- proteção contra força bruta;
- tempo de expiração do Token;
- uso obrigatório de HTTPS;
- proteção de informações sensíveis.

> **TODO:** Documentar conforme implementação.

---

# Limitações Conhecidas

Registrar limitações atuais da implementação.

Não propor soluções neste documento.

---

# Documentos Relacionados

- ../architecture/authentication.md
- ../architecture/authorization.md
- ../business/permissions.md
- ./conventions.md
- ./errors.md
- ../development/environment.md

---

# Manutenção

Sempre que houver alterações na autenticação:

- atualizar este documento;
- revisar a documentação relacionada;
- atualizar diagramas;
- validar exemplos;
- garantir que o conteúdo reflita a implementação atual.

Este documento deve permanecer sincronizado com o código-fonte.