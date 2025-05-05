using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilium.api.manager.ViewModels.ContatoViewModel;
using agilium.api.manager.ViewModels.EnderecoViewModel;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.FornecedorViewModel
{
    public class FornecedorViewModel
    {
        public long Id { get; set; }
        [Display(Name = "ID Endereco")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDENDERECO { get; set; }
        [Display(Name = "Codigo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Razao Social")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string RazaoSocial { get;set; }
        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NomeFantasia { get; set; }
        [Display(Name = "Tipo Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoPessoa TipoPessoa { get; set; }
        [Display(Name = "CPF/CNPJ")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CpfCnpj { get; set; }
        [Display(Name = "Inscrição Estadual/Municipal")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string InscricaoEstdualMunicipal { get; set; }
        [Display(Name = "Tipo Fiscal")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoFiscal TipoFiscal{ get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int Situacao { get; set; }
        [Display(Name = "Endereco")]
        public EnderecoIndexViewModel Endereco { get; set; }
        public List<FornecedorContatoViewModel> Contatos { get; set; } = new List<FornecedorContatoViewModel>();
    }

    public class FornecedorContatoViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDCONTATO { get; set; }
        public ContatoIndexViewModel Contato { get; set; } = new ContatoIndexViewModel();
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDFORN { get; set; }
    }
}

