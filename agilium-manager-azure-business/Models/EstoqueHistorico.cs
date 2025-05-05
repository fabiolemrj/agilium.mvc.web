using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class EstoqueHistorico : Entity
    {
        public virtual Estoque Estoque { get; private set; }
        public Int64? IDESTOQUE { get; private set; }
        public virtual Produto Produto { get; private set; }
        public Int64? IDPRODUTO { get; private set; }
        public Int64? IDITEM { get; private set; }
        public virtual CompraItem CompraItem { get; set; }
        public Int64? IDLANC { get; private set; }
        public virtual PlanoContaLancamento PlanoContaLancamento { get; set; }
        public DateTime? DTHRHST { get; private set; }
        public string NMUSUARIO { get; private set; }
        public int? TPHST { get; private set; }
        public string DSHST { get; private set; }
        public double? QTDHST { get; private set; }
        public virtual List<Perda> Perdas { get; set; } = new List<Perda>();
        public EstoqueHistorico()
        {            
        }

        public EstoqueHistorico(long? iDESTOQUE, long? iDPRODUTO, long? iDITEM, long? iDLANC, DateTime? dTHRHST, string nMUSUARIO, int? tPHST, string dSHST, double? qTDHST)
        {
            IDESTOQUE = iDESTOQUE;
            IDPRODUTO = iDPRODUTO;
            IDITEM = iDITEM;
            IDLANC = iDLANC;
            DTHRHST = dTHRHST;
            NMUSUARIO = nMUSUARIO;
            TPHST = tPHST;
            DSHST = dSHST;
            QTDHST = qTDHST;
        }
        //public virtual List<Perda> Perdas { get; set; }
        public void AdicionarPlanoConta(PlanoContaLancamento planoConta) => PlanoContaLancamento = planoConta;


    }
}
