using System.ComponentModel.DataAnnotations;

namespace agilum.mvc.web.Enums
{
    public enum ETipoEstoque
    {
        Almoxarifado = 1,
        Combustiveis = 2

    }

    public enum ETipoFuncionario
    {
        Padrao = 1,
        Entregador = 2
    }

    public enum EEstoqueProduto
    {
        [Display(Name = "Retorna o Produto ao Estoque")]
        RetornaEstoque = 0,
        [Display(Name = "Não Retorna o Produto ao Estoque")]
        NaoRetornaEstoque = 1
    }

}

