using agilium.api.business.Enums;

namespace agilium.api.pdv.ViewModels.ProdutoViewModel
{
    public class ProdutoPesqViewModel
    {
        public string Id { get; set; }
        public string idEmpresa { get; set; }
        public string IDGRUPO { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public string Grupo { get; set; }
        public string CodigoBarra { get; set; }
        public ECategoriaProduto? Categoria { get; set; }
        public ETipoProduto? Tipo { get; set; }
        public string UnidadeVenda { get; set; }
        public double? Preco { get; set; }
        public EAtivo? Situacao { get; set; }
        public double? ValorCustoMedio { get; set; }
    }
}
