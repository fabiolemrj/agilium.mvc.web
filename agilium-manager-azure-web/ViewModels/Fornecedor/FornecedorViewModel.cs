using agilium.webapp.manager.mvc.Enums;
using agilium.webapp.manager.mvc.ViewModels.Contato;
using agilium.webapp.manager.mvc.ViewModels.Endereco;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.Fornecedor
{
    public class FornecedorViewModel
    {
        [Key]
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
        public string RazaoSocial { get; set; }
        [Display(Name = "Nome Fantasia")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NomeFantasia { get; set; }
        [Display(Name = "Tipo Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoPessoa TipoPessoa { get; set; }
        [Display(Name = "CPF/CNPJ")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CpfCnpj { get; set; }
        [Display(Name = "CNPJ")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Cnpj { get; set; }
        [Display(Name = "CPF")]
        [StringLength(15, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Cpf { get; set; }
        [Display(Name = "Inscrição Estadual/Municipal")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string InscricaoEstdualMunicipal { get; set; }
        [Display(Name = "Tipo Fiscal")]
        public ETipoFiscal TipoFiscal { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EAtivo Situacao { get; set; }
        [Display(Name = "Endereco")]
        public EnderecoIndexViewModel Endereco { get; set; } = new EnderecoIndexViewModel();
        public List<ContatoFornecedorViewModel> Contatos { get; set; } = new List<ContatoFornecedorViewModel>();
    }
}
