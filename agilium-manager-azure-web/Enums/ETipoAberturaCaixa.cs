using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Enums
{
    public enum ETipoAberturaCaixa
    {
        [Description("Abrir com saldo do caixa anterior")]
        SaldoCaixaAnterior = 1 ,
        [Description("Abrir com saldo zerado")]
        SaldoZerado = 2
    }
}
