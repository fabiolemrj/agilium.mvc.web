# Autenticação

## Objetivo

Documentar a arquitetura de autenticação utilizada pelo ecossistema Agilium Manager, descrevendo os mecanismos de autenticação empregados pelas aplicações Web, APIs e demais consumidores, bem como sua configuração, fluxo de autenticação e boas práticas de segurança.

Este documento é a referência principal para autenticação em toda a plataforma.

---

# Escopo

Este documento contempla:

- Arquitetura de Autenticação
- ASP.NET Core Identity
- Cookie Authentication
- JWT Bearer Authentication
- Login
- Logout
- Sessão
- Renovação de Credenciais
- Configuração
- Segurança
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura
- Componentes
- Fluxo de Autenticação
- ASP.NET Core Identity
- Cookie Authentication
- JWT Bearer Authentication
- Configuração
- Sessão
- Logout
- Segurança
- Boas Práticas
- Limitações
- Documentação Relacionada

---

# Visão Geral

O ecossistema Agilium utiliza diferentes mecanismos de autenticação conforme o tipo de aplicação.

| Aplicação | Mecanismo |
|-----------|-----------|
| MVC Web | Cookie Authentication |
| APIs REST | JWT Bearer (quando implementado) |
| Serviços Internos | Conforme implementação |

A estratégia busca oferecer uma experiência consistente para usuários finais e consumidores das APIs.

---

# Arquitetura

```text
Usuário

    │

Login

    │

Identity / Serviço de Autenticação

    │

Credenciais Validadas

    │

─────────────────────────────

MVC
    │
    └── Cookie Authentication

API
    │
    └── JWT Bearer

─────────────────────────────

Autorização

    │

Controllers

    │

Services
```

---

# Componentes

Os principais componentes da autenticação são:

- ASP.NET Core Identity
- Cookie Authentication
- JWT Bearer Authentication
- AuthService
- AutenticacaoService
- CaUsuarioIdentity
- AppTokenSettings
- dbIdentityContext

A presença e utilização de cada componente deve ser confirmada em cada projeto da solução.

---

# Fluxo de Autenticação

Fluxo geral:

```text
Usuário

      │

Login

      │

Validação das Credenciais

      │

Identity / Serviço de Autenticação

      │

Geração da Credencial

      │

Cookie ou JWT

      │

Requisições Autenticadas
```

---

# ASP.NET Core Identity

O ASP.NET Core Identity é responsável pelo gerenciamento da identidade dos usuários.

Responsabilidades:

- autenticação;
- gerenciamento de usuários;
- gerenciamento de senhas;
- gerenciamento de Claims;
- gerenciamento de Roles;
- bloqueio de usuários;
- redefinição de senha.

A configuração efetiva (como `ApplicationDbContext`, entidades de usuário e políticas de senha) deve refletir a implementação existente no projeto.

---

# Cookie Authentication

A aplicação MVC utiliza autenticação baseada em Cookies.

Fluxo:

```text
Login

↓

Cookie

↓

Browser

↓

Requisições

↓

Validação

↓

Controller
```

Características:

- autenticação persistente;
- gerenciamento de sessão;
- expiração configurável;
- proteção contra acesso não autenticado.

---

# JWT Bearer Authentication

As APIs podem utilizar autenticação baseada em JWT.

Fluxo:

```text
Login

↓

JWT

↓

Cliente

↓

Authorization: Bearer {token}

↓

API

↓

Validação

↓

Controller
```

A configuração normalmente envolve:

- geração do token;
- assinatura;
- emissor (Issuer);
- público (Audience);
- tempo de expiração;
- chave de assinatura.

A implementação deve ser confirmada durante a análise dos projetos de API.

---

# Configuração

As configurações de autenticação normalmente encontram-se em:

- appsettings.json;
- appsettings.Development.json;
- appsettings.Production.json;
- User Secrets;
- Variáveis de Ambiente.

Itens comuns:

- chave de assinatura;
- issuer;
- audience;
- tempo de expiração;
- configurações do Cookie.

Informações sensíveis nunca devem ser armazenadas diretamente no repositório.

---

# Sessão

Para aplicações MVC, a autenticação é mantida por sessão baseada em Cookie.

Aspectos importantes:

- tempo de expiração;
- renovação automática (quando aplicável);
- invalidação após logout;
- proteção contra sessões expiradas.

---

# Logout

O processo de logout deve:

1. invalidar a autenticação;
2. remover o Cookie ou descartar o JWT no cliente;
3. encerrar a sessão do usuário;
4. redirecionar para a tela de login (quando aplicável).

---

# Segurança

Boas práticas recomendadas:

- utilizar HTTPS;
- proteger chaves de assinatura;
- configurar expiração adequada para tokens e cookies;
- nunca registrar senhas em logs;
- utilizar políticas de senha;
- limitar tentativas de login;
- invalidar sessões comprometidas.

---

# Boas Práticas

Sempre:

- centralizar a configuração de autenticação;
- reutilizar serviços de autenticação;
- documentar alterações na estratégia;
- revisar políticas de segurança periodicamente;
- manter autenticação e autorização desacopladas.

Evitar:

- armazenar credenciais em texto puro;
- expor informações sensíveis em respostas;
- duplicar lógica de autenticação em Controllers.

---

# Limitações Conhecidas

O levantamento técnico confirmou a utilização de ASP.NET Core Identity e Cookie Authentication na aplicação MVC.

A configuração completa da autenticação JWT nas APIs deverá ser confirmada durante a análise dos projetos:

- agilium-manager-azure-api
- agilium-pdv-azure-api

Também deverão ser verificados:

- AppTokenSettings;
- configuração do JwtBearer;
- políticas de senha;
- lockout;
- configuração do Identity.

---

# Documentação Relacionada

- authorization.md
- ../api/authentication.md
- ../api/swagger.md
- ../api/versioning.md
- ../architecture/security.md
- ../architecture/request-pipeline.md