using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IValeRepository: IRepository<Vale>
    {
    }

    public interface IValeDapperRepository
    {
        Task<string> GerarCodigo(long idEmpresa);
        Task<long> GerarValeAtualizaDevolucao( long idDevolucao);
        Task<string> GerarCodigoBarra();
        Task<bool> AtualizarDevolucaoComVale(long idDevolucao, long idvale);
        Task<bool> UtilizarValePorVenda(long idVenda);
    }
}
