using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class UnidadeValidation : AbstractValidator<Unidade>
    {
        public UnidadeValidation()
        {
            RuleFor(x => x.Codigo)
                .NotEmpty().WithMessage("O campo  {PropertyName}  é obrigatório");
            RuleFor(x => x.Descricao)
                .NotEmpty().WithMessage("O campo  {PropertyName}  é obrigatório");
        }
    }
}
