# Estratégia de Testes

# Objetivo

Documentar a estratégia de testes do Agilium Manager, estabelecendo diretrizes para validação da qualidade, confiabilidade e estabilidade da solução.

Este documento define os princípios gerais de testes, sua organização e as boas práticas adotadas durante o desenvolvimento.

---

# Escopo

Este documento contempla:

- Estratégia de Testes
- Tipos de Teste
- Organização dos Projetos
- Convenções
- Execução dos Testes
- Cobertura
- Automação
- Boas Práticas

---

# Índice

- Visão Geral
- Estratégia de Testes
- Tipos de Testes
- Organização
- Convenções
- Escrita dos Testes
- Execução
- Automação
- Cobertura
- Boas Práticas
- Anti-Padrões
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

Os testes têm como objetivo reduzir riscos durante a evolução da aplicação, garantindo que alterações mantenham o comportamento esperado da solução.

A estratégia de testes deverá acompanhar a arquitetura da aplicação e evoluir conforme novos componentes forem incorporados.

---

# Estratégia de Testes

A solução poderá utilizar diferentes níveis de testes.

Exemplo conceitual:

```text
Testes de Interface

↓

Testes de Integração

↓

Testes de Aplicação

↓

Testes de Domínio

↓

Testes de Infraestrutura
```

A estratégia definitiva deverá refletir os processos adotados pela equipe.

---

# Tipos de Testes

Dependendo da arquitetura da solução, poderão existir:

## Testes Unitários

Validam componentes isoladamente.

---

## Testes de Integração

Validam a interação entre componentes e dependências externas.

---

## Testes Funcionais

Validam funcionalidades completas da aplicação.

---

## Testes de Regressão

Garantem que alterações não afetem funcionalidades existentes.

---

## Testes Manuais

Utilizados para validação exploratória e cenários específicos quando aplicável.

---

# Organização

Os projetos de testes devem refletir a organização da solução.

Exemplo:

```text
tests/

Application.Tests

Domain.Tests

Infrastructure.Tests

Integration.Tests
```

A estrutura definitiva dependerá da organização adotada no repositório.

---

# Convenções

Os testes devem:

- possuir nomes descritivos;
- validar um comportamento por teste;
- ser independentes entre si;
- ser reproduzíveis;
- possuir dados previsíveis;
- evitar dependências externas desnecessárias.

---

# Escrita dos Testes

Recomenda-se que cada teste siga uma estrutura organizada.

Exemplo:

```text
Arrange

↓

Act

↓

Assert
```

Outros padrões poderão ser adotados conforme a estratégia da equipe.

---

# Execução

Os procedimentos para execução dos testes deverão contemplar:

- execução local;
- execução automatizada;
- validação antes da publicação;
- registro dos resultados.

Os comandos específicos serão documentados após validação da estrutura da solução.

---

# Automação

Quando aplicável, os testes deverão integrar-se ao processo automatizado de build e publicação.

A estratégia de automação deverá definir:

- quando executar;
- quais testes executar;
- critérios de aprovação;
- tratamento de falhas.

---

# Cobertura

A estratégia de cobertura deverá definir:

- componentes críticos;
- áreas prioritárias;
- metas de cobertura (quando adotadas);
- critérios para inclusão de novos testes.

Não se recomenda utilizar cobertura percentual como único indicador de qualidade.

---

# Boas Práticas

Sempre:

- criar testes para novos comportamentos;
- manter os testes simples;
- revisar testes juntamente com o código;
- manter isolamento entre casos de teste;
- atualizar testes após alterações funcionais.

Evitar:

- testes dependentes entre si;
- duplicação de cenários;
- dependência de ambientes externos quando desnecessário;
- excesso de lógica dentro dos próprios testes.

---

# Anti-Padrões

Evitar:

- múltiplas validações sem relação no mesmo teste;
- dados aleatórios sem controle;
- dependência da ordem de execução;
- testes frágeis;
- ignorar falhas recorrentes sem investigação.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização da plataforma .NET;
- organização da solução em múltiplos projetos.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-azure-api`;
- `agilium-manager-azure-business`;
- `agilium-manager-git-azure-infra`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- framework oficial de testes;
- existência e organização dos projetos de testes;
- estratégia de mocking;
- ferramentas de cobertura;
- automação em CI/CD;
- critérios mínimos de qualidade;
- comandos oficiais para execução dos testes.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- adoção de novos frameworks de testes;
- alteração da estratégia de validação;
- reorganização dos projetos de testes;
- alteração do pipeline de integração contínua;
- definição de novas políticas de cobertura.

---

# Documentação Relacionada

## Desenvolvimento

- development/getting-started.md
- development/build.md
- development/code-review.md

## Arquitetura

- architecture/overview.md
- architecture/layers.md

## Infraestrutura

- infrastructure/ci-cd.md

## Qualidade

- quality/code-quality.md
- quality/static-analysis.md