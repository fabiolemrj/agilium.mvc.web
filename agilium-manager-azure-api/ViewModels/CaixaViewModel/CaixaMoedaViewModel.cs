using agilium.api.business.Models;
using System;

namespace agilium.api.manager.ViewModels.CaixaViewModel
{
    public class CaixaMoedaViewModel
    {
        public long Id { get; set; }
        public Int64? IDCAIXA { get; set; }
        public string CaixaNome { get; set; }
        public Int64? IDMOEDA { get; set; }
        public string MoedaNome { get; set; }
        public double? ValorOriginal { get; set; }
        public double? ValorCorrecao { get; set; }
        public Int64? IDUSUARIOCORRECAO { get; set; }
        public string UsuarioCorrecao { get; set; }
        public DateTime? DataCorrecao { get; set; }
    }
}
