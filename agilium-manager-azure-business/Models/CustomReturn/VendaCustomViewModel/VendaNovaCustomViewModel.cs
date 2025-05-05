using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models.CustomReturn.VendaCustomViewModel
{
    public class VendaNovaCustomViewModel
    {
        public string IdCaixa { get; set; }
        public string IdEstoque { get; set; }
        public string IdPdv { get; set; }
        public int SqVenda { get; set; }
        public int SqCaixa { get; set; }
        public int Turno { get; set; }
    }
}
