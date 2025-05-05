using Microsoft.AspNetCore.Mvc.RazorPages;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.ViewModels
{
    public class UserViewModel
    {
        public class RegisterUserViewModel
        {
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
            public string Password { get; set; }

            [Compare("Password", ErrorMessage = "As senhas não conferem.")]
            public string ConfirmPassword { get; set; }
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            [StringLength(255, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 3)]
            public string Nome { get; set; }
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            [StringLength(15, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 11)]

            public string CPF { get; set; }
            public bool? Conectar { get; set; } = true;
            public string Usuario { get; set; }
        }

        public class LoginUserViewModel
        {
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
            public string Email { get; set; }

            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            [StringLength(100, ErrorMessage = "O campo {0} precisa ter entre {2} e {1} caracteres", MinimumLength = 6)]
            public string Password { get; set; }
        }

        public class LoginRefreshToken
        {
            [EmailAddress(ErrorMessage = "O campo {0} está em formato inválido")]
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            public string Email { get; set; }
        }

        public class RefreshTokenUser
        {
            public string refreshToken { get; set; }
        }

        public class UserTokenViewModel
        {
            public string Id { get; set; }
            public string Email { get; set; }
            public string  Nome { get; set; }
            public IEnumerable<ClaimViewModel> Claims { get; set; }
        }

        public class LoginResponseViewModel
        {
            public string AccessToken { get; set; }
            public double ExpiresIn { get; set; }
            public Guid RefreshToken { get; set; }
            public UserTokenViewModel UserToken { get; set; }
            public ResponseResult ResponseResult { get; set; }
        }

        public class RespPadrao
        {
            public object objeto { get; set; }
            public ResponseResult ResponseResult { get; set; }
        }

        public class ClaimViewModel
        {
            public string Value { get; set; }
            public string Type { get; set; }
        }

        public class ResponseResult
        {
            public ResponseResult()
            {
                Errors = new ResponseErrorMessages();
            }

            public string Title { get; set; }
            public int Status { get; set; }
            public ResponseErrorMessages Errors { get; set; }
        }

        public class ResponseErrorMessages
        {
            public ResponseErrorMessages()
            {
                Mensagens = new List<string>();
            }

            public List<string> Mensagens { get; set; }
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

        public class ClaimModel
        {
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            public string ClaimType { get; set; }
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            public string ClaimValue { get; set; }
        }

        public class UserClaim
        {
            [Required(ErrorMessage = "O campo {0} é obrigatório")]
            public string email { get; set; }
            public List<ClaimModel> claimModels { get; set; } = new List<ClaimModel>();

        }
    }
}
