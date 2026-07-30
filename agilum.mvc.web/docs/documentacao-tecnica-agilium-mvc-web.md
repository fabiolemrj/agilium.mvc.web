# 📚 Documentação Técnica — `agilium.mvc.web`

> **Data:** 2026-07-14  
> **Versão:** .NET Core 3.1  
> **Tipo:** Guia de Referência para Desenvolvimento  
> **Objetivo:** Documentar arquitetura, padrões, convenções e fluxos para manter consistência em novas implementações

---

## 🏗️ 1. Arquitetura Geral

### 1.1 Modelo Arquitetural

O projeto segue o padrão **MVC com camadas separadas**, similar a Clean Architecture:

```text
┌──────────────────────────────────────────────────────────────────┐
│                    agilum.mvc.web                                 │
│                 (Camada de Apresentação)                          │
│  Controllers → Views (.cshtml) → ViewModels                      │
│  Extensions (Middleware, Auth, ViewComponents)                    │
│  Services (Autenticação, Email, Criptografia)                    │
├──────────────────────────────────────────────────────────────────┤
│              agilium-manager-azure-business                       │
│                 (Camada de Negócio)                               │
│  Services → Models → Enums → Validations → Interfaces            │
├──────────────────────────────────────────────────────────────────┤
│            agilium-manager-git-azure-infra                        │
│              (Camada de Infraestrutura)                           │
│  Repository (EF Core + Dapper) → Context → Mappings              │
├──────────────────────────────────────────────────────────────────┤
│                      MySQL 8.0                                    │
│            agiliumadm / cardapio_digital                          │
└──────────────────────────────────────────────────────────────────┘
```

### 1.2 Fluxo Completo de uma Requisição

```text
Browser
  │
  ▼
Middleware Pipeline (Startup.Configure)
  ├── EmpresaSelecionadaMiddleware (verifica sessão _empSelec)
  ├── ExceptionMiddleware (captura erros globais)
  ├── Authentication (Cookie + Claims)
  └── Authorization ([Authorize] + [ClaimsAuthorizeAttribute])
  │
  ▼
Controller (herda de MainController)
  ├── ObterObjetoEmpresaSelecionada() — sessão obrigatória
  ├── VerificarValidadeLicenca() — valida licença
  └── [ClaimsAuthorizeAttribute(tag)] — permissão por tag numérica
  │
  ▼
Service (Business Layer) — lógica de negócio + validação
  │
  ▼
Repository (Infra Layer)
  ├── EF Core (AgiliumContext) — CRUD padrão
  └── Dapper — queries complexas/paginadas
  │
  ▼
MySQL Database
  │
  ▼
Controller → ViewModel → View (.cshtml) → Browser
```

---

## 2. Estrutura de Diretórios

| Pasta | Responsabilidade |
| --- | --- |
| `Configuration/` | Configurações de DI, AutoMapper, Identity, MVC, globalização |
| `Controllers/` | 28 Controllers MVC — um por módulo de negócio |
| `Data/` | `dbIdentityContext` — Identity separado do contexto de negócio |
| `Enums/` | Enums específicos da camada de apresentação |
| `Extensions/` | Middlewares, filtros de autorização, ViewComponents, TagHelpers, HTML helpers |
| `Interfaces/` | Interfaces da camada web (`IAutenticacaoService`, `IImportarXMLNfe`) |
| `Services/` | Serviços da camada web (AuthService, Email, Criptografia, Código de Barras) |
| `ViewModels/` | +40 ViewModels organizados por domínio (subpastas) |
| `Views/` | Razor Views organizadas por Controller + `Shared/` + `Components/` |
| `wwwroot/` | Assets estáticos: AdminLTE 3, Bootstrap 4, jQuery, plugins |

---

## 3. Convenções e Padrões Obrigatórios

### 3.1 Nomenclatura

| Elemento | Convenção | Exemplo |
| --- | --- | --- |
| **Controllers** | `{Entidade}Controller` | `ProdutoController`, `EmpresaController` |
| **Actions** | `{Verbo}{Entidade}` | `CreateProduto`, `EditProduto` |
| **Rotas** | `[Route("{entidade}")]` no controller | `[Route("produto")]` |
| **ViewModels** | `{Entidade}ViewModel` | `ProdutoViewModel`, `EmpresaViewModel` |
| **Views** | Nome da action | `Index.cshtml`, `CreateEditProduto.cshtml` |
| **Interfaces** | `I{Entidade}Service` / `I{Entidade}Repository` | `IProdutoService` |
| **Métodos** | Sufixo `Async` | `ObterPorIdAsync()`, `AdicionarAsync()` |

