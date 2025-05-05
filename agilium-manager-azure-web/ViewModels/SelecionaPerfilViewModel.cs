using agilium.webapp.manager.mvc.ViewModels.ControleAcesso;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels
{

    public class SelecionarPerfilViewModel
    {
        public string IdAspNetUser { get; set; }
        [Display(Name = "Nome Usuário")]
        public string NomeUsuario { get; set; }
        [Display(Name = "Novo Perfil")]
        public long? idPerfil { get; set; }
        public List<PerfilIndexViewModel> Perfis { get; set; } = new List<PerfilIndexViewModel>();
        [Display(Name = "Perfil Atual")]
        public string PerfilAtual { get; set; }
    }
}
