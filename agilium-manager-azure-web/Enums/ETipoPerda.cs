using System.ComponentModel;
using System.ComponentModel.DataAnnotations;

namespace agilium.webapp.manager.mvc.Enums
{
    public enum ETipoPerda
    {
        [Display(Name ="Quebra ou Inutilização")]
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
}
