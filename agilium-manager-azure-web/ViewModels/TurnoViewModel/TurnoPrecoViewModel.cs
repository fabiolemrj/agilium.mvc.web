using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using agilium.webapp.manager.mvc.Enums;
using System.Collections.Generic;
using agilium.webapp.manager.mvc.ViewModels.Cliente;

namespace agilium.webapp.manager.mvc.ViewModels.TurnoViewModel
{
    public class TurnoPrecoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public Int64 IDPRODUTO { get; set; }
        [Display(Name = "Numero do Turno")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int NumeroTurno { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int Diferenca { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public int TipoValor { get; set; }
        [Display(Name = "Valor")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        [Range(0.0, Double.MaxValue, ErrorMessage = "O campo {0} deve ser maior que {1}.")]
        public double Valor { get; set; }
        [Display(Name = "Usuario")]
        public string Usuario { get; set; }
        [Display(Name = "Data/Hora")]
        public DateTime DataHora { get; set; }
        public string NomeCliente { get; set; }
        public double ValorFinal { get; set; } = 0;
        [Display(Name = "Diferença")]
        public ETpDiferencaPreco DescricaoTipoDiferenca { get; set; }
        [Display(Name = "Tipo de Valor")]
        public ETipoValorPreco DescricaoTipoValor { get; set; }
        public List<ClienteViewModel> Clientes { get; set; } = new List<ClienteViewModel>();
    }
}
