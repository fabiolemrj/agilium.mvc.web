using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Contato: Entity
    {
        public ETipoContato TPCONTATO { get; private set; }
        public string DESCR1 { get; private set; }
        public string DESCR2 { get; private set; }
        public DateTime DataCadastro { get; private set; } = DateTime.Now;
        public virtual List<ContatoEmpresa> ContatoEmpresas { get; set; } = new List<ContatoEmpresa>();
        public virtual List<FornecedorContato> FornecedoresContatos { get; set; } = new List<FornecedorContato>();
        public virtual List<ClienteContato> ClienteContatos { get; set; } = new List<ClienteContato>();
        public Contato()
        {

        }

        public Contato(ETipoContato tPCONTATO, string dESCR1, string dESCR2)
        {
            TPCONTATO = tPCONTATO;
            DESCR1 = dESCR1;
            DESCR2 = dESCR2;
        }
    }
}
