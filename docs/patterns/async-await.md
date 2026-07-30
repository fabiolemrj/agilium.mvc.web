# Padrão Async/Await

# Objetivo

Documentar o padrão de programação assíncrona adotado pelo Agilium Manager, descrevendo as convenções utilizadas na camada MVC, nos serviços e no acesso a dados.

Este documento estabelece diretrizes para implementação de operações assíncronas na solução.

---

# Escopo

Este documento contempla:

- Programação assíncrona
- Controllers
- Services
- Repositories
- Convenções
- Boas práticas

---

# Índice

- Visão Geral
- Arquitetura Assíncrona
- Convenções
- Controllers
- Services
- Repositories
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

O levantamento técnico identificou a utilização de programação assíncrona em diferentes camadas da aplicação, especialmente nos Controllers e Services.

A adoção de métodos assíncronos contribui para melhorar a escalabilidade da aplicação durante operações de acesso a dados e processamento de requisições. :contentReference[oaicite:1]{index=1}

---

# Arquitetura Assíncrona

O fluxo típico de execução segue a sequência:

```text
Controller

↓

Service

↓

Repository

↓

Entity Framework Core / Dapper

↓

Banco de Dados
```

Cada camada deve preservar o comportamento assíncrono durante o processamento da requisição. :contentReference[oaicite:2]{index=2}

---

# Convenções

O levantamento identificou as seguintes convenções:

## Métodos assíncronos

Os métodos assíncronos utilizam o sufixo:

```text
Async
```

Exemplo:

```text
ObterPorIdAsync()
```

Essa convenção é adotada para facilitar a identificação de operações assíncronas na solução. :contentReference[oaicite:3]{index=3}

---

# Controllers

Os Controllers implementam Actions assíncronas utilizando `Task<ActionResult>`.

Exemplo conceitual:

```csharp
public async Task<ActionResult> Index()

public async Task<ActionResult> Create()

public async Task<ActionResult> Edit(int id)
```

A responsabilidade dos Controllers permanece limitada ao fluxo da requisição, delegando o processamento para a camada de negócio. :contentReference[oaicite:4]{index=4}

---

# Services

Os Services também seguem a convenção de métodos assíncronos quando necessário.

Além da lógica de negócio, permanecem responsáveis por:

- validações;
- Notification Pattern;
- comunicação com Repositories.

Os Services implementam interfaces específicas da camada Business. :contentReference[oaicite:5]{index=5}

---

# Repositories

A camada de persistência utiliza:

- Entity Framework Core;
- Dapper.

As operações assíncronas devem ser implementadas respeitando as capacidades de cada tecnologia empregada. :contentReference[oaicite:6]{index=6}

---

# Boas Práticas

Sempre:

- utilizar a convenção `Async` para métodos assíncronos;
- manter a responsabilidade das operações distribuída entre Controller, Service e Repository;
- preservar a separação entre apresentação, negócio e persistência;
- utilizar APIs assíncronas disponibilizadas pelas bibliotecas adotadas quando apropriado.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização de métodos assíncronos;
- convenção de nomenclatura com sufixo `Async`;
- Controllers assíncronos;
- Services assíncronos;
- utilização de Entity Framework Core;
- utilização de Dapper. :contentReference[oaicite:7]{index=7}

Ainda deverão ser validados diretamente no código-fonte:

- adoção do princípio "async all the way";
- uso de `CancellationToken`;
- utilização de `ConfigureAwait(false)`;
- políticas para `.Result` e `.Wait()`;
- diretrizes específicas para operações assíncronas com MongoDB;
- padrões de tratamento de exceções em fluxos assíncronos.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração das convenções de programação assíncrona;
- adoção de novas tecnologias de persistência;
- criação de novos padrões para Controllers ou Services;
- evolução da arquitetura de acesso a dados.

---

# Documentação Relacionada

## Desenvolvimento

- development/coding-standards.md
- development/testing.md

## Arquitetura

- architecture/layers.md
- architecture/overview.md

## Persistência

- database/overview.md