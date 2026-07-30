# Agilium Manager MVC Web — Technical Reference

> **Levantamento de contexto completo do projeto `agilum.mvc.web`**
> Gerado em: 2026-07-28 | Status: Análise concluída

---

## 1. Arquitetura Geral

### 1.1 Tipo de Arquitetura

O projeto adota uma **arquitetura em camadas (Layered Architecture)** com padrão **MVC** para o frontend, combinada com uma camada de negócio separada e uma camada de infraestrutura para acesso a dados.

### 1.2 Diagrama de Fluxo de Requisição

```
Browser (AdminLTE + jQuery + AJAX)
        │
        ▼
┌──────────────────────────────────────┐
│  Middleware Pipeline                  │
│  UseStaticFiles → UseSession →       │
│  UseAuthentication → UseAuthorization│
│  EmpresaSelecionadaMiddleware →      │
│  ExceptionMiddleware                 │
└──────────────────────────────────────┘
        │
        ▼
┌──────────────────────────────────────┐
│  Controller (agilum.mvc.web)         │
│  Herda de MainController             │
│  [Authorize] + [ClaimsAuthorize(id)] │
│  Injeção: Services, IMapper, IUser   │
└──────────────────────────────────────┘
        │
        ▼
┌──────────────────────────────────────┐
│  Business Services                   │
│  (agilium-manager-azure-business)    │
│  BaseService → *Service              │
│  INotificador (Notification Pattern) │
│  Validações de negócio               │
└──────────────────────────────────────┘
        │
        ▼
┌──────────────────────────────────────┐
│  Repository Layer                    │
│  (agilium-manager-git-azure-infra)   │
│  Repository<T> genérico + *Repository│
│  Dapper (consultas complexas)        │
│  Entity Framework Core 3.1           │
│  MongoDB (fotos de usuário)          │
└──────────────────────────────────────┘
        │
        ▼
┌──────────────────────────────────────┐
│  Banco de Dados                      │
│  MySQL 8.0 (principal)               │
│  Pomelo.EntityFrameworkCore.MySql    │
│  MongoDB (documentos/fotos)          │
└──────────────────────────────────────┘
```

### 1.3 Responsabilidades dos Projetos

| Projeto | Camada | Responsabilidade |
|---------|--------|------------------|
| `agilum.mvc.web` | Apresentação | Controllers, Views, ViewModels, Identity UI, Autenticação Cookie, DI, Configuração |
| `agilium-manager-azure-business` | Negócio | Models de domínio, Interfaces, Services, Enums, Notificações, Validações |
| `agilium-manager-git-azure-infra` | Infraestrutura | DbContext (AgiliumContext), Repositories (EF Core + Dapper), Mapeamentos, MongoDB |
| `agilium-manager-azure-api` | API REST | API REST versionada para consumo externo (não é foco desta análise) |
| `agilium-pdv-azure-api` | API PDV | API REST específica para módulo PDV (não é foco desta análise) |
| `agilum.mvc.web.tests` | Testes | Testes unitários com xUnit |

### 1.4 Dependências Entre Projetos

```
agilum.mvc.web
  ├── agilium-manager-azure-business (referência de projeto)
  └── agilium-manager-git-azure-infra (referência de projeto)
```

---

## 2. Estrutura da Solução — `agilum.mvc.web`

| Pasta | Responsabilidade |
|-------|------------------|
| `Controllers/` | 28 controllers MVC, todos herdando de `MainController` |
| `Views/` | Views Razor organizadas por controller (uma pasta por entidade) |
| `ViewModels/` | ViewModels/DTOs organizados por entidade (uma pasta por domínio) |
| `Areas/` | Área `Identity` (Razor Pages do Identity UI) |
| `Configuration/` | Classes de configuração: Identity, AutoMapper, DI, MVC, Globalization |
| `Services/` | Serviços específicos do MVC: `AutenticacaoService`, `AuthService`, `ServiceEmail`, `PassCrypto` |
| `Extensions/` | Middlewares, classes de extensão, helpers: `CustomAuth`, `AspNetUser`, `ExceptionMiddleware`, `EmpresaSelecionadaMiddleware`, `PaginacaoViewComponent` |
| `Interfaces/` | Interfaces locais: `IAutenticacaoService`, `IImportarXMLNfe` |
| `Data/` | DbContext do Identity (`dbIdentityContext`) + RefreshToken |
| `Enums/` | Enum `Enums.cs` |
| `wwwroot/` | Assets estáticos: AdminLTE (dist/), CSS customizado, JS, lib/, imagens |
| `Properties/` | `launchSettings.json` |
| `.config/` | Configurações adicionais de runtime |

---

## 3. Convenções do Projeto

### 3.1 Nomenclatura

| Elemento | Convenção | Exemplo |
|----------|-----------|---------|
| Controllers | `{Entidade}Controller`, PascalCase | `ProdutoController` |
| Views | Pasta = nome do controller, PascalCase | `Views/Produto/Index.cshtml` |
| ViewModels | `{Entidade}{Acao}ViewModel`, PascalCase | `ProdutoViewModel`, `ClienteIndexViewModel` |
| Services (negócio) | `I{Entidade}Service` / `{Entidade}Service` | `IProdutoService`, `ProdutoService` |
| Repositories | `I{Entidade}Repository` / `{Entidade}Repository` | `IProdutoRepository`, `ProdutoRepository` |
| Models | PascalCase, representa tabela | `Produto`, `Cliente`, `Venda` |
| Métodos | PascalCase, verbos de ação | `ObterPorId()`, `Adicionar()` |
| Async | Sufixo `Async` nos métodos | `ObterPorIdAsync()` |
| Interfaces | Prefixo `I` | `INotificador`, `IUser`, `IMapper` |
| Namespaces | `agilum.mvc.web.{Pasta}` (MVC) / `agilium.api.business.{Pasta}` (Business) | |

### 3.2 Padrão de Controller

- Todo controller herda de **`MainController`** (abstrato)
- `MainController` herda de `Controller` e fornece:
  - `INotificador` → validações (Notification Pattern)
  - `IConfiguration` → acesso a settings
  - `IMapper` → AutoMapper
  - `IUser` → usuário autenticado
  - `IUtilDapperRepository` → geração de IDs e queries utilitárias
  - `ILogService` → logging
  - `ILicencaService` → verificação de licença
  - `IAuthService` → autenticação customizada
- Atributos: `[Authorize]` no controller, `[ClaimsAuthorize(idTag)]` por ação
- Rotas customizadas com `[Route("produto")]`

### 3.3 Padrão de Service (Camada de Negócio)

- Serviços em `agilium-manager-azure-business/Services/`
- Herdam de `BaseService` que provê `INotificador`
- Cada serviço implementa uma interface em `Interfaces/IService/`
- Injeção de dependência via construtor
- Retornam models de domínio (não ViewModels)
- Validações de negócio disparam notificações via `Notificar()`

### 3.4 Padrão de Repository

- Repositórios em `agilium-manager-git-azure-infra/Repository/`
- `Repository<T>` genérico com operações CRUD básicas
- Repositórios específicos herdam de `Repository<T>` e implementam interface
- **Dapper** usado para consultas complexas em `Repository/Dapper/`
- **MongoDB** para `UsuarioFoto` via `RepositoryMongo.cs`

---

## 4. Tecnologias Utilizadas

### 4.1 Stack Principal

| Tecnologia | Versão | Uso |
|------------|--------|-----|
| .NET Core | 3.1 | Runtime |
| ASP.NET Core MVC | 3.1 | Framework web |
| Entity Framework Core | 3.1.32 | ORM principal |
| Pomelo MySQL | 3.2.7 | Provider EF Core para MySQL |
| MySQL | 8.0.19 | Banco de dados relacional |
| MongoDB Driver | 2.22.0 (API) | Banco NoSQL para fotos |
| Dapper | (via infra) | Micro-ORM para queries complexas |

### 4.2 Autenticação e Identidade

| Tecnologia | Uso |
|------------|-----|
| ASP.NET Core Identity | Gerenciamento de usuários, roles, claims |
| `AddIdentityCore` | Identity sem UI padrão conflitante |
| Cookie Authentication | Esquema padrão `CookieAuthenticationDefaults` |
| `SignInManager`, `UserManager` | Gerenciamento de sessão |
| `dbIdentityContext` | DbContext separado para tabelas Identity |
| `CaUsuarioIdentity` | Entidade de usuário customizada |
| BouncyCastle 1.8.9 | Criptografia |

### 4.3 Frontend

| Tecnologia | Uso |
|------------|-----|
| AdminLTE 3.x | Template administrativo (Bootstrap 4) |
| Bootstrap | Framework CSS |
| jQuery | Manipulação DOM e AJAX |
| Toastr | Notificações toast |
| DataTables | Tabelas interativas |
| Select2 | Dropdowns avançados |
| Inputmask | Máscaras de input |

### 4.4 Bibliotecas Adicionais

| Biblioteca | Versão | Uso |
|------------|--------|-----|
| AutoMapper | 8.1.1 | Mapeamento Model ↔ ViewModel |
| Polly | 6.0.36 | Resiliência HTTP (retry, circuit breaker) |
| QRCoder | 1.4.3 | Geração de QR Codes |
| ZXing.Net | 0.16.11 | Leitura de códigos de barras |
| SixLabors.ImageSharp | 2.1.7 | Manipulação de imagens |
| System.DirectoryServices | 6.0.0 | Integração Active Directory |
| Newtonsoft.Json | (via API) | Serialização JSON |

---

## 5. Configuração da Aplicação

### 5.1 Startup.cs — `ConfigureServices`

```
AddControllers (JSON sem camelCase)
  → AddDbContext<AgiliumContext> (MySQL via Pomelo)
  → AddControllersWithViews
  → AddSingleton<IHttpContextAccessor>
  → ResolveDependencies (DI de todos serviços/repositórios)
  → AddIdentityConfiguration (Identity Core + Cookie Auth)
  → AddRazorPages
  → AddMvcConfiguration (model binding messages pt-BR + AntiForgery)
  → AddLogging (Console + Debug)
  → AddAutoMapper
  → AddSession (3h timeout, HttpOnly, Essential)
```

### 5.2 Startup.cs — `Configure` (Pipeline)

```
Dev: UseDeveloperExceptionPage + UseDatabaseErrorPage
Prod: UseExceptionHandler("/Error") + UseHsts
  → Log headers
  → UseHttpsRedirection (exceto Render)
  → UseStaticFiles
  → UseRouting
  → UseSession
  → UseAuthentication
  → UseAuthorization
  → EmpresaSelecionadaMiddleware
  → ExceptionMiddleware
  → Cultura pt-BR (decimal separator ",", date "dd/MM/yyyy")
  → Endpoints: RazorPages, Controllers, Areas, default route
```

### 5.3 appsettings.json

```json
{
  "ConnectionStrings": {
    "ConnectionDb": "",         // MySQL principal (via env var)
    "dbIdentityContextConnection": "",  // MySQL Identity
    "versaobd-major": "8",     // Usado para validação de schema
    "versaobd-minor": "0",
    "versaobd-build": "19"
  },
  "EmailSettings": { ... },    // Configuração de e-mail
  "CardapioDigital": { ... },  // Integração cardápio digital
  "AppTokenSettings": { "RefreshTokenExpiration": 8 }
}
```

> **IMPORTANTE:** Connection strings são injetadas via **variáveis de ambiente** em produção. O método `ObterConnectionString()` em `Startup.cs` implementa fallback: `appsettings.json` → env var direta → `ConnectionStrings__{name}`.

### 5.4 Program.cs

- `Host.CreateDefaultBuilder` → `ConfigureWebHostDefaults`
- Kestrel: `MaxRequestHeadersTotalSize = 65536`
- Porta: variável de ambiente `PORT` ou fallback `5000`
- Detecta ambiente Render para desabilitar HTTPS redirect

---

## 6. Fluxo de Autenticação

### 6.1 Diagrama

```
[Login Page]  (/Identity/Account/Login)
      │
      ▼
SignInManager.PasswordSignInAsync()
      │
      ▼
Cookie Authentication (CookieAuthenticationDefaults)
      │
      ▼
Claims: UserId, Email, Roles
      │
      ▼
EmpresaSelecionadaMiddleware
  → Usuário deve selecionar uma empresa
  → Empresa armazenada na Session
      │
      ▼
[Authorize] nos Controllers
  → CustomAuthorization: ClaimsAuthorizeAttribute(idTag)
  → ICaService.UsuarioTemPermissao(idUsuario, idTag)
```

