using agilium.api.business.Models;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.api.manager.ViewModels.TurnoViewModel
{
    public class TurnoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public Int64? IDEMPRESA { get; set; }
        [Display(Name = "Usuario Inicial")]
        public Int64? IDUSUARIOINI { get; set; }
        [Display(Name = "Data Final")]
        public Int64? IDUSUARIOFIM { get; set; }
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

    public class TurnoIndexViewModel: TurnoViewModel
    {
        public string Empresa { get; set; }
        public string UsuarioInicial { get; set; }
        public string UsuarioFinal { get; set; }
    }
}
