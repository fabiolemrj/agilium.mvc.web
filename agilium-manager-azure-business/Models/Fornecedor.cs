using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Fornecedor: Entity
    {
        public long IDENDERECO { get; private set; }
        public virtual Endereco Endereco { get; private set; }
        public string CDFORN { get; private set; }
        public string NMRZSOCIAL { get; private set; }
        public string NMFANTASIA { get; private set; }
        public string TPPESSOA { get; private set; }
        public string NUCPFCNPJ { get; private set; }
        public string DSINSCR { get; private set; }
        public ETipoFiscal TPFISCAL { get; private set; }
        public int STFORNEC { get; private set; }
        public virtual List<FornecedorContato> FornecedoresContatos { get; set; } = new List<FornecedorContato>();
        public virtual List<ContaPagar> ContaPagar { get; set; } = new List<ContaPagar>();
        public virtual List<Compra> Compras { get; set; } = new List<Compra>();
        public virtual List<PedidoItem> Pedidoitens { get; set; } = new List<PedidoItem>();
        public Fornecedor()
        {
            
        }

        public Fornecedor(long iDENDERECO, string cDFORN, string nMRZSOCIAL, string nMFANTASIA, string tPPESSOA, string nUCPFCNPJ, string dSINSCR, ETipoFiscal tPFISCAL, int sTFORNEC)
        {
            IDENDERECO = iDENDERECO;
            CDFORN = cDFORN;
            NMRZSOCIAL = nMRZSOCIAL;
            NMFANTASIA = nMFANTASIA;
            TPPESSOA = tPPESSOA;
            NUCPFCNPJ = nUCPFCNPJ;
            DSINSCR = dSINSCR;
            TPFISCAL = tPFISCAL;
            STFORNEC = sTFORNEC;
        }

        public Fornecedor(string cDFORN, string nMRZSOCIAL, string nMFANTASIA, string tPPESSOA, string nUCPFCNPJ, string dSINSCR, ETipoFiscal tPFISCAL, int sTFORNEC)
        {
            CDFORN = cDFORN;
            NMRZSOCIAL = nMRZSOCIAL;
            NMFANTASIA = nMFANTASIA;
            TPPESSOA = tPPESSOA;
            NUCPFCNPJ = nUCPFCNPJ;
            DSINSCR = dSINSCR;
            TPFISCAL = tPFISCAL;
            STFORNEC = sTFORNEC;
        }

        public void AdicionarEndereco(Endereco endereco) => Endereco = endereco;

        public void AdicionarEndereco(long idEndereco) => IDENDERECO = idEndereco;

    }
}
