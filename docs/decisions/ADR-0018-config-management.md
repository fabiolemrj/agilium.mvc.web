# ADR-0018 - Estratégia de Gerenciamento de Configurações (Configuration Management)

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é composto por diversas aplicações que executam em ambientes distintos:

- Desenvolvimento
- Testes
- Homologação
- Produção
- Containers Docker
- Cloud
- Pipelines CI/CD

Cada ambiente possui configurações próprias como:

- Strings de conexão;
- Chaves JWT;
- URLs de APIs;
- Configurações SMTP;
- Serviços externos;
- Cache;
- Integrações;
- Feature Flags.

Historicamente parte dessas configurações era armazenada diretamente em arquivos de configuração ou alterada manualmente após o deploy, tornando o processo suscetível a erros.

Era necessário definir uma estratégia única de gerenciamento de configurações para toda a plataforma.

---

# Problema

Sem um padrão de gerenciamento de configurações ocorrem diversos problemas:

- Configurações diferentes entre ambientes;
- Segredos armazenados no repositório;
- Deploys inconsistentes;
- Dificuldade de automação;
- Baixa segurança;
- Erros de configuração.

Também havia dificuldade para executar a mesma aplicação em ambientes distintos sem alterar o código.

---

# Alternativas Consideradas

## Alternativa 1 — Configurações fixas no código

### Vantagens

- Implementação simples.

### Desvantagens

- Baixa flexibilidade.
- Necessidade de recompilar a aplicação.
- Alto risco de exposição de segredos.

---

## Alternativa 2 — Apenas appsettings.json

### Vantagens

- Padrão do ASP.NET Core.
- Fácil utilização.

### Desvantagens

- Segredos podem ser versionados.
- Difícil gerenciamento em ambientes Cloud.
- Não recomendado para produção.

---

## Alternativa 3 — Configuração Hierárquica (Escolhida)

Utilizar o sistema de configuração nativo do ASP.NET Core combinado com variáveis de ambiente e provedores externos.

### Vantagens

- Segurança.
- Portabilidade.
- Flexibilidade.
- Compatibilidade com Docker e Cloud.
- Integração com CI/CD.

### Desvantagens

- Configuração inicial mais elaborada.

---

# Decisão

Foi adotada uma estratégia de **Configuration Management** baseada no mecanismo oficial do ASP.NET Core.

As configurações deverão ser carregadas seguindo a ordem de precedência definida pela plataforma.

Configurações sensíveis nunca deverão ser armazenadas no código-fonte ou no repositório Git.

---

# Objetivos

Esta decisão possui os seguintes objetivos:

- Centralizar configurações.
- Padronizar ambientes.
- Melhorar segurança.
- Facilitar deploy.
- Suportar Docker e Cloud.
- Integrar com pipelines CI/CD.

---

# Hierarquia de Configuração

A aplicação deverá carregar configurações na seguinte ordem:

```text
appsettings.json

↓

appsettings.{Environment}.json

↓

User Secrets (Desenvolvimento)

↓

Variáveis de Ambiente

↓

Provedores Externos (Azure Key Vault, AWS Secrets Manager, etc.)
```

Cada nível sobrescreve o anterior.

---

# Organização

Estrutura recomendada:

```text
src/

├── Agilium.Manager.Api/

│   ├── appsettings.json

│   ├── appsettings.Development.json

│   ├── appsettings.Staging.json

│   ├── appsettings.Production.json

│   └── Program.cs
```

---

# Classes Tipadas

As configurações deverão ser representadas por classes fortemente tipadas utilizando o padrão **Options Pattern**.

Exemplo:

```text
Configuration

↓

Options Class

↓

Dependency Injection

↓

Application
```

Exemplo:

```csharp
public class JwtOptions
{
    public string Issuer { get; set; }
    public string Audience { get; set; }
    public int ExpirationMinutes { get; set; }
}
```

Registro:

```csharp
services.Configure<JwtOptions>(
    Configuration.GetSection("Jwt"));
```

---

# Variáveis de Ambiente

Todas as configurações sensíveis deverão ser fornecidas por variáveis de ambiente.

Exemplos:

```text
ConnectionStrings__DefaultConnection

Jwt__Secret

Jwt__Issuer

Jwt__Audience

Redis__ConnectionString

Smtp__Password
```

