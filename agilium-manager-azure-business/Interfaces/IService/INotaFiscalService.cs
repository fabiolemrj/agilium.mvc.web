using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface INotaFiscalService: IDisposable
    {
        Task Salvar();

        #region Nota Fiscal Inutil
        Task Adicionar(NotaFiscalInutil nf);
        Task Atualizar(NotaFiscalInutil nf);
        Task ApagarNFInutil(long id);
        Task<NotaFiscalInutil> ObterNFInutilPorId(long id);
        //Task<IEnumerable<NotaFiscalInutil>> ObterNFInutilPorDescricao(string descricao);
        Task<PagedResult<NotaFiscalInutil>> ObterNFInutilPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<NotaFiscalInutil> ObterNFInutilCompletoPorId(long id);
        Task<IEnumerable<NotaFiscalInutil>> ObterTodasNFInutil(long idEmpresa);
        #endregion
    }
}
