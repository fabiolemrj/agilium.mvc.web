using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class TurnoPreco: Entity
    {
        public Int64? IDPRODUTO { get; private set; }
        public virtual Produto Produto { get; private set; }
        public int? NUTURNO { get; private set; }
        public int? TPDIFERENCA { get; private set; }
        public int? TPVALOR { get; private set; }
        public double? NUVALOR { get; private set; }
        public string NMUSUARIO { get; private set; }
        public DateTime? DTHRCAD { get; private set; } = new DateTime();

        public TurnoPreco()
        {
            
        }

        public TurnoPreco(long? iDPRODUTO, int? nUTURNO, int? tPDIFERENCA, int? tPVALOR, double? nUVALOR, string nMUSUARIO, DateTime? dTHRCAD)
        {
            IDPRODUTO = iDPRODUTO;
            NUTURNO = nUTURNO;
            TPDIFERENCA = tPDIFERENCA;
            TPVALOR = tPVALOR;
            NUVALOR = nUVALOR;
            NMUSUARIO = nMUSUARIO;
            DTHRCAD = dTHRCAD;
        }
    }
}
