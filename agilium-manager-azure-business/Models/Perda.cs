using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace agilium.api.business.Models
{
    public class Perda: Entity
    {
        [Column("IDEMPRESA")]
        [ForeignKey("Empresa")]
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        [Column("IDESTOQUE")]
        [ForeignKey("Estoque")]
        public Int64? IDESTOQUE { get; private set; }
        public virtual Estoque Estoque { get; private set; }
        [Column("IDESTOQUEHST")]
        [ForeignKey("EstoqueHistorico")]
        public Int64? IDESTOQUEHST { get; private set; }
        public virtual EstoqueHistorico EstoqueHistorico { get; private set; }
        [Column("IDPRODUTO")]
        [ForeignKey("Produto")]
        public Int64? IDPRODUTO { get; private set; }
        public virtual Produto Produto { get; private set; }
        [Column("IDUSUARIO")]
        [ForeignKey("Usuario")]
        public Int64? IDUSUARIO { get; private set; }
        public virtual Usuario Usuario { get; private set; }
        public string CDPERDA { get; private set; }
        public DateTime? DTHRPERDA { get; private set; }
        public ETipoPerda? TPPERDA { get; private set; }
        public ETipoMovimentoPerda? TPMOV { get; private set; }
        public double? NUQTDPERDA { get; private set; }
        public double? VLCUSTOMEDIO { get; private set; }
        public string DSOBS { get; private set; }
        public virtual List<InventarioItem> InventarioItem { get; set; } = new List<InventarioItem>();
        public Perda()
        {
            
        }
        public Perda(long? iDEMPRESA, long? iDESTOQUE, long? iDESTOQUEHST, long? iDPRODUTO, long? iDUSUARIO, string cDPERDA, DateTime? dTHRPERDA, ETipoPerda? tPPERDA, ETipoMovimentoPerda? tPMOV, double? nUQTDPERDA, double? vLCUSTOMEDIO, string dSOBS)
        {
            IDEMPRESA = iDEMPRESA;
            IDESTOQUE = iDESTOQUE;
            IDESTOQUEHST = iDESTOQUEHST;
            IDPRODUTO = iDPRODUTO;
            IDUSUARIO = iDUSUARIO;
            CDPERDA = cDPERDA;
            DTHRPERDA = dTHRPERDA;
            TPPERDA = tPPERDA;
            TPMOV = tPMOV;
            NUQTDPERDA = nUQTDPERDA;
            VLCUSTOMEDIO = vLCUSTOMEDIO;
            DSOBS = dSOBS;
        }
    }
}
