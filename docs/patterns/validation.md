# Validation

# Objetivo

Documentar a arquitetura de validação utilizada pelo Agilium Manager, descrevendo as responsabilidades de cada camada de validação e os mecanismos empregados para garantir a consistência dos dados e das regras de negócio.

---

# Escopo

Este documento contempla:

- Arquitetura de Validação
- Data Annotations
- FluentValidation
- Notification Pattern
- ViewModels
- Regras de Negócio
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura de Validação
- Validação na Interface
- Data Annotations
- FluentValidation
- Notification Pattern
- Fluxo de Validação
- Convenções
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager adota uma estratégia de validação em múltiplas camadas.

Cada mecanismo possui responsabilidades específicas, permitindo separar validações da interface das validações de domínio e regras de negócio.

Os principais componentes identificados no levantamento técnico são:

- Data Annotations;
- FluentValidation;
- Notification Pattern. :contentReference[oaicite:1]{index=1}

---

# Arquitetura de Validação

O fluxo de validação segue a arquitetura:

```text
View

↓

ViewModel

↓

Data Annotations

↓

Controller

↓

Service

↓

FluentValidation

↓

Notification Pattern

↓

Repository
```

Essa organização mantém as responsabilidades distribuídas entre apresentação, negócio e persistência. :contentReference[oaicite:2]{index=2}

---

# Validação na Interface

A camada MVC utiliza **ViewModels** para representar os dados enviados e recebidos pelas Views.

Os ViewModels concentram as validações relacionadas à entrada de dados da interface, contribuindo para a consistência antes do processamento da camada de negócio. :contentReference[oaicite:3]{index=3}

---

# Data Annotations

O levantamento técnico confirma a utilização de **Data Annotations** nos ViewModels.

As Data Annotations são utilizadas para definir regras de validação da camada de apresentação e integrar o processo de validação do ASP.NET Core MVC. :contentReference[oaicite:4]{index=4}

A identificação dos atributos específicos utilizados deverá ser realizada por inspeção direta dos ViewModels.

---

# FluentValidation

A camada Business utiliza **FluentValidation** para implementação das regras de validação de domínio.

Os validadores atuam antes da persistência dos dados, verificando se os objetos atendem às regras definidas pela aplicação. :contentReference[oaicite:5]{index=5}

---

# Notification Pattern

O projeto utiliza o **Notification Pattern** para comunicar violações de regras de negócio.

Os componentes identificados são:

```text
Business/

└── Notificacoes/

    ├── INotificador

    ├── Notificador

    └── Notificacao
```

Esse mecanismo permite registrar erros de negócio sem utilizar exceções como fluxo normal de processamento. :contentReference[oaicite:6]{index=6}

---

# Fluxo de Validação

O processamento das validações ocorre conforme o fluxo abaixo:

```text
Usuário

↓

View

↓

ViewModel

↓

Data Annotations

↓

Controller

↓

Service

↓

FluentValidation

↓

Notification Pattern

↓

Resposta
```

Quando uma regra de negócio não é satisfeita, a ocorrência é registrada pelo Notification Pattern e tratada de forma padronizada pelo `MainController`. :contentReference[oaicite:7]{index=7}

---

# Convenções

A estratégia de validação segue as seguintes diretrizes:

- utilizar ViewModels para entrada de dados;
- concentrar validações da interface em Data Annotations;
- implementar regras de negócio na camada Business;
- utilizar FluentValidation para validações de domínio;
- comunicar erros funcionais por meio do Notification Pattern;
- manter a separação entre validações de interface e regras de negócio.

---

# Boas Práticas

Sempre:

- validar dados de entrada utilizando ViewModels;
- manter regras de negócio na camada Business;
- utilizar FluentValidation para validações complexas;
- registrar violações por meio do Notification Pattern;
- reutilizar validadores sempre que possível.

Evitar:

- implementar regras de negócio nas Views;
- duplicar validações em múltiplas camadas sem necessidade;
- utilizar exceções para representar erros funcionais esperados;
- acoplar validações da interface à persistência.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de ViewModels;
- utilização de Data Annotations;
- utilização de FluentValidation;
- utilização do Notification Pattern;
- existência de `INotificador`;
- existência de `Notificador`;
- existência de `Notificacao`;
- integração do Notification Pattern com a camada Business e o `MainController`. :contentReference[oaicite:8]{index=8}

Ainda deverão ser documentados mediante análise do código-fonte:

- inventário dos atributos de Data Annotations utilizados;
- catálogo dos Validators;
- atributos de validação personalizados, se existentes;
- organização das classes de validação;
- mensagens padronizadas de validação;
- integração detalhada entre FluentValidation e Notification Pattern.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- criação de novos Validators;
- alteração das regras de negócio;
- inclusão de novos ViewModels;
- evolução do Notification Pattern;
- alteração da estratégia de validação.

---

# Documentação Relacionada

## Desenvolvimento

- development/notification-pattern.md

## Negócio

- business/validations.md

## Arquitetura

- architecture/patterns.md
- architecture/layers.md

## Interface

- ui/mvc.md
- ui/razor.md