
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Estoque;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.Enums;

namespace agilum.mvc.web.ViewModels.Inventario
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
        public List<EstoqueViewModel> Estoques { get; set; } = new List<EstoqueViewModel>();
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

        // 🔹 Paginação
        public int PaginaAtual { get; set; } = 1;         // Página que está sendo exibida
        public int TotalPaginas { get; set; } = 1;        // Quantidade total de páginas
        public int TamanhoPagina { get; set; } = 20;      // Itens por página (default 20)
        public int TotalItens { get; set; } = 0;          // Quantidade total de registros

        // 🔹 Helpers (úteis para a View)
        public bool TemPaginaAnterior => PaginaAtual > 1;
        public bool TemProximaPagina => PaginaAtual < TotalPaginas;
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
        private long _id;

        public long Id
        {
            get => _id;
            set
            {
                _id = value;
                Id_String = value.ToString(); // atualiza automaticamente
            }
        }

        public string Id_String { get; set; }
        [Display(Name = "Empresa")]
        public Int64? idEmpresa { get; set; }
        [Display(Name = "Grupo")]
        public Int64? IDGRUPO { get; set; }
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }
        [Display(Name = "Descricao")]
        public string Nome { get; set; }
        [Display(Name = "Categoria")]
        public agilium.api.business.Enums.ECategoriaProduto Categoria { get; set; }
        [Display(Name = "Tipo Produto")]
        public agilium.api.business.Enums.ETipoProduto? Tipo { get; set; }
        public bool Selecionado { get; set; }
    }
}
