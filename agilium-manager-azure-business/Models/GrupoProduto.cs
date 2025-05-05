using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class GrupoProduto : Entity
    {
        public virtual Empresa Empresa { get; set; }
        public long idEmpresa { get; set; }
        public string Nome { get; set; }
        public string CDGRUPO { get; set; }
        public EAtivo? StAtivo { get; set; }
        public virtual List<SubGrupoProduto> SubGrupoProdutos { get; set; } = new List<SubGrupoProduto>();
        public virtual List<Produto> Produtos { get; set; } = new List<Produto>();

        public GrupoProduto()
        {
          
        }

        public GrupoProduto(Empresa empresa, long idEmpresa, string nome, string cDGRUPO, EAtivo? stAtivo)
        {
            Empresa = empresa;
            this.idEmpresa = idEmpresa;
            Nome = nome;
            CDGRUPO = cDGRUPO;
            StAtivo = stAtivo;
        }
    }
}
