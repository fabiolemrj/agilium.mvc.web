using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Cst : Entity
    {
        public string CST { get; set; }
        public string DESCR { get; set; }
        public Cst()
        {

        }

        public Cst(string cST, string dESCR)
        {
            CST = cST;
            DESCR = dESCR;
        }
    }
}
