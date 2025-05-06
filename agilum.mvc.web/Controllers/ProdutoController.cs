using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Produtos;
using agilum.mvc.web.ViewModels.UnidadeViewModel;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;

namespace agilum.mvc.web.Controllers
{
    [Route("produto")]
    [Authorize]
    public class ProdutoController : MainController
    {
        private readonly IProdutoService _produtoService;
 

        private readonly string _nomeEntidadeDepart = "Departamento";
        private readonly string _nomeEntidadeMarca = "Marca";
        private readonly string _nomeEntidadeGrupo = "Grupo de Produtos";
        private readonly string _nomeEntidadeSubGrupo = "SubGrupo de Produtos";

        #region Listas Auxiliares
        private List<EmpresaViewModel> listaEmpresaViewModels { get; set; } = new List<EmpresaViewModel>();

        #endregion
        public ProdutoController( INotificador notificador, IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, configuration, appUser, utilDapperRepository, logService)
        {

       

        }

        #region GrupoProduto

        #endregion
      
    }
}
