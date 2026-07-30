# Como Criar um Controller

## Objetivo

Guia passo a passo para criar um novo **Controller MVC** seguindo os padrões do projeto Agilium Manager.

---

## Pré-requisitos

- Service e Repository já implementados
- ViewModel já criado
- Mapeamento AutoMapper já registrado
- DI já configurado em `ResolveDependencyConfig.cs`

---

## Passo a Passo

### 1. Criar o arquivo

**Local:** `agilum.mvc.web/Controllers/{Nome}Controller.cs`

### 2. Estrutura Base

```csharp
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilum.mvc.web.ViewModels.{Dominio};
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Route("nome-entidade")]        // Rota amigável (opcional)
    [Authorize]                      // Sempre presente
    public class NomeController : MainController
    {
        private readonly INomeService _nomeService;
        private readonly string _nomeEntidade = "Nome da Entidade";

        #region Construtor
        public NomeController(
            INomeService nomeService,
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
        {
            _nomeService = nomeService;
        }
        #endregion

        #region Index (Listagem)
        [HttpGet]
        public async Task<ActionResult> Index()
        {
            var empresa = ObterObjetoEmpresaSelecionada();
            if (empresa == null) return RedirectToAction("Index", "Home");

            var lista = await _nomeService.ObterTodas(
                Convert.ToInt64(empresa.IDEMPRESA));
            var viewModels = _mapper.Map<List<NomeViewModel>>(lista);

            return View(viewModels);
        }
        #endregion

        #region Create (Novo)
        [HttpGet]
        [ClaimsAuthorizeAttribute(XXXX)]  // idTag da permissão
        public async Task<ActionResult> Create()
        {
            ViewBag.operacao = "I";  // Insert
            ViewBag.acao = "Create";

            var model = new NomeViewModel();
            PopularListasAuxiliares(model);
            return View("CreateEdit", model);
        }

        [HttpPost]
        [ClaimsAuthorizeAttribute(XXXX)]
        public async Task<ActionResult> Create(NomeViewModel model)
        {
            ViewBag.operacao = "I";
            ViewBag.acao = "Create";

            if (!ModelState.IsValid) return View("CreateEdit", model);

            var entidade = _mapper.Map<Nome>(model);
            await _nomeService.Adicionar(entidade);

            if (!OperacaoValida())
            {
                PopularListasAuxiliares(model);
                return View("CreateEdit", model);
            }

            await _nomeService.Salvar();
            LogInformacao($"Criado: {model.Id}", _nomeEntidade, "Create", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";
            return RedirectToAction("Index");
        }
        #endregion

        #region Edit (Editar)
        [HttpGet]
        [ClaimsAuthorizeAttribute(XXXX)]
        public async Task<ActionResult> Edit(long id)
        {
            ViewBag.operacao = "E";  // Edit
            ViewBag.acao = "Edit";

            var entidade = await _nomeService.ObterPorId(id);
            if (entidade == null)
            {
                TempData["Erros"] = $"{_nomeEntidade} não localizada";
                return RedirectToAction("Index");
            }

            var model = _mapper.Map<NomeViewModel>(entidade);
            PopularListasAuxiliares(model);
            return View("CreateEdit", model);
        }

        [HttpPost]
        [ClaimsAuthorizeAttribute(XXXX)]
        public async Task<ActionResult> Edit(NomeViewModel model)
        {
            ViewBag.operacao = "E";
            ViewBag.acao = "Edit";

            if (!ModelState.IsValid) return View("CreateEdit", model);

            var entidade = _mapper.Map<Nome>(model);
            await _nomeService.Atualizar(entidade);

            if (!OperacaoValida())
            {
                PopularListasAuxiliares(model);
                return View("CreateEdit", model);
            }

            await _nomeService.Salvar();
            LogInformacao($"Atualizado: {model.Id}", _nomeEntidade, "Edit", null);

            TempData["Mensagem"] = "Operação realizada com sucesso";
            TempData["TipoMensagem"] = "success";
            return RedirectToAction("Index");
        }
        #endregion

        #region Métodos Privados
        private void PopularListasAuxiliares(NomeViewModel model)
        {
            // Popular dropdowns (empresas, categorias, etc.)
        }
        #endregion
    }
}
```

---

## Checklist do Controller

| Item | ✔️ |
|------|-----|
| Herda de `MainController` | ☐ |
| Tem `[Authorize]` na classe | ☐ |
| Tem `[ClaimsAuthorizeAttribute(idTag)]` nas ações sensíveis | ☐ |
| Construtor injeta dependências e chama `base(...)` | ☐ |
| `ModelState.IsValid` verificado no POST | ☐ |
| `OperacaoValida()` verificado após chamar Service | ☐ |
| `CreateEdit.cshtml` reutilizado para Create e Edit | ☐ |
| `ViewBag.operacao` = "I" ou "E" | ☐ |
| `ViewBag.acao` = "Create" ou "Edit" | ☐ |
| `TempData["Mensagem"]` para feedback pós-redirect | ☐ |
| `LogInformacao` / `LogErro` nas operações | ☐ |
| `ObterObjetoEmpresaSelecionada()` para obter empresa | ☐ |
| Sem lógica de negócio — apenas coordenação | ☐ |
| Sem acesso direto a Repository | ☐ |

---

## Exemplo Real

Veja o `CompraController` como referência completa:

- **Local:** `agilum.mvc.web/Controllers/CompraController.cs`
- **Características:** 8 serviços injetados, rotas customizadas, múltiplas ações (Create, Edit, Cancelar, Efetivar, ImportarXML)
