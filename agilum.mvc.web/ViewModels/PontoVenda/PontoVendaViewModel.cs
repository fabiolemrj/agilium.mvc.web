using agilium.api.business.Enums;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.Estoque;

namespace agilum.mvc.web.ViewModels.PontoVenda
{
    public class PontoVendaViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Empresa")]
        public Int64? IDEMPRESA { get; set; }
        [Display(Name = "Codigo")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Codigo { get; set; }
        [Display(Name = "Descricao")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string Descricao { get; set; }
        [Display(Name = "Nome da Maquina")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NomeMaquina { get; set; }
        [Display(Name = "Caminho Certificado Digital")]
        [StringLength(255, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CaminhoCertificadoDigital { get; set; }
        [Display(Name = "Senha Certificado Digital")]
        [StringLength(30, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string SenhaCertificadoDigital { get; set; }
        [Display(Name = "Estoque")]
        public Int64? IDESTOQUE { get; set; }
        [Display(Name = "Nome da impressora")]
        [StringLength(100, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NMIMPRESSORA { get; set; }
        [Display(Name = "Gaveta")]
        public ESimNao? Gaveta { get; set; }
        public string PortaImpressora { get; set; }
        public int? CDMODELOBAL { get; set; }
        //CDHANDSHAKEBAL int (11)
        public int? CDHANDSHAKEBAL { get; set; }
        //CDPARITYBAL int (11) 
        public int? CDPARITYBAL { get; set; }
        //CDSERIALSTOPBITBAL int (11)
        public int? CDSERIALSTOPBITBAL { get; set; }
        //NUDATABITBAL int (11)
        public int? NUDATABITBAL { get; set; }
        //NUBAUDRATEBAL int (11)
        public int? NUBAUDRATEBAL { get; set; }
        //DSPORTABAL varchar(20)
        public string DSPORTABAL { get; set; }
        [Display(Name = "Situação")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public EAtivo? Situacao { get; set; }
        public List<Empresa.EmpresaViewModel> Empresas { get; set; } = new List<EmpresaViewModel>();
        public List<Estoque.EstoqueViewModel> Estoques { get; set; } = new List<EstoqueViewModel>();
    }
}
