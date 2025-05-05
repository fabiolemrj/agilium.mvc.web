using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.ViewModels
{
    public class UsuarioFotoViewModel
    {
        public string GerarId()
        {
            Guid guid = Guid.NewGuid();

            byte[] _bytes = guid.ToByteArray();
            return BitConverter.ToInt64(_bytes, 0).ToString();
        }

        public string id { get; set; } 
        public string idAspNetUser { get; set; }
        public string Foto {get; set; }
        public DateTime DataCadastro { get; set; } = new DateTime();
        public IFormFile ImagemUpLoad { get; set; }
        public string ImagemConvertida { get; set; }
        public string Ativo { get; set; }
        public string NomeArquivo { get; set; }
        public string NomeArquivoExtensao { get; set; }

        public UsuarioFotoViewModel()
        {
            id = GerarId();
        }
    }
}
