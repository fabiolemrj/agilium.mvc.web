using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.PdvViewModel.CaixaReturnViewModel;
using agilium.api.business.Models.CustomReturn.PedidoViewModel;
using agilium.api.pdv.ViewModels;
using agilium.api.pdv.ViewModels.CaixaViewModel;
using agilium.api.pdv.ViewModels.ConfigViewModel;
using agilium.api.pdv.ViewModels.EmpresaViewModel;
using agilium.api.pdv.ViewModels.EnderecoViewModel;
using agilium.api.pdv.ViewModels.FuncionarioViewModel;
using agilium.api.pdv.ViewModels.MoedaViewModel;
using agilium.api.pdv.ViewModels.PedidoViewModel;
using agilium.api.pdv.ViewModels.ProdutoViewModel;
using agilium.api.pdv.ViewModels.TurnoViewModel;
using agilium.api.pdv.ViewModels.ClienteViewModel;
using AutoMapper;
using System;
using agilium.api.pdv.ViewModels.VendaViewModel;
using agilium.api.business.Models.CustomReturn.ProdutoReturnViewModel;

namespace agilium.api.pdv.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            #region Usuario
            CreateMap<Usuario, UsuarioFotoViewModel>()
               .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.ativo == "1" || src.ativo == "S" ? "Ativo" : "Inativo"))
              .ForMember(dest => dest.ImagemUpLoad, act => act.Ignore())
              .ForMember(dest => dest.ImagemConvertida, act => act.Ignore())
              .ForMember(dest => dest.idAspNetUser, opt => opt.MapFrom(src => src.idUserAspNet))
              .ReverseMap();

            CreateMap<Usuario, UsuarioPadrao>().ReverseMap()
                    .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => DateTime.Parse(src.dtnasc)))
                   .ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo == "1" || src.ativo == "S" ? "Ativo" : "Inativo"));
            #endregion

            #region Empresa
            CreateMap<Empresa, EmpresaViewModel>().ReverseMap();
            CreateMap<Empresa, EmpresaCreateViewModel>()
            .ForMember(origem => origem.ContatosEmpresa, opt => opt.MapFrom(src => src.ContatoEmpresas))
            .ForMember(origem => origem.ContatosEmpresa, opt => opt.MapFrom(src => src.ContatoEmpresas))
            .ReverseMap();

            CreateMap<Empresa, EmpresaSimplesViewModel>()
                .ForMember(dest => dest.NomeFantasia, opt => opt.MapFrom(src => src.NMFANTASIA))
                .ForMember(dest => dest.RazaoSocial, opt => opt.MapFrom(src => src.NMRZSOCIAL))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDEMPRESA))
                .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.NUCNPJ))
                .ReverseMap();
            #endregion

            #region Endereco
            CreateMap<Endereco, EnderecoIndexViewModel>()
                 .ForMember(dest => dest.Logradouro, opt => opt.MapFrom(src => src.Logradouro))
                .ReverseMap();
            CreateMap<Cep, CepViewModel>().ReverseMap();
            #endregion

            #region Turno
            //CreateMap<TurnoPreco, TurnoPrecoViewModel>()
            //    .ForMember(dest => dest.NumeroTurno, opt => opt.MapFrom(src => src.NUTURNO))
            //    .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
            //    .ForMember(dest => dest.Diferenca, opt => opt.MapFrom(src => src.TPDIFERENCA))
            //    .ForMember(dest => dest.TipoValor, opt => opt.MapFrom(src => src.TPVALOR))
            //    .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.NUVALOR))
            //    .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.NMUSUARIO))
            //    .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRCAD))
            //   .ReverseMap();

            CreateMap<Turno, TurnoViewModel>()
            .ForMember(dest => dest.NumeroTurno, opt => opt.MapFrom(src => src.NUTURNO))
            .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
            .ForMember(dest => dest.IDUSUARIOFIM, opt => opt.MapFrom(src => src.IDUSUARIOFIM))
            .ForMember(dest => dest.IDUSUARIOINI, opt => opt.MapFrom(src => src.IDUSUARIOINI))
            .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DTTURNO))
            .ForMember(dest => dest.DataFinal, opt => opt.MapFrom(src => src.DTHRFIM))
            .ForMember(dest => dest.DataInicial, opt => opt.MapFrom(src => src.DTHRINI))
               .ForMember(dest => dest.Obs, opt => opt.MapFrom(src => src.DSOBS))
           .ReverseMap();

            CreateMap<Turno, TurnoIndexViewModel>()
             .ForMember(dest => dest.NumeroTurno, opt => opt.MapFrom(src => src.NUTURNO))
             .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
             .ForMember(dest => dest.IDUSUARIOFIM, opt => opt.MapFrom(src => src.IDUSUARIOFIM))
             .ForMember(dest => dest.IDUSUARIOINI, opt => opt.MapFrom(src => src.IDUSUARIOINI))
             .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DTTURNO))
             .ForMember(dest => dest.DataFinal, opt => opt.MapFrom(src => src.DTHRFIM))
             .ForMember(dest => dest.DataInicial, opt => opt.MapFrom(src => src.DTHRINI))
             .ForMember(dest => dest.Obs, opt => opt.MapFrom(src => src.DSOBS))
             .ForMember(dest => dest.Empresa, act => act.Ignore())
             .ForMember(dest => dest.UsuarioFinal, act => act.Ignore())
             .ForMember(dest => dest.UsuarioInicial, act => act.Ignore())
            .ReverseMap();
            #endregion

            #region Caixa
            CreateMap<Caixa, CaixaViewModel>()
              .ForMember(dest => dest.IDFUNC, opt => opt.MapFrom(src => src.IDFUNC))
              .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
              .ForMember(dest => dest.IDPDV, opt => opt.MapFrom(src => src.IDPDV))
              .ForMember(dest => dest.Sequencial, opt => opt.MapFrom(src => src.SQCAIXA))
              .ForMember(dest => dest.DataAbertura, opt => opt.MapFrom(src => src.DTHRABT))
              .ForMember(dest => dest.DataFechamento, opt => opt.MapFrom(src => src.DTHRFECH))
              .ForMember(dest => dest.IDTURNO, opt => opt.MapFrom(src => src.IDTURNO))
              .ForMember(dest => dest.ValorAbertura, opt => opt.MapFrom(src => src.VLABT))
              .ForMember(dest => dest.ValorFechamento, opt => opt.MapFrom(src => src.VLFECH))
              .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCAIXA))
             .ReverseMap();

            CreateMap<Caixa, CaixaindexViewModel>()
                .ForMember(dest => dest.IDFUNC, opt => opt.MapFrom(src => src.IDFUNC))
                .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
                .ForMember(dest => dest.IDPDV, opt => opt.MapFrom(src => src.IDPDV))
                .ForMember(dest => dest.Sequencial, opt => opt.MapFrom(src => src.SQCAIXA))
                .ForMember(dest => dest.DataAbertura, opt => opt.MapFrom(src => src.DTHRABT))
                .ForMember(dest => dest.DataFechamento, opt => opt.MapFrom(src => src.DTHRFECH))
                .ForMember(dest => dest.IDTURNO, opt => opt.MapFrom(src => src.IDTURNO))
                .ForMember(dest => dest.ValorAbertura, opt => opt.MapFrom(src => src.VLABT))
                .ForMember(dest => dest.ValorFechamento, opt => opt.MapFrom(src => src.VLFECH))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCAIXA))
                .ForMember(dest => dest.Empresa, act => act.Ignore())
                .ForMember(dest => dest.Funcionario, act => act.Ignore())
                .ForMember(dest => dest.Turno, act => act.Ignore())
                .ForMember(dest => dest.PDV, act => act.Ignore())
               .ReverseMap();

            CreateMap<FecharCaixaInicializarViewModel, FecharCaixaInicializa>()
                     .ForMember(dest => dest.DataTurno, opt => opt.MapFrom(src => src.DataTurno.ToString("dd/MM/yyyy")))
                     .ForMember(dest => dest.DataAbertura, opt => opt.MapFrom(src => src.DataAbertura.ToString("dd/MM/yyyy HH:mm:ss")))
                     .ReverseMap();

            #endregion

            #region Config
            CreateMap<Config, ConfigIndexViewModel>().ReverseMap();
            CreateMap<PontoVenda, PontoVendaAssociacao>()
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSPDV))
                .ReverseMap();
            #endregion

            #region Pedido
                CreateMap<PedidoListaViewModel, PedidoViewModel>().ReverseMap();
                CreateMap<PedidoItemListaViewModel, PedidoItemViewModel>().ReverseMap();
                CreateMap<PedidoFormaPagamentoListaViewModel, PedidoFormaPagamentoViewModel>().ReverseMap();
                CreateMap<PedidosEstatisticasListaViewModel, PedidosEstatisticasViewModel>().ReverseMap();
                CreateMap<ClientePedidoCustomViewModel, ClientePedidoViewModel>().ReverseMap();
                CreateMap<Pedido, PedidoSalvarViewModel>().ReverseMap();
                CreateMap<PedidoItem, PedidoItemViewModel>().ReverseMap();
                CreateMap<Pedido, PedidoViewModel>().ReverseMap();
                CreateMap<PedidoSalvarCustomViewModel, PedidoSalvarViewModel>().ReverseMap();
                CreateMap<PedidoFuncionarioCustomViewModel, PedidoPorFuncionarioViewModel>().ReverseMap();
            #endregion

            #region Cliente
            CreateMap<Cliente, ClientePedidoViewModel>()
                     .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMCLIENTE))
                     .ForMember(dest => dest.dataCadastro, opt => opt.MapFrom(src => src.DTCAD))
                     .ReverseMap();
            #endregion

            #region Moeda
                CreateMap<Moeda, MoedaViewModel>()
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STMOEDA))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSMOEDA))
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDMOEDA))
                .ForMember(dest => dest.TipoDocFiscal, opt => opt.MapFrom(src => src.TPDOCFISCAL))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPMOEDA))
                .ForMember(dest => dest.PorcentTaxa, opt => opt.MapFrom(src => src.PCTAXA))
                .ForMember(dest => dest.SitucacaoTroco, opt => opt.MapFrom(src => src.STTROCO))
                .ForMember(dest => dest.TECLA_ATALHO, opt => opt.MapFrom(src => src.TECLA_ATALHO))
                .ForMember(dest => dest.COR_FONTE, opt => opt.MapFrom(src => src.COR_FONTE))
                .ForMember(dest => dest.COR_BOTAO, opt => opt.MapFrom(src => src.COR_BOTAO))
                .ReverseMap();
            #endregion

            #region Produto
            CreateMap<Produto, ProdutoViewModel>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDPRODUTO))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMPRODUTO))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STPRODUTO))
                .ForMember(dest => dest.QuantMinima, opt => opt.MapFrom(src => src.NUQTDMIN))
                .ForMember(dest => dest.PCIBPTEST, opt => opt.MapFrom(src => src.PCIBPTEST))
                .ForMember(dest => dest.STESTOQUE, opt => opt.MapFrom(src => src.STESTOQUE))
                .ForMember(dest => dest.AliquotaCofins, opt => opt.MapFrom(src => src.PCCOFINS_ALIQ))
                .ForMember(dest => dest.AliquotaICMS, opt => opt.MapFrom(src => src.PCICMS_ALIQ))
                .ForMember(dest => dest.AliquotaICMS_ST, opt => opt.MapFrom(src => src.PCICMSST_ALIQ))
                .ForMember(dest => dest.AliquotaIPI, opt => opt.MapFrom(src => src.PCIPI_ALIQ))
                .ForMember(dest => dest.AliquotaMargemValorAgregadoICMS_ST, opt => opt.MapFrom(src => src.PCICMSST_MVA))
                .ForMember(dest => dest.AliquotaPIS, opt => opt.MapFrom(src => src.PCPIS_ALIQ))
                .ForMember(dest => dest.DSICMS_CST, opt => opt.MapFrom(src => src.DSICMS_CST))
                //.ForMember(dest => dest.Categoria, opt => opt.MapFrom(src => src.CTPRODUTO))
                .ForMember(dest => dest.Categoria, opt => opt.MapFrom((src, dest) =>
                {
                    if (src.CTPRODUTO == "2")
                        return ECategoriaProduto.Composto;
                    if (src.CTPRODUTO == "3")
                        return ECategoriaProduto.Combo;
                    else if (src.CTPRODUTO == "4")
                        return ECategoriaProduto.Insumo;
                    else return ECategoriaProduto.Simples;
                }))
                .ForMember(dest => dest.CFOPVenda, opt => opt.MapFrom(src => src.NUCFOP))
                .ForMember(dest => dest.CodigoANP, opt => opt.MapFrom(src => src.CDANP))
                .ForMember(dest => dest.CodigoCest, opt => opt.MapFrom(src => src.CDCEST))
                .ForMember(dest => dest.CodigoNCM, opt => opt.MapFrom(src => src.CDNCM))
                .ForMember(dest => dest.CodigoSefaz, opt => opt.MapFrom(src => src.CDSEFAZ))
                .ForMember(dest => dest.CodigoServ, opt => opt.MapFrom(src => src.CDSERV))
                .ForMember(dest => dest.CodigoSituacaoTributariaCofins, opt => opt.MapFrom(src => src.DSCOFINS_CST))
                .ForMember(dest => dest.CodigoSituacaoTributariaIPI, opt => opt.MapFrom(src => src.DSIPI_CST))
                .ForMember(dest => dest.CodigoSituacaoTributariaPIS, opt => opt.MapFrom(src => src.DSPIS_CST))
                .ForMember(dest => dest.FLG_IFOOD, opt => opt.MapFrom(src => src.FLG_IFOOD))
                .ForMember(dest => dest.Preco, opt => opt.MapFrom(src => src.NUPRECO))
                .ForMember(dest => dest.ReducaoBaseCalculoICMS, opt => opt.MapFrom(src => src.PCICMS_REDUCBC))
                .ForMember(dest => dest.ReducaoBaseCalculoICMS_ST, opt => opt.MapFrom(src => src.PCICMSST_REDUCBC))
                .ForMember(dest => dest.ValorCustoMedio, opt => opt.MapFrom(src => src.VLCUSTOMEDIO))
                .ForMember(dest => dest.ValorUltimaCompra, opt => opt.MapFrom(src => src.VLULTIMACOMPRA))
                .ForMember(dest => dest.RelacaoCompraVenda, opt => opt.MapFrom(src => src.NURELACAO))
                .ForMember(dest => dest.UtilizaBalanca, opt => opt.MapFrom(src => src.STBALANCA))
                .ForMember(dest => dest.UnidadeVenda, opt => opt.MapFrom(src => src.UNVENDA))
                .ForMember(dest => dest.UnidadeCompra, opt => opt.MapFrom(src => src.UNCOMPRA))
                .ForMember(dest => dest.PCIBPTFED, opt => opt.MapFrom(src => src.PCIBPTFED))
                .ForMember(dest => dest.PCIBPTIMP, opt => opt.MapFrom(src => src.PCIBPTIMP))
                .ForMember(dest => dest.PCIBPTMUN, opt => opt.MapFrom(src => src.PCIBPTMUN))
                .ForMember(dest => dest.IDDEP, opt => opt.MapFrom(src => src.IDDEP))
                .ForMember(dest => dest.IDGRUPO, opt => opt.MapFrom(src => src.IDGRUPO))
                .ForMember(dest => dest.IDSUBGRUPO, opt => opt.MapFrom(src => src.IDSUBGRUPO))
                .ForMember(dest => dest.IDMARCA, opt => opt.MapFrom(src => src.IDMARCA))
                .ForMember(dest => dest.idEmpresa, opt => opt.MapFrom(src => src.idEmpresa))
                .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPPRODUTO))
                .ForMember(dest => dest.Volume, opt => opt.MapFrom(src => src.DSVOLUME))

                .ReverseMap();

            CreateMap<ProdutoPesqReturnViewModel, ProdutoPesqViewModel>().ReverseMap();
            #endregion

            #region Funcionario
            CreateMap<Funcionario, FuncionarioViewModel>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDFUNC))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMFUNC))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STFUNC))
                .ForMember(dest => dest.Noturno, opt => opt.MapFrom(src => src.NUTURNO))
                .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.NUCPF))
                .ForMember(dest => dest.Documento, opt => opt.MapFrom(src => src.NURG))
                .ForMember(dest => dest.DataAdmissao, opt => opt.MapFrom(src => src.DTADM))
                .ForMember(dest => dest.DataDemissao, opt => opt.MapFrom(src => src.DTDEM))
                .ForMember(dest => dest.DSRFID, opt => opt.MapFrom(src => src.DSRFID))
                .ForMember(dest => dest.Turno, opt => opt.MapFrom(src => src.STNOTURNO))
                  .ReverseMap();
            #endregion

            #region Cliente
            CreateMap<Cliente, ClienteViewModel>()
              .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDCLIENTE))
              .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCLIENTE))
              .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMCLIENTE))
              .ForMember(dest => dest.TipoPessoa, opt => opt.MapFrom(src => src.TPCLIENTE))
              .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.DTCAD))
              .ForMember(dest => dest.PublicaEmail, opt => opt.MapFrom(src => src.STPUBEMAIL))
              .ForMember(dest => dest.PublicaSMS, opt => opt.MapFrom(src => src.STPUBSMS))
              .ForMember(dest => dest.IDENDERECO, opt => opt.MapFrom(src => src.IDENDERECO))
              .ForMember(dest => dest.IDENDERECOCOB, opt => opt.MapFrom(src => src.IDENDERECOCOB))
              .ForMember(dest => dest.IDENDERECOFAT, opt => opt.MapFrom(src => src.IDENDERECOFAT))
              .ForMember(dest => dest.IDENDERECONTREGA, opt => opt.MapFrom(src => src.IDENDERECONTREGA))
              .ForMember(dest => dest.Endereco, act => act.Ignore())
              .ForMember(dest => dest.EnderecoCobranca, act => act.Ignore())
              .ForMember(dest => dest.EnderecoEntrega, act => act.Ignore())
              .ForMember(dest => dest.EnderecoFaturamento, act => act.Ignore())
              .ForMember(dest => dest.ClientePessoaFisica, act => act.Ignore())
              .ForMember(dest => dest.ClientePessoaJuridica, act => act.Ignore())
              .ReverseMap();

            CreateMap<ClientePF, ClientePFViewModel>()
              .ForMember(dest => dest.CPF, opt => opt.MapFrom(src => src.NUCPF))
              .ForMember(dest => dest.NumeroDocumento, opt => opt.MapFrom(src => src.NURG))
              .ForMember(dest => dest.DataNascimento, opt => opt.MapFrom(src => src.DTNASC))
              .ReverseMap();

            CreateMap<ClientePJ, ClientePJViewModel>()
              .ForMember(dest => dest.Cnpj, opt => opt.MapFrom(src => src.NUCNPJ))
              .ForMember(dest => dest.InscricaoEstadual, opt => opt.MapFrom(src => src.DSINSCREST))
              .ForMember(dest => dest.RazaoSocial, opt => opt.MapFrom(src => src.NMRZSOCIAL))
              .ReverseMap();

            CreateMap<ClientePreco, ClientePrecoViewModel>()
                 .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
                 .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
                 .ForMember(dest => dest.Diferenca, opt => opt.MapFrom(src => src.TPDIFERENCA))
                 .ForMember(dest => dest.TipoValor, opt => opt.MapFrom(src => src.TPVALOR))
                 .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.NUVALOR))
                 .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.NmUsuario))
                 .ForMember(dest => dest.Datahora, opt => opt.MapFrom(src => src.DTHRCAD))
                .ReverseMap();

            CreateMap<Cliente, ClienteBasicoViewModel>()
           .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMCLIENTE))
           .ReverseMap();
            #endregion

            #region Venda
            CreateMap<Venda, VendaIncluirViewModel>()
               .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
               .ForMember(dest => dest.IDCAIXA, opt => opt.MapFrom(src => src.IDCAIXA))
               .ForMember(dest => dest.NumeroNF, opt => opt.MapFrom(src => src.NUNF))
               .ForMember(dest => dest.InformacaoComplementar, opt => opt.MapFrom(src => src.DSINFCOMPL))
               .ForMember(dest => dest.SerieNF, opt => opt.MapFrom(src => src.DSSERIE))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STVENDA))
               .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLVENDA))
               .ForMember(dest => dest.ChaveAcesso, opt => opt.MapFrom(src => src.DSCHAVEACESSO))
               .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.NUCPFCNPJ))
               .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DTHRVENDA))
               .ForMember(dest => dest.Emissao, opt => opt.MapFrom(src => src.STEMISSAO))
               .ForMember(dest => dest.Sequencial, opt => opt.MapFrom(src => src.SQVENDA))
               .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.TPDOC))
               .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRES))
               .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
               .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTAL))
               .ForMember(dest => dest.ValorTotalIbptEst, opt => opt.MapFrom(src => src.VLTOTIBPTEST))
               .ForMember(dest => dest.ValorTotalIbptFed, opt => opt.MapFrom(src => src.VLTOTIBPTFED))
               .ForMember(dest => dest.ValorTotalIbptImp, opt => opt.MapFrom(src => src.VLTOTIBPTIMP))
               .ForMember(dest => dest.ValorTotalIbptMun, opt => opt.MapFrom(src => src.VLTOTIBPTMUN))
               .ForMember(dest => dest.CaixaNome, act => act.Ignore())
               .ForMember(dest => dest.ClienteNome, act => act.Ignore())
               .ForMember(dest => dest.PDVNome, act => act.Ignore())
               .ForMember(dest => dest.FuncionarioNome, act => act.Ignore())
          .ReverseMap();

            CreateMap<Venda, VendaViewModel>()
             .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
             .ForMember(dest => dest.IDCAIXA, opt => opt.MapFrom(src => src.IDCAIXA))
             .ForMember(dest => dest.NumeroNF, opt => opt.MapFrom(src => src.NUNF))
             .ForMember(dest => dest.InformacaoComplementar, opt => opt.MapFrom(src => src.DSINFCOMPL))
             .ForMember(dest => dest.SerieNF, opt => opt.MapFrom(src => src.DSSERIE))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STVENDA))
             .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLVENDA))
             .ForMember(dest => dest.ChaveAcesso, opt => opt.MapFrom(src => src.DSCHAVEACESSO))
             .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.NUCPFCNPJ))
             .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DTHRVENDA))
             .ForMember(dest => dest.Emissao, opt => opt.MapFrom(src => src.STEMISSAO))
             .ForMember(dest => dest.Sequencial, opt => opt.MapFrom(src => src.SQVENDA))
             .ForMember(dest => dest.TipoDocumento, opt => opt.MapFrom(src => src.TPDOC))
             .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRES))
             .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
             .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTAL))
             .ForMember(dest => dest.ValorTotalIbptEst, opt => opt.MapFrom(src => src.VLTOTIBPTEST))
             .ForMember(dest => dest.ValorTotalIbptFed, opt => opt.MapFrom(src => src.VLTOTIBPTFED))
             .ForMember(dest => dest.ValorTotalIbptImp, opt => opt.MapFrom(src => src.VLTOTIBPTIMP))
             .ForMember(dest => dest.ValorTotalIbptMun, opt => opt.MapFrom(src => src.VLTOTIBPTMUN))
             .ForMember(dest => dest.CaixaNome, act => act.Ignore())
             .ForMember(dest => dest.ClienteNome, act => act.Ignore())
             .ForMember(dest => dest.PDVNome, act => act.Ignore())
             .ForMember(dest => dest.FuncionarioNome, act => act.Ignore())
            .ReverseMap();

            CreateMap<VendaItem, VendaItemViewModel>()
               .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
               .ForMember(dest => dest.IDVENDA, opt => opt.MapFrom(src => src.IDVENDA))
               .ForMember(dest => dest.PCIBPTEST, opt => opt.MapFrom(src => src.PCIBPTEST))
               .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.NUQTD))
               .ForMember(dest => dest.PCIBPTFED, opt => opt.MapFrom(src => src.PCIBPTFED))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STITEM))
               .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLITEM))
               .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRES))
               .ForMember(dest => dest.ValorCustoMedio, opt => opt.MapFrom(src => src.VLCUSTOMEDIO))
               .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
               .ForMember(dest => dest.PCIBPTIMP, opt => opt.MapFrom(src => src.PCIBPTIMP))
               .ForMember(dest => dest.PCIBPTMUN, opt => opt.MapFrom(src => src.PCIBPTMUN))
               .ForMember(dest => dest.Sequencial, opt => opt.MapFrom(src => src.SQITEM))
               .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTAL))
               .ForMember(dest => dest.ValorUnitario, opt => opt.MapFrom(src => src.VLUNIT))
               .ForMember(dest => dest.cancelado, opt => opt.MapFrom(src => src.STITEM == ESituacaoItemVenda.Cancelado ? true : false ))
               .ForMember(dest => dest.ProdutoNome, act => act.Ignore())
                .ForMember(dest => dest.VendaNome, act => act.Ignore())
                .ForMember(dest => dest.CodigoProduto, act => act.Ignore())
                .ForMember(dest => dest.SituacaoProduto, act => act.Ignore())
              .ReverseMap();

            CreateMap<VendaMoeda, VendaMoedaViewModel>()
                .ForMember(dest => dest.IDMOEDA, opt => opt.MapFrom(src => src.IDMOEDA))
                .ForMember(dest => dest.IDVENDA, opt => opt.MapFrom(src => src.IDVENDA))
                .ForMember(dest => dest.IDVALE, opt => opt.MapFrom(src => src.IDVALE))
                .ForMember(dest => dest.NSU, opt => opt.MapFrom(src => src.NSU))
                .ForMember(dest => dest.ValorTroco, opt => opt.MapFrom(src => src.VLTROCO))
                .ForMember(dest => dest.NumeroParcela, opt => opt.MapFrom(src => src.NUPARCELAS))
                .ForMember(dest => dest.ValorPago, opt => opt.MapFrom(src => src.VLPAGO))
                .ForMember(dest => dest.MoedaNome, act => act.Ignore())
                .ForMember(dest => dest.VendaNome, act => act.Ignore())
               .ReverseMap();

            #endregion

        }
    }
}
