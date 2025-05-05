using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Caixa: Entity
    {
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public Int64? IDTURNO { get; private set; }
        public virtual Turno Turno { get; private set; }
        public Int64? IDPDV { get; private set; }
        public virtual PontoVenda PontoVenda { get; private set; }
        public Int64? IDFUNC { get; private set; }
        public virtual Funcionario Funcionario { get; private set; }
        public int? SQCAIXA { get; private set; }
        public ESituacaoCaixa? STCAIXA { get; private set; }
        public DateTime? DTHRABT { get; private set; }
        public double? VLABT { get; private set; }
        public DateTime? DTHRFECH { get; private set; }
        public double? VLFECH { get; private set; }
        public virtual List<CaixaMoeda> CaixaMoeda { get; set; } = new List<CaixaMoeda>();
        public virtual List<CaixaMovimento> CaixaMovimento { get; set; } = new List<CaixaMovimento>();
        public virtual List<Venda> Venda { get; set; } = new List<Venda>();
        public virtual List<VendaTemporaria> VendaTemporaria { get; set; } = new List<VendaTemporaria>();
        public virtual List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public Caixa()
        {
            
        }

        public Caixa(long? iDEMPRESA, long? iDTURNO, long? iDPDV, long? iDFUNC, int? sQCAIXA, ESituacaoCaixa? sTCAIXA, DateTime? dTHRABT, double? vLABT, DateTime? dTHRFECH, double? vLFECH)
        {
            IDEMPRESA = iDEMPRESA;
            IDTURNO = iDTURNO;
            IDPDV = iDPDV;
            IDFUNC = iDFUNC;
            SQCAIXA = sQCAIXA;
            STCAIXA = sTCAIXA;
            DTHRABT = dTHRABT;
            VLABT = vLABT;
            DTHRFECH = dTHRFECH;
            VLFECH = vLFECH;
        }
    }
}
