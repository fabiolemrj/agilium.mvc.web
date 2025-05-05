using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class FormaPagamentoValidation : AbstractValidator<FormaPagamento>
    {
        public FormaPagamentoValidation()
        {
            RuleFor(x => x.IDEmpresa).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.DSFormaPagamento).NotEmpty().WithMessage("O campo Descricao é obrigatório");
     
        }
    }
}
