using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.UnidadeViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IUnidadeService
    {
        Task<PagedViewModel<UnidadeIndexViewModel>> ObterPorRazaoSocial(string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, UnidadeIndexViewModel model);
        Task<ResponseResult> Adicionar(UnidadeIndexViewModel model);
        Task<UnidadeIndexViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<ResponseResult> MudarSituacao(long id);
        Task<List<UnidadeIndexViewModel>> ObterTodas();
    }
}
