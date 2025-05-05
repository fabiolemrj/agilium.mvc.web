using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;

namespace agilium.webapp.manager.mvc.ViewModels.TurnoViewModel
{
    public class TurnoIndexViewModel
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
        public string Empresa { get; set; }
        public string UsuarioInicial { get; set; }
        public string UsuarioFinal { get; set; }
    }

    public class TurnoFechamentoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64? IDEMPRESA { get; set; }
        [Display(Name = "Usuario Inicial")]
        public Int64? IDUSUARIOINI { get; set; }
        [Display(Name = "Data Final")]
        public Int64? IDUSUARIOFIM { get; set; }
        [Display(Name = "Data")]
        public DateTime? Data { get; set; }
        [Display(Name = "Numero Turno")]
        public int? NumeroTurno { get; set; }
        [Display(Name = "Data Inicial")]
        public DateTime? DataInicial { get; set; }
        [Display(Name = "Data Final")]
        public DateTime? DataFinal { get; set; }
        [Display(Name = "Observação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Obs { get; set; }
        public string Empresa { get; set; }
        public string UsuarioInicial { get; set; }
        public string UsuarioFinal { get; set; }
        public TurnoFechamentoViewModel()
        {
            
        }

        public TurnoFechamentoViewModel(long id, long? iDEMPRESA, long? iDUSUARIOINI, long? iDUSUARIOFIM, DateTime? data, int? numeroTurno, DateTime? dataInicial, DateTime? dataFinal, string obs, string empresa, string usuarioInicial, string usuarioFinal)
        {
            Id = id;
            IDEMPRESA = iDEMPRESA;
            IDUSUARIOINI = iDUSUARIOINI;
            IDUSUARIOFIM = iDUSUARIOFIM;
            Data = data;
            NumeroTurno = numeroTurno;
            DataInicial = dataInicial;
            DataFinal = dataFinal;
            Obs = obs;
            Empresa = empresa;
            UsuarioInicial = usuarioInicial;
            UsuarioFinal = usuarioFinal;
        }
    }
}
