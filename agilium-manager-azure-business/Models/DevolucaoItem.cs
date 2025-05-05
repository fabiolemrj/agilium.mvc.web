using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class DevolucaoItem: Entity
    {
        public virtual Devolucao Devolucao { get; private set; }
        public Int64? IDDEV { get; private set; }
        public virtual VendaItem VendaItem { get; private set; }
        public Int64? IDVENDA_ITEM { get; private set; }
        public double? NUQTD { get; private set; }
        public double? VLITEM { get; private set; }

        public DevolucaoItem()
        {            
        }

        public DevolucaoItem(long? iDDEV, long? iDVENDA_ITEM, double? nUQTD, double? vLITEM)
        {
            IDDEV = iDDEV;
            IDVENDA_ITEM = iDVENDA_ITEM;
            NUQTD = nUQTD;
            VLITEM = vLITEM;
        }
    }
}
