using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class CompraValidation : AbstractValidator<Compra>
    {
        public CompraValidation()
        {
            RuleFor(x => x.CDCOMPRA)
        .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDEMPRESA)
        .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDFORN)
        .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DTCOMPRA)
        .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DTCAD)
        .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.STCOMPRA)
        .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.TPCOMPROVANTE)
        .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.NUCFOP)
        .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLTOTAL)
        .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLTOTPROD)
        .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class CompraItemValidation : AbstractValidator<CompraItem>
    {
        public CompraItemValidation()
        {
       //     RuleFor(x => x.IDPRODUTO)
       //.NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDCOMPRA)
       .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
       //     RuleFor(x => x.IDESTOQUE)
       //.NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class CompraFiscalValidation : AbstractValidator<CompraFiscal>
    {
        public CompraFiscalValidation()
        {
            RuleFor(x => x.IDCOMPRA)
      .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }
}
