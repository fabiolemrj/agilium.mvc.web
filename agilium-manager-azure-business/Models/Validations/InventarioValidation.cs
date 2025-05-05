using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class InventarioValidation : AbstractValidator<Inventario>
    {
        public InventarioValidation()
        {
            RuleFor(x => x.CDINVENT).NotEmpty().WithMessage("O campo codigo é obrigatório");
            RuleFor(x => x.DSINVENT).NotEmpty().WithMessage("O campo descrição é obrigatório");
            RuleFor(x => x.IDEMPRESA).NotNull().WithMessage("O campo empresa é obrigatório");
            RuleFor(x => x.IDESTOQUE).NotNull().WithMessage("O campo estoque é obrigatório");
            RuleFor(x => x.STINVENT).NotNull().WithMessage("O campo situação é obrigatório");
            RuleFor(x => x.TPANALISE).NotNull().WithMessage("O campo Tipo analise é obrigatório");
        }
    }

    public class InventarioItemValidation : AbstractValidator<InventarioItem>
    {
        public InventarioItemValidation()
        {
            RuleFor(x => x.IDPRODUTO).NotNull().WithMessage("O campo produto é obrigatório");
        }
    }
}
