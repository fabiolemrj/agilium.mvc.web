using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class PedidoItemSitemercado: Entity
    {
        public long? IDPedidoSitemercado { get; private set; }
        public long? IDVendaItem { get; private set; }
        public int? _ID { get; private set; }
        public int? IDX { get; private set; }
        public string Codigo { get; private set; }
        public string CodigoLoja { get; private set; }
        public int? PesoVariavel { get; private set; }
        public string CodigoBarras { get; private set; }
        public long? PLU { get; private set; }
        public string Produto { get; private set; }
        public int? IDProduto { get; private set; }
        public string Observacao { get; private set; }
        public double? Quantidade { get; private set; }
        public double? Quantidade3 { get; private set; }
        public double? Valor { get; private set; }
        public double? ValorTotal { get; private set; }
        public int? Indisponivel { get; private set; }
        public int? Desistencia { get; private set; }

        // Relacionamentos
        public virtual PedidoSitemercado PedidoSitemercado { get; set; }
        public virtual VendaItem VendaItem { get; set; }
        public PedidoItemSitemercado()
        {
            
        }

        public PedidoItemSitemercado(long? iDPedidoSitemercado, long? iDVendaItem, int? iD, int? iDX, string codigo, string codigoLoja, int? pesoVariavel, string codigoBarras, long? pLU, string produto, int? iDProduto, string observacao, double? quantidade, double? quantidade3, double? valor, double? valorTotal, int? indisponivel, int? desistencia)
        {
            IDPedidoSitemercado = iDPedidoSitemercado;
            IDVendaItem = iDVendaItem;
            _ID = iD;
            IDX = iDX;
            Codigo = codigo;
            CodigoLoja = codigoLoja;
            PesoVariavel = pesoVariavel;
            CodigoBarras = codigoBarras;
            PLU = pLU;
            Produto = produto;
            IDProduto = iDProduto;
            Observacao = observacao;
            Quantidade = quantidade;
            Quantidade3 = quantidade3;
            Valor = valor;
            ValorTotal = valorTotal;
            Indisponivel = indisponivel;
            Desistencia = desistencia;
        }
    }

}
