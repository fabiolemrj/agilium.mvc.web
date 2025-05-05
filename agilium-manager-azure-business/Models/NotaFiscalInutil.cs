using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class NotaFiscalInutil: Entity
    {
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public string CDNFINUTIL { get; private set; }
        public string DSMOTIVO { get; private set; }
        public int? NUANO { get; private set; }
        public string DSMODELO { get; private set; }
        public string DSSERIE { get; private set; }
        public int? NUNFINI { get; private set; }
        public int? NUNFFIM { get; private set; }
        public DateTime? DTHRINUTIL { get; private set; }
        public ESituacaoNFInutil? STINUTIL { get; private set; }
        public string DSPROTOCOLO { get; private set; }
        public string DSXML { get; private set; }
        public NotaFiscalInutil()
        {
            
        }

        public NotaFiscalInutil(long? iDEMPRESA, string cDNFINUTIL, string dSMOTIVO, int? nUANO, string dSMODELO, string dSSERIE, int? nUNFINI, int? nUNFFIM, DateTime? dTHRINUTIL, ESituacaoNFInutil? sTINUTIL, string dSPROTOCOLO, string dSXML)
        {
            IDEMPRESA = iDEMPRESA;
            CDNFINUTIL = cDNFINUTIL;
            DSMOTIVO = dSMOTIVO;
            NUANO = nUANO;
            DSMODELO = dSMODELO;
            DSSERIE = dSSERIE;
            NUNFINI = nUNFINI;
            NUNFFIM = nUNFFIM;
            DTHRINUTIL = dTHRINUTIL;
            STINUTIL = sTINUTIL;
            DSPROTOCOLO = dSPROTOCOLO;
            DSXML = dSXML;
        }
    }
}
