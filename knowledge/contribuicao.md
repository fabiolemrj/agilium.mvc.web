# Contribuição

## Objetivo

Este documento apresenta uma visão geral do processo de **contribuição** para o **Agilium Manager**, definindo as práticas, responsabilidades e etapas para garantir que toda alteração mantenha a qualidade, consistência e evolução sustentável da solução.

A documentação oficial encontra-se em:

```text
docs/contribuicao/
```

Este documento serve como um guia rápido para desenvolvedores e agentes de IA sobre como contribuir corretamente com o projeto.

---

# Visão Geral

Toda contribuição deve:

- Respeitar a arquitetura da solução.
- Seguir os padrões de desenvolvimento.
- Preservar as regras de negócio.
- Manter compatibilidade com implementações existentes.
- Atualizar documentação quando necessário.
- Possuir testes compatíveis com a alteração.

Contribuir significa melhorar o projeto sem comprometer sua consistência.

---

# Fluxo de Contribuição

Todo desenvolvimento deve seguir o fluxo abaixo.

```text
Receber Demanda

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

Criar ou Atualizar Testes

↓

Atualizar Documentação

↓

Code Review

↓

Merge
```

---

# Antes de Contribuir

Antes de iniciar qualquer alteração, verifique:

- O objetivo da demanda está claro?
- Existe documentação para o módulo?
- Existem regras de negócio relacionadas?
- Há ADRs aplicáveis?
- Existe implementação semelhante?
- A alteração impacta outros módulos?

---

# Tipos de Contribuição

São consideradas contribuições válidas:

- Novas funcionalidades
- Correção de bugs
- Refatorações
- Melhorias de desempenho
- Atualização de documentação
- Criação de testes
- Melhorias arquiteturais
- Atualização de diagramas
- Criação de ADRs

---

# Boas Práticas

Ao contribuir:

- Faça alterações pequenas e objetivas.
- Evite mudanças não relacionadas à demanda.
- Reutilize código existente.
- Respeite os padrões arquiteturais.
- Documente decisões relevantes.
- Mantenha o histórico do projeto consistente.

---

# Desenvolvimento

Toda implementação deve respeitar:

- Arquitetura em Camadas
- Repository Pattern
- Service Layer
- Notification Pattern
- Dependency Injection
- Padrões de nomenclatura
- Convenções do projeto

Consulte:

```text
knowledge/development.md
```

e

```text
knowledge/patterns.md
```

---

# Regras de Negócio

Nenhuma contribuição deve alterar regras de negócio sem análise prévia.

Antes de modificar qualquer comportamento funcional:

- Consulte a documentação do módulo.
- Verifique impactos em outros processos.
- Atualize a documentação correspondente.

Consulte:

```text
knowledge/business-rules.md
```

---

# Arquitetura

Toda alteração deve preservar a arquitetura da solução.

Consulte:

```text
knowledge/architecture.md
```

---

# Banco de Dados

Ao alterar persistência:

- Atualize entidades.
- Atualize mappings.
- Gere uma Migration.
- Execute testes.
- Atualize documentação.

Consulte:

```text
knowledge/database.md
```

---

# APIs

Ao alterar ou criar endpoints:

- Utilize DTOs.
- Preserve compatibilidade quando possível.
- Respeite o versionamento.
- Atualize a documentação da API.

Consulte:

```text
knowledge/api.md
```

---

# Documentação

Toda alteração significativa deve refletir na documentação.

Atualize quando necessário:

- Documentação técnica
- Regras de negócio
- APIs
- Diagramas
- Fluxos
- Templates
- ADRs

Código e documentação devem permanecer sincronizados.

---

# Testes

Toda contribuição deve ser validada por testes apropriados.

Prioridade:

```text
Testes Unitários

↓

Testes de Integração

↓

Testes End-to-End
```

Caso não seja possível automatizar um cenário, documente a estratégia de validação adotada.

---

# Code Review

Antes do merge, verificar:

- Código segue os padrões?
- Regras de negócio preservadas?
- Testes executados?
- Documentação atualizada?
- Sem duplicação de código?
- Sem dependências inadequadas?
- ADRs respeitadas?

---

# Pull Request

Toda Pull Request deve conter:

- Objetivo
- Descrição das alterações
- Motivação
- Impactos
- Evidências de testes
- Documentação atualizada
- Referência da Issue (quando existir)

Utilize o template oficial:

```text
docs/templates/pull-request.md
```

---

# Quando Criar uma ADR

Crie uma nova ADR quando a contribuição introduzir:

- Novo padrão arquitetural.
- Mudança permanente na arquitetura.
- Nova tecnologia principal.
- Nova estratégia de autenticação.
- Mudança significativa de infraestrutura.
- Alteração estrutural que afete múltiplos módulos.

Consulte:

```text
knowledge/decisions.md
```

---

# Checklist de Contribuição

Antes de concluir uma entrega, confirme:

- Requisito compreendido.
- Arquitetura respeitada.
- ADRs consultadas.
- Regras de negócio preservadas.
- Código revisado.
- Testes executados.
- Documentação atualizada.
- Sem código morto.
- Sem dependências desnecessárias.
- Sem quebra de compatibilidade não planejada.

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
| Fluxos | knowledge/fluxos.md |
| Padrões | knowledge/patterns.md |
| Regras de Negócio | knowledge/business-rules.md |
| Templates | knowledge/templates.md |
| Prompts | knowledge/prompts.md |
| Decisões Arquiteturais | knowledge/decisions.md |

---

# Documentação Oficial

Para informações detalhadas consulte:

```text
docs/contribuicao/
```

A documentação oficial contém:

- Processo de contribuição
- Fluxo de Git
- Convenções de branches
- Processo de revisão
- Checklist de qualidade
- Critérios para aprovação
- Guias de colaboração

---

# Fluxo Recomendado para Agentes de IA

```text
Ler contribuicao.md

↓

Compreender a demanda

↓

Consultar documentação relacionada

↓

Consultar ADRs

↓

Planejar implementação

↓

Implementar alterações

↓

Criar ou atualizar testes

↓

Atualizar documentação

↓

Preparar Pull Request
```

---

# Resumo

Este documento apresenta uma visão geral do processo de contribuição do Agilium Manager.

Antes de contribuir com qualquer alteração:

- compreenda o contexto da demanda;
- consulte a documentação oficial e os ADRs;
- siga os padrões arquiteturais e de desenvolvimento;
- valide a implementação com testes;
- mantenha código, documentação e arquitetura sempre alinhados.