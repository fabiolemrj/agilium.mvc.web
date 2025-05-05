using agilium.api.business.Enums;
using agilium.api.pdv.ViewModels.ContatoViewModel;
using agilium.api.pdv.ViewModels.EnderecoViewModel;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.pdv.ViewModels.EmpresaViewModel
{

    public class EmpresaSimplesViewModel
    {
        public string Id { get; set; }
        public string Cnpj { get; set; }
        public string Codigo { get; set; }
        public string RazaoSocial { get; set; }
        public string NomeFantasia { get; set; }
    }

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
        [Display(Name = "Inscrição Estadual Vinculada")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string DSINSCRESTVINC { get; set; }
        [Display(Name = "Inscrição Municipal")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string DSINSCRMUN { get; set; }
        [Display(Name = "Nome Distribuidora")]
        [StringLength(50, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NMDISTRIBUIDORA { get; set; }
        [Display(Name = "Registro Junta Comercial")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NUREGJUNTACOM { get; set; }
        public decimal? NUCAPARM { get; set; } = 0;
        [Display(Name = "MicroEmpresa")]
        public ESimNao? STMICROEMPRESA { get; set; }
        [Display(Name = "LucroPresumido")]
        public ESimNao? STLUCROPRESUMIDO { get; set; }
        [Display(Name = "Tipo Empresa")]
        public ETipoEmpresa? TPEMPRESA { get; set; }
        [Display(Name = "Codigo Regime Tributario")]
        public ECodigoRegimeTributario CRT { get; set; }
        public string IDCSC { get; set; }
        [Display(Name = "Codigo Segurança Contribuinte")]
        [StringLength(40, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CSC { get; set; }
        [Display(Name = "CNAE")]
        [StringLength(10, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string NUCNAE { get; set; }
        public string IDCSC_HOMOL { get; set; }
        [Display(Name = "Codigo de segurança Contribuinte Homologação")]
        [StringLength(40, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CSC_HOMOL { get; set; }
        [Display(Name = "CNAE")]
        [StringLength(20, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string IDLOJA_SITEMARCADO { get; set; }
        [Display(Name = "Cliente Site mercadi")]
        [StringLength(10, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CLIENTID_SITEMERCADO { get; set; }
        [Display(Name = "Senha Cliente Site mercado")]
        [StringLength(10, ErrorMessage = "Quantidade maxima de caracteres para o campo {0} deve ser de até {1}")]
        public string CLIENTSECRET_SITEMERCADO { get; set; }
    }

    public class EmpresaCreateViewModel : EmpresaViewModel
    {
        public EnderecoIndexViewModel Endereco { get; set; } = new EnderecoIndexViewModel();
        public List<ContatoEmpresaViewModel> ContatosEmpresa { get; set; } = new List<ContatoEmpresaViewModel>();
    }
}
