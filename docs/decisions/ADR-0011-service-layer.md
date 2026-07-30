# ADR-0011 - Adoção da Camada de Services (Service Layer)

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é composto por uma arquitetura em camadas onde Controllers, APIs, Repositories e Domínio possuem responsabilidades bem definidas.

Durante a evolução da plataforma foi identificado que diversas regras de orquestração dos casos de uso estavam sendo implementadas diretamente nas Controllers ou nos Repositories, causando:

- Alto acoplamento;
- Duplicação de código;
- Mistura de responsabilidades;
- Dificuldade para testes;
- Crescimento excessivo das Controllers.

Era necessário criar uma camada intermediária responsável pela coordenação dos casos de uso da aplicação.

---

# Problema

Sem uma camada de Services:

- Controllers passam a conhecer regras de negócio;
- Repositories recebem responsabilidades que não lhes pertencem;
- Regras ficam duplicadas entre APIs e MVC;
- Casos de uso tornam-se difíceis de reutilizar;
- Testes unitários tornam-se mais complexos.

Era necessário centralizar a lógica de aplicação sem violar a separação de responsabilidades.

---

# Alternativas Consideradas

## Alternativa 1 — Controllers chamando Repositories diretamente

### Vantagens

- Implementação simples.
- Menor quantidade de classes.

### Desvantagens

- Alto acoplamento.
- Controllers grandes.
- Baixa reutilização.
- Regras duplicadas.

---

## Alternativa 2 — Regras diretamente nos Repositories

### Vantagens

- Poucas camadas.

### Desvantagens

- Repository deixa de ser responsável apenas pela persistência.
- Mistura regras de negócio com acesso a dados.
- Viola o Repository Pattern.

---

## Alternativa 3 — Service Layer (Escolhida)

### Vantagens

- Separação clara das responsabilidades.
- Reutilização de casos de uso.
- Melhor organização.
- Maior testabilidade.
- Baixo acoplamento.

### Desvantagens

- Maior quantidade de classes.
- Camada adicional na arquitetura.

---

# Decisão

Foi adotada oficialmente a **Service Layer** como responsável pela implementação dos casos de uso da aplicação.

Toda operação executada pela aplicação deverá passar pela camada de Services antes de acessar qualquer Repository.

As Controllers nunca deverão acessar diretamente a camada de persistência.

---

# Objetivos

Esta estratégia possui os seguintes objetivos:

- Centralizar casos de uso.
- Reduzir acoplamento.
- Melhorar reutilização.
- Facilitar testes.
- Padronizar fluxo da aplicação.
- Separar regras de aplicação das regras de domínio.

---

# Fluxo Arquitetural

```text
Controller

↓

Service

↓

Domain

↓

Repository

↓

Persistence

↓

Database
```

---

# Responsabilidades

## Controller

Responsável apenas por:

- Receber requisições.
- Validar entrada.
- Chamar Services.
- Retornar respostas.

Não deve conter regras de negócio.

---

## Service

Responsável por:

- Implementar casos de uso.
- Orquestrar chamadas entre componentes.
- Controlar transações.
- Coordenar múltiplos Repositories.
- Acionar regras de domínio.
- Integrar serviços externos.
- Aplicar regras da aplicação.

---

## Domain

Responsável exclusivamente pelas regras de negócio.

Exemplos:

- Venda permitida.
- Produto disponível.
- Cliente bloqueado.
- Caixa aberto.

---

## Repository

Responsável apenas pelo acesso aos dados.

Não deve conter:

- Regras de negócio.
- Regras de autorização.
- Regras de aplicação.

---

# O que pertence ao Service

Exemplos:

- Abrir venda.
- Finalizar pedido.
- Registrar pagamento.
- Cancelar documento.
- Integrar sistema externo.
- Atualizar estoque após venda.
- Validar permissões do caso de uso.
- Publicar eventos.

---

# O que NÃO pertence ao Service

Não implementar:

- SQL.
- Acesso direto ao banco.
- Configuração do EF Core.
- Regras específicas da infraestrutura.

Essas responsabilidades pertencem aos Repositories e à camada de Persistência.

---

# Organização

Estrutura recomendada:

```text
Application/

├── Services/

│   ├── ProdutoService.cs

│   ├── ClienteService.cs

│   ├── VendaService.cs

│   ├── CaixaService.cs

│   └── ...

│

├── Interfaces/

│   ├── IProdutoService.cs

│   ├── IClienteService.cs

│   ├── IVendaService.cs

│   └── ...
```

---

# Dependências

Os Services poderão depender de:

- Repositories;
- Outros Services (quando necessário e sem criar dependências circulares);
- Notification Context;
- Unit of Work;
- Serviços de infraestrutura;
- Clientes HTTP;
- Serviços de autenticação;
- Logging.

Sempre através de interfaces.

---

# Transações

Quando um caso de uso envolver múltiplas operações de persistência, o Service será responsável por coordenar a transação.

Exemplo:

```text
Registrar Venda

↓

Salvar Venda

↓

Salvar Itens

↓

Atualizar Estoque

↓

Registrar Financeiro

↓

Commit
```

---

# Integrações

Toda comunicação com sistemas externos deverá ser iniciada pelo Service.

Exemplos:

- API Fiscal;
- Gateway de Pagamento;
- Cardápio Digital;
- Serviços de Licenciamento;
- Serviços de Notificação.

---

# Testabilidade

A camada de Services deverá ser completamente testável através de mocks.

Exemplo:

```text
Service

↓

Mock Repository

↓

Mock Notification

↓

Mock HTTP Client
```

Nenhum teste unitário deverá depender do banco de dados.

---

# Benefícios

- Código organizado.
- Alta reutilização.
- Melhor separação de responsabilidades.
- Facilidade para testes.
- Controllers pequenas.
- Baixo acoplamento.
- Maior escalabilidade.

---

# Desvantagens

- Mais classes na solução.
- Camada adicional.
- Necessidade de organização.

---

# Riscos

Caso esta estratégia não seja seguida:

- Controllers grandes.
- Repository com regras de negócio.
- Código duplicado.
- Baixa reutilização.
- Dificuldade para testes.
- Arquitetura inconsistente.

---

# Impacto

Esta decisão impacta:

- Controllers
- APIs
- MVC
- Application
- Domain
- Repository
- Persistência
- Integrações
- Testes

---

# Plano de Implementação

1. Criar interfaces para todos os Services.
2. Implementar casos de uso na camada Application.
3. Remover lógica de negócio das Controllers.
4. Remover lógica de aplicação dos Repositories.
5. Registrar Services no Container de DI.
6. Criar testes unitários para os Services.
7. Atualizar documentação técnica.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Toda Controller depende apenas de Services.
- Nenhuma Controller acessa Repositories diretamente.
- Repositories contêm apenas acesso a dados.
- Os casos de uso são implementados na camada de Services.
- As regras de domínio permanecem encapsuladas na camada Domain.
- Todos os Services são registrados via Dependency Injection.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0002 — Repository Pattern
- ADR-0003 — Notification Pattern
- ADR-0007 — Estratégia de Validação
- ADR-0009 — Estratégia de Dependency Injection
- ADR-0010 — Dapper para Consultas de Alta Performance
- ADR-0014 — Tratamento Global de Exceções

---

# Referências

- Martin Fowler — *Patterns of Enterprise Application Architecture*
- Eric Evans — *Domain-Driven Design*
- Robert C. Martin — *Clean Architecture*
- Microsoft — *ASP.NET Core Application Architecture*
- Microsoft — *Dependency Injection in ASP.NET Core*

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | 2026-07-29 | Criação da ADR definindo a camada de Services como responsável pela implementação dos casos de uso da aplicação, estabelecendo sua posição entre Controllers e Repositories e padronizando a orquestração das regras de aplicação no Agilium Manager. |