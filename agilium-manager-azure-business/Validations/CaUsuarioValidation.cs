using agilium.api.business.Models;
using FluentValidation;

namespace agilium.api.business.Models.Validations
{
    /// <summary>
    /// Validacao para CaUsuarioIdentity (IdentityUser&lt;long&gt; para login).
    /// Valida as propriedades do Identity e da entidade Usuario vinculada.
    /// </summary>
    public class CaUsuarioValidation : AbstractValidator<CaUsuarioIdentity>
    {
        public CaUsuarioValidation()
        {
            // Validacao das propriedades do Identity
            RuleFor(c => c.UserName)
                .NotEmpty().WithMessage("O campo Usuario precisa ser fornecido")
                .Length(3, 20).WithMessage("O campo Usuario precisa ter entre {MinLength} e {MaxLength} caracteres");

            RuleFor(c => c.Email)
                .NotEmpty().WithMessage("O campo Email precisa ser fornecido")
                .EmailAddress().WithMessage("O campo Email esta em formato invalido");

            // Validacao das propriedades da entidade Usuario vinculada
            When(c => c.Usuario != null, () =>
            {
                RuleFor(c => c.Usuario.nome)
                    .NotEmpty().WithMessage("O campo Nome precisa ser fornecido")
                    .Length(2, 100).WithMessage("O campo Nome precisa ter entre {MinLength} e {MaxLength} caracteres");

                RuleFor(c => c.Usuario.cpf)
                    .MaximumLength(14).WithMessage("O campo CPF pode ter no maximo {MaxLength} caracteres");

                RuleFor(c => c.Usuario.ender)
                    .MaximumLength(100).WithMessage("O campo Endereco pode ter no maximo {MaxLength} caracteres");

                RuleFor(c => c.Usuario.bairro)
                    .MaximumLength(40).WithMessage("O campo Bairro pode ter no maximo {MaxLength} caracteres");

                RuleFor(c => c.Usuario.cidade)
                    .MaximumLength(40).WithMessage("O campo Cidade pode ter no maximo {MaxLength} caracteres");

                RuleFor(c => c.Usuario.uf)
                    .MaximumLength(2).WithMessage("O campo UF pode ter no maximo {MaxLength} caracteres");

                RuleFor(c => c.Usuario.cep)
                    .MaximumLength(9).WithMessage("O campo CEP pode ter no maximo {MaxLength} caracteres");

                RuleFor(c => c.Usuario.tel1)
                    .MaximumLength(20).WithMessage("O campo Telefone pode ter no maximo {MaxLength} caracteres");

                RuleFor(c => c.Usuario.cel)
                    .MaximumLength(20).WithMessage("O campo Celular pode ter no maximo {MaxLength} caracteres");
            });
        }
    }
}
