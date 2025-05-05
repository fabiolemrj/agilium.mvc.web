using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;

namespace agilium.api.business.Enums
{
    public enum ECodigoRegimeTributario
    {
        [Description("Simples Nacional")]
        SimplesNacional = 1,
        [Description("Simples Excesso Receita Bruta")]
        SimplesExcessoReceitaBruta = 2,
        [Description("Regime Normal")]
        RegimeNormal = 3
    }
}
