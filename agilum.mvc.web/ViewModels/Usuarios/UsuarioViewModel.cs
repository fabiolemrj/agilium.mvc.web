using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using System.ComponentModel;

namespace agilum.mvc.web.ViewModels.Usuarios
{
    public class UsuarioViewModel
    {

        public long id { get; set; }

        [StringLength(100)]
        [Display(Name = "Nome")]
        public string nome { get; set; }

        // [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]
        [Display(Name = "CPF")]
        public string cpf { get; set; }
        [Display(Name = "Endereço")]
        // [StringLength(60, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 5)]
        public string ender { get; set; }
        [Display(Name = "Numero")]
        public int? num { get; set; }
        [Display(Name = "Complemento")]
        //  [StringLength(35, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string compl { get; set; }
        [Display(Name = "Bairro")]
        //   [StringLength(35, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]

        public string bairro { get; set; }
        [Display(Name = "Cep")]
        //   [StringLength(10)]
        public string cep { get; set; }
        [Display(Name = "Cidade")]
        //   [StringLength(40, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string cidade { get; set; }
        [Display(Name = "UF")]
        //  [StringLength(2, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string uf { get; set; }
        [Display(Name = "Telefone 1")]
        //  [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]
        public string tel1 { get; set; }
        [Display(Name = "Celular 1")]
        //   [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]

        public string cel { get; set; }
        [Display(Name = "Data Nascimento")]

        public DateTime? dtnasc { get; set; }
        [Display(Name = "Usuario")]
        //    [StringLength(20, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 5)]

        public string usuario { get; set; }
        [Display(Name = "Email")]
        //   [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 5)]
        //   [EmailAddress]
        public string email { get; set; }
        [Display(Name = "Telefone 2")]
        //  [StringLength(14, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 1)]

        public string tel2 { get; set; }
        [Display(Name = "Ativo")]
        public string ativo { get; set; }
        public string idUserAspNet { get; set; }

    }

    public class UsuarioPadrao
    {
        public string id { get; set; }
        public string nome { get; set; }
        public string cpf { get; set; }
        public string ender { get; set; }
        public string num { get; set; }
        public string compl { get; set; }
        public string bairro { get; set; }
        public string cep { get; set; }
        public string cidade { get; set; }
        public string uf { get; set; }
        public string tel1 { get; set; }
        public string cel { get; set; }
        public string dtnasc { get; set; }
        public string email { get; set; }
        public string tel2 { get; set; }
        public string ativo { get; set; }
        public string idUserAspNet { get; set; }
        public string usuario { get; set; }
        public string Foto { get; set; }
        public string idperfil { get; set; }
        public string idperfilManager { get; set; }
        public string PerfilDescricao { get; set; }
    }
    public class UserFull
    {
        public string id { get; set; }
        [Display(Name = "Nome")]
        public string nome { get; set; }
        [Display(Name = "CPF")]
        [MaxLength(14, ErrorMessage = "O campo {0} pode ter até {1} caracteres")]
        public string cpf { get; set; }
        [Display(Name = "Logradouro")]
        [MaxLength(100, ErrorMessage = "O campo {0} pode ter até {1} caracteres")]
        public string ender { get; set; }
        [Display(Name = "Numero")]
        public string num { get; set; }
        [MaxLength(25, ErrorMessage = "O campo {0} pode ter até {1} caracteres")]
        [Display(Name = "Complemento")]
        public string compl { get; set; }
        [Display(Name = "Bairro")]
        [MaxLength(40, ErrorMessage = "O campo {0} pode ter até {1} caracteres")]
        public string bairro { get; set; }
        [Display(Name = "Cep")]
        public string cep { get; set; }
        [Display(Name = "Cidade")]
        [MaxLength(40, ErrorMessage = "O campo {0} pode ter até {1} caracteres")]
        public string cidade { get; set; }
        [Display(Name = "Estado")]
        [MaxLength(2, ErrorMessage = "O campo {0} pode ter até {1} caracteres")]
        public string uf { get; set; }
        [Display(Name = "Telefone 1")]
        [MaxLength(20, ErrorMessage = "O campo {0} pode ter até {1} caracteres")]
        public string tel1 { get; set; }
        [MaxLength(20, ErrorMessage = "O campo {0} pode ter até {1} caracteres")]
        [Display(Name = "Celular")]
        public string cel { get; set; }
        [Display(Name = "Data de Nascimento")]
        public string dtnasc { get; set; }

        [Display(Name = "E-mail")]
        public string email { get; set; }
        [Display(Name = "Telefone 2")]
        [MaxLength(20, ErrorMessage = "O campo {0} pode ter até {1} caracteres")]
        public string tel2 { get; set; }

        public string ativo { get; set; }
        [Display(Name = "Ativo?")]
        public bool AtivoBool
        {
            get { return ativo == "S"; }
            set { ativo = value ? "S" : "N"; }
        }
        public string idUserAspNet { get; set; }
        [Display(Name = "Nome Usuario")]
        public string Usuario { get; set; }

        public string Foto { get; set; }
        [Display(Name = "Perfil")]
        public string PerfilDescricao { get; set; }
        [Display(Name = "Perfil")]
        public string idperfilManager { get; set; }
        public bool UsuarioPossuiAcessoWeb { get; set; } = false;
    }

    public class UsuarioLogin
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Password { get; set; }
    }


    public class UserChangePassword
    {
        [Required]
        [DataType(DataType.Password)]
        [Display(Name = "Senha Atual")]
        public string OldPassword { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Nova Senha")]
        public string NewPassword { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Nova Senha")]
        [Compare("NewPassword", ErrorMessage = "A nova senha e a senha de confirmação não correspondem")]
        public string ConfirmPassword { get; set; }

        [DataType(DataType.EmailAddress)]
        [Required]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class UserForgotPassword
    {
        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }
    }

    public class UserResetPassword
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        [Display(Name = "Senha")]
        [StringLength(100, ErrorMessage = "O {0} deve ter pelo menos {2} e no máximo {1} caracteres.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirmar Senha")]
        [Compare("Password", ErrorMessage = "A senha e a senha de confirmação não correspondem.")]
        public string ConfirmPassword { get; set; }

        public string Code { get; set; }
    }

    public class NovoUsuarioModel
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Nome Completo")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("CPF")]
        public string CPF { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        [DisplayName("Senha")]
        public string Password { get; set; } = "Abc@123";

        [DisplayName("Confirme sua senha")]
        [Compare("Password", ErrorMessage = "As senhas não conferem.")]
        public string ConfirmPassword { get; set; } = "Abc@123";

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Nome Simplificado")]
        [StringLength(20, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
        public string Usuario { get; set; }
    }

    public class CaPerfilManagerViewModel
    {
        public int IdPerfil { get; set; }
        public string Descricao { get; set; }
    }

}
