using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;

namespace agilium.api.manager.ViewModels.SiteMercadoViewModel
{
    public class MoedaSiteMercadoViewModel
    {
        public long Id { get; set; }
        public string MoedaPdv { get; set; }
        public long? IDMOEDA { get; set; }
        public long? IDEMPRESA { get; set; }
        public ETipoMoedaSiteMercado? IDSM { get; set; }
        public DateTime? DataHora { get; set; }
    }
}