### 6.2 Componentes

| Componente | Localização |
|------------|-------------|
| Identity UI | `Areas/Identity/` (Razor Pages) |
| `dbIdentityContext` | `Data/dbIdentityContext.cs` |
| `CaUsuarioIdentity` | `agilium-manager-azure-business/Models/` |
| `IdentityConfig` | `Configuration/IdentityConfig.cs` |
| `AspNetUser` (IUser) | `Extensions/AspNetUser.cs` |
| `CustomAuthorization` | `Extensions/CustomAuth.cs` |
| `AuthService` | `Services/AuthService.cs` |
| `EmpresaSelecionadaMiddleware` | `Extensions/EmpresaSelecionadaMiddleware.cs` |
| `CaService` | `agilium-manager-azure-business/Services/` |

### 6.3 Configuração de Senha

- `RequireDigit = true`
- `RequiredLength = 6`
- `RequireNonAlphanumeric = false`
- `RequireUppercase = false`
- `RequireLowercase = false`
- Lockout: 5 tentativas, 5 minutos

---

## 7. Controllers

### 7.1 Lista Completa (28 controllers)

| Controller | Rota | Domínio |
|------------|------|---------|
| `CaixaController` | — | Abertura/fechamento de caixa |
| `CategoriaFinanceiraController` | — | Categorias financeiras |
| `ClienteController` | — | Clientes (PF/PJ) |
| `CompraController` | — | Compras e NFe |
| `ConfigController` | — | Configurações do sistema |
| `ContaController` | — | Contas a pagar/receber |
| `DevolucaoController` | — | Devoluções |
| `EmpresaController` | — | Empresas (multi-empresa) |
| `EnderecoController` | — | Endereços |
| `EstoqueController` | — | Estoque e movimentações |
| `FormaPagamentoController` | — | Formas de pagamento |
| `FornecedorController` | — | Fornecedores |
| `FuncionarioController` | — | Funcionários |
| `HomeController` | `/` e `/licenca` | Dashboard e licença |
| `InventarioController` | — | Inventário |
| `LicencaController` | — | Gerenciamento de licenças |
| `LogController` | — | Logs do sistema |
| `MainController` | (base abstrata) | Controller base |
| `MoedaController` | — | Moedas e cotações |
| `PerdaController` | — | Perdas de produtos |
| `PlanoContaController` | — | Plano de contas |
| `PontoVendaController` | — | Pontos de venda |
| `ProdutoController` | `[Route("produto")]` | Produtos (mais complexo) |
| `TurnoController` | — | Turnos |
| `UnidadeController` | — | Unidades de medida |
| `UsuarioController` | — | Usuários e permissões |
| `ValeController` | — | Vales |
| `VendaController` | — | Vendas/PDV |

### 7.2 Estrutura Padrão de Controller

```csharp
[Route("entidade")]              // opcional
[Authorize]                      // sempre presente
public class EntidadeController : MainController
{
    // Serviços injetados como private readonly
    private readonly IEntidadeService _entidadeService;
    
    public EntidadeController(
        IEntidadeService service,
        INotificador notificador,
        IConfiguration configuration,
        IUser appUser,
        IUtilDapperRepository utilDapperRepository,
        ILogService logService,
        IMapper mapper,
        ILicencaService licencaService,
        IAuthService authService
    ) : base(notificador, configuration, appUser, utilDapperRepository, 
             logService, mapper, licencaService, authService)
    { }
    
    public async Task<ActionResult> Index() { ... }
    public async Task<ActionResult> Create() { ... }
    [HttpPost] public async Task<ActionResult> Create(ViewModel model) { ... }
    public async Task<ActionResult> Edit(int id) { ... }
}
```

---

## 8. Services (Camada de Negócio)

### 8.1 Organização

Localizados em `agilium-manager-azure-business/Services/`, os serviços são o coração da lógica de negócio:

- **40+ serviços** cobrindo todos os domínios
- Todos herdam de `BaseService`
- Implementam interfaces em `Interfaces/IService/`
- `BaseService` fornece acesso ao `INotificador`

### 8.2 Serviços MVC-Específicos (em `agilum.mvc.web/Services/`)

| Serviço | Responsabilidade |
|---------|------------------|
| `AutenticacaoService` | Logout, refresh token, validação de token |
| `AuthService` | Autenticação customizada (substitui SignInManager) |
| `ServiceEmail` | Envio de e-mails, criptografia de config |
| `PassCrypto` | Criptografia de senhas |
| `CodigoProdutoGenerator` | Geração de códigos de produto |
| `ListasAuxilares` | Listas auxiliares para dropdowns |
| `Utils` | Utilitários gerais |

---

## 9. Repositories (Camada de Infraestrutura)

### 9.1 Organização

Localizados em `agilium-manager-git-azure-infra/Repository/`:

- **`Repository<T>`**: genérico com CRUD básico via EF Core
- **40+ repositórios específicos** herdando de `Repository<T>`
- **Dapper**: `Repository/Dapper/` para consultas SQL otimizadas
- **MongoDB**: `RepositoryMongo.cs` e `UsuarioFotoRepositoryMongo.cs`

### 9.2 Padrão de Acesso a Dados

| Abordagem | Quando Usar |
|-----------|-------------|
| EF Core (`Repository<T>`) | CRUD padrão, operações simples |
| Dapper | Consultas complexas, relatórios, performance crítica |
| MongoDB | Documentos (fotos de usuário) |

---

## 10. Banco de Dados

### 10.1 Tecnologias

| Banco | Provider | Uso |
|-------|----------|-----|
| MySQL 8.0 | Pomelo.EntityFrameworkCore.MySql 3.2.7 | Dados relacionais principais |
| MongoDB | MongoDB.Driver 2.22.0 (na API) | Fotos de usuário e documentos |
| SQLite | EF Core Sqlite 3.1.32 | (referenciado, uso pontual) |
| SQL Server | EF Core SqlServer 3.1.32 | (referenciado, uso pontual) |

### 10.2 DbContexts

| DbContext | Projeto | Finalidade |
|-----------|---------|------------|
| `AgiliumContext` | `agilium-manager-git-azure-infra` | Dados de negócio (~100+ tabelas) |
| `dbIdentityContext` | `agilum.mvc.web` | Tabelas Identity (aspnetusers, aspnetroles, etc.) |

### 10.3 Models (Entidades)

**100+ models** em `agilium-manager-azure-business/Models/` cobrindo:
- Produtos, Compras, Vendas, Estoque
- Clientes, Fornecedores, Funcionários, Usuários
- Financeiro (contas, plano de contas, categorias, moedas)
- Fiscal (CFOP, CST, CSOSN, CEST, NCM, IBPT)
- PDV (caixa, turno, vale, forma de pagamento)
- Configurações, Licenças, Logs

---

## 11. ViewModels

### 11.1 Organização

Localizados em `agilum.mvc.web/ViewModels/`, organizados por domínio em subpastas:

```
ViewModels/
├── Caixa/          ├── Empresa/        ├── Moedas/
├── CategeoriaFinanceira/  ├── EmpresaUsuario/  ├── Perda/
├── Cliente/        ├── Endereco/       ├── PlanoConta/
├── Compra/         ├── Estoque/        ├── PontoVenda/
├── Config/         ├── FormaPagamento/ ├── Produtos/
├── Conta/          ├── Fornecedor/     ├── Turno/
├── Contato/        ├── Funcionarios/   ├── UnidadeViewModel/
├── Devolucao/      ├── Impostos/       ├── Usuarios/
│                   ├── Inventario/     ├── Vale/
│                   ├── Licenca/        ├── Venda/
│                   ├── Log/            └── VendaRepot/
```

### 11.2 Padrão

- Sufixo `ViewModel`: `ProdutoViewModel`, `ClienteIndexViewModel`
- Mapeamento via AutoMapper no `AutomapperConfig.cs`
- Data Annotations para validação (`[Required]`, `[StringLength]`, etc.)

---

## 12. Views

### 12.1 Organização

