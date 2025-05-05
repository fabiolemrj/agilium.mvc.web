using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models
{
    public class CaAreaManager: Entity
    {
        public int IdArea { get; set; }
        public int hierarquia { get; set; }
        public int ordem { get; set; }
        public string titulo { get; set; }
        public int subitem { get; set; }
        public int idtag { get; set; }

        public virtual List<CaPermissaoManager> CaPermissaoManagers { get; set; }=new List<CaPermissaoManager>() { };
    }
}