---

# Configurações Sensíveis

Nunca armazenar no repositório:

- Senhas;
- Connection Strings de produção;
- JWT Secret;
- API Keys;
- Certificados;
- Tokens;
- Chaves privadas.

Essas informações deverão ser obtidas por:

- Variáveis de ambiente;
- Secret Managers;
- Serviços do provedor de hospedagem.

---

# Ambientes

Cada ambiente poderá possuir configurações específicas.

Exemplo:

```text
Development

↓

Homologation

↓

Production
```

As diferenças deverão estar limitadas à configuração, nunca ao código.

---

# Docker

Os containers deverão receber configurações por:

- Variáveis de ambiente;
- Arquivos `.env`;
- Docker Secrets (quando disponível).

Nenhuma configuração deverá ser embutida na imagem Docker.

---

# CI/CD

Os pipelines deverão injetar as configurações durante o processo de implantação.

Fluxo:

```text
Pipeline

↓

Secrets

↓

Environment Variables

↓

Deploy
```

---

# Validação

A aplicação deverá validar configurações obrigatórias durante a inicialização.

Caso alguma configuração essencial esteja ausente, a aplicação deverá falhar imediatamente com mensagem clara registrada nos logs.

---

# Logging

Valores sensíveis nunca deverão ser registrados nos logs.

Permitido registrar:

- Nome da configuração;
- Ambiente;
- Fonte da configuração.

Não permitido registrar:

- Valores de segredos;
- Senhas;
- Tokens.

---

# Feature Flags

Recursos experimentais deverão ser controlados através de configurações externas.

Fluxo:

```text
Configuration

↓

Feature Flag

↓

Application Behavior
```

Isso evita novas publicações para habilitar ou desabilitar funcionalidades.

---

# Benefícios

- Configurações centralizadas.
- Maior segurança.
- Deploy simplificado.
- Compatibilidade com Docker.
- Compatibilidade com Cloud.
- Melhor integração com CI/CD.
- Código independente do ambiente.

---

# Desvantagens

- Configuração inicial mais detalhada.
- Necessidade de gerenciamento dos segredos.

---

# Riscos

Caso esta estratégia não seja seguida:

- Exposição de segredos.
- Configurações inconsistentes.
- Problemas de deploy.
- Dificuldade de automação.
- Alto custo operacional.

---

# Impacto

Esta decisão impacta:

- APIs
- MVC
- Docker
- Infraestrutura
- DevOps
- CI/CD
- Cloud
- Segurança
- Integrações

---

# Plano de Implementação

1. Organizar arquivos `appsettings`.
2. Criar classes Options para configurações.
3. Configurar binding utilizando Options Pattern.
4. Externalizar todos os segredos.
5. Configurar variáveis de ambiente nos ambientes.
6. Validar configurações obrigatórias na inicialização.
7. Atualizar documentação operacional.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Todas as configurações utilizam o sistema oficial do ASP.NET Core.
- Configurações sensíveis não estão versionadas.
- A aplicação utiliza Options Pattern.
- Os ambientes diferem apenas por configuração.
- Os containers recebem configurações externamente.
- Configurações obrigatórias são validadas no startup.

---

# ADRs Relacionados

- ADR-0005 — Estratégia de Autenticação
- ADR-0009 — Dependency Injection
- ADR-0012 — Estratégia de Containerização e Deploy
- ADR-0013 — Estratégia de Logging
- ADR-0017 — Estratégia de Auditoria
- ADR-0019 — Estratégia de Migrations
- ADR-0020 — Estratégia de Testes

---

# Referências

- Microsoft — Configuration in ASP.NET Core
- Microsoft — Options Pattern
- Microsoft — Secret Manager
- Microsoft — Environment Variables
- Twelve-Factor App — Config
- OWASP Secrets Management Cheat Sheet

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | **2026-07-29** | Criação da ADR definindo a estratégia oficial de gerenciamento de configurações do Agilium Manager, estabelecendo o uso do sistema de configuração do ASP.NET Core, Options Pattern, variáveis de ambiente, gerenciamento seguro de segredos e suporte a ambientes Docker, Cloud e CI/CD. |