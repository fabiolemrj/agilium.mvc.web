using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class VendaMoeda: Entity
    {
        public Int64? IDVENDA { get; private set; }
        public virtual Venda Venda { get; private set; }
        public Int64? IDMOEDA { get; private set; }
        public virtual Moeda Moeda { get; private set; }
        public Int64? IDVALE { get; private set; }
        public virtual Vale Vale { get; private set; }
        public double? VLPAGO { get; private set; }
        public double? VLTROCO { get; private set; }
        public int? NUPARCELAS { get; private set; }
        public string NSU { get; private set; }
        public virtual List<PedidoPagamentoSitemercado> PedidoPagamentoSitemercados { get; set; } = new List<PedidoPagamentoSitemercado>();
        public VendaMoeda()
        {            
        }

        public VendaMoeda(long? iDVENDA, long? iDMOEDA, long? iDVALE, double? vLPAGO, double? vLTROCO, int? nUPARCELAS, string nSU)
        {
            IDVENDA = iDVENDA;
            IDMOEDA = iDMOEDA;
            IDVALE = iDVALE;
            VLPAGO = vLPAGO;
            VLTROCO = vLTROCO;
            NUPARCELAS = nUPARCELAS;
            NSU = nSU;
        }

        public void AdicionarMoeda(Moeda moeda) => Moeda = moeda;
    }
}
