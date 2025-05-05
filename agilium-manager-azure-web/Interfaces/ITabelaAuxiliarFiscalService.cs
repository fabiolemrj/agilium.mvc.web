using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using agilium.webapp.manager.mvc.ViewModels.ImpostoViewModel;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface ITabelaAuxiliarFiscalService
    {
        Task<TabelasAxuliaresFiscalViewModel> ObterTabelasAuxiliaresFiscal();
    }
}