```
Views/
├── _ViewImports.cshtml     # @using global + TagHelpers
├── _ViewStart.cshtml        # Layout = "_main"
├── Shared/
│   ├── _main.cshtml         # Layout principal AdminLTE
│   ├── _Layout.cshtml       # Layout alternativo
│   ├── _ASideMenu.cshtml    # Menu lateral
│   ├── _LoginPartial.cshtml # Header login
│   ├── _rodape.cshtml       # Rodapé
│   ├── _ValidationScriptsPartial.cshtml
│   └── Components/          # View Components
├── Home/                    # Views do Home
├── Produto/                 # Views de Produto
├── ... (uma pasta por controller)
```

### 12.2 Layout

- **Template AdminLTE 3.x** como base
- Layout principal: `_main.cshtml`
- Menu lateral dinâmico: `_ASideMenu.cshtml`
- Bootstrap 4 + jQuery

---

## 13. Front-end

### 13.1 Assets Estáticos (`wwwroot/`)

```
wwwroot/
├── css/
│   ├── site.css              # Estilos customizados
│   └── toastr.css            # Notificações
├── dist/                     # AdminLTE (CSS, JS, imagens)
├── js/                       # Scripts customizados
├── lib/                      # Bibliotecas de terceiros
├── Images/                   # Imagens do sistema
├── imagens-cardapio/         # Imagens do cardápio digital
├── local/                    # Assets localizados
├── font/                     # Fontes
└── favicon.ico
```

### 13.2 Bibliotecas Front-end

| Biblioteca | Uso |
|------------|-----|
| AdminLTE 3.x | Template admin (Bootstrap 4) |
| Bootstrap 4 | Framework CSS |
| jQuery | DOM, AJAX, plugins |
| DataTables | Tabelas paginadas, ordenáveis |
| Select2 | Dropdowns com busca |
| Toastr | Notificações toast |
| Inputmask | Máscaras (CPF, CNPJ, telefone, etc.) |
| Chart.js | Gráficos no dashboard |

---

## 14. Validação

### 14.1 Camadas de Validação

| Camada | Mecanismo |
|--------|-----------|
| Client-side | jQuery Validation + Data Annotations (unobtrusive) |
| Server-side (MVC) | `ModelState.IsValid` + Data Annotations |
| Server-side (Negócio) | Notification Pattern via `INotificador` |

### 14.2 Notification Pattern

```csharp
// No serviço de negócio
if (produto == null)
{
    Notificar("Produto não encontrado.");
    return null;
}

// No controller
if (!OperacaoValida())
{
    var erros = ObterNotificacoes();
    // Retornar erros para a view
}
```

### 14.3 AntiForgery

- `[AutoValidateAntiforgeryToken]` global via `MvcConfig`
- Proteção contra CSRF em todos os formulários POST

---

## 15. Tratamento de Erros

### 15.1 Estratégia

| Mecanismo | Escopo |
|-----------|--------|
| `try/catch` nos controllers | Erros específicos de ação |
| `ExceptionMiddleware` | Erros não tratados (global) |
| `CustomHttpRequestException` | Erros de requisição HTTP |
| Log via `ILogService` | Registro de erros no banco |
| `UseDeveloperExceptionPage` | Dev apenas |
| `UseExceptionHandler("/Error")` | Produção |

### 15.2 Páginas de Erro

- `/Error` — página de erro genérica
- `/sistema-indisponivel` — sistema indisponível
- `/Identity/Account/AccessDenied` — acesso negado

---

## 16. Segurança

| Recurso | Implementação |
|---------|---------------|
| Autenticação | ASP.NET Core Identity + Cookie Auth |
| Autorização | `[Authorize]` + `ClaimsAuthorizeAttribute` |
| CSRF | `[AutoValidateAntiforgeryToken]` global |
| Cookies | HttpOnly, Essential, SecurePolicy SameAsRequest |
| Session | 3h timeout, HttpOnly |
| Senhas | Identity Core com BouncyCastle |
| Headers | HSTS em produção (exceto Render) |
| Connection Strings | Variáveis de ambiente (nunca hardcoded) |

---

## 17. Dependências Externas

