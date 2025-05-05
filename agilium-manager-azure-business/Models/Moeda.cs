using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Moeda: Entity
    {
        public long? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public string CDMOEDA { get; private set; }
        public string DSMOEDA { get; private set; }
        public EAtivo? STMOEDA { get; private set; }
        public ETipoDocFiscal? TPDOCFISCAL { get; private set; }
        public ETipoMoeda? TPMOEDA { get; private set; }
        public double? PCTAXA { get; private set; }
        public ESituacaoTroco? STTROCO { get; private set; }
        public string COR_BOTAO { get; private set; }
        public string COR_FONTE { get; private set; }
        public string TECLA_ATALHO { get; private set; }
        public virtual List<CaixaMoeda> CaixaMoeda { get; private set; }= new List<CaixaMoeda>();
        public virtual List<VendaMoeda> VendaMoeda { get; private set; } = new List<VendaMoeda>();
        public virtual List<MoedaSiteMercado> MoedasSiteMercados { get; set; } = new List<MoedaSiteMercado>();
        public virtual List<PedidoPagamento> PedidoPagamentos { get; set; } = new List<PedidoPagamento>();
        public virtual List<PedidoPagamentoSitemercado> PedidoPagamentoSitemercados { get; set; } = new List<PedidoPagamentoSitemercado>();
        public virtual List<VendaTemporariaMoeda> VendaTemporariaMoeda { get; set; } = new List<VendaTemporariaMoeda>();

        public Moeda()
        {            
        }

        public Moeda(long? iDEMPRESA, string cDMOEDA, string dSMOEDA, EAtivo? sTMOEDA, ETipoDocFiscal? tPDOCFISCAL, ETipoMoeda? tPMOEDA, double? pCTAXA, ESituacaoTroco? sTTROCO, string cOR_BOTAO, string cOR_FONTE, string tECLA_ATALHO)
        {
            IDEMPRESA = iDEMPRESA;
            CDMOEDA = cDMOEDA;
            DSMOEDA = dSMOEDA;
            STMOEDA = sTMOEDA;
            TPDOCFISCAL = tPDOCFISCAL;
            TPMOEDA = tPMOEDA;
            PCTAXA = pCTAXA;
            STTROCO = sTTROCO;
            COR_BOTAO = cOR_BOTAO;
            COR_FONTE = cOR_FONTE;
            TECLA_ATALHO = tECLA_ATALHO;
        }
    }
}
