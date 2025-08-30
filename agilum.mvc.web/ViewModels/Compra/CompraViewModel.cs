using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilum.mvc.web.Configuration;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Estoque;
using agilum.mvc.web.ViewModels.Fornecedor;
using agilum.mvc.web.ViewModels.Impostos;
using agilum.mvc.web.ViewModels.Produtos;
using agilum.mvc.web.ViewModels.Turno;
using agilum.mvc.web.ViewModels.UnidadeViewModel;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Xml.Serialization;
using static agilum.mvc.web.ViewModels.Compra.EIcmsTipo;

namespace agilum.mvc.web.ViewModels.Compra
{
    #region Compra
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
        public string ValorIcmsRetido { get; set; }
        [Display(Name = "Base Calc. ICMS")]
        public string ValorBaseCalculoIcms { get; set; } 
        [Display(Name = "Valor ICMS")]
        
        [Moeda]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public string ValorIcms { get; set; } 
        [Display(Name = "Base Calc. Subs")]
        [Moeda]
        
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public string ValorBaseCalculoSub { get; set; } 
        [Display(Name = "ICMS Subs")]
        public string ValorIcmsSub { get; set; } 
        [Display(Name = "Isenção")]
        public string ValorIsencao { get; set; } 
        
        [Display(Name = "Valor Total dos Itens")]
        public string ValorTotalProduto { get; set; } 

        [Display(Name = "Frete")]
        public string ValorFrete { get; set; } 
        
        [Display(Name = "Seguro")]
        public string ValorSeguro { get; set; } 
        [Display(Name = "Desconto")]
        public string ValorDesconto { get; set; } 
        
        [Display(Name = "Outros")]
        public string ValorOutros { get; set; } 
        [Display(Name = "Valor IPI")]
        public string ValorIpi { get; set; } 
        [Display(Name = "Valor Total")]
        public string ValorTotal { get; set; } 
        [Display(Name = "Observação")]
        public string Observacao { get; set; }
        [Display(Name = "É Importada?")]
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
        public List<ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel>();
        public List<EstoqueViewModel> Estoques { get; set; } = new List<EstoqueViewModel>();
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
        public List<ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel>();
        public List<EstoqueViewModel> Estoques { get; set; } = new List<EstoqueViewModel>();
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
    #endregion

    #region NfeProc
    public class ImportacaoArquivo
    {
        public long idCompra { get; set; }
        public IFormFile XmlArquivo { get; set; }
    }
    
    [XmlRoot(ElementName = "nfeProc", Namespace = "http://www.portalfiscal.inf.br/nfe")]
    public class NFeProc
    {
        [XmlAttribute("versao")]
        public string versao { get; set; }

        [XmlElement("NFe", Namespace = "http://www.portalfiscal.inf.br/nfe")]
        public NFe NotaFiscalEletronica { get; set; } = new NFe();
        [Display(Name = "Caminho Arquivo")]
        public string CaminhoArquivo { get; set; }
        public long idCompra { get; set; }

        [XmlElement("protNFe")]
        public protNFe ProtNFe { get; set; } = new protNFe();

        [XmlIgnore]
        public bool sucesso { get; set; }
        [XmlIgnore]
        public string ArquivoXml { get; set; }

    }
    public class NFe
    {
        [XmlElement(ElementName = "infNFe")]
        public InfNFe InformacoesNFe { get; set; } = new InfNFe();

        [XmlElement]
        public Signature Signature { get; set; }

    }
    public class InfNFe
    {
        [XmlElement("ide")]
        public Identificacao Identificacao { get; set; } = new Identificacao();

        [XmlElement("emit")]
        public Emitente Emitente { get; set; } = new Emitente();

        [XmlElement("dest")]
        public Destinatario Destinatario { get; set; } = new Destinatario();

        [XmlElement("det")]
        public List<Detalhe> Detalhe { get; set; } = new List<Detalhe>();
        [XmlElement("total")]
        public Total TotalNFE { get; set; }
        [XmlElement("infAdic")]
        public infAdic InformacaoAdicional { get; set; }
        [XmlElement("infRespTec")]
        public infRespTec infRespTec { get; set; }
    }

    public class infRespTec
    {
        [XmlElement]
        public string CNPJ { get; set; }
        [XmlElement]
        public string xContato { get; set; }
        [XmlElement]
        public string email { get; set; }
        [XmlElement]
        public string fone { get; set; }
    }

    public class infAdic
    {
        [XmlElement("infCpl")]
        [Display(Name = "Informações Complementares")]
        public string infCpl { get; set; }
    }

    public class protNFe
    {
        [XmlAttribute("versao")]
        public string versao { get; set; }
        [XmlElement("infProt")]
        public infProt InfProt { get; set; } = new infProt();

    }

    public class infProt
    {
        [XmlElement("tpAmb")]
        public int tpAmb { get; set; }
        [XmlElement("verAplic")]
        public string verAplic { get; set; }
        [XmlElement("chNFe")]
        public string chNFe { get; set; }
        [XmlElement("dhRecbto")]
        public string dhRecbto { get; set; }
        [XmlElement("nProt")]
        public ulong nProt { get; set; }
        [XmlElement("digVal")]
        public string digVal { get; set; }
        [XmlElement("cStat")]
        public int cStat { get; set; }
        [XmlElement("xMotivo")]
        public string xMotivo { get; set; }
    }

    public class Signature
    {
        [XmlElement("SignedInfo")]
        public SignedInfo SignedInfo { get; set; } = new SignedInfo();
        [XmlElement("SignatureValue")]
        public string SignatureValue { get; set; }
        [XmlElement("KeyInfo")]
        public KeyInfo KeyInfo { get; set; } = new KeyInfo();
    }

    public class KeyInfo
    {
        [XmlElement("X509Data")]
        public X509Data X509Data { get; set; } = new X509Data();
    }

    public class X509Data
    {
        [XmlElement("X509Certificate")]
        public string X509Certificate { get; set; }
    }

    public class SignedInfo
    {
        [XmlElement("CanonicalizationMethod")]
        public Algoritmo CanonicalizationMethod { get; set; } = new Algoritmo();
        [XmlElement("SignatureMethod")]
        public Algoritmo SignatureMethod { get; set; } = new Algoritmo() { };
        [XmlElement("Reference")]
        public Reference Reference { get; set; } = new Reference() { };
    }

    public struct Algoritmo
    {
        [XmlAttribute]
        public string Algorithm { get; set; }
    }
    public class Reference
    {
        [XmlAttribute]
        public string URI { get; set; }

        [XmlArray]
        [XmlArrayItem(ElementName = "Transform")]
        public Algoritmo[] Transforms { get; set; }

        public Algoritmo DigestMethod { get; set; } = new Algoritmo();
        [XmlAttribute]
        public string DigestValue { get; set; }
    }

    public class Identificacao
    {
        [Display(Name = "UF")]
        public int cUF { get; set; }
        public string cNF { get; set; }
        [Display(Name = "Natureza da Operação")]
        public string natOp { get; set; }
        public int indPag { get; set; }
        [Display(Name = "Modelo")]
        public string mod { get; set; }
        [Display(Name = "Serie")]
        public int serie { get; set; }
        [Display(Name = "Numero")]
        public string nNF { get; set; }
        [Display(Name = "Data Emissão")]
        public DateTime dhEmi { get; set; }
        public DateTime dhSaiEnt { get; set; }
    }

