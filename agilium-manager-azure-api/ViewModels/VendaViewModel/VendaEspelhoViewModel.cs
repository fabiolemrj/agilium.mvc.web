using agilium.api.business.Models;
using System;

namespace agilium.api.manager.ViewModels.VendaViewModel
{
    public class VendaEspelhoViewModel
    {
        public long Id { get; set; }
        public Int64? IDVENDA { get; set; }
        public string EspelhoVenda { get; set; }
        public string SequencialVenda { get; set; }
    }
}
