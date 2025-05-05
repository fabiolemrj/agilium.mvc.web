using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace agilium.webapp.manager.mvc.Enums
{
    public enum EOrigemProduto
    {
        [Display(Name = "Nacional")]
        Nacional = 1,
        [Display(Name ="Estrangeira Importação Direta")]
        EstrangeiraImportacaoDireta = 2,
        [Display(Name ="Estrangeira Adquirida Brasil")]
        EstrangeiraAdquiridaBrasil = 3
    }
}
