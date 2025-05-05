using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class ConfigValidation : AbstractValidator<Config>
    {
        public ConfigValidation()
        {
            //RuleFor(x => x.VALOR).NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            //RuleFor(x => x.CHAVE).NotEmpty().WithMessage("O campo Nome {PropertyName} é obrigatório");
            RuleFor(x => x.IDEMPRESA).NotNull().WithMessage("O campo {PropertyName} da Empresa é obrigatório");
        }
    }
}
