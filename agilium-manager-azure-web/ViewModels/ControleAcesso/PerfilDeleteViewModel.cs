using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.ControleAcesso
{
    public class PerfilDeleteViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Descrição")]
        public string Descricao { get; set; }
        [Display(Name = "Situação")]
        public string Situacao { get; set; }

        public PerfilDeleteViewModel()
        {

        }
        public PerfilDeleteViewModel(long id, string descricao, string situacao)
        {
            Id = id;
            Descricao = descricao;
            Situacao = situacao;
        }
    }
}
