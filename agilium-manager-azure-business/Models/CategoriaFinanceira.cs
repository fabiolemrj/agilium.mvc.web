using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class CategoriaFinanceira : Entity
    {
        public string NMCATEG { get; private set; }
        public EAtivo STCATEG { get; private set; }
        public virtual List<ContaPagar> ContaPagar { get; set; } = new List<ContaPagar>();
        public virtual List<ContaReceber> ContaReceber { get; set; } = new List<ContaReceber>();
        public CategoriaFinanceira()
        {
        }

        public CategoriaFinanceira(string nMCATEG, EAtivo sTCATEG)
        {          
            NMCATEG = nMCATEG;
            STCATEG = sTCATEG;
        }
    }
}
