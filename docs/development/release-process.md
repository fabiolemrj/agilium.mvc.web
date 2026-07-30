# Processo de Release

# Objetivo

Documentar o processo de preparação, validação, versionamento e publicação de novas versões do Agilium Manager.

Este documento estabelece as diretrizes para garantir que cada release seja reproduzível, rastreável e consistente em todos os ambientes.

---

# Escopo

Este documento contempla:

- Estratégia de Versionamento
- Planejamento de Releases
- Fluxo de Publicação
- Validação
- Implantação
- Rollback
- Boas Práticas
- Governança

---

# Índice

- Visão Geral
- Estratégia de Versionamento
- Planejamento da Release
- Fluxo de Release
- Validações Obrigatórias
- Publicação
- Implantação
- Rollback
- Comunicação
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

Uma release representa um conjunto controlado de alterações disponibilizadas para um ambiente da solução.

Todo processo de release deve garantir:

- rastreabilidade;
- estabilidade;
- repetibilidade;
- validação adequada;
- possibilidade de reversão.

---

# Estratégia de Versionamento

A estratégia oficial de versionamento deverá ser documentada após validação do processo utilizado pela equipe.

A documentação deverá definir:

- formato das versões;
- política para versões principais;
- versões de manutenção;
- correções emergenciais;
- identificação de builds.

---

# Planejamento da Release

Antes da publicação de uma nova versão recomenda-se verificar:

- funcionalidades concluídas;
- correções aprovadas;
- testes executados;
- documentação atualizada;
- dependências revisadas;
- impactos conhecidos.

---

# Fluxo de Release

Fluxo conceitual:

```text
Desenvolvimento

↓

Revisão Técnica

↓

Validação

↓

Build

↓

Testes

↓

Publicação

↓

Implantação

↓

Monitoramento
```

As etapas obrigatórias dependerão do processo adotado pela equipe.

---

# Validações Obrigatórias

Antes da publicação recomenda-se validar:

- compilação da solução;
- execução dos testes disponíveis;
- revisão técnica;
- documentação atualizada;
- versão identificada corretamente;
- artefatos gerados.

---

# Publicação

A publicação deverá documentar:

- versão publicada;
- data;
- ambiente;
- responsável;
- artefatos gerados;
- alterações incluídas.

Caso sejam utilizados pipelines automatizados, estes deverão ser documentados na documentação de infraestrutura.

---

# Implantação

O processo de implantação deverá definir:

- ambientes disponíveis;
- ordem de implantação;
- validações pós-deploy;
- critérios para conclusão da implantação.

Os procedimentos específicos dependerão da infraestrutura utilizada.

---

# Rollback

Toda estratégia de release deve prever um procedimento de reversão.

O plano de rollback deverá informar:

- condições para acionamento;
- responsável pela decisão;
- procedimento de reversão;
- validações após o rollback;
- comunicação às partes interessadas.

Os detalhes da estratégia dependerão do ambiente de implantação.

---

# Comunicação

Cada release deve possuir um registro contendo, no mínimo:

- versão;
- data;
- alterações implementadas;
- correções;
- impactos conhecidos;
- instruções especiais (quando aplicável).

---

# Boas Práticas

Sempre:

- publicar apenas versões validadas;
- manter rastreabilidade das alterações;
- documentar mudanças relevantes;
- revisar impactos antes da implantação;
- possuir estratégia de rollback.

Evitar:

- implantações sem validação;
- alterações não documentadas;
- múltiplas mudanças críticas em uma única release sem planejamento;
- publicação manual sem controle.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de uma solução baseada em .NET;
- organização em múltiplos projetos.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-azure-api`;
- `agilium-manager-azure-business`;
- `agilium-manager-git-azure-infra`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- estratégia oficial de versionamento;
- fluxo de branches;
- processo de revisão e aprovação;
- ambientes de implantação;
- pipeline de CI/CD;
- estratégia de rollback;
- ferramenta de automação utilizada.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração da estratégia de versionamento;
- mudança no processo de release;
- alteração da infraestrutura de implantação;
- adoção de novas ferramentas de automação;
- atualização do fluxo de aprovação.

---

# Documentação Relacionada

## Desenvolvimento

- development/build.md
- development/code-review.md
- development/versioning.md

## Arquitetura

- architecture/deployment.md

## Infraestrutura

- infrastructure/ci-cd.md
- infrastructure/deployment.md
- infrastructure/environments.md