using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class ContaValidation : AbstractValidator<ContaPagar>
    {
        public ContaValidation()
        {
            RuleFor(x => x.IDCATEG_FINANC).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDFORNEC).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDCONTA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDEMPRESA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DTVENC).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLCONTA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.PARCINI).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.STCONTA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class ContaReceberValidation : AbstractValidator<ContaReceber>
    {
        public ContaReceberValidation()
        {
            RuleFor(x => x.IDCATEG_FINANC).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDCLIENTE).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDCONTA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDEMPRESA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DTVENC).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLCONTA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.PARCINI).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.STCONTA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }
}
