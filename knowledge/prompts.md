# Prompts

## Objetivo

Este documento apresenta uma visão geral da biblioteca de **prompts padronizados** utilizada pelo **Agilium Manager**.

Os prompts têm como objetivo padronizar a interação com agentes de IA (GitHub Copilot, ChatGPT, Claude, Cursor, etc.), garantindo respostas consistentes, aderentes à arquitetura do projeto e alinhadas às regras de negócio.

A documentação oficial encontra-se em:

```text
docs/prompts/
```

Este documento serve como um índice para localizar rapidamente o prompt adequado para cada atividade de desenvolvimento.

---

# Visão Geral

Os prompts são utilizados para auxiliar em atividades como:

- Levantamento de contexto
- Planejamento de funcionalidades
- Implementação
- Refatoração
- Correção de bugs
- Criação de documentação
- Revisão de código
- Geração de testes
- Migração de código
- Análise arquitetural

Todos os prompts devem considerar a documentação oficial, os ADRs e os padrões definidos pelo projeto.

---

# Organização

A documentação oficial normalmente encontra-se organizada em:

```text
docs/prompts/

README.md

analysis/

architecture/

development/

documentation/

testing/

migration/

review/

templates/
```

Cada diretório agrupa prompts voltados para uma etapa específica do ciclo de desenvolvimento.

---

# Categorias de Prompts

## Análise

Utilizados para compreender o contexto antes de qualquer alteração.

Exemplos:

- Levantamento arquitetural
- Levantamento técnico
- Levantamento funcional
- Mapeamento de dependências
- Identificação de impactos
- Discovery de mecanismos internos (system-mechanism-discovery)

Objetivo:

- Compreender a solução antes de modificar o código.

---

## Arquitetura

Auxiliam na análise e evolução da arquitetura.

Exemplos:

- Avaliação arquitetural
- Identificação de padrões
- Revisão de dependências
- Propostas de refatoração
- Avaliação de impacto

---

## Desenvolvimento

Utilizados para implementar novas funcionalidades.

Exemplos:

- Criar endpoint
- Criar módulo
- Criar serviço
- Criar entidade
- Implementar regra de negócio
- Criar integração

Todos os prompts devem respeitar:

- Arquitetura
- ADRs
- Regras de negócio
- Convenções do projeto

---

## Documentação

Auxiliam na criação e atualização da documentação.

Exemplos:

- Documentar módulo
- Criar ADR
- Atualizar documentação técnica
- Criar diagramas
- Atualizar README
- Documentar APIs

---

## Testes

Prompts voltados à qualidade do software.

Exemplos:

- Criar testes unitários
- Criar testes de integração
- Revisar cobertura
- Identificar cenários de teste

---

## Migração

Auxiliam em processos de modernização e evolução do sistema.

Exemplos:

- Migração de autenticação
- Atualização de framework
- Refatoração de arquitetura
- Migração de banco
- Migração de APIs

---

## Revisão

Prompts utilizados para validação de código.

Exemplos:

- Code Review
- Revisão arquitetural
- Revisão de segurança
- Revisão de performance
- Revisão de documentação

---

# Estrutura Recomendada de um Prompt

Todo prompt deve conter:

- Objetivo
- Contexto
- Escopo
- Restrições
- Requisitos
- Critérios de aceitação
- Resultado esperado

Exemplo:

```text
Objetivo

Contexto

Requisitos

Restrições

Arquivos envolvidos

Resultado esperado
```

---

# Boas Práticas

Ao utilizar prompts:

- Informe o contexto do projeto.
- Referencie a documentação oficial.
- Cite os ADRs aplicáveis.
- Delimite claramente o escopo.
- Informe as restrições técnicas.
- Evite solicitações genéricas.

Quanto mais contexto for fornecido, maior será a qualidade das respostas.

---

# Fluxo Recomendado

```text
Identificar a necessidade

↓

Selecionar o prompt adequado

↓

Consultar a documentação oficial

↓

Consultar ADRs

↓

Executar o prompt

↓

Validar o resultado

↓

Atualizar documentação
```

---

# Regras para Agentes de IA

Antes de gerar código, um agente deve:

- Compreender a arquitetura da solução.
- Consultar as regras de negócio.
- Respeitar os padrões definidos.
- Evitar duplicação de código.
- Não criar novas arquiteturas sem justificativa.
- Manter compatibilidade com o projeto existente.

---

# Integração com a Documentação

Os prompts devem sempre utilizar como referência:

- `knowledge/architecture.md`
- `knowledge/business-rules.md`
- `knowledge/database.md`
- `knowledge/domain.md`
- `knowledge/patterns.md`
- `knowledge/development.md`
- `knowledge/decisions.md`

Quando necessário, consultar a documentação oficial correspondente em `docs/`.

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
| Decisões Arquiteturais | knowledge/decisions.md |
| Templates | knowledge/templates.md |

---

# Documentação Oficial

Para informações detalhadas consulte:

```text
docs/prompts/
```

A documentação oficial contém:

- Biblioteca de prompts
- Modelos reutilizáveis
- Guias de utilização
- Exemplos práticos
- Convenções para engenharia de prompts

---

# Fluxo Recomendado para Agentes de IA

```text
Ler prompts.md

↓

Identificar a atividade

↓

Selecionar o prompt apropriado

↓

Consultar a documentação relacionada

↓

Executar o prompt

↓

Validar o resultado

↓

Atualizar documentação, se necessário
```

---

# Resumo

Este documento apresenta uma visão geral da biblioteca de prompts do Agilium Manager.

Antes de solicitar qualquer implementação ou análise:

- escolha o prompt mais adequado ao objetivo;
- forneça contexto suficiente;
- referencie a documentação oficial e os ADRs relevantes;
- valide o resultado antes de incorporá-lo ao projeto;
- mantenha os prompts alinhados com a evolução da arquitetura e da documentação.