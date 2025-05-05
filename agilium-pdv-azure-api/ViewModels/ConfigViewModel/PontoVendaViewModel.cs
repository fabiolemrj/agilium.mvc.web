using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Xml.Linq;
using System;
using agilium.api.business.Enums;

namespace agilium.api.pdv.ViewModels.ConfigViewModel
{
    public class PontoVendaAssociacao
    {
        public string Id { get; set; }
        public string Descricao { get; set; }
    }
    public class PontoVendaViewModel
    {
        public string Id { get; set; }
        public string IDEMPRESA { get; set; }
        public string Codigo { get; set; }
        public string Descricao { get; set; }
        public string NomeMaquina { get; set; }
        public string CaminhoCertificadoDigital { get; set; }
        public string SenhaCertificadoDigital { get; set; }
        public string IDESTOQUE { get; set; }
        public string NMIMPRESSORA { get; set; }
        public ESimNao? Gaveta { get; set; }
        public string PortaImpressora { get; set; }
        public string CDMODELOBAL { get; set; }
        //CDHANDSHAKEBAL int (11)
        public string CDHANDSHAKEBAL { get; set; }
        //CDPARITYBAL int (11) 
        public string CDPARITYBAL { get; set; }
        //CDSERIALSTOPBITBAL int (11)
        public string CDSERIALSTOPBITBAL { get; set; }
        //NUDATABITBAL int (11)
        public string NUDATABITBAL { get; set; }
        //NUBAUDRATEBAL int (11)
        public string NUBAUDRATEBAL { get; set; }
        //DSPORTABAL varchar(20)
        public string DSPORTABAL { get; set; }
        public EAtivo? Situacao { get; set; }
    }
}
