using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.Validations
{
    public class ProdutoValidation : AbstractValidator<Produto>
    {
        public ProdutoValidation()
        {
            RuleFor(x => x.NMPRODUTO)
              .NotEmpty().WithMessage("O campo nome é obrigatório");
            RuleFor(x => x.CDPRODUTO)
                .NotEmpty().WithMessage("O campo codigo é obrigatório");
            RuleFor(x => x.STPRODUTO)
                .NotNull().WithMessage("O campo situação é obrigatório");
            RuleFor(x => x.NUPRECO)
                .GreaterThanOrEqualTo(0).WithMessage("O campo {PropertyName} deve ser maior que zero");
            //RuleFor(x => x.IDGRUPO)
            //    .NotNull().WithMessage("O campo grupo é obrigatório");
            //RuleFor(x => x.IDSUBGRUPO)
            //    .NotNull().WithMessage("O campo subgrupo é obrigatório");
            //RuleFor(x => x.IDMARCA)
            //    .NotNull().WithMessage("O campo marca é obrigatório");
            //RuleFor(x => x.IDDEP)
            //    .NotNull().WithMessage("O campo departamento é obrigatório");
        }
    }

    public class ProdutoDepartamentoValidation : AbstractValidator<ProdutoDepartamento>
    {
        public ProdutoDepartamentoValidation()
        {
            RuleFor(x => x.NMDEP)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.CDDEP)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.STDEP)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class ProdutoMarcaValidation : AbstractValidator<ProdutoMarca>
    {
        public ProdutoMarcaValidation()
        {
            RuleFor(x => x.NMMARCA)
             .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.CDMARCA)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.STMARCA)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class GrupoProdutoValidation : AbstractValidator<GrupoProduto>
    {
        public GrupoProdutoValidation()
        {
            RuleFor(x => x.CDGRUPO)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.Nome)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.StAtivo)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class SubGrupoProdutoValidation : AbstractValidator<SubGrupoProduto>
    {
        public SubGrupoProdutoValidation()
        {
            RuleFor(x => x.NMSUBGRUPO)
                .NotEmpty().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.IDGRUPO)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }
    }

    public class ProdutoPrecoValidation : AbstractValidator<ProdutoPreco>
    {
        public ProdutoPrecoValidation()
        {
            RuleFor(x => x.idProduto)
                  .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.Preco)
                .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.Preco)
                .GreaterThanOrEqualTo(0).WithMessage("O campo {PropertyName} deve ser maior que zero");
        }
    }

    public class ProdutoFotoValidation : AbstractValidator<ProdutoFoto>
    {
        public ProdutoFotoValidation()
        {
            RuleFor(x => x.idProduto)
               .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
            RuleFor(x => x.Foto)
               .NotNull().WithMessage("O campo {PropertyName} é obrigatório");
        }

    }
}
