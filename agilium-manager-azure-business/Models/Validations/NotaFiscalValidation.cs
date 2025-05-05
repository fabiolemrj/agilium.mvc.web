using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class NotaFiscalValidation
    {
    }

    public class NotaFiscalInutilaValidation : AbstractValidator<NotaFiscalInutil>
    {
        public NotaFiscalInutilaValidation()
        {
            RuleFor(x => x.DSMOTIVO)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.CDNFINUTIL)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDEMPRESA)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DSMODELO)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DTHRINUTIL)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DSSERIE)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.NUANO)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
        }

    }
}
