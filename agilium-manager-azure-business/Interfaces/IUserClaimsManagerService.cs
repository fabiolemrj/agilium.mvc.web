using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces
{
    public interface IUserClaimsManagerService : IDisposable
    {
        Task<bool> Adicionar(ObjetoClaim objetoClaim);
        Task<bool> Atualizar(ObjetoClaim objetoClaim);
        Task<bool> Remover(long id);

        Task<ObjetoClaim> ObterClaimPorType(string typeClaim);
        Task<List<ObjetoClaim>> ObterTodos();
        Task<ObjetoClaim> ObterClaimPorId(long id);
        Task<List<string>> ObterListaClaim();
        Task<List<ClaimValue>> ObterListaClaimValues();
        Task<ClaimValue> ObterClaimValuePOrValue(string value);
    }
}
