# ADR-0006 - Estratégia de Autorização Baseada em Claims e Permissões

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é composto por diversos sistemas (Manager, PDV, Mobile, Cardápio Digital, APIs e futuras aplicações) que compartilham um mesmo modelo de autenticação.

Após a autenticação do usuário (definida na ADR-0005), torna-se necessário controlar quais recursos cada usuário pode acessar.

A plataforma possui perfis distintos (Administrador, Operador, Supervisor, Financeiro, Estoque, etc.) e diferentes permissões por empresa, módulo, funcionalidade e operação.

Era necessário definir uma estratégia única de autorização que pudesse ser utilizada por todas as aplicações da suíte.

---

# Problema

Controlar acesso utilizando verificações manuais espalhadas pelo código gera diversos problemas:

- Código duplicado;
- Regras inconsistentes;
- Dificuldade para manutenção;
- Baixa reutilização;
- Dificuldade para auditoria;
- Alto risco de falhas de segurança.

Era necessário centralizar o processo de autorização.

---

# Alternativas Consideradas

## Alternativa 1 — Controle manual nas Controllers

### Vantagens

- Simples.
- Não exige infraestrutura adicional.

### Desvantagens

- Código repetitivo.
- Alto risco de erros.
- Difícil manutenção.
- Baixa reutilização.

---

## Alternativa 2 — Controle baseado apenas em Roles

### Vantagens

- Fácil implementação.
- Suporte nativo do ASP.NET.

### Desvantagens

- Pouco flexível.
- Grande quantidade de Roles.
- Difícil granularidade.

---

## Alternativa 3 — Claims + Permissões (Escolhida)

### Vantagens

- Alta flexibilidade.
- Baixo acoplamento.
- Controle granular.
- Fácil expansão.
- Funciona para APIs e MVC.
- Escalável.

### Desvantagens

- Exige gerenciamento das permissões.
- Necessita infraestrutura para carregamento das Claims.

---

# Decisão

Foi adotada uma estratégia de autorização baseada em **Claims** associadas às permissões do usuário.

O Token JWT emitido durante a autenticação conterá apenas as informações necessárias para identificar o usuário.

As permissões poderão ser obtidas durante a autenticação ou carregadas pela aplicação conforme a estratégia definida para cada sistema.

Toda autorização deverá ocorrer através de Policies ou filtros centralizados.

---

# Objetivos

Esta decisão possui os seguintes objetivos:

- Padronizar autorização.
- Eliminar verificações manuais.
- Centralizar regras de acesso.
- Facilitar manutenção.
- Permitir granularidade.
- Atender todos os sistemas da suíte.

---

# Fluxo

```text
Login

↓

JWT

↓

Middleware Authentication

↓

ClaimsPrincipal

↓

Authorization Policy

↓

Controller

↓

Service

↓

Recurso protegido
```

---

# Modelo de Autorização

A autorização será composta por:

- Usuário
- Perfil
- Empresa
- Claims
- Permissões
- Policies

---

# Tipos de Permissão

As permissões poderão representar:

- Módulos
- Funcionalidades
- Operações
- Recursos

Exemplos:

```text
Produto.Visualizar

Produto.Incluir

Produto.Alterar

Produto.Excluir

Venda.Realizar

Venda.Cancelar

Financeiro.Consultar

Financeiro.Alterar

Usuario.Gerenciar
```

---

# Claims

As Claims poderão conter informações como:

- UserId
- EmpresaId
- Perfil
- Tenant
- Permissões
- Tipo de Usuário

As Claims não devem armazenar dados sensíveis.

---

# Policies

Toda autorização deverá utilizar Policies.

Exemplo:

```csharp
[Authorize(Policy = "Produto.Alterar")]
```

ou

```csharp
[Authorize(Policy = "Venda.Realizar")]
```

Evitar verificações manuais dentro das Controllers.

---

# Responsabilidades

## Authentication

Responsável por:

- Validar identidade.
- Gerar JWT.

---

## Authorization

Responsável por:

- Validar permissões.
- Validar Claims.
- Negar acesso quando necessário.

---

## Controller

Responsável apenas por:

- Declarar a Policy necessária.

Não deve implementar regras de autorização.

---

## Service

Pode realizar validações complementares relacionadas ao domínio, mas não deve substituir a autorização da aplicação.

---

# Granularidade

O sistema deve permitir autorização em diferentes níveis:

- Sistema
- Empresa
- Módulo
- Funcionalidade
- Operação

---

# Quando utilizar

Utilizar autorização para:

- Endpoints REST.
- Controllers MVC.
- Funcionalidades administrativas.
- Operações críticas.
- Recursos protegidos.

---

# Quando NÃO utilizar

Não utilizar autorização para validar regras de negócio.

Exemplos:

- Produto sem estoque.
- Venda cancelada.
- Caixa fechado.

Essas validações pertencem ao domínio da aplicação.

---

# Benefícios

- Código limpo.
- Segurança centralizada.
- Facilidade para auditoria.
- Escalabilidade.
- Reutilização.
- Menor acoplamento.
- Controle granular.

---

# Desvantagens

- Infraestrutura inicial maior.
- Necessidade de manter catálogo de permissões.
- Controle adicional durante autenticação.

---

# Riscos

Caso esta decisão não seja seguida:

- Verificações espalhadas.
- Duplicação de código.
- Falhas de segurança.
- Regras inconsistentes.
- Dificuldade para manutenção.

---

# Impacto

Esta decisão impacta:

- API
- MVC
- Mobile
- Cardápio Digital
- PDV
- Licenciamento
- Segurança
- Integrações

---

# Plano de Implementação

1. Definir catálogo de permissões.
2. Criar Policies.
3. Implementar Authorization Handlers quando necessário.
4. Configurar Middleware de Authorization.
5. Adaptar Controllers.
6. Atualizar documentação.
7. Validar permissões durante Code Review.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Toda autorização utiliza Policies.
- Não existem verificações manuais repetidas nas Controllers.
- O acesso é baseado em Claims e permissões.
- O Token JWT identifica corretamente o usuário autenticado.
- As permissões são centralizadas e reutilizáveis.
- As regras de negócio permanecem separadas das regras de autorização.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0002 — Repository Pattern
- ADR-0005 — Estratégia de Autenticação baseada em JWT
- ADR-0007 — Estratégia de Validação
- ADR-0014 — Tratamento Global de Exceções
- ADR-0015 — Padronização das Respostas da API

---

# Referências

- Microsoft — ASP.NET Core Authorization
- Microsoft — Policy-Based Authorization
- OWASP — Authorization Cheat Sheet
- RFC 7519 — JSON Web Token (JWT)
- NIST SP 800-63B — Digital Identity Guidelines

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| 1.0 | 2026-07-29 | Criação da ADR definindo a autorização baseada em Claims, Policies e permissões como padrão oficial do Agilium Manager, garantindo controle de acesso granular e centralizado para toda a plataforma. |