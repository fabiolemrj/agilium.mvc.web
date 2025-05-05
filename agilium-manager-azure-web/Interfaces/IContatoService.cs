using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Contato;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IContatoService
    {
        Task<ContatoEmpresaViewModel> ObterPorId(long idContato, long idEmpresa);
        Task<ResponseResult> Adicionar(ContatoEmpresaViewModel contatoEmpresa);
        Task<ResponseResult> Atualizar(ContatoEmpresaViewModel contatoEmpresa);
        Task<ResponseResult> RemoverContatoEmpresa(long idContato, long idEmpresa);
    }
}
