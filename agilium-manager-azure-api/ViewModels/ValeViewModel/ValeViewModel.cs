using agilium.api.business.Enums;
using agilium.api.business.Models;
using System;

namespace agilium.api.manager.ViewModels.ValeViewModel
{
    public class ValeViewModel
    {
        public long Id { get; set; }
        public Int64? IDEMPRESA { get; set; }
        public string EmpresaNome { get; set; }
        public Int64? IDCLIENTE { get; set; }
        public string ClienteNome { get; set; }
        public string Codigo { get; set; }
        public DateTime? DataHora { get; set; }
        public ETipoVale? Tipo { get; set; }
        public ESituacaoVale? Situacao { get; set; }
        public double? Valor { get; set; }
        public string CodigoBarra { get; set; }
    }
}
