
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using agilum.mvc.web.ViewModels.Produtos;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Estoque;
using agilum.mvc.web.Enums;

namespace agilum.mvc.web.ViewModels.Perda
{
    public class PerdaViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDEMPRESA { get; set; }
        public string EmpresaNome { get; set; }
        [Display(Name = "Estoque")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDESTOQUE { get; set; }
        public string EstoqueNome { get; set; }
        [Display(Name = "Estoque Historico")]
        public Int64? IDESTOQUEHST { get; set; }
        public string EstoqueHistoricoNome { get; set; }
        public string ProdutoNome { get; set; }
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDPRODUTO { get; set; }
        [Display(Name = "Produto")]
        public Int64? IDUSUARIO { get; set; }
        public string UsuarioNome { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        public DateTime? DataHora { get; set; }
        [Display(Name = "Tipo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoPerda? Tipo { get; set; }
        [Display(Name = "Movimento")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoMovimentoPerda? Movimento { get; set; }
        [Display(Name = "Quantidade")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public double? Quantidade { get; set; }
        public double? ValorCustoMedio { get; set; }
        [Display(Name = "Observação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Observacao { get; set; }
        public List<ProdutoViewModel> Produtos { get; set; } = new List<ProdutoViewModel>();
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
        public List<EstoqueViewModel> Estoques { get; set; } = new List<EstoqueViewModel>();
    }
}
