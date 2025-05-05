using System.ComponentModel.DataAnnotations;
using System;
using agilium.api.business.Enums;
using System.Collections.Generic;
using agilium.api.pdv.ViewModels.EnderecoViewModel;
using agilium.api.pdv.ViewModels.ContatoViewModel;

namespace agilium.api.pdv.ViewModels.ClienteViewModel
{
    public class ClienteTesteViewModel
    {
        [Key]
        public long Id { get; set; }
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Nome { get; set; }
        [Display(Name = "Tipo Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoPessoa? TipoPessoa { get; set; }
        public long? IDENDERECO { get; set; }
        [Display(Name = "Endereco")]
        public EnderecoIndexViewModel Endereco { get; set; } = new EnderecoIndexViewModel();
        public long? IDENDERECOCOB { get; set; }
        [Display(Name = "Endereco Cobrança")] public EnderecoIndexViewModel EnderecoCobranca { get; set; } = new EnderecoIndexViewModel();
        public long? IDENDERECOFAT { get; set; }
        [Display(Name = "Endereco Faturamento")] public EnderecoIndexViewModel EnderecoFaturamento { get; set; } = new EnderecoIndexViewModel();
        public long? IDENDERECONTREGA { get; set; }
        [Display(Name = "Endereco Entrega")] public EnderecoIndexViewModel EnderecoEntrega { get; set; } = new EnderecoIndexViewModel();
        public DateTime? DataCadastro { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EAtivo? Situacao { get; set; }
        [Display(Name = "Publicação Email")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ESimNao? PublicaEmail { get; set; }
        [Display(Name = "Publicação SMS")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ESimNao? PublicaSMS { get; set; }
        public List<ClienteContatoViewModel> Contatos { get; set; } = new List<ClienteContatoViewModel>();

    }
    public class ClienteViewModel
    {
        [Key]
        public long Id { get; set; }
        [Display(Name = "Codigo")]
        public string Codigo { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Nome { get; set; }
        [Display(Name = "Tipo Pessoa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ETipoPessoa? TipoPessoa { get; set; }
        public long? IDENDERECO { get; set; }
        [Display(Name = "Endereco")]
        public EnderecoIndexViewModel Endereco { get; set; } = new EnderecoIndexViewModel();
        public long? IDENDERECOCOB { get; set; }
        [Display(Name = "Endereco Cobrança")] public EnderecoIndexViewModel EnderecoCobranca { get; set; } = new EnderecoIndexViewModel();
        public long? IDENDERECOFAT { get; set; }
        [Display(Name = "Endereco Faturamento")] public EnderecoIndexViewModel EnderecoFaturamento { get; set; } = new EnderecoIndexViewModel();
        public long? IDENDERECONTREGA { get; set; }
        [Display(Name = "Endereco Entrega")] public EnderecoIndexViewModel EnderecoEntrega { get; set; } = new EnderecoIndexViewModel();
        public DateTime? DataCadastro { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EAtivo? Situacao { get; set; }
        [Display(Name = "Publicação Email")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ESimNao? PublicaEmail { get; set; }
        [Display(Name = "Publicação SMS")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public ESimNao? PublicaSMS { get; set; }
        public List<ClienteContatoViewModel> Contatos { get; set; } = new List<ClienteContatoViewModel>();
        public ClientePFViewModel ClientePessoaFisica { get; set; }
        public ClientePJViewModel ClientePessoaJuridica { get; set; }
    }

    public class ClienteContatoViewModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDCONTATO { get; set; }
        public ContatoIndexViewModel Contato { get; set; } = new ContatoIndexViewModel();
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDCLIENTE { get; set; }
    }

    public class ClientePFViewModel
    {
        public long Id { get; set; }
        [Display(Name = "CPF")]
        public string CPF { get; set; }
        [Display(Name = "Numero Documento")]
        public string NumeroDocumento { get; set; }
        public DateTime? DataNascimento { get; set; }
    }

    public class ClientePJViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Razão Social")]
        public string RazaoSocial { get; set; }
        [Display(Name = "CNPJ")]
        public string Cnpj { get; set; }
        [Display(Name = "Inscrição Estadual")]
        public string InscricaoEstadual { get; set; }
    }

    public class ClientePrecoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDCLIENTE { get; set; }
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDPRODUTO { get; set; }
        [Display(Name = "Diferença")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int Diferenca { get; set; }
        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double Valor { get; set; }
        [Display(Name = "Data Hora")]
        public DateTime Datahora { get; set; }
        [Display(Name = "Tipo de Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int TipoValor { get; set; }
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
    }
}
