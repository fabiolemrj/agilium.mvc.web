using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Cep: Entity
    {
        public int id_logradouro { get; set; }
        public int id_cidade { get; set; }
        public string Endereco { get; set; }
        public string Numero { get; set; }
        public string uf { get; set; }
        public string cidade { get; set; }
        public string bairro { get; set; }
        public int ibge { get; set; }
        public string tipo { get; set; }
        public Cep()
        {

        }
    }
}
