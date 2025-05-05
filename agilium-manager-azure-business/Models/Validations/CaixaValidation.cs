using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class CaixaValidation : AbstractValidator<Caixa>
    {
        public CaixaValidation()
        {
            RuleFor(x => x.IDEMPRESA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDFUNC).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLABT).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DTHRABT).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class CaixaMoedaValidation : AbstractValidator<CaixaMoeda>
    {
        public CaixaMoedaValidation()
        {
            RuleFor(x => x.IDCAIXA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDMOEDA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLMOEDACORRECAO).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDUSUARIOCORRECAO).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DTHRCORRECAO).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLMOEDACORRECAO)
                .GreaterThanOrEqualTo(0).WithMessage("O campo {PropertyName} deve ser maior que zero");

        }
    }
}
