using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class CompraItem: Entity
    {
        public Int64? IDCOMPRA { get; private set; }
        public virtual Compra Compra { get; private set; }
        public Int64? IDPRODUTO { get; private set; }
        public virtual Produto Produto { get; private set; }
        public Int64? IDESTOQUE { get; private set; }
        public virtual Estoque Estoque { get; private set; }
        public string DSPRODUTO { get; private set; }
        public string CDEAN { get; private set; }
        public string CDNCM { get; private set; }
        public string CDCEST { get; private set; }
        public string SGUN { get; private set; }
        public double? NUQTD { get; private set; }
        public double? NURELACAO { get; private set; }
        public double? VLUNIT { get; private set; }
        public double? VLTOTAL { get; private set; }
        public DateTime? DTVALIDADE { get; private set; }
        public int? NUCFOP { get; private set; }
        public double? VLOUTROS { get; private set; }
        public double? VLBSRET { get; private set; }
        public double? PCICMSRET { get; private set; }
        public double? PCREDUCAO { get; private set; }
        public string CDCSTICMS { get; private set; }
        public string CDCSTPIS { get; private set; }
        public string CDCSTCOFINS { get; private set; }
        public string CDCSTIPI { get; private set; }
        public double? VLALIQPIS { get; private set; }
        public double? VLALIQCOFINS { get; private set; }
        public double? VLALIQICMS { get; private set; }
        public double? VLALIQIPI { get; private set; }
        public double? VLBSCALCPIS { get; private set; }
        public double? VLBSCALCCOFINS { get; private set; }
        public double? VLBSCALCICMS { get; private set; }
        public double? VLBSCALCIPI { get; private set; }
        public double? VLICMS { get; private set; }
        public double? VLPIS { get; private set; }
        public double? VLCOFINS { get; private set; }
        public double? VLIPI { get; private set; }
        public string CDPRODFORN { get; private set; }
        public double? VLNOVOPRECOVENDA { get; private set; }
        public virtual List<EstoqueHistorico> EstoquesHistoricos { get; set; } = new List<EstoqueHistorico>();
        public CompraItem()
        {            
        }

        public CompraItem(long? iDCOMPRA, long? iDPRODUTO, string dSPRODUTO, string cDEAN, string cDNCM, string cDCEST, string sGUN, double? nUQTD, double? nURELACAO, double? vLUNIT, double? vLTOTAL, int? nUCFOP, double? vLOUTROS, double? vLBSRET, double? pCICMSRET, double? pCREDUCAO, string cDCSTICMS, string cDCSTPIS, string cDCSTCOFINS, string cDCSTIPI, double? vLALIQPIS, double? vLALIQCOFINS, double? vLALIQICMS, double? vLALIQIPI, double? vLBSCALCPIS, double? vLBSCALCCOFINS, double? vLBSCALCICMS, double? vLBSCALCIPI, double? vLICMS, double? vLPIS, double? vLCOFINS, double? vLIPI, string cDPRODFORN, double? vLNOVOPRECOVENDA)
        {
            IDCOMPRA = iDCOMPRA;
            IDPRODUTO = iDPRODUTO;
            DSPRODUTO = dSPRODUTO;
            CDEAN = cDEAN;
            CDNCM = cDNCM;
            CDCEST = cDCEST;
            SGUN = sGUN;
            NUQTD = nUQTD;
            NURELACAO = nURELACAO;
            VLUNIT = vLUNIT;
            VLTOTAL = vLTOTAL;
            NUCFOP = nUCFOP;
            VLOUTROS = vLOUTROS;
            VLBSRET = vLBSRET;
            PCICMSRET = pCICMSRET;
            PCREDUCAO = pCREDUCAO;
            CDCSTICMS = cDCSTICMS;
            CDCSTPIS = cDCSTPIS;
            CDCSTCOFINS = cDCSTCOFINS;
            CDCSTIPI = cDCSTIPI;
            VLALIQPIS = vLALIQPIS;
            VLALIQCOFINS = vLALIQCOFINS;
            VLALIQICMS = vLALIQICMS;
            VLALIQIPI = vLALIQIPI;
            VLBSCALCPIS = vLBSCALCPIS;
            VLBSCALCCOFINS = vLBSCALCCOFINS;
            VLBSCALCICMS = vLBSCALCICMS;
            VLBSCALCIPI = vLBSCALCIPI;
            VLICMS = vLICMS;
            VLPIS = vLPIS;
            VLCOFINS = vLCOFINS;
            VLIPI = vLIPI;
            CDPRODFORN = cDPRODFORN;
            VLNOVOPRECOVENDA = vLNOVOPRECOVENDA;
        }

        public void AdicionarItemImportadoNfe(long? iDCOMPRA, long? iDPRODUTO, long? iDESTOQUE, string dSPRODUTO, string cDEAN, string cDNCM, string cDCEST, string sGUN, double? nUQTD, double? nURELACAO, double? vLUNIT, double? vLTOTAL, DateTime? dTVALIDADE, int? nUCFOP, double? vLOUTROS, double? vLBSRET, double? pCICMSRET, double? pCREDUCAO, string cDCSTICMS, string cDCSTPIS, string cDCSTCOFINS, string cDCSTIPI, double? vLALIQPIS, double? vLALIQCOFINS, double? vLALIQICMS, double? vLALIQIPI, double? vLBSCALCPIS, double? vLBSCALCCOFINS, double? vLBSCALCICMS, double? vLBSCALCIPI, double? vLICMS, double? vLPIS, double? vLCOFINS, double? vLIPI, string cDPRODFORN, double? vLNOVOPRECOVENDA)
        {
            IDCOMPRA = iDCOMPRA;
            IDPRODUTO = iDPRODUTO;
            IDESTOQUE = iDESTOQUE;
            DSPRODUTO = dSPRODUTO;
            CDEAN = cDEAN;
            CDNCM = cDNCM;
            CDCEST = cDCEST;
            SGUN = sGUN;
            NUQTD = nUQTD;
            NURELACAO = nURELACAO;
            VLUNIT = vLUNIT;
            VLTOTAL = vLTOTAL;
            DTVALIDADE = dTVALIDADE;
            NUCFOP = nUCFOP;
            VLOUTROS = vLOUTROS;
            VLBSRET = vLBSRET;
            PCICMSRET = pCICMSRET;
            PCREDUCAO = pCREDUCAO;
            CDCSTICMS = cDCSTICMS;
            CDCSTPIS = cDCSTPIS;
            CDCSTCOFINS = cDCSTCOFINS;
            CDCSTIPI = cDCSTIPI;
            VLALIQPIS = vLALIQPIS;
            VLALIQCOFINS = vLALIQCOFINS;
            VLALIQICMS = vLALIQICMS;
            VLALIQIPI = vLALIQIPI;
            VLBSCALCPIS = vLBSCALCPIS;
            VLBSCALCCOFINS = vLBSCALCCOFINS;
            VLBSCALCICMS = vLBSCALCICMS;
            VLBSCALCIPI = vLBSCALCIPI;
            VLICMS = vLICMS;
            VLPIS = vLPIS;
            VLCOFINS = vLCOFINS;
            VLIPI = vLIPI;
            CDPRODFORN = cDPRODFORN;
            VLNOVOPRECOVENDA = vLNOVOPRECOVENDA;
        }

        public void AdicionarProduto(Produto produto) => Produto = produto;
        public void AdicionarEstoqueHistorico(EstoqueHistorico estoqueHistorico)=> EstoquesHistoricos.Add(estoqueHistorico);
    }
}
