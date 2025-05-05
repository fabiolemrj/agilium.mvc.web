using System;
using System.Collections.Generic;

namespace agilium.api.manager.ViewModels.ImpostoViewModel
{

    public class TabelasAxuliaresFiscalViewModel
    {
        public List<CfopViewModel> Cfops { get; set; } = new List<CfopViewModel>();
        public List<CstViewModel> Csts { get; set; } = new List<CstViewModel>();
        public List<CestViewModel> Cests { get; set; } = new List<CestViewModel>();
       public List<CsosnViewModel> Csosn { get; set; } = new List<CsosnViewModel>();
    }

    public class CfopViewModel
    {
        public int CDCFOP { get; set; }
        public string DSCFOP { get; set; }
        public string TPCFOP { get; set; }
    }

    public class CestViewModel
    {
        public string CDCEST { get; set; }
        public string CDNCM { get; set; }
        public string DSDESCR { get; set; }
    }

    public class CstViewModel
    {
        public string CST { get; set; }
        public string DESCR { get; set; }
    }

    public class IbptViewModel
    {
        public string NCM { get; set; }
        public int? EX { get; set; }
        public int? TIPO { get; set; }
        public string DESCRICAO { get; set; }
        public double? NACIONALFEDERAL { get; set; }
        public double? IMPORTADOSFEDERAL { get; set; }
        public double? ESTADUAL { get; set; }
        public double? MUNICIPAL { get; set; }
        public DateTime? INICIOVIG { get; set; }
        public DateTime? FIMVIG { get; set; }
        public string VERSAO { get; set; }
    }

    public class CsosnViewModel
    {
        public string CST { get; set; }
        public string DESCR { get; set; }
    }

    public class NcmViewModel
    {
        public string CDNCM { get; set; }
        public string DSDESCR { get; set; }

    }
}
