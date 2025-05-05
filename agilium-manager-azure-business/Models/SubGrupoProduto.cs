using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class SubGrupoProduto : Entity
    {
        public virtual GrupoProduto GrupoProduto { get; set; }
        public long? IDGRUPO { get; set; }
        public string NMSUBGRUPO { get; set; }
        public EAtivo? STATIVO { get; set; }
        public virtual List<Produto> Produtos { get; set; } = new List<Produto>();
        public SubGrupoProduto()
        {
         
        }

        public SubGrupoProduto(long? iDGRUPO, string nMSUBGRUPO, EAtivo? sTATIVO)
        {
            IDGRUPO = iDGRUPO;
            NMSUBGRUPO = nMSUBGRUPO;
            STATIVO = sTATIVO;
        }

    }
}
