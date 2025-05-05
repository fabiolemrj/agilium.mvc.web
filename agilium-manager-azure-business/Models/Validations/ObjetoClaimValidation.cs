using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class ObjetoClaimValidation : AbstractValidator<ObjetoClaim>
    {
        public ObjetoClaimValidation()
        {
          
            RuleFor(x => x.ClaimType)
             .NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido").MaximumLength(10)
             .WithMessage("O campo {PropertyName} deve poasuir no maximo {MaxLength} caracteres"); ;
        }
    }
}
