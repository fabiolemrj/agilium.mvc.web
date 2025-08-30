
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.Enums;


namespace agilum.mvc.web.ViewModels.PlanoConta
{
    public class PlanoContaViewModel : PlanoContaEditViewModel
    {
        public List<PlanoContaViewModel> PlanosContas { get; set; } = new List<PlanoContaViewModel>();
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
    }

    public class PlanoContaEditViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64 IDEMPRESA { get; set; }
        [Display(Name = "Conta de nível hierarquico superior")]
        public Int64? IDCONTAPAI { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Tipo Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoContaLancacmento Tipo { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public agilium.api.business.Enums.EAtivo Situacao { get; set; }
        [Display(Name = "Conta de nível hierarquico superior")]
        public string NomeContaPai { get; set; }
        public double Saldo { get; set; } = 0;
    }

    public class PlanoContaSaldoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Plano de conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64 IDCONTA { get; set; }
        [Display(Name = "Data/Hora")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime DataHora { get; set; } = DateTime.Now;
        [Display(Name = "Ano/Mês Referência")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int? AnoMesReferencia { get; set; }
        [Display(Name = "Valor Saldo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public double ValorSaldo { get; set; } = 0;
    }
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
        public agilium.api.business.Enums.EAtivo Situacao { get; set; }

    }
}