| Integração | Descrição |
|------------|-----------|
| E-mail (SMTP) | `ServiceEmail` + `EmailSettings` |
| Cardápio Digital | API REST separada (`CardapioDigital` config) |
| NFe (XML) | Importação de XML de Nota Fiscal |
| Active Directory | `System.DirectoryServices` (referenciado) |
| Site Mercado | Integração com marketplace (models e services) |
| Polly | Resiliência para chamadas HTTP externas |

---

## 18. Padrões Arquiteturais Identificados

| Padrão | Onde é Aplicado |
|--------|-----------------|
| **MVC** | Estrutura base do `agilum.mvc.web` |
| **Repository Pattern** | `agilium-manager-git-azure-infra/Repository/` |
| **Service Layer** | `agilium-manager-azure-business/Services/` |
| **Dependency Injection** | Nativo do ASP.NET Core, `ResolveDependencyConfig.cs` |
| **Notification Pattern** | `agilium-manager-azure-business/Notificacoes/` |
| **DTO/ViewModel** | `agilum.mvc.web/ViewModels/` |
| **AutoMapper** | `Configuration/AutomapperConfig.cs` |
| **Middleware Pipeline** | `EmpresaSelecionadaMiddleware`, `ExceptionMiddleware` |
| **Base Controller** | `MainController` abstrato |
| **Base Service** | `BaseService` abstrato |
| **Unit of Work** | Implícito via EF Core DbContext (Scoped) |
| **SOLID** | Interfaces segregadas, DI, Repository genérico |

---

## 19. Fluxo Completo de uma Funcionalidade — Exemplo: Criar Produto

```
[View] Views/Produto/Create.cshtml
  → Form POST com AntiForgery
       ↓
[Controller] ProdutoController.Create(ProdutoViewModel model)
  → Verifica ModelState.IsValid
  → Mapeia ViewModel → Model (AutoMapper)
  → Chama _produtoService.Adicionar(produto)
       ↓
[Service] ProdutoService.Adicionar(Produto produto)
  → Valida regras de negócio
  → Notificar("Produto já existe") se duplicado
  → Chama _produtoRepository.Adicionar(produto)
       ↓
[Repository] ProdutoRepository (EF Core)
  → DbSet<Produto>.Add(produto)
  → SaveChanges()
       ↓
[Banco] MySQL → INSERT INTO produto (...)
       ↓
[Retorno] Controller verifica OperacaoValida()
  → Se OK: RedirectToAction("Index")
  → Se erro: Retorna View(model) com notificações
       ↓
[View] Exibe mensagens de sucesso/erro via TempData
```

### Arquivos Envolvidos

| Camada | Arquivos |
|--------|----------|
| View | `Views/Produto/Create.cshtml` |
| ViewModel | `ViewModels/Produtos/ProdutoViewModel.cs` |
| Controller | `Controllers/ProdutoController.cs` |
| Interface Service | `agilium-manager-azure-business/Interfaces/IService/IProdutoService.cs` |
| Service | `agilium-manager-azure-business/Services/ProdutoService.cs` |
| Model | `agilium-manager-azure-business/Models/Produto.cs` |
| Interface Repository | `agilium-manager-azure-business/Interfaces/IRepository/IProdutoRepository.cs` |
| Repository | `agilium-manager-git-azure-infra/Repository/ProdutoReposiotry.cs` |
| DbContext | `agilium-manager-git-azure-infra/Context/AgiliumContext.cs` |
| AutoMapper | `Configuration/AutomapperConfig.cs` |
| DI | `Configuration/ResolveDependencyConfig.cs` |

---

## 20. Guia para Novos Desenvolvimentos

### 20.1 Onde Criar Cada Artefato

