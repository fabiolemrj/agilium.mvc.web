using System;

namespace agilum.mvc.web.ViewModels.UnidadeViewModel
{
    public class ListaUsuarioViewModel
    {
        public long id { get; private set; }
        public string NomeUsuario { get; private set; }
        public string CpfUsuario { get; private set; }
        public string ativo { get; private set; }
        public DateTime? DataCadastro { get; private set; }
        public string idUserAspNet { get; set; }
    }
}
