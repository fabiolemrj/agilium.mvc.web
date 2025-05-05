using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ProdutoFoto : Entity
    {
        public Int64? idProduto { get; private set; }
        public virtual Produto Produto { get; private set; }
        public string Descricao { get; private set; }
        public DateTime? Data { get; private set; }
        public byte[] Foto { get; private set; }

        public ProdutoFoto()
        {
        }

        public ProdutoFoto(long? idProduto, string descricao, DateTime? data, byte[] foto)
        {
            this.idProduto = idProduto;
            Descricao = descricao;
            Data = data;
            Foto = foto;
        }

        public ProdutoFoto(long? idProduto, string descricao, DateTime? data)
        {
            this.idProduto = idProduto;
            Descricao = descricao;
            Data = data;
        }

        public void AdicionarId() => Id = GerarId();
        public void AdiconarFoto(byte[] foto) => Foto = foto;
    }
}
