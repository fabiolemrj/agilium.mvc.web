using agilium.api.manager.Extension;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace agilium.api.manager.ViewModels.ConfigViewModel
{
    public class ConfigIndexViewModel
    {
        public string CHAVE { get; set; }
        public long? IDEMPRESA { get; set; }
        public string VALOR { get; set; }
     
        public IFormFile Arquivo { get; set; }
    }
}
