using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class PlanoContaValidation : AbstractValidator<PlanoConta>
    {
        public PlanoContaValidation()
        {
            RuleFor(x => x.IDEMPRESA).NotNull().WithMessage("O campo Empresa é obrigatório");
            RuleFor(x => x.CDCONTA).NotEmpty().WithMessage("O campo Codigo da conta é obrigatório");
            RuleFor(x => x.DSCONTA).NotEmpty().WithMessage("O campo Descrição da conta é obrigatório");
            RuleFor(x => x.TPCONTA).NotNull().WithMessage("O campo Tipo da conta é obrigatório");
            RuleFor(x => x.STCONTA).NotNull().WithMessage("O campo situação é obrigatório");
        }
    }

    public class PlanoContaSaldoValidation : AbstractValidator<PlanoContaSaldo>
    {
        public PlanoContaSaldoValidation()
        {
            RuleFor(x => x.IDCONTA)
              .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLSALDO)
              .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class PlanoContaLancamentoValidation : AbstractValidator<PlanoContaLancamento>
    {
        public PlanoContaLancamentoValidation()
        {
            RuleFor(x => x.IDCONTA)
              .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.TPLANC)
              .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.NUANOMESREF)
              .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DTREF)
              .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.STLANC)
              .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }
}