### 3.2 Herança de Controllers

```
Controller (Microsoft.AspNetCore.Mvc)
  └── MainController (abstract)
        ├── HomeController
        ├── ProdutoController
        └── ... (todos os 27 controllers)
```

### 3.3 Estrutura Padrão de Controller

```csharpcsharp
[Route("entidade")]
[Authorize]
public class EntidadeController : MainController
{
    // Constantes privadas
    private readonly IEntidadeService _entidadeService;
    // ... demais dependências

    public EntidadeController(
        IEntidadeService entidadeService,
        // + dependências base do MainController (8)
    ) : base(notificador, configuration, appUser, 
             utilDapperRepository, logService, mapper, licencaService, authService)
    {
        _entidadeService = entidadeService;
        // Pré-carregar listas auxiliares no construtor
    }

    [Route("lista")]
    [ClaimsAuthorizeAttribute(TAG)]
    public async Task<IActionResult> Index(int page, int ps, string q)
    {
        var empresa = ObterObjetoEmpresaSelecionada();
        if (empresa == null) return RedirectComErro("Selecione uma empresa");
        
        var lista = await _service.ObterPaginacao(...);
        return View(lista);
    }
}
```

### 3.4 Injeção de Dependência

**Registro:** `Configuration/ResolveDependencyConfig.cs` — método `ResolveDependencies()`

```csharp
// Padrão: Scoped, Interface → Implementação
services.AddScoped<IProdutoService, ProdutoService>();
services.AddScoped<IProdutoRepository, ProdutoReposiotry>();
services.AddScoped<IProdutoDapper, ProdutoDapper>();

// Organizado por #region (geral, Produto, Estoque, Venda, etc.)
// Serviços da camada infra registrados aqui também:
services.AddScoped<IIntegracaoCardapioService, IntegracaoCardapioService>();
```

**Ordem no `Startup.ConfigureServices`:**

1. `AddControllers()` / `AddControllersWithViews()`
2. `AddDbContext<AgiliumContext>(Pomelo MySQL)`
3. `services.ResolveDependencies(Configuration)` ← DI
4. `services.AddIdentityConfiguration(Configuration)` ← Identity + Cookie Auth
5. `AddAutoMapper(typeof(Startup))`
6. Session (3h, HttpOnly, Essential)

### 3.5 Pipeline de Middleware (Startup.Configure)

```text
DeveloperExceptionPage (dev)
  → Error handler (prod)
  → HSTS (prod, exceto Render)
  → Request header logging
  → HTTPS redirection (exceto Render)
  → StaticFiles
  → Routing
  → Session
  → Authentication → Authorization
  → EmpresaSelecionadaMiddleware
  → ExceptionMiddleware
```

---

## 4. Tecnologias Utilizadas

| Categoria | Tecnologia | Versão | Onde |
| --- | --- | --- | --- |
| **Runtime** | .NET Core | 3.1 | Todos os projetos |
| **Framework** | ASP.NET Core MVC | 3.1 | `agilum.mvc.web` |
| **ORM** | EF Core (Pomelo MySQL) | 3.1.32 / 3.2.7 | `agilium-manager-git-azure-infra` |
| **Micro-ORM** | Dapper | 2.1.21 | Infra (consultas complexas) |
| **Banco** | MySQL | 8.0.19 | `agiliumadm` + `cardapio_digital` |
| **Autenticação** | ASP.NET Core Identity | 3.1.32 | Cookie + Claims |
| **Mapeamento** | AutoMapper | 8.1.1 | `Configuration/AutomapperConfig.cs` |
| **Validação** | FluentValidation | 11.3.0 | Business layer |
| **Frontend** | AdminLTE 3 + Bootstrap 4 + jQuery | — | `wwwroot/` |
| **Código Barras** | ZXing.Net + QRCoder | 0.16.1 / 1.4.3 | `Services/CodigoProdutoGenerator.cs` |
| **Criptografia** | BouncyCastle (Blowfish) | 1.8.9 | `Services/PassCrypto.cs` |
| **Resiliência** | Polly | 6.0.36 | HttpClient retries |
| **Log** | KissLog | 5.1.2 | Infra |
| **Container** | Docker | — | Dockerfile multi-stage |

