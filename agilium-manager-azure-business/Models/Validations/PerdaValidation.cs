using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class PerdaValidation : AbstractValidator<Perda>
    {
        public PerdaValidation()
        {
            RuleFor(x => x.CDPERDA)
               .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDPRODUTO)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDESTOQUE)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDUSUARIO)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDPRODUTO)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");

        }
    }
}
