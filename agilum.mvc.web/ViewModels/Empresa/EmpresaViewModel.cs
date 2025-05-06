using agilium.api.business.Enums;

using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System;
using agilum.mvc.web.Configuration;
using agilum.mvc.web.ViewModels.Endereco;
using agilum.mvc.web.ViewModels.Contato;

namespace agilum.mvc.web.ViewModels.Empresa
{
    public class EmpresaViewModel
    {
        public long Id { get; set; }
        [Display(Name = "CNPJ")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NUCNPJ { get; set; }
        [Display(Name = "Endereço")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDENDERECO { get; set; }
        [Display(Name = "Codigo")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(6, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CDEMPRESA { get; set; }
        [Display(Name = "Razão Social")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NMRZSOCIAL { get; set; }
        [Display(Name = "Nome Fantasia")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [StringLength(70, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NMFANTASIA { get; set; }
        [Display(Name = "Inscrição Estadual")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string DSINSCREST { get; set; }
        [Display(Name = "Inscr. Estadual Vinculada")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string DSINSCRESTVINC { get; set; }
        [Display(Name = "Insc. Municipal")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string DSINSCRMUN { get; set; }
        [Display(Name = "Nome Distribuidora")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NMDISTRIBUIDORA { get; set; }
        [Display(Name = "Registro Junta Comercial")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NUREGJUNTACOM { get; set; }
        [Moeda]
        [Range(0, Double.PositiveInfinity, ErrorMessage = "O campo {0} deve ser maior que {1}")]
        public decimal? NUCAPARM { get; set; } = 0;
        [Display(Name = "Micro Empresa")]
        public ESimNao? STMICROEMPRESA { get; set; }
        [Display(Name = "Lucro Presumido")]
        public ESimNao? STLUCROPRESUMIDO { get; set; }
        [Display(Name = "Tipo Empresa")]
        public ETipoEmpresa? TPEMPRESA { get; set; }
        [Display(Name = "Codigo Regime Tributario")]
        public ECodigoRegimeTributario CRT { get; set; }
        [Display(Name = "ID Codigo Seg. Contribuinte")]
        public string IDCSC { get; set; }
        [Display(Name = "Codigo Seg. Contribuinte")]
        [StringLength(40, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CSC { get; set; }
        [Display(Name = "CNAE")]
        [StringLength(10, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NUCNAE { get; set; }
        [Display(Name = "ID Cod. Seg. Contrib. Homol.")]
        [StringLength(10, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string IDCSC_HOMOL { get; set; }
        [Display(Name = "Codigo Seg. Contrib. Homol.")]
        [StringLength(10, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CSC_HOMOL { get; set; }
        [Display(Name = "Loja SM")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string IDLOJA_SITEMARCADO { get; set; }
        [Display(Name = "ID Site Mercado (SM)")]
        [StringLength(10, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CLIENTID_SITEMERCADO { get; set; }
        [Display(Name = "Senha SM")]
        [StringLength(10, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CLIENTSECRET_SITEMERCADO { get; set; }
    }

    public class EmpresaCreateViewModel : EmpresaViewModel
    {
        public EnderecoIndexViewModel Endereco { get; set; } = new EnderecoIndexViewModel();
        public List<ContatoEmpresaViewModel> ContatosEmpresa { get; set; } = new List<ContatoEmpresaViewModel>();
    }
}
