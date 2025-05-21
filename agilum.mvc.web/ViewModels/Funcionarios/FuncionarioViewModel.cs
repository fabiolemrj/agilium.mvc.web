using agilium.api.business.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using agilum.mvc.web.ViewModels.Endereco;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Usuarios;
using agilum.mvc.web.Enums;

namespace agilum.mvc.web.ViewModels.Funcionarios
{
    public class FuncionarioViewModel
    {
        public long Id { get; set; }

        public long? IDUSUARIO { get; set; }
        [Display(Name = "Id Endereco")]
        public long? IDENDERECO { get; set; }
        [Display(Name = "Empresa")]
        public long? IDEMPRESA { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Nome")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Nome { get; set; }
        [Display(Name = "Turno")]
        public int? Turno { get; set; }
        [Display(Name = "Situação")]
        public EAtivo? Situacao { get; set; }
        [Display(Name = "CPF")]
        public string CPF { get; set; }
        [Display(Name = "Documento")]
        public string Documento { get; set; }
        [Display(Name = "Data Admissão")]
        public DateTime? DataAdmissao { get; set; }
        [Display(Name = "Data Demissão")]
        public DateTime? DataDemissao { get; set; }
        [Display(Name = "Codigo RFId")]
        public string DSRFID { get; set; }
        [Display(Name = "É Noturno")]
        public int? Noturno { get; set; }
        [Display(Name = "Endereco")]
        public EnderecoIndexViewModel Endereco { get; set; } = new EnderecoIndexViewModel();
        public List<EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
        [Display(Name = "Usuario")]
        public UserFull Usuario { get; set; } = new UserFull();
        public string Perfil { get; set; }
        public ETipoFuncionario? TipoFuncionario { get; set; } = ETipoFuncionario.Padrao;
        public List<UserFull> Usuarios { get; set; } = new List<UserFull>();
    }
}
