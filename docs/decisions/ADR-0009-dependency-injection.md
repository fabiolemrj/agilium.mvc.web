# ADR-0009 - Estratégia de Injeção de Dependência (Dependency Injection)

| Campo | Valor |
|-------|-------|
| **Status** | Accepted |
| **Data** | 2026-07-29 |
| **Autor** | Equipe Agilium |
| **Versão** | 1.0 |

---

# Contexto

O Agilium Manager possui uma arquitetura em camadas composta por MVC, APIs, Application Services, Domínio, Repositories e Persistência.

Ao longo do crescimento da plataforma, diversos componentes passaram a depender entre si:

- Services
- Repositories
- DbContexts
- Provedores de autenticação
- Serviços de infraestrutura
- Clientes HTTP
- Serviços de cache
- Serviços de arquivos
- Serviços de e-mail
- Logging

A criação manual dessas dependências geraria alto acoplamento, dificultando testes, manutenção e evolução da solução.

Era necessário definir uma estratégia oficial para gerenciamento das dependências da aplicação.

---

# Problema

Instanciar dependências utilizando o operador `new` dentro das classes gera diversos problemas:

- Alto acoplamento;
- Baixa testabilidade;
- Violação do Princípio da Inversão de Dependência (DIP);
- Dificuldade para substituir implementações;
- Código difícil de manter;
- Forte dependência da infraestrutura.

Era necessário desacoplar as implementações concretas das regras de negócio.

---

# Alternativas Consideradas

## Alternativa 1 — Instanciação Manual

```csharp
var repository = new ProdutoRepository();
```

### Vantagens

- Simples.
- Sem infraestrutura adicional.

### Desvantagens

- Alto acoplamento.
- Dificulta testes.
- Difícil manutenção.
- Viola princípios SOLID.

---

## Alternativa 2 — Service Locator

### Vantagens

- Centraliza resolução.

### Desvantagens

- Dependências ocultas.
- Código menos legível.
- Considerado um Anti-Pattern.

---

## Alternativa 3 — Dependency Injection Nativa do ASP.NET Core (Escolhida)

### Vantagens

- Suporte oficial da Microsoft.
- Baixo acoplamento.
- Fácil manutenção.
- Excelente integração com ASP.NET Core.
- Facilita testes unitários.
- Alta extensibilidade.

### Desvantagens

- Exige configuração inicial.
- Necessidade de organização dos registros.

---

# Decisão

Foi adotado o **Container de Dependency Injection nativo do ASP.NET Core** como mecanismo oficial de resolução de dependências.

Todas as dependências da aplicação deverão ser resolvidas exclusivamente através do Container de DI.

A criação manual de dependências utilizando `new` deverá ser evitada, exceto para objetos de domínio simples (Value Objects, DTOs, Models etc.).

---

# Objetivos

Esta estratégia possui os seguintes objetivos:

- Reduzir acoplamento.
- Facilitar manutenção.
- Melhorar testabilidade.
- Permitir substituição de implementações.
- Centralizar configurações.
- Seguir os princípios SOLID.

---

# Fluxo

```text
Controller

↓

Interface

↓

Container DI

↓

Implementação

↓

Repository

↓

DbContext
```

---

# Registro das Dependências

Todas as dependências deverão ser registradas durante a inicialização da aplicação.

Exemplo:

```csharp
services.AddScoped<IProdutoRepository, ProdutoRepository>();

services.AddScoped<IProdutoService, ProdutoService>();

services.AddScoped<IUsuarioService, UsuarioService>();

services.AddScoped<INotificationContext, NotificationContext>();
```

---

# Injeção por Construtor

A única forma recomendada para obtenção de dependências é através da injeção pelo construtor.

Exemplo:

```csharp
public class ProdutoService : IProdutoService
{
    private readonly IProdutoRepository _repository;

    public ProdutoService(IProdutoRepository repository)
    {
        _repository = repository;
    }
}
```

---

# Lifetimes

## Singleton

Uma única instância durante toda a vida da aplicação.

Utilizar para:

- Configurações.
- Serviços sem estado.
- Cache compartilhado.

Exemplo:

```csharp
services.AddSingleton<IConfiguracao, Configuracao>();
```

---

## Scoped (Padrão)

Uma instância por requisição HTTP.

Utilizar para:

- Services.
- Repositories.
- DbContext.
- Unit of Work.

Exemplo:

