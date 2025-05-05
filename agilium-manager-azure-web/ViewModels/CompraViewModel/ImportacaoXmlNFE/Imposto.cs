using System.Xml.Serialization;
using static agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE.EIcmsTipo;

namespace agilium.webapp.manager.mvc.ViewModels.CompraViewModel.ImportacaoXmlNFE
{

 

    public class Imposto
    {
        [XmlElement]
        public double vTotTrib { get; set; }
        [XmlElement(ElementName = "ICMS")]
        public Icms ICMS { get; set; }= new Icms();
        [XmlElement(ElementName = "COFINS")]
        public Cofins COFINS { get; set; }=new Cofins();
        [XmlElement(ElementName = "PIS")]
        public PIS Pis { get; set; }= new PIS();
    }

    public class Icms
    {
        [XmlElement]
        public ICMS00 ICMS00 { get; set; }=new ICMS00();
        [XmlElement]
        public ICMS10 ICMS10 { get; set; } = new ICMS10();
        [XmlElement]
        public ICMS60 ICMS60 { get; set; } = new ICMS60();
        [XmlElement]
        public ICMS70 ICMS70 { get; set; } = new ICMS70();
        [XmlElement]
        public ICMSSN102 ICMSSN102 { get; set; } = new ICMSSN102();
    }

    #region ICMS
    public class ICMS00
    {
        [XmlElement]
        public int orig { get; set; }
        [XmlElement]
        public int CST { get; set; }
        [XmlElement]
        public int modBC { get; set; }
        [XmlElement]
        public double vBC { get; set; }
        [XmlElement]
        public double pICMS { get; set; }
        [XmlElement]
        public double vICMS { get; set; }
    }

    public class ICMS10
    {
        [XmlElement(Order = 1)]
        public OrigemMercadoria orig { get; set; }

        /// <summary>
        ///     N12- Situação Tributária
        /// </summary>
        [XmlElement(Order = 2)]
        public Csticms CST { get; set; }

        /// <summary>
        ///     N13 - Modalidade de determinação da BC do ICMS
        /// </summary>
        [XmlElement(Order = 3)]
        public DeterminacaoBaseIcms modBC { get; set; }

        /// <summary>
        ///     N15 - Valor da BC do ICMS
        /// </summary>
        [XmlElement(Order = 4)]
        public decimal vBC
        {
            get;
            set;
        }

        /// <summary>
        ///     N16 - Alíquota do imposto
        /// </summary>
        [XmlElement(Order = 5)]
        public decimal pICMS
        {
            get;
            set;
        }

        /// <summary>
        ///     N17 - Valor do ICMS
        /// </summary>
        [XmlElement(Order = 6)]
        public decimal vICMS
        {
            get;
            set;
        }

