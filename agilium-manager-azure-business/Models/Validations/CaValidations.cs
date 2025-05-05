using agilium.api.business.Interfaces.IRepository;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models.Validations
{
    public class CaPerfilValidation : AbstractValidator<CaPerfil>
    {
        public CaPerfilValidation()
        {
            RuleFor(x => x.Descricao)
          .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class CaModeloValidation : AbstractValidator<CaModelo>
    {
        public CaModeloValidation()
        {
            RuleFor(x => x.idPerfil)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.idPermissao)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class CaPermissaoItemValidation : AbstractValidator<CaPermissaoItem>
    {
        public CaPermissaoItemValidation()
        {
            RuleFor(x => x.Descricao)
       .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class CaPerfilManagerValidation : AbstractValidator<CaPerfiManager>
    {
        public CaPerfilManagerValidation()
        {
            RuleFor(x => x.Descricao)
          .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class CaPermissaoManagerValidation : AbstractValidator<CaPermissaoManager>
    {
        public CaPermissaoManagerValidation()
        {
            RuleFor(x => x.IdArea).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IdPerfil).NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

}
