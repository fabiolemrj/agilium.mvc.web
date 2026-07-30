# ADR-0012 - Estratégia de Containerização e Deploy com Docker

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é composto por aplicações ASP.NET Core MVC, APIs REST, serviços de integração e futuras aplicações auxiliares que precisam ser executadas de forma consistente em diferentes ambientes.

Os ambientes da plataforma incluem:

- Desenvolvimento
- Homologação
- Produção
- Ambientes Cloud
- Containers locais
- Pipelines de CI/CD

Ao longo da evolução da solução foram identificadas diferenças entre ambientes, problemas de configuração, dependências de sistema operacional e dificuldades na publicação.

Era necessário definir um padrão de empacotamento e implantação para toda a plataforma.

---

# Problema

Realizar deploy diretamente no sistema operacional gera diversos problemas:

- Diferenças entre ambientes;
- Dependências não documentadas;
- Problemas de configuração;
- Dificuldade para reproduzir erros;
- Deploy manual;
- Escalabilidade limitada.

Era necessário padronizar a execução das aplicações independentemente do ambiente.

---

# Alternativas Consideradas

## Alternativa 1 — Deploy Manual

### Vantagens

- Simples.
- Não exige ferramentas adicionais.

### Desvantagens

- Processo sujeito a erros.
- Diferenças entre ambientes.
- Baixa escalabilidade.
- Difícil automação.

---

## Alternativa 2 — Máquinas Virtuais

### Vantagens

- Isolamento completo.
- Ambiente previsível.

### Desvantagens

- Alto consumo de recursos.
- Inicialização lenta.
- Custos maiores.
- Gerenciamento complexo.

---

## Alternativa 3 — Docker (Escolhida)

### Vantagens

- Ambiente padronizado.
- Baixo consumo de recursos.
- Inicialização rápida.
- Fácil integração com CI/CD.
- Portabilidade.
- Escalabilidade.

### Desvantagens

- Curva de aprendizado.
- Necessidade de gerenciamento de imagens.

---

# Decisão

Foi adotado o **Docker** como padrão oficial para empacotamento e execução das aplicações do Agilium Manager.

Todas as aplicações executáveis deverão possuir um **Dockerfile** próprio.

Quando necessário executar múltiplos serviços simultaneamente, deverá ser utilizado **Docker Compose**.

---

# Objetivos

Esta decisão possui os seguintes objetivos:

- Padronizar ambientes.
- Facilitar deploy.
- Automatizar publicações.
- Simplificar configuração.
- Melhorar portabilidade.
- Integrar com pipelines de CI/CD.

---

# Arquitetura

```text
Código Fonte

↓

Build

↓

Docker Image

↓

Container

↓

Ambiente

↓

Produção
```

---

# Estrutura Recomendada

```text
src/

├── Agilium.Manager.Web/

│   ├── Dockerfile

│

├── Agilium.Manager.Api/

│   ├── Dockerfile

│

docker/

├── docker-compose.yml

├── docker-compose.dev.yml

├── docker-compose.prod.yml

└── .env
```

---

# Dockerfile

Cada aplicação deverá possuir seu próprio Dockerfile.

Exemplo de fluxo:

```text
Restore

↓

Build

↓

Publish

↓

Runtime Image
```

Sempre utilizar **Multi-Stage Build**.

---

# Multi-Stage Build

A construção da imagem deverá separar:

- Ambiente de Build
- Ambiente de Runtime

Objetivos:

- Reduzir tamanho da imagem.
- Melhorar segurança.
- Reduzir tempo de download.
- Eliminar dependências desnecessárias.

---

# Imagens Base

As aplicações ASP.NET Core deverão utilizar imagens oficiais da Microsoft.

Exemplo:

```text
mcr.microsoft.com/dotnet/sdk

↓

Build

↓

mcr.microsoft.com/dotnet/aspnet

↓

Runtime
```

Não utilizar imagens de terceiros sem aprovação da equipe de arquitetura.

---

# Docker Compose

O Docker Compose deverá ser utilizado para orquestrar ambientes locais.

Exemplo:

```text
Application

↓

Database

↓

Redis

↓

RabbitMQ

↓

Outros Serviços
```

Cada serviço deverá possuir:

- Nome.
- Porta.
- Variáveis de ambiente.
- Volumes quando necessários.

---

# Configuração

Toda configuração deverá ser realizada através de:

