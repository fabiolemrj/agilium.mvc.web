using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PontoVenda: Entity
    {
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public string CDPDV { get; private set; }
        public string DSPDV { get; private set; }
        public string NMMAQUINA { get; private set; }
        public string DSCAMINHO_CERT { get; private set; }
        public string DSSENHA_CERT { get; private set; }
        public Int64? IDESTOQUE { get; private set; }
        public virtual Estoque Estoque { get; private set; }
        //NMIMPRESSORA varchar(100)
        public string NMIMPRESSORA { get; private set; }
        //STGAVETA int (11) 
        public ESimNao? STGAVETA { get; private set; }
        //DSPORTAIMPRESSORA varchar(20)
        public string DSPORTAIMPRESSORA { get; private set; }
        //CDMODELOBAL int (11)
        public int? CDMODELOBAL { get; private set; }
        //CDHANDSHAKEBAL int (11)
        public int? CDHANDSHAKEBAL { get; private set; }
        //CDPARITYBAL int (11) 
        public int? CDPARITYBAL { get; private set; }
        //CDSERIALSTOPBITBAL int (11)
        public int? CDSERIALSTOPBITBAL { get; private set; }
        //NUDATABITBAL int (11)
        public int? NUDATABITBAL { get; private set; }
        //NUBAUDRATEBAL int (11)
        public int? NUBAUDRATEBAL { get; private set; }
        //DSPORTABAL varchar(20)
        public string DSPORTABAL { get; private set; }
        public EAtivo? STPDV { get; private set; }
        public virtual List<Caixa> Caixas { get; set; } = new List<Caixa>();
        public virtual List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public PontoVenda()
        {            
        }

        public PontoVenda(long? iDEMPRESA, string cDPDV, string dSPDV, string nMMAQUINA, string dSCAMINHO_CERT, string dSSENHA_CERT, long? iDESTOQUE, string nMIMPRESSORA, ESimNao? sTGAVETA, string dSPORTAIMPRESSORA, int? cDMODELOBAL, int? cDHANDSHAKEBAL, int? cDPARITYBAL, int? cDSERIALSTOPBITBAL, int? nUDATABITBAL, int? nUBAUDRATEBAL, string dSPORTABAL, EAtivo? sTPDV)
        {
            IDEMPRESA = iDEMPRESA;
            CDPDV = cDPDV;
            DSPDV = dSPDV;
            NMMAQUINA = nMMAQUINA;
            DSCAMINHO_CERT = dSCAMINHO_CERT;
            DSSENHA_CERT = dSSENHA_CERT;
            IDESTOQUE = iDESTOQUE;
            NMIMPRESSORA = nMIMPRESSORA;
            STGAVETA = sTGAVETA;
            DSPORTAIMPRESSORA = dSPORTAIMPRESSORA;
            CDMODELOBAL = cDMODELOBAL;
            CDHANDSHAKEBAL = cDHANDSHAKEBAL;
            CDPARITYBAL = cDPARITYBAL;
            CDSERIALSTOPBITBAL = cDSERIALSTOPBITBAL;
            NUDATABITBAL = nUDATABITBAL;
            NUBAUDRATEBAL = nUBAUDRATEBAL;
            DSPORTABAL = dSPORTABAL;
            STPDV = sTPDV;
        }
    }
}
