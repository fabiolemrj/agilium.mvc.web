using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class UsuarioValidation: AbstractValidator<Usuario>
    {
        public UsuarioValidation()
        {
            RuleFor(x => x.Id)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            RuleFor(x => x.idUserAspNet)
             .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            RuleFor(c => c.nome)
            .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido")
            .Length(2, 50).WithMessage("O campo {PropertyName} precisa ter entre {MinLength} e {MaxLength} caracteres");
        }
    }
}
