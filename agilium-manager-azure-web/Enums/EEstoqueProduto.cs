using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.Enums
{
    public enum EEstoqueProduto
    {
        [Display(Name = "Retorna o Produto ao Estoque")]
        RetornaEstoque = 0,
        [Display(Name = "Não Retorna o Produto ao Estoque")]
        NaoRetornaEstoque = 1
    }
}
