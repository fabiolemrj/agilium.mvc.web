using agilium.api.manager.ViewModels.ControleAcessoViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels
{
    public class SelecionarPerfilViewModel
    {
        public string IdAspNetUser { get; set; }
        [Display(Name = "Nome Usuário")]
        public string NomeUsuario { get; set; }
        [Display(Name = "Perfil")]
        public long? idPerfil { get; set; }
        public string PerfilAtual { get; set; }
        public List<PerfilIndexViewModel> Perfis { get; set; }=new List<PerfilIndexViewModel>();
    }
}
