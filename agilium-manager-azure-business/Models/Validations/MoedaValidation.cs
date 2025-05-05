using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class MoedaValidation : AbstractValidator<Moeda>
    {
        public MoedaValidation()
        {
            RuleFor(x => x.CDMOEDA)
             .NotEmpty().WithMessage("O campo codigo é obrigatório");
            RuleFor(x => x.DSMOEDA)
                .NotEmpty().WithMessage("O campo descrição é obrigatório");
            RuleFor(x => x.COR_BOTAO)
              .NotEmpty().WithMessage("O campo Cor do botão é obrigatório");
            RuleFor(x => x.COR_FONTE)
              .NotEmpty().WithMessage("O campo Cor da fonte é obrigatório");
            RuleFor(x => x.TPDOCFISCAL)
              .NotNull().WithMessage("O campo Tipo documento fiscal é obrigatório");
            RuleFor(x => x.STTROCO)
              .NotNull().WithMessage("O campo Situação de troco é obrigatório");
            RuleFor(x => x.TPMOEDA)
              .NotNull().WithMessage("O campo Tipo de moeda é obrigatório");
            RuleFor(x => x.TECLA_ATALHO)
              .NotNull().WithMessage("O campo tecla atalho é obrigatório");
            RuleFor(x => x.IDEMPRESA)
              .NotNull().WithMessage("O campo descrição é obrigatório");

        }
    }
}
