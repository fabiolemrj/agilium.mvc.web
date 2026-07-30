# Development

## Objetivo

Este documento apresenta uma visão geral do processo de desenvolvimento do **Agilium Manager**, consolidando os padrões, convenções e práticas adotadas pela equipe.

A documentação oficial encontra-se em:

```text
docs/development/
```

Este documento serve como um guia rápido para desenvolvedores e agentes de IA, indicando como novas funcionalidades devem ser planejadas, implementadas, testadas e documentadas.

---

# Filosofia de Desenvolvimento

Todo desenvolvimento deve priorizar:

- Simplicidade
- Legibilidade
- Manutenibilidade
- Reutilização
- Testabilidade
- Baixo acoplamento
- Alta coesão

A implementação deve seguir os princípios definidos pela arquitetura da solução e pelas ADRs do projeto.

---

# Fluxo de Desenvolvimento

Toda nova funcionalidade deve seguir o fluxo abaixo:

```text
Receber Requisito

↓

Analisar Contexto

↓

Consultar Documentação

↓

Consultar ADRs

↓

Planejar Implementação

↓

Implementar

↓

Criar Testes

↓

Executar Testes

↓

Atualizar Documentação

↓

Code Review

↓

Merge
```

---

# Antes de Implementar

Antes de escrever código, verifique:

- Existe documentação do módulo?
- Existem regras de negócio documentadas?
- Existe ADR relacionada?
- Existe implementação semelhante?
- O impacto em outros módulos foi avaliado?
- A alteração exige atualização da documentação?

---

# Organização do Código

Cada componente deve possuir uma responsabilidade clara.

Exemplo:

```text
Controller

↓

Application Service

↓

Domain

↓

Repository

↓

Persistence
```

Evite criar dependências entre camadas que não estejam previstas na arquitetura.

---

# Convenções

Todo código deve seguir as convenções estabelecidas pelo projeto.

Exemplos:

- Nomes claros e descritivos.
- Métodos pequenos.
- Classes com responsabilidade única.
- Dependências injetadas.
- Código reutilizável.
- Baixo acoplamento.

Consulte:

```text
docs/development/
```

---

# Estrutura da Solution

Resumo da organização do projeto.

```text
src/

MVC/

API/

Application/

Domain/

Repository/

Persistence/

Infrastructure/

tests/

docs/

.ai/
```

A estrutura detalhada encontra-se na documentação oficial.

---

# Padrões Utilizados

O projeto adota os seguintes padrões:

- Layered Architecture
- Repository Pattern
- Service Layer
- Dependency Injection
- Notification Pattern
- Options Pattern
- Soft Delete
- Auditoria Automática

Consulte:

```text
knowledge/patterns.md
```

---

# Regras Gerais

Durante o desenvolvimento:

- Não implementar regras de negócio em Controllers.
- Não acessar banco diretamente.
- Não duplicar lógica.
- Utilizar DTOs para comunicação.
- Utilizar Dependency Injection.
- Respeitar os limites entre camadas.

---

# Implementação de Funcionalidades

Toda funcionalidade deve seguir a sequência:

```text
Requisito

↓

Application Service

↓

Domain

↓

Repository

↓

Persistence

↓

Testes

↓

Documentação
```

---

# Tratamento de Erros

Utilizar:

- Notification Pattern
- Middleware Global
- Responses padronizadas
- Logs estruturados

Evitar:

- try/catch desnecessários.
- Exceptions para regras de negócio.
- Tratamentos duplicados.

---

# Banco de Dados

Ao alterar persistência:

- Atualizar entidades.
- Atualizar mappings.
- Criar Migration.
- Executar testes.
- Atualizar documentação.

Consulte:

```text
knowledge/database.md
```

---

# APIs

Ao criar endpoints:

- Criar DTOs.
- Validar entrada.
- Utilizar Services.
- Respeitar versionamento.
- Retornar respostas padronizadas.

Consulte:

```text
knowledge/api.md
```

---

# Regras de Negócio

Antes de implementar qualquer regra:

- Consultar documentação do módulo.
- Validar impactos.
- Identificar ADRs relacionadas.

Consulte:

```text
knowledge/business-rules.md
```

---

# Testes

Toda implementação deve possuir testes compatíveis com sua complexidade.

Prioridade:

```text
Testes Unitários

↓

Testes de Integração

↓

Testes End-to-End
```

Consulte:

```text
docs/testing/
```

---

# Documentação

Toda alteração relevante deve atualizar:

- Documentação técnica.
- Regras de negócio.
- Diagramas (quando necessário).
- ADRs (quando aplicável).

Nunca deixe código e documentação divergentes.

---

# Code Review

Antes do merge, verificar:

- Código segue os padrões?
- Regras de negócio corretas?
- Testes executados?
- Documentação atualizada?
- Sem duplicação de código?
- Sem dependências indevidas?
- ADRs respeitadas?

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
| Regras de Negócio | knowledge/business-rules.md |
| Padrões | knowledge/patterns.md |
| Decisões Arquiteturais | knowledge/decisions.md |

---

# Documentação Oficial

Para informações detalhadas consulte:

```text
docs/development/
```

A documentação oficial contém:

- Convenções de código
- Estrutura dos projetos
- Padrões de desenvolvimento
- Processo de contribuição
- Boas práticas
- Guias de implementação

---

# Fluxo Recomendado para Agentes de IA

```text
Ler development.md

↓

Consultar architecture.md

↓

Consultar decisions.md

↓

Identificar módulo

↓

Consultar documentação oficial

↓

Planejar implementação

↓

Implementar

↓

Criar testes

↓

Atualizar documentação
```

---

# Resumo

Este documento apresenta uma visão geral do processo de desenvolvimento do Agilium Manager.

Antes de iniciar qualquer implementação:

- compreenda o contexto da funcionalidade;
- consulte a documentação oficial;
- siga os padrões definidos pelo projeto;
- respeite as ADRs existentes;
- mantenha código, testes e documentação sempre sincronizados.