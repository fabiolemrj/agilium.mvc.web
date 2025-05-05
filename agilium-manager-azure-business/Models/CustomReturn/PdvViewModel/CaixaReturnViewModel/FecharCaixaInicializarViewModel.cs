using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models.CustomReturn.PdvViewModel.CaixaReturnViewModel
{
    public class FecharCaixaInicializarViewModel
    {
        public long idCaixa { get; set; }
        public long idEmpresa { get; set; }
        public long idTurno { get; set; }
        public int SqCaixa { get; set; }
        public DateTime DataAbertura { get; set; }
        public string NuTurno { get; set; }
        public string Pdv { get; set; }
        public string usuario { get; set; }
        public DateTime DataTurno { get; set; }
    }
}
