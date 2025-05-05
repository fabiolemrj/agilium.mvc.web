using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;

namespace agilium.webapp.manager.mvc.ViewModels.ProdutoViewModel
{
    public class ProdutoFotoViewModel
    {
        public long Id { get; set; }
        [Display(Name = "Produto")]
        public Int64? idProduto { get; set; }
        [Display(Name = "Descrição")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public string Descricao { get; set; }
        [Display(Name = "Data")]
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public DateTime? Data { get; set; }
        [Display(Name = "Foto")]
        public IFormFile Foto { get; set; }
        public byte[] FotoConvertida { get; set; }
    }
}
