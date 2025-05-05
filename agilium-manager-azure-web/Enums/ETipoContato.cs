using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Enums
{
    public enum ETipoContato
    {
        [Description("E-mail")]
        Email = 1,
        [Description("Facebook")]
        Facebook = 2,
        [Description("Nextel")]
        Nextel = 3,
        [Description("Site")]
        Site = 4,
        [Description("Celular")]
        TelefoneCelular = 5,
        [Description("Tel. Comercial")]
        TelefoneComercial = 6,
        [Description("Tel. Recado")]
        TelefoneRecado = 7,
        [Description("Tel. Residencial")]
        TelefoneResidencial = 8
    }
}
