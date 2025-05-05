using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.CompraViewModel
{
    public class CompraItemViewModel
    {
        public long Id { get; set; }
        public Int64? IDCOMPRA { get;  set; }
        public string NomeCompra { get;  set; }
        public Int64? IDPRODUTO { get;  set; }
        public Int64? IDESTOQUE { get;  set; }
        public string NomeEstoque { get;  set; }
        public string CodigoProduto { get; set; }
        public string NomeProduto { get;  set; }
        public string DescricaoProdutoCompra { get; set; }
        public string CodigoEan { get;  set; }
        public string CodigoNCM { get;  set; }
        public string CodigoCEST { get;  set; }
        public string SGUN { get;  set; }
        public double? Quantidade { get;  set; }
        public double? Relacao { get;  set; }
        public double? ValorUnitario { get;  set; }
        public double? ValorTotal { get;  set; }
        public DateTime? DataValidade { get;  set; }
        public int? NumeroCFOP { get;  set; }
        public double? ValorOUTROS { get;  set; }
        public double? ValorBaseRetido { get;  set; }
        public double? PorcentagemIcmsRetido { get;  set; }
        public double? PorcentagemReducao { get;  set; }
        public string CodigoCstIcms { get;  set; }
        public string CodigoCstPis { get;  set; }
        public string CodigoCstCofins { get;  set; }
        public string CodigoCstIpi { get;  set; }
        public double? ValorAliquotaPis { get;  set; }
        public double? ValorAliquotaCofins { get;  set; }
        public double? ValorAliquotaIcms { get;  set; }
        public double? ValorAliquotaIpi { get;  set; }
        public double? ValorBaseCalculoPis { get;  set; }
        public double? ValorBaseCalculoCofins { get;  set; }
        public double? ValorBaseCalculoIcms { get;  set; }
        public double? ValorBaseCalculoIpi { get;  set; }
        public double? ValorIcms { get;  set; }
        public double? ValorPis { get;  set; }
        public double? ValorCofins { get;  set; }
        public double? ValorIpi { get;  set; }
        public string CodigoProdutoFornecedor { get;  set; }
        public double? ValorNovoPrecoVenda { get;  set; }
    }

    public class CompraItemEditViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Compra")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDCOMPRA { get; set; }
        public string NomeCompra { get; set; }
        [Display(Name = "Produto")]
        public Int64? IDPRODUTO { get; set; }
        public string CodigoProduto { get; set; }
        [Display(Name = "Descrição Produto NF")]
        public string DescricaoProdutoCompra { get; set; }
        [Display(Name = "Estoque")]
        public Int64? IDESTOQUE { get; set; }
        [Display(Name = "Unidade")]
        [StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string SGUN { get; set; }
        [Display(Name = "Quantidade")]
        public double? Quantidade { get; set; }
        [Display(Name = "Relação")]
        public double? Relacao { get; set; }
        [Display(Name = "Valor Unitário")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorUnitario { get; set; }
        [Display(Name = "Valor Total")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorTotal { get; set; } = 0;
        [Display(Name = "Preço Venda")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorNovoPrecoVenda { get; set; } = 0;
        public bool Importada { get; set; } = false;
    }
    public class CompraFiscalViewModel
    {
        public long Id { get; set; }
        public Int64? IDCOMPRA { get; set; }
        public string NomeCompra { get; set; }
        public ETipoManifestoCompra? TipoManifesto { get; set; }
        public string Xml { get; set; }
    }
}
