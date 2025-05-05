using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Enums
{
    public enum ETipoPerda
    {
        Quebra =1,
        Devolucao =2,
        Vencido =3,
        AcertoSaldo=4,
        FalhaOpercional=5,
        Outros=6
    }

    public enum ETipoMovimentoPerda
    {
        Perda =1,
        Sobra=2
    }
}
