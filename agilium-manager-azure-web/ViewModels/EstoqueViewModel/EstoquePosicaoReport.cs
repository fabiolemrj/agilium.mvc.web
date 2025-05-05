using agilium.webapp.manager.mvc.Enums;
using System.Collections.Generic;

namespace agilium.webapp.manager.mvc.ViewModels.EstoqueViewModel
{
    public class EstoquePosicaoReport
    {
        public long Id { get; set; }
        public long? idEmpresa { get; set; }
        public string Descricao { get; set; }
        public int? Tipo { get; set; }
        public decimal? Capacidade { get; set; }
        public EAtivo? STESTOQUE { get; set; }
        public string NMProduto { get; set; }
        public string CdProduto { get; set; }
        public string NmGrupo { get; set; }
        public double NuQtdMin { get; set; }
        public double VlCustoMedio { get; set; }
        public double VlFinanc { get; set; }
        public double Quantidade { get; set; }
        public int Situacao { get; set; }
    }

    public class FiltroEstoquePosicao
    {
        public long IdEstoque { get; set; }
        public List<EstoquePosicaoReport> Lista { get; set; } = new List<EstoquePosicaoReport>();
        public List<EstoqueViewModel> Estoques { get; set; } = new List<EstoqueViewModel>();
    }
}
