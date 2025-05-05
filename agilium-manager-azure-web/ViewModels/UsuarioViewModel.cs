using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.ViewModels
{
    public class UsuarioRegistro
    {
        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("Nome Completo")]
        public string Nome { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [DisplayName("CPF")]
      
        public string Cpf { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
        [DisplayName("E-mail")]
        public string Email { get; set; }

        [Required(ErrorMessage = "O campo {0} é obrigatório")]
        [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
        public string Senha { get; set; }

        [DisplayName("Confirme sua senha")]
        [Compare("Senha", ErrorMessage = "As senhas não conferem.")]
        public string SenhaConfirmacao { get; set; }
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

    public class UsuarioRespostaLogin
    {
        public string AccessToken { get; set; }
        public string RefreshToken { get; set; }
        public double ExpiresIn { get; set; }
        public UsuarioToken UsuarioToken { get; set; }
        public ResponseResult ResponseResult { get; set; }
    }

    public class RespPadrao
    {
        public object objeto { get; set; }
        public ResponseResult ResponseResult { get; set; }
    }

    public class UsuarioToken
    {
        public string Id { get; set; }
        public string Email { get; set; }
        public IEnumerable<UsuarioClaim> Claims { get; set; }
    }

    public class UsuarioClaim
    {
        public string Value { get; set; }
        public string Type { get; set; }
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
            get { return ativo == "1"; }
            set { ativo = value ? "1" : "2"; }
        }
        public string idUserAspNet { get; set; }
        [Display(Name = "Nome simplificado")]
        public string Usuario { get; set; }

        public string Foto { get; set; }
        [Display(Name = "Perfil")]
        public string PerfilDescricao { get; set; }
        [Display(Name = "Perfil")]
        public string idperfilManager { get; set; }
    }

    public class UserClaimViewModel 
    {
        public string id { get; set; }
        [Display(Name = "Descrição")]
        public string claimType { get; set; }
    }

    public class ListaClaims
    {
        public List<UserClaimViewModel> Claims { get; set; }
    }

    public class ListaUsuarioViewModel
    { 
        public IEnumerable<UserFull> Usuarios { get; set; }
        public string Filtro { get; set; }
    }

    public class AssociarUsuarioClaims
    {
        public string id { get; set; }
        public string idUserAspNet { get; set; }
        [Display(Name = "Nome do Usuário")]
        public string Nome { get; set; }
        [Display(Name = "Claims Disponíveis")]
        public string claimSelecionada { get; set; }
        public List<ClaimSelecionada> Claims { get; set; } = new List<ClaimSelecionada>();
        //public List<string> Acoes { get; set; } = new List<string>();
        public List<AcoesClaims> AcoesClaims { get; set; } = new List<AcoesClaims>();
    }

    public  class AcoesClaims
    {
        public string Acao { get; set; }
        public bool Selecao { get; set; }
    }

    public class AcoesSelecionadas
    {
        public string Claim { get; set; }
        public List<AcoesClaims> AcoesClaims { get; set; } = new List<AcoesClaims>();
    }

    public class ClaimSelecionada
    {
        public string claim { get; set; }
        public List<string> ClaimValue { get; set; } = new List<string>();

        public ClaimSelecionada()
        {

        }

        public ClaimSelecionada(string claim, List<string> claimValue)
        {
            this.claim = claim;
            ClaimValue = claimValue;
        }

        public void AdicionarClaimType(string type) => ClaimValue.Add(type);

    }

    public class ClaimsPorUsuarioViewModel 
    {

        public List<ClaimSelecionada> ClaimsSelecionadas { get; set; } = new List<ClaimSelecionada>();
        public string id { get; set; }
        public string idUserAspNet { get; set; }
        [Display(Name = "Nome do Usuário")]
        public string Nome { get; set; }
    }

    public class DuplicarUsuarioClaimViewModel
    {
        public string id { get; set; }
        public string idUserAspNet { get; set; }
        [Display(Name = "Nome do Usuário Selecionado")]
        public string Nome { get; set; }
        [Display(Name = "Usuario base para clone")]
        public string idUserSelecionado { get; set; }
        public string NomeUsuarioBase { get; set; }
        public List<ClaimSelecionada> ClaimsSelecionadas { get; set; } = new List<ClaimSelecionada>();
        public List<ClaimSelecionada> ClaimsAdicionadas { get; set; } = new List<ClaimSelecionada>();
        public List<AcoesClaims> AcoesClaims { get; set; } = new List<AcoesClaims>();
        [Display(Name = "Claim Selecionada")]
        public string claimSelecionada { get; set; }
    }

    public class DuplicarUsuarioRetornoViewModel
    {
        public string idOrigem { get; set; }
        public string idDestino { get; set; }
    }

    public class UsuarioClaimsViewModel
    {
        public UserFull Usuario { get; set; }
        public List<ClaimSelecionada> ClaimSelecionadas { get; set; } = new List<ClaimSelecionada>();
    }

    public class ClaimEditaAcaoIndividualPorUsuario
    {
        public string IdUserAspNet { get; set; }
        public string ClaimType { get; set; }
        public string ClaimValue { get; set; }
    }

    public class ClaimAcoesUsuario
    {
        public string IdUserAspNet { get; set; }
        public string ClaimType { get; set; }
        public List<string> ClaimValue { get; set; } = new List<string>();
                
    }

    public class UsuarioFotoViewModel
    {
        public string id { get; set; }
        public string idAspNetUser { get; set; }
        public string Foto { get; set; }
        public DateTime DataCadastro { get; set; } = new DateTime();
        public IFormFile ImagemUpLoad { get; set; }
        public string ImagemConvertida { get; set; }
        public string Ativo { get; set; }
        public string NomeArquivo { get; set; }
        public string NomeArquivoExtensao { get; set; }
    }

    public class UsuarioFotoSalvar
    {
        public string id { get; set; }
        public string idAspNetUser { get; set; }
        public string imagem { get; set; }
    }
}