---

## 5. Autenticação e Autorização

### 5.1 Fluxo

```text
POST /Identity/Account/Login
  → AuthService.LoginAsync()
    → IUsuarioRepository (valida credenciais)
    → MD5 hash da senha
    → ClaimsPrincipal (NameIdentifier, Name, Email)
    → HttpContext.SignInAsync (Cookie Authentication)
  → Redireciona para seleção de empresa
```

### 5.2 Autorização

| Mecanismo | Onde | Como |
|---|---|---|
| `[Authorize]` | Todo controller | Exige autenticação |
| `[ClaimsAuthorizeAttribute(tag)]` | Cada action | Verifica `ICaService.UsuarioTemPermissao(idUsuario, tag)` |
| `EmpresaSelecionadaMiddleware` | Global | Bloqueia acesso sem empresa na sessão |

### 5.3 Tags Numéricas de Permissão

| Faixa | Módulo |
| --- | --- |
| 1000-1999 | Usuários e Controle de Acesso |
| 2000-2099 | Empresas e Clientes |
| 2050-2099 | Produtos e Estoque |
| 2100-2199 | Inventário, PDV, Turno, Caixa, Venda |

---

## 6. Sessão e Multi-Empresa

- Empresa ativa armazenada em `HttpContext.Session["_empSelec"]` como JSON
- `MainController.ObterObjetoEmpresaSelecionada()` — método padrão em toda action
- Middleware `EmpresaSelecionadaMiddleware` — whitelist de ~30 paths permitidos sem empresa
- `MainController.VerificarValidadeLicenca()` — valida `ILicencaService.DataValida()`

---

## 7. ViewModels — Padrão

### 7.1 Organização

Cada domínio tem sua subpasta em `ViewModels/`:
```
ViewModels/
├── Produtos/ProdutoViewModel.cs
├── Empresa/EmpresaViewModel.cs
├── Venda/VendaViewModel.cs
└── ...
```

### 7.2 Propriedades Double com Formato Brasileiro

```csharpcsharp
public double? Preco { get; set; }
private string precoView;

[Display(Name = "Preço")]
public string PrecoView
{
    get => precoView;
    set { precoView = value; Preco = ParseDecimal(value); }
}
```

### 7.3 Listas Auxiliares no ViewModel

ViewModels contêm listas para dropdowns populadas no controller:
```csharp
public List<EmpresaViewModel> Empresas { get; set; }
public List<GrupoProdutoViewModel> Grupos { get; set; }
// ...
```

---

## 8. AutoMapper — Padrão

**Arquivo:** `Configuration/AutomapperConfig.cs`  
**Herança:** `Profile`  
**Padrão de mapeamento:**

```csharp
CreateMap<Produto, ProdutoViewModel>()
    .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDPRODUTO))
    .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMPRODUTO))
    // ... demais propriedades
    .ForMember(dest => dest.Empresas, act => act.Ignore())
    .ReverseMap();
```

---

## 9. Views — Padrão

### 9.1 Layout Principal

`_main.cshtml` — AdminLTE 3 com sidebar, navbar, content-wrapper, modal de loading.

### 9.2 Página de Listagem (Index)

```html
@model PagedViewModel<ProdutoViewModel>

<section class="barra-de-menu-principal">
    <div class="barra-de-botoes-menu-principal">
        <a asp-action="CreateProduto" onclick="on()">
            <span class="fa fa-plus-square" id="btnNovoCadastro"></span>
        </a>
        <!-- Botões de ação adicionais aqui -->
    </div>
</section>

<!-- Formulário de busca -->
<form asp-action="Index" method="get">
    <input name="q" value="@ViewBag.Pesquisa" />
    <button type="submit"><i class="fa fa-search"></i></button>
</form>

<!-- Grid de resultados -->
<table class="table table-hover">
    <!-- iteração sobre Model.List -->
</table>

<!-- Paginação via ViewComponent -->
@await Component.InvokeAsync("Paginacao", Model)
```

