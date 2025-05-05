using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class VendaFiscal: Entity
    {
        public Int64? IDVENDA { get; private set; }
        public virtual Venda Venda { get; private set; }
        public int? TPDOC { get; private set; }
        public string DSXML { get; private set; }
        public int? STDOCFISCAL { get; private set; }
        public DateTime? DTHREMISSAO { get; private set; }
        public VendaFiscal()
        {            
        }

        public VendaFiscal(long? iDVENDA, int? tPDOC, string dSXML, int? sTDOCFISCAL, DateTime? dTHREMISSAO)
        {
            IDVENDA = iDVENDA;
            TPDOC = tPDOC;
            DSXML = dSXML;
            STDOCFISCAL = sTDOCFISCAL;
            DTHREMISSAO = dTHREMISSAO;
        }
    }
}
