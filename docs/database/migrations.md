# Estratégia de Migrações

## Objetivo

Documentar a estratégia de evolução do modelo de dados utilizada pelo Agilium Manager, definindo as diretrizes para criação, revisão, aplicação e manutenção das migrações do banco de dados.

Este documento estabelece as práticas de governança relacionadas à evolução da estrutura de persistência da aplicação.

---

# Escopo

Este documento contempla:

- Estratégia de Migração
- Evolução do Modelo de Dados
- Convenções
- Processo de Revisão
- Aplicação das Migrações
- Compatibilidade
- Controle de Versão
- Rollback
- Boas Práticas

---

# Índice

- Visão Geral
- Estratégia de Migração
- Controle de Versão
- Convenções
- Processo de Criação
- Processo de Revisão
- Aplicação das Migrações
- Compatibilidade
- Rollback
- Boas Práticas
- Anti-Padrões
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

As migrações representam o mecanismo utilizado para controlar a evolução do modelo de dados ao longo do ciclo de vida da aplicação.

Toda alteração estrutural do banco deve possuir rastreabilidade, histórico e documentação.

---

# Estratégia de Migração

A estratégia de migração adotada pela solução deverá garantir:

- evolução controlada do esquema do banco;
- versionamento das alterações;
- rastreabilidade das mudanças;
- compatibilidade entre versões;
- possibilidade de implantação em diferentes ambientes.

A estratégia específica utilizada (por exemplo, Code First, Database First ou outra abordagem) deverá refletir a implementação existente e ser documentada somente após validação.

---

# Controle de Versão

Toda alteração estrutural deve estar vinculada ao controle de versão da aplicação.

Cada migração deve possuir:

- identificador único;
- descrição clara;
- data de criação;
- autor (quando aplicável);
- justificativa da alteração.

O histórico de migrações deve permanecer íntegro e auditável.

---

# Convenções

As migrações devem seguir padrões consistentes de nomenclatura.

Cada migração deve:

- representar uma única alteração lógica;
- possuir nome descritivo;
- evitar múltiplas responsabilidades;
- manter compatibilidade com o restante da solução.

Exemplos de nomes:

```text
AddClienteEndereco

CreateTabelaPedidos

UpdateIndiceProduto

RemoveCampoObsoleto
```

---

# Processo de Criação

Toda nova migração deve ser precedida por:

1. análise do impacto no modelo de dados;
2. validação das entidades afetadas;
3. revisão dos relacionamentos;
4. avaliação dos índices;
5. verificação de compatibilidade.

A geração das migrações deve seguir o processo definido pela tecnologia de persistência utilizada pela solução.

---

# Processo de Revisão

Antes da aplicação, cada migração deve ser revisada quanto a:

- alterações estruturais;
- impacto em dados existentes;
- criação ou remoção de índices;
- alteração de relacionamentos;
- compatibilidade entre versões.

Mudanças destrutivas devem possuir justificativa técnica e plano de mitigação.

---

# Aplicação das Migrações

A aplicação das migrações deve considerar os diferentes ambientes da solução.

Antes da execução devem ser avaliados:

- compatibilidade da versão;
- disponibilidade do ambiente;
- existência de backup;
- dependências entre migrações.

O processo operacional de aplicação deverá ser documentado após confirmação da estratégia utilizada pela solução.

---

# Compatibilidade

Sempre que possível, as alterações devem preservar compatibilidade entre versões.

Alterações que possam causar indisponibilidade devem ser planejadas previamente.

Sempre avaliar:

- impacto sobre aplicações existentes;
- compatibilidade entre APIs;
- dependência de versões anteriores.

---

# Rollback

Toda estratégia de migração deve prever procedimentos para recuperação em caso de falha.

Sempre que tecnicamente viável, deve existir:

- identificação da migração;
- estratégia de reversão;
- plano de contingência;
- validação da integridade após o rollback.

Os procedimentos específicos dependerão da tecnologia utilizada e deverão ser documentados após validação.

---

# Boas Práticas

Sempre:

- criar migrações pequenas e objetivas;
- documentar todas as alterações;
- revisar antes da aplicação;
- manter histórico completo;
- avaliar impacto em produção;
- sincronizar documentação e implementação.

---

# Anti-Padrões

Evitar:

- alterações estruturais não versionadas;
- migrações com múltiplos objetivos;
- remoção de histórico;
- alterações diretas no banco sem rastreabilidade;
- aplicação de migrações sem revisão.

---

# Limitações Conhecidas

O levantamento técnico confirmou a utilização do Entity Framework Core na camada de persistência.

Entretanto, ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-git-azure-infra`;
- `agilium-manager-azure-business`;
- `agilium-manager-azure-api`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- estratégia de migração efetivamente adotada;
- utilização de EF Core Migrations;
- abordagem Code First ou Database First;
- processo de implantação em cada ambiente;
- estratégia de rollback;
- automação de migrações.

---

# Atualização

Este documento deve ser atualizado sempre que ocorrer:

- alteração da estratégia de migração;
- mudança no processo de implantação;
- adoção de novas ferramentas;
- alteração das convenções;
- evolução da arquitetura de persistência.

---

# Documentação Relacionada

## Banco de Dados

- database/overview.md
- database/entities.md
- database/relationships.md
- database/mappings.md
- database/indexes.md
- database/constraints.md

## Arquitetura

- architecture/database.md
- architecture/layers.md

## Desenvolvimento

- development/deployment.md
- development/versioning.md