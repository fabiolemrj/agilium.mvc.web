using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class MoedaSiteMercadoValidation : AbstractValidator<MoedaSiteMercado>
    {
        public MoedaSiteMercadoValidation()
        {
            RuleFor(x => x.IDEMPRESA).NotNull().WithMessage("O campo Empresa é obrigatório");
            RuleFor(x => x.IDSM).NotNull().WithMessage("O campo Moeda Site Mercado é obrigatório");
            RuleFor(x => x.IDMOEDA).NotNull().WithMessage("O campo Moeda Pdv é obrigatório");
        }
    }

    public class ProdutoSiteMercadoValidation : AbstractValidator<ProdutoSiteMercado>
    {
        public ProdutoSiteMercadoValidation()
        {
            RuleFor(x => x.IDEMPRESA).NotNull().WithMessage("O campo Empresa é obrigatório");
            RuleFor(x => x.IDPRODUTO).NotNull().WithMessage("O campo Empresa é obrigatório");
        }
    }
}
