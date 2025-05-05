using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models
{
    public class CaPerfiManager:Entity
    {
        public int IdPerfil { get; set; }
        public string Descricao { get; set; }

        public virtual List<CaPermissaoManager> CaPermissaoManagers { get; set; } = new List<CaPermissaoManager>();
    }
}
