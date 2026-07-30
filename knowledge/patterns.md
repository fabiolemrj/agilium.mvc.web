# Patterns

## Objetivo

Este documento apresenta uma visão geral dos **padrões arquiteturais, de projeto e de implementação** adotados pelo **Agilium Manager**.

Seu objetivo é orientar desenvolvedores e agentes de IA sobre quais padrões devem ser utilizados durante o desenvolvimento, evitando soluções inconsistentes e garantindo uniformidade em toda a solução.

A documentação oficial encontra-se em:

```text
docs/patterns/
```

Este documento é um guia resumido. A documentação oficial contém exemplos, justificativas e diretrizes detalhadas para cada padrão.

---

# Visão Geral

Os padrões adotados pelo projeto têm como objetivos:

- Reduzir acoplamento
- Melhorar reutilização
- Facilitar manutenção
- Tornar o código previsível
- Facilitar testes
- Melhorar escalabilidade
- Padronizar implementações

Todo novo desenvolvimento deve seguir estes padrões.

---

# Organização

A documentação oficial normalmente encontra-se organizada em:

```text
docs/patterns/

README.md

architecture.md

repository.md

service-layer.md

dependency-injection.md

notification-pattern.md

specification.md

factory.md

builder.md

strategy.md

mediator.md

cqrs.md
```

---

# Padrões Arquiteturais

## Layered Architecture

A solução utiliza arquitetura em camadas.

Fluxo:

```text
Presentation

↓

Application

↓

Domain

↓

Repository

↓

Persistence
```

Cada camada possui responsabilidades bem definidas.

Consulte:

```text
knowledge/architecture.md
```

---

## Separation of Concerns (SoC)

Cada componente deve possuir apenas uma responsabilidade.

Exemplos:

- Controllers recebem requisições.
- Application Services coordenam casos de uso.
- Domain implementa regras de negócio.
- Repository realiza persistência.
- Infrastructure integra serviços externos.

---

## Dependency Inversion

Camadas superiores dependem de abstrações e não de implementações concretas.

Toda dependência deve ser resolvida através de **Dependency Injection**.

---

# Padrões de Projeto

## Repository Pattern

Responsável pelo acesso aos dados do domínio.

Responsabilidades:

- Persistência
- Consultas
- Atualizações
- Exclusões lógicas

Não implementa regras de negócio.

Relacionamento:

```text
Domain

↓

Repository Interface

↓

Repository Implementation

↓

Database
```

---

## Service Layer

Centraliza os casos de uso da aplicação.

Responsabilidades:

- Coordenar operações
- Orquestrar entidades
- Controlar transações
- Invocar serviços de domínio

Não deve concentrar regras de negócio complexas.

---

## Notification Pattern

Utilizado para representar erros de negócio sem lançar exceções para fluxos esperados.

Benefícios:

- Validação centralizada
- Melhor legibilidade
- Fluxos previsíveis
- Facilidade de testes

---

## Specification Pattern

Encapsula regras reutilizáveis do domínio.

Exemplos:

- ClienteAtivoSpecification
- ProdutoDisponivelSpecification
- UsuarioPodeVenderSpecification

Permite reutilizar regras sem duplicação.

---

## Factory Pattern

Utilizado para encapsular a criação de objetos complexos.

Aplicações comuns:

- Entidades
- Value Objects
- Objetos de integração

---

## Builder Pattern

Utilizado quando um objeto exige construção em múltiplas etapas.

Aplicações comuns:

- DTOs complexos
- Objetos de integração
- Cenários de testes

---

## Strategy Pattern

Utilizado quando diferentes algoritmos executam a mesma responsabilidade.

Exemplos:

- Cálculo de descontos
- Formas de pagamento
- Regras tributárias
- Estratégias de integração

---

## Mediator Pattern

Quando utilizado, centraliza a comunicação entre componentes reduzindo o acoplamento.

Aplicável principalmente para:

- Comandos
- Eventos
- Notificações

---

## CQRS

Pode ser utilizado quando houver necessidade de separar leitura e escrita.

Aplicações típicas:

- Relatórios
- Dashboards
- Consultas complexas
- Processamentos de alta performance

Não deve ser adotado sem necessidade justificada.

---

# Padrões de Persistência

## Entity Framework Core

ORM principal da solução.

Responsável por:

