# Templates

## Objetivo

Este documento apresenta uma visão geral dos **templates oficiais** utilizados no **Agilium Manager** para padronizar documentação, código e processos.

Os templates garantem consistência entre módulos, facilitam a manutenção da documentação e auxiliam desenvolvedores e agentes de IA na criação de novos artefatos seguindo os padrões definidos pelo projeto.

A documentação oficial encontra-se em:

```text
docs/templates/
```

Este documento funciona como um índice para localizar rapidamente o template apropriado para cada situação.

---

# Visão Geral

Os templates padronizam a criação de:

- Documentação técnica
- Documentação de módulos
- ADRs
- APIs
- Diagramas
- Regras de negócio
- Entidades
- Casos de uso
- Pull Requests
- Issues
- Prompts

Todos os novos documentos devem ser criados utilizando os templates oficiais.

---

# Organização

A documentação oficial normalmente encontra-se organizada em:

```text
docs/templates/

README.md

adr.md

api.md

business-module.md

business-rule.md

class.md

database.md

diagram.md

entity.md

feature.md

issue.md

pull-request.md

prompt.md

service.md

system-mechanism-discovery.md

test.md
```

Cada template define uma estrutura padrão para um tipo específico de documento ou artefato.

---

# Templates Disponíveis

## ADR

Utilizado para registrar decisões arquiteturais.

Conteúdo típico:

- Objetivo
- Contexto
- Problema
- Decisão
- Consequências
- Alternativas
- Referências

Documentação:

```text
docs/templates/adr.md
```

---

## API

Utilizado para documentar endpoints REST.

Conteúdo típico:

- Objetivo
- Endpoint
- Método HTTP
- Autenticação
- Request
- Response
- Validações
- Erros
- Exemplos

Documentação:

```text
docs/templates/api.md
```

---

## Módulo de Negócio

Utilizado para documentar um módulo funcional.

Conteúdo típico:

- Objetivo
- Responsabilidades
- Fluxos
- Dependências
- Regras
- APIs
- Entidades
- ADRs

Documentação:

```text
docs/templates/business-module.md
```

---

## Regra de Negócio

Utilizado para documentar uma regra funcional.

Conteúdo típico:

- Objetivo
- Escopo
- Pré-condições
- Fluxo principal
- Validações
- Exceções
- Pós-condições

Documentação:

```text
docs/templates/business-rule.md
```

---

## Entidade

Utilizado para documentar entidades do domínio.

Conteúdo típico:

- Descrição
- Responsabilidades
- Propriedades
- Relacionamentos
- Regras
- Eventos
- Agregado

Documentação:

```text
docs/templates/entity.md
```

---

## Banco de Dados

Utilizado para documentar tabelas e estruturas de persistência.

Conteúdo típico:

- Entidade
- Campos
- Relacionamentos
- Índices
- Constraints
- Auditoria

Documentação:

```text
docs/templates/database.md
```

---

## Diagrama

Utilizado para documentar diagramas arquiteturais e funcionais.

Conteúdo típico:

- Objetivo
- Tipo de diagrama
- Descrição
- Componentes
- Fluxo
- Referências

Documentação:

```text
docs/templates/diagram.md
```

---

## Serviço

Utilizado para documentar serviços de domínio ou aplicação.

Conteúdo típico:

- Objetivo
- Responsabilidades
- Dependências
- Fluxo
- Entradas
- Saídas
- Regras

Documentação:

```text
docs/templates/service.md
```

---

## Testes

Utilizado para documentar estratégias e casos de teste.

Conteúdo típico:

- Objetivo
- Cenário
- Pré-condições
- Passos
- Resultado esperado

Documentação:

```text
docs/templates/test.md
```

---

## Prompt

Utilizado para padronizar prompts destinados a agentes de IA.

Conteúdo típico:

- Objetivo
- Contexto
- Escopo
- Restrições
- Critérios de aceitação
- Resultado esperado

Documentação:

```text
docs/templates/prompt.md
```

---

## System Mechanism Discovery

Utilizado para documentar mecanismos transversais e funcionalidades internas descobertos via análise de código. Diferente do template de módulo (focado em negócio), este template foca em **mecanismos internos**: sistemas de ajuda, notificações, logging, caching, validação cross-cutting, etc.

