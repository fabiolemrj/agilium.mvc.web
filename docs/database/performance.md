# Performance da Camada de Persistência

## Objetivo

Documentar os princípios, diretrizes e boas práticas relacionados ao desempenho da camada de persistência do Agilium Manager.

Este documento estabelece recomendações para construção de consultas eficientes, utilização adequada dos recursos de persistência e monitoramento contínuo da performance do banco de dados.

---

# Escopo

Este documento contempla:

- Princípios de Performance
- Estratégias de Consulta
- Gerenciamento de Conexões
- Utilização de Índices
- Monitoramento
- Gargalos Comuns
- Boas Práticas
- Anti-Padrões

---

# Índice

- Visão Geral
- Objetivos de Performance
- Estratégias de Consulta
- Gerenciamento de Conexões
- Utilização de Índices
- Operações de Escrita
- Operações de Leitura
- Monitoramento
- Gargalos Comuns
- Boas Práticas
- Anti-Padrões
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

A camada de persistência deve ser projetada para oferecer desempenho adequado às necessidades da aplicação, mantendo equilíbrio entre:

- desempenho;
- legibilidade;
- manutenibilidade;
- integridade dos dados;
- escalabilidade.

A otimização deve ser baseada em evidências obtidas durante o monitoramento da aplicação.

---

# Objetivos de Performance

A estratégia de persistência deve buscar:

- reduzir tempo de resposta;
- minimizar acessos desnecessários ao banco;
- reduzir transferência excessiva de dados;
- otimizar consultas frequentes;
- preservar consistência dos dados;
- manter baixo acoplamento entre domínio e infraestrutura.

---

# Estratégias de Consulta

Sempre que possível, as consultas devem:

- retornar apenas os dados necessários;
- utilizar filtros adequados;
- evitar carregamento excessivo de entidades;
- utilizar paginação em grandes volumes de dados;
- aproveitar índices existentes;
- evitar consultas redundantes.

A estratégia específica de carregamento de entidades deverá refletir a implementação da solução.

---

# Gerenciamento de Conexões

A camada de persistência deve garantir:

- abertura de conexões apenas quando necessário;
- encerramento adequado das conexões;
- reutilização dos mecanismos fornecidos pela infraestrutura;
- tratamento adequado de falhas de comunicação.

Detalhes de configuração (pooling, timeouts e parâmetros específicos do provedor) deverão ser documentados após validação da infraestrutura utilizada.

---

# Utilização de Índices

As consultas devem considerar os índices definidos para o modelo de dados.

A criação de novos índices deve ser baseada em:

- frequência de utilização;
- volume de dados;
- planos de execução;
- métricas de desempenho.

Os detalhes encontram-se em:

```text
database/indexes.md
```

---

# Operações de Escrita

Operações de inserção, atualização e exclusão devem:

- executar apenas alterações necessárias;
- evitar transações excessivamente longas;
- minimizar bloqueios;
- preservar integridade referencial.

---

# Operações de Leitura

Consultas de leitura devem priorizar:

- projeção apenas dos campos necessários;
- paginação para grandes conjuntos de dados;
- reutilização de consultas recorrentes;
- redução da quantidade de dados trafegados.

---

# Monitoramento

A camada de persistência deve ser monitorada continuamente.

Sempre que possível, devem ser acompanhados indicadores como:

- tempo médio de execução das consultas;
- consultas lentas;
- utilização de índices;
- quantidade de leituras;
- quantidade de gravações;
- bloqueios;
- crescimento do banco;
- utilização de conexões.

As ferramentas de monitoramento dependerão da infraestrutura utilizada.

---

# Gargalos Comuns

Os seguintes cenários devem ser evitados sempre que possível:

- consultas retornando dados desnecessários;
- ausência de paginação;
- consultas repetidas para os mesmos dados;
- utilização inadequada de índices;
- transações muito longas;
- excesso de operações de escrita em lote sem planejamento.

A identificação desses gargalos deve ser baseada em monitoramento e análise de desempenho.

---

# Boas Práticas

Sempre:

- consultar apenas os dados necessários;
- utilizar paginação para grandes volumes;
- revisar consultas críticas periodicamente;
- documentar otimizações relevantes;
- revisar índices após alterações estruturais;
- medir desempenho antes e depois de otimizações.

---

# Anti-Padrões

Evitar:

- consultas sem filtros quando desnecessário;
- carregamento de grandes volumes de dados sem necessidade;
- duplicação de consultas;
- otimizações prematuras sem evidências;
- criação indiscriminada de índices;
- dependência de comportamento específico do provedor de banco de dados sem documentação.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de uma camada de persistência baseada em Entity Framework Core;
- utilização de entidades persistidas;
- utilização de repositórios.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-git-azure-infra`;
- `agilium-manager-azure-business`;
- `agilium-manager-azure-api`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- utilização de Dapper;
- estratégias de carregamento de entidades (Lazy Loading, Eager Loading ou Explicit Loading);
- utilização de `AsNoTracking`;
- configuração de pooling de conexões;
- políticas de timeout;
- ferramentas de monitoramento;
- otimizações específicas implementadas na camada de persistência.

---

# Atualização

Este documento deve ser atualizado sempre que ocorrer:

- alteração da estratégia de persistência;
- adoção de novas tecnologias de acesso aos dados;
- implementação de novas otimizações;
- alteração da estratégia de monitoramento;
- evolução significativa da infraestrutura de banco de dados.

---

# Documentação Relacionada

## Banco de Dados

- database/overview.md
- database/entities.md
- database/relationships.md
- database/indexes.md
- database/mappings.md
- database/migrations.md

## Arquitetura

- architecture/layers.md
- architecture/database.md

## Desenvolvimento

- development/coding-standards.md
- development/performance.md