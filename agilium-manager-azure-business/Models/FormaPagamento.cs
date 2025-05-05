using agilium.api.business.Enums;
using System.Collections.Generic;

namespace agilium.api.business.Models
{
    public class FormaPagamento: Entity
    {
        public long? IDEmpresa { get; private set; }
        public string DSFormaPagamento { get; private set; }
        public EAtivo? STFormaPagamento { get; private set; }
        public virtual List<PedidoPagamento> PedidoPagamentos { get; set; } = new List<PedidoPagamento>();

        // Relacionamentos
        public virtual Empresa Empresa { get; set; }
        public FormaPagamento()
        {            
        }

        public FormaPagamento(long? iDEmpresa, string dSFormaPagamento, EAtivo? sTFormaPagamento)
        {
            IDEmpresa = iDEmpresa;
            DSFormaPagamento = dSFormaPagamento;
            STFormaPagamento = sTFormaPagamento;
        }
    }
}