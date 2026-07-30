# ADR-0001 - Arquitetura em Camadas (Layered Architecture)

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager é uma plataforma responsável por gerenciar diversos módulos de negócio, como clientes, usuários, produtos, vendas, estoque, financeiro, licenciamento e integrações com outros sistemas da suíte Agilium.

O projeto é composto por múltiplos projetos dentro da mesma Solution, incluindo aplicações MVC, APIs, bibliotecas de negócio e infraestrutura. À medida que o sistema evolui, torna-se essencial manter uma arquitetura que favoreça:

- Separação de responsabilidades;
- Baixo acoplamento entre camadas;
- Facilidade de manutenção;
- Reutilização de código;
- Testabilidade;
- Evolução incremental da solução.

Foi necessário definir uma arquitetura padrão para todos os novos módulos e funcionalidades.

---

# Problema

Sem uma arquitetura definida, diferentes desenvolvedores poderiam implementar funcionalidades utilizando abordagens distintas, ocasionando:

- Forte acoplamento entre interface e banco de dados;
- Regras de negócio espalhadas pela aplicação;
- Dificuldade para reutilizar componentes;
- Baixa testabilidade;
- Código duplicado;
- Maior custo de manutenção.

Era necessário estabelecer uma organização arquitetural única para toda a solução.

---

# Alternativas Consideradas

## Alternativa 1 — Arquitetura Monolítica sem separação de camadas

### Vantagens

- Implementação rápida;
- Menor quantidade de projetos.

### Desvantagens

- Alto acoplamento;
- Difícil manutenção;
- Baixa reutilização;
- Regras de negócio misturadas com interface;
- Crescimento desorganizado da solução.

---

## Alternativa 2 — Clean Architecture completa

### Vantagens

- Excelente separação de responsabilidades;
- Alta testabilidade;
- Baixo acoplamento.

### Desvantagens

- Complexidade maior;
- Curva de aprendizado elevada;
- Excesso de abstrações para o porte atual do projeto.

---

## Alternativa 3 — Arquitetura em Camadas (Escolhida)

### Vantagens

- Organização simples;
- Boa separação de responsabilidades;
- Fácil entendimento pela equipe;
- Boa integração com ASP.NET MVC e ASP.NET Core;
- Fácil evolução futura para arquiteturas mais sofisticadas.

### Desvantagens

- Dependências entre camadas precisam ser rigorosamente controladas;
- Requer disciplina para evitar violações arquiteturais.

---

# Decisão

Foi adotada uma **Arquitetura em Camadas (Layered Architecture)** como padrão oficial do Agilium Manager.

Cada camada possui responsabilidades bem definidas e somente pode depender das camadas imediatamente inferiores.

A organização da solução deverá seguir, sempre que possível, a seguinte estrutura:

```text
MVC / Web

↓

API

↓

Application / Services

↓

Business (Domínio)

↓

Repository

↓

Persistence (EF Core / Dapper)

↓

Database
```

Cada camada deverá possuir responsabilidades específicas:

## Interface (MVC / Web)

Responsável por:

- Interface com o usuário;
- Views;
- Controllers MVC;
- Consumo da API;
- Validações de apresentação.

Não deve conter regras de negócio.

---

## API

Responsável por:

- Exposição dos endpoints REST;
- Autenticação;
- Autorização;
- Validação dos Requests;
- Conversão entre DTOs e Services.

Não deve acessar diretamente o banco de dados.

---

## Services

Responsável por:

- Orquestração dos casos de uso;
- Coordenação entre componentes;
- Aplicação das regras de negócio;
- Controle de transações.

---

## Business (Domínio)

Responsável por:

- Regras de negócio;
- Entidades;
- Validações de domínio;
- Objetos de valor;
- Eventos de domínio.

Esta camada representa o núcleo do sistema.

---

## Repository

Responsável exclusivamente pelo acesso aos dados.

Não deve conter regras de negócio.

---

## Persistência

Implementação utilizando:

- Entity Framework Core;
- Dapper (consultas específicas de alta performance).

---

## Banco de Dados

Responsável pela persistência definitiva das informações.

---

# Dependências permitidas

```text
MVC
    ↓
API
    ↓
Services
    ↓
Business
    ↓
Repository
    ↓
Persistence
    ↓
Database
```

Dependências inversas não são permitidas.

Exemplos de violações:

- Repository chamando Controller;
- Business acessando View;
- Controller acessando DbContext diretamente;
- MVC acessando Repository diretamente.

---

# Consequências

## Benefícios

- Código organizado;
- Separação clara de responsabilidades;
- Facilidade para manutenção;
- Melhor reutilização de componentes;
- Maior testabilidade;
- Menor acoplamento;
- Melhor escalabilidade da solução;
- Facilidade para novos desenvolvedores compreenderem o projeto.

---

## Desvantagens

- Maior quantidade de projetos e classes;
- Necessidade de disciplina arquitetural;
- Pequeno aumento na complexidade inicial.

---

## Riscos

Caso as dependências entre camadas não sejam respeitadas, poderá ocorrer:

- Violação da arquitetura;
- Acoplamento excessivo;
- Regras duplicadas;
- Dificuldade para manutenção.

---

# Impacto

Esta decisão impacta diretamente:

- API
- MVC
- Business
- Repository
- Persistência
- Banco de Dados
- Testes
- Documentação
- Novos módulos da solução

---

# Plano de Implementação

- Definir a arquitetura como padrão oficial do projeto.
- Garantir que novos módulos respeitem a separação em camadas.
- Revisar implementações existentes quando necessário.
- Documentar as responsabilidades de cada camada.
- Validar a arquitetura durante Code Reviews.

---

# Critérios de Aceitação

Uma implementação é considerada aderente a esta decisão quando:

- Controllers não acessam diretamente o banco de dados.
- Toda regra de negócio está concentrada na camada Business ou Services.
- Repositories possuem apenas responsabilidade de persistência.
- A interface não contém lógica de negócio.
- As dependências seguem o fluxo definido nesta ADR.
- A documentação arquitetural permanece alinhada com esta decisão.

---

# ADRs Relacionados

- ADR-0002 — Repository Pattern
- ADR-0003 — Notification Pattern
- ADR-0004 — Entity Framework Core
- ADR-0009 — Dependency Injection
- ADR-0011 — Service Layer

---

# Referências

- Martin Fowler — *Patterns of Enterprise Application Architecture*
- Eric Evans — *Domain-Driven Design*
- Microsoft — *ASP.NET Core Architecture Guide*
- Clean Architecture — Robert C. Martin

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| 1.0 | 2026-07-29 | Criação da ADR definindo a Arquitetura em Camadas como padrão oficial do Agilium Manager. |