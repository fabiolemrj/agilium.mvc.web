using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class TurnoValidation : AbstractValidator<TurnoPreco>
    {
        public TurnoValidation()
        {
            RuleFor(x => x.IDPRODUTO)
                .NotNull().WithMessage("O campo Produto é obrigatório");

            RuleFor(x => x.NUTURNO)
                .NotNull().WithMessage("O campo Numero do Turno é obrigatório");
            RuleFor(x => x.NUVALOR)
                .NotNull().WithMessage("O campo valo é obrigatório");
        }
    }
}
