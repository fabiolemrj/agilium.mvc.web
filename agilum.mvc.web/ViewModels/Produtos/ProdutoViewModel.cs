using agilium.api.business.Enums;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.UnidadeViewModel;
using agilum.mvc.web.ViewModels.Impostos;
using Microsoft.AspNetCore.Http;
using System.Globalization;

namespace agilum.mvc.web.ViewModels.Produtos
{
    public class ProdutoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public Int64? idEmpresa { get; set; }
        [Display(Name = "Grupo")]
        public Int64? IDGRUPO { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Nome { get; set; }
        [Display(Name = "Categoria")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ECategoriaProduto Categoria { get; set; }
        [Display(Name = "Tipo Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoProduto? Tipo { get; set; }
        [Display(Name = "Unidade de Compra")]
        //[StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string UnidadeCompra { get; set; }
        [Display(Name = "Unidade de Venda")]
        //[StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string UnidadeVenda { get; set; }
        [Display(Name = "Relação compra/venda")]
        //[Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int? RelacaoCompraVenda { get; set; }
          
        [Display(Name = "Codigo Sefaz")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoSefaz { get; set; }
        [Display(Name = "Codigo ANP")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoANP { get; set; }
        [Display(Name = "Codigo NCM")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoNCM { get; set; }
        [Display(Name = "Codigo Cest")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoCest { get; set; }
        [Display(Name = "Codigo Serviço")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoServ { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EAtivo? Situacao { get; set; }
        [Display(Name = "CFOP da Venda")]
        public int? CFOPVenda { get; set; }
        [Display(Name = "Origem do Produto")]
        public EOrigemProduto? OrigemProduto { get; set; }
        [Display(Name = "Codigo Situação Oper. Simples Nacional (CSOSN) ICMS ")]
        [StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string DSICMS_CST { get; set; }
        public string CodigoSituacaoTributariaIPI { get; set; }
        [Display(Name = "Aliquota IPI (%)")]
        public string CodigoSituacaoTributariaPIS { get; set; }
        [Display(Name = "Aliquota PIS (%)")]
        public string CodigoSituacaoTributariaCofins { get; set; }
        [Display(Name = "Quando a Venda for cancelada?")]
        public int? STESTOQUE { get; set; }
        [Display(Name = "Utiliza Balança?")]
        public ESimNao? UtilizaBalanca { get; set; }
        public int? FLG_IFOOD { get; set; }
        [Display(Name = "Exportar para Pedido?")]
        public ESimNao? ExportarPedido { get; set; }
        [Display(Name = "Marca")]
        public long? IDMARCA { get; set; }
        [Display(Name = "Departamento")]
        public long? IDDEP { get; set; }
        [Display(Name = "Sub Grupo")]
        public long? IDSUBGRUPO { get; set; }
        [Display(Name = "Volume")]
        //[StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Volume { get; set; }
        public string DescricaoUnidadeCompra { get; set; }
        public string DescricaoUnidadeVenda { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
        public List<CfopViewModel> Cfops { get; set; } = new List<CfopViewModel>();
        public List<CstViewModel> Csts { get; set; } = new List<CstViewModel>();
        public List<CestViewModel> Cests { get; set; } = new List<CestViewModel>();
        public List<GrupoProdutoViewModel> Grupos { get; set; } = new List<GrupoProdutoViewModel>();
        public List<SubGrupoViewModel> SubGrupos { get; set; } = new List<SubGrupoViewModel>();
        public List<ProdutoDepartamentoViewModel> Departamentos { get; set; } = new List<ProdutoDepartamentoViewModel>();
        public List<ProdutoMarcaViewModel> Marcas { get; set; } = new List<ProdutoMarcaViewModel>();
        public List<UnidadeIndexViewModel> Unidades { get; set; } = new List<UnidadeIndexViewModel>();
        public List<CsosnViewModel> Csosn { get; set; } = new List<CsosnViewModel>();

        // Propriedades double? sem DataAnnotation
        public double? QuantMinima { get; set; }
        private string quantMinimaView;
        [Display(Name = "Quantidade Minima")]
        public string QuantMinimaView
        {
            get => quantMinimaView;
            set
            {
                quantMinimaView = value;
                QuantMinima = ParseDecimal(value);
            }
        }

        public double? Preco { get; set; }
        private string precoView;
        [Display(Name = "Preço")]
        public string PrecoView
        {
            get => precoView;
            set
            {
                precoView = value;
                Preco = ParseDecimal(value);
            }
        }

        public double? ValorUltimaCompra { get; set; }
        private string valorUltimaCompraView;
        [Display(Name = "Valor Última Compra")]
        public string ValorUltimaCompraView
        {
            get => valorUltimaCompraView;
            set
            {
                valorUltimaCompraView = value;
                ValorUltimaCompra = ParseDecimal(value);
            }
        }

        public double? ValorCustoMedio { get; set; }
        private string valorCustoMedioView;
        [Display(Name = "Valor Custo Médio")]
        public string ValorCustoMedioView
        {
            get => valorCustoMedioView;
            set
            {
                valorCustoMedioView = value;
                ValorCustoMedio = ParseDecimal(value);
            }
        }

        public double? PCIBPTFED { get; set; }
        private string pCIBPTFEDView;
        [Display(Name = "% IBPT Federal")]
        public string PCIBPTFEDView
        {
            get => pCIBPTFEDView;
            set
            {
                pCIBPTFEDView = value;
                PCIBPTFED = ParseDecimal(value);
            }
        }

        public double? PCIBPTEST { get; set; }
        private string pCIBPTESTView;
        [Display(Name = "% IBPT Estadual")]
        public string PCIBPTESTView
        {
            get => pCIBPTESTView;
            set
            {
                pCIBPTESTView = value;
                PCIBPTEST = ParseDecimal(value);
            }
        }

        public double? PCIBPTIMP { get; set; }
        private string pCIBPTIMPView;
        [Display(Name = "% IBPT Municipal")]
        public string PCIBPTIMPView
        {
            get => pCIBPTIMPView;
            set
            {
                pCIBPTIMPView = value;
                PCIBPTIMP = ParseDecimal(value);
            }
        }

        public double? AliquotaICMS { get; set; }
        private string aliquotaICMSView;
        [Display(Name = "Aliquota ICMS (%)")]
        public string AliquotaICMSView
        {
            get => aliquotaICMSView;
            set
            {
                aliquotaICMSView = value;
                AliquotaICMS = ParseDecimal(value);
            }
        }

        public double? ReducaoBaseCalculoICMS { get; set; }
        private string reducaoBaseCalculoICMSView;
        [Display(Name = "Redução Base Cálculo ICMS (%)")]
        public string ReducaoBaseCalculoICMSView
        {
            get => reducaoBaseCalculoICMSView;
            set
            {
                reducaoBaseCalculoICMSView = value;
                ReducaoBaseCalculoICMS = ParseDecimal(value);
            }
        }

        public double? AliquotaICMS_ST { get; set; }
        private string aliquotaICMS_STView;
        [Display(Name = "Aliquota ICMS ST (%)")]
        public string AliquotaICMS_STView
        {
            get => aliquotaICMS_STView;
            set
            {
                aliquotaICMS_STView = value;
                AliquotaICMS_ST = ParseDecimal(value);
            }
        }

        public double? AliquotaMargemValorAgregadoICMS_ST { get; set; }
        private string aliquotaMargemValorAgregadoICMS_STView;
        [Display(Name = "Margem Valor Agregado ICMS ST (%)")]
        public string AliquotaMargemValorAgregadoICMS_STView
        {
            get => aliquotaMargemValorAgregadoICMS_STView;
            set
            {
                aliquotaMargemValorAgregadoICMS_STView = value;
                AliquotaMargemValorAgregadoICMS_ST = ParseDecimal(value);
            }
        }

        public double? ReducaoBaseCalculoICMS_ST { get; set; }
        private string reducaoBaseCalculoICMS_STView;
        [Display(Name = "Redução Base Cálculo ICMS ST (%)")]
        public string ReducaoBaseCalculoICMS_STView
        {
            get => reducaoBaseCalculoICMS_STView;
            set
            {
                reducaoBaseCalculoICMS_STView = value;
                ReducaoBaseCalculoICMS_ST = ParseDecimal(value);
            }
        }

        public double? AliquotaIPI { get; set; }
        private string aliquotaIPIView;
        [Display(Name = "Aliquota IPI (%)")]
        public string AliquotaIPIView
        {
            get => aliquotaIPIView;
            set
            {
                aliquotaIPIView = value;
                AliquotaIPI = ParseDecimal(value);
            }
        }

        public double? AliquotaPIS { get; set; }
        private string aliquotaPISView;
        [Display(Name = "Aliquota PIS (%)")]
        public string AliquotaPISView
        {
            get => aliquotaPISView;
            set
            {
                aliquotaPISView = value;
                AliquotaPIS = ParseDecimal(value);
            }
        }

        public double? AliquotaCofins { get; set; }
        private string aliquotaCofinsView;
        [Display(Name = "Aliquota COFINS (%)")]
        public string AliquotaCofinsView
        {
            get => aliquotaCofinsView;
            set
            {
                aliquotaCofinsView = value;
                AliquotaCofins = ParseDecimal(value);
            }
        }

        public double? PCIBPTMUN { get; set; }
        private string pCIBPTMUNView;
        [Display(Name = "Aliquota COFINS (%)")]
        public string PCIBPTMUNView
        {
            get => pCIBPTMUNView;
            set
            {
                pCIBPTMUNView = value;
                PCIBPTMUN = ParseDecimal(value);
            }
        }

        // Método auxiliar para converter string pt-BR para double?
        private double? ParseDecimal(string value)
        {
            if (string.IsNullOrWhiteSpace(value)) return null;

            try
            {
                return double.Parse(value, new CultureInfo("pt-BR"));
            }
            catch
            {
                return null;
            }
        }

        private string FormatDecimal(double? value)
        {
            if (!value.HasValue)
                return null; // Se for nulo, não retorna nada

            // Converte o valor numérico para string no formato brasileiro (pt-BR)
            return value.Value.ToString("N2", new CultureInfo("pt-BR"));
        }

        public void PreencherViews()
        {
            QuantMinimaView = FormatDecimal(QuantMinima);
            PrecoView = FormatDecimal(Preco);
            ValorUltimaCompraView = FormatDecimal(ValorUltimaCompra);
            ValorCustoMedioView = FormatDecimal(ValorCustoMedio);
            PCIBPTFEDView = FormatDecimal(PCIBPTFED);
            PCIBPTESTView = FormatDecimal(PCIBPTEST);
            PCIBPTIMPView = FormatDecimal(PCIBPTIMP);
            AliquotaICMSView = FormatDecimal(AliquotaICMS);
            ReducaoBaseCalculoICMSView = FormatDecimal(ReducaoBaseCalculoICMS);
            AliquotaICMS_STView = FormatDecimal(AliquotaICMS_ST);
            AliquotaMargemValorAgregadoICMS_STView = FormatDecimal(AliquotaMargemValorAgregadoICMS_ST);
            ReducaoBaseCalculoICMS_STView = FormatDecimal(ReducaoBaseCalculoICMS_ST);
            AliquotaIPIView = FormatDecimal(AliquotaIPI);
            AliquotaPISView = FormatDecimal(AliquotaPIS);
            AliquotaCofinsView = FormatDecimal(AliquotaCofins);
        }

    }

    public class ListaProdutos
    {
        public string Id { get; set; }
        public string Nome { get; set; }
    }
    public class ListasAuxiliaresProdutoViewModel
    {
        public List<GrupoProdutoViewModel> Grupos { get; set; } = new List<GrupoProdutoViewModel>();
        public List<SubGrupoViewModel> SubGrupos { get; set; } = new List<SubGrupoViewModel>();
        public List<ProdutoDepartamentoViewModel> Departamentos { get; set; } = new List<ProdutoDepartamentoViewModel>();
        public List<ProdutoMarcaViewModel> Marcas { get; set; } = new List<ProdutoMarcaViewModel>();
    }

    public class ProdutoDepartamentoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public long? idEmpresa { get; set; }
        [Display(Name = "Codigo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Nome { get; set; }
        [Display(Name = "Situação")]
        public EAtivo? situacao { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel> { };
    }

    public class ProdutoMarcaViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public long? idEmpresa { get; set; }
        [Display(Name = "Codigo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(30, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Nome { get; set; }
        [Display(Name = "Situação")]
        public EAtivo? situacao { get; set; }
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel> { };
    }

    public class ProdutoCodigoBarraViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        public Int64? IDPRODUTO { get; set; }
        [Display(Name = "Codigo de Barra")]
        public string CDBARRA { get; set; }
        public virtual List<ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel>();
    }

    public class ProdutoFotoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        public Int64? idProduto { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Descricao { get; set; }
        [Display(Name = "Data")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime? Data { get; set; }
        [Display(Name = "Foto")]
        public IFormFile Foto { get; set; }
        public byte[] FotoConvertida { get; set; }
    }

    public class ProdutoPrecoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        public long? idProduto { get; set; }
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
        [Display(Name = "Preço Atual")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Moeda]
        public decimal? Preco { get; set; }
        [Display(Name = "Preço Anterior")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Moeda]
        public decimal? PrecoAnterior { get; set; }
        [Display(Name = "Data")]
        public DateTime? DataPreco { get; set; }
    }
}
