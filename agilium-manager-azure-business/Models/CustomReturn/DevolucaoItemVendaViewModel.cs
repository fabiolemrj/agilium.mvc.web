using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.CustomReturn
{
    public class DevolucaoItemVendaCustom
    {
        public bool selecionado { get; set; }
        public int SeqVenda { get; set; }
        public string ProdutoNome { get; set; }
        public double QuantidadeVendida { get; set; }
        public double ValorVendido { get; set; }
        public double QuantidadeDevolucao { get; set; }
        public double ValorDevolucao { get; set; }
        public long idItemVenda { get; set; }
        public long idDevolucaoItem { get; set; }
        public long idProduto { get; set; }
        public double ValorTotal { get; set; }
        public long idDevolucao { get; set; }
    }
}