        /// <summary>
        /// N17a - Valor da Base de Cálculo do FCP
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 7)]
        public decimal? vBCFCP
        {
            get;
            set;
        }

        /// <summary>
        /// N17b - Percentual do Fundo de Combate à Pobreza (FCP)
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 8)]
        public decimal? pFCP
        {
            get;
            set;
        }
      
        /// <summary>
        /// N17c - Valor do Fundo de Combate à Pobreza (FCP)
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 9)]
        public decimal? vFCP
        {
            get;
            set;
        }      

        /// <summary>
        ///     N18 - Modalidade de determinação da BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 10)]
        public DeterminacaoBaseIcmsSt modBCST { get; set; }

        /// <summary>
        ///     N19 - Percentual da margem de valor Adicionado do ICMS ST
        /// </summary>
        [XmlElement(Order = 11)]
        public decimal? pMVAST
        {
            get;
            set;
        }

        /// <summary>
        ///     N20 - Percentual da Redução de BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 12)]
        public decimal? pRedBCST
        {
            get;
            set;
        }

        /// <summary>
        ///     N21 - Valor da BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 13)]
        public decimal vBCST
        {
            get;
            set;
        }

        /// <summary>
        ///     N22 - Alíquota do imposto do ICMS ST
        /// </summary>
        [XmlElement(Order = 14)]
        public decimal pICMSST
        {
            get;
            set;
        }

        /// <summary>
        ///     N23 - Valor do ICMS ST
        /// </summary>
        [XmlElement(Order = 15)]
        public decimal vICMSST
        {
            get;
            set;
        }

        /// <summary>
        /// N23a - Valor da Base de Cálculo do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 16)]
        public decimal? vBCFCPST
        {
            get;
            set;
        }

        /// <summary>
        /// N23b - Percentual do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 17)]
        public decimal? pFCPST
        {
            get;
            set;
        }


        /// <summary>
        /// N23d - Valor do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 18)]
        public decimal? vFCPST
        {
            get;
            set;
        }
    }

    public class ICMS60
    {
        /// <summary>
        ///     N11 - Origem da Mercadoria
        /// </summary>
        [XmlElement(Order = 1)]
        public OrigemMercadoria orig { get; set; }

        /// <summary>
        ///     N12- Situação Tributária
        /// </summary>
        [XmlElement(Order = 2)]
        public Csticms CST { get; set; }

        /// <summary>
        ///     N26 - Valor da BC do ICMS ST retido
        /// </summary>
        [XmlElement(Order = 3)]
        public decimal? vBCSTRet { get; set; }
        public bool ShouldSerializevBCSTRet()
        {
            return vBCSTRet.HasValue;
        }

        /// <summary>
        ///     N26a - Alíquota suportada pelo Consumidor Final
        /// </summary>
        [XmlElement(Order = 4)]
        public decimal? pST { get; set; }
        
        [XmlElement(Order = 5)]
        public decimal? vICMSSubstituto { get; set; }
        
        /// <summary>
        ///     N27 - Valor do ICMS ST retido
        /// </summary>
        [XmlElement(Order = 6)]
        public decimal? vICMSSTRet { get; set; }


        /// <summary>
        /// N27a - Valor da Base de Cálculo do FCP retido anteriormente por ST 
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 7)]
        public decimal? vBCFCPSTRet { get; set; }


        /// <summary>
        /// N27b - Percentual do FCP retido anteriormente por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 8)]
        public decimal? pFCPSTRet { get; set; }


        /// <summary>
        /// N27d - Valor do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 9)]
        public decimal? vFCPSTRet { get; set; }
        
        /// <summary>
        ///     N34 - Percentual de redução da base de cálculo efetiva 
        /// </summary>
        [XmlElement(Order = 10)]
        public decimal? pRedBCEfet { get; set; }

        /// <summary>
        ///     N35 - Valor da base de cálculo efetiva 
        /// </summary>
        [XmlElement(Order = 11)]
        public decimal? vBCEfet { get; set; }

        /// <summary>
        ///     N36 - Alíquota do ICMS efetiva 
        /// </summary>
        [XmlElement(Order = 12)]
        public decimal? pICMSEfet { get; set; }

        /// <summary>
        ///     N37 - Valor do ICMS efetivo 
        /// </summary>
        [XmlElement(Order = 13)]
        public decimal? vICMSEfet { get; set; }


    }

    public class ICMS70
    {
        /// <summary>
        ///     N11 - Origem da Mercadoria
        /// </summary>
        [XmlElement(Order = 1)]
        public OrigemMercadoria orig { get; set; }

        /// <summary>
        ///     N12- Situação Tributária
        /// </summary>
        [XmlElement(Order = 2)]
        public Csticms CST { get; set; }

        /// <summary>
        ///     N13 - Modalidade de determinação da BC do ICMS
        /// </summary>
        [XmlElement(Order = 3)]
        public DeterminacaoBaseIcms modBC { get; set; }

        /// <summary>
        ///     N14 - Percentual de redução da BC
        /// </summary>
        [XmlElement(Order = 4)]
        public decimal pRedBC { get; set; }

        /// <summary>
        ///     N15 - Valor da BC do ICMS
        /// </summary>
        [XmlElement(Order = 5)]
        public decimal vBC { get; set; }

        /// <summary>
        ///     N16 - Alíquota do imposto
        /// </summary>
        [XmlElement(Order = 6)]
        public decimal pICMS { get; set; }

        /// <summary>
        ///     N17 - Valor do ICMS
        /// </summary>
        [XmlElement(Order = 7)]
        public decimal vICMS { get; set; }

        /// <summary>
        /// N17a - Valor da Base de Cálculo do FCP
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 8)]
        public decimal? vBCFCP { get; set; }


        /// <summary>
        /// N17b - Percentual do Fundo de Combate à Pobreza (FCP)
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 9)]
        public decimal? pFCP { get; set; }


        /// <summary>
        /// N17c - Valor do Fundo de Combate à Pobreza (FCP)
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 10)]
        public decimal? vFCP { get; set; }


        /// <summary>
        ///     N18 - Modalidade de determinação da BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 11)]
        public DeterminacaoBaseIcmsSt modBCST { get; set; }

        /// <summary>
        ///     N19 - Percentual da margem de valor Adicionado do ICMS ST
        /// </summary>
        [XmlElement(Order = 12)]
        public decimal? pMVAST { get; set; }
        /// <summary>
        ///     N20 - Percentual da Redução de BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 13)]
        public decimal? pRedBCST { get; set; }

        /// <summary>
        ///     N21 - Valor da BC do ICMS ST
        /// </summary>
        [XmlElement(Order = 14)]
        public decimal vBCST { get; set; }

        /// <summary>
        ///     N22 - Alíquota do imposto do ICMS ST
        /// </summary>
        [XmlElement(Order = 15)]
        public decimal pICMSST { get; set; }

        /// <summary>
        ///     N23 - Valor do ICMS ST
        /// </summary>
        [XmlElement(Order = 16)]
        public decimal vICMSST { get; set; }

        /// <summary>
        /// N23a - Valor da Base de Cálculo do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 17)]
        public decimal? vBCFCPST { get; set; }


        /// <summary>
        /// N23b - Percentual do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 18)]
        public decimal? pFCPST { get; set; }

        /// <summary>
        /// N23d - Valor do FCP retido por Substituição Tributária
        /// Versão 4.00
        /// </summary>
        [XmlElement(Order = 19)]
        public decimal? vFCPST { get; set; }
       
        /// <summary>
        ///     N27a - Valor do ICMS desonerado
        /// </summary>
        [XmlElement(Order = 20)]
        public decimal? vICMSDeson { get; set; }

        /// <summary>
        ///     N28 - Motivo da desoneração do ICMS
        /// </summary>
        [XmlElement(Order = 21)]
        public MotivoDesoneracaoIcms? motDesICMS { get; set; }
    }

    public class ICMSSN102
    {
        /// <summary>
        ///     N11 - Origem da Mercadoria
        /// </summary>
        [XmlElement(Order = 1)]
        public OrigemMercadoria orig { get; set; }

        /// <summary>
        ///     N12a - Código de Situação da Operação – Simples Nacional
        /// </summary>
        [XmlElement(Order = 2)]
        public Csosnicms CSOSN { get; set; }
    }
    #endregion

    #region PIS
    public class PIS 
    {
        [XmlElement(ElementName = "PISAliq")]
        public PISAliq PISAliq { get; set; } = new PISAliq();
    }
    public class PISAliq
    {
        [XmlElement]
        public string CST { get; set; }
        [XmlElement]
        public double vBC { get; set; }
        [XmlElement]
        public double pPIS { get; set; }
        [XmlElement]
        public double vPIS { get; set; }
    }
    #endregion

    #region Cofins

    public class Cofins
    {
        [XmlElement]
        public COFINSAliq COFINSAliq { get; set; } = new COFINSAliq();
    }

    public class COFINSAliq
    {
        [XmlElement]
        public string CST { get; set; }
        [XmlElement]
        public double vBC { get; set; }
        [XmlElement]
        public double pCOFINS { get; set; }
        [XmlElement]
        public double vCOFINS { get; set; }
    }
    #endregion
}