### 9.3 ViewComponents

| ViewComponent | Uso |
|---|---|
| `Paginacao` | `@await Component.InvokeAsync("Paginacao", Model)` |
| `Summary` | `@await Component.InvokeAsync("Summary")` — validações do `INotificador` |

---

## 10. Validação e Tratamento de Erros

### 10.1 Validação

| Tipo | Mecanismo |
| --- | --- |
| **ModelState** | DataAnnotations no ViewModel (`[Required]`, `[StringLength]`) |
| **Negócio** | FluentValidation no business layer (`ProdutoValidation`) |
| **Notificações** | `INotificador` — erros coletados e exibidos via `SummaryViewComponent` |
| **AntiForgery** | `[AutoValidateAntiforgeryToken]` global |

### 10.2 Tratamento de Erros

| Mecanismo | Arquivo |
| --- | --- |
| **Middleware global** | `Extensions/ExceptionMiddleware.cs` |
| **Try-catch nos controllers** | Padrão: `if (!OperacaoValida()) { AdicionarErroValidacao(...); return View(); }` |
| **Log** | `MainController.LogErro()` → `ILogService` |

---

## 11. Banco de Dados

| Aspecto | Detalhe |
| --- | --- |
| **ORM Principal** | EF Core 3.1 (Pomelo MySQL) |
| **ORM Secundário** | Dapper (queries complexas, paginação) |
| **Contexto de Negócio** | `AgiliumContext` (infra) |
| **Contexto Identity** | `dbIdentityContext` (web/Data) |
| **Migrations** | No projeto infra (`agilium-manager-git-azure-infra`) |
| **Mapeamentos** | Fluent API em `Mappings/` (ex: `ProdutoMapping`) |
| **Colunas** | snake_case: `IDPRODUTO`, `NMPRODUTO`, `STPRODUTO` |

---

## 12. Front-end

| Tecnologia | Uso |
| --- | --- |
| **AdminLTE 3** | Template base (sidebar, navbar, cards) |
| **Bootstrap 4** | Grid, formulários, modais |
| **jQuery** | Manipulação DOM, AJAX |
| **jQuery Validation** | Validação client-side |
| **Toastr** | Notificações toast |
| **Chart.js** | Gráficos (dashboard) |
| **DataTables** | Tabelas avançadas |
| **Select2** | Dropdowns com busca |
| **SweetAlert2** | Diálogos de confirmação |
| **Font Awesome** | Ícones |
| **Inputmask** | Máscaras de input |

### JavaScript Customizado

`wwwroot/js/site.js` — funções:
- `on()` — mostra overlay de loading
- `off()` — esconde overlay
- `ConfirmDelete(descricao, itemid, action)` — diálogo de exclusão

---

## 13. Guia para Novas Implementações

### 13.1 Sequência Correta

```text
1. MODEL (Business)    → agilium-manager-azure-business/Models/
2. INTERFACE (Business)→ agilium-manager-azure-business/Interfaces/
3. REPOSITORY (Infra)  → agilium-manager-git-azure-infra/Repository/
4. MAPPING (Infra)     → agilium-manager-git-azure-infra/Mappings/
5. SERVICE (Business)  → agilium-manager-azure-business/Services/
6. VALIDATION (Bus.)   → agilium-manager-azure-business/Validations/
7. VIEWMODEL (Web)     → agilum.mvc.web/ViewModels/{Dominio}/
8. AUTOMAPPER (Web)    → agilum.mvc.web/Configuration/AutomapperConfig.cs
9. CONTROLLER (Web)    → agilum.mvc.web/Controllers/
10. VIEWS (Web)         → agilum.mvc.web/Views/{Controller}/
11. DI REGISTRO (Web)   → agilum.mvc.web/Configuration/ResolveDependencyConfig.cs
```

### 13.2 Onde Criar Cada Artefato

