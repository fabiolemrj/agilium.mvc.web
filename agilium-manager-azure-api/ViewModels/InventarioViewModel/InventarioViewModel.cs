using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.InventarioViewModel
{
    public class InventarioViewModel
    {
        public long Id { get; set; }
        public Int64? IDEMPRESA { get; set; }
        public Int64? IDESTOQUE { get; set; }
        public string NomeEstoque { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public DateTime? Data { get; set; }
        public ESituacaoInventario? Situacao { get; set; }
        public string Observacao { get; set; }
        public ETipoAnalise? TipoAnalise { get; set; }
    }

    public class InventarioItemViewModel
    {
        public long Id { get; set; }
        public Int64? IDINVENT { get; set; }
        public Int64? IDPRODUTO { get; set; }
        public string NomeProduto { get; set; }
        public string CodigoProduto { get; set; }
        public Int64? IDPERDA { get; set; }
        public string NomePerda { get; set; }
        public Int64? IDUSUARIOANALISE { get; set; }
        public string NomeUsuarioAnalise { get; set; }
        public DateTime? DataHora { get; set; }
        public double? QuantidadeAnalise { get; set; }
        public double? QuantidadeEstoque { get; set; }
        public double? ValorCustoMedio { get; set; }
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
