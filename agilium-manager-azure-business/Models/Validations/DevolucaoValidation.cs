using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class DevolucaoValidation : AbstractValidator<Devolucao>
    {
        public DevolucaoValidation()
        {
            RuleFor(x => x.IDEMPRESA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDMOTDEV).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDVENDA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.CDDEV).NotEmpty().WithMessage("O campo  {PropertyName}  é obrigatório");
        }
    }

    public class DevolucaoItemValidation : AbstractValidator<DevolucaoItem>
    {
        public DevolucaoItemValidation()
        {
            RuleFor(x => x.IDDEV).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.NUQTD).NotNull().WithMessage("O campo {PropertyName} é obrigatório")
                .GreaterThanOrEqualTo(0).WithMessage("O campo {PropertyName} deve ser maior que zero"); ;
            RuleFor(x => x.VLITEM)
          .GreaterThanOrEqualTo(0).WithMessage("O campo {PropertyName} deve ser maior que zero");
        }
    }
}
