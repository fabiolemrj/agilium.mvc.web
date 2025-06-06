﻿using agilium.api.business.Enums;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using agilum.mvc.web.Extensions;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.UnidadeViewModel;
using agilum.mvc.web.ViewModels.Impostos;
using Microsoft.AspNetCore.Http;

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
        [Display(Name = "Preço")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Moeda]
        public double? Preco { get; set; }
        [Display(Name = "Quantidade Minima")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Moeda]
        public double? QuantMinima { get; set; }
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
        [Display(Name = "Valor Ultima Compra")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorUltimaCompra { get; set; }
        [Display(Name = "Valor Custo Médio")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ValorCustoMedio { get; set; }
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? PCIBPTFED { get; set; }
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? PCIBPTEST { get; set; }
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? PCIBPTMUN { get; set; }
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? PCIBPTIMP { get; set; }
        [Display(Name = "CFOP da Venda")]
        public int? CFOPVenda { get; set; }
        [Display(Name = "Origem do Produto")]
        public EOrigemProduto? OrigemProduto { get; set; }
        [Display(Name = "Codigo Situação Oper. Simples Nacional (CSOSN) ICMS ")]
        [StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string DSICMS_CST { get; set; }
        [Display(Name = "ALiquota ICMS (%)")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? AliquotaICMS { get; set; }
        [Display(Name = "Redução Base Calculo (%)")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ReducaoBaseCalculoICMS { get; set; }
        [Display(Name = "ALiquota ICMS ST (%)")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? AliquotaICMS_ST { get; set; }
        [Display(Name = "Margem valor Agregado (MVA) ICMS ST (%)")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? AliquotaMargemValorAgregadoICMS_ST { get; set; }
        [Display(Name = "Redução da Base de calculo (%)")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? ReducaoBaseCalculoICMS_ST { get; set; }
        [Display(Name = "Codigo de situação tributaria (CST) IPI")]
        //[StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoSituacaoTributariaIPI { get; set; }
        [Display(Name = "Aliquota IPI (%)")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? AliquotaIPI { get; set; }
        [Display(Name = "Codigo Situação Tributaria (CST) PIS")]
        //[StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoSituacaoTributariaPIS { get; set; }
        [Display(Name = "Aliquota PIS (%)")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? AliquotaPIS { get; set; }
        [Display(Name = "Codigo Situação Tributaria (CST) COFINS")]
        //[StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CodigoSituacaoTributariaCofins { get; set; }
        [Display(Name = "Aliquota COFINS (%)")]
        //[StringLength(5, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double? AliquotaCofins { get; set; }
        [Display(Name = "Quando a Venda for cancelada?")]
        public int? STESTOQUE { get; set; }
        [Display(Name = "Utiliza Balança?")]
        public ESimNao? UtilizaBalanca { get; set; }
        public int? FLG_IFOOD { get; set; }
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
