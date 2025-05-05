using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class UsuarioFotoValidation : AbstractValidator<Usuario>
    {
        public UsuarioFotoValidation()
        {
            RuleFor(x => x.idUserAspNet)
            .NotEmpty().WithMessage("O codigo do usuario não fornecido");
            RuleFor(x => x.foto)
                .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
        }
    }
}
