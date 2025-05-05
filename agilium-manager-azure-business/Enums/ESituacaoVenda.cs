using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Enums
{
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
        NFE =2
    }

    public enum ESituacaoItemVenda
    {
        Cancelado=0,
        Ativo =1,
        Devolvido =2
    }
}
