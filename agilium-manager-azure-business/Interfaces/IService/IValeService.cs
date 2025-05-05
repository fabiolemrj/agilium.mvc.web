using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IValeService: IDisposable
    {
        Task Salvar();
        Task Adicionar(Vale vale);
        Task Atualizar(Vale vale);
        Task Apagar(long id);
        Task<Vale> ObterPorId(long id);
        Task<PagedResult<Vale>> ObterValePorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<IEnumerable<Vale>> ObterTodas(long idEmpresa);
        Task<string> GerarCodigoBarraVale();
        Task CancelarVale(long id);
        Task<long> GerarVale(long idEmpresa, long? idCliente, ETipoVale tipoVale, double valor);
        Task<long> GerarVale(long idDevolucao);
        Task<bool> UtilizarValePorVenda(long idVenda);
    }
}
