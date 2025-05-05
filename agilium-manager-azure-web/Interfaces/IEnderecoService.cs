using agilium.webapp.manager.mvc.ViewModels.Endereco;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IEnderecoService
    {
        Task<CepViewModel> ObterCepPorNumeroCep(string cep);
    }
}
