# ADR-0005 - Estratégia de Autenticação baseada em JWT e Usuário do Domínio

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é composto por aplicações Web (MVC), APIs REST e integrações entre diferentes sistemas da suíte Agilium.

Inicialmente, alguns projetos utilizavam o ASP.NET Core Identity como mecanismo de autenticação. Entretanto, a evolução da plataforma exigiu a unificação do controle de acesso utilizando exclusivamente a entidade de domínio **Usuario**, eliminando a dependência das tabelas padrão do Identity.

Além disso, diversos sistemas clientes (PDV, Mobile, Cardápio Digital e futuras integrações) necessitam autenticar usuários através de uma API comum utilizando Tokens JWT.

Era necessário definir uma estratégia única de autenticação para toda a plataforma.

---

# Problema

A utilização do ASP.NET Core Identity introduzia uma série de limitações para a arquitetura adotada:

- Duplicação de dados entre Identity e entidade Usuario;
- Necessidade de sincronização entre tabelas;
- Complexidade desnecessária para o domínio da aplicação;
- Forte dependência da infraestrutura do Identity;
- Dificuldade para integração entre diferentes sistemas da suíte.

Era necessário que toda autenticação utilizasse exclusivamente o modelo de domínio existente.

---

# Alternativas Consideradas

## Alternativa 1 — ASP.NET Core Identity

### Vantagens

- Solução pronta.
- Ampla documentação.
- Recursos completos de autenticação.

### Desvantagens

- Duplicação da entidade de usuário.
- Forte acoplamento ao Identity.
- Maior complexidade.
- Baixa aderência ao modelo de domínio existente.

---

## Alternativa 2 — Autenticação baseada em Sessão (Cookies)

### Vantagens

- Simples para aplicações MVC.

### Desvantagens

- Pouco adequada para APIs REST.
- Dificulta integrações.
- Não atende aplicações Mobile.

---

## Alternativa 3 — JWT utilizando a entidade Usuario (Escolhida)

### Vantagens

- Modelo único de usuário.
- Stateless.
- Ideal para APIs REST.
- Fácil integração entre sistemas.
- Escalabilidade.
- Baixo acoplamento.

### Desvantagens

- Necessidade de gerenciamento de expiração.
- Revogação de tokens exige estratégia adicional.

---

# Decisão

Foi adotada a autenticação baseada em **JSON Web Token (JWT)** utilizando exclusivamente a entidade **Usuario** do domínio.

O ASP.NET Core Identity não será utilizado como mecanismo de autenticação.

Toda validação de credenciais deverá consultar diretamente o repositório de usuários da aplicação.

Após autenticação bem-sucedida, será emitido um Token JWT contendo apenas as informações necessárias para identificação do usuário.

---

# Objetivos

Esta decisão possui os seguintes objetivos:

- Unificar o modelo de autenticação.
- Eliminar dependências do ASP.NET Identity.
- Utilizar uma única entidade de usuário.
- Facilitar integrações entre sistemas.
- Padronizar autenticação em toda a plataforma.
- Simplificar manutenção.

---

# Fluxo de Autenticação

```text
Cliente

↓

API

↓

Validação do Usuário

↓

Repository

↓

Banco de Dados

↓

Geração do JWT

↓

Resposta

↓

Requisições autenticadas
```

---

# Processo de Login

1. Cliente envia usuário e senha.
2. API valida as credenciais.
3. O usuário é localizado utilizando a entidade Usuario.
4. A senha é validada.
5. São verificadas regras de acesso.
6. É gerado o Token JWT.
7. O Token é retornado ao cliente.

---

# Estrutura do Token

O Token JWT deverá conter apenas informações necessárias para identificação e autorização.

Exemplo de Claims:

- UserId
- Usuario
- EmpresaId
- Perfil
- Permissões
- Data de Expiração

Informações sensíveis nunca deverão ser armazenadas no Token.

---

# Entidade Oficial

A única entidade responsável pela autenticação é:

```text
Usuario
```

Não deverão existir entidades paralelas para autenticação.

---

# Credenciais

O processo de autenticação deverá utilizar:

- Usuário
- Senha

O login por e-mail não faz parte da estratégia padrão da plataforma, salvo necessidade específica de algum sistema.

---

# Armazenamento de Senhas

As senhas nunca deverão ser armazenadas em texto puro.

Devem ser protegidas através de algoritmo de hash seguro.

Requisitos:

- Hash criptográfico.
- Salt.
- Comparação segura.

---

# Expiração do Token

Todo Token deverá possuir:

- Data de emissão.
- Data de expiração.
- Tempo de vida configurável.

Após a expiração será necessário realizar nova autenticação.

---

# Renovação

A renovação do Token deverá ocorrer através de mecanismo específico da aplicação.

Não é permitido emitir Tokens permanentes.

---

# Logout

O processo de logout consiste em:

- Descartar o Token pelo cliente.
- Invalidar Tokens quando necessário através da estratégia definida pela aplicação.

---

# Quando utilizar JWT

Utilizar JWT para:

- APIs REST.
- Aplicações Mobile.
- Integrações entre sistemas.
- Aplicações Web desacopladas.
- Serviços internos.

---

# Quando NÃO utilizar

Evitar autenticação baseada em sessão para APIs REST.

Evitar armazenar estado de autenticação no servidor sempre que possível.

---

# Benefícios

- Arquitetura Stateless.
- Escalabilidade.
- Melhor integração entre sistemas.
- Modelo único de usuário.
- Redução de acoplamento.
- Facilidade para autenticação distribuída.
- Simplificação do domínio.

---

# Desvantagens

- Tokens precisam possuir tempo de expiração.
- Revogação exige estratégia específica.
- Cuidados adicionais com armazenamento do Token pelo cliente.

---

# Riscos

Caso esta decisão não seja seguida:

- Duplicação de usuários.
- Inconsistência entre sistemas.
- Complexidade na autenticação.
- Acoplamento ao ASP.NET Identity.
- Maior custo de manutenção.

---

# Impacto

Esta decisão impacta:

- API
- MVC
- Mobile
- Cardápio Digital
- PDV
- Licenciamento
- Integrações
- Segurança
- Banco de Dados

---

# Plano de Implementação

1. Utilizar exclusivamente a entidade Usuario.
2. Remover dependências do ASP.NET Core Identity.
3. Implementar autenticação JWT.
4. Configurar Middleware JWT.
5. Implementar geração de Claims.
6. Configurar expiração do Token.
7. Atualizar documentação.
8. Revisar integrações existentes.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Apenas a entidade Usuario participa da autenticação.
- Não existem tabelas do ASP.NET Identity para autenticação.
- Toda autenticação gera Token JWT.
- O Token contém apenas Claims necessárias.
- As senhas são armazenadas utilizando hash seguro.
- Toda API protegida valida JWT antes da execução.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0002 — Repository Pattern
- ADR-0004 — Entity Framework Core
- ADR-0006 — Estratégia de Autorização
- ADR-0009 — Dependency Injection
- ADR-0015 — Padronização das Respostas da API

---

# Referências

- RFC 7519 — JSON Web Token (JWT)
- Microsoft — ASP.NET Core JWT Bearer Authentication
- OWASP — Authentication Cheat Sheet
- Microsoft — ASP.NET Core Security Best Practices

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| 1.0 | 2026-07-29 | Criação da ADR definindo JWT como mecanismo oficial de autenticação do Agilium Manager, utilizando exclusivamente a entidade de domínio Usuario e eliminando a dependência do ASP.NET Core Identity. |