using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class EmpresaValidation : AbstractValidator<Empresa>
    {
        public EmpresaValidation()
        {
            RuleFor(x => x.NMRZSOCIAL)
          .NotEmpty().WithMessage("O campo Razão Social é obrigatório");
            RuleFor(x => x.NMFANTASIA)
            .NotEmpty().WithMessage("O campo Nome Fantasia é obrigatório");
            RuleFor(x => x.CDEMPRESA)
            .NotEmpty().WithMessage("O campo Codigo da Empresa é obrigatório");
            RuleFor(x => x.TPEMPRESA)
            .NotNull().WithMessage("O campo Tipo Empresa é obrigatório");
            RuleFor(x => x.NUCNPJ).NotEmpty()
            .WithMessage("O campo CNPJ é obrigatório");
            RuleFor(x => x.CRT).NotNull()
            .WithMessage("O campo CRT é obrigatório");
           // RuleFor(x => x.IDENDERECO).NotNull()
           //.WithMessage("O campo Endereço é obrigatório");

            When(x => ((!string.IsNullOrEmpty(x.NUCNPJ))), () =>
            {
                RuleFor(f => CnpjValidacao.Validar(f.NUCNPJ)).Equal(true)
                    .WithMessage("O documento fornecido é inválido.");
            });
        }
    }
}
