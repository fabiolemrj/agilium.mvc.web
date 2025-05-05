using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class FuncionarioValidation : AbstractValidator<Funcionario>
    {
        public FuncionarioValidation()
        {
            RuleFor(x => x.NMFUNC).NotEmpty().WithMessage("O campo Nome é obrigatório");
            RuleFor(x => x.STFUNC).NotNull().WithMessage("O campo situação é obrigatório");
            RuleFor(x => x.CDFUNC).NotEmpty().WithMessage("O campo Codigo é obrigatório");
  
        }
    }
}
