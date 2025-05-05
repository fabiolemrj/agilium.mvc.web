using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class ContatoValidation : AbstractValidator<Contato>
    {
        public ContatoValidation()
        {
            RuleFor(x => x.TPCONTATO)
         .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DESCR1)
        .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");

        }
    }
}
