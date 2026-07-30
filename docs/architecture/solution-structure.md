# Estrutura da Solution

## Objetivo

Documentar a organização física da solution do Agilium Manager, descrevendo os projetos que a compõem, suas responsabilidades, convenções estruturais e regras de organização do código.

Este documento serve como referência para o entendimento da solução e para a criação de novos projetos e módulos.

---

# Escopo

Este documento contempla:

- Organização da Solution
- Projetos
- Responsabilidades
- Estrutura de Pastas
- Convenções
- Organização de Código
- Projetos Compartilhados
- Regras para Novos Projetos

---

# Índice

- Visão Geral
- Organização da Solution
- Projetos
- Responsabilidades
- Estrutura Interna
- Convenções de Pastas
- Convenções de Nomenclatura
- Inclusão de Novos Projetos
- Boas Práticas
- Limitações
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager é organizado em múltiplos projetos especializados, separados por responsabilidade e alinhados à arquitetura em camadas.

Essa organização reduz o acoplamento entre componentes e facilita a evolução da solução.

---

# Organização da Solution

Estrutura conceitual:

```text
Agilium Manager Solution

│

├── Presentation
│     ├── MVC
│     └── APIs
│
├── Application
│
├── Domain
│
├── Infrastructure
│
├── Shared
│
└── Tests
```

A estrutura definitiva deverá refletir os projetos existentes na solution.

---

# Projetos

## agilium.mvc.web

**Tipo**

ASP.NET Core MVC

**Responsabilidade**

Aplicação Web responsável pela interface do usuário, processamento das requisições MVC, autenticação, renderização de Views e interação com a camada de aplicação.

---

## agilium-manager-azure-api

**Tipo**

ASP.NET Core Web API

**Responsabilidade**

Exposição dos serviços REST da plataforma.

> A estrutura interna deverá ser documentada após a análise deste projeto.

---

## agilium-pdv-azure-api

**Tipo**

ASP.NET Core Web API

**Responsabilidade**

Serviços REST utilizados pelo ecossistema PDV.

> A estrutura interna deverá ser documentada após o levantamento técnico.

---

## agilium-manager-azure-business

**Tipo**

Class Library

**Responsabilidade**

Camada de aplicação contendo serviços, regras de negócio, interfaces e orquestração dos casos de uso.

A responsabilidade exata deverá ser confirmada durante a análise do projeto.

---

## agilium-manager-git-azure-infra

**Tipo**

Class Library

**Responsabilidade**

Infraestrutura da solução, incluindo persistência, repositórios, integrações e componentes técnicos.

A implementação efetiva deverá ser confirmada durante o levantamento.

---

## Projetos de Teste

Projetos destinados à validação automatizada da solução.

A estratégia de testes deverá ser documentada após a análise da solution.

---

# Estrutura Interna dos Projetos

Embora cada projeto possua particularidades, a organização deve seguir convenções consistentes.

Exemplo:

```text
Controllers/
Services/
Interfaces/
Repositories/
Entities/
Configurations/
Extensions/
Middlewares/
ViewModels/
DTOs/
Mappings/
Validators/
Helpers/
Resources/
```

Nem todas essas pastas necessariamente existem em todos os projetos; a estrutura deve refletir a implementação real.

---

# Convenções de Pastas

## Controllers

Responsáveis pelo recebimento das requisições.

---

## Services

Contêm a lógica de aplicação.

---

## Interfaces

Definem contratos utilizados entre camadas.

---

## Repositories

Encapsulam o acesso aos dados.

---

## Entities

Representam entidades do domínio.

---

## DTOs

Objetos utilizados na comunicação entre camadas.

---

## ViewModels

Modelos utilizados exclusivamente pela camada MVC.

---

## Configuration

Configuração de serviços, autenticação, banco de dados e infraestrutura.

---

## Extensions

Métodos de extensão utilizados pela aplicação.

---

## Middlewares

Componentes responsáveis pelo processamento transversal das requisições.

---

## Mappings

Configurações do AutoMapper e demais mecanismos de mapeamento.

---

## Validators

Validações de entrada e regras auxiliares.

---

# Convenções de Nomenclatura

Projetos:

```text
Agilium.<Camada>.<Módulo>
```

Interfaces:

```text
IProdutoService
IClienteRepository
```

Services:

```text
ProdutoService
ClienteService
```

Repositories:

```text
ProdutoRepository
```

DTOs:

```text
ProdutoDto
ProdutoCreateDto
ProdutoUpdateDto
```

ViewModels:

```text
ProdutoViewModel
```

---

# Inclusão de Novos Projetos

Antes de criar um novo projeto, verificar:

- existe responsabilidade claramente definida?
- já existe um projeto equivalente?
- respeita a arquitetura em camadas?
- introduz dependências desnecessárias?

Todo novo projeto deve possuir documentação própria.

---

# Boas Práticas

Sempre:

- organizar código por responsabilidade;
- manter convenções consistentes;
- evitar dependências circulares;
- documentar novos projetos;
- manter nomes padronizados.

Evitar:

- projetos genéricos;
- responsabilidades duplicadas;
- código compartilhado sem propósito claro;
- mistura de regras de negócio com infraestrutura.

---

# Limitações Conhecidas

O levantamento técnico confirmou detalhadamente apenas a estrutura do projeto **agilium.mvc.web**.

As responsabilidades, organização interna e dependências dos projetos:

- agilium-manager-azure-api;
- agilium-pdv-azure-api;
- agilium-manager-azure-business;
- agilium-manager-git-azure-infra;

deverão ser validadas durante seus respectivos levantamentos arquiteturais.

---

# Atualização

Sempre que houver:

- criação de um novo projeto;
- reorganização da solution;
- alteração de responsabilidades;
- inclusão de novas camadas;

este documento deverá ser atualizado.

---

# Documentação Relacionada

- architecture/overview.md
- architecture/layers.md
- architecture/dependency-flow.md
- patterns/dependency-injection.md
- database/overview.md
- api/overview.md