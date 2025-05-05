using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IContatoService: IDisposable
    {
        Task Adicionar(Contato contato);
        Task Atualizar(Contato contato);
        Task Apagar(long id);
        Task<Contato> ObterPorId(long id);

        #region Empresa
        Task Adicionar(ContatoEmpresa contatoEmpresa);
        Task Atualizar(ContatoEmpresa contatoEmpresa);
        Task Apagar(long idContato, long idEmpresa);
        Task<ContatoEmpresa> ObterPorId(long idContato, long idEmpresa);
        #endregion

        Task Salvar();
    }
}