| Artefato | Local |
|----------|-------|
| **Controller** | `Controllers/{Nome}Controller.cs` |
| **View (Index)** | `Views/{Nome}/Index.cshtml` |
| **View (Create/Edit)** | `Views/{Nome}/Create.cshtml`, `Edit.cshtml` |
| **ViewModel** | `ViewModels/{Nome}/{Nome}ViewModel.cs` |
| **Model (domínio)** | `agilium-manager-azure-business/Models/{Nome}.cs` |
| **Interface Service** | `agilium-manager-azure-business/Interfaces/IService/I{Nome}Service.cs` |
| **Service** | `agilium-manager-azure-business/Services/{Nome}Service.cs` |
| **Interface Repository** | `agilium-manager-azure-business/Interfaces/IRepository/I{Nome}Repository.cs` |
| **Repository** | `agilium-manager-git-azure-infra/Repository/{Nome}Repository.cs` |
| **Enum** | `agilium-manager-azure-business/Enums/E{Nome}.cs` |
| **Extensão/Helper** | `Extensions/{Nome}.cs` |
| **Mapeamento AutoMapper** | `Configuration/AutomapperConfig.cs` |
| **Registro DI** | `Configuration/ResolveDependencyConfig.cs` |

### 20.2 Sequência de Implementação

1. **Model** → Criar entidade de domínio no Business
2. **Enum** → Criar enums se necessário
3. **Interface Repository** → Definir contrato de dados
4. **Repository** → Implementar acesso a dados (EF Core / Dapper)
5. **Interface Service** → Definir contrato de negócio
6. **Service** → Implementar lógica de negócio com validações
7. **Registrar DI** → `ResolveDependencyConfig.cs`
8. **ViewModel** → Criar DTOs para as views
9. **AutoMapper** → Adicionar mapeamentos
10. **Controller** → Criar controller herdando `MainController`
11. **Views** → Criar views Razor seguindo padrão AdminLTE
12. **Testes** → Adicionar testes unitários

### 20.3 Checklist de Consistência

- [ ] Controller herda de `MainController`
- [ ] Controller tem `[Authorize]`
- [ ] Serviço herda de `BaseService`
- [ ] Repositório herda de `Repository<T>`
- [ ] Interfaces seguem o padrão `I{Nome}Service` / `I{Nome}Repository`
- [ ] ViewModel usa Data Annotations para validação
- [ ] AutoMapper configurado em `AutomapperConfig.cs`
- [ ] DI registrado em `ResolveDependencyConfig.cs`
- [ ] Métodos async com sufixo `Async`
- [ ] Validações de negócio usam `Notificar()` do BaseService
- [ ] Conexão com banco usa `ObterConnectionString()` do Startup

---

## 21. Pontos de Atenção

### 21.1 Acoplamentos

- Controllers têm **muitas dependências** (9+ parâmetros no construtor via MainController)
- `ResolveDependencyConfig.cs` é uma classe grande que registra todos os serviços
- `AutomapperConfig.cs` é monolítico com mapeamentos de todos os domínios

### 21.2 Código Legado

- Bibliotecas depreciadas: `Microsoft.EntityFrameworkCore.Relational.Design` 1.1.6
- .NET Core 3.1 está **fora de suporte** desde dezembro de 2022
- `AutenticacaoService` tem métodos com `throw new NotImplementedException()`

### 21.3 Riscos

- Sem segregação de interface para o `MainController` (muitas dependências)
- `DbSession` e `CardapioDigitalDbSession` — possível vazamento de conexão
- Conexão com MongoDB gerenciada apenas no projeto API, não no MVC

### 21.4 Boas Práticas já Utilizadas

- Separação clara em camadas (MVC → Business → Infra)
- Notification Pattern evita exceções para regras de negócio
- Cookie Authentication com Identity Core bem configurado
- Multi-empresa via sessão e middleware
- Variáveis de ambiente para connection strings (segurança)
- AntiForgery global
- Suporte a Render (cloud) com detecção de ambiente
- Dapper para consultas otimizadas (evita sobrecarga do EF Core)

---

## 22. Recomendações para Evolução

1. **Migrar para .NET 8+** — versão atual (3.1) está fora de suporte
2. **Atualizar Pacotes NuGet** — remover pacotes depreciados
3. **Refatorar MainController** — reduzir número de dependências ou usar MediatR
4. **Quebrar AutomapperConfig** — separar profiles por domínio
5. **Quebrar ResolveDependencyConfig** — usar extension methods por módulo
6. **Implementar métodos pendentes** — `AutenticacaoService` tem stubs
7. **Adicionar mais testes** — cobertura atual é baixa (poucos testes em `agilum.mvc.web.tests`)
8. **Documentar APIs** — `agilium-manager-azure-api` e `agilium-pdv-azure-api`
