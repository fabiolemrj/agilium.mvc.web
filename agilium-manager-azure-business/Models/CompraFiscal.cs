using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class CompraFiscal: Entity
    {
        public Int64? IDCOMPRA { get; private set; }
        public virtual Compra Compra { get; private set; }
        public ETipoManifestoCompra? STMANIFESTO { get; private set; }
        public string DSXML { get; private set; }
        public CompraFiscal()
        {            
        }

        public CompraFiscal(long? iDCOMPRA, ETipoManifestoCompra? sTMANIFESTO, string dSXML)
        {
            IDCOMPRA = iDCOMPRA;
            STMANIFESTO = sTMANIFESTO;
            DSXML = dSXML;
        }
    }
}
