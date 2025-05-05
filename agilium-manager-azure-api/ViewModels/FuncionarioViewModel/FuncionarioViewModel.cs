using agilium.api.business.Models;
using agilium.api.manager.ViewModels.EnderecoViewModel;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.FuncionarioViewModel
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
        public int? Situacao { get; set; }
        [Display(Name = "CPF")]
        public string CPF { get;  set; }
        [Display(Name = "Documento")]
        public string Documento { get; set; }
        [Display(Name = "Data Admissão")]
        public DateTime? DataAdmissao { get; set; }
        [Display(Name = "Data Demissão")]
        public DateTime? DataDemissao { get; set; }
        public string DSRFID { get; set; }
        [Display(Name = "Noturno")]
        public int? Noturno { get; set; }
        [Display(Name = "Endereco")]
        public EnderecoIndexViewModel Endereco { get; set; } = new EnderecoIndexViewModel();
        public List<EmpresaViewModel.EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel.EmpresaViewModel> { };
        [Display(Name = "Usuario")]
        public UsuarioPadrao Usuario { get; set; } = new UsuarioPadrao();
        public string Perfil { get; set; }
        public List<UsuarioPadrao> Usuarios { get; set; } = new List<UsuarioPadrao>();
        public int? TipoFuncionario { get; set; } 
    }
}
