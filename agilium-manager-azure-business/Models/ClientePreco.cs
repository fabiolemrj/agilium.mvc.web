using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ClientePreco: Entity
    {
        public long IDCLIENTE { get; private set; }
        public virtual Cliente Cliente { get; private set; }
        public long IDPRODUTO { get; private set; }
        public virtual Produto Produto { get; private set; }
        public int TPDIFERENCA { get; private set; }
        public double NUVALOR { get; private set; }
        public DateTime DTHRCAD { get; private set; }
        public int TPVALOR { get; private set; }
        public string NmUsuario { get; private set; }

        public ClientePreco()
        {
            
        }

        public ClientePreco(long iDCLIENTE, long iDPRODUTO, int tPDIFERENCA, double nUVALOR, DateTime dTHRCAD, int tPVALOR, string nmUsuario)
        {
            IDCLIENTE = iDCLIENTE;
            IDPRODUTO = iDPRODUTO;
            TPDIFERENCA = tPDIFERENCA;
            NUVALOR = nUVALOR;
            DTHRCAD = dTHRCAD;
            TPVALOR = tPVALOR;
            NmUsuario = nmUsuario;
        }
    }
}
