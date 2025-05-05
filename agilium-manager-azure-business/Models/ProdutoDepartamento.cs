using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class ProdutoDepartamento : Entity
    {
        public virtual Empresa Empresa { get; private set; }
        public long? idEmpresa { get; private set; }
        public string CDDEP { get; private set; }
        public string NMDEP { get; private set; }
        public EAtivo? STDEP { get; private set; }
        public virtual List<Produto> Produtos { get; set; } = new List<Produto>();
        public ProdutoDepartamento()
        {
           
        }

        public ProdutoDepartamento(long? idEmpresa, string cDDEP, string nMDEP, EAtivo? sTDEP)
        {
            
            this.idEmpresa = idEmpresa;
            CDDEP = cDDEP;
            NMDEP = nMDEP;
            STDEP = sTDEP;
        }

        public ProdutoDepartamento(string cDDEP, string nMDEP, EAtivo? sTDEP)
        {
            
            CDDEP = cDDEP;
            NMDEP = nMDEP;
            STDEP = sTDEP;
        }
    }
}
