using agilium.api.business.Enums;
using System;
using System.ComponentModel.DataAnnotations;

namespace agilium.api.pdv.ViewModels.CaixaViewModel
{
    public class CaixaViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Empresa")]
        public string IDEMPRESA { get; set; }
        [Display(Name = "Turno")]
        public string IDTURNO { get; set; }
        [Display(Name = "PDV")]
        public string IDPDV { get; set; }
        [Display(Name = "Funcionario")]
        public string IDFUNC { get; set; }
        [Display(Name = "Sequencial")]
        public string Sequencial { get; set; }
        [Display(Name = "Situação")]
        public ESituacaoCaixa? Situacao { get; set; }
        [Display(Name = "Data Abertura")]
        public DateTime? DataAbertura { get; set; }
        [Display(Name = "Valor Abertura")]
        public double? ValorAbertura { get; set; }
        [Display(Name = "Data Fechamento")]
        public DateTime? DataFechamento { get; set; }
        [Display(Name = "Valor Fechamento")]
        public double? ValorFechamento { get; set; }
    }
    public class CaixaindexViewModel : CaixaViewModel
    {
        public string Empresa { get; set; }
        public string Turno { get; set; }
        public string PDV { get; set; }
        public string Funcionario { get; set; }
    }

    public class AbrirCaixaViewModel
    {
        public string idempresa { get; set; }
        public string idpdv { get; set; }

    }

    public class AbrirCaixaRetornoViewModel
    {
        public string NumeroCaixa { get; set; }
    }
}
