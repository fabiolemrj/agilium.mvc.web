using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class ClienteValidation : AbstractValidator<Cliente>
    {
        public ClienteValidation()
        {
            RuleFor(x => x.STCLIENTE).NotNull().WithMessage("O campo situação é obrigatório");
            RuleFor(x => x.STPUBEMAIL).NotNull().WithMessage("O campo situação email é obrigatório");
            RuleFor(x => x.STPUBSMS).NotNull().WithMessage("O campo situação SMS é obrigatório");
            RuleFor(x => x.TPCLIENTE).NotNull().WithMessage("O campo situação tipo cliente é obrigatório");
        }
    }

    public class ClientePrecoValidation : AbstractValidator<ClientePreco>
    {
        public ClientePrecoValidation()
        {
            RuleFor(x => x.IDCLIENTE).NotNull().WithMessage("O campo Cliente é obrigatório");
            RuleFor(x => x.IDPRODUTO).NotNull().WithMessage("O campo Produto é obrigatório");
            RuleFor(x => x.NUVALOR).NotNull().WithMessage("O campo Valor é obrigatório");
        }
    }
}
