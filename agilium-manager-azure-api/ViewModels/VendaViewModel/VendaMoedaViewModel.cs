using agilium.api.business.Models;
using System;

namespace agilium.api.manager.ViewModels.VendaViewModel
{
    public class VendaMoedaViewModel
    {
        public Int64? IDVENDA { get; set; }
        public string VendaNome { get; set; }
        public Int64? IDMOEDA { get; set; }
        public string MoedaNome { get; set; }
        public Int64? IDVALE { get; set; }
        public double? ValorPago { get; set; }
        public double? ValorTroco { get; set; }
        public int? NumeroParcela { get; set; }
        public string NSU { get; set; }
    }
}
