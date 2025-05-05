using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.ViewModels.EnderecoViewModel
{
    public class EnderecoIndexViewModel
    {
        public long Id { get; set; }
        public string Logradouro { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cep { get; set; }
        public string Cidade { get; set; }
        public string Uf { get; set; }
        public string Pais { get; set; }
        public int? Ibge { get; set; }
        public string PontoReferencia { get; set; }
        public string Numero { get; set; }
    }

}
