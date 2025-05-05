using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Csosn : Entity
    {
        public string CST { get; set; }
        public string DESCR { get; set; }
        public Csosn()
        {

        }

        public Csosn(string cST, string dESCR)
        {
            CST = cST;
            DESCR = dESCR;
        }
    }
}
