
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IEmpresaService
    {
        Task<PagedViewModel<EmpresaViewModel>> ObterEmpresasPorRazaoSocial(string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, EmpresaCreateViewModel empresa);
        Task<ResponseResult> Adicionar(EmpresaCreateViewModel empresa);
        Task<EmpresaCreateViewModel> ObterPorId(string id);
        Task<EmpresaCreateViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<EmpresaViewModel>> ObterTodas();
    }
}
