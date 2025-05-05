using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.Enums
{
    public enum ETipoFiscal
    {
        [Display(Name = "Distribuição")]
        Distribuicao = 1,
        [Display(Name = "Industria")]
        Industria = 2,
        [Display(Name = "Simples Nacional")]
        SimplesNacional = 3
    }
}
