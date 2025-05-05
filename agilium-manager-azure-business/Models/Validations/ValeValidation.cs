using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class ValeValidation : AbstractValidator<Vale>
    {
        public ValeValidation()
        {
            RuleFor(x => x.IDEMPRESA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDCLIENTE).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.TPVALE).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.STVALE).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLVALE)
               .GreaterThanOrEqualTo(0).WithMessage("O campo {PropertyName} deve ser maior que zero");
        }
    }
}
