using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models.CustomReturn.ProdutoReturnViewModel
{
    public class ProdutoPesqReturnViewModel
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
