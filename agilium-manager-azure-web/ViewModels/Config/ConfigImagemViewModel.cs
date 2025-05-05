using Microsoft.AspNetCore.Http;

namespace agilium.webapp.manager.mvc.ViewModels.Config
{
    public class ConfigImagemViewModel
    {
        public string CHAVE { get; set; }
        public long? IDEMPRESA { get; set; }
        public byte[] IMG { get; set; }
        public IFormFile ImagemUpLoad { get; set; }
        public string ImagemConvertida { get; set; }
        public string Descricao { get; set; }
    }
}
