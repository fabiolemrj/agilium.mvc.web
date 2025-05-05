using agilium.api.business.Models;
using Microsoft.AspNetCore.Http;
using System;
using System.ComponentModel.DataAnnotations;

namespace agilium.api.manager.ViewModels.ProdutoVewModel
{
    public class ProdutoFotoViewModel
    {
        public long Id { get; set; }
        public Int64? idProduto { get; set; }
        public string Descricao { get; set; }
        public DateTime? Data { get; set; }
        [Required(ErrorMessage = "O campo {0} é obrigatorio")]
        public IFormFile Foto { get; set; }
        public byte[] FotoConvertida { get; set; }
    }
}
