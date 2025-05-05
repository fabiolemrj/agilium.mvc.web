using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Ibpt : Entity
    {
        public string NCM { get; set; }
        public int? EX { get; set; }
        public int? TIPO { get; set; }
        public string DESCRICAO { get; set; }
        public double? NACIONALFEDERAL { get; set; }
        public double? IMPORTADOSFEDERAL { get; set; }
        public double? ESTADUAL { get; set; }
        public double? MUNICIPAL { get; set; }
        public DateTime? INICIOVIG { get; set; }
        public DateTime? FIMVIG { get; set; }
        public string VERSAO { get; set; }
        public Ibpt()
        {

        }

        public Ibpt(string nCM, int? eX, int? tIPO, string dESCRICAO, double? nACIONALFEDERAL, double? iMPORTADOSFEDERAL, double? eSTADUAL, double? mUNICIPAL, DateTime? iNICIOVIG, DateTime? fIMVIG, string vERSAO)
        {
            NCM = nCM;
            EX = eX;
            TIPO = tIPO;
            DESCRICAO = dESCRICAO;
            NACIONALFEDERAL = nACIONALFEDERAL;
            IMPORTADOSFEDERAL = iMPORTADOSFEDERAL;
            ESTADUAL = eSTADUAL;
            MUNICIPAL = mUNICIPAL;
            INICIOVIG = iNICIOVIG;
            FIMVIG = fIMVIG;
            VERSAO = vERSAO;
        }
    }
}
