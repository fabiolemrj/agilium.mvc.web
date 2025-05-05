using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.infra.ViewModelDapper
{
    public class DevolucaoItemVendaViewModel
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
    }
}
