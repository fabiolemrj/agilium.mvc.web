# Troubleshooting

## Objetivo

Este documento reúne os problemas mais comuns encontrados durante o desenvolvimento do **Agilium Manager**, suas possíveis causas e as soluções recomendadas.

Seu objetivo é reduzir o tempo gasto com diagnóstico de problemas recorrentes e fornecer um ponto de consulta rápida para desenvolvedores e agentes de IA.

A documentação oficial encontra-se em:

```text
docs/
```

Sempre consulte a documentação oficial antes de concluir que um comportamento é um erro.

---

# Como Utilizar

Sempre que encontrar um problema:

```text
Identificar o sintoma

↓

Localizar a categoria

↓

Verificar possíveis causas

↓

Aplicar solução

↓

Executar testes

↓

Atualizar documentação (quando necessário)
```

---

# Categorias

Os problemas estão agrupados em:

- Build
- Compilação
- Banco de Dados
- Entity Framework
- APIs
- Autenticação
- Autorização
- Docker
- Configuração
- Performance
- Testes
- Documentação
- Arquitetura

---

# Build

## Projeto não compila

Possíveis causas:

- Dependências ausentes.
- Pacotes desatualizados.
- Erros de referência.
- Alterações incompatíveis.

Verificar:

- Restore de pacotes.
- Arquivos `.csproj`.
- Dependências do projeto.
- SDK do .NET.

---

## Conflitos de Merge

Verificar:

- Arquivos alterados.
- Dependências.
- Migrations.
- Documentação.

Nunca resolver conflitos sem compreender o impacto.

---

# Banco de Dados

## Migration não funciona

Verificar:

- DbContext correto.
- Connection String.
- Projeto inicial.
- Ordem das migrations.

Consultar:

```text
knowledge/database.md
```

---

## Erro de relacionamento

Possíveis causas:

- Fluent API incorreta.
- Navegações inconsistentes.
- Chaves estrangeiras ausentes.

Consultar:

```text
docs/database/
```

---

## Dados inconsistentes

Verificar:

- Regras de negócio.
- Constraints.
- Soft Delete.
- Auditoria.
- Transações.

---

# Entity Framework Core

## Entidade não é persistida

Verificar:

- Mapping.
- Chave primária.
- Estado da entidade.
- SaveChanges.
- Rastreamento (Tracking).

---

## Include não funciona

Verificar:

- Relacionamento.
- Lazy Loading.
- Navegação.
- Configuração do Mapping.

---

## Migration gera alterações inesperadas

Verificar:

- Fluent API.
- Convenções.
- Tipos de dados.
- Alterações anteriores.

---

# APIs

## Endpoint retorna erro

Verificar:

- DTO.
- Model Validation.
- Regras de negócio.
- Middleware.
- Permissões.

Consultar:

```text
knowledge/api.md
```

---

## Erro 401

Verificar:

- Token.
- Autenticação.
- Expiração.
- Configuração JWT.

---

## Erro 403

Verificar:

- Permissões.
- Claims.
- Roles.
- Políticas.

---

## Erro 404

Verificar:

- Rota.
- Versionamento.
- Controller.
- Endpoint.

---

## Erro 500

Verificar:

- Logs.
- Exceções.
- Regras de negócio.
- Configuração.

---

# Domínio

## Regra de negócio não executa

Verificar:

- Camada correta.
- Domain Service.
- Specification.
- Notification Pattern.

Consultar:

```text
knowledge/domain.md
```

---

## Regra implementada no local errado

Lembre-se:

Nunca implementar regras de negócio em:

- Controller
- Repository
- DbContext
- Infrastructure

---

# Arquitetura

## Dependência entre camadas

Verificar se a implementação respeita:

```text
Presentation

↓

Application

↓

Domain

↓

Repository

↓

Persistence
```

Consulte:

```text
knowledge/architecture.md
```

---

## Código duplicado

Antes de implementar:

- Procurar implementação semelhante.
- Reutilizar componentes existentes.
- Avaliar criação de abstrações.

---

# Docker

## Container não inicia

Verificar:

- Dockerfile.
- docker-compose.
- Variáveis de ambiente.
- Portas.
- Logs.

---

## Banco não conecta

Verificar:

- Host.
- Porta.
- Usuário.
- Senha.
- Rede Docker.

---

# Configuração

## Configuração não é carregada

Verificar:

- appsettings.
- Variáveis de ambiente.
- Ordem de carregamento.
- Profiles.

---

## Dependency Injection

Erro:

```text
Unable to resolve service...
```

Verificar:

- Registro do serviço.
- Lifetime.
- Interface.
- Implementação.

---

# Testes

## Testes falham

Verificar:

- Alterações recentes.
- Dados de teste.
- Dependências.
- Mocks.
- Fixtures.

---

## Cobertura baixa

Avaliar:

- Casos críticos.
- Regras de negócio.
- Fluxos principais.
- Integrações.

---

# Performance

## Consulta lenta

Verificar:

- Índices.
- Includes.
- Paginação.
- Projeções.
- Uso de Dapper.

---

## Consumo elevado de memória

Verificar:

- Objetos grandes.
- Tracking do EF.
- Cache.
- Coleções carregadas.

---

# Documentação

## Documentação desatualizada

Após qualquer alteração relevante verificar:

- APIs.
- Regras de negócio.
- Diagramas.
- Fluxos.
- Templates.
- ADRs.

Código e documentação devem permanecer sincronizados.

---

# Problemas Arquiteturais

Verifique sempre:

- Existe ADR relacionada?
- A arquitetura está sendo respeitada?
- Existe padrão equivalente?
- Existe documentação oficial?

Consulte:

```text
knowledge/decisions.md
```

---

# Checklist de Diagnóstico

Antes de corrigir qualquer problema:

- Reproduziu o erro?
- Consultou os logs?
- Consultou a documentação?
- Consultou os ADRs?
- Avaliou impacto?
- Existe teste cobrindo o cenário?
- A solução respeita os padrões do projeto?

---

# Documentação Relacionada

| Assunto | Documento |
|----------|-----------|
| Arquitetura | knowledge/architecture.md |
| APIs | knowledge/api.md |
| Banco de Dados | knowledge/database.md |
| Domínio | knowledge/domain.md |
| Desenvolvimento | knowledge/development.md |
| Padrões | knowledge/patterns.md |
| Regras de Negócio | knowledge/business-rules.md |
| Decisões Arquiteturais | knowledge/decisions.md |

---

# Documentação Oficial

Consulte a documentação oficial correspondente ao problema em:

```text
docs/
```

A documentação detalha:

- Arquitetura
- Banco de Dados
- APIs
- Regras de Negócio
- Fluxos
- ADRs
- Desenvolvimento
- Infraestrutura

---

# Fluxo Recomendado para Agentes de IA

```text
Identificar o erro

↓

Classificar a categoria

↓

Consultar troubleshooting.md

↓

Consultar documentação relacionada

↓

Consultar ADRs

↓

Analisar impacto

↓

Implementar correção

↓

Executar testes

↓

Atualizar documentação
```

---

# Resumo

Este documento reúne os problemas mais frequentes encontrados durante o desenvolvimento do Agilium Manager e fornece um ponto de partida para diagnóstico e resolução.

Antes de aplicar qualquer correção:

- identifique corretamente o sintoma;
- consulte a documentação oficial da área afetada;
- valide as regras de negócio e os ADRs relacionados;
- confirme a solução por meio de testes;
- mantenha a documentação sincronizada com a implementação.