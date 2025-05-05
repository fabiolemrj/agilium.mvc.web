using FluentValidation;
using System;
using System.Collections.Generic;
using System.Security.Cryptography.X509Certificates;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class FornecedorValidation : AbstractValidator<Fornecedor>
    {
        public FornecedorValidation()
        {
            RuleFor(x => x.NMRZSOCIAL)
             .NotEmpty().WithMessage("O campo Razão Social é obrigatório");
            RuleFor(x => x.NMFANTASIA)
            .NotEmpty().WithMessage("O campo Nome Fantasia é obrigatório");
            RuleFor(x => x.CDFORN)
            .NotEmpty().WithMessage("O campo Codigo é obrigatório");
            RuleFor(x => x.TPPESSOA)
            .NotNull().WithMessage("O campo Tipo de pessoa é obrigatório");
            RuleFor(x => x.TPPESSOA)
             .NotNull().WithMessage("O campo Tipo de pessoa é obrigatório");
            RuleFor(x => x.STFORNEC)
            .NotNull().WithMessage("O campo situação é obrigatório");

            When(x => ((!string.IsNullOrEmpty(x.NUCPFCNPJ)) && x.TPPESSOA == "J"), () =>
            {
                RuleFor(f => CnpjValidacao.Validar(f.NUCPFCNPJ)).Equal(true)
                                   .WithMessage("O documento fornecido é inválido.");
            });

            When(x => ((!string.IsNullOrEmpty(x.NUCPFCNPJ)) && x.TPPESSOA == "F"), () =>
            {
                RuleFor(f => CpfValidacao.Validar(f.NUCPFCNPJ)).Equal(true)
                                 .WithMessage("O documento fornecido é inválido.");
            });
        }
    }
}
