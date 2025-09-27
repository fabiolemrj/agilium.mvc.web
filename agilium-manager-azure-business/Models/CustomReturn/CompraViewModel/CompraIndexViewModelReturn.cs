using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium_manager_azure_business.Models.CustomReturn.CompraViewModel
{
    public class CompraIndexViewModelReturn
    {
        public long Id { get; set; }
        public string Fornecedor { get; set; }
        public DateTime DataCompra { get; set; }
        public ESituacaoCompra? Situacao { get; set; }
        public ETipoCompravanteCompra? TipoComprovante { get; set; }
        public string NumeroNF { get; set; }
        public string ValorDesconto { get; set; }
        public string ValorTotal { get; set; }
        public string ValorIsencao { get; set; }
        public string Codigo { get; set; }
    }
}
