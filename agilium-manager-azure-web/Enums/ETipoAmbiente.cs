using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Enums
{
    public enum ETipoAmbiente
    {
        [Description("Homologação")]
        Homologacao =1,
        [Description("Produção")]
        Producao =2
    }
}
