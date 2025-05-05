using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Cliente: Entity
    {

        public string CDCLIENTE { get; private set; }
        public string NMCLIENTE { get; private set; }
        public ETipoPessoa? TPCLIENTE { get; private set; }
        public long? IDENDERECO { get; private set; }
        public virtual Endereco Endereco { get; private set; }
        public long? IDENDERECOCOB { get; private set; }
        public virtual Endereco EnderecoCobranca { get; private set; }
        public long? IDENDERECOFAT { get; private set; }
        public virtual Endereco EnderecoFaturamento { get; private set; }
        public long? IDENDERECONTREGA { get; private set; }
        public virtual Endereco EnderecoEntrega { get; private set; }
        public DateTime? DTCAD { get; private set; }
        public EAtivo? STCLIENTE { get; private set; }
        public ESimNao? STPUBEMAIL { get; private set; }
        public ESimNao? STPUBSMS { get; private set; }
        public virtual ClientePF ClientesPFs { get; set; } 
        public virtual ClientePJ ClientesPJs { get; set; } 
        public virtual List<ClienteContato> ClienteContatos { get; set; } = new List<ClienteContato>();
        public virtual List<ClientePreco> ClientePrecos { get; set; } = new List<ClientePreco>();
        public virtual List<ContaReceber> ContaReceber { get; set; } = new List<ContaReceber>();
        public virtual List<Vale> Vales { get; set; } = new List<Vale>();
        public virtual List<Venda> Venda { get; set; } = new List<Venda>();
        public virtual List<Devolucao> Devolucao { get; set; } = new List<Devolucao>();
        public virtual List<PedidoSitemercado> PedidosSiteMercado { get; set; }=new List<PedidoSitemercado>();
        public virtual List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public virtual List<VendaTemporaria> VendaTemporaria { get; set; } = new List<VendaTemporaria>();
        public Cliente()
        {
            
        }

        public Cliente(string cDCLIENTE, string nMCLIENTE, ETipoPessoa tPCLIENTE, long? iDENDERECO, long? iDENDERECOCOB, long? iDENDERECOFAT, long? iDENDERECONTREGA, DateTime? dTCAD, EAtivo? sTCLIENTE)
        {
            CDCLIENTE = cDCLIENTE;
            NMCLIENTE = nMCLIENTE;
            TPCLIENTE = tPCLIENTE;
            IDENDERECO = iDENDERECO;
            IDENDERECOCOB = iDENDERECOCOB;
            IDENDERECOFAT = iDENDERECOFAT;
            IDENDERECONTREGA = iDENDERECONTREGA;
            DTCAD = dTCAD;
            STCLIENTE = sTCLIENTE;
        }

        public Cliente(string cDCLIENTE, string nMCLIENTE, ETipoPessoa tPCLIENTE, long? iDENDERECO, long? iDENDERECOCOB, long? iDENDERECOFAT, long? iDENDERECONTREGA, EAtivo? sTCLIENTE)
        {
            CDCLIENTE = cDCLIENTE;
            NMCLIENTE = nMCLIENTE;
            TPCLIENTE = tPCLIENTE;
            IDENDERECO = iDENDERECO;
            IDENDERECOCOB = iDENDERECOCOB;
            IDENDERECOFAT = iDENDERECOFAT;
            IDENDERECONTREGA = iDENDERECONTREGA;
            STCLIENTE = sTCLIENTE;
        }

        public void AdicionarEndereco(long idender) => IDENDERECO = idender;
        public void AdicionarEnderecoCobranca(long idender) => IDENDERECOCOB = idender;
        public void AdicionarEnderecoFaturamento(long idender) => IDENDERECOFAT = idender;
        public void AdicionarEnderecoEntrega(long idender) => IDENDERECONTREGA = idender;

        public void AdicionarEndereco(Endereco endereco) => Endereco = endereco;
        public void AdicionarEnderecoCobranca(Endereco endereco) => EnderecoCobranca = endereco;
        public void AdicionarEnderecoFaturamento(Endereco endereco) => EnderecoFaturamento = endereco;
        public void AdicionarEnderecoEntrega(Endereco endereco) => EnderecoEntrega = endereco;

        public void AdicionarPessoaFisica(ClientePF cliente) => ClientesPFs = cliente;
        public void AdicionarPessoaJuridica(ClientePJ cliente) => ClientesPJs = cliente;
    }
}
