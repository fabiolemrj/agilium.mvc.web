using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class VendaItem: Entity
    {
        public Int64? IDVENDA { get; private set; }
        public virtual Venda Venda { get; private set; }
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
        public ESituacaoItemVenda? STITEM { get; private set; }
        public double? PCIBPTFED { get; private set; }
        public double? PCIBPTEST { get; private set; }
        public double? PCIBPTMUN { get; private set; }
        public double? PCIBPTIMP { get; private set; }

        public virtual List<DevolucaoItem> DevolucaoItem { get; set; } = new List<DevolucaoItem>();
        public virtual List<PedidoItemSitemercado> PedidoItemSiteMercado { get; set; } = new List<PedidoItemSitemercado>();
        public virtual List<PedidoVendaItem> PedidosVendaItems { get; set; } = new List<PedidoVendaItem>();

        public VendaItem()
        {            
        }

        public VendaItem(long? iDVENDA, long? iDPRODUTO, int? sQITEM, double? vLUNIT, double? nUQTD, double? vLITEM, double? vLACRES, double? vLDESC, double? vLTOTAL, double? vLCUSTOMEDIO, ESituacaoItemVenda? sTITEM, double? pCIBPTFED, double? pCIBPTEST, double? pCIBPTMUN, double? pCIBPTIMP)
        {
            IDVENDA = iDVENDA;
            IDPRODUTO = iDPRODUTO;
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

        public void AdicionarProduto(Produto produto) => Produto = produto;


    }
}
