using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.pdv.ViewModels
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
