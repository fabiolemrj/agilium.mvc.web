using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models
{
    public class CaPermissaoManager: Entity
    {
        public int IdPerfil { get; set; }
        public virtual CaPerfiManager CaPerfiManager { get; set; }=new CaPerfiManager();
        public int IdArea { get; set; }
        public virtual CaAreaManager CaAreaManager { get; set; }=new CaAreaManager();
    }
}
