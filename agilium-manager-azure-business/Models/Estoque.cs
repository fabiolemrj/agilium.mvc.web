using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Estoque : Entity
    {
        public long? idEmpresa { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public string Descricao { get; private set; }
        public int? Tipo { get; private set; }
        public decimal? Capacidade { get; private set; }
        public EAtivo? STESTOQUE { get; private set; }
        public virtual List<ProdutoComposicao> ProdutosComposicoes { get; set; } = new List<ProdutoComposicao>();
         public virtual List<EstoqueProduto> EstoqueProdutos { get; set; }= new List<EstoqueProduto>();
         public virtual List<EstoqueHistorico> EstoqueHistoricos { get; set; }= new List<EstoqueHistorico>();
         public virtual List<CompraItem> CompraItems { get; set; }= new List<CompraItem>();
         public virtual List<Perda> Perdas { get; set; } = new List<Perda>();
        public virtual List<PontoVenda> PontosVendas { get; set; }= new List<PontoVenda>();
        public virtual List<Inventario> Inventarios { get; set; }= new List<Inventario>();
        public virtual List<PedidoItem> Pedidoitens { get; set; } = new List<PedidoItem>();

        public Estoque()
        {
            
        }

        public Estoque(long? idEmpresa, string descricao, int? tipo, decimal? capacidade, EAtivo? sTESTOQUE)
        {
            this.idEmpresa = idEmpresa;
            Descricao = descricao;
            Tipo = tipo;
            Capacidade = capacidade;
            STESTOQUE = sTESTOQUE;
        }




        //public void AdicionarProduto(long? idProduto, double? nuQtd)
        //                    => EstoqueProdutos.Add(new EstoqueProduto(idProduto, this, nuQtd));

        //public void AdicionarHistorico(string nmusuario, DateTime? dthrhst, int? tphst, string dshst, double? qtdhst, long? idItem, long? idLanc)
        //                    => EstoqueHistoricos.Add(new EstoqueHistorico(this, idItem, idLanc, dthrhst, nmusuario, tphst, dshst, qtdhst));
    }
}
