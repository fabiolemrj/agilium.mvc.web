using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class CategoriaFinanceiraValidation : AbstractValidator<CategoriaFinanceira>
    {
        public CategoriaFinanceiraValidation()
        {
            RuleFor(x => x.NMCATEG)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.STCATEG)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

  
}
