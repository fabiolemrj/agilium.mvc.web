using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Ncm : Entity
    {
        public string CDNCM { get; set; }
        public string DSDESCR { get; set; }
        public Ncm()
        {

        }

        public Ncm(string cDNCM, string dSDESCR)
        {
            CDNCM = cDNCM;
            DSDESCR = dSDESCR;
        }
    }
}
