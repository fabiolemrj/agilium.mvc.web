using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.CustomReturn.ReportViewModel.EstoqueReportViewModel
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
        public string NmGrupo { get;set; }
        public double NuQtdMin { get; set; }
        public double VlCustoMedio { get; set; }
        public double VlFinanc { get; set; }
        public double Quantidade { get; set; }
        public int Situacao { get; set; }

    }
}