| Artefato | Local | Namespace |
| --- | --- | --- |
| **Entidade** | `business/Models/` | `agilium.api.business.Models` |
| **Enum** | `business/Enums/` | `agilium.api.business.Enums` |
| **Interface Service** | `business/Interfaces/` | `agilium.api.business.Interfaces` |
| **Service (negócio)** | `business/Services/` | `agilium.api.business.Services` |
| **Service (infra/Dapper)** | `infra/Services/` | `agilium.api.infra.Services` |
| **Interface Repository** | `business/Interfaces/IRepository/` | `agilium.api.business.Interfaces.IRepository` |
| **Repository (EF)** | `infra/Repository/` | `agilium.api.infra.Repository` |
| **Repository (Dapper)** | `infra/Repository/Dapper/` | `agilium.api.infra.Repository.Dapper` |
| **Mapping EF** | `infra/Mappings/` | `agilium.api.infra.Mappings` |
| **ViewModel** | `web/ViewModels/{Dominio}/` | `agilum.mvc.web.ViewModels.{Dominio}` |
| **Controller** | `web/Controllers/` | `agilum.mvc.web.Controllers` |
| **View** | `web/Views/{Controller}/` | — |
| **DI Registro** | `web/Configuration/ResolveDependencyConfig.cs` | — |
| **AutoMapper** | `web/Configuration/AutomapperConfig.cs` | — |

### 13.3 Checklist de Nova Funcionalidade

- [ ] Criar entidade em `business/Models/`
- [ ] Criar interface de repositório em `business/Interfaces/IRepository/`
- [ ] Criar repository (EF) em `infra/Repository/`
- [ ] Criar mapping Fluent API em `infra/Mappings/`
- [ ] Adicionar `DbSet<T>` no `AgiliumContext`
- [ ] Criar interface de serviço em `business/Interfaces/`
- [ ] Criar service em `business/Services/`
- [ ] Criar validação FluentValidation em `business/Validations/`
- [ ] Criar ViewModel em `web/ViewModels/{Dominio}/`
- [ ] Adicionar mapeamento AutoMapper em `web/Configuration/AutomapperConfig.cs`
- [ ] Criar Controller herdando de `MainController` com `[Route]` e `[Authorize]`
- [ ] Adicionar `[ClaimsAuthorizeAttribute(tag)]` nas actions
- [ ] Chamar `ObterObjetoEmpresaSelecionada()` no início de cada action
- [ ] Criar Views: `Index.cshtml`, `CreateEdit{Entidade}.cshtml`
- [ ] Registrar DI: serviço e repositório em `ResolveDependencyConfig.cs`
- [ ] Criar migration: `dotnet ef migrations add NomeMigration`
- [ ] Testar fluxo completo

---

## 14. Pontos de Atenção

| Ponto | Detalhe |
| --- | --- |
| **Construtores inchados** | Controllers têm 10-17 dependências. Considere refatoração futura (Facade/MediatR). |
| **Acoplamento web→infra** | `Controllers/` importam namespaces de `Repository/`. Violação DIP. |
| **Sessão como estado global** | `_empSelec` na sessão, verificada manualmente. |
| **.NET Core 3.1 EOL** | Fora de suporte desde dez/2022. |
| **Sem testes automatizados** | Nenhum teste unitário ou de integração. |
| **Código comentado** | Blocos extensos de código comentado não removido. |
| **DI duplicados** | Alguns serviços registrados 2x no `ResolveDependencyConfig`. |

---

## 15. Resumo de Arquivos Importantes

| Arquivo | Função |
| --- | --- |
| `Program.cs` | Entry point, Kestrel config, Render port binding |
| `Startup.cs` | Pipeline completo (services + middleware) |
| `Configuration/ResolveDependencyConfig.cs` | TODOS os registros DI |
| `Configuration/AutomapperConfig.cs` | TODOS os mapeamentos AutoMapper |
| `Configuration/IdentityConfig.cs` | Cookie auth + Identity config |
| `Controllers/MainController.cs` | Base abstrata: sessão, licença, log, notificações |
| `Extensions/CustomAuth.cs` | `ClaimsAuthorizeAttribute` — permissões por tag |
| `Extensions/EmpresaSelecionadaMiddleware.cs` | Força seleção de empresa |
| `Extensions/ExceptionMiddleware.cs` | Tratamento global de exceções |
| `Data/dbIdentityContext.cs` | Identity DbContext separado |
| `Views/Shared/_main.cshtml` | Layout AdminLTE principal |
| `Views/Shared/_ASideMenu.cshtml` | Menu lateral |

---

> **Documento gerado como guia de referência para desenvolvimento. Qualquer nova implementação deve seguir os padrões aqui documentados.**
