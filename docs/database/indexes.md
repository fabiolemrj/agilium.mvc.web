# Estratégia de Indexação

## Objetivo

Documentar a estratégia de indexação utilizada pelo Agilium Manager, descrevendo os princípios adotados para otimização de consultas, integridade dos dados e desempenho da camada de persistência.

Este documento define as diretrizes para criação, manutenção e documentação dos índices utilizados pela plataforma.

---

# Escopo

Este documento contempla:

- Estratégia de Indexação
- Tipos de Índices
- Índices das Entidades
- Convenções
- Critérios para Criação
- Impactos na Performance
- Boas Práticas
- Monitoramento
- Recomendações

---

# Índice

- Visão Geral
- Estratégia de Indexação
- Critérios para Criação
- Tipos de Índices
- Estrutura de Documentação
- Monitoramento
- Boas Práticas
- Anti-Padrões
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

Os índices possuem como objetivo otimizar o acesso aos dados persistidos pela aplicação.

Sua criação deve considerar o equilíbrio entre:

- desempenho das consultas;
- custo de manutenção;
- consumo de armazenamento;
- impacto em operações de escrita.

Todo índice deve possuir finalidade claramente documentada.

---

# Estratégia de Indexação

A criação de índices deve seguir os seguintes princípios:

- suportar consultas frequentes;
- otimizar filtros utilizados pela aplicação;
- melhorar operações de ordenação;
- otimizar relacionamentos entre entidades;
- preservar a integridade dos dados quando necessário.

Índices não devem ser criados apenas por conveniência.

Toda inclusão deve ser baseada em necessidades reais de acesso aos dados.

---

# Critérios para Criação

Um índice pode ser criado quando:

- uma consulta é executada frequentemente;
- existe filtragem recorrente por determinado campo;
- existe ordenação frequente;
- existe necessidade de garantir unicidade;
- há junções recorrentes entre entidades.

Antes da criação de novos índices deve ser avaliado:

- frequência de leitura;
- frequência de escrita;
- volume esperado de dados;
- impacto sobre inserções e atualizações.

---

# Tipos de Índices

A plataforma poderá utilizar diferentes estratégias de indexação conforme o mecanismo de persistência adotado.

Exemplos:

- índice simples;
- índice composto;
- índice único;
- índice para chave estrangeira;
- índice para ordenação.

A utilização de recursos específicos de um banco de dados deverá ser documentada apenas após confirmação da tecnologia utilizada.

---

# Estrutura de Documentação

Cada índice deve possuir documentação contendo:

## Nome

Nome do índice.

---

## Entidade

Entidade relacionada.

---

## Campos

Campos indexados.

---

## Tipo

Exemplo:

- Simples
- Composto
- Único

---

## Objetivo

Descrição da finalidade do índice.

---

## Consultas Beneficiadas

Descrição das consultas que utilizam o índice.

---

## Impacto Esperado

Descrição dos ganhos esperados.

---

## Observações

Informações adicionais relevantes.

---

# Monitoramento

A eficiência dos índices deve ser periodicamente avaliada considerando:

- consultas lentas;
- crescimento do banco;
- utilização dos índices;
- custo de manutenção;
- necessidade de reorganização.

Alterações na estratégia de indexação devem ser baseadas em evidências obtidas durante o monitoramento.

---

# Boas Práticas

Sempre:

- criar índices apenas quando houver justificativa técnica;
- documentar todos os índices;
- revisar índices periodicamente;
- avaliar o impacto em operações de escrita;
- manter consistência entre documentação e implementação.

---

# Anti-Padrões

Evitar:

- índices duplicados;
- excesso de índices em uma mesma entidade;
- índices sem utilização conhecida;
- criação de índices sem análise de consultas;
- dependência de índices não documentados.

---

# Limitações Conhecidas

O levantamento técnico confirmou a existência de uma camada de persistência baseada em Entity Framework Core.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-git-azure-infra`;
- `agilium-manager-azure-business`;
- `agilium-manager-azure-api`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- banco de dados efetivamente utilizado;
- estratégia de indexação implementada;
- índices existentes;
- convenções adotadas para nomenclatura;
- índices específicos do mecanismo de persistência.

---

# Atualização

Este documento deve ser atualizado sempre que ocorrer:

- criação de novos índices;
- remoção de índices;
- alteração da estratégia de persistência;
- otimização de consultas;
- evolução do modelo de dados.

---

# Documentação Relacionada

## Banco de Dados

- database/entities.md
- database/relationships.md
- database/mappings.md
- database/migrations.md

## Arquitetura

- architecture/database.md
- architecture/layers.md

## Performance

- database/performance.md