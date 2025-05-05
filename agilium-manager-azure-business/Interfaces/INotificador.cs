﻿using agilium.api.business.Notificacoes;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Interfaces
{
    public interface INotificador
    {
        bool TemNotificacao();
        List<Notificacao> ObterNotificacoes();
        void Handle(Notificacao notificacao);
    }
}
