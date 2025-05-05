using System.Collections;
using System.Collections.Generic;

namespace agilium.api.manager.ViewModels.CaManagerViewModel
{
    public class CaAreaManagerViewModel
    {
        public int IdArea { get; set; }
        public int hierarquia { get; set; }

        public int ordem { get; set; }
        public string titulo { get; set; }
        public int subitem { get; set; }
        public int idtag { get; set; }
        public bool Selecao { get; set; }
    }

    public class CaPerfilManagerViewModel
    {

        public int IdPerfil { get; set; }
        public string Descricao { get; set; }
    }

    public class CaPermissaoPerfilViewModel
    {
        public CaPerfilManagerViewModel Perfil { get; set; }= new CaPerfilManagerViewModel();
        public List<CaAreaManagerViewModel> Area { get; set; }= new List<CaAreaManagerViewModel>();
        
    }

    public class CaAreaManagerSalvarViewModel
    {
        public int IdPerfil { get; set; }
        public int IdArea { get; set; }
    }
}
