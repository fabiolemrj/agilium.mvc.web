using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class VendaValidation : AbstractValidator<Venda>
    {
        public VendaValidation()
        {
            RuleFor(x => x.IDCAIXA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.SQVENDA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.STVENDA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DTHRVENDA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLVENDA).NotNull().WithMessage("O campo {PropertyName} é obrigatório");            
        }
    }
}
