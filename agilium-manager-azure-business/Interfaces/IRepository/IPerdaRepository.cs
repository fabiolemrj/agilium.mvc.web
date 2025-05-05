using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IRepository
{
    public interface IPerdaRepository : IRepository<Perda>
    {
    }

    public interface IPerdaDapperRepository
    {
        Task<long> lancarPerdaRetornaIdHistoricoGerado(long idPerda, string usuarioPerda);
        Task<bool> InserirPerda(long idEmpresa, long idEstoque, long idPerda, long idProduto, long idUsuario, ETipoPerda tipoPerda, ETipoMovimentoPerda eTipoMovimentoPerda,
            double quantidadePerda, string Observacao);
    }
}
