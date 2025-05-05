using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Funcionario;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IFuncionarioService
    {
        Task<PagedViewModel<FuncionarioViewModel>> ObterPorNome(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long id, FuncionarioViewModel fornecedorViewModel);
        Task<ResponseResult> Adicionar(FuncionarioViewModel fornecedorViewModel);
        Task<FuncionarioViewModel> ObterPorId(long id);
        Task<ResponseResult> Remover(long id);
        Task<IEnumerable<FuncionarioViewModel>> ObterTodas();
        Task<FuncionarioViewModel> ObterListasAuxiliares();
    }
}
