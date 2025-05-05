using agilium.api.business.Enums;
using agilium.api.business.Models;
using Microsoft.AspNetCore.Http;
using System;

namespace agilium.api.manager.ViewModels.CompraViewModel
{
    public class CompraViewModel
    {
        public long Id { get; set; }
        public Int64? IDEMPRESA { get; set; }
        public Int64? IDFORN { get; set; }
        public string NomeFornecedor { get; set; }
        public Int64? IDTURNO { get; set; }
        public string NomeTurno { get; set; }
        public DateTime? DataCompra { get; set; }
        public DateTime? DataCadastro { get; set; }
        public string Codigo { get; set; }
        public ESituacaoCompra? Situacao { get; set; }
        public DateTime? DataNF { get; set; }
        public string NumeroNF { get; set; }
        public string SerieNF { get; set; }
        public string ChaveNFE { get; set; }
        public ETipoCompravanteCompra? TipoComprovante { get; set; }
        public int? NumeroCFOP { get; set; }
        public double? ValorIcmsRetido { get; set; }
        public double? ValorBaseCalculoIcms{ get; set; }
        public double? ValorIcms { get; set; }
        public double? ValorBaseCalculoSub { get; set; }
        public double? ValorIcmsSub { get; set; }
        public double? ValorIsencao { get; set; }
        public double? ValorTotalProduto { get; set; }
        public double? ValorFrete { get; set; }
        public double? ValorSeguro { get; set; }
        public double? ValorDesconto { get; set; }
        public double? ValorOutros { get; set; }
        public double? ValorIpi { get; set; }
        public double? ValorTotal { get; set; }
        public string Observacao { get; set; }
        public ESimNao? Importada { get; set; }
    }

    public class ImportacaoArquivo
    {
        public long idCompra { get; set; }
        public IFormFile XmlArquivo { get; set; }
    }
}
