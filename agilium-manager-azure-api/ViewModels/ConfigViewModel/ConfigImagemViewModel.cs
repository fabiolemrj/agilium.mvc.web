using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.ViewModels.ConfigViewModel
{
    public class ConfigImagemViewModel
    {
        public string CHAVE { get; set; }
        public long? IDEMPRESA { get; set; }
        public byte[] IMG { get; set; }
        public IFormFile ImagemUpLoad { get; set; }
        public string ImagemConvertida { get; set; }
    }
}
