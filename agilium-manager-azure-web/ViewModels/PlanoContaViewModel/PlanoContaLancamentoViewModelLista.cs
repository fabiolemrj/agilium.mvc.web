using agilium.webapp.manager.mvc.Enums;

using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.PlanoContaViewModel
{
    public class PlanoContaLancamentoListaViewModel
    {
    
        public DateTime? DataInicial { get; set; }
  
        public DateTime? DataFinal { get; set; }
        public long IdPlano { get; set; }
        public List<PlanoContaLancamentoViewModel> Lancamentos { get; set; } = new List<PlanoContaLancamentoViewModel>();
    }

    public class PlanoContaLancamentoViewModel 
    {
        public long Id { get; set; }
        public Int64? IDCONTA { get; set; }
        [Display(Name = "Data/Hora")]
        public DateTime DataHora { get; set; }
        [Display(Name = "Data Referencia")]
        public DateTime DataReferencia { get; set; }
        [Display(Name = "Ano/Mês Referência")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int? AnoMesReferencia { get; set; }
        public string DescricaoLancamento { get; set; }
        public double Valor { get; set; } = 0;
        [Display(Name = "Tipo Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoContaLancacmento Tipo { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EAtivo Situacao { get; set; }
     
    }

}
