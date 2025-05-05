using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class MotivoDevolucaoValidation : AbstractValidator<MotivoDevolucao>
    {
        public MotivoDevolucaoValidation()
        {
            RuleFor(x => x.DSMOTDEV)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.STMOTDEV)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDEMPRESA)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }
}
