using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Models.CustomReturn.PedidoViewModel
{
    public class ClientePedidoCustomViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public DateTime? dataCadastro { get; set; } = DateTime.Now;
        public EAtivo? STCLIENTE { get; set; } = EAtivo.Ativo;
        public ESimNao? STPUBEMAIL { get; set; } = ESimNao.Sim;
        public ESimNao? STPUBSMS { get; set; } = ESimNao.Sim;
        public Endereco Endereco { get; set; } = new Endereco();
    }
}