    public class Destinatario
    {
        [Display(Name = "CNPJ")]
        public string CNPJ { get; set; }
        [Display(Name = "CPF")]
        public string CPF { get; set; }
        [Display(Name = "Nome Fantasia")]
        public string xNome { get; set; }
        [XmlElement("enderDest")]
        public Endereco Endereco { get; set; } = new Endereco();
        [Display(Name = "E-mail")]
        public string email { get; set; }
    }

    public class Detalhe
    {
        [XmlAttribute("nItem")]
        [Display(Name = "Item")]
        public int nItem { get; set; }

        [XmlElement("prod")]
        public Produto Produto { get; set; } = new Produto();
        [XmlElement("imposto")]
        public Imposto imposto { get; set; } = new Imposto();
    }

    public enum CSTCOFINS
    {
        /// <summary>
        /// 01 - Operação Tributável (base de cálculo = valor da operação (alíquota normal (cumulativo/não cumulativo)))
        /// </summary>
        [Description("Operação Tributável (base de cálculo = valor da operação (alíquota normal (cumulativo/não cumulativo)))")]
        [XmlEnum("01")]
        cofins01 = 01,

        /// <summary>
        /// 02 - Operação Tributável (base de cálculo = valor da operação (alíquota diferenciada))
        /// </summary>
        [Description("Operação Tributável (base de cálculo = valor da operação (alíquota diferenciada))")]
        [XmlEnum("02")]
        cofins02 = 02,

        /// <summary>
        /// 03 - Operação Tributável (base de cálculo = quantidade vendida (alíquota por unidade de produto))
        /// </summary>
        [Description("Operação Tributável (base de cálculo = quantidade vendida (alíquota por unidade de produto))")]
        [XmlEnum("03")]
        cofins03 = 03,

        /// <summary>
        /// 04 - Operação Tributável (tributação monofásica (alíquota zero))
        /// </summary>
        [Description("Operação Tributável (tributação monofásica (alíquota zero))")]
        [XmlEnum("04")]
        cofins04 = 04,

        /// <summary>
        /// 05 - Operação Tributável (Substituição Tributária)
        /// </summary>
        [Description("Operação Tributável (Substituição Tributária)")]
        [XmlEnum("05")]
        cofins05 = 05,

        /// <summary>
        /// 06 - Operação Tributável (alíquota zero)
        /// </summary>
        [Description("Operação Tributável (alíquota zero)")]
        [XmlEnum("06")]
        cofins06 = 06,

        /// <summary>
        /// 07 - Operação Isenta da Contribuição
        /// </summary>
        [Description("Operação Isenta da Contribuição")]
        [XmlEnum("07")]
        cofins07 = 07,

        /// <summary>
        /// 08 - Operação Sem Incidência da Contribuição
        /// </summary>
        [Description("Operação Sem Incidência da Contribuição")]
        [XmlEnum("08")]
        cofins08 = 08,

        /// <summary>
        /// 09 - Operação com Suspensão da Contribuição
        /// </summary>
        [Description("Operação com Suspensão da Contribuição")]
        [XmlEnum("09")]
        cofins09 = 09,

        /// <summary>
        /// 49 - Outras Operações de Saída
        /// </summary>
        [Description("Outras Operações de Saída")]
        [XmlEnum("49")]
        cofins49 = 49,

        /// <summary>
        /// 50 - Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Tributada no Mercado Interno
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Tributada no Mercado Interno")]
        [XmlEnum("50")]
        cofins50 = 50,

        /// <summary>
        /// 51 - Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Não Tributada no Mercado Interno
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Não Tributada no Mercado Interno")]
        [XmlEnum("51")]
        cofins51 = 51,

        /// <summary>
        /// 52 - Operação com Direito a Crédito - Vinculada Exclusivamente a Receita de Exportação
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada Exclusivamente a Receita de Exportação")]
        [XmlEnum("52")]
        cofins52 = 52,

        /// <summary>
        /// 53 - Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno")]
        [XmlEnum("53")]
        cofins53 = 53,

        /// <summary>
        /// 54 - Operação com Direito a Crédito - Vinculada a Receitas Tributadas no Mercado Interno e de Exportação
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada a Receitas Tributadas no Mercado Interno e de Exportação")]
        [XmlEnum("54")]
        cofins54 = 54,

        /// <summary>
        /// 55 - Operação com Direito a Crédito - Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação")]
        [XmlEnum("55")]
        cofins55 = 55,

        /// <summary>
        /// 56 - Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação")]
        [XmlEnum("56")]
        cofins56 = 56,

        /// <summary>
        /// 60 - Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Tributada no Mercado Interno
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Tributada no Mercado Interno")]
        [XmlEnum("60")]
        cofins60 = 60,

        /// <summary>
        /// 61 - Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Não-Tributada no Mercado Interno
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Não-Tributada no Mercado Interno")]
        [XmlEnum("61")]
        cofins61 = 61,

        /// <summary>
        /// 62 - Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita de Exportação
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita de Exportação")]
        [XmlEnum("62")]
        cofins62 = 62,

        /// <summary>
        /// 63 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno")]
        [XmlEnum("63")]
        cofins63 = 63,

        /// <summary>
        /// 64 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas no Mercado Interno e de Exportação
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas no Mercado Interno e de Exportação")]
        [XmlEnum("64")]
        cofins64 = 64,

        /// <summary>
        /// 65 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação")]
        [XmlEnum("65")]
        cofins65 = 65,

        /// <summary>
        /// 66 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação")]
        [XmlEnum("66")]
        cofins66 = 66,

        /// <summary>
        /// 67 - Crédito Presumido - Outras Operações
        /// </summary>
        [Description("Crédito Presumido - Outras Operações")]
        [XmlEnum("67")]
        cofins67 = 67,

        /// <summary>
        /// 70 - Operação de Aquisição sem Direito a Crédito
        /// </summary>
        [Description("Operação de Aquisição sem Direito a Crédito")]
        [XmlEnum("70")]
        cofins70 = 70,

        /// <summary>
        /// 71 - Operação de Aquisição com Isenção
        /// </summary>
        [Description("Operação de Aquisição com Isenção")]
        [XmlEnum("71")]
        cofins71 = 71,

        /// <summary>
        /// 72 - Operação de Aquisição com Suspensão
        /// </summary>
        [Description("Operação de Aquisição com Suspensão")]
        [XmlEnum("72")]
        cofins72 = 72,

        /// <summary>
        /// 73 - Operação de Aquisição a Alíquota Zero
        /// </summary>
        [Description("Operação de Aquisição a Alíquota Zero")]
        [XmlEnum("73")]
        cofins73 = 73,

        /// <summary>
        /// 74 - Operação de Aquisição sem Incidência da Contribuição
        /// </summary>
        [Description("Operação de Aquisição sem Incidência da Contribuição")]
        [XmlEnum("74")]
        cofins74 = 74,

        /// <summary>
        /// 75 - Operação de Aquisição por Substituição Tributária
        /// </summary>
        [Description("Operação de Aquisição por Substituição Tributária")]
        [XmlEnum("75")]
        cofins75 = 75,

        /// <summary>
        /// 98 - Outras Operações de Entrada
        /// </summary>
        [Description("Outras Operações de Entrada")]
        [XmlEnum("98")]
        cofins98 = 98,

        /// <summary>
        /// 99 - Outras Operações
        /// </summary>
        [Description("Outras Operações")]
        [XmlEnum("99")]
        cofins99 = 99
    }

    public class EIcmsTipo
    {
        #region Origem da Mercadoria

        /// <summary>
        ///     <para>0-Nacional exceto as indicadas nos códigos 3, 4, 5 e 8;</para>
        ///     <para>1-Estrangeira - Importação direta;</para>
        ///     <para>2-Estrangeira - Adquirida no mercado interno;</para>
        ///     <para>3-Nacional, conteudo superior 40% e inferior ou igual a 70%;</para>
        ///     <para>4-Nacional, processos produtivos básicos;</para>
        ///     <para>5-Nacional, conteudo inferior 40%;</para>
        ///     <para>6-Estrangeira - Importação direta, com similar nacional, lista CAMEX;</para>
        ///     <para>7-Estrangeira - mercado interno, sem simular,lista CAMEX;</para>
        ///     <para>8-Nacional, Conteúdo de Importação superior a 70%.</para>
        /// </summary>
        public enum OrigemMercadoria
        {
            /// <summary>
            /// 0-Nacional exceto as indicadas nos códigos 3, 4, 5 e 8
            /// </summary>
            [Description("Nacional exceto as indicadas nos códigos 3, 4, 5 e 8")]
            [XmlEnum("0")]
            OmNacional = 0,

            /// <summary>
            /// 1-Estrangeira - Importação direta
            /// </summary>
            [Description("Estrangeira - Importação direta")]
            [XmlEnum("1")]
            OmEstrangeiraImportacaoDireta = 1,

            /// <summary>
            /// 2-Estrangeira - Adquirida no mercado interno
            /// </summary>
            [Description("Estrangeira - Adquirida no mercado interno")]
            [XmlEnum("2")]
            OmEstrangeiraAdquiridaBrasil = 2,

            /// <summary>
            /// 3-Nacional, conteudo superior 40% e inferior ou igual a 70%
            /// </summary>
            [Description("Nacional, conteudo superior 40% e inferior ou igual a 70%")]
            [XmlEnum("3")]
            OmNacionalConteudoImportacaoSuperior40 = 3,

            /// <summary>
            /// 4-Nacional, processos produtivos básicos
            /// </summary>
            [Description("Nacional, processos produtivos básicos")]
            [XmlEnum("4")]
            OmNacionalProcessosBasicos = 4,

            /// <summary>
            /// 5-Nacional, conteudo inferior 40%
            /// </summary>
            [Description("Nacional, conteudo inferior 40%")]
            [XmlEnum("5")]
            OmNacionalConteudoImportacaoInferiorIgual40 = 5,

            /// <summary>
            /// 6-Estrangeira - Importação direta, com similar nacional, lista CAMEX
            /// </summary>
            [Description("Estrangeira - Importação direta, com similar nacional, lista CAMEX")]
            [XmlEnum("6")]
            OmEstrangeiraImportacaoDiretaSemSimilar = 6,

            /// <summary>
            /// 7-Estrangeira - mercado interno, sem simular,lista CAMEX
            /// </summary>
            [Description("Estrangeira - mercado interno, sem simular,lista CAMEX")]
            [XmlEnum("7")]
            OmEstrangeiraAdquiridaBrasilSemSimilar = 7,

            /// <summary>
            /// 8-Nacional, Conteúdo de Importação superior a 70%
            /// </summary>
            [Description("Nacional, Conteúdo de Importação superior a 70%")]
            [XmlEnum("8")]
            OmNacionalConteudoImportacaoSuperior70 = 8
        }

        #endregion

        #region Situação Tributária do ICMS

        /// <summary>
        ///     <para>00 - Tributada integralmente</para>
        ///     <para>02 - Tributação monofásica própria sobre combustíveis</para>
        ///     <para>10 - Tributada e com cobrança do ICMS por substituição tributária</para>
        ///     <para>15 - Tributação monofásica própria e com responsabilidade pela retenção sobre combustíveis</para>
        ///     <para>20 - Com redução de base de cálculo</para>
        ///     <para>30 - Isenta ou não tributada e com cobrança do ICMS por substituição tributária</para>
        ///     <para>40 - Isenta</para>
        ///     <para>41 - Não tributada</para>
        ///     <para>50 - Suspensão</para>
        ///     <para>51 - Diferimento</para>
        ///     <para>53 - Tributação monofásica sobre combustíveis com recolhimento diferido</para>
        ///     <para>60 - ICMS cobrado anteriormente por substituição tributária</para>
        ///     <para>61 - Tributação monofásica sobre combustíveis cobrada anteriormente</para>
        ///     <para>70 - Com redução de base de cálculo e cobrança do ICMS por substituição tributária</para>
        ///     <para>90 - Outras</para>
        /// </summary>
        public enum Csticms
        {
            /// <summary>
            /// 00 - Tributada integralmente
            /// </summary>
            [Description("Tributada integralmente")]
            [XmlEnum("00")]
            Cst00,

            /// <summary>
            /// 02 - Tributação monofásica própria sobre combustíveis
            /// </summary>
            [Description("Tributação monofásica própria sobre combustíveis")]
            [XmlEnum("02")]
            Cst02,

            /// <summary>
            /// 10 - Tributada e com cobrança do ICMS por substituição tributária
            /// </summary>
            [Description("Tributada e com cobrança do ICMS por substituição tributária")]
            [XmlEnum("10")]
            Cst10,

            /// <summary>
            /// 15 - Tributação monofásica própria e com responsabilidade pela retenção sobre combustíveis
            /// </summary>
            [Description("Tributação monofásica própria e com responsabilidade pela retenção sobre combustíveis")]
            [XmlEnum("15")]
            Cst15,

            /// <summary>
            /// 10 - Tributada e com cobrança do ICMS por substituição tributária
            /// </summary>
            [Description("Tributada e com cobrança do ICMS por substituição tributária")]
            [XmlEnum("10")]
            CstPart10,

            /// <summary>
            /// 20 - Com redução de base de cálculo
            /// </summary>
            [Description("Com redução de base de cálculo")]
            [XmlEnum("20")]
            Cst20,

            /// <summary>
            /// 30 - Isenta ou não tributada e com cobrança do ICMS por substituição tributária
            /// </summary>
            [Description("Isenta ou não tributada e com cobrança do ICMS por substituição tributária")]
            [XmlEnum("30")]
            Cst30,

            /// <summary>
            /// 40 - Isenta
            /// </summary>
            [Description("Isenta")]
            [XmlEnum("40")]
            Cst40,

            /// <summary>
            /// 41 - Não tributada
            /// </summary>
            [Description("Não tributada")]
            [XmlEnum("41")]
            Cst41,

            /// <summary>
            /// 41 - Não tributada
            /// </summary>
            [Description("Não tributada")]
            [XmlEnum("41")]
            CstRep41,

            /// <summary>
            /// 50 - Suspensão
            /// </summary>
            [Description("Suspensão")]
            [XmlEnum("50")]
            Cst50,

            /// <summary>
            /// 51 - Diferimento
            /// </summary>
            [Description("Diferimento")]
            [XmlEnum("51")]
            Cst51,

            /// <summary>
            /// 53 - Tributação monofásica sobre combustíveis com recolhimento diferido
            /// </summary>
            [Description("Tributação monofásica sobre combustíveis com recolhimento diferido")]
            [XmlEnum("53")]
            Cst53,

            /// <summary>
            /// 60 - ICMS cobrado anteriormente por substituição tributária
            /// </summary>
            [Description("ICMS cobrado anteriormente por substituição tributária")]
            [XmlEnum("60")]
            Cst60,

            /// <summary>
            /// 60 - ICMS cobrado anteriormente por substituição tributária
            /// </summary>
            [XmlEnum("60")] CstRep60,

            /// <summary>
            /// 61 - Tributação monofásica sobre combustíveis cobrada anteriormente
            /// </summary>
            [Description("Tributação monofásica sobre combustíveis cobrada anteriormente")]
            [XmlEnum("61")]
            Cst61,

            /// <summary>
            /// 70 - Com redução de base de cálculo e cobrança do ICMS por substituição tributária
            /// </summary>
            [Description("Com redução de base de cálculo e cobrança do ICMS por substituição tributária")]
            [XmlEnum("70")]
            Cst70,

            /// <summary>
            /// 90 - Outras
            /// </summary>
            [Description("Outras")]
            [XmlEnum("90")]
            Cst90,

            /// <summary>
            /// 90 - Outras
            /// </summary>
            [Description("Outras")]
            [XmlEnum("90")]
            CstPart90
        }

        #endregion

        #region Modalidade de determinação da BC do ICMS

        /// <summary>
        ///     <para>0 - Margem Valor Agregado (%);</para>
        ///     <para>1 - Pauta (valor);</para>
        ///     <para>2 - Preço Tabelado Máximo (valor);</para>
        ///     <para>3 - Valor da Operação.</para>
        /// </summary>
        public enum DeterminacaoBaseIcms
        {
            /// <summary>
            /// 0 - Margem Valor Agregado (%)
            /// </summary>
            [Description("Margem Valor Agregado (%)")]
            [XmlEnum("0")]
            DbiMargemValorAgregado = 0,

            /// <summary>
            /// 1 - Pauta (valor)
            /// </summary>
            [Description("Pauta (valor)")]
            [XmlEnum("1")]
            DbiPauta = 1,

            /// <summary>
            /// 2 - Preço Tabelado Máximo (valor)
            /// </summary>
            [Description("Preço Tabelado Máximo (valor)")]
            [XmlEnum("2")]
            DbiPrecoTabelado = 2,

            /// <summary>
            /// 3 - Valor da Operação
            /// </summary>
            [Description("Valor da Operação")]
            [XmlEnum("3")]
            DbiValorOperacao = 3
        }

        #endregion

        #region Modalidade de determinação da BC do ICMS ST

        /// <summary>
        ///     <para>0 – Preço tabelado ou máximo  sugerido;</para>
        ///     <para>1 - Lista Negativa (valor);</para>
        ///     <para>2 - Lista Positiva (valor);</para>
        ///     <para>3 - Lista Neutra (valor);</para>
        ///     <para>4 - Margem Valor Agregado (%);</para>
        ///     <para>5 - Pauta (valor);</para>
        ///     <para>6 - Valor da Operação;</para>
        /// </summary>
        public enum DeterminacaoBaseIcmsSt
        {
            /// <summary>
            /// 0 – Preço tabelado ou máximo  sugerido
            /// </summary>
            [Description("Preço tabelado ou máximo  sugerido")]
            [XmlEnum("0")]
            DbisPrecoTabelado = 0,

            /// <summary>
            /// 1 - Lista Negativa (valor)
            /// </summary>
            [Description("Lista Negativa (valor)")]
            [XmlEnum("1")]
            DbisListaNegativa = 1,

            /// <summary>
            /// 2 - Lista Positiva (valor)
            /// </summary>
            [Description("Lista Positiva (valor)")]
            [XmlEnum("2")]
            DbisListaPositiva = 2,

            /// <summary>
            /// 3 - Lista Neutra (valor)
            /// </summary>
            [Description("Lista Neutra (valor)")]
            [XmlEnum("3")]
            DbisListaNeutra = 3,

            /// <summary>
            /// 4 - Margem Valor Agregado (%)
            /// </summary>
            [Description("Margem Valor Agregado (%)")]
            [XmlEnum("4")]
            DbisMargemValorAgregado = 4,

            /// <summary>
            /// 5 - Pauta (valor)
            /// </summary>
            [Description("Pauta (valor)")]
            [XmlEnum("5")]
            DbisPauta = 5,

            /// <summary>
            /// 6 - Valor da Operação
            /// </summary>
            [Description("Valor da Operação")]
            [XmlEnum("6")]
            DbisValordaOperacao = 6
        }

        #endregion

        #region Situação Tributária do CSOSN

        /// <summary>
        ///     <para>101 - Tributada pelo Simples Nacional com permissão de crédito.(v.2.0)</para>
        ///     <para>102 - Tributada pelo Simples Nacional sem permissão de crédito.</para>
        ///     <para>103 – Isenção do ICMS  no Simples Nacional para faixa de receita bruta.</para>
        ///     <para>201 - Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS por Substituição Tributária (v.2.0)</para>
        ///     <para>202 - Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por Substituição Tributária</para>
        ///     <para>203 - Isenção do ICMS nos Simples Nacional para faixa de receita bruta e com cobrança do ICMS por Substituição Tributária (v.2.0)</para>
        ///     <para>300 – Imune.</para>
        ///     <para>400 – Não tributda pelo Simples Nacional (v.2.0)</para>
        ///     <para>500 – ICMS cobrado anterirmente por substituição tributária (substituído) ou por antecipação (v.2.0)</para>
        ///     <para>Tributação pelo ICMS 900 - Outros(v2.0)</para>
        /// </summary>
        public enum Csosnicms
        {
            /// <summary>
            /// 101 - Tributada pelo Simples Nacional com permissão de crédito
            /// </summary>
            [Description("Tributada pelo Simples Nacional com permissão de crédito")]
            [XmlEnum("101")]
            Csosn101 = 101,

            /// <summary>
            /// 102 - Tributada pelo Simples Nacional sem permissão de crédito
            /// </summary>
            [Description("Tributada pelo Simples Nacional sem permissão de crédito")]
            [XmlEnum("102")]
            Csosn102 = 102,

            /// <summary>
            /// 103 – Isenção do ICMS  no Simples Nacional para faixa de receita bruta
            /// </summary>
            [Description("Isenção do ICMS  no Simples Nacional para faixa de receita bruta")]
            [XmlEnum("103")]
            Csosn103 = 103,

            /// <summary>
            /// 201 - Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS por Substituição Tributária
            /// </summary>
            [Description("Tributada pelo Simples Nacional com permissão de crédito e com cobrança do ICMS por Substituição Tributária")]
            [XmlEnum("201")]
            Csosn201 = 201,

            /// <summary>
            /// 202 - Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por Substituição Tributária
            /// </summary>
            [Description("Tributada pelo Simples Nacional sem permissão de crédito e com cobrança do ICMS por Substituição Tributária")]
            [XmlEnum("202")]
            Csosn202 = 202,

            /// <summary>
            /// 203 - Isenção do ICMS nos Simples Nacional para faixa de receita bruta e com cobrança do ICMS por Substituição Tributária
            /// </summary>
            [Description("Isenção do ICMS nos Simples Nacional para faixa de receita bruta e com cobrança do ICMS por Substituição Tributária")]
            [XmlEnum("203")]
            Csosn203 = 203,

            /// <summary>
            /// 300 – Imune
            /// </summary>
            [Description("Imune")]
            [XmlEnum("300")]
            Csosn300 = 300,

            /// <summary>
            /// 400 – Não tributada pelo Simples Nacional
            /// </summary>
            [Description("Não tributada pelo Simples Nacional")]
            [XmlEnum("400")]
            Csosn400 = 400,

            /// <summary>
            /// 500 – ICMS cobrado anterirmente por substituição tributária (substituído) ou por antecipação
            /// </summary>
            [Description("ICMS cobrado anterirmente por substituição tributária (substituído) ou por antecipação")]
            [XmlEnum("500")]
            Csosn500 = 500,

            /// <summary>
            /// 900 - Outros
            /// </summary>
            [Description("Outros")]
            [XmlEnum("900")]
            Csosn900 = 900
        }

        #endregion

        #region Motivo da desoneração do ICMS

        /// <summary>
        ///     <para>1 – Táxi;</para>
        ///     <para>2 – Deficiente Físico;</para>
        ///     <para>3 – Produtor Agropecuário;</para>
        ///     <para>4 – Frotista/Locadora;</para>
        ///     <para>5 – Diplomático/Consular;</para>
        ///     <para>6 – Utilitários e Motocicletas da Amazônia Ocidental e Áreas de Livre Comércio (Resolução 714/88 e 790/94 – CONTRAN e suas alterações);</para>
        ///     <para>7 – SUFRAMA;</para>
        ///     <para>8 – Venda a Orgãos Publicos;</para>
        ///     <para>9 – Outros. (v2.0)</para>
        ///     <para>10 – Deficiente Condutor (Convênio ICMS 38/12). (v3.1)</para>
        ///     <para>11 – Deficiente não Condutor (Convênio ICMS 38/12). (v3.1)</para>
        ///     <para>12 – Fomento agropecuário</para>
        ///     <para>16 - Olimpíadas Rio 2016</para>
        ///     <para>90 - Solicitado pelo Fisco</para>
        /// </summary>
        public enum MotivoDesoneracaoIcms
        {
            /// <summary>
            /// 1 – Táxi
            /// </summary>
            [Description("Táxi")]
            [XmlEnum("1")]
            MdiTaxi = 1,

            /// <summary>
            /// 2 – Deficiente Físico
            /// </summary>
            [Description("Deficiente Físico")]
            [XmlEnum("2")]
            MdiDeficienteFisico = 2,

            /// <summary>
            /// 3 – Produtor Agropecuário
            /// </summary>
            [Description("Produtor Agropecuário")]
            [XmlEnum("3")]
            MdiProdutorAgropecuario = 3,

            /// <summary>
            /// 4 – Frotista/Locadora
            /// </summary>
            [Description("Frotista/Locadora")]
            [XmlEnum("4")]
            MdiFrotistaLocadora = 4,

            /// <summary>
            /// 5 – Diplomático/Consular
            /// </summary>
            [Description("Diplomático/Consular")]
            [XmlEnum("5")]
            MdiDiplomaticoConsular = 5,

            /// <summary>
            /// 6 – Utilitários e Motocicletas da Amazônia Ocidental e Áreas de Livre Comércio (Resolução 714/88 e 790/94 – CONTRAN e suas alterações)
            /// </summary>
            [Description("Utilitários e Motocicletas da Amazônia Ocidental e Áreas de Livre Comércio (Resolução 714/88 e 790/94 – CONTRAN e suas alterações)")]
            [XmlEnum("6")]
            MdiAmazoniaLivreComercio = 6,

            /// <summary>
            /// 7 – SUFRAMA
            /// </summary>
            [Description("SUFRAMA")]
            [XmlEnum("7")]
            MdiSuframa = 7,

            /// <summary>
            /// 8 – Venda a Orgãos Publicos
            /// </summary>
            [Description("Venda a Orgãos Publicos")]
            [XmlEnum("8")]
            MdiVendaOrgaosPublicos = 8,

            /// <summary>
            /// 9 – Outros. (v2.0)
            /// </summary>
            [Description("Outros")]
            [XmlEnum("9")]
            MdiOutros = 9,

            /// <summary>
            /// 10 – Deficiente Condutor (Convênio ICMS 38/12). (v3.1)
            /// </summary>
            [Description("Deficiente Condutor (Convênio ICMS 38/12)")]
            [XmlEnum("10")]
            MdiDeficienteCondutor = 10,

            /// <summary>
            /// 11 – Deficiente não Condutor (Convênio ICMS 38/12). (v3.1)
            /// </summary>
            [Description("Deficiente não Condutor (Convênio ICMS 38/12)")]
            [XmlEnum("11")]
            MdiDeficienteNaoCondutor = 11,

            /// <summary>
            /// 12 – Fomento agropecuário
            /// </summary>
            [Description("Fomento agropecuário")]
            [XmlEnum("12")]
            MdiFomentoAgropecuario = 12,

            /// <summary>
            /// 16 - Olimpíadas Rio 2016
            /// </summary>
            [Description("Olimpíadas Rio 2016")]
            [XmlEnum("16")]
            MdiOlimpiadasRio2016 = 16,

            /// <summary>
            /// 90 - Solicitado pelo Fisco
            /// </summary>
            [Description("Solicitado pelo Fisco")]
            [XmlEnum("90")]
            MdiSolicitadoPeloFisco = 90
        }

        #endregion
    }

    public class Emitente
    {
        [Display(Name = "CPF/CNPJ")]
        public string CNPJ { get; set; }
        [Display(Name = "Razão Social")]
        public string xNome { get; set; }
        [Display(Name = "Nome Fantasia")]
        public string xFant { get; set; }
        [XmlElement("enderEmit")]
        [Display(Name = "Logradouro")]
        public Endereco Endereco { get; set; } = new Endereco();
        [Display(Name = "Inscrição Estadual")]
        public string IE { get; set; }
        public string IEST { get; set; }
        public int CRT { get; set; }

    }

    public class Endereco
    {
        [Display(Name = "Logradouro")]
        public string xLgr { get; set; }
        [Display(Name = "Numero")]
        public string nro { get; set; }
        [Display(Name = "Bairro")]
        public string xBairro { get; set; }
        [Display(Name = "Municipio")]
        public string cMun { get; set; }
        [Display(Name = "Municipio")]
        public string xMun { get; set; }
        [Display(Name = "Estado")]
        public string UF { get; set; }
        [Display(Name = "CEP")]
        public string CEP { get; set; }
        [Display(Name = "País")]
        public int cPais { get; set; }
        [Display(Name = "País")]
        public string xPais { get; set; }
        public string fone { get; set; }
    }

    public enum CSTPIS
    {
        /// <summary>
        /// 01 - Operação Tributável (base de cálculo = valor da operação (alíquota normal (cumulativo/não cumulativo)))
        /// </summary>
        [Description("Operação Tributável (base de cálculo = valor da operação (alíquota normal (cumulativo/não cumulativo)))")]
        [XmlEnum("01")]
        pis01 = 01,

        /// <summary>
        /// 02 - Operação Tributável (base de cálculo = valor da operação (alíquota diferenciada))
        /// </summary>
        [Description("Operação Tributável (base de cálculo = valor da operação (alíquota diferenciada))")]
        [XmlEnum("02")]
        pis02 = 02,

        /// <summary>
        /// 03 - Operação Tributável (base de cálculo = quantidade vendida (alíquota por unidade de produto))
        /// </summary>
        [Description("Operação Tributável (base de cálculo = quantidade vendida (alíquota por unidade de produto))")]
        [XmlEnum("03")]
        pis03 = 03,

        /// <summary>
        /// 04 - Operação Tributável (tributação monofásica (alíquota zero))
        /// </summary>
        [Description("Operação Tributável (tributação monofásica (alíquota zero))")]
        [XmlEnum("04")]
        pis04 = 04,

        /// <summary>
        /// 05 - Operação Tributável (Substituição Tributária)
        /// </summary>
        [Description("Operação Tributável (Substituição Tributária)")]
        [XmlEnum("05")]
        pis05 = 05,

        /// <summary>
        /// 06 - Operação Tributável (alíquota zero)
        /// </summary>
        [Description("Operação Tributável (alíquota zero)")]
        [XmlEnum("06")]
        pis06 = 06,

        /// <summary>
        /// 07 - Operação Isenta da Contribuição
        /// </summary>
        [Description("Operação Isenta da Contribuição")]
        [XmlEnum("07")]
        pis07 = 07,

        /// <summary>
        /// 08 - Operação Sem Incidência da Contribuição
        /// </summary>
        [Description("Operação Sem Incidência da Contribuição")]
        [XmlEnum("08")]
        pis08 = 08,

        /// <summary>
        /// 09 - Operação com Suspensão da Contribuição
        /// </summary>
        [Description("Operação com Suspensão da Contribuição")]
        [XmlEnum("09")]
        pis09 = 09,

        /// <summary>
        /// 49 - Outras Operações de Saída
        /// </summary>
        [Description("Outras Operações de Saída")]
        [XmlEnum("49")]
        pis49 = 49,

        /// <summary>
        /// 50 - Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Tributada no Mercado Interno
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Tributada no Mercado Interno")]
        [XmlEnum("50")]
        pis50 = 50,

        /// <summary>
        /// 51 - Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Não Tributada no Mercado Interno
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada Exclusivamente a Receita Não Tributada no Mercado Interno")]
        [XmlEnum("51")]
        pis51 = 51,

        /// <summary>
        /// 52 - Operação com Direito a Crédito - Vinculada Exclusivamente a Receita de Exportação
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada Exclusivamente a Receita de Exportação")]
        [XmlEnum("52")]
        pis52 = 52,

        /// <summary>
        /// 53 - Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno")]
        [XmlEnum("53")]
        pis53 = 53,

        /// <summary>
        /// 54 - Operação com Direito a Crédito - Vinculada a Receitas Tributadas no Mercado Interno e de Exportação
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada a Receitas Tributadas no Mercado Interno e de Exportação")]
        [XmlEnum("54")]
        pis54 = 54,

        /// <summary>
        /// 55 - Operação com Direito a Crédito - Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação")]
        [XmlEnum("55")]
        pis55 = 55,

        /// <summary>
        /// 56 - Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação
        /// </summary>
        [Description("Operação com Direito a Crédito - Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação")]
        [XmlEnum("56")]
        pis56 = 56,

        /// <summary>
        /// 60 - Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Tributada no Mercado Interno
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Tributada no Mercado Interno")]
        [XmlEnum("60")]
        pis60 = 60,

        /// <summary>
        /// 61 - Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Não-Tributada no Mercado Interno
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita Não-Tributada no Mercado Interno")]
        [XmlEnum("61")]
        pis61 = 61,

        /// <summary>
        /// 62 - Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita de Exportação
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada Exclusivamente a Receita de Exportação")]
        [XmlEnum("62")]
        pis62 = 62,

        /// <summary>
        /// 63 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno")]
        [XmlEnum("63")]
        pis63 = 63,

        /// <summary>
        /// 64 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas no Mercado Interno e de Exportação
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas no Mercado Interno e de Exportação")]
        [XmlEnum("64")]
        pis64 = 64,

        /// <summary>
        /// 65 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada a Receitas Não-Tributadas no Mercado Interno e de Exportação")]
        [XmlEnum("65")]
        pis65 = 65,

        /// <summary>
        /// 66 - Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação
        /// </summary>
        [Description("Crédito Presumido - Operação de Aquisição Vinculada a Receitas Tributadas e Não-Tributadas no Mercado Interno, e de Exportação")]
        [XmlEnum("66")]
        pis66 = 66,

        /// <summary>
        /// 67 - Crédito Presumido - Outras Operações
        /// </summary>
        [Description("Crédito Presumido - Outras Operações")]
        [XmlEnum("67")]
        pis67 = 67,

        /// <summary>
        /// 70 - Operação de Aquisição sem Direito a Crédito
        /// </summary>
        [Description("Operação de Aquisição sem Direito a Crédito")]
        [XmlEnum("70")]
        pis70 = 70,

        /// <summary>
        /// 71 - Operação de Aquisição com Isenção
        /// </summary>
        [Description("Operação de Aquisição com Isenção")]
        [XmlEnum("71")]
        pis71 = 71,

        /// <summary>
        /// 72 - Operação de Aquisição com Suspensão
        /// </summary>
        [Description("Operação de Aquisição com Suspensão")]
        [XmlEnum("72")]
        pis72 = 72,

        /// <summary>
        /// 73 - Operação de Aquisição a Alíquota Zero
        /// </summary>
        [Description("Operação de Aquisição a Alíquota Zero")]
        [XmlEnum("73")]
        pis73 = 73,

        /// <summary>
        /// 74 - Operação de Aquisição sem Incidência da Contribuição
        /// </summary>
        [Description("Operação de Aquisição sem Incidência da Contribuição")]
        [XmlEnum("74")]
        pis74 = 74,

        /// <summary>
        /// 75 - Operação de Aquisição por Substituição Tributária
        /// </summary>
        [Description("Operação de Aquisição por Substituição Tributária")]
        [XmlEnum("75")]
        pis75 = 75,

        /// <summary>
        /// 98 - Outras Operações de Entrada
        /// </summary>
        [Description("Outras Operações de Entrada")]
        [XmlEnum("98")]
        pis98 = 98,

        /// <summary>
        /// 99 - Outras Operações
        /// </summary>
        [Description("Outras Operações")]
        [XmlEnum("99")]
        pis99 = 99
    }

    public class Imposto
    {
        [XmlElement]
        public double vTotTrib { get; set; }
        [XmlElement(ElementName = "ICMS")]
        public Icms ICMS { get; set; } = new Icms();
        [XmlElement(ElementName = "COFINS")]
        public Cofins COFINS { get; set; } = new Cofins();
        [XmlElement(ElementName = "PIS")]
        public PIS Pis { get; set; } = new PIS();
    }

    public class Icms
    {
        [XmlElement]
        public ICMS00 ICMS00 { get; set; } = new ICMS00();
        [XmlElement]
        public ICMS10 ICMS10 { get; set; } = new ICMS10();
        [XmlElement]
        public ICMS60 ICMS60 { get; set; } = new ICMS60();
        [XmlElement]
        public ICMS70 ICMS70 { get; set; } = new ICMS70();
        [XmlElement]
        public ICMSSN102 ICMSSN102 { get; set; } = new ICMSSN102();
    }

    #region ICMS
    public class ICMS00
    {
        [XmlElement]
        public int orig { get; set; }
        [XmlElement]
        public int CST { get; set; }
        [XmlElement]
        public int modBC { get; set; }
        [XmlElement]
        public double vBC { get; set; }
        [XmlElement]
        public double pICMS { get; set; }
        [XmlElement]
        public double vICMS { get; set; }
    }

    public class ICMS10
    {
        [XmlElement(Order = 1)]
        public OrigemMercadoria orig { get; set; }

        /// <summary>
        ///     N12- Situação Tributária
        /// </summary>
        [XmlElement(Order = 2)]
        public Csticms CST { get; set; }

        /// <summary>
        ///     N13 - Modalidade de determinação da BC do ICMS
        /// </summary>
        [XmlElement(Order = 3)]
        public DeterminacaoBaseIcms modBC { get; set; }

        /// <summary>
        ///     N15 - Valor da BC do ICMS
        /// </summary>
        [XmlElement(Order = 4)]
        public decimal vBC
        {
            get;
            set;
        }

        /// <summary>
        ///     N16 - Alíquota do imposto
        /// </summary>
        [XmlElement(Order = 5)]
        public decimal pICMS
        {
            get;
            set;
        }

        /// <summary>
        ///     N17 - Valor do ICMS
        /// </summary>
        [XmlElement(Order = 6)]
        public decimal vICMS
        {
            get;
            set;
        }

        /// <summary>
        /// N17a - Valor da Base de Cálculo do FCP
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 7)]
        public decimal? vBCFCP
        {
            get;
            set;
        }

        /// <summary>
        /// N17b - Percentual do Fundo de Combate à Pobreza (FCP)
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 8)]
        public decimal? pFCP
        {
            get;
            set;
        }

        /// <summary>
        /// N17c - Valor do Fundo de Combate à Pobreza (FCP)
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 9)]
        public decimal? vFCP
        {
            get;
            set;
        }

        /// <summary>
        ///     N18 - Modalidade de determinação da BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 10)]
        public DeterminacaoBaseIcmsSt modBCST { get; set; }

        /// <summary>
        ///     N19 - Percentual da margem de valor Adicionado do ICMS ST
        /// </summary>
        [XmlElement(Order = 11)]
        public decimal? pMVAST
        {
            get;
            set;
        }

        /// <summary>
        ///     N20 - Percentual da Redução de BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 12)]
        public decimal? pRedBCST
        {
            get;
            set;
        }

        /// <summary>
        ///     N21 - Valor da BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 13)]
        public decimal vBCST
        {
            get;
            set;
        }

        /// <summary>
        ///     N22 - Alíquota do imposto do ICMS ST
        /// </summary>
        [XmlElement(Order = 14)]
        public decimal pICMSST
        {
            get;
            set;
        }

        /// <summary>
        ///     N23 - Valor do ICMS ST
        /// </summary>
        [XmlElement(Order = 15)]
        public decimal vICMSST
        {
            get;
            set;
        }

        /// <summary>
        /// N23a - Valor da Base de Cálculo do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 16)]
        public decimal? vBCFCPST
        {
            get;
            set;
        }

        /// <summary>
        /// N23b - Percentual do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 17)]
        public decimal? pFCPST
        {
            get;
            set;
        }


        /// <summary>
        /// N23d - Valor do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 18)]
        public decimal? vFCPST
        {
            get;
            set;
        }
    }

    public class ICMS60
    {
        /// <summary>
        ///     N11 - Origem da Mercadoria
        /// </summary>
        [XmlElement(Order = 1)]
        public OrigemMercadoria orig { get; set; }

        /// <summary>
        ///     N12- Situação Tributária
        /// </summary>
        [XmlElement(Order = 2)]
        public Csticms CST { get; set; }

        /// <summary>
        ///     N26 - Valor da BC do ICMS ST retido
        /// </summary>
        [XmlElement(Order = 3)]
        public decimal? vBCSTRet { get; set; }
        public bool ShouldSerializevBCSTRet()
        {
            return vBCSTRet.HasValue;
        }

        /// <summary>
        ///     N26a - Alíquota suportada pelo Consumidor Final
        /// </summary>
        [XmlElement(Order = 4)]
        public decimal? pST { get; set; }

        [XmlElement(Order = 5)]
        public decimal? vICMSSubstituto { get; set; }

        /// <summary>
        ///     N27 - Valor do ICMS ST retido
        /// </summary>
        [XmlElement(Order = 6)]
        public decimal? vICMSSTRet { get; set; }


        /// <summary>
        /// N27a - Valor da Base de Cálculo do FCP retido anteriormente por ST 
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 7)]
        public decimal? vBCFCPSTRet { get; set; }


        /// <summary>
        /// N27b - Percentual do FCP retido anteriormente por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 8)]
        public decimal? pFCPSTRet { get; set; }


        /// <summary>
        /// N27d - Valor do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 9)]
        public decimal? vFCPSTRet { get; set; }

        /// <summary>
        ///     N34 - Percentual de redução da base de cálculo efetiva 
        /// </summary>
        [XmlElement(Order = 10)]
        public decimal? pRedBCEfet { get; set; }

        /// <summary>
        ///     N35 - Valor da base de cálculo efetiva 
        /// </summary>
        [XmlElement(Order = 11)]
        public decimal? vBCEfet { get; set; }

        /// <summary>
        ///     N36 - Alíquota do ICMS efetiva 
        /// </summary>
        [XmlElement(Order = 12)]
        public decimal? pICMSEfet { get; set; }

        /// <summary>
        ///     N37 - Valor do ICMS efetivo 
        /// </summary>
        [XmlElement(Order = 13)]
        public decimal? vICMSEfet { get; set; }


    }

    public class ICMS70
    {
        /// <summary>
        ///     N11 - Origem da Mercadoria
        /// </summary>
        [XmlElement(Order = 1)]
        public OrigemMercadoria orig { get; set; }

        /// <summary>
        ///     N12- Situação Tributária
        /// </summary>
        [XmlElement(Order = 2)]
        public Csticms CST { get; set; }

        /// <summary>
        ///     N13 - Modalidade de determinação da BC do ICMS
        /// </summary>
        [XmlElement(Order = 3)]
        public DeterminacaoBaseIcms modBC { get; set; }

        /// <summary>
        ///     N14 - Percentual de redução da BC
        /// </summary>
        [XmlElement(Order = 4)]
        public decimal pRedBC { get; set; }

        /// <summary>
        ///     N15 - Valor da BC do ICMS
        /// </summary>
        [XmlElement(Order = 5)]
        public decimal vBC { get; set; }

        /// <summary>
        ///     N16 - Alíquota do imposto
        /// </summary>
        [XmlElement(Order = 6)]
        public decimal pICMS { get; set; }

        /// <summary>
        ///     N17 - Valor do ICMS
        /// </summary>
        [XmlElement(Order = 7)]
        public decimal vICMS { get; set; }

        /// <summary>
        /// N17a - Valor da Base de Cálculo do FCP
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 8)]
        public decimal? vBCFCP { get; set; }


        /// <summary>
        /// N17b - Percentual do Fundo de Combate à Pobreza (FCP)
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 9)]
        public decimal? pFCP { get; set; }


        /// <summary>
        /// N17c - Valor do Fundo de Combate à Pobreza (FCP)
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 10)]
        public decimal? vFCP { get; set; }


        /// <summary>
        ///     N18 - Modalidade de determinação da BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 11)]
        public DeterminacaoBaseIcmsSt modBCST { get; set; }

        /// <summary>
        ///     N19 - Percentual da margem de valor Adicionado do ICMS ST
        /// </summary>
        [XmlElement(Order = 12)]
        public decimal? pMVAST { get; set; }
        /// <summary>
        ///     N20 - Percentual da Redução de BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 13)]
        public decimal? pRedBCST { get; set; }

        /// <summary>
        ///     N21 - Valor da BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 14)]
        public decimal vBCST { get; set; }

        /// <summary>
        ///     N22 - Alíquota do imposto do ICMS ST
        /// </summary>
        [XmlElement(Order = 15)]
        public decimal pICMSST { get; set; }

        /// <summary>
        ///     N23 - Valor do ICMS ST
        /// </summary>
        [XmlElement(Order = 16)]
        public decimal vICMSST { get; set; }

        /// <summary>
        /// N23a - Valor da Base de Cálculo do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 17)]
        public decimal? vBCFCPST { get; set; }


        /// <summary>
        /// N23b - Percentual do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 18)]
        public decimal? pFCPST { get; set; }

        /// <summary>
        /// N23d - Valor do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 19)]
        public decimal? vFCPST { get; set; }

        /// <summary>
        ///     N27a - Valor do ICMS desonerado
        /// </summary>
        [XmlElement(Order = 20)]
        public decimal? vICMSDeson { get; set; }

        /// <summary>
        ///     N28 - Motivo da desoneração do ICMS
        /// </summary>
        [XmlElement(Order = 21)]
        public MotivoDesoneracaoIcms? motDesICMS { get; set; }
    }

    public class ICMSSN102
    {
        /// <summary>
        ///     N11 - Origem da Mercadoria
        /// </summary>
        [XmlElement(Order = 1)]
        public OrigemMercadoria orig { get; set; }

        /// <summary>
        ///     N12a - Código de Situação da Operação – Simples Nacional
        /// </summary>
        [XmlElement(Order = 2)]
        public Csosnicms CSOSN { get; set; }
    }
    #endregion

    #region PIS
    public class PIS
    {
        [XmlElement(ElementName = "PISAliq")]
        public PISAliq PISAliq { get; set; } = new PISAliq();
    }
    public class PISAliq
    {
        [XmlElement]
        public string CST { get; set; }
        [XmlElement]
        public double vBC { get; set; }
        [XmlElement]
        public double pPIS { get; set; }
        [XmlElement]
        public double vPIS { get; set; }
    }
    #endregion

    #region Cofins

    public class Cofins
    {
        [XmlElement]
        public COFINSAliq COFINSAliq { get; set; } = new COFINSAliq();
    }

    public class COFINSAliq
    {
        [XmlElement]
        public string CST { get; set; }
        [XmlElement]
        public double vBC { get; set; }
        [XmlElement]
        public double pCOFINS { get; set; }
        [XmlElement]
        public double vCOFINS { get; set; }
    }
    #endregion

    public class Produto
    {
        [XmlElement("cProd")]
        public string cProd { get; set; }
        [Display(Name = "Cod. EAN")]
        [XmlElement]
        public string cEAN { get; set; }
        [Display(Name = "Desc. Produto")]
        [XmlElement]
        public string xProd { get; set; }
        [Display(Name = "NCM")]
        [XmlElement]
        public string NCM { get; set; }
        [Display(Name = "CFOP")]
        [XmlElement]
        public string CFOP { get; set; }
        [Display(Name = "Unidade")]
        [XmlElement]
        public string uCom { get; set; }
        [Display(Name = "Quantidade")]
        [XmlElement]
        public double qCom { get; set; }
        [Display(Name = "Valor Untario")]
        [XmlElement]
        public double vUnCom { get; set; }
        [Display(Name = "Valor Total")]
        [XmlElement("vProd")]
        public double vProd { get; set; }
        [Display(Name = "CEST")]
        [XmlElement]
        public string CEST { get; set; }
    }

    public class Total
    {
        [XmlElement("ICMSTot")]
        public ICMSTotal IcmsTotal { get; set; }
    }

    public class ICMSTotal
    {

        [XmlElement("vBC")]
        [Display(Name = "Base calc. Icms")]
        public double ValorBaseCalculoICMS { get; set; }
        [XmlElement("vICMS")]
        [Display(Name = "Vl Icms")]
        public double ValorICMS { get; set; }
        [XmlElement("vICMSDeson")]
        public double ValorICMSRetido { get; set; }
        [XmlElement("vBCST")]
        [Display(Name = "Base calc. subst. Icms ")]
        public double ValorBaseCalculoCst { get; set; }
        [XmlElement("vST")]
        [Display(Name = "Vl Icms Substituição")]
        public double ValorIcmsSub { get; set; }
        [XmlElement("vIPI")]
        [Display(Name = "Vl Frete")]
        public double ValorIPI { get; set; }
        [XmlElement("vFrete")]
        [Display(Name = "Vl Frete")]
        public double ValorFrete { get; set; }
        [XmlElement("vSeg")]
        [Display(Name = "Vl Seguro")]
        public double ValorSeguro { get; set; }
        [XmlElement("vOutro")]
        [Display(Name = "Outras Despesas")]
        public double ValorOutros { get; set; }
        [XmlElement("vProd")]
        [Display(Name = "Vl Total Produtos")]
        public double ValorTotalProduto { get; set; }
        [XmlElement("vDesc")]
        [Display(Name = "Vl Desconto")]
        public double ValorDesconto { get; set; }
        [XmlElement("vNF")]
        [Display(Name = "Vl Total Nota")]
        public double ValorTotal { get; set; }
        [XmlElement("Vl Total Tribut")]
        public double ValorTotalTributacao { get; set; }
    }
    #endregion
}
