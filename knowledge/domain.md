# Domain

## Objetivo

Este documento apresenta uma visão geral da camada de **Domínio** do Agilium Manager, responsável por representar o negócio e concentrar todas as regras que definem o comportamento da aplicação.

A documentação oficial encontra-se em:

```text
docs/domain/
```

Este documento serve como um guia rápido para desenvolvedores e agentes de IA, indicando como o domínio está organizado e onde localizar a documentação detalhada.

---

# Visão Geral

O domínio representa o núcleo da aplicação.

É nesta camada que estão concentrados:

- Regras de negócio
- Entidades
- Value Objects
- Aggregates
- Domain Services
- Domain Events
- Especificações
- Validações de negócio

Nenhuma outra camada deve conhecer mais sobre o negócio do que o próprio domínio.

---

# Papel do Domínio

O domínio é responsável por responder perguntas como:

- O que o sistema faz?
- Quais são as regras do negócio?
- Quais operações são permitidas?
- Quais validações devem ser executadas?
- Quais estados uma entidade pode assumir?

---

# Arquitetura

O domínio encontra-se no centro da arquitetura.

```text
MVC / API

↓

Application

↓

Domain

↓

Repository

↓

Persistence

↓

Database
```

Toda regra de negócio deve ser implementada nesta camada.

---

# Organização

A documentação oficial normalmente encontra-se organizada em:

```text
docs/domain/

README.md

entities/

aggregates/

value-objects/

domain-services/

domain-events/

specifications/

validations/
```

---

# Componentes do Domínio

## Entities

Representam objetos de negócio com identidade própria.

Exemplos:

- Cliente
- Empresa
- Produto
- Pedido
- Venda
- Usuário
- Caixa

Documentação:

```text
docs/domain/entities/
```

---

## Value Objects

Representam conceitos imutáveis do domínio.

Exemplos:

- Endereço
- Documento
- Dinheiro
- E-mail
- Telefone

Características:

- Imutáveis
- Comparados por valor
- Sem identidade própria

Documentação:

```text
docs/domain/value-objects/
```

---

## Aggregates

Agrupam entidades relacionadas e garantem consistência transacional.

Responsabilidades:

- Controlar alterações
- Garantir invariantes
- Definir Aggregate Root

Documentação:

```text
docs/domain/aggregates/
```

---

## Domain Services

Implementam regras que envolvem múltiplas entidades.

Exemplos:

- Processamento de venda
- Cálculo de descontos
- Fechamento de caixa
- Liberação de licenciamento

Documentação:

```text
docs/domain/domain-services/
```

---

## Domain Events

Representam acontecimentos relevantes do domínio.

Exemplos:

- VendaRealizada
- PedidoCancelado
- CaixaFechado
- ClienteInativado

Utilizados para desacoplamento e integração entre componentes.

Documentação:

```text
docs/domain/domain-events/
```

---

## Specifications

Centralizam regras reutilizáveis.

Exemplos:

- ClienteAtivoSpecification
- ProdutoDisponivelSpecification
- UsuarioPodeRealizarVendaSpecification

Documentação:

```text
docs/domain/specifications/
```

---

## Validations

Implementam validações específicas do domínio.

Exemplos:

- Limites de desconto
- Estoque disponível
- Cliente ativo
- Caixa aberto

Documentação:

```text
docs/domain/validations/
```

---

# Regras Gerais

O domínio deve:

- Conter todas as regras de negócio.
- Ser independente da interface.
- Ser independente do banco de dados.
- Não depender de frameworks.
- Ser reutilizável.
- Ser facilmente testável.

---

# O que NÃO pertence ao Domínio

Não implementar no domínio:

- Controllers
- APIs
- Views
- DTOs
- Entity Framework
- SQL
- HTTP
- Infraestrutura
- Docker
- Configuração

---

# Relação com outras Camadas

```text
Controller

↓

Application Service

↓

Domain

↓

Repository

↓

Persistence
```

O domínio nunca deve depender das camadas superiores.

---

# Regras de Negócio

As regras de negócio do domínio encontram-se documentadas em:

```text
docs/business-rules/
```

Consulte:

```text
knowledge/business-rules.md
```

---

# Persistência

A persistência deve respeitar o modelo definido pelo domínio.

Consulte:

```text
knowledge/database.md
```

---

# APIs

As APIs devem consumir os casos de uso definidos pelo domínio através da camada de Application.

Consulte:

```text
knowledge/api.md
```

---

# ADRs Relacionadas

| Tema | ADR |
|------|-----|
| Arquitetura em Camadas | ADR-0001 |
| Repository Pattern | ADR-0002 |
| Notification Pattern | ADR-0003 |
| Entity Framework Core | ADR-0004 |
| Estratégia de Validação | ADR-0007 |
| Service Layer | ADR-0011 |
| Soft Delete | ADR-0016 |
| Auditoria | ADR-0017 |
| Estratégia de Testes | ADR-0020 |

Consulte:

```text
knowledge/decisions.md
```

---

# Antes de Alterar o Domínio

Verifique:

- Existe documentação da entidade?
- Existe regra de negócio relacionada?
- Existe impacto em outros agregados?
- Existe um Domain Service adequado?
- Existe um ADR aplicável?
- A alteração preserva as invariantes do domínio?

---

# Documentação Relacionada

| Assunto | Documento |
|----------|-----------|
| Arquitetura | knowledge/architecture.md |
| APIs | knowledge/api.md |
| Banco de Dados | knowledge/database.md |
| Regras de Negócio | knowledge/business-rules.md |
| Desenvolvimento | knowledge/development.md |
| Padrões | knowledge/patterns.md |
| Decisões Arquiteturais | knowledge/decisions.md |

---

# Documentação Oficial

Para informações detalhadas consulte:

```text
docs/domain/
```

A documentação oficial contém:

- Entidades
- Value Objects
- Aggregates
- Domain Services
- Domain Events
- Specifications
- Validações
- Diagramas do domínio

---

# Fluxo Recomendado para Agentes de IA

```text
Ler domain.md

↓

Identificar a entidade ou agregado

↓

Consultar business-rules.md

↓

Consultar decisions.md

↓

Ler a documentação oficial

↓

Implementar alteração

↓

Criar ou atualizar testes

↓

Atualizar documentação
```

---

# Resumo

Este documento apresenta uma visão geral da camada de domínio do Agilium Manager.

Antes de implementar qualquer funcionalidade:

- identifique a entidade ou agregado envolvido;
- implemente regras de negócio exclusivamente no domínio;
- mantenha o domínio independente de infraestrutura;
- consulte as ADRs relacionadas;
- utilize `docs/domain/` como fonte oficial de documentação.