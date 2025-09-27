
using agilum.mvc.web.ViewModels.CategeoriaFinanceira;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Fornecedor;
using agilum.mvc.web.ViewModels.PlanoConta;
using agilum.mvc.web.Enums;
using agilum.mvc.web.ViewModels.Cliente;
using System.Globalization;
using System.Linq;

namespace agilum.mvc.web.ViewModels.Conta
{
    public class ContaPagarViewModel
    {
        public long Id { get; set; }
        public long? IDCONTAPAI { get; set; }

        [Display(Name = "Categoria Financeira")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long? IDCATEG_FINANC { get; set; }

        [Display(Name = "Usuario")]
        public long? IDUSUARIO { get; set; }

        [Display(Name = "Fornecedor")]
        public long? IDFORNEC { get; set; }

        [Display(Name = "Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long? IDCONTA { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long? IDEMPRESA { get; set; }

        [Display(Name = "Lançamentos")]
        public long? IDLANC { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(100, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }

        [Display(Name = "Data Vencimento")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime? DataVencimento { get; set; }

        [Display(Name = "Data Pagamento")]
        public DateTime? DataPagamento { get; set; }

        // ===== CAMPOS DOUBLE + STRING CORRESPONDENTES =====

        public double? ValorConta { get; set; }
        [Display(Name = "Valor Conta")]
        public string ValorConta_String { get; set; }

                public double? ValorDesconto { get; set; }
        [Display(Name = "Valor Desconto")]
        public string ValorDesconto_String { get; set; }

        public double? ValorAcrescimo { get; set; }
        [Display(Name = "Valor Acréscimo")]
        public string ValorAcrescimo_String {get; set;}

        // ===== OUTROS CAMPOS =====

        [Display(Name = "Parcela")]
        public int? ParcelaInicial { get; set; } = 1;

        [Display(Name = "Tipo")]
        public agilium.api.business.Enums.ETipoConta? TipoConta { get; set; }

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
        public List<PlanoContaViewModel> PlanosContas { get; set; } = new List<PlanoContaViewModel>();

        // Método para sincronizar strings de volta para doubles
        public void SincronizarValores()
        {
            if (double.TryParse(ValorConta_String, out var valorConta))
                ValorConta = valorConta;

            if (double.TryParse(ValorDesconto_String, out var valorDesconto))
                ValorDesconto = valorDesconto;

            if (double.TryParse(ValorAcrescimo_String, out var valorAcrescimo))
                ValorAcrescimo = valorAcrescimo;
        }

        public void SincronizarValoresString()
        {
            ValorConta_String = ValorConta?.ToString("N2") ?? string.Empty;
            ValorDesconto_String = ValorDesconto?.ToString("N2") ?? string.Empty;
            ValorAcrescimo_String = ValorAcrescimo?.ToString("N2") ?? string.Empty;
        }

    }

    public class ContaPagarViewModelIndex : ContaPagarViewModel
    {
        public string CategoriaFinanceira { get; set; }
        public string Usuario { get; set; }
        public string Fornecedor { get; set; }
        public string Conta { get; set; }
    }

    public class ContaReceberViewModel
    {
        public long Id { get; set; }
        public long? IDCONTAPAI { get; set; }

        [Display(Name = "Categoria Financeira")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long? IDCATEG_FINANC { get; set; }

        [Display(Name = "Usuario")]
        public long? IDUSUARIO { get; set; }

        [Display(Name = "Cliente")]
        public long? IDCLIENTE { get; set; }

        [Display(Name = "Conta")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long? IDCONTA { get; set; }

        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long? IDEMPRESA { get; set; }

        [Display(Name = "Lançamentos")]
        public long? IDLANC { get; set; }

        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(100, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }

        [Display(Name = "Data Vencimento")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime? DataVencimento { get; set; }

        [Display(Name = "Data Pagamento")]
        public DateTime? DataPagamento { get; set; }

        // ====== Valores numéricos com string correspondente ======

       
        public double? ValorConta { get; set; } = 0;
        [Display(Name = "Valor Conta")]
        public string ValorConta_String { get; set; } = string.Empty;

   
        public double? ValorDesconto { get; set; } = 0;
        [Display(Name = "Valor Desconto")]
        public string ValorDesconto_String { get; set; } = string.Empty;

        public double? ValorAcrescimo { get; set; } = 0;
        [Display(Name = "Valor Acréscimo")]
        public string ValorAcrescimo_String { get; set; } = string.Empty;

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
        public List<ClienteViewModel> Clientes { get; set; } = new List<ClienteViewModel>();
        public List<CategeoriaFinanceiraViewModel> CategoriasFinanceiras { get; set; } = new List<CategeoriaFinanceiraViewModel>();
        public List<PlanoContaViewModel> PlanosContas { get; set; } = new List<PlanoContaViewModel>();

        // ===== Métodos de Sincronização =====

        /// <summary>
        /// Converte os doubles para strings formatadas (ex: "1.234,56").
        /// </summary>
        public void SincronizarDeDoubleParaString()
        {
            ValorConta_String = ValorConta?.ToString("N2") ?? string.Empty;
            ValorDesconto_String = ValorDesconto?.ToString("N2") ?? string.Empty;
            ValorAcrescimo_String = ValorAcrescimo?.ToString("N2") ?? string.Empty;
        }

        /// <summary>
        /// Converte as strings (quando preenchidas) para double?.
        /// </summary>
        public void SincronizarDeStringParaDouble()
        {
            if (double.TryParse(ValorConta_String, out var valorConta))
                ValorConta = valorConta;

            if (double.TryParse(ValorDesconto_String, out var valorDesconto))
                ValorDesconto = valorDesconto;

            if (double.TryParse(ValorAcrescimo_String, out var valorAcrescimo))
                ValorAcrescimo = valorAcrescimo;
        }
    }


    public class ContaReceberViewModelIndex : ContaReceberViewModel
    {
        public string CategoriaFinanceira { get; set; }
        public string Usuario { get; set; }
        public string Cliente { get; set; }
        public string Conta { get; set; }
    }
}
