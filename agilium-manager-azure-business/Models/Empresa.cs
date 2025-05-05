using agilium.api.business.Enums;
using System;
using System.Collections.Generic;
using System.Text;

namespace agilium.api.business.Models
{
    public class Empresa: Entity
    {
        public string NUCNPJ { get; private set; }
        public long IDENDERECO { get; private set; }
        public virtual Endereco Endereco { get; set; }
        public string CDEMPRESA { get; private set; }
        public string NMRZSOCIAL { get; private set; }
        public string NMFANTASIA { get; private set; }
        public string DSINSCREST { get; private set; }
        public string DSINSCRESTVINC { get; private set; }
        public string DSINSCRMUN { get; private set; }
        public string NMDISTRIBUIDORA { get; private set; }
        public string NUREGJUNTACOM { get; private set; }
        public decimal? NUCAPARM { get; private set; } = 0;
        public ESimNao? STMICROEMPRESA { get; private set; }
        public ESimNao? STLUCROPRESUMIDO { get; private set; }
        public ETipoEmpresa? TPEMPRESA { get; private set; }
        public ECodigoRegimeTributario CRT { get; private set; }
        public string IDCSC { get; private set; }
        public string CSC { get; private set; }
        public string NUCNAE { get; private set; }
        public string IDCSC_HOMOL { get; private set; }
        public string CSC_HOMOL { get; private set; }
        public string IDLOJA_SITEMARCADO { get; private set; }
        public string CLIENTID_SITEMERCADO { get; private set; }
        public string CLIENTSECRET_SITEMERCADO { get; private set; }

        #region Listas Virtuais
        public virtual List<ContatoEmpresa> ContatoEmpresas { get; set; } = new List<ContatoEmpresa>();
        public virtual List<Config> Configuracoes { get; set; } = new List<Config>();
        public virtual List<EmpresaAuth> EmpresasAuth { get; set; } = new List<EmpresaAuth>();
        public virtual List<ConfigImagem> ConfigImagem { get; set; } = new List<ConfigImagem>();
        public virtual List<CaPerfil> Perfil { get; set; } = new List<CaPerfil>();
        public virtual List<ProdutoDepartamento> ProdutosDepartamentos { get; set; } = new List<ProdutoDepartamento>();
        public virtual List<ProdutoMarca> ProdutosMarcas { get; set; } = new List<ProdutoMarca>();
        public virtual List<MotivoDevolucao> MotivosDevolucao { get; set; } = new List<MotivoDevolucao>();
        public virtual List<GrupoProduto> GrupoProduto { get; set; } = new List<GrupoProduto>();
        public virtual List<Estoque> Estoques { get; set; } = new List<Estoque>();
        public virtual List<Funcionario> Funcionarios { get; set; } = new List<Funcionario>();
        public virtual List<Moeda> Moedas { get; set; } = new List<Moeda>();
        public virtual List<PontoVenda> PontosVendas { get; set; } = new List<PontoVenda>();
        public virtual List<Produto> Produtos { get; set; } = new List<Produto>();
        public virtual List<PlanoConta> PlanoContas { get; set; } = new List<PlanoConta>();
        public virtual List<ContaPagar> ContaPagar { get; set; } = new List<ContaPagar>();
        public virtual List<ContaReceber> ContaReceber { get; set; } = new List<ContaReceber>();
        public virtual List<NotaFiscalInutil> NotaFiscalInutil { get; set; } = new List<NotaFiscalInutil>();
        public virtual List<Turno> Turnos { get; set; } = new List<Turno>();
        public virtual List<Caixa> Caixas { get; set; } = new List<Caixa>();
        public virtual List<Vale> Vales { get; set; }=new List<Vale>();
        public virtual List<Perda> Perdas { get; set; } = new List<Perda>();
        public virtual List<Devolucao> Devolucao { get; set; } = new List<Devolucao>();
        public virtual List<Compra> Compras { get; set; } = new List<Compra>();
        public virtual List<Inventario> Inventarios { get; set; } = new List<Inventario>();
        public virtual List<ProdutoSiteMercado> ProdutoSiteMercado { get; set; } = new List<ProdutoSiteMercado>();
        public virtual List<MoedaSiteMercado> MoedasSiteMercados { get; set; } = new List<MoedaSiteMercado>();
        public virtual List<PedidoSitemercado> PedidosSiteMercado { get; set; } = new List<PedidoSitemercado>();
        public virtual List<FormaPagamento> FormasPagamento { get; set; } = new List<FormaPagamento>();
        public virtual List<Pedido> Pedidos { get; set; } = new List<Pedido>();
        #endregion
        public Empresa()
        {
        }

        public Empresa(string nUCNPJ, long iDENDERECO, string cDEMPRESA, string nMRZSOCIAL, string nMFANTASIA, string dSINSCREST, string dSINSCRESTVINC, string dSINSCRMUN, string nMDISTRIBUIDORA, string nUREGJUNTACOM, decimal? nUCAPARM, ESimNao? sTMICROEMPRESA, ESimNao? sTLUCROPRESUMIDO, ETipoEmpresa? tPEMPRESA, ECodigoRegimeTributario cRT, string iDCSC, string cSC, string nUCNAE, string iDCSC_HOMOL, string cSC_HOMOL, string iDLOJA_SITEMARCADO, string cLIENTID_SITEMERCADO, string cLIENTSECRET_SITEMERCADO)
        {
            NUCNPJ = nUCNPJ;
            IDENDERECO = iDENDERECO;
            CDEMPRESA = cDEMPRESA;
            NMRZSOCIAL = nMRZSOCIAL;
            NMFANTASIA = nMFANTASIA;
            DSINSCREST = dSINSCREST;
            DSINSCRESTVINC = dSINSCRESTVINC;
            DSINSCRMUN = dSINSCRMUN;
            NMDISTRIBUIDORA = nMDISTRIBUIDORA;
            NUREGJUNTACOM = nUREGJUNTACOM;
            NUCAPARM = nUCAPARM;
            STMICROEMPRESA = sTMICROEMPRESA;
            STLUCROPRESUMIDO = sTLUCROPRESUMIDO;
            TPEMPRESA = tPEMPRESA;
            CRT = cRT;
            IDCSC = iDCSC;
            CSC = cSC;
            NUCNAE = nUCNAE;
            IDCSC_HOMOL = iDCSC_HOMOL;
            CSC_HOMOL = cSC_HOMOL;
            IDLOJA_SITEMARCADO = iDLOJA_SITEMARCADO;
            CLIENTID_SITEMERCADO = cLIENTID_SITEMERCADO;
            CLIENTSECRET_SITEMERCADO = cLIENTSECRET_SITEMERCADO;
        }
    }
}
