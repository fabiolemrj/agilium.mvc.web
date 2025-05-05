using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace agilium.api.pdv.ViewModels.TurnoViewModel
{
    public class TurnoViewModel
    {
        public string Id { get; set; }
        [Display(Name = "Empresa")]
        public string IDEMPRESA { get; set; }
        [Display(Name = "Usuario Inicial")]
        public string IDUSUARIOINI { get; set; }
        [Display(Name = "Data Final")]
        public string IDUSUARIOFIM { get; set; }
        [Display(Name = "Data")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime? Data { get; set; }
        [Display(Name = "Numero Turno")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int? NumeroTurno { get; set; }
        [Display(Name = "Data Inicial")]
        public DateTime? DataInicial { get; set; }
        [Display(Name = "Data Final")]
        public DateTime? DataFinal { get; set; }
        [Display(Name = "Observação")]
        public string Obs { get; set; }
    }

    public class TurnoIndexViewModel : TurnoViewModel
    {
        public string Empresa { get; set; }
        public string UsuarioInicial { get; set; }
        public string UsuarioFinal { get; set; }
    }

}
