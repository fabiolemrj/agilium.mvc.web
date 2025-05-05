using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ProdutoMarca : Entity
    {
        public virtual Empresa Empresa { get; set; }
        public long? idEmpresa { get; set; }
        public string CDMARCA { get; set; }
        public string NMMARCA { get; set; }
        public int? STMARCA { get; set; }
        public virtual List<Produto> Produtos { get; set; } = new List<Produto>();
        public ProdutoMarca(long? idEmpresa, string cDMARCA, string nMMARCA, int? sTMARCA)
        {

            this.idEmpresa = idEmpresa;
            CDMARCA = cDMARCA;
            NMMARCA = nMMARCA;
            STMARCA = sTMARCA;
        }

        public ProdutoMarca()
        {
        }

        public ProdutoMarca(string cDMARCA, string nMMARCA, int? sTMARCA)
        {

            CDMARCA = cDMARCA;
            NMMARCA = nMMARCA;
            STMARCA = sTMARCA;
        }
    }
}
