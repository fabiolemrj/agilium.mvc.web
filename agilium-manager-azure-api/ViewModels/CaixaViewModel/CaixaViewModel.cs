using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.CaixaViewModel
{
    public class CaixaViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public Int64? IDEMPRESA { get; set; }
        [Display(Name = "Turno")]
        public Int64? IDTURNO { get; set; }
        [Display(Name = "PDV")]
        public Int64? IDPDV { get; set; }
        [Display(Name = "Funcionario")]
        public Int64? IDFUNC { get; set; }
        [Display(Name = "Sequencial")]
        public int? Sequencial { get; set; }
        [Display(Name = "Situação")]
        public ESituacaoCaixa? Situacao { get; set; }
        [Display(Name = "Data Abertura")]
        public DateTime? DataAbertura { get; set; }
        [Display(Name = "Valor Abertura")]
        public double? ValorAbertura { get; set; }
        [Display(Name = "Data Fechamento")]
        public DateTime? DataFechamento { get;set; }
        [Display(Name = "Valor Fechamento")]
        public double? ValorFechamento { get; set; }
    }
    public class CaixaindexViewModel: CaixaViewModel 
    {
        public string Empresa { get; set; }
        public string Turno { get; set; }
        public string PDV { get; set; }
        public string Funcionario { get; set; }
    }
}
