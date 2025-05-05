using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Devolucao: Entity
    {
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public Int64? IDVENDA { get; private set; }
        public virtual Venda Venda { get; private set; }
        public Int64? IDCLIENTE { get; private set; }
        public virtual Cliente Cliente { get; private set; }
        public Int64? IDMOTDEV { get; private set; }
        public virtual MotivoDevolucao MotivoDevolucao { get; private set; }
        public Int64? IDVALE { get; private set; }
        public virtual Vale Vale { get; private set; }
        public string CDDEV { get; private set; }
        public DateTime? DTHRDEV { get; private set; }
        public double? VLTOTALDEV { get; private set; }
        public string DSOBSDEV { get; private set; }
        public ESituacaoDevolucao? STDEV { get; private set; }
        public virtual List<DevolucaoItem> DevolucaoItem { get; set; } = new List<DevolucaoItem>();
        public Devolucao()
        {            
        }

        public Devolucao(long? iDEMPRESA, long? iDVENDA, long? iDCLIENTE, long? iDMOTDEV, long? iDVALE, string cDDEV, DateTime? dTHRDEV, double? vLTOTALDEV, string dSOBSDEV, ESituacaoDevolucao? sTDEV)
        {
            IDEMPRESA = iDEMPRESA;
            IDVENDA = iDVENDA;
            IDCLIENTE = iDCLIENTE;
            IDMOTDEV = iDMOTDEV;
            IDVALE = iDVALE;
            CDDEV = cDDEV;
            DTHRDEV = dTHRDEV;
            VLTOTALDEV = vLTOTALDEV;
            DSOBSDEV = dSOBSDEV;
            STDEV = sTDEV;
        }

        public void AddVenda(Venda venda) => Venda = venda;
        public void AddCliente(Cliente cliente) => Cliente = cliente;
        public void AddMotivoDev(MotivoDevolucao motivoDevolucao) => MotivoDevolucao = motivoDevolucao;
        public void Cancelar() => STDEV = ESituacaoDevolucao.Cancelada;
        public void AddVale(long idVale) => IDVALE = idVale;
    }
}
