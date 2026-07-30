# Notification Pattern

# Objetivo

Documentar a arquitetura do Notification Pattern utilizada pelo Agilium Manager para comunicação de erros de validação e regras de negócio entre as camadas da aplicação.

O padrão permite registrar inconsistências de negócio sem utilizar exceções como mecanismo de controle de fluxo.

---

# Escopo

Este documento contempla:

- Notification Pattern
- Componentes
- Fluxo de Notificações
- Integração com Services
- Integração com Controllers
- Convenções
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura do Notification Pattern
- Componentes
- Fluxo das Notificações
- Utilização nos Services
- Utilização nos Controllers
- Convenções
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager adota o **Notification Pattern** como mecanismo para tratamento de validações de negócio.

Em vez de utilizar exceções para comunicar erros esperados de domínio, as inconsistências são registradas como notificações e retornadas de forma controlada à camada de apresentação. :contentReference[oaicite:1]{index=1}

---

# Arquitetura do Notification Pattern

O fluxo de processamento segue a arquitetura:

```text
Controller

↓

Service

↓

Validações

↓

Notificador

↓

Controller

↓

View / Resposta
```

Essa abordagem desacopla as regras de negócio da camada de apresentação e facilita o tratamento uniforme de erros funcionais. :contentReference[oaicite:2]{index=2}

---

# Componentes

O levantamento identificou a existência dos seguintes componentes:

```text
Business/

└── Notificacoes/

    ├── INotificador

    ├── Notificador

    └── Notificacao
```

Esses componentes formam a infraestrutura responsável pelo gerenciamento das notificações de negócio. :contentReference[oaicite:3]{index=3}

---

# Fluxo das Notificações

O processamento ocorre da seguinte forma:

```text
Controller

↓

Service

↓

Validação

↓

Notificador

↓

Controller

↓

Resposta para o usuário
```

Quando uma regra de negócio não é satisfeita, a informação é registrada no componente de notificações e posteriormente tratada pelo Controller. :contentReference[oaicite:4]{index=4}

---

# Utilização nos Services

Os Services concentram as regras de negócio da aplicação.

Durante o processamento, quando uma validação funcional identifica uma inconsistência, o Notification Pattern é utilizado para registrar a ocorrência, permitindo que o fluxo continue de maneira controlada. :contentReference[oaicite:5]{index=5}

---

# Integração com Validações

O levantamento técnico identificou a utilização do **FluentValidation** na camada Business.

Os validadores trabalham em conjunto com o Notification Pattern para registrar violações das regras de negócio antes que os dados avancem para a camada de persistência. :contentReference[oaicite:6]{index=6}

---

# Utilização nos Controllers

O `MainController` recebe uma instância de:

```text
INotificador
```

por meio da Injeção de Dependência.

Além disso, concentra métodos responsáveis por verificar o resultado das operações e encaminhar as notificações para a camada de apresentação de forma padronizada. :contentReference[oaicite:7]{index=7}

---

# Convenções

A utilização do Notification Pattern segue as seguintes diretrizes:

- utilizar notificações para erros de negócio previsíveis;
- manter validações concentradas na camada Business;
- evitar utilização de exceções para validações funcionais;
- centralizar o tratamento das notificações nos Controllers;
- preservar o desacoplamento entre domínio e apresentação.

---

# Boas Práticas

Sempre:

- registrar violações de regras de negócio por meio do Notification Pattern;
- manter mensagens objetivas e compreensíveis;
- utilizar o `MainController` para tratamento uniforme das notificações;
- integrar os validadores ao mecanismo de notificações.

Evitar:

- utilizar exceções para representar validações esperadas;
- duplicar mensagens de erro;
- realizar validações de negócio diretamente nas Views;
- espalhar lógica de tratamento de notificações por múltiplos Controllers.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização do Notification Pattern;
- existência de `INotificador`;
- existência de `Notificador`;
- existência de `Notificacao`;
- utilização do `MainController`;
- integração com a camada Business;
- utilização de FluentValidation em conjunto com o padrão. :contentReference[oaicite:8]{index=8}

Ainda deverão ser documentados mediante análise do código-fonte:

- métodos públicos de `INotificador`;
- estrutura da classe `Notificacao`;
- estratégia de armazenamento das notificações;
- ciclo de vida do `Notificador`;
- formato exato das respostas retornadas para Views e APIs.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração do Notification Pattern;
- criação de novos validadores;
- evolução da camada Business;
- alteração do fluxo de retorno das notificações.

---

# Documentação Relacionada

## Arquitetura

- architecture/patterns.md
- architecture/layers.md

## Desenvolvimento

- development/validation.md

## Negócio

- business/validations.md

## Interface

- ui/mvc.md