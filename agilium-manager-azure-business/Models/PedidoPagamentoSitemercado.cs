using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PedidoPagamentoSitemercado: Entity
    {
        public long? IDPedidoSM { get; private set; }
        public long? IDMoeda { get; private set; }
        public long? IDVendaMoeda { get; private set; }
        public int? _ID { get; private set; }
        public string Nome { get; private set; }
        public double? Valor { get; private set; }
        public double? VLCorrigido { get; private set; }
        public string Tipo { get; private set; }

        // Relacionamentos
        public virtual Moeda Moeda { get; set; }
        public virtual PedidoSitemercado PedidoSitemercado { get; set; }
        public virtual VendaMoeda VendaMoeda { get; set; }
        public PedidoPagamentoSitemercado()
        {            
        }

        public PedidoPagamentoSitemercado(long? iDPedidoSM, long? iDMoeda, long? iDVendaMoeda, int? iD, string nome, double? valor, double? vLCorrigido, string tipo)
        {
            IDPedidoSM = iDPedidoSM;
            IDMoeda = iDMoeda;
            IDVendaMoeda = iDVendaMoeda;
            _ID = iD;
            Nome = nome;
            Valor = valor;
            VLCorrigido = vLCorrigido;
            Tipo = tipo;
        }
    }
}
