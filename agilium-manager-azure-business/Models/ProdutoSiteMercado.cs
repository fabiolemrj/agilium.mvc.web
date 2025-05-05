using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ProdutoSiteMercado: Entity
    {
        public long? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public long? IDPRODUTO { get; private set; }
        public virtual Produto Produto { get; private set; }
        public string DSPROD { get; private set; }
        public double? NUQTDATACADO { get; private set; }
        public double? VLPROMOCAO { get; private set; }
        public double? VLATACADO { get; private set; }
        public double? VLCOMPRA { get; private set; }
        public ESituacaoProdutoSiteMercada? STDISPSITE { get; private set; }
        public EValidadeSiteMercado? STVALIDADEPROXIMA { get; private set; }
        public DateTime? DTHRATU { get; private set; }
        public ProdutoSiteMercado()
        {            
        }

        public ProdutoSiteMercado(long? iDEMPRESA, long? iDPRODUTO, string dSPROD, double? nUQTDATACADO, double? vLPROMOCAO, double? vLATACADO, double? vLCOMPRA, ESituacaoProdutoSiteMercada? sTDISPSITE, EValidadeSiteMercado? sTVALIDADEPROXIMA, DateTime? dTHRATU)
        {
            IDEMPRESA = iDEMPRESA;
            IDPRODUTO = iDPRODUTO;
            DSPROD = dSPROD;
            NUQTDATACADO = nUQTDATACADO;
            VLPROMOCAO = vLPROMOCAO;
            VLATACADO = vLATACADO;
            VLCOMPRA = vLCOMPRA;
            STDISPSITE = sTDISPSITE;
            STVALIDADEPROXIMA = sTVALIDADEPROXIMA;
            DTHRATU = dTHRATU;
        }
    }
}
