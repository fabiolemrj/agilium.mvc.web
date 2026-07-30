# Dependency Injection

# Objetivo

Documentar a arquitetura de Injeção de Dependência utilizada pelo Agilium Manager, descrevendo a configuração do container, o registro dos serviços, a organização das dependências e as convenções adotadas na solução.

---

# Escopo

Este documento contempla:

- Arquitetura de Dependency Injection
- Registro de Dependências
- Organização das Configurações
- Lifetimes
- Convenções
- Boas Práticas

---

# Índice

- Visão Geral
- Arquitetura de Dependency Injection
- Registro das Dependências
- Configuração da Aplicação
- Organização das Dependências
- Lifetimes
- Convenções
- Boas Práticas
- Limitações Conhecidas
- Atualização
- Documentação Relacionada

---

# Visão Geral

O Agilium Manager utiliza o container nativo de **Dependency Injection** do ASP.NET Core para gerenciamento das dependências da aplicação.

A configuração é centralizada durante a inicialização da aplicação, permitindo que Controllers, Services e demais componentes recebam suas dependências por meio de Injeção de Dependência (Constructor Injection). :contentReference[oaicite:2]{index=2}

---

# Arquitetura de Dependency Injection

A resolução das dependências segue a arquitetura:

```text
Startup

↓

ConfigureServices

↓

ResolveDependencyConfig

↓

Dependency Injection Container

↓

Controllers

↓

Services

↓

Repositories
```

Essa organização centraliza a configuração da aplicação e reduz o acoplamento entre os componentes. :contentReference[oaicite:3]{index=3}

---

# Registro das Dependências

O levantamento técnico identificou que o registro das dependências está centralizado em:

```text
Configuration/
└── ResolveDependencyConfig.cs
```

Durante a inicialização da aplicação, o `Startup.cs` invoca esse componente para registrar os serviços utilizados pela solução. :contentReference[oaicite:4]{index=4}

---

# Configuração da Aplicação

Além do registro dos serviços da aplicação, o `Startup.cs` configura diversos componentes da infraestrutura, incluindo:

- `AddDbContext<AgiliumContext>()`;
- `AddControllersWithViews()`;
- `AddSingleton<IHttpContextAccessor>()`;
- `ResolveDependencies()`;
- `AddIdentityConfiguration()`;
- `AddRazorPages()`;
- `AddMvcConfiguration()`;
- `AddLogging()`;
- `AddAutoMapper()`;
- `AddSession()`.

Esses registros compõem a configuração inicial da camada de apresentação. :contentReference[oaicite:5]{index=5}

---

# Organização das Dependências

A Injeção de Dependência é utilizada em diferentes camadas da aplicação.

## Controllers

Os Controllers recebem suas dependências por construtor.

O `MainController` concentra dependências compartilhadas, como:

- `INotificador`;
- `IMapper`;
- `IUser`;
- `ILogService`;
- `ILicencaService`;
- `IAuthService`;
- `IConfiguration`;
- `IUtilDapperRepository`.

Os Controllers específicos herdam esse comportamento e recebem adicionalmente os Services necessários ao domínio correspondente. :contentReference[oaicite:6]{index=6}

---

## Services

Os Services são registrados no container e consumidos pelos Controllers através de suas interfaces.

Cada Service implementa uma interface localizada na camada Business, preservando o desacoplamento entre apresentação e regras de negócio. :contentReference[oaicite:7]{index=7}

---

## Repositories

Os Repositories também são registrados por meio de interfaces, sendo consumidos pela camada Business.

Essa abordagem favorece testabilidade e desacoplamento entre negócio e persistência. :contentReference[oaicite:8]{index=8}

---

# Lifetimes

O levantamento técnico confirma os seguintes registros:

| Registro | Observação |
|----------|------------|
| `AddDbContext<AgiliumContext>()` | Registro do contexto de persistência via ASP.NET Core |
| `AddSingleton<IHttpContextAccessor>()` | Instância única compartilhada |
| Demais Services e Repositories | Registrados em `ResolveDependencyConfig.cs` |

A definição exata dos lifetimes (`Transient`, `Scoped` e `Singleton`) para cada serviço deverá ser obtida por inspeção direta de `ResolveDependencyConfig.cs`. :contentReference[oaicite:9]{index=9}

---

# Convenções

A arquitetura de Injeção de Dependência segue as seguintes diretrizes:

- registrar dependências de forma centralizada;
- consumir componentes por interfaces;
- utilizar Injeção de Dependência por construtor;
- evitar criação manual de dependências (`new`);
- manter o desacoplamento entre Controllers, Services e Repositories.

---

# Boas Práticas

Sempre:

- registrar novos componentes em `ResolveDependencyConfig.cs`;
- depender de abstrações (interfaces);
- utilizar Constructor Injection;
- manter responsabilidades bem definidas por camada;
- revisar os registros ao incluir novos módulos.

Evitar:

- dependências concretas quando houver interface correspondente;
- resolução manual de serviços;
- duplicação de registros;
- acoplamento entre camadas.

---

# Limitações Conhecidas

O levantamento técnico confirmou:

- utilização do container nativo do ASP.NET Core;
- centralização do registro em `ResolveDependencyConfig.cs`;
- utilização de Constructor Injection;
- registro do `AgiliumContext`;
- registro de `IHttpContextAccessor`;
- registro do AutoMapper;
- registro do Identity;
- registro da Session;
- registro dos Services e Repositories. :contentReference[oaicite:10]{index=10}

Ainda deverão ser documentados mediante análise de `ResolveDependencyConfig.cs`:

- inventário completo dos serviços registrados;
- lifetime individual de cada registro;
- organização interna dos módulos de registro;
- padrões específicos para novos registros.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- inclusão de novos serviços;
- alteração da estratégia de Injeção de Dependência;
- reorganização de `ResolveDependencyConfig.cs`;
- adoção de novos módulos ou frameworks de DI.

---

# Documentação Relacionada

## Arquitetura

- architecture/overview.md
- architecture/layers.md

## Desenvolvimento

- development/coding-standards.md

## Persistência

- database/overview.md
- database/repositories.md