using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class CestNcm : Entity
    {
        public string CDCEST { get; set; }
        public string CDNCM { get; set; }
        public string DSDESCR { get; set; }

        public CestNcm(string cDCEST, string cDNCM, string dSDESCR)
        {
            CDCEST = cDCEST;
            CDNCM = cDNCM;
            DSDESCR = dSDESCR;
        }

        public CestNcm()
        {

        }
    }
}
