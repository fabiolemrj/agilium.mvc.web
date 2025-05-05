using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class EstoqueValidation : AbstractValidator<Estoque>
    {
        public EstoqueValidation()
        {
            RuleFor(x => x.Descricao)
            .NotEmpty().WithMessage("O campo Descrição é obrigatório");
            RuleFor(x => x.idEmpresa)
            .NotEmpty().WithMessage("O campo Empresa é obrigatório");
            RuleFor(x => x.STESTOQUE)
            .NotNull().WithMessage("O campo situação é obrigatório");
            RuleFor(x => x.Tipo)
            .NotNull().WithMessage("O campo Tipo é obrigatório");
          
        }
    }
}
