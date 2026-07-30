# Arquitetura MVC

## Objetivo

Documentar a arquitetura MVC utilizada pelo **Agilium Manager**, descrevendo a organização da camada de apresentação no projeto `agilum.mvc.web`, o fluxo de requisições, os padrões adotados e as responsabilidades de cada componente.

Este documento serve como referência para manutenção e desenvolvimento de novas funcionalidades.

---

## Escopo

Este documento contempla:

- Arquitetura MVC em camadas
- 28 Controllers (herdando de `MainController`)
- Views Razor com AdminLTE 3.x
- ViewModels organizados por domínio
- Área Identity (Razor Pages)
- Pipeline de requisição completo
- View Components, Tag Helpers, Html Helpers
- Convenções, boas práticas e checklist

---

## Índice

- [Visão Geral](#visão-geral)
- [Arquitetura MVC](#arquitetura-mvc)
- [Organização do Projeto](#organização-do-projeto)
- [Pipeline de Requisição](#pipeline-de-requisição)
- [Controllers](#controllers)
- [Views](#views)
- [ViewModels](#viewmodels)
- [Layouts e Componentes Compartilhados](#layouts-e-componentes-compartilhados)
- [View Components](#view-components)
- [Tag Helpers](#tag-helpers)
- [Html Helpers](#html-helpers)
- [Área Identity](#área-identity)
- [Fluxo de uma Funcionalidade](#fluxo-de-uma-funcionalidade)
- [Convenções](#convenções)
- [Boas Práticas](#boas-práticas)
- [Limitações Conhecidas](#limitações-conhecidas)
- [Checklist](#checklist)

---

## Visão Geral

O projeto **`agilum.mvc.web`** representa a camada de apresentação da solução Agilium Manager.

A aplicação adota o padrão **ASP.NET Core MVC (.NET Core 3.1)** com arquitetura em camadas:

```
agilum.mvc.web (Apresentação)
       ↓
agilium-manager-azure-business (Negócio)
       ↓
agilium-manager-git-azure-infra (Infraestrutura)
       ↓
MySQL 8.0 / MongoDB
```

---

## Arquitetura MVC

```
Browser (AdminLTE + jQuery + AJAX)
      │
      ▼
┌──────────────────────────────────────────┐
│  Middleware Pipeline                      │
│  UseStaticFiles → UseSession →           │
│  UseAuthentication → UseAuthorization →  │
│  EmpresaSelecionadaMiddleware →          │
│  ExceptionMiddleware                      │
└──────────────────────────────────────────┘
      │
      ▼
┌──────────────────────────────────────────┐
│  Controller                               │
│  Herda de MainController                  │
│  [Authorize] + [ClaimsAuthorize(idTag)]   │
│  Injeção: Services, IMapper, IUser,       │
│           ILogService, ILicencaService    │
└──────────────────────────────────────────┘
      │
      ▼
┌──────────────────────────────────────────┐
│  Business Services                        │
│  BaseService → *Service                   │
│  FluentValidation + Notification Pattern  │
└──────────────────────────────────────────┘
      │
      ▼
┌──────────────────────────────────────────┐
│  Repository (EF Core / Dapper / MongoDB)  │
└──────────────────────────────────────────┘
      │
      ▼
┌──────────────────────────────────────────┐
│  Controller (retorno)                     │
│  AutoMapper: Model → ViewModel            │
│  View() / PartialView() / Redirect()      │
└──────────────────────────────────────────┘
      │
      ▼
┌──────────────────────────────────────────┐
│  View Razor (AdminLTE + jQuery)           │
│  @model ViewModel                         │
│  Layout = _main.cshtml                    │
└──────────────────────────────────────────┘
```

---

## Organização do Projeto

```
agilum.mvc.web/
├── Controllers/            # 28 controllers MVC
│   ├── MainController.cs   # Classe base abstrata (todos herdam)
│   ├── HomeController.cs
│   ├── ProdutoController.cs
│   ├── CompraController.cs
│   ├── VendaController.cs
│   └── ...                 # Um por domínio
│
├── Views/                  # Views Razor
│   ├── _ViewImports.cshtml # @using + TagHelpers globais
│   ├── _ViewStart.cshtml   # Layout = "_main"
│   ├── Shared/             # Layouts, partials, componentes
│   ├── Produto/            # Views de Produto
│   ├── Compra/             # Views de Compra
│   └── ...                 # Uma pasta por controller
│
├── ViewModels/             # DTOs da camada de apresentação
│   ├── Produtos/
│   ├── Compra/
│   ├── Venda/
│   └── ...                 # Uma pasta por domínio
│
├── Areas/
│   └── Identity/           # Razor Pages (login, logout, registro)
│
├── Configuration/          # Identity, AutoMapper, DI, MVC, Globalization
├── Services/               # Serviços MVC-específicos (Auth, Email, Crypto)
├── Extensions/             # Middlewares, HtmlHelpers, TagHelpers, ViewComponents
├── Interfaces/             # Interfaces locais (IAutenticacaoService)
├── Data/                   # dbIdentityContext + RefreshToken
├── Enums/                  # Enums locais
├── wwwroot/                # AdminLTE, CSS, JS, imagens, libs
├── Properties/             # launchSettings.json
├── Program.cs              # Entry point
├── Startup.cs              # Configuração do pipeline
└── appsettings.json        # Configurações
```

---

## Pipeline de Requisição

Ordem real configurada em `Startup.Configure()`:

```
1. UseDeveloperExceptionPage / UseExceptionHandler + UseDatabaseErrorPage (dev)
2. UseHsts (produção, exceto Render cloud)
3. Log headers (middleware inline)
4. UseHttpsRedirection (exceto Render cloud)
5. UseStaticFiles              (wwwroot)
6. UseRouting                  (rotas)
7. UseSession                  (Cookie: HttpOnly, IsEssential)
8. UseAuthentication           (Cookie Auth)
9. UseAuthorization            (Claims + Roles)
10. EmpresaSelecionadaMiddleware (bloqueia sem empresa na sessão)
11. ExceptionMiddleware         (captura exceções não tratadas)
12. UseRequestLocalization      (cultura pt-BR: decimal ",", data "dd/MM/yyyy")
13. UseEndpoints               (RazorPages, Controllers, Areas, default route)
```

### Rotas Configuradas

```csharp
endpoints.MapRazorPages();
endpoints.MapControllers();
endpoints.MapControllerRoute("areas", "{area:exists}/{controller=Home}/{action=Index}/{id?}");
endpoints.MapAreaControllerRoute("Back", "Back", "back/{controller=Home}/{action=Index}/{id?}");
endpoints.MapControllerRoute("default", "{controller=Home}/{action=Index}/{id?}");
```

---

## Controllers

### Lista Completa (28 controllers)

| Controller | Rota | Domínio |
|------------|------|---------|
| `MainController` | — | **Classe base abstrata** |
| `HomeController` | `/` e `/licenca` | Dashboard, licença |
| `ProdutoController` | `[Route("produto")]` | Produtos |
| `CompraController` | `[Route("compra")]` | Compras e NFe |
| `VendaController` | — | Vendas/PDV |
| `CaixaController` | — | Abertura/fechamento de caixa |
| `TurnoController` | — | Turnos |
| `ClienteController` | — | Clientes (PF/PJ) |
| `FornecedorController` | — | Fornecedores |
| `FuncionarioController` | — | Funcionários |
| `EmpresaController` | — | Empresas (multi-empresa) |
| `UsuarioController` | — | Usuários e permissões |
| `EstoqueController` | — | Estoque |
| `FormaPagamentoController` | — | Formas de pagamento |
| `ContaController` | — | Contas a pagar/receber |
| `PlanoContaController` | — | Plano de contas |
| `CategoriaFinanceiraController` | — | Categorias financeiras |
| `MoedaController` | — | Moedas e cotações |
| `DevolucaoController` | — | Devoluções |
| `InventarioController` | — | Inventário |
| `PerdaController` | — | Perdas |
| `ValeController` | — | Vales |
| `PontoVendaController` | — | Pontos de venda |
| `UnidadeController` | — | Unidades de medida |
| `ConfigController` | — | Configurações |
| `LicencaController` | — | Licenças |
| `LogController` | — | Logs do sistema |
| `EnderecoController` | — | Endereços |

### Estrutura Padrão

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

    [HttpGet]
    public async Task<ActionResult> Index() { ... }

    [HttpGet]
    [ClaimsAuthorizeAttribute(2067)]
    public async Task<ActionResult> Create() { ... }

    [HttpPost]
    [ClaimsAuthorizeAttribute(2067)]
    public async Task<ActionResult> Create(ViewModel model)
    {
        if (!ModelState.IsValid) return View(model);

        var entity = _mapper.Map<Entity>(model);
        await _service.Adicionar(entity);

        if (!OperacaoValida())
        {
            var erros = ObterNotificacoes();
            foreach (var erro in erros)
                ModelState.AddModelError(string.Empty, erro);
            return View(model);
        }

        await _service.Salvar();
        TempData["Mensagem"] = "Operação realizada com sucesso";
        return RedirectToAction("Index");
    }

    public async Task<ActionResult> Edit(long id) { ... }
}
```

### MainController — Serviços Compartilhados

| Serviço | Propósito |
|---------|-----------|
| `INotificador` | Validações (Notification Pattern) |
| `IConfiguration` | Acesso a settings |
| `IMapper` | AutoMapper |
| `IUser` (`AspNetUser`) | Usuário autenticado (claims) |
| `IUtilDapperRepository` | Geração de IDs (`GerarUUID`, `GerarIdInt`) |
| `ILogService` | Log de operações e erros |
| `ILicencaService` | Verificação de licença |
| `IAuthService` | Autenticação customizada |

### Métodos Auxiliares do MainController

```csharp
protected bool OperacaoValida()                // Tem notificações?
protected async Task<long> GerarId()            // UUID via Dapper
protected async Task<int> GerarIdInt(string)    // ID sequencial
protected void NotificarErro(string)            // Adiciona notificação
protected string[] ObterNotificacoes()          // Lista mensagens
protected void LogInformacao(...)               // Log estruturado
protected void LogErro(...)                     // Log de erro
protected EmpresaUsuarioViewModel ObterObjetoEmpresaSelecionada()  // Empresa da sessão
protected string ObterNomeUsuarioLogado()       // Nome do usuário atual
```

---

## Views

### Organização

```
Views/
├── _ViewImports.cshtml          # @using globais + @addTagHelper
├── _ViewStart.cshtml             # Layout = "_main"
│
├── Shared/
│   ├── _main.cshtml              # Layout AdminLTE principal
│   ├── _Layout.cshtml            # Layout alternativo
│   ├── _ASideMenu.cshtml         # Menu lateral dinâmico
│   ├── _LoginPartial.cshtml      # Header com info do usuário
│   ├── _rodape.cshtml            # Rodapé
│   ├── _ValidationScriptsPartial.cshtml  # jQuery validation
│   └── Components/               # View Components
│       ├── Paginacao/
│       └── Summary/
│
├── Home/                         # Index, Licenca, Error
├── Produto/                      # Index, CreateEdit, etc.
├── Compra/                       # IndexCompra, CreateEdit, Cancelar, etc.
└── ...                           # Uma pasta por controller
```

### _ViewImports.cshtml

```razor
@using agilum.mvc.web
@using agilum.mvc.web.ViewModels
@addTagHelper *, Microsoft.AspNetCore.Mvc.TagHelpers
@addTagHelper *, agilum.mvc.web
```

### _ViewStart.cshtml

```razor
@{
    Layout = "_main";
}
```

---

## ViewModels

### Organização

```
ViewModels/
├── Produtos/          ProdutoViewModel, GrupoProdutoViewModel...
├── Compra/            CompraViewModel, CompraItemViewModel...
├── Venda/             VendaViewModel, VendaItemViewModel...
├── Cliente/           ClienteViewModel...
├── Fornecedor/        FornecedorViewModel...
├── Empresa/           EmpresaViewModel...
├── Usuarios/          UsuarioViewModel...
├── Estoque/           EstoqueViewModel...
├── Caixa/             CaixaViewModel...
├── Turno/             TurnoIndexViewModel...
├── Conta/             ContaViewModel...
├── PlanoConta/        PlanoContaViewModel...
├── FormaPagamento/    FormaPagamentoViewModel...
├── Impostos/          CfopViewModel, CstViewModel, NcmViewModel...
├── Funcionarios/      FuncionarioViewModel...
├── Moedas/            MoedaViewModel...
├── Devolucao/         DevolucaoViewModel...
├── Inventario/        InventarioViewModel...
├── Perda/             PerdaViewModel...
├── Vale/              ValeViewModel...
├── PontoVenda/        PontoVendaViewModel...
├── UnidadeViewModel/  UnidadeIndexViewModel...
├── Config/            ConfigViewModel...
├── Licenca/           LicencaViewModel...
├── Log/               LogViewModel...
├── Endereco/          EnderecoViewModel...
├── Contato/           ContatoViewModel...
├── EmpresaUsuario/    EmpresaUsuarioViewModel...
├── CategeoriaFinanceira/  CategoriaFinanceiraViewModel...
│
├── PagedResult.cs     # Paginação genérica
├── ErrorViewModel.cs  # Modelo de erro
├── RefreshToken.cs    # Token de refresh
└── Estado.cs          # Enum de estado
```

### Padrão

- Sufixo `ViewModel` em todos os nomes
- Data Annotations para validação
- Mapeamento centralizado via `AutomapperConfig.cs`
- Um ViewModel por operação (Index, Create/Edit são reutilizados com `CreateEdit.cshtml`)

---

## Layouts e Componentes Compartilhados

### _main.cshtml (Principal)

Layout base AdminLTE 3.x com:

- Sidebar (`_ASideMenu.cshtml`)
- Navbar com info do usuário (`_LoginPartial.cshtml`)
- Content area (`@RenderBody()`)
- Footer (`_rodape.cshtml`)
- Scripts: jQuery, Bootstrap, AdminLTE, DataTables, Select2, Toastr, Inputmask
- `@RenderSection("Scripts", required: false)`

### _ASideMenu.cshtml

Menu lateral dinâmico — renderiza itens baseados nas permissões do usuário.

### _LoginPartial.cshtml

Exibe nome do usuário logado, empresa selecionada e link de logout.

### _ValidationScriptsPartial.cshtml

```html
<script src="~/lib/jquery-validation/dist/jquery.validate.min.js"></script>
<script src="~/lib/jquery-validation-unobtrusive/jquery.validate.unobtrusive.min.js"></script>
```

---

## View Components

### PaginacaoViewComponent

```csharp
// Extensions/PaginacaoViewComponent.cs
public class PaginacaoViewComponent : ViewComponent
{
    public IViewComponentResult Invoke(IPagedList modeloPaginado)
    {
        return View(modeloPaginado);
    }
}
```

Uso nas Views:
```razor
@await Component.InvokeAsync("Paginacao", new { modeloPaginado = Model })
```

### SummaryViewComponent

```csharp
// Extensions/SummaryViewComponent.cs
public class SummaryViewComponent : ViewComponent
{
    private readonly INotificador _notificador;

    public SummaryViewComponent(INotificador notificador)
    {
        _notificador = notificador;
    }

    public async Task<IViewComponentResult> InvokeAsync()
    {
        var notificacoes = await Task.FromResult(_notificador.ObterNotificacoes());
        notificacoes.ForEach(c => ViewData.ModelState.AddModelError(string.Empty, c.Mensagem));
        return View();
    }
}
```

> Exibe notificações de validação acumuladas no `INotificador`.

---

## Tag Helpers

### MoneyInputTagHelper

```csharp
// Extensions/TagHelpers.cs
[HtmlTargetElement("input", Attributes = "asp-for")]
public class MoneyInputTagHelper : TagHelper
{
    public ModelExpression For { get; set; }

    public override void Process(TagHelperContext context, TagHelperOutput output)
    {
        var type = For.ModelExplorer.ModelType;
        if (type == typeof(double) || type == typeof(double?) ||
            type == typeof(decimal) || type == typeof(decimal?))
        {
            // Adiciona classe CSS "money" automaticamente
            output.Attributes.SetAttribute("class", existingClass + " money");
        }
    }
}
```

> Adiciona automaticamente a classe `money` a inputs do tipo `double`/`decimal` para formatação monetária com máscara JavaScript.

---

## Html Helpers

### GetEnumValueSelectList

```csharp
// Extensions/HtmlExtensions.cs
public static IEnumerable<SelectListItem> GetEnumValueSelectList<TEnum>(
    this IHtmlHelper htmlHelper) where TEnum : struct
{
    return new SelectList(Enum.GetValues(typeof(TEnum)).OfType<Enum>()
        .Select(x => new SelectListItem
        {
            Text = x.GetType().GetField(x.ToString())
                     .GetCustomAttribute<DisplayAttribute>()?.Name,
            Value = x.ToString()
        }), "Value", "Text");
}
```

> Gera `<select>` options a partir de enums com `[Display]` attribute. Usado em dropdowns de situação, tipo, etc.

---

## Área Identity

```
Areas/Identity/
└── Pages/
    └── Account/
        ├── Login.cshtml
        ├── Logout.cshtml
        ├── Register.cshtml
        ├── Lockout.cshtml
        ├── ForgotPassword.cshtml
        ├── ResetPassword.cshtml
        ├── AccessDenied.cshtml
        └── ...
```

- Razor Pages do ASP.NET Core Identity
- Autenticação via **Cookie** (não JWT na web)
- `IdentityConfig.cs` configura:
  - `AddIdentityCore<CaUsuarioIdentity>()`
  - `.AddRoles<IdentityRole>()`
  - Cookie auth com `CookieAuthenticationDefaults`
  - Login em `/Identity/Account/Login`

---

## Fluxo de uma Funcionalidade

### Exemplo: Criar Produto

```
[GET] /produto/novo
      │
      ▼
ProdutoController.Create()
  → Popular listas (grupos, unidades, NCM, etc.)
  → return View("CreateEdit", produtoViewModel)
      │
      ▼
[View] Views/Produto/CreateEdit.cshtml
  → Form com AntiForgery + Data Annotations
      │
      ▼
[POST] /produto/novo
      │
      ▼
ProdutoController.Create(ProdutoViewModel model)
  ├── ModelState.IsValid? ──Não──→ return View(model)
  ├── _mapper.Map<Produto>(model)
  ├── _produtoService.Adicionar(produto)
  │     ├── ExecutarValidacao(new ProdutoValidation(), produto)
  │     ├── Regras de negócio (código único, etc.)
  │     └── _produtoRepository.AdicionarSemSalvar(produto)
  │
  ├── OperacaoValida()?
  │     ├── true  → _produtoService.Salvar() → Redirect Index
  │     └── false → Adicionar erros ao ModelState → return View(model)
```

### Arquivos Envolvidos

| Camada | Arquivo |
|--------|---------|
| View | `Views/Produto/CreateEdit.cshtml` |
| ViewModel | `ViewModels/Produtos/ProdutoViewModel.cs` |
| Controller | `Controllers/ProdutoController.cs` |
| AutoMapper | `Configuration/AutomapperConfig.cs` |
| Service Interface | `Business/Interfaces/IService/IProdutoService.cs` |
| Service | `Business/Services/ProdutoService.cs` |
| FluentValidation | `Business/Models/Validations/ProdutoValidation.cs` |
| Model | `Business/Models/Produto.cs` |
| Repository Interface | `Business/Interfaces/IRepository/IProdutoRepository.cs` |
| Repository | `Infra/Repository/ProdutoReposiotry.cs` |
| DbContext | `Infra/Context/AgiliumContext.cs` |

---

## Convenções

### Controllers

- Herdam de `MainController`
- `[Authorize]` no nível da classe
- `[ClaimsAuthorizeAttribute(idTag)]` por ação sensível
- `[Route("nome")]` para rota amigável
- Métodos `async Task<ActionResult>`
- Injeção de dependência via construtor
- **Nunca** acessam Repository diretamente
- **Nunca** contêm regras de negócio

### Views

- Uma pasta por Controller
- `CreateEdit.cshtml` reutilizado para criar e editar
- `_nome.cshtml` para partial views
- `@model ViewModel` fortemente tipado
- Layout padrão: `_main.cshtml`

### ViewModels

- Sufixo `ViewModel`
- Data Annotations para validação estrutural
- Um arquivo por operação ou reutilização com `CreateEdit`
- Mapeados via AutoMapper (NUNCA manualmente no Controller)

### Rotas

| Padrão | Exemplo |
|--------|---------|
| Sem prefixo | `/{controller}/{action}/{id?}` |
| Com prefixo | `[Route("produto")]` → `/produto/novo` |
| Área | `/{area}/{controller}/{action}/{id?}` |

---

## Boas Práticas

| Fazer | Evitar |
|-------|--------|
| Herdar de `MainController` | Controller sem `[Authorize]` |
| Usar ViewModels tipados | Usar `ViewBag`/`ViewData` para dados complexos |
| Mapear com AutoMapper | Mapeamento manual no Controller |
| `AdicionarSemSalvar` + `Salvar()` | `SaveChanges` a cada operação |
| Verificar `OperacaoValida()` | Ignorar notificações do Service |
| Partial Views para reuso | Duplicar HTML entre Views |
| View Components para lógica de UI | Lógica complexa na View |
| `TempData` para mensagens pós-redirect | `ViewBag` entre redirects (perde) |

---

## Limitações Conhecidas

- **MainController com muitas dependências** — 8 parâmetros no construtor base; controllers filhos herdam todos
- **AutoMapperConfig monolítico** — único arquivo com mapeamentos de todos os domínios
- **ResolveDependencyConfig monolítico** — todos os registros de DI em uma classe
- **Alguns controllers usam `.Result`** — bloqueante; preferir `await`
- **Listas auxiliares populadas no construtor** — `listaEmpresaViewModels` carregadas no construtor do controller com `.Result`
- **.NET Core 3.1** — versão fora de suporte desde dez/2022

---

## Checklist

Antes de criar/alterar uma funcionalidade MVC:

☐ Controller herda de `MainController`

☐ Controller tem `[Authorize]`

☐ Ações sensíveis têm `[ClaimsAuthorizeAttribute(idTag)]`

☐ ViewModel criado em `ViewModels/{Dominio}/`

☐ ViewModel usa Data Annotations

☐ Mapeamento AutoMapper adicionado em `AutomapperConfig.cs`

☐ Controller chama Service (nunca Repository)

☐ `OperacaoValida()` verificado após chamar Service

☐ View usa `@model ViewModel` tipado

☐ Form POST inclui `@Html.AntiForgeryToken()`

☐ Mensagens de sucesso/erro via `TempData`

☐ Partial Views para trechos reutilizáveis

☐ Métodos assíncronos (`async Task<ActionResult>`)

☐ Serviço registrado como Scoped no DI
- Notification Pattern;
- ASP.NET Core Identity;
- Dependency Injection.

Ainda deverão ser detalhados em documentos específicos:

- catálogo completo de Controllers;
- catálogo completo de Views;
- catálogo de View Components;
- catálogo de Partial Views;
- documentação detalhada dos filtros personalizados.

---

# Atualização

Este documento deve ser revisado sempre que ocorrer:

- alteração da arquitetura MVC;
- criação de novos módulos;
- inclusão de novos Controllers;
- alteração do pipeline de requisição;
- evolução da arquitetura da camada de apresentação.

---

# Documentação Relacionada

## Interface

- ui/razor.md
- ui/layouts.md
- ui/components.md
- ui/css.md
- ui/javascript.md

## Arquitetura

- architecture/overview.md
- architecture/layers.md
- architecture/patterns.md

## Desenvolvimento

- development/coding-standards.md
- development/testing.md