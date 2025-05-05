using agilium.api.business.Enums;

namespace agilium.api.pdv.ViewModels.CaixaViewModel
{
    public class MovimentoCaixaViewModel
    {
        public string idCaixa { get; set; }
        public double ValorSangria { get; set; } = 0;
        public string Obs { get; set; }
        public ETipoMovCaixa TipoMovimento { get; set; }
    }
}
