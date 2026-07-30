# Deployment

## Objetivo

Este documento apresenta uma visão geral do processo de **deployment** do **Agilium Manager**, descrevendo como as aplicações são construídas, configuradas, publicadas e disponibilizadas nos diferentes ambientes.

Seu objetivo é orientar desenvolvedores e agentes de IA sobre o fluxo de entrega da solução, garantindo implantações consistentes, reproduzíveis e seguras.

A documentação oficial encontra-se em:

```text
docs/deployment/
```

Este documento é um resumo para navegação. A documentação oficial permanece como a fonte de verdade.

---

# Visão Geral

O processo de deployment compreende todas as etapas necessárias para disponibilizar uma nova versão da aplicação.

Fluxo geral:

```text
Desenvolvimento

↓

Build

↓

Testes

↓

Empacotamento

↓

Publicação

↓

Deploy

↓

Validação

↓

Monitoramento
```

Cada etapa deve garantir que a aplicação permaneça íntegra e funcional.

---

# Ambientes

A solução normalmente é distribuída entre diferentes ambientes.

```text
Development

↓

Testing

↓

Homologation

↓

Production
```

Cada ambiente possui configurações específicas.

---

# Organização da Documentação

A documentação oficial normalmente encontra-se organizada em:

```text
docs/deployment/

README.md

environments.md
build.md
docker.md
configuration.md
pipelines.md
ci-cd.md
release.md
rollback.md
monitoring.md
checklist.md
```

---

# Build

O processo de build deve:

- Restaurar dependências.
- Compilar todos os projetos.
- Executar testes.
- Gerar artefatos.
- Validar erros de compilação.

Fluxo:

```text
Restore

↓

Build

↓

Tests

↓

Publish

↓

Artifacts
```

---

# Configuração

Toda configuração deve ser externa ao código.

Exemplos:

- Connection Strings
- Chaves de API
- JWT Secrets
- Configurações SMTP
- URLs externas
- Feature Flags

Nunca armazenar segredos diretamente no código-fonte.

---

# Variáveis de Ambiente

As configurações devem utilizar variáveis de ambiente sempre que possível.

Exemplos:

```text
ASPNETCORE_ENVIRONMENT

ConnectionStrings__DefaultConnection

Jwt__Secret

Logging__Level
```

Cada ambiente deve possuir sua própria configuração.

---

# Docker

Quando a solução utilizar containers:

Fluxo típico:

```text
Build Image

↓

Push Registry

↓

Deploy Container

↓

Health Check

↓

Application Ready
```

A documentação oficial deve detalhar:

- Dockerfile
- docker-compose
- Redes
- Volumes
- Variáveis
- Health Checks

---

# CI/CD

O pipeline de integração contínua normalmente executa:

```text
Restore

↓

Build

↓

Static Analysis

↓

Tests

↓

Publish

↓

Deploy
```

Os pipelines devem ser automatizados sempre que possível.

---

# Release

Antes de publicar uma nova versão:

- Validar requisitos.
- Executar testes.
- Atualizar documentação.
- Revisar Migrations.
- Atualizar versão.
- Validar artefatos.

---

# Banco de Dados

Caso existam alterações estruturais:

Fluxo recomendado:

```text
Backup

↓

Migration

↓

Validação

↓

Deploy

↓

Smoke Tests
```

Nunca aplicar alterações estruturais sem validação prévia.

Consulte:

```text
knowledge/database.md
```

---

# Rollback

Toda estratégia de deployment deve prever rollback.

Situações comuns:

- Falha na aplicação.
- Migration incompatível.
- Problemas de infraestrutura.
- Erros críticos.

O plano de rollback deve estar documentado.

---

# Monitoramento

Após o deployment verificar:

- Aplicação iniciada.
- Logs.
- Uso de CPU.
- Uso de memória.
- Conectividade.
- Banco de dados.
- APIs externas.
- Filas.
- Jobs.

---

# Smoke Tests

Após cada publicação executar testes mínimos:

- Login.
- Consulta principal.
- Cadastro.
- APIs.
- Banco de dados.
- Serviços externos.

O objetivo é confirmar que a aplicação está operacional.

---

# Segurança

Durante o deployment garantir:

- HTTPS habilitado.
- Certificados válidos.
- Variáveis protegidas.
- Secrets armazenados com segurança.
- Logs sem informações sensíveis.

Nunca expor credenciais em arquivos versionados.

---

# Checklist de Deployment

Antes do deploy:

- Build executado.
- Testes aprovados.
- Documentação atualizada.
- Migrations revisadas.
- Configurações validadas.
- Backup realizado (quando necessário).
- Plano de rollback disponível.

Após o deploy:

- Smoke Tests executados.
- Logs analisados.
- Monitoramento ativo.
- Ambiente validado.

---

# ADRs Relacionadas

| Tema | ADR |
|------|-----|
| Arquitetura em Camadas | ADR-0001 |
| Configuration Management | ADR-0018 |
| Database Migrations | ADR-0019 |
| Estratégia de Testes | ADR-0020 |

Consulte:

```text
knowledge/decisions.md
```

---

# Documentação Relacionada

| Assunto | Documento |
|----------|-----------|
| Arquitetura | knowledge/architecture.md |
| Banco de Dados | knowledge/database.md |
| APIs | knowledge/api.md |
| Desenvolvimento | knowledge/development.md |
| Padrões | knowledge/patterns.md |
| Fluxos | knowledge/fluxos.md |
| Contribuição | knowledge/contribuicao.md |
| Decisões Arquiteturais | knowledge/decisions.md |

---

# Documentação Oficial

Consulte sempre:

```text
docs/deployment/
```

A documentação oficial contém:

- Ambientes
- Processo de Build
- Docker
- Configuração
- Pipelines
- CI/CD
- Releases
- Rollback
- Monitoramento
- Checklists

---

# Fluxo Recomendado para Agentes de IA

```text
Ler deployment.md

↓

Identificar o ambiente de destino

↓

Consultar documentação oficial

↓

Validar configurações

↓

Executar Build

↓

Executar Testes

↓

Publicar artefatos

↓

Executar Deployment

↓

Executar Smoke Tests

↓

Validar Monitoramento
```

---

# Resumo

Este documento apresenta uma visão geral do processo de deployment do Agilium Manager.

Antes de realizar qualquer implantação:

- valide o ambiente de destino;
- confirme que o build e os testes foram executados com sucesso;
- preserve a integridade das configurações e dos segredos;
- execute smoke tests após a publicação;
- mantenha documentação, pipelines e estratégias de rollback sempre atualizadas.