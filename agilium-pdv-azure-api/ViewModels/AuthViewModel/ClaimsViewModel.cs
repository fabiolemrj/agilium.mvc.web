using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.pdv.ViewModels
{
    public class ClaimSelecionadaViewModel
    {
        public string claim { get; set; }
        public List<string> ClaimValue { get; set; } = new List<string>();

    
    }

    public class ClaimAcaoIndividualPorUsuario
    {
        public string IdUserAspNet { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }
}
