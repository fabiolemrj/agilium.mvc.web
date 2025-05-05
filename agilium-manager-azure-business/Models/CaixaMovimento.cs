using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class CaixaMovimento: Entity
    {
        public Int64? IDCAIXA { get; private set; }
        public virtual Caixa Caixa { get; private set; }
        public ETipoMovCaixa? TPMOV { get; private set; }
        public string DSMOV { get; private set; }
        public double? VLMOV { get; private set; }
        public ESituacaoMovCaixa? STMOV { get; private set; }
        public CaixaMovimento()
        {            
        }

        public CaixaMovimento(long? iDCAIXA, ETipoMovCaixa? tPMOV, string dSMOV, double? vLMOV, ESituacaoMovCaixa? sTMOV)
        {
            IDCAIXA = iDCAIXA;
            TPMOV = tPMOV;
            DSMOV = dSMOV;
            VLMOV = vLMOV;
            STMOV = sTMOV;
        }
    }
}
