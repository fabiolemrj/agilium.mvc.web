using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;

namespace agilium.api.business.Models
{
    public class Compra: Entity
    {
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public Int64? IDFORN { get; private set; }
        public virtual Fornecedor Fornecedor { get; private set; }
        public Int64? IDTURNO { get; private set; }
        public virtual Turno Turno { get; private set; }
        public DateTime? DTCOMPRA { get; private set; }
        public DateTime? DTCAD { get; private set; }
        public string CDCOMPRA { get; private set; }
        public ESituacaoCompra? STCOMPRA { get; private set; }
        public DateTime? DTNF { get; private set; }
        public string NUNF { get; private set; }
        public string DSSERIENF { get; private set; }
        public string DSCHAVENFE { get; private set; }
        public ETipoCompravanteCompra? TPCOMPROVANTE { get; private set; }
        public int? NUCFOP { get; private set; }
        public double? VLICMSRETIDO { get; private set; }
        public double? VLBSCALCICMS { get; private set; }
        public double? VLICMS { get; private set; }
        public double? VLBSCALCSUB { get; private set; }
        public double? VLICMSSUB { get; private set; }
        public double? VLISENCAO { get; private set; }
        public double? VLTOTPROD { get; private set; }
        public double? VLFRETE { get; private set; }
        public double? VLSEGURO { get; private set; }
        public double? VLDESCONTO { get; private set; }
        public double? VLOUTROS { get; private set; }
        public double? VLIPI { get; private set; }
        public double? VLTOTAL { get; private set; }
        public string DSOBS { get; private set; }
        public int? STIMPORTADA { get; private set; }

        public virtual List<CompraFiscal> ComprasFiscais { get; set; } = new List<CompraFiscal>();
        public virtual List<CompraItem> Itens { get; set; } = new List<CompraItem>();
        public Compra()
        {            
        }

        public void AtualizarImportacaoNfe(DateTime? dTCOMPRA, DateTime? dTNF, string nUNF, string dSSERIENF, string dSCHAVENFE, ETipoCompravanteCompra? tPCOMPROVANTE, int? nUCFOP, double? vLICMSRETIDO, double? vLBSCALCICMS, double? vLICMS, double? vLBSCALCSUB, double? vLICMSSUB, double? vLTOTPROD, double? vLFRETE, double? vLSEGURO, double? vLDESCONTO, double? vLOUTROS, double? vLIPI, double? vLTOTAL, string dSOBS, int? sTIMPORTADA)
        {
            DTCOMPRA = dTCOMPRA;
            DTNF = dTNF;
            NUNF = nUNF;
            DSSERIENF = dSSERIENF;
            DSCHAVENFE = dSCHAVENFE;
            TPCOMPROVANTE = tPCOMPROVANTE;
            NUCFOP = nUCFOP;
            VLICMSRETIDO = vLICMSRETIDO;
            VLBSCALCICMS = vLBSCALCICMS;
            VLICMS = vLICMS;
            VLBSCALCSUB = vLBSCALCSUB;
            VLICMSSUB = vLICMSSUB;
            VLTOTPROD = vLTOTPROD;
            VLFRETE = vLFRETE;
            VLSEGURO = vLSEGURO;
            VLDESCONTO = vLDESCONTO;
            VLOUTROS = vLOUTROS;
            VLIPI = vLIPI;
            VLTOTAL = vLTOTAL;
            DSOBS = dSOBS;
            STIMPORTADA = sTIMPORTADA;
        }

        public Compra(long? iDEMPRESA, long? iDFORN, long? iDTURNO, DateTime? dTCOMPRA, DateTime? dTCAD, string cDCOMPRA, ESituacaoCompra? sTCOMPRA, DateTime? dTNF, string nUNF, string dSSERIENF, string dSCHAVENFE, ETipoCompravanteCompra? tPCOMPROVANTE, int? nUCFOP, double? vLICMSRETIDO, double? vLBSCALCICMS, double? vLICMS, double? vLBSCALCSUB, double? vLICMSSUB, double? vLISENCAO, double? vLTOTPROD, double? vLFRETE, double? vLSEGURO, double? vLDESCONTO, double? vLOUTROS, double? vLIPI, double? vLTOTAL, string dSOBS, int? sTIMPORTADA)
        {
            IDEMPRESA = iDEMPRESA;
            IDFORN = iDFORN;
            IDTURNO = iDTURNO;
            DTCOMPRA = dTCOMPRA;
            DTCAD = dTCAD;
            CDCOMPRA = cDCOMPRA;
            STCOMPRA = sTCOMPRA;
            DTNF = dTNF;
            NUNF = nUNF;
            DSSERIENF = dSSERIENF;
            DSCHAVENFE = dSCHAVENFE;
            TPCOMPROVANTE = tPCOMPROVANTE;
            NUCFOP = nUCFOP;
            VLICMSRETIDO = vLICMSRETIDO;
            VLBSCALCICMS = vLBSCALCICMS;
            VLICMS = vLICMS;
            VLBSCALCSUB = vLBSCALCSUB;
            VLICMSSUB = vLICMSSUB;
            VLISENCAO = vLISENCAO;
            VLTOTPROD = vLTOTPROD;
            VLFRETE = vLFRETE;
            VLSEGURO = vLSEGURO;
            VLDESCONTO = vLDESCONTO;
            VLOUTROS = vLOUTROS;
            VLIPI = vLIPI;
            VLTOTAL = vLTOTAL;
            DSOBS = dSOBS;
            STIMPORTADA = sTIMPORTADA;
        }

        public void AdicionarItem(CompraItem item) => Itens.Add(item);
        public void AdicionarFiscal(CompraFiscal fiscal) => ComprasFiscais.Add(fiscal);
    }
}
