namespace agilium.webapp.manager.mvc.ViewModels.ControleAcesso
{
    public class PerfilIndexViewModel
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
        public long idEmpresa { get; set; }
    }

    public class PermissaoItemIndexViewModel
    {
        public long Id { get; set; }
        public string Descricao { get; set; }
        public string Situacao { get; set; }
    }
}