- CRUD
- Relacionamentos
- Mapeamentos
- Migrations

---

## Dapper

Utilizado exclusivamente para consultas especializadas.

Exemplos:

- Dashboards
- Relatórios
- Consultas de alto desempenho

Nunca utilizar para implementar regras de negócio.

---

## Soft Delete

Estratégia oficial para exclusão lógica.

Benefícios:

- Auditoria
- Recuperação
- Histórico
- Integridade

---

## Auditoria

Todas as entidades auditáveis devem registrar automaticamente:

- DataCadastro
- UsuarioCadastro
- DataAlteracao
- UsuarioAlteracao
- DataExclusao
- UsuarioExclusao

---

# Padrões para APIs

As APIs devem seguir:

- REST
- Versionamento
- DTOs
- Responses padronizadas
- Middleware global
- Validações centralizadas

Consulte:

```text
knowledge/api.md
```

---

# Padrões de Código

Todo código deve seguir:

- SOLID
- DRY (Don't Repeat Yourself)
- KISS (Keep It Simple, Stupid)
- YAGNI (You Aren't Gonna Need It)
- Clean Code

Esses princípios orientam a escrita de código simples, reutilizável e de fácil manutenção.

---

# O que Evitar

Evite:

- Lógica de negócio em Controllers.
- SQL dentro do domínio.
- Dependências entre camadas inadequadas.
- Código duplicado.
- Métodos excessivamente longos.
- Classes com múltiplas responsabilidades.
- Acoplamento direto entre módulos.

---

# Quando Utilizar Cada Padrão

| Situação | Padrão Recomendado |
|----------|--------------------|
| Persistência | Repository Pattern |
| Casos de uso | Service Layer |
| Regras reutilizáveis | Specification |
| Validação de negócio | Notification Pattern |
| Criação complexa de objetos | Factory Pattern |
| Construção incremental | Builder Pattern |
| Algoritmos variáveis | Strategy Pattern |
| Comunicação desacoplada | Mediator Pattern |
| Consultas complexas | CQRS + Dapper |

---

# ADRs Relacionadas

| Tema | ADR |
|------|-----|
| Arquitetura em Camadas | ADR-0001 |
| Repository Pattern | ADR-0002 |
| Notification Pattern | ADR-0003 |
| Entity Framework Core | ADR-0004 |
| Estratégia de Validação | ADR-0007 |
| Dependency Injection | ADR-0009 |
| Dapper | ADR-0010 |
| Service Layer | ADR-0011 |
| Logging | ADR-0013 |
| Soft Delete | ADR-0016 |
| Auditoria | ADR-0017 |

Consulte:

```text
knowledge/decisions.md
```

---

# Antes de Implementar

Verifique:

- Existe um padrão já definido para este cenário?
- O padrão escolhido respeita a arquitetura?
- Há um ADR relacionado?
- Existe implementação semelhante no projeto?
- A solução mantém baixo acoplamento e alta coesão?

---

# Documentação Relacionada

| Assunto | Documento |
|----------|-----------|
| Arquitetura | knowledge/architecture.md |
| APIs | knowledge/api.md |
| Banco de Dados | knowledge/database.md |
| Domínio | knowledge/domain.md |
| Desenvolvimento | knowledge/development.md |
| Regras de Negócio | knowledge/business-rules.md |
| Decisões Arquiteturais | knowledge/decisions.md |

---

# Documentação Oficial

Para informações detalhadas consulte:

```text
docs/patterns/
```

A documentação oficial contém:

- Motivação dos padrões
- Diretrizes de utilização
- Exemplos de implementação
- Boas práticas
- Cenários de uso
- Anti-patterns

---

# Fluxo Recomendado para Agentes de IA

```text
Ler patterns.md

↓

Identificar o problema

↓

Selecionar o padrão adequado

↓

Consultar ADRs relacionadas

↓

Consultar a documentação oficial

↓

Implementar

↓

Executar testes

↓

Atualizar documentação
```

---

# Resumo

Este documento apresenta os principais padrões utilizados no Agilium Manager.

Antes de implementar qualquer funcionalidade:

- identifique o padrão adequado ao problema;
- siga as convenções arquiteturais da solução;
- consulte os ADRs relacionados;
- utilize a documentação oficial como referência para exemplos e detalhes;
- mantenha consistência com as implementações existentes.