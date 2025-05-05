using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.Extensions;
using agilium.webapp.manager.mvc.ViewModels.Empresa;
using agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.SiteMercadoViewModel
{
    public class ProdutoSiteMercadoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long? IDEMPRESA { get; set; }
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long? IDPRODUTO { get; set; }
        public string ProdutoPdv { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Quantidade Atacado")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Moeda]
        public double? QuantidadeAtacado { get; set; }
        [Display(Name = "Valor Promoção")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Moeda]
        public double? ValorPromocao { get; set; }
        [Display(Name = "Valor Atacado")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Moeda]
        public double? ValorAtacado { get; set; }
        [Display(Name = "Valor Compra")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        [Moeda]
        public double? ValorCompra { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ESituacaoProdutoSiteMercada? Situacao { get; set; }
        [Display(Name = "Validade Próxima")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EValidadeSiteMercado? Validade { get; set; }
        [Display(Name = "Data/Hora")]
         public DateTime? DataHora { get; set; }
        public List<ProdutoViewModel.ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel.ProdutoViewModel>();
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
    }
}
