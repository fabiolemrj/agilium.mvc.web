using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using agilium.webapp.manager.mvc.Enums;
using System.Collections.Generic;

namespace agilium.webapp.manager.mvc.ViewModels.Cliente
{
    public class ClientePrecoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Cliente")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDCLIENTE { get; set; }
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public long IDPRODUTO { get; set; }
     
        public int Diferenca { get; set; }
        [Display(Name = "Valor")]
        [Range(0.0, double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double Valor { get; set; } = 0;
        [Display(Name = "Data Hora")]
        public DateTime Datahora { get; set; }
      
        public int TipoValor { get; set; }
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
        public string NomeCliente { get; set; }
        public double ValorFinal { get; set; } = 0;
        [Display(Name = "Diferença")]
        public ETpDiferencaPreco DescricaoTipoDiferenca { get; set; }
        [Display(Name = "Tipo de Valor")]
        public ETipoValorPreco DescricaoTipoValor { get; set; }
        public List<ClienteViewModel> Clientes { get; set; } = new List<ClienteViewModel>();
    }
}
