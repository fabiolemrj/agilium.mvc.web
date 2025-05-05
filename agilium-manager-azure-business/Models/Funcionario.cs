using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Funcionario: Entity
    {
        public long? IDUSUARIO { get; private set; }
        public virtual Usuario Usuario { get; private set; }
        public long? IDENDERECO { get; private set; }
        public virtual Endereco Endereco { get; private set; }
        public long? IDEMPRESA { get; private set; }
        public virtual Empresa Empresa { get; private set; }
        public string CDFUNC { get; private set; }
        public string NMFUNC { get; private set; }
        public int? NUTURNO { get; private set; }
        public int? STFUNC { get; private set; }
        public string NUCPF { get; private set; }
        public string NURG { get; private set; }
        public DateTime? DTADM { get; private set; }
        public DateTime? DTDEM { get; private set; }
        public string DSRFID { get; private set; }
        public int? STNOTURNO { get; private set; }
        public int? TipoFuncionario { get; private set; }
        public virtual List<Caixa> Caixas { get; set; } = new List<Caixa>();
        public virtual List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        public Funcionario()
        {            
        }

        public Funcionario(long? iDUSUARIO, long? iDENDERECO, long? iDEMPRESA, string cDFUNC, string nMFUNC, int? nUTURNO, int? sTFUNC, string nUCPF, string nURG, DateTime? dTADM, DateTime? dTDEM, string dSRFID, int? sTNOTURNO, int? tipoFuncionario)
        {
            IDUSUARIO = iDUSUARIO;
            IDENDERECO = iDENDERECO;
            IDEMPRESA = iDEMPRESA;
            CDFUNC = cDFUNC;
            NMFUNC = nMFUNC;
            NUTURNO = nUTURNO;
            STFUNC = sTFUNC;
            NUCPF = nUCPF;
            NURG = nURG;
            DTADM = dTADM;
            DTDEM = dTDEM;
            DSRFID = dSRFID;
            STNOTURNO = sTNOTURNO;
            TipoFuncionario = tipoFuncionario;
        }

        public void AdicionarEndereco(Endereco endereco) => Endereco = endereco;

        public void AdicionarEmpresa(Empresa empresa) => Empresa = empresa;

        public void AdicionarUsuario(Usuario caUsuario) => Usuario = caUsuario;
    }
}
