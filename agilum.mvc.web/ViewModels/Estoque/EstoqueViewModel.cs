using agilium.api.business.Enums;
using agilum.mvc.web.Enums;
using agilum.mvc.web.ViewModels.Empresa;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace agilum.mvc.web.ViewModels.Estoque
{
    public class EstoqueViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long idEmpresa { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoEstoque Tipo { get; set; } = ETipoEstoque.Almoxarifado;
        [Display(Name = "Capacidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor de {0} deve ser maior que {1}")]
        public decimal Capacidade { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EAtivo? situacao { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel> { };
    }

    public class EstoqueProdutoListaViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        public long IDPRODUTO { get; set; }
        public long IDESTOQUE { get; set; }
        [Display(Name = "Estoque")]
        public string Estoque { get; set; }
        [Display(Name = "Quantidade Atual")]
        public double QuantidadeAtual { get; set; }
        [Display(Name = "Situação")]
        public EAtivo Situacao { get; set; }
        public string TipoEsotque { get; set; }
        public decimal Capacidade { get; set; }
    }

    public class ProdutoPorEstoqueViewModel
    {
        public long Id { get; set; }
        public long idProduto { get; set; }
        [Display(Name = "Produto")]
        public string Produto { get; set; }
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }
        [Display(Name = "Quantidade Atual")]
        public double QuantidadeAtual { get; set; }
        [Display(Name = "Ultima Compra")]
        public double ValorUltimaCompra { get; set; }
        [Display(Name = "Custo Medio")]
        public double ValorCustoMedio { get; set; }
    }

    public class EstoqueHistoricoViewModel
    {
        [Display(Name = "Estoque")]
        public long? IDESTOQUE { get; set; }
        [Display(Name = "Produto")]
        public long? IDPRODUTO { get; set; }
        public long? IDITEM { get; set; }
        public long? IDLANC { get; set; }
        [Display(Name = "Data/Hora")]
        public DateTime? DataHora { get; set; }
        [Display(Name = "Usuario")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NomeUsuario { get; set; }
        [Display(Name = "Tipo")]
        public int? TipoHistorico { get; set; }
        [Display(Name = "Descricao")]
        [StringLength(250, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Quantidade")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor de {0} deve ser maior que {1}")]
        public double? Quantidade { get; set; }
    }

    public class EstoquePosicaoReport
    {
        public long Id { get; set; }
        public long? idEmpresa { get; set; }
        public string Descricao { get; set; }
        public int? Tipo { get; set; }
        public decimal? Capacidade { get; set; }
        public EAtivo? STESTOQUE { get; set; }
        public string NMProduto { get; set; }
        public string CdProduto { get; set; }
        public string NmGrupo { get; set; }
        public double NuQtdMin { get; set; }
        public double VlCustoMedio { get; set; }
        public double VlFinanc { get; set; }
        public double Quantidade { get; set; }
        public int Situacao { get; set; }
    }

    public class EstoqueProdutoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDPRODUTO { get; set; }
        [Display(Name = "Estoque")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDESTOQUE { get; set; }
        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(0, double.MaxValue, ErrorMessage = "O valor de {0} deve ser maior que {1}")]
        public double? Quantidade { get; set; }

    }

    public class FiltroEstoquePosicao
    {
        public long IdEstoque { get; set; }
        public List<EstoquePosicaoReport> Lista { get; set; } = new List<EstoquePosicaoReport>();
        public List<EstoqueViewModel> Estoques { get; set; } = new List<EstoqueViewModel>();
    }
}
