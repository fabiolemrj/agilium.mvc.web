using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Vale: Entity
    {
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public Int64? IDCLIENTE { get; private set; }
        public virtual Cliente Cliente { get; private set; }
        public string CDVALE { get; private set; }
        public DateTime? DTHRVALE { get; private set; }
        public ETipoVale? TPVALE { get; private set; }
        public ESituacaoVale? STVALE { get; private set; }
        public double? VLVALE { get; private set; }
        public string CDBARRA { get; private set; }
        public virtual List<VendaMoeda> VendaMoeda { get; private set; } = new List<VendaMoeda>();
        public virtual List<Devolucao> Devolucao { get; private set; } = new List<Devolucao>();
        public Vale()
        {            
        }

        public Vale(long? iDEMPRESA, long? iDCLIENTE, string cDVALE, DateTime? dTHRVALE, ETipoVale? tPVALE, ESituacaoVale? sTVALE, double? vLVALE, string cDBARRA)
        {
            IDEMPRESA = iDEMPRESA;
            IDCLIENTE = iDCLIENTE;
            CDVALE = cDVALE;
            DTHRVALE = dTHRVALE;
            TPVALE = tPVALE;
            STVALE = sTVALE;
            VLVALE = vLVALE;
            CDBARRA = cDBARRA;
        }

        public void Cancelar() => STVALE = ESituacaoVale.Cancelado;
        public void Ativar() => STVALE = ESituacaoVale.Ativo;
        public void Utilizar() => STVALE = ESituacaoVale.Utilizado;
    }
}
