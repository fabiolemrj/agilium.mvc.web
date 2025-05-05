using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class PedidoValidation : AbstractValidator<Pedido>
    {
        public PedidoValidation()
        {
            RuleFor(x => x.IDCaixa).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDEmpresa).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDCliente).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.VLPedido).GreaterThan(0).WithMessage("O campo deve ser maior que zero");
            RuleFor(x => x.IDEndereco).GreaterThan(0).WithMessage("'Endereço de entrega não selecionado. ");
            RuleFor(x => x.IDCliente).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }
}
