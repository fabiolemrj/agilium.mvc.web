using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.InventarioViewModel
{
    public class InventarioViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDEMPRESA { get; set; }
        [Display(Name = "Estoque")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDESTOQUE { get; set; }
        public string NomeEstoque { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Codigo { get; set; }
        [Display(Name = "Descrição")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Descricao { get; set; }
        [Display(Name = "Data")]
        public DateTime? Data { get; set; } = DateTime.Now;
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ESituacaoInventario? Situacao { get; set; }
        public string Observacao { get; set; }
        [Display(Name = "Tipo Analise")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoAnalise? TipoAnalise { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
        public List<EstoqueViewModel.EstoqueViewModel> Estoques { get; set; } = new List<EstoqueViewModel.EstoqueViewModel>();
    }

    public class InventarioItemViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Inventario")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDINVENT { get; set; }
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDPRODUTO { get; set; }
        public string NomeProduto { get; set; }
        [Display(Name = "Perda")]
        public Int64? IDPERDA { get; set; }
        public string NomePerda { get; set; }
        [Display(Name = "Usuario")]
        public Int64? IDUSUARIOANALISE { get; set; }
        public string NomeUsuarioAnalise { get; set; }
        [Display(Name = "Data/Hora Analise")]
        public DateTime? DataHora { get; set; } = DateTime.Now;
        [Display(Name = "Quantidade Analise")]
        public double? QuantidadeAnalise { get; set; } = 0;
        [Display(Name = "Quantidade Estoque")]
        public double? QuantidadeEstoque { get; set; } = 0;
        [Display(Name = "Valor Custo Médio")]
        [Moeda]
        public double? ValorCustoMedio { get; set; } = 0;
        public string CodigoProduto { get; set; }
        //public List<ProdutoViewModel.ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel.ProdutoViewModel>();
        public bool Selecionado { get; set; }
    }

    public class ListaInventarioItemViewModel
    {
        public long idInventario { get; set; }
        public ESituacaoInventario Situacao { get; set; }
        public string NomeInventario { get; set; }
        public ETipoAnalise? TipoAnalise { get; set; }
        public List<InventarioItemViewModel> Itens { get; set; } = new List<InventarioItemViewModel>();
    }

    public class AdicionarListaProdutosDisponiveisViewModel
    {
        public long idInventario { get; set; }
        public ESituacaoInventario? Situacao { get; set; }
        public string NomeInventario { get; set; }
        public Int64? IDEMPRESA { get; set; }
        public List<ProdutoDisponivelViewModel> Produtos { get; set; } = new List<ProdutoDisponivelViewModel>();
    }

    public class ProdutoDisponivelViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public Int64? idEmpresa { get; set; }
        [Display(Name = "Grupo")]
        public Int64? IDGRUPO { get; set; }
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }
        [Display(Name = "Descricao")]
        public string Nome { get; set; }
        [Display(Name = "Categoria")]
        public ECategoriaProduto Categoria { get; set; }
        [Display(Name = "Tipo Produto")]
        public ETipoProduto? Tipo { get; set; }
        public bool Selecionado { get; set; }
    }
}
