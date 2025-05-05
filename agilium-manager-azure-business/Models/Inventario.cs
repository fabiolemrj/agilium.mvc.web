using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Inventario: Entity
    {
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public Int64? IDESTOQUE { get; private set; }
        public virtual Estoque Estoque { get; private set; }
        public string CDINVENT { get; private set; }
        public string DSINVENT { get; private set; }
        public DateTime? DTINVENT { get; private set; }
        public ESituacaoInventario? STINVENT { get; private set; }
        public string DSOBS { get; private set; }
        public ETipoAnalise? TPANALISE { get; private set; }
        public virtual List<InventarioItem> InventarioItem { get; private set; }= new List<InventarioItem>();
        public Inventario()
        {            
        }
        public Inventario(long? iDEMPRESA, long? iDESTOQUE, string cDINVENT, string dSINVENT, DateTime? dTINVENT, ESituacaoInventario? sTINVENT, string dSOBS, ETipoAnalise? tPANALISE)
        {
            IDEMPRESA = iDEMPRESA;
            IDESTOQUE = iDESTOQUE;
            CDINVENT = cDINVENT;
            DSINVENT = dSINVENT;
            DTINVENT = dTINVENT;
            STINVENT = sTINVENT;
            DSOBS = dSOBS;
            TPANALISE = tPANALISE;
        }

        public void Cancelar() => STINVENT = ESituacaoInventario.Cancelada;
        public void Executar() => STINVENT = ESituacaoInventario.Execucao;
        public void Abrir() => STINVENT = ESituacaoInventario.Aberta;
        public void Concluir() => STINVENT = ESituacaoInventario.Concluida;


    }
}
