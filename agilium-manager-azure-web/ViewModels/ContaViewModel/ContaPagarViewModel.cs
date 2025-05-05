using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.ViewModels.CategFinancViewModel;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.Fornecedor;
using agilium.webapp.manager.mvc.ViewModels.PlanoContaViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.ContaViewModel
{
    public class ContaPagarViewModel
    {
        public long Id { get; set; }
        public Int64? IDCONTAPAI { get; set; }
        [Display(Name = "Categoria Financeira")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDCATEG_FINANC { get; set; }
        [Display(Name = "Usuario")]
        public Int64? IDUSUARIO { get; set; }
        [Display(Name = "Fornecedor")]
        public Int64? IDFORNEC { get; set; }
        [Display(Name = "Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDCONTA { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDEMPRESA { get; set; }
        [Display(Name = "Lançamentos")]
        public Int64? IDLANC { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(100, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Data Vencimento")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime? DataVencimento { get; set; }
        [Display(Name = "Data Pagamento")]
        public DateTime? DataPagamento { get; set; }
        [Display(Name = "Valor Conta")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorConta { get; set; } = 0;
        [Display(Name = "Valor Desconto")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorDesconto { get; set; } = 0;
        [Display(Name = "Valor Acréscimo")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorAcrescimo { get; set; } = 0;
        [Display(Name = "Parcela")]
        public int? ParcelaInicial { get; set; } = 1;
        [Display(Name = "Tipo")]
        public ETipoConta? TipoConta { get; set; }
        [Display(Name = "Situação")]
        public int? Situacao { get; set; }
        [Display(Name = "Observação")]
        [StringLength(255, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string OBS { get; set; }
        [Display(Name = "Nº NFE")]
        public string NumeroNotaFiscal { get; set; }
        [Display(Name = "Data NFE")]
        public DateTime? DataNotaFiscal { get; set; }
        [Display(Name = "Data Cadastro")]
        public DateTime? DatCadastro { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
        public List<FornecedorViewModel> Fornecedores { get; set; } = new List<FornecedorViewModel>();
        public List<CategeoriaFinanceiraViewModel> CategoriasFinanceiras { get; set; } = new List<CategeoriaFinanceiraViewModel>();
        public List<PlanoContaViewModel.PlanoContaViewModel> PlanosContas { get; set; } = new List<PlanoContaViewModel.PlanoContaViewModel>();

    }

    public class ContaPagarViewModelIndex : ContaPagarViewModel
    {
        public string CategoriaFinanceira { get; set; }
        public string Usuario { get; set; }
        public string Fornecedor { get; set; }
        public string Conta { get; set; }
    }
}
