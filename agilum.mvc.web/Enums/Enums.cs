using System.ComponentModel;
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

    public enum ETipoPerda
    {
        [Display(Name = "Quebra ou Inutilização")]
        Quebra = 1,
        [Display(Name = "Devolução de Cliente")]
        Devolucao = 2,
        [Display(Name = "Validade Vencida")]
        Vencido = 3,
        [Display(Name = "Acerto de Saldo")]
        AcertoSaldo = 4,
        [Display(Name = "Falha Operacional")]
        FalhaOpercional = 5,
        [Display(Name = "Outros")]
        Outros = 6
    }

    public enum ETipoMovimentoPerda
    {
        Perda = 1,
        Sobra = 2
    }

    public enum ESituacaoInventario
    {
        Cancelada = 0,
        Aberta = 1,
        Execucao = 2,
        Concluida = 3
    }

    public enum ETipoAnalise
    {
        Manual = 1,
        App = 2
    }

    public enum ESituacaoCaixa
    {
        Fechado = 0,
        Aberto = 1
    }

    public enum ETipoMovCaixa
    {
        Sangria = 1,
        Suprimento = 2
    }

    public enum ESituacaoMovCaixa
    {
        Ativo = 0,
        Cancelado = 1
    }

    public enum ETipoDocFiscal
    {
        Todos = 0,
        NFCe = 1,
        NFe = 2
    }
    public enum ESituacaoVenda
    {
        Inativo = 0,
        Ativo = 1
    }

    public enum ETipoEmissaoVenda
    {
        NaoEmitido = 0,
        Emitido = 1,
        Contigencia = 2,
        Cancelada = 3
    }

    public enum ESituacaoVendaFiscal
    {
        Emitido = 1,
        Contingencia = 2,
        Cancelado = 3
    }

    public enum ETipoDocVenda
    {
        NFCE = 1,
        NFE = 2
    }

    public enum ESituacaoItemVenda
    {
        Cancelado = 0,
        Ativo = 1,
        Devolvido = 2
    }

    public enum ESituacaoDevolucao
    {
        Cancelada = 0,
        Aberta = 1,
        Realizada = 2
    }

    public enum ESituacaoVale
    {
        Cancelado = 0,
        Ativo = 1,
        Utilizado = 2
    }

    public enum ETipoVale
    {
        Troca = 1,
        Presente = 2,
        Promocao = 3
    }

    public enum ETipoContaLancacmento
    {
        Debito = 1,
        Credito = 2
    }

    public enum ETipoConta
    {
        Eventual = 1,
        Fixa = 2
    }

    public enum ESituacaoConta
    {
        Prevista = 1,
        Consolidada = 2
    }
    public enum ETipoCompnenteConfig
    {
        Texto,
        SimNao,
        Numero,
        Cores,
        Moeda,
        TipoAmbiente,
        TipoAberturaCaixa
    }

    public enum EClassificacaoConfiguracao
    {
        NaoExibir,
        Gerais,
        Empresa,
        Caixa,
        NFCe,
        Pedido,
        Contabil,
        Cores,
        Email,
        PDV,
    }
    public enum ESimNaoMvc
    {
        Sim = 1,
        Nao = 2
    }
    public enum ETipoAberturaCaixa
    {
        [Description("Abrir com saldo do caixa anterior")]
        SaldoCaixaAnterior = 1,
        [Description("Abrir com saldo zerado")]
        SaldoZerado = 2
    }

    public enum ETipoAmbiente
    {
        [Description("Homologação")]
        Homologacao = 1,
        [Description("Produção")]
        Producao = 2
    }

    public enum EResultadoFiltroRanking
    {
        Grupo = 0,
        Produto = 1,
        Data = 2,
        DiaSemana = 3,
        Mes = 4,
        Ano = 5
    }

    public enum EOrdenacaoFiltroRanking
    {
        Venda = 0,
        Quantidade = 1,
        Lucro = 2
    }


}

