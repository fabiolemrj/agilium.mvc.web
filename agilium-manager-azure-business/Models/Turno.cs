using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Turno: Entity
    {
        public Int64? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public Int64? IDUSUARIOINI { get; private set; }
        public virtual Usuario UsuarioInicial { get; private set; }
        public Int64? IDUSUARIOFIM { get; private set; }
        public virtual Usuario UsuarioFinal { get; private set; }
        public DateTime? DTTURNO { get; private set; }
        public int? NUTURNO { get; private set; }
        public DateTime? DTHRINI { get; private set; }
        public DateTime? DTHRFIM { get; private set; }
        public string DSOBS { get; private set; }
        public virtual List<Caixa> Caixas { get; set; } = new List<Caixa>();
        public virtual List<Compra> Compras { get; set; } = new List<Compra>();

        public Turno()
        {            
        }

        public Turno(long? iDEMPRESA, long? iDUSUARIOINI, long? iDUSUARIOFIM, DateTime? dTTURNO, int? nUTURNO, DateTime? dTHRINI, DateTime? dTHRFIM, string dSOBS)
        {
            IDEMPRESA = iDEMPRESA;
            IDUSUARIOINI = iDUSUARIOINI;
            IDUSUARIOFIM = iDUSUARIOFIM;
            DTTURNO = dTTURNO;
            NUTURNO = nUTURNO;
            DTHRINI = dTHRINI;
            DTHRFIM = dTHRFIM;
            DSOBS = dSOBS;
        }
        //     public virtual List<Compra> Compras { get; set; }

        public void AdicionarObservacao(string obs) => DSOBS = obs;

    }
}
