namespace agilum.mvc.web.ViewModels.Endereco
{
    public class CepViewModel
    {
        public int id_logradouro { get; set; }
        public int id_cidade { get; set; }
        public string endereco { get; set; }
        public string numero { get; set; }
        public string uf { get; set; }
        public string cidade { get; set; }
        public string bairro { get; set; }
        public int ibge { get; set; }
        public string tipo { get; set; }
    }
}
