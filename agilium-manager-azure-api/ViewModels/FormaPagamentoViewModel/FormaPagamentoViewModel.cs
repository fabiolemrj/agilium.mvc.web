using agilium.api.business.Enums;

namespace agilium.api.manager.ViewModels.FormaPagamentoViewModel
{
    public class FormaPagamentoViewModel
    {
        public long Id { get; set; }
        public long? IDEmpresa { get; set; }
        public string Descricao { get; set; }
        public EAtivo? Situacao { get; set; }
    }
}
