using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class EnderecoValidation : AbstractValidator<Endereco>
    {
        public EnderecoValidation()
        {
            RuleFor(x => x.Logradouro).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            RuleFor(x => x.Bairro).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            RuleFor(x => x.Cidade).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
            RuleFor(x => x.Uf).NotEmpty().WithMessage("O campo {PropertyName} precisa ser fornecido");
        }
    }
}
