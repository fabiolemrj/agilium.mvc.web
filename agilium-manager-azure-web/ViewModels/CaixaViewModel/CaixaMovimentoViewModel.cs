using agilium.webapp.manager.mvc.Enums;
using System;

namespace agilium.webapp.manager.mvc.ViewModels.CaixaViewModel
{
    public class CaixaMovimentoViewModel
    {
        public long Id { get; set; }
        public Int64? IDCAIXA { get; set; }
        public ETipoMovCaixa? Tipo { get; set; }
        public string Descricao { get; set; }
        public double? Valor { get; set; }
        public ESituacaoCaixa? Situacao { get; set; }
        public string Caixa { get; set; }
    }
}
