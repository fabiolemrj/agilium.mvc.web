namespace agilium.api.pdv.ViewModels.CaixaViewModel
{
    public class FecharCaixa
    {
        public string idCaixa { get; set; }
        public double ValorSangria { get; set; } = 0;
        public string Obs { get; set; }
    }

    public class FecharCaixaInicializa
    {
        public string idCaixa { get; set; }
        public string idEmpresa { get; set; }
        public string idTurno { get; set; }
        public int SqCaixa { get; set; }
        public string DataAbertura { get; set; }
        public string NuTurno { get; set; }
        public string Pdv { get; set; }
        public string usuario { get; set; }
        public string DataTurno { get; set; }
    }


}
