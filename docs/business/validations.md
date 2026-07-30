# Estratégias de Validação

## Objetivo

Documentar a arquitetura e as estratégias de validação utilizadas pelo Agilium Manager, definindo onde cada tipo de validação deve ser implementado, quais são as responsabilidades de cada camada e como essas validações participam do fluxo completo da aplicação.

Este documento estabelece as diretrizes oficiais para implementação de validações em toda a plataforma e complementa a documentação de Regras de Negócio, Notification Pattern e Arquitetura em Camadas.

---

# Escopo

Este documento contempla:

- Estratégias de Validação
- Arquitetura de Validação
- Responsabilidades por Camada
- Validação de Entrada
- Validação de Negócio
- Validação de Persistência
- Validação de Segurança
- Notification Pattern
- Data Annotations
- Validações de Serviço
- Fluxo de Validação
- Matriz de Responsabilidades
- Boas Práticas
- Anti-Padrões

---

# Índice

- Visão Geral
- Arquitetura de Validação
- Responsabilidades por Camada
- Tipos de Validação
- Fluxo de Validação
- Notification Pattern
- Data Annotations
- Validações na Camada de Serviço
- Onde Implementar Cada Validação
- Matriz de Responsabilidades
- Boas Práticas
- Anti-Padrões
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

A plataforma utiliza diferentes mecanismos de validação conforme a responsabilidade de cada camada da arquitetura.

Cada validação deve ser implementada no nível adequado, evitando duplicidade de código, reduzindo acoplamento e preservando a separação de responsabilidades.

O objetivo principal é garantir que:

- dados inválidos sejam rejeitados o mais cedo possível;
- regras de negócio permaneçam centralizadas na camada de Application/Business;
- validações técnicas não sejam confundidas com regras funcionais;
- erros previsíveis sejam tratados através do Notification Pattern.

---

# Arquitetura de Validação

O fluxo geral de validação ocorre da seguinte forma:

```text
HTTP Request

↓

Model Binding

↓

Data Annotations

↓

Controller

↓

Application Service

↓

Business Validation

↓

BaseService

↓

Notification Pattern

↓

Repository

↓

Database

↓

HTTP Response
```

Cada etapa possui responsabilidades específicas e independentes.

---

# Responsabilidades por Camada

| Camada | Responsabilidade |
|---------|------------------|
| Model Binding | Conversão dos dados recebidos para objetos da aplicação |
| Data Annotations | Validação estrutural dos modelos de entrada |
| Controller | Receber a requisição, validar o ModelState e iniciar o caso de uso |
| Application Service | Orquestrar o fluxo da operação |
| BaseService | Compartilhar validações comuns e lógica reutilizável |
| Notification Pattern | Registrar violações de regras de negócio |
| Repository | Persistência dos dados |
| Banco de Dados | Garantir integridade física e restrições estruturais |

---

# Tipos de Validação

As validações utilizadas na plataforma podem ser classificadas em quatro categorias.

---

## Validação de Entrada

Responsável por garantir que os dados recebidos possuam estrutura válida.

Exemplos:

- campos obrigatórios;
- tamanho máximo;
- tamanho mínimo;
- formato;
- tipo de dado;
- intervalo de valores.

No projeto **agilium.mvc.web**, a validação de entrada é realizada principalmente através do mecanismo de **Model Binding** do ASP.NET Core MVC em conjunto com **Data Annotations**.

Essas validações possuem caráter estrutural e não devem conter regras de negócio.

---

## Validação de Negócio

Responsável por garantir que as regras funcionais sejam respeitadas.

Exemplos:

- produto não pode ser excluído caso possua movimentações;
- pedido somente pode ser faturado após aprovação;
- usuário deve pertencer à empresa selecionada;
- operação deve respeitar o estado atual da entidade.

As regras de negócio pertencem exclusivamente à camada de Application/Business.

---

## Validação de Persistência

Relacionada às restrições implementadas pelo banco de dados.

Exemplos:

- chave primária;
- chave estrangeira;
- unicidade;
- integridade referencial;
- tipos de dados.

As restrições do banco complementam a validação da aplicação, mas nunca substituem regras de negócio.

---

## Validação de Segurança

Relacionada ao controle de acesso da aplicação.

Exemplos:

- autenticação;
- autorização;
- permissões;
- claims;
- contexto da empresa;
- acesso a funcionalidades.

---

# Fluxo de Validação

Fluxo simplificado da validação durante uma requisição.

```text
HTTP Request

↓

Model Binding

↓

Data Annotations

↓

Controller

↓

Application Service

↓

Business Validation

↓

Notification Pattern

↓

Repository

↓

Database

↓

HTTP Response
```

Caso uma regra seja violada:

```text
Application Service

↓

Business Validation

↓

Notification Pattern

↓

Controller

↓

Bad Request / View

↓

Usuário
```

---

# Notification Pattern

O levantamento técnico identificou a utilização do Notification Pattern como mecanismo principal para tratamento de erros de negócio previsíveis.

Fluxo resumido:

```text
Application Service

↓

Business Validation

↓

Notification

↓

Controller

↓

Response
```

Ao invés de lançar exceções para regras de negócio esperadas, as violações são registradas em uma coleção de notificações que posteriormente será utilizada para construção da resposta.

Os detalhes completos da implementação encontram-se em:

```text
patterns/notification-pattern.md
```

---

# Data Annotations

As Data Annotations são utilizadas principalmente para validação estrutural dos modelos de entrada.

Exemplos comuns:

- Required
- StringLength
- MaxLength
- MinLength
- Range
- Compare
- EmailAddress
- Phone
- Display

Essas validações garantem apenas a consistência dos dados recebidos.

Não devem implementar regras de negócio.

---

# Validações na Camada de Serviço

As regras de negócio devem permanecer centralizadas na camada de Application/Business.

Fluxo recomendado:

```text
Controller

↓

Application Service

↓

Business Validation

↓

BaseService

↓

Notification Pattern

↓

Repository
```

Controllers não devem conter regras de negócio.

Repositories não devem validar regras funcionais.

---

# Onde Implementar Cada Validação

| Situação | Local recomendado |
|----------|-------------------|
| Campo obrigatório | Data Annotation |
| Formato inválido | Data Annotation |
| Tamanho máximo | Data Annotation |
| Produto inexistente | Application Service |
| Cliente inativo | Application Service |
| Operação não permitida | Application Service |
| Usuário sem permissão | Autorização |
| Empresa inválida | Application Service |
| Registro duplicado | Application Service + Banco |
| Integridade referencial | Banco de Dados |

---

# Matriz de Responsabilidades

| Tipo de Validação | Data Annotation | Controller | Application Service | Repository | Banco |
|-------------------|:--------------:|:----------:|:-------------------:|:----------:|:-----:|
| Campo obrigatório | ✔ | | | | |
| Formato | ✔ | | | | |
| Tamanho | ✔ | | | | |
| Regra de negócio | | | ✔ | | |
| Permissões | | | ✔ | | |
| Empresa selecionada | | | ✔ | | |
| Registro duplicado | | | ✔ | ✔ | ✔ |
| Chave Primária | | | | | ✔ |
| Chave Estrangeira | | | | | ✔ |
| Integridade Referencial | | | | | ✔ |

---

# Boas Práticas

Sempre:

- validar dados de entrada na camada de apresentação;
- manter Controllers responsáveis apenas pela orquestração da requisição;
- centralizar regras de negócio na camada de Application/Business;
- utilizar Notification Pattern para erros previsíveis;
- reutilizar validações comuns através da BaseService;
- documentar novas validações relevantes.

---

# Anti-Padrões

Evitar:

- regras de negócio em Controllers;
- regras de negócio em Repositories;
- Data Annotations contendo lógica funcional;
- duplicação de validações entre camadas;
- acesso ao banco apenas para validar campos obrigatórios;
- lançar exceções para regras de negócio previsíveis;
- mensagens de erro inconsistentes entre módulos.

---

# Limitações Conhecidas

O levantamento técnico confirmou no projeto **agilium.mvc.web**:

- utilização do Notification Pattern;
- existência de BaseService;
- utilização de Data Annotations na camada MVC;
- separação entre validações de entrada e regras de negócio.

Ainda deverão ser confirmados durante a análise dos projetos:

- `agilium-manager-azure-business`;
- `agilium-manager-azure-api`;
- `agilium-pdv-azure-api`;

os seguintes aspectos:

- utilização de FluentValidation;
- existência de classes Validator dedicadas;
- utilização de Specification Pattern;
- organização completa das validações de domínio;
- outras estratégias específicas de validação eventualmente utilizadas.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- criação de uma nova estratégia de validação;
- alteração do Notification Pattern;
- inclusão de novos mecanismos de validação;
- criação de validadores compartilhados;
- alteração da arquitetura da camada de Application;
- evolução das regras de negócio.

---

# Documentação Relacionada

## Arquitetura

- architecture/overview.md
- architecture/layers.md

## Padrões

- patterns/notification-pattern.md
- patterns/validation.md

## Negócio

- business-rules.md
- workflows.md

## Segurança

- security/authorization.md
- security/permissions.md