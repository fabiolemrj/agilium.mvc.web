using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class PontoVendaValidation : AbstractValidator<PontoVenda>
    {
        public PontoVendaValidation()
        {
            RuleFor(x => x.CDPDV)
       .NotEmpty().WithMessage("O campo Código é obrigatório");
            RuleFor(x => x.DSPDV)
       .NotEmpty().WithMessage("O campo Descrição é obrigatório");
            RuleFor(x => x.IDEMPRESA)
       .NotNull().WithMessage("O campo Empresa é obrigatório");
            RuleFor(x => x.IDESTOQUE)
      .NotNull().WithMessage("O campo Estoque é obrigatório");
        }
    }
}