- Variáveis de ambiente;
- Arquivos `.env`;
- Configuração externa.

Não é permitido armazenar configurações específicas do ambiente dentro do código-fonte.

---

# Secrets

Credenciais sensíveis nunca deverão ser armazenadas:

- No Dockerfile;
- No docker-compose;
- No código-fonte;
- No repositório Git.

Utilizar:

- Variáveis de ambiente;
- Secret Managers;
- Ferramentas do provedor de hospedagem.

---

# Persistência

Os containers devem ser considerados **estateless**.

Dados persistentes deverão utilizar:

- Banco de Dados;
- Volumes Docker;
- Serviços externos.

Nunca armazenar dados permanentes dentro do container.

---

# Logs

Os containers deverão escrever logs utilizando **stdout** e **stderr**.

A coleta de logs deverá ser realizada pela infraestrutura.

Não criar arquivos locais de log dentro do container.

---

# Health Check

Toda aplicação deverá disponibilizar endpoint de verificação de saúde.

Exemplo:

```text
/health
```

Esse endpoint deverá ser utilizado pelos orquestradores para validar disponibilidade.

---

# Atualizações

O processo de atualização deverá seguir:

```text
Nova Imagem

↓

Deploy

↓

Validação

↓

Remoção da versão anterior
```

Sempre que possível utilizar estratégia de **Rolling Update** ou **Blue-Green Deployment**.

---

# Segurança

Os containers deverão seguir as seguintes diretrizes:

- Executar com usuário não privilegiado.
- Minimizar permissões.
- Utilizar imagens oficiais.
- Atualizar imagens periodicamente.
- Não expor portas desnecessárias.
- Não instalar ferramentas desnecessárias.

---

# Integração Contínua

Os pipelines deverão:

1. Restaurar dependências.
2. Executar testes.
3. Gerar Build.
4. Criar imagem Docker.
5. Publicar imagem.
6. Executar Deploy.

---

# Benefícios

- Ambientes padronizados.
- Deploy reproduzível.
- Escalabilidade.
- Melhor integração com CI/CD.
- Portabilidade.
- Redução de erros de configuração.
- Facilidade para onboarding de novos desenvolvedores.

---

# Desvantagens

- Necessidade de conhecimento em Docker.
- Gerenciamento de imagens.
- Maior complexidade inicial.

---

# Riscos

Caso esta estratégia não seja seguida:

- Diferenças entre ambientes.
- Deploys inconsistentes.
- Configurações divergentes.
- Dificuldade para reproduzir erros.
- Maior custo operacional.

---

# Impacto

Esta decisão impacta:

- APIs
- MVC
- Serviços
- Integrações
- Infraestrutura
- CI/CD
- DevOps
- Produção
- Homologação
- Desenvolvimento

---

# Plano de Implementação

1. Criar Dockerfile para todas as aplicações.
2. Padronizar Multi-Stage Build.
3. Criar Docker Compose para ambiente local.
4. Externalizar configurações utilizando variáveis de ambiente.
5. Configurar Health Checks.
6. Integrar criação de imagens ao pipeline de CI/CD.
7. Atualizar documentação operacional.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Todas as aplicações possuem Dockerfile.
- As imagens utilizam Multi-Stage Build.
- Configurações são fornecidas por variáveis de ambiente.
- Nenhum segredo está armazenado na imagem ou no repositório.
- A aplicação disponibiliza endpoint `/health`.
- Os containers são stateless.
- O ambiente local pode ser iniciado através do Docker Compose.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0004 — Entity Framework Core
- ADR-0005 — Estratégia de Autenticação
- ADR-0009 — Estratégia de Dependency Injection
- ADR-0010 — Dapper para Consultas de Alta Performance
- ADR-0013 — Estratégia de Logging
- ADR-0018 — Gerenciamento de Configurações

---

# Referências

- Docker Documentation
- Microsoft — Containerize a .NET Application
- Microsoft — Docker Support for ASP.NET Core
- Twelve-Factor App
- OCI (Open Container Initiative) Specifications

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | 2026-07-29 | Criação da ADR definindo o Docker como padrão oficial de empacotamento e implantação do Agilium Manager, estabelecendo diretrizes para Dockerfiles, Docker Compose, Multi-Stage Build, configuração por variáveis de ambiente, segurança e integração com pipelines de CI/CD. |