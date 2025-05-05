using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;

namespace agilium.api.manager.ViewModels.SiteMercadoViewModel
{
    public class ProdutoSiteMercadoViewModel
    {
        public long Id { get; set; }
        public long? IDEMPRESA { get; set; }
        public long? IDPRODUTO { get; set; }
        public string ProdutoPdv { get; set; }
        public string Descricao { get; set; }
        public double? QuantidadeAtacado { get; set; }
        public double? ValorPromocao { get; set; }
        public double? ValorAtacado { get; set; }
        public double? ValorCompra { get; set; }
        public ESituacaoProdutoSiteMercada? Situacao { get; set; }
        public EValidadeSiteMercado? Validade { get; set; }
        public DateTime? DataHora { get; set; }
    }
}
