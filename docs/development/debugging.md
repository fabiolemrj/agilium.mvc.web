# Depuração e Diagnóstico

# Objetivo

Documentar as diretrizes, ferramentas, estratégias e boas práticas para depuração e diagnóstico do Agilium Manager durante o desenvolvimento, testes e manutenção da aplicação.

Este documento define como investigar falhas, identificar problemas e analisar o comportamento da solução.

---

# Escopo

Este documento contempla:

- Processo de Depuração
- Ferramentas Utilizadas
- Configuração do Ambiente
- Logging
- Tratamento de Exceções
- Diagnóstico de Problemas
- Boas Práticas
- Solução de Problemas Comuns

---

# Índice

- Visão Geral
- Estratégia de Depuração
- Ferramentas
- Configuração do Ambiente
- Logging
- Tratamento de Exceções
- Diagnóstico
- Problemas Comuns
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

A depuração deve permitir identificar rapidamente problemas de funcionamento da aplicação, preservando a rastreabilidade dos erros e reduzindo o tempo necessário para análise e correção.

Sempre que possível, a investigação deve utilizar informações produzidas pela própria aplicação, evitando alterações temporárias no código apenas para fins de diagnóstico.

---

# Estratégia de Depuração

O processo recomendado consiste nas seguintes etapas:

```text
Identificação do Problema

↓

Reprodução

↓

Análise dos Logs

↓

Depuração

↓

Correção

↓

Validação

↓

Documentação da Solução
```

Toda correção relevante deve resultar na atualização da documentação técnica quando necessário.

---

# Ferramentas

As ferramentas utilizadas para depuração poderão incluir:

- IDE utilizada pela equipe;
- depurador da plataforma .NET;
- logs da aplicação;
- monitoramento da infraestrutura;
- ferramentas de inspeção de banco de dados;
- ferramentas de análise de requisições HTTP.

As ferramentas oficialmente adotadas deverão ser documentadas após validação da arquitetura e da infraestrutura.

---

# Configuração do Ambiente

O ambiente de desenvolvimento deverá permitir:

- execução local da aplicação;
- depuração por pontos de interrupção (breakpoints);
- inspeção de variáveis;
- análise da pilha de chamadas (call stack);
- execução passo a passo do código.

Detalhes específicos de perfis de inicialização e configurações da IDE deverão ser documentados conforme a implementação existente.

---

# Logging

O mecanismo de logging deverá fornecer informações suficientes para apoiar o diagnóstico de falhas.

Os registros devem priorizar:

- erros;
- exceções;
- eventos relevantes da aplicação;
- informações necessárias para rastreamento.

A estratégia de logging (biblioteca utilizada, níveis de log e configuração) será documentada após validação da implementação.

---

# Tratamento de Exceções

As exceções devem:

- ser tratadas em nível apropriado;
- registrar informações suficientes para diagnóstico;
- evitar exposição de detalhes internos ao usuário final;
- manter consistência com a estratégia de tratamento de erros da solução.

---

# Diagnóstico

Ao investigar um problema, recomenda-se verificar:

- comportamento reproduzível;
- mensagens de erro;
- registros de log;
- requisições realizadas;
- estado da aplicação;
- dependências externas;
- acesso aos dados;
- integrações envolvidas.

---

# Problemas Comuns

Esta seção deve ser atualizada conforme novos cenários forem identificados.

| Problema | Sintoma | Possível Causa | Solução |
|----------|---------|----------------|----------|
| Exemplo | Erro ao iniciar a aplicação | Configuração inválida | Revisar configurações do ambiente |

---

# Boas Práticas

Sempre:

- reproduzir o problema antes de corrigi-lo;
- utilizar logs para apoiar a investigação;
- remover códigos temporários de depuração após a análise;
- documentar problemas recorrentes;
- validar a correção antes da publicação.

Evitar:

- utilizar mensagens de depuração em produção;
- registrar informações sensíveis em logs;
- alterar o comportamento da aplicação apenas para facilitar a depuração;
- ignorar exceções silenciosamente.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização da plataforma ASP.NET Core;
- organização da solução em múltiplos projetos.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-azure-api`;
- `agilium-manager-azure-business`;
- `agilium-manager-git-azure-infra`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- ferramenta oficial de logging;
- utilização ou não de KissLog;
- configuração dos níveis de log;
- estratégia de tratamento de exceções;
- configuração dos perfis de inicialização;
- estratégia de depuração remota;
- ferramentas de monitoramento adotadas.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração da estratégia de logging;
- adoção de novas ferramentas de diagnóstico;
- alteração da estratégia de tratamento de exceções;
- inclusão de novos procedimentos de depuração.

---

# Documentação Relacionada

## Desenvolvimento

- development/environment.md
- development/build.md
- development/getting-started.md

## Arquitetura

- architecture/overview.md
- architecture/layers.md

## Padrões

- patterns/error-handling.md
- patterns/logging.md

## Infraestrutura

- infrastructure/monitoring.md
- infrastructure/observability.md