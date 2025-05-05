using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models
{
    public class VendaTemporariaItem : Entity
    {
        public Int64? IDVENDA { get; private set; }
        public virtual VendaTemporaria Venda { get; private set; }
        public Int64? IDPRODUTO { get; private set; }
        public virtual Produto Produto { get; private set; }
        public int? SQITEM { get; private set; }
        public double? VLUNIT { get; private set; }
        public double? NUQTD { get; private set; }
        public double? VLITEM { get; private set; }
        public double? VLACRES { get; private set; }
        public double? VLDESC { get; private set; }
        public double? VLTOTAL { get; private set; }
        public double? VLCUSTOMEDIO { get; private set; }
        public int? STITEM { get; private set; }
        public double? PCIBPTFED { get; private set; }
        public double? PCIBPTEST { get; private set; }
        public double? PCIBPTMUN { get; private set; }
        public double? PCIBPTIMP { get; private set; }
        public VendaTemporariaItem()
        {

        }

        public VendaTemporariaItem(int? sQITEM, double? vLUNIT, double? nUQTD, double? vLITEM, double? vLACRES, double? vLDESC, double? vLTOTAL, double? vLCUSTOMEDIO, int? sTITEM, double? pCIBPTFED, double? pCIBPTEST, double? pCIBPTMUN, double? pCIBPTIMP)
        {
            SQITEM = sQITEM;
            VLUNIT = vLUNIT;
            NUQTD = nUQTD;
            VLITEM = vLITEM;
            VLACRES = vLACRES;
            VLDESC = vLDESC;
            VLTOTAL = vLTOTAL;
            VLCUSTOMEDIO = vLCUSTOMEDIO;
            STITEM = sTITEM;
            PCIBPTFED = pCIBPTFED;
            PCIBPTEST = pCIBPTEST;
            PCIBPTMUN = pCIBPTMUN;
            PCIBPTIMP = pCIBPTIMP;
        }
    }
}
