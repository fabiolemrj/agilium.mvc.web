
using System.ComponentModel.DataAnnotations;
using System;
using agilum.mvc.web.Enums;

namespace agilum.mvc.web.ViewModels.Caixa
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

    public class CaixaMovimentoViewModel
    {
        public long Id { get; set; }
        public Int64? IDCAIXA { get; set; }
        public ETipoMovCaixa? Tipo { get; set; }
        public string Descricao { get; set; }
        public double? Valor { get; set; }
        public ESituacaoCaixa? Situacao { get; set; }
        public string Caixa { get; set; }
    }

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
        [Display(Name = "Valor correto de fechamento")]
        public double? ValorCorrecao { get; set; }
        public Int64? IDUSUARIOCORRECAO { get; set; }
        public string UsuarioCorrecao { get; set; }
        public DateTime? DataCorrecao { get; set; }
    }

}
