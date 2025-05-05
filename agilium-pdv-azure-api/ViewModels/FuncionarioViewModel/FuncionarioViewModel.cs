using System;

namespace agilium.api.pdv.ViewModels.FuncionarioViewModel
{
    public class FuncionarioViewModel
    {
        public string Id { get; set; }
        public string IDUSUARIO { get; set; }
        public string IDENDERECO { get; set; }
        public string IDEMPRESA { get; set; }
        public string Codigo { get; set; }
        public string Nome { get; set; }
        public int? Turno { get; set; }
        public int? Situacao { get; set; }
        public string CPF { get; set; }
        public string Documento { get; set; }
        public DateTime? DataAdmissao { get; set; }
        public DateTime? DataDemissao { get; set; }
        public string DSRFID { get; set; }
        public int? Noturno { get; set; }
        public int? TipoFuncionario { get; set; }
    }

  
}
