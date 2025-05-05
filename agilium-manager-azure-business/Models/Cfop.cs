using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Cfop : Entity
    {
        public int CDCFOP { get; set; }
        public string DSCFOP { get; set; }
        public string TPCFOP { get; set; }
        public Cfop()
        {

        }

        public Cfop(int cDCFOP, string dSCFOP, string tPCFOP)
        {
            CDCFOP = cDCFOP;
            DSCFOP = dSCFOP;
            TPCFOP = tPCFOP;
        }
    }
}