```csharp
services.AddScoped<IProdutoService, ProdutoService>();
```

---

## Transient

Nova instância a cada resolução.

Utilizar para:

- Objetos leves.
- Estratégias.
- Fábricas.
- Adaptadores.

Exemplo:

```csharp
services.AddTransient<IImportador, ImportadorCsv>();
```

---

# Organização

Recomenda-se centralizar os registros em métodos de extensão.

Estrutura sugerida:

```text
IoC/

├── DependencyInjection.cs

├── RepositoryInjection.cs

├── ServiceInjection.cs

├── InfrastructureInjection.cs

└── AuthenticationInjection.cs
```

Exemplo:

```csharp
public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<IProdutoService, ProdutoService>();
        services.AddScoped<IClienteService, ClienteService>();

        return services;
    }
}
```

---

# Regras

Todas as dependências devem utilizar interfaces.

Exemplo:

```text
Controller

↓

IProdutoService

↓

ProdutoService

↓

IProdutoRepository

↓

ProdutoRepository
```

Não é permitido depender diretamente de implementações concretas quando houver abstração disponível.

---

# O que NÃO fazer

Evitar:

```csharp
var repository = new ProdutoRepository();
```

Evitar:

```csharp
public ProdutoService()
{
    _repository = new ProdutoRepository();
}
```

Evitar:

- Service Locator.
- Dependências estáticas.
- Classes globais.
- Singletons mutáveis.

---

# Testabilidade

A utilização de Dependency Injection permite:

- Mock de interfaces.
- Testes unitários.
- Testes de integração.
- Substituição de implementações.
- Injeção de objetos simulados.

---

# Benefícios

- Baixo acoplamento.
- Código limpo.
- Melhor manutenção.
- Alta testabilidade.
- Reutilização.
- Escalabilidade.
- Aderência ao SOLID.

---

# Desvantagens

- Maior configuração inicial.
- Necessidade de gerenciamento do ciclo de vida das dependências.
- Registro incorreto pode causar erros em tempo de execução.

---

# Riscos

Caso esta estratégia não seja seguida:

- Alto acoplamento.
- Dificuldade para testes.
- Código rígido.
- Dependências ocultas.
- Baixa reutilização.

---

# Impacto

Esta decisão impacta:

- Controllers
- Services
- Repositories
- Infrastructure
- Autenticação
- Logging
- Cache
- Banco de Dados
- Testes

---

# Plano de Implementação

1. Centralizar o registro das dependências.
2. Registrar todos os Services por interface.
3. Registrar todos os Repositories por interface.
4. Configurar corretamente os lifetimes.
5. Remover instanciações manuais.
6. Criar módulos de registro por responsabilidade.
7. Atualizar documentação técnica.

---

# Critérios de Aceitação

Uma implementação é considerada aderente quando:

- Todas as dependências são resolvidas pelo Container de DI.
- Controllers recebem dependências apenas via construtor.
- Services dependem de interfaces.
- Repositories dependem de interfaces quando aplicável.
- Não existem instanciações manuais de serviços ou repositórios.
- Os lifetimes (`Singleton`, `Scoped` e `Transient`) são utilizados corretamente.

---

# ADRs Relacionados

- ADR-0001 — Arquitetura em Camadas
- ADR-0002 — Repository Pattern
- ADR-0003 — Notification Pattern
- ADR-0004 — Entity Framework Core
- ADR-0005 — Estratégia de Autenticação
- ADR-0006 — Estratégia de Autorização
- ADR-0011 — Service Layer
- ADR-0013 — Estratégia de Logging

---

# Referências

- Microsoft — *Dependency Injection in ASP.NET Core*
- Microsoft — *.NET Dependency Injection Guidelines*
- Robert C. Martin — *Clean Architecture*
- Robert C. Martin — *Agile Software Development: Principles, Patterns, and Practices*
- Martin Fowler — *Inversion of Control Containers and the Dependency Injection Pattern*

---

# Histórico

| Versão | Data | Descrição |
|---------|------|-----------|
| **1.0** | 2026-07-29 | Criação da ADR definindo o Container de Dependency Injection nativo do ASP.NET Core como mecanismo oficial para gerenciamento de dependências do Agilium Manager, estabelecendo diretrizes para uso de interfaces, injeção por construtor, organização dos registros e utilização adequada dos lifetimes (`Singleton`, `Scoped` e `Transient`). |