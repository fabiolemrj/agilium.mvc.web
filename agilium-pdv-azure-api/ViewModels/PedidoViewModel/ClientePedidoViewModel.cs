using agilium.api.business.Enums;
using agilium.api.pdv.ViewModels.EnderecoViewModel;
using System;
using System.Collections;
using System.Collections.Generic;

namespace agilium.api.pdv.ViewModels.PedidoViewModel
{
    public class ClientePedidoViewModel
    {
        public string Id { get; set; }
        public string Nome { get; set; }
        public DateTime? dataCadastro { get; set; }=DateTime.Now;
        public EAtivo? STCLIENTE { get; set; } = EAtivo.Ativo;
        public ESimNao? STPUBEMAIL { get; set; } = ESimNao.Sim;
        public ESimNao? STPUBSMS { get; set; } = ESimNao.Sim;
        public IEnumerable<EnderecoIndexViewModel> Enderecos { get; set; } = new List<EnderecoIndexViewModel>();
    }


}
