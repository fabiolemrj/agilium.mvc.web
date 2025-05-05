using System;
using System.ComponentModel.DataAnnotations;

namespace agilium.webapp.manager.mvc.ViewModels.CaixaViewModel
{
    public class CaixaMoedaViewModel
    {
        public long Id { get; set; }
        public Int64? IDCAIXA { get; set; }
        public string CaixaNome { get; set; }
        public Int64? IDMOEDA { get; set; }
        public string MoedaNome { get; set; }
        [DataType(DataType.Currency)]
        public double? ValorOriginal { get; set; }
        [DataType(DataType.Currency)]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Display(Name ="Valor correto de fechamento")]
        public double? ValorCorrecao { get; set; }
        public Int64? IDUSUARIOCORRECAO { get; set; }
        public string UsuarioCorrecao { get; set; }
        public DateTime? DataCorrecao { get; set; }
    }

}
