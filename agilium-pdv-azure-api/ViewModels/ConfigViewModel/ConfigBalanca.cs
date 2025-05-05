using System.Collections;
using System.Collections.Generic;

namespace agilium.api.pdv.ViewModels.ConfigViewModel
{
    public class ConfigBalanca
    {
        public string cdModelo { get; set; }
        public string cdHandShake { get; set; }
        public string cdParity { get; set; }
        public string cdSerialStop { get; set; }
        public string nuDataBits { get; set; }
        public string nuBaudRate { get; set; }
        public string dsPorta { get; set; }         
    }

    public class ListaBalanca
    {
        public int Id { get; set; }
        public string Descricao { get; set; }

        public ListaBalanca(int id, string descricao)
        {
            Id = id;
            Descricao = descricao;
        }
        public ListaBalanca()
        {
            
        }
    }

    public class ListasViewModel
    {
        public List<ListaBalanca> Handshaking { get; set; }= new List<ListaBalanca>();
        public List<ListaBalanca> Modelos { get; set; } = new List<ListaBalanca>();
        public List<ListaBalanca> Parity { get; set; } = new List<ListaBalanca>();
        public List<ListaBalanca> PortaSerial { get; set; } = new List<ListaBalanca>();
        public List<ListaBalanca> StopBits { get; set; } = new List<ListaBalanca>();
        public List<ListaBalanca> BaudRate { get; set; } = new List<ListaBalanca>();
        public List<ListaBalanca> DataBits { get; set; } = new List<ListaBalanca>();
    }
}
