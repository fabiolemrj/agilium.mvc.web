using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.Fornecedor;
using agilium.webapp.manager.mvc.ViewModels.ImpostoViewModel;
using agilium.webapp.manager.mvc.ViewModels.TurnoViewModel;
using agilium.webapp.manager.mvc.ViewModels.UnidadeViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.CompraViewModel
{
    public class CompraViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDEMPRESA { get; set; }
        [Display(Name = "Fornecedor")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDFORN { get; set; }
        public string NomeFornecedor { get; set; }
        [Display(Name = "Turno")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDTURNO { get; set; }
        public string NomeTurno { get; set; }
        [Display(Name = "Data Compra")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime? DataCompra { get; set; }
        public DateTime? DataCadastro { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Codigo { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ESituacaoCompra? Situacao { get; set; }
        [Display(Name = "Data")]
        public DateTime? DataNF { get; set; }
        [Display(Name = "Numero")]
        public string NumeroNF { get; set; }
        [Display(Name = "Serie")]
        public string SerieNF { get; set; }
        [Display(Name = "Chave")]
        public string ChaveNFE { get; set; }
        public ETipoCompravanteCompra? TipoComprovante { get; set; }
        [Display(Name = "Numero CFOP")]
        public int? NumeroCFOP { get; set; }
        [Display(Name = "ICMS Retido")]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorIcmsRetido { get; set; } = 0;
        [Display(Name = "Base Calc. ICMS")]
        [Moeda]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorBaseCalculoIcms { get; set; } = 0;
        [Display(Name = "Valor ICMS")]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Moeda]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorIcms { get; set; } = 0;
        [Display(Name = "Base Calc. Subs")]
        [Moeda]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorBaseCalculoSub { get; set; } = 0;
        [Display(Name = "ICMS Subs")]
        [Moeda]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorIcmsSub { get; set; } = 0;
        [Display(Name = "Isenção")]
        [Moeda]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorIsencao { get; set; } = 0;
        [Display(Name = "Valor Total dos Itens")]
        [Moeda]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorTotalProduto { get; set; } = 0;
        [Display(Name = "Frete")]
        [Moeda]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorFrete { get; set; } = 0;
        [Display(Name = "Seguro")]
        [Moeda]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorSeguro { get; set; } = 0;
        [Display(Name = "Desconto")]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Moeda]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorDesconto { get; set; } = 0;
        [Display(Name = "Outros")]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorOutros { get; set; } = 0;
        [Display(Name = "Valor IPI")]
        [Moeda]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorIpi { get; set; } = 0;
        [Display(Name = "Valor Total")]
        [RegularExpression(@"^\$?\d+(\,(\d{2}))?$")]
        [Moeda]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorTotal { get; set; } = 0;
        [Display(Name = "Observação")]
        public string Observacao { get; set; }
        [Display(Name = "É Importada?")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public ESimNao? Importada { get; set; }

        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
        public List<FornecedorViewModel> Fornecedores { get; set; } = new List<FornecedorViewModel>();
        public List<TurnoIndexViewModel> Turnos { get; set; } = new List<TurnoIndexViewModel>();
        public List<CfopViewModel> Cfops { get; set; } = new List<CfopViewModel>();
    }

    public class CompraItemViewModel
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
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string DescricaoProdutoCompra { get; set; }
        [Display(Name = "Estoque")]
        public Int64? IDESTOQUE { get; set; }
        public string NomeEstoque { get; set; }
        public string NomeProduto { get; set; }
        [Display(Name = "Codigo EAN")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoEan { get; set; }
        [Display(Name = "Codigo NCM")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoNCM { get; set; }
        [Display(Name = "CEST")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoCEST { get; set; }
        [Display(Name = "Unidade")]
        [StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string SGUN { get; set; }
        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public double? Quantidade { get; set; }
        [Display(Name = "Relação")]
        public double? Relacao { get; set; }
        [Display(Name = "Valor Unitário")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public double? ValorUnitario { get; set; }
        [Display(Name = "Valor Total")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorTotal { get; set; } = 0;
        [Display(Name = "Data Validade")]
        public DateTime? DataValidade { get; set; }
        [Display(Name = "CFOP")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int? NumeroCFOP { get; set; }
        [Display(Name = "Valor Outros")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorOUTROS { get; set; } = 0;
        [Display(Name = "Valor Base Redução")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorBaseRetido { get; set; } = 0;
        [Display(Name = "% ICMS Retido")]
        [Range(0.0, 100, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? PorcentagemIcmsRetido { get; set; } = 0;
        [Display(Name = "Porcentagem Redução")]
        [Range(0.0, 100, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? PorcentagemReducao { get; set; } = 0;
        [Display(Name = "Codigo CST ICMS")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoCstIcms { get; set; }
        [Display(Name = "Codigo CST PIS")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoCstPis { get; set; }
        [Display(Name = "Codigo CST Cofins")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoCstCofins { get; set; }
        [Display(Name = "Codigo CST IPI")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoCstIpi { get; set; }
        [Display(Name = "Aliquota PIS")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorAliquotaPis { get; set; } = 0;
        [Display(Name = "Aliquota Cofins")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorAliquotaCofins { get; set; } = 0;
        [Display(Name = "Aliquota ICMS")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorAliquotaIcms { get; set; } = 0;
        [Display(Name = "Aliquota IPI")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorAliquotaIpi { get; set; } = 0;
        [Display(Name = "Base Calc. PIS")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorBaseCalculoPis { get; set; } = 0;
        [Display(Name = "Base Calc Cofins")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorBaseCalculoCofins { get; set; } = 0;
        [Display(Name = "Base Calc ICMS")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorBaseCalculoIcms { get; set; } = 0;
        [Display(Name = "Base Calc IPI")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorBaseCalculoIpi { get; set; } = 0;
        [Display(Name = "Valor ICMS")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorIcms { get; set; } = 0;
        [Display(Name = "Valor PIS")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorPis { get; set; } = 0;
        [Display(Name = "Valor Cofins")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorCofins { get; set; }
        [Display(Name = "Valor IPI")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorIpi { get; set; } = 0;
        [Display(Name = "Cod Produto/Fornecedor")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoProdutoFornecedor { get; set; }
        [Display(Name = "Preço Venda")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorNovoPrecoVenda { get; set; } = 0;
        public List<ProdutoViewModel.ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel.ProdutoViewModel>();
        public List<EstoqueViewModel.EstoqueViewModel> Estoques { get; set; } = new List<EstoqueViewModel.EstoqueViewModel>();
        public List<UnidadeIndexViewModel> Unidades { get; set; } = new List<UnidadeIndexViewModel>();
        public bool Importada { get; set; } = false;
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
        public List<ProdutoViewModel.ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel.ProdutoViewModel>();
        public List<EstoqueViewModel.EstoqueViewModel> Estoques { get; set; } = new List<EstoqueViewModel.EstoqueViewModel>();
        public List<UnidadeIndexViewModel> Unidades { get; set; } = new List<UnidadeIndexViewModel>();
        public bool Importada { get; set; } = false;
        public string certo { get; set; }
    }

    public class CompraFiscalViewModel
    {
        public long Id { get; set; }
        public Int64? IDCOMPRA { get; set; }
        public string NomeCompra { get; set; }
        public ETipoManifestoCompra? TipoManifesto { get; set; }
        public string Xml { get; set; }
    }

    public class ImportacaoArquivo
    {
        public long idCompra { get; set; }
        public IFormFile XmlArquivo { get; set; }
    }
}