Conteúdo típico:

- Arquitetura em camadas
- Componentes envolvidos
- Fluxo de execução
- Distribuição no código (estatísticas)
- API / Contrato de uso
- Problemas conhecidos
- Checklist de extensão
- Referência rápida para agentes de IA

Documentação:

```text
docs/templates/system-mechanism-discovery.md
```

Prompt de discovery:

```text
docs/prompts/system-mechanism-discovery.md
```

---

## Issue

Utilizado para abertura de demandas.

Conteúdo típico:

- Descrição
- Contexto
- Objetivo
- Critérios de aceitação
- Dependências

Documentação:

```text
docs/templates/issue.md
```

---

## Pull Request

Utilizado para padronizar revisões de código.

Conteúdo típico:

- Objetivo
- Alterações realizadas
- Impactos
- Testes executados
- Checklist
- Documentação atualizada

Documentação:

```text
docs/templates/pull-request.md
```

---

# Quando Utilizar

| Situação | Template |
|----------|----------|
| Nova decisão arquitetural | ADR |
| Novo endpoint | API |
| Novo módulo | Business Module |
| Nova regra | Business Rule |
| Nova entidade | Entity |
| Alteração no banco | Database |
| Novo diagrama | Diagram |
| Novo serviço | Service |
| Novos testes | Test |
| Novo prompt | Prompt |
| Descoberta de mecanismo interno | System Mechanism Discovery |
| Nova demanda | Issue |
| Revisão de código | Pull Request |

---

# Boas Práticas

Ao utilizar um template:

- Não remover seções obrigatórias.
- Preencher todas as informações relevantes.
- Adaptar apenas quando necessário.
- Manter a estrutura padrão.
- Referenciar ADRs relacionadas.
- Atualizar links para documentação relacionada.

Os templates existem para garantir uniformidade em toda a documentação.

---

# Fluxo Recomendado

```text
Identificar o artefato

↓

Selecionar o template adequado

↓

Preencher as informações

↓

Validar consistência

↓

Publicar

↓

Atualizar documentação relacionada
```

---

# Integração com a Base de Conhecimento

Antes de utilizar um template, consulte:

- `knowledge/architecture.md`
- `knowledge/business-rules.md`
- `knowledge/database.md`
- `knowledge/domain.md`
- `knowledge/development.md`
- `knowledge/patterns.md`
- `knowledge/decisions.md`

Isso garante que o conteúdo criado esteja alinhado aos padrões da solução.

---

# ADRs Relacionadas

| Tema | ADR |
|------|-----|
| Arquitetura em Camadas | ADR-0001 |
| Repository Pattern | ADR-0002 |
| Notification Pattern | ADR-0003 |
| Estratégia de Validação | ADR-0007 |
| Dependency Injection | ADR-0009 |
| Service Layer | ADR-0011 |
| Logging | ADR-0013 |
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
| APIs | knowledge/api.md |
| Banco de Dados | knowledge/database.md |
| Domínio | knowledge/domain.md |
| Desenvolvimento | knowledge/development.md |
| Padrões | knowledge/patterns.md |
| Regras de Negócio | knowledge/business-rules.md |
| Prompts | knowledge/prompts.md |
| Decisões Arquiteturais | knowledge/decisions.md |

---

# Documentação Oficial

Para detalhes completos consulte:

```text
docs/templates/
```

A documentação oficial contém os modelos completos utilizados pelo projeto, incluindo exemplos de preenchimento e orientações específicas para cada tipo de artefato.

---

# Fluxo Recomendado para Agentes de IA

```text
Ler templates.md

↓

Identificar o artefato a ser criado

↓

Selecionar o template correspondente

↓

Consultar documentação relacionada

↓

Preencher o template

↓

Validar consistência

↓

Atualizar documentação
```

---

# Resumo

Este documento apresenta a biblioteca de templates utilizada pelo Agilium Manager.

Antes de criar qualquer documento, módulo ou artefato:

- selecione o template oficial correspondente;
- mantenha a estrutura padronizada;
- consulte a documentação relacionada e os ADRs aplicáveis;
- garanta consistência entre documentação, código e arquitetura.