using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn;
using agilium.api.infra.Mappings;
using agilium.api.manager.ViewModels;
using agilium.api.manager.ViewModels.CaixaViewModel;
using agilium.api.manager.ViewModels.CaManagerViewModel;
using agilium.api.manager.ViewModels.CategoriaFinancViewModel;
using agilium.api.manager.ViewModels.ClienteViewModel;
using agilium.api.manager.ViewModels.CompraViewModel;
using agilium.api.manager.ViewModels.ConfigViewModel;
using agilium.api.manager.ViewModels.ContatoViewModel;
using agilium.api.manager.ViewModels.ContaViewModel;
using agilium.api.manager.ViewModels.ControleAcessoViewModel;
using agilium.api.manager.ViewModels.DevolucaoViewModel;
using agilium.api.manager.ViewModels.EmpresaViewModel;
using agilium.api.manager.ViewModels.EnderecoViewModel;
using agilium.api.manager.ViewModels.EstoqueViewModel;
using agilium.api.manager.ViewModels.FormaPagamentoViewModel;
using agilium.api.manager.ViewModels.FornecedorViewModel;
using agilium.api.manager.ViewModels.FuncionarioViewModel;
using agilium.api.manager.ViewModels.ImpostoViewModel;
using agilium.api.manager.ViewModels.InventarioViewModel;
using agilium.api.manager.ViewModels.LogSistemaViewModel;
using agilium.api.manager.ViewModels.MoedaViewModel;
using agilium.api.manager.ViewModels.NotaFiscalViewModel;
using agilium.api.manager.ViewModels.PerdaViewModel;
using agilium.api.manager.ViewModels.PlanoContaViewModel;
using agilium.api.manager.ViewModels.PontoVendaViewModel;
using agilium.api.manager.ViewModels.ProdutoVewModel;
using agilium.api.manager.ViewModels.SiteMercadoViewModel;
using agilium.api.manager.ViewModels.TurnoViewModel;
using agilium.api.manager.ViewModels.UnidadeViewModel;
using agilium.api.manager.ViewModels.ValeViewModel;
using agilium.api.manager.ViewModels.VendaViewModel;
using AutoMapper;
using Microsoft.CodeAnalysis;
using Microsoft.CodeAnalysis.Operations;
using System;
using System.Linq.Expressions;

namespace agilium.api.manager.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            #region Controle Acessso / Usuarios
            CreateMap<Usuario, UsuarioViewModel>().ReverseMap();
            CreateMap<business.Models.PagedResult<Usuario>, business.Models.PagedResult<UsuarioViewModel>>().ReverseMap();

            CreateMap<Usuario, UsuarioPadrao>()
                     .ForMember(dest => dest.dtnasc, opt => opt.MapFrom(src => src.DataCadastro.ToString()))
                     .ForMember(dest => dest.idperfilManager, opt => opt.MapFrom(src => src.id_perfil))
                      .ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo)).ReverseMap();
                    //.ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo == "1" || src.ativo == "S" ? "Ativo" : "Inativo"));

            CreateMap<business.Models.PagedResult<Usuario>, business.Models.PagedResult<UsuarioPadrao>>().ReverseMap()
                .ForMember(dest => dest.List, opt => opt.MapFrom(src => src.List));

            CreateMap<Usuario, ListaUsuarioViewModel>()
                .ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo == "1" ? "Ativo": "Inativo"))
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.nome))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CpfUsuario,  opt => opt.MapFrom(x=>x.cpf));

            CreateMap<ObjetoClaim, UserClaimViewModel>().ReverseMap();

            CreateMap<Usuario, UsuarioFotoViewModel>()
                 .ForMember(dest => dest.Ativo, opt => opt.MapFrom(src => src.ativo == "1" || src.ativo == "S" ? "Ativo" : "Inativo"))
                .ForMember( dest => dest.ImagemUpLoad, act => act.Ignore())
                .ForMember(dest => dest.ImagemConvertida, act => act.Ignore())
                .ForMember(dest => dest.idAspNetUser, opt => opt.MapFrom( src => src.idUserAspNet))
                .ReverseMap();

            CreateMap<EmpresaAuth, EmpresaUsuarioViewModel>()
               // .ForMember(dest => dest.NomeEmpresa, opt => opt.MapFrom(src => src.Empresa.NMRZSOCIAL))
                  .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA.ToString()))
                  .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO.ToString()))
                .ReverseMap();

            CreateMap<CaAreaManager, CaAreaManagerViewModel>().ReverseMap();
            CreateMap<CaPerfiManager, CaPerfilManagerViewModel>().ReverseMap();

            CreateMap<CaPermissaoManager, CaAreaManagerSalvarViewModel>().ReverseMap();
            #endregion

            #region Log
            CreateMap<LogSistema, LogSistemaViewModel>()
                      .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.id_log))
                      .ForMember(dest => dest.Maquina, opt => opt.MapFrom(src => src.maquina))
                      .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.data_log))
                      .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.descr))
                      .ForMember(dest => dest.AcaoOriem, opt => opt.MapFrom(src => src.controle))
                      .ForMember(dest => dest.TelaOrigem, opt => opt.MapFrom(src => src.tela))
                      .ForMember(dest => dest.so, opt => opt.MapFrom(src => src.so))
                      .ForMember(dest => dest.Hora, opt => opt.MapFrom(src => src.hora_log))
                      .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.usuario))
                .ReverseMap();
            #endregion

            #region Controle Acesso
            CreateMap<CaPerfil, PerfilIndexViewModel>()
           .ReverseMap();
            CreateMap<CaPerfil, CreateEditPerfilViewModel>()
          .ReverseMap();
            CreateMap<CaPerfil, PerfilDeleteViewModel>()
          .ReverseMap();
            CreateMap<CaPermissaoItem, CreateEditPermissaoItemViewModel>()
                .ReverseMap();
            CreateMap<CaPermissaoItem, PermissaoItemIndexViewModel>()
                .ReverseMap();
            CreateMap<CaModelo, CreateModeloItemViewModel>()
                 .ForMember(dest => dest.idPerfil, opt => opt.MapFrom(x => x.idPerfil))
                 .ForMember(dest => dest.idPermissao, opt => opt.MapFrom(x => x.idPermissao))
                 .ForMember(dest => dest.selecaoIncluir, opt => opt.MapFrom(src => src.Incluir == "S" || src.Incluir == "A" ? true : false))
                 .ForMember(dest => dest.selecaoAlterar, opt => opt.MapFrom(src => src.Alterar == "S" || src.Alterar == "A" ? true : false))
                 .ForMember(dest => dest.selecaoExcluir, opt => opt.MapFrom(src => src.Excluir == "S" || src.Excluir == "A" ? true : false))
                 .ForMember(dest => dest.selecaoRelatorio, opt => opt.MapFrom(src => src.Relatorio == "S" || src.Relatorio == "A" ? true : false))
                .ReverseMap();
            CreateMap<CaModelo, CreateModeloViewModel>()
          .ReverseMap();
            #endregion

            #region Empresa
            CreateMap<Empresa, EmpresaViewModel>().ReverseMap();
                CreateMap<Empresa, EmpresaCreateViewModel>()
                .ForMember(origem => origem.ContatosEmpresa, opt => opt.MapFrom(src => src.ContatoEmpresas))
                .ForMember(origem => origem.ContatosEmpresa, opt => opt.MapFrom(src => src.ContatoEmpresas))
                .ReverseMap();
            #endregion

            #region Endereco
                CreateMap<Endereco, EnderecoIndexViewModel>().ReverseMap();
            CreateMap<Cep, CepViewModel>().ReverseMap();
            #endregion

            #region Contato
            CreateMap<Contato, ContatoIndexViewModel>().ReverseMap();
            CreateMap<ContatoEmpresa, ContatoEmpresaViewModel>()
               .ForMember(origem => origem.Contato, opt => opt.MapFrom(src => src.Contato))
               //.ForMember(origem => origem.Empresa, opt => opt.MapFrom(src => src.Empresa))
               .ReverseMap();

            CreateMap<FornecedorContato, FornecedorContatoViewModel>()
          .ForMember(origem => origem.Contato, opt => opt.MapFrom(src => src.Contato))
          .ForMember(origem => origem.IDFORN, opt => opt.MapFrom(src => src.IDFORN))
          .ReverseMap();

            CreateMap<ClienteContato, ClienteContatoViewModel>()
            .ForMember(origem => origem.Contato, opt => opt.MapFrom(src => src.Contato))
            .ForMember(origem => origem.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
            .ReverseMap();
            #endregion

            #region Unidade
            CreateMap<Unidade, UnidadeIndexViewModel>().ReverseMap();
            #endregion

            #region Config
            CreateMap<Config, ConfigIndexViewModel>().ReverseMap();
            CreateMap<ConfigImagem, ConfigImagemViewModel>().ReverseMap();
            #endregion

            #region CategoriaFinanceira
            CreateMap<CategoriaFinanceira, CategoriaFinancViewModel>().ReverseMap();
            #endregion

            #region Produto

            CreateMap<ProdutoFoto, ProdutoFotoViewModel>()
                 .ForMember(dest => dest.Foto, act => act.Ignore())
            .ReverseMap();

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
                .ForMember(dest => dest.Empresas, act => act.Ignore())
                .ForMember(dest => dest.Cfops, act => act.Ignore())
                .ForMember(dest => dest.Csts, act => act.Ignore())
                .ForMember(dest => dest.Cests, act => act.Ignore())
                .ReverseMap();

            CreateMap<ProdutoDepartamento, ProdutoDepartamentoViewModel>()
                .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDDEP))
                .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMDEP))
                .ForMember(dest => dest.situacao, opt => opt.MapFrom(src => src.STDEP))
                .ForMember(dest => dest.Empresas, act => act.Ignore())
                .ReverseMap();
            CreateMap<ProdutoMarca, ProdutoMarcaViewModel>()
              .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDMARCA))
              .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMMARCA))
              .ForMember(dest => dest.situacao, opt => opt.MapFrom(src => src.STMARCA))
              .ForMember(dest => dest.Empresas, act => act.Ignore())
              .ReverseMap();
            CreateMap<GrupoProduto, GrupoProdutoViewModel>()
             .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDGRUPO))
             .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.Nome))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.StAtivo))
             .ForMember(dest => dest.Empresas, act => act.Ignore())
             .ReverseMap();
            CreateMap<SubGrupoProduto, SubGrupoViewModel>()
             .ForMember(dest => dest.Nome, opt => opt.MapFrom(src => src.NMSUBGRUPO))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STATIVO))
             .ForMember(dest => dest.NomeGrupo, act => act.Ignore())
             .ReverseMap();

            CreateMap<ProdutoCodigoBarra, ProdutoCodigoBarraViewModel>()
                .ForMember(dest => dest.CDBARRA, opt => opt.MapFrom(src => src.CDBARRA))
                .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
             .ReverseMap();

            CreateMap<ProdutoPreco, ProdutoPrecoViewModel>()
                .ReverseMap();


            #endregion

            #region Devolucao
            CreateMap<MotivoDevolucao, MotivoDevolucaoViewModel>()
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSMOTDEV))
               .ForMember(dest => dest.situacao, opt => opt.MapFrom(src => src.STMOTDEV))
               .ForMember(dest => dest.Empresas, act => act.Ignore())
               .ReverseMap();

            CreateMap<Devolucao, DevolucaoViewModel>()
             .ForMember(dest => dest.IDVENDA, opt => opt.MapFrom(src => src.IDVENDA))
             .ForMember(dest => dest.IDVALE, opt => opt.MapFrom(src => src.IDVALE))
             .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDDEV))
             .ForMember(dest => dest.IDMOTDEV, opt => opt.MapFrom(src => src.IDMOTDEV))
             .ForMember(dest => dest.Observacao, opt => opt.MapFrom(src => src.DSOBSDEV))
             .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRDEV))
             .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
                    .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STDEV))
             .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTALDEV))
             .ForMember(dest => dest.EmpresaNome, act => act.Ignore())
             .ForMember(dest => dest.ValeNome, act => act.Ignore())
             .ForMember(dest => dest.MotivoDevolucaoNome, act => act.Ignore())
             .ForMember(dest => dest.ClienteNome, act => act.Ignore())
             .ForMember(dest => dest.Itens, act => act.Ignore())
             .ForMember(dest => dest.VendaNome, act => act.Ignore())
             .ForMember(dest => dest.DevolucaoItens, act => act.Ignore())
               .ReverseMap();

            CreateMap<DevolucaoItem, DevolucaoItemViewModel>()
            .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.NUQTD))
            .ForMember(dest => dest.IDDEV, opt => opt.MapFrom(src => src.IDDEV))
            .ForMember(dest => dest.IDVENDA_ITEM, opt => opt.MapFrom(src => src.IDVENDA_ITEM))
            .ForMember(dest => dest.ValorItem, opt => opt.MapFrom(src => src.VLITEM))
            .ForMember(dest => dest.DevolucaoNome, act => act.Ignore())
            .ForMember(dest => dest.VendaItemNome, act => act.Ignore())
             .ForMember(dest => dest.ProdutoNome, act => act.Ignore())
              .ForMember(dest => dest.ValorItemVenda, act => act.Ignore())
               .ForMember(dest => dest.SequencialVenda, act => act.Ignore())
            .ReverseMap();

            CreateMap<DevolucaoItemVendaCustom, DevolucaoItemVendaViewModel>()
                .ReverseMap();
            #endregion

            #region Estoque
            CreateMap<Estoque, EstoqueViewModel>()
              .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STESTOQUE))
              .ForMember(dest => dest.Empresas, act => act.Ignore())
              .ReverseMap();

            CreateMap<EstoqueProduto, EstoqueProdutoViewModel>()
                .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.NUQTD))
                .ReverseMap();

            CreateMap<EstoqueHistorico, EstoqueHistoricoViewModel>()
                  .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRHST))
                  .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
                  .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.QTDHST))
                  .ForMember(dest => dest.IDITEM, opt => opt.MapFrom(src => src.IDITEM))
                  .ForMember(dest => dest.IDESTOQUE, opt => opt.MapFrom(src => src.IDESTOQUE))
                  .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSHST))
                  .ForMember(dest => dest.IDLANC, opt => opt.MapFrom(src => src.IDLANC))
                  .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.NMUSUARIO))
                  .ForMember(dest => dest.TipoHistorico, opt => opt.MapFrom(src => src.TPHST))
                .ReverseMap();
            #endregion

            #region Fornecedor
            CreateMap<Fornecedor, FornecedorViewModel>()
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STFORNEC))
               .ForMember(dest => dest.NomeFantasia, opt => opt.MapFrom(src => src.NMFANTASIA))
               .ForMember(dest => dest.RazaoSocial, opt => opt.MapFrom(src => src.NMRZSOCIAL))
               .ForMember(dest => dest.TipoPessoa, opt => opt.MapFrom(src => src.TPPESSOA))
                     .ForMember(dest => dest.TipoPessoa, opt => opt.MapFrom(src => src.TPPESSOA == "J" ? ETipoPessoa.J : ETipoPessoa.F))
              // .ForMember(dest => dest.TipoFiscal, opt => opt.MapFrom(src => src.TPFISCAL))
               .ForMember(dest => dest.CpfCnpj, opt => opt.MapFrom(src => src.NUCPFCNPJ))
               .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDFORN))
               .ForMember(dest => dest.InscricaoEstdualMunicipal, opt => opt.MapFrom(src => src.DSINSCR))
                 .ForMember(dest => dest.TipoFiscal, opt => opt.MapFrom(src => src.TPFISCAL))
                 .ForMember(dest => dest.Endereco, act => act.Ignore())
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
                //.ForMember(dest => dest.IDENDERECO, act => act.Ignore())
                 .ForMember(dest => dest.Empresas, act => act.Ignore())
                //  .ForMember(dest => dest.Usuario, act => act.Ignore())
                  .ReverseMap();
            #endregion

            #region Moeda
            CreateMap<Moeda, MoedaViewModel>()
             .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDMOEDA))
             .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSMOEDA))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STMOEDA))
             .ForMember(dest => dest.SitucacaoTroco, opt => opt.MapFrom(src => src.STTROCO))
             .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPMOEDA))
             .ForMember(dest => dest.TipoDocFiscal, opt => opt.MapFrom(src => src.TPDOCFISCAL))
             .ForMember(dest => dest.PorcentTaxa, opt => opt.MapFrom(src => src.PCTAXA))
             .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
             .ForMember(dest => dest.COR_BOTAO, opt => opt.MapFrom(src => src.COR_BOTAO))
             .ForMember(dest => dest.COR_FONTE, opt => opt.MapFrom(src => src.COR_FONTE))
             .ForMember(dest => dest.TECLA_ATALHO, opt => opt.MapFrom(src => src.TECLA_ATALHO))
              //.ForMember(dest => dest.IDENDERECO, act => act.Ignore())
              .ForMember(dest => dest.Empresas, act => act.Ignore())
               //  .ForMember(dest => dest.Usuario, act => act.Ignore())
               .ReverseMap();
            #endregion

            #region PDV
            CreateMap<PontoVenda, PontoVendaViewModel>()
               .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDPDV))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSPDV))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STPDV))
               .ForMember(dest => dest.NomeMaquina, opt => opt.MapFrom(src => src.NMMAQUINA))
               .ForMember(dest => dest.CaminhoCertificadoDigital, opt => opt.MapFrom(src => src.DSCAMINHO_CERT))
               .ForMember(dest => dest.SenhaCertificadoDigital, opt => opt.MapFrom(src => src.DSSENHA_CERT))
               .ForMember(dest => dest.PortaImpressora, opt => opt.MapFrom(src => src.DSPORTAIMPRESSORA))
               .ForMember(dest => dest.NUBAUDRATEBAL, opt => opt.MapFrom(src => src.NUBAUDRATEBAL))
               .ForMember(dest => dest.CDHANDSHAKEBAL, opt => opt.MapFrom(src => src.CDHANDSHAKEBAL))
               .ForMember(dest => dest.CDPARITYBAL, opt => opt.MapFrom(src => src.CDPARITYBAL))
               .ForMember(dest => dest.CDMODELOBAL, opt => opt.MapFrom(src => src.CDMODELOBAL))
               .ForMember(dest => dest.CDSERIALSTOPBITBAL, opt => opt.MapFrom(src => src.CDSERIALSTOPBITBAL))
               .ForMember(dest => dest.DSPORTABAL, opt => opt.MapFrom(src => src.DSPORTABAL))
               .ForMember(dest => dest.Estoques, act => act.Ignore())
               .ForMember(dest => dest.Empresas, act => act.Ignore())
                 //  .ForMember(dest => dest.Usuario, act => act.Ignore())
             .ReverseMap();
            #endregion

            #region Impostos
            CreateMap<Cst, CstViewModel>().ReverseMap();
            CreateMap<Csosn, CsosnViewModel>().ReverseMap();
            CreateMap<CestNcm, CestViewModel>().ReverseMap();
            CreateMap<Ncm, NcmViewModel>().ReverseMap();
            CreateMap<Ibpt, IbptViewModel>().ReverseMap();
            CreateMap<Cfop, CfopViewModel>().ReverseMap();
            #endregion

            #region Turno
            CreateMap<TurnoPreco, TurnoPrecoViewModel>()
                .ForMember(dest => dest.NumeroTurno, opt => opt.MapFrom(src => src.NUTURNO))
                .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
                .ForMember(dest => dest.Diferenca, opt => opt.MapFrom(src => src.TPDIFERENCA))
                .ForMember(dest => dest.TipoValor, opt => opt.MapFrom(src => src.TPVALOR))
                .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.NUVALOR))
                .ForMember(dest => dest.Usuario, opt => opt.MapFrom(src => src.NMUSUARIO))
                .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRCAD))
               .ReverseMap();

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

            #region Plano Conta
            CreateMap<PlanoConta, PlanoContaViewModel>()
                  .ForMember(dest => dest.IDCONTAPAI, opt => opt.MapFrom(src => src.IDCONTAPAI))
                  .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
                  .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDCONTA))
                  .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSCONTA))
                  .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPCONTA))
                  .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCONTA))
                 .ReverseMap();

            CreateMap<PlanoContaSaldo, PlanoContaSaldoViewModel>()
                .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
                .ForMember(dest => dest.AnoMesReferencia, opt => opt.MapFrom(src => src.NUANOMESREF))
                .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRATU))
                .ForMember(dest => dest.ValorSaldo, opt => opt.MapFrom(src => src.VLSALDO))
                .ReverseMap();

            CreateMap<PlanoContaLancamento, PlanoContaLancamentoViewModel>()
             .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
             .ForMember(dest => dest.AnoMesReferencia, opt => opt.MapFrom(src => src.NUANOMESREF))
             .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTCAD))
             .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLLANC))
              .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STLANC))
               .ForMember(dest => dest.DataReferencia, opt => opt.MapFrom(src => src.DTREF))
               .ForMember(dest => dest.DescricaoLancamento, opt => opt.MapFrom(src => src.DSLANC))
               .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPLANC))
             .ReverseMap();
            #endregion

            #region Conta
            CreateMap<ContaPagar, ContaPagarViewModel>()
               .ForMember(dest => dest.IDCONTAPAI, opt => opt.MapFrom(src => src.IDCONTAPAI))
               .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
               .ForMember(dest => dest.IDCATEG_FINANC, opt => opt.MapFrom(src => src.IDCATEG_FINANC))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DESCR))
               .ForMember(dest => dest.TipoConta, opt => opt.MapFrom(src => src.TPCONTA))
               .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
               .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
               .ForMember(dest => dest.IDFORNEC, opt => opt.MapFrom(src => src.IDFORNEC))
               .ForMember(dest => dest.IDLANC, opt => opt.MapFrom(src => src.IDLANC))
               .ForMember(dest => dest.DataNotaFiscal, opt => opt.MapFrom(src => src.DTNF))
               .ForMember(dest => dest.NumeroNotaFiscal, opt => opt.MapFrom(src => src.NUMNF))
               .ForMember(dest => dest.DatCadastro, opt => opt.MapFrom(src => src.DTCAD))
               .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DTVENC))
               .ForMember(dest => dest.OBS, opt => opt.MapFrom(src => src.OBS))
               .ForMember(dest => dest.ParcelaInicial, opt => opt.MapFrom(src => src.PARCINI))
               .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRESC))
               .ForMember(dest => dest.ValorConta, opt => opt.MapFrom(src => src.VLCONTA))
               .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCONTA))
              .ReverseMap();

            CreateMap<ContaPagar, ContaPagarViewModelIndex>()
               .ForMember(dest => dest.IDCONTAPAI, opt => opt.MapFrom(src => src.IDCONTAPAI))
               .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
               .ForMember(dest => dest.IDCATEG_FINANC, opt => opt.MapFrom(src => src.IDCATEG_FINANC))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DESCR))
               .ForMember(dest => dest.TipoConta, opt => opt.MapFrom(src => src.TPCONTA))
               .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
               .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
               .ForMember(dest => dest.IDFORNEC, opt => opt.MapFrom(src => src.IDFORNEC))
               .ForMember(dest => dest.IDLANC, opt => opt.MapFrom(src => src.IDLANC))
               .ForMember(dest => dest.DataNotaFiscal, opt => opt.MapFrom(src => src.DTNF))
               .ForMember(dest => dest.NumeroNotaFiscal, opt => opt.MapFrom(src => src.NUMNF))
               .ForMember(dest => dest.DatCadastro, opt => opt.MapFrom(src => src.DTCAD))
               .ForMember(dest => dest.OBS, opt => opt.MapFrom(src => src.OBS))
               .ForMember(dest => dest.ParcelaInicial, opt => opt.MapFrom(src => src.PARCINI))
               .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRESC))
               .ForMember(dest => dest.ValorConta, opt => opt.MapFrom(src => src.VLCONTA))
               .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
               .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DTVENC))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCONTA))
               .ForMember(dest => dest.CategoriaFinanceira, act => act.Ignore())
               .ForMember(dest => dest.Conta, act => act.Ignore())
                .ForMember(dest => dest.Fornecedor, act => act.Ignore())
                .ForMember(dest => dest.Usuario, act => act.Ignore())
              .ReverseMap();

            CreateMap<ContaReceber, ContaReceberViewModelIndex>()
               .ForMember(dest => dest.IDCONTAPAI, opt => opt.MapFrom(src => src.IDCONTAPAI))
               .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
               .ForMember(dest => dest.IDCATEG_FINANC, opt => opt.MapFrom(src => src.IDCATEG_FINANC))
               .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DESCR))
               .ForMember(dest => dest.TipoConta, opt => opt.MapFrom(src => src.TPCONTA))
               .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
               .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
               .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
               .ForMember(dest => dest.IDLANC, opt => opt.MapFrom(src => src.IDLANC))
               .ForMember(dest => dest.DataNotaFiscal, opt => opt.MapFrom(src => src.DTNF))
               .ForMember(dest => dest.NumeroNotaFiscal, opt => opt.MapFrom(src => src.NUMNF))
               .ForMember(dest => dest.DatCadastro, opt => opt.MapFrom(src => src.DTCAD))
               .ForMember(dest => dest.OBS, opt => opt.MapFrom(src => src.OBS))
               .ForMember(dest => dest.ParcelaInicial, opt => opt.MapFrom(src => src.PARCINI))
               .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRES))
               .ForMember(dest => dest.ValorConta, opt => opt.MapFrom(src => src.VLCONTA))
               .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCONTA))
               .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DTVENC))
               .ForMember(dest => dest.CategoriaFinanceira, act => act.Ignore())
               .ForMember(dest => dest.Conta, act => act.Ignore())
                .ForMember(dest => dest.Cliente, act => act.Ignore())
                .ForMember(dest => dest.Usuario, act => act.Ignore())
              .ReverseMap();

            CreateMap<ContaReceber, ContaReceberViewModel>()
                .ForMember(dest => dest.IDCONTAPAI, opt => opt.MapFrom(src => src.IDCONTAPAI))
                .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
                .ForMember(dest => dest.IDCATEG_FINANC, opt => opt.MapFrom(src => src.IDCATEG_FINANC))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DESCR))
                .ForMember(dest => dest.TipoConta, opt => opt.MapFrom(src => src.TPCONTA))
                .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
                .ForMember(dest => dest.IDCONTA, opt => opt.MapFrom(src => src.IDCONTA))
                .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
                .ForMember(dest => dest.IDLANC, opt => opt.MapFrom(src => src.IDLANC))
                .ForMember(dest => dest.DataNotaFiscal, opt => opt.MapFrom(src => src.DTNF))
                .ForMember(dest => dest.NumeroNotaFiscal, opt => opt.MapFrom(src => src.NUMNF))
                .ForMember(dest => dest.DatCadastro, opt => opt.MapFrom(src => src.DTCAD))
                        .ForMember(dest => dest.DataVencimento, opt => opt.MapFrom(src => src.DTVENC))
                .ForMember(dest => dest.OBS, opt => opt.MapFrom(src => src.OBS))
                .ForMember(dest => dest.ParcelaInicial, opt => opt.MapFrom(src => src.PARCINI))
                .ForMember(dest => dest.ValorAcrescimo, opt => opt.MapFrom(src => src.VLACRES))
                .ForMember(dest => dest.ValorConta, opt => opt.MapFrom(src => src.VLCONTA))
                .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESC))
                .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCONTA))
                .ReverseMap();

            CreateMap<ContaPagarViewModel, ContaPagarViewModelIndex>().ReverseMap();
            #endregion

            #region Nota Fiscal Inutil
            CreateMap<NotaFiscalInutil, NotaFiscalViewModel>()
           .ForMember(dest => dest.Protocolo, opt => opt.MapFrom(src => src.DSPROTOCOLO))
           .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
           .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDNFINUTIL))
           .ForMember(dest => dest.Motivo, opt => opt.MapFrom(src => src.DSMOTIVO))
           .ForMember(dest => dest.Serie, opt => opt.MapFrom(src => src.DSSERIE))
           .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STINUTIL))
           .ForMember(dest => dest.Ano, opt => opt.MapFrom(src => src.NUANO))
           .ForMember(dest => dest.NumeroInicial, opt => opt.MapFrom(src => src.NUNFINI))
           .ForMember(dest => dest.NumeroFinal, opt => opt.MapFrom(src => src.NUNFFIM))
           .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DTHRINUTIL))
           .ForMember(dest => dest.Modelo, opt => opt.MapFrom(src => src.DSMODELO))
           .ForMember(dest => dest.XML, opt => opt.MapFrom(src => src.DSXML))
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

            CreateMap<CaixaMovimento, CaixaMovimentoViewModel>()
             .ForMember(dest => dest.IDCAIXA, opt => opt.MapFrom(src => src.IDCAIXA))
             .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPMOV))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STMOV))
             .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSMOV))
             .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLMOV))
             .ForMember(dest => dest.Caixa, act => act.Ignore())
             .ReverseMap();

            CreateMap<CaixaMoeda, CaixaMoedaViewModel>()
             .ForMember(dest => dest.IDCAIXA, opt => opt.MapFrom(src => src.IDCAIXA))
             .ForMember(dest => dest.IDMOEDA, opt => opt.MapFrom(src => src.IDMOEDA))
             .ForMember(dest => dest.IDUSUARIOCORRECAO, opt => opt.MapFrom(src => src.IDUSUARIOCORRECAO))
             .ForMember(dest => dest.DataCorrecao, opt => opt.MapFrom(src => src.DTHRCORRECAO))
             .ForMember(dest => dest.ValorCorrecao, opt => opt.MapFrom(src => src.VLMOEDACORRECAO))
             .ForMember(dest => dest.ValorOriginal, opt => opt.MapFrom(src => src.VLMOEDAORIGINAL))
             .ForMember(dest => dest.CaixaNome, act => act.Ignore())
             .ForMember(dest => dest.MoedaNome, act => act.Ignore())
             .ReverseMap();

            #endregion

            #region Vale
            CreateMap<Vale, ValeViewModel>()
               .ForMember(dest => dest.IDCLIENTE, opt => opt.MapFrom(src => src.IDCLIENTE))
               .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
               .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDVALE))
               .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRVALE))
               .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPVALE))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STVALE))
               .ForMember(dest => dest.Valor, opt => opt.MapFrom(src => src.VLVALE))
               .ForMember(dest => dest.CodigoBarra, opt => opt.MapFrom(src => src.CDBARRA))
               .ForMember(dest => dest.ClienteNome, act => act.Ignore())
               .ForMember(dest => dest.EmpresaNome, act => act.Ignore())
              .ReverseMap();
            #endregion

            #region Venda
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

                CreateMap<VendaEspelho, VendaEspelhoViewModel>()
                .ForMember(dest => dest.IDVENDA, opt => opt.MapFrom(src => src.IDVENDA))
                .ForMember(dest => dest.EspelhoVenda, opt => opt.MapFrom(src => src.DSESPELHO))
                .ForMember(dest => dest.SequencialVenda, act => act.Ignore())
                  .ReverseMap();
            #endregion

            #region Perda
             CreateMap<Perda, PerdaViewModel>()
               .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
               .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
               .ForMember(dest => dest.IDESTOQUE, opt => opt.MapFrom(src => src.IDESTOQUE))
               .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO))
               .ForMember(dest => dest.IDESTOQUEHST, opt => opt.MapFrom(src => src.IDESTOQUEHST))
               .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDPERDA))
               .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRPERDA))
               .ForMember(dest => dest.Tipo, opt => opt.MapFrom(src => src.TPPERDA))
               .ForMember(dest => dest.Movimento, opt => opt.MapFrom(src => src.TPMOV))
               .ForMember(dest => dest.ValorCustoMedio, opt => opt.MapFrom(src => src.VLCUSTOMEDIO))
               .ForMember(dest => dest.Observacao, opt => opt.MapFrom(src => src.DSOBS))
               .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.NUQTDPERDA))
               .ForMember(dest => dest.EstoqueNome, act => act.Ignore())
               .ForMember(dest => dest.EmpresaNome, act => act.Ignore())
               .ForMember(dest => dest.ProdutoNome, act => act.Ignore())
               .ForMember(dest => dest.EstoqueHistoricoNome, act => act.Ignore())
               .ForMember(dest => dest.UsuarioNome, act => act.Ignore())
              .ReverseMap();
            #endregion

            #region Compra
            CreateMap<Compra, CompraViewModel>()
            .ForMember(dest => dest.IDFORN, opt => opt.MapFrom(src => src.IDFORN))
            .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
            .ForMember(dest => dest.IDTURNO, opt => opt.MapFrom(src => src.IDTURNO))
            .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDCOMPRA))
            .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STCOMPRA))
            .ForMember(dest => dest.SerieNF, opt => opt.MapFrom(src => src.DSSERIENF))
            .ForMember(dest => dest.DataCadastro, opt => opt.MapFrom(src => src.DTCAD))
            .ForMember(dest => dest.NumeroNF, opt => opt.MapFrom(src => src.NUNF))
            .ForMember(dest => dest.DataNF, opt => opt.MapFrom(src => src.DTNF))
            .ForMember(dest => dest.DataCompra, opt => opt.MapFrom(src => src.DTCOMPRA))
            .ForMember(dest => dest.ValorIcmsSub, opt => opt.MapFrom(src => src.VLICMSSUB))
            .ForMember(dest => dest.Observacao, opt => opt.MapFrom(src => src.DSOBS))
            .ForMember(dest => dest.Importada, opt => opt.MapFrom(src => src.STIMPORTADA))
            .ForMember(dest => dest.NumeroCFOP, opt => opt.MapFrom(src => src.NUCFOP))
            .ForMember(dest => dest.TipoComprovante, opt => opt.MapFrom(src => src.TPCOMPROVANTE))
            .ForMember(dest => dest.ValorBaseCalculoIcms, opt => opt.MapFrom(src => src.VLBSCALCICMS))
            .ForMember(dest => dest.ValorBaseCalculoSub, opt => opt.MapFrom(src => src.VLBSCALCSUB))
            .ForMember(dest => dest.ValorDesconto, opt => opt.MapFrom(src => src.VLDESCONTO))
            .ForMember(dest => dest.ValorFrete, opt => opt.MapFrom(src => src.VLFRETE))
            .ForMember(dest => dest.ValorIcms, opt => opt.MapFrom(src => src.VLICMS))
            .ForMember(dest => dest.ValorIcmsRetido, opt => opt.MapFrom(src => src.VLICMSRETIDO))
            .ForMember(dest => dest.ValorIpi, opt => opt.MapFrom(src => src.VLIPI))
            .ForMember(dest => dest.ValorIsencao, opt => opt.MapFrom(src => src.VLISENCAO))
            .ForMember(dest => dest.ValorOutros, opt => opt.MapFrom(src => src.VLOUTROS))
            .ForMember(dest => dest.ValorSeguro, opt => opt.MapFrom(src => src.VLSEGURO))
            .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTAL))
            .ForMember(dest => dest.ValorTotalProduto, opt => opt.MapFrom(src => src.VLTOTPROD))
            .ForMember(dest => dest.ChaveNFE, opt => opt.MapFrom(src => src.DSCHAVENFE))

            .ForMember(dest => dest.NomeFornecedor, act => act.Ignore())
            .ForMember(dest => dest.NomeTurno, act => act.Ignore())
            .ReverseMap();

            CreateMap<CompraItem, CompraItemViewModel>()
              .ForMember(dest => dest.IDCOMPRA, opt => opt.MapFrom(src => src.IDCOMPRA))
              .ForMember(dest => dest.IDESTOQUE, opt => opt.MapFrom(src => src.IDESTOQUE))
              .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
              .ForMember(dest => dest.ValorAliquotaCofins, opt => opt.MapFrom(src => src.VLALIQCOFINS))
              .ForMember(dest => dest.ValorAliquotaIcms, opt => opt.MapFrom(src => src.VLALIQICMS))
              .ForMember(dest => dest.ValorAliquotaIpi, opt => opt.MapFrom(src => src.VLALIQIPI))
              .ForMember(dest => dest.ValorAliquotaPis, opt => opt.MapFrom(src => src.VLALIQPIS))
              .ForMember(dest => dest.ValorBaseCalculoCofins, opt => opt.MapFrom(src => src.VLBSCALCCOFINS))
              .ForMember(dest => dest.ValorBaseCalculoIcms, opt => opt.MapFrom(src => src.VLBSCALCICMS))
              .ForMember(dest => dest.ValorBaseCalculoIpi, opt => opt.MapFrom(src => src.VLBSCALCIPI))
              .ForMember(dest => dest.ValorBaseCalculoPis, opt => opt.MapFrom(src => src.VLBSCALCPIS))
              .ForMember(dest => dest.ValorBaseRetido, opt => opt.MapFrom(src => src.VLBSRET))
              .ForMember(dest => dest.ValorCofins, opt => opt.MapFrom(src => src.VLCOFINS))
              .ForMember(dest => dest.ValorIcms, opt => opt.MapFrom(src => src.VLICMS))
              .ForMember(dest => dest.ValorIpi, opt => opt.MapFrom(src => src.VLIPI))
              .ForMember(dest => dest.ValorNovoPrecoVenda, opt => opt.MapFrom(src => src.VLNOVOPRECOVENDA))
              .ForMember(dest => dest.ValorOUTROS, opt => opt.MapFrom(src => src.VLOUTROS))
              .ForMember(dest => dest.ValorPis, opt => opt.MapFrom(src => src.VLPIS))
              .ForMember(dest => dest.ValorTotal, opt => opt.MapFrom(src => src.VLTOTAL))
              .ForMember(dest => dest.ValorUnitario, opt => opt.MapFrom(src => src.VLUNIT))
              .ForMember(dest => dest.Quantidade, opt => opt.MapFrom(src => src.NUQTD))
              .ForMember(dest => dest.CodigoCEST, opt => opt.MapFrom(src => src.CDCEST))
              .ForMember(dest => dest.CodigoCstCofins, opt => opt.MapFrom(src => src.CDCSTCOFINS))
              .ForMember(dest => dest.CodigoCstIcms, opt => opt.MapFrom(src => src.CDCSTICMS))
              .ForMember(dest => dest.CodigoCstIpi, opt => opt.MapFrom(src => src.CDCSTIPI))
              .ForMember(dest => dest.CodigoCstPis, opt => opt.MapFrom(src => src.CDCSTPIS))
              .ForMember(dest => dest.CodigoEan, opt => opt.MapFrom(src => src.CDEAN))
              .ForMember(dest => dest.CodigoNCM, opt => opt.MapFrom(src => src.CDNCM))
              .ForMember(dest => dest.CodigoProdutoFornecedor, opt => opt.MapFrom(src => src.CDPRODFORN))
              .ForMember(dest => dest.PorcentagemIcmsRetido, opt => opt.MapFrom(src => src.PCICMSRET))
              .ForMember(dest => dest.PorcentagemReducao, opt => opt.MapFrom(src => src.PCREDUCAO))
              .ForMember(dest => dest.DataValidade, opt => opt.MapFrom(src => src.DTVALIDADE))
              .ForMember(dest => dest.SGUN, opt => opt.MapFrom(src => src.SGUN))
              .ForMember(dest => dest.NumeroCFOP, opt => opt.MapFrom(src => src.NUCFOP))
              .ForMember(dest => dest.Relacao, opt => opt.MapFrom(src => src.NURELACAO))
              .ForMember(dest => dest.DescricaoProdutoCompra, opt => opt.MapFrom(src => src.DSPRODUTO))
              .ForMember(dest => dest.NomeProduto, act => act.Ignore())
              .ForMember(dest => dest.NomeCompra, act => act.Ignore())
              .ForMember(dest => dest.NomeEstoque, act => act.Ignore())
               .ForMember(dest => dest.CodigoProduto, act => act.Ignore())
              .ReverseMap();

            CreateMap<CompraFiscal, CompraFiscalViewModel>()
            .ForMember(dest => dest.IDCOMPRA, opt => opt.MapFrom(src => src.IDCOMPRA))
            .ForMember(dest => dest.TipoManifesto, opt => opt.MapFrom(src => src.STMANIFESTO))
            .ForMember(dest => dest.Xml, opt => opt.MapFrom(src => src.DSXML))
            .ReverseMap();
            #endregion

            #region Inventario
            CreateMap<Inventario, InventarioViewModel>()
             .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
             .ForMember(dest => dest.TipoAnalise, opt => opt.MapFrom(src => src.TPANALISE))
             .ForMember(dest => dest.IDESTOQUE, opt => opt.MapFrom(src => src.IDESTOQUE))
             .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STINVENT))
             .ForMember(dest => dest.Observacao, opt => opt.MapFrom(src => src.DSOBS))
             .ForMember(dest => dest.Codigo, opt => opt.MapFrom(src => src.CDINVENT))
             .ForMember(dest => dest.Data, opt => opt.MapFrom(src => src.DTINVENT))
              .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSINVENT))
             .ForMember(dest => dest.NomeEstoque, act => act.Ignore())
             .ReverseMap();

            CreateMap<InventarioItem, InventarioItemViewModel>()
             .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
             .ForMember(dest => dest.IDINVENT, opt => opt.MapFrom(src => src.IDINVENT))
             .ForMember(dest => dest.IDPERDA, opt => opt.MapFrom(src => src.IDPERDA))
             .ForMember(dest => dest.IDUSUARIOANALISE, opt => opt.MapFrom(src => src.IDUSUARIOANALISE))
             .ForMember(dest => dest.QuantidadeEstoque, opt => opt.MapFrom(src => src.NUQTDESTOQUE))
             .ForMember(dest => dest.QuantidadeAnalise, opt => opt.MapFrom(src => src.NUQTDANALISE))
             .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRANALISE))
             .ForMember(dest => dest.ValorCustoMedio, opt => opt.MapFrom(src => src.VLCUSTOMEDIO))
             .ForMember(dest => dest.NomePerda, act => act.Ignore())
             .ForMember(dest => dest.NomeProduto, act => act.Ignore())
             .ForMember(dest => dest.NomeUsuarioAnalise, act => act.Ignore())
              .ForMember(dest => dest.CodigoProduto, act => act.Ignore())
             .ReverseMap();

            #endregion

            #region SiteMercado
                CreateMap<ProdutoSiteMercado, ProdutoSiteMercadoViewModel>()
              .ForMember(dest => dest.IDPRODUTO, opt => opt.MapFrom(src => src.IDPRODUTO))
              .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
              .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSPROD))
              .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STDISPSITE))
              .ForMember(dest => dest.Validade, opt => opt.MapFrom(src => src.STVALIDADEPROXIMA))
              .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRATU))
              .ForMember(dest => dest.QuantidadeAtacado, opt => opt.MapFrom(src => src.NUQTDATACADO))
              .ForMember(dest => dest.ValorAtacado, opt => opt.MapFrom(src => src.VLATACADO))
              .ForMember(dest => dest.ValorCompra, opt => opt.MapFrom(src => src.VLCOMPRA))
              .ForMember(dest => dest.ValorPromocao, opt => opt.MapFrom(src => src.VLPROMOCAO))
              .ForMember(dest => dest.ProdutoPdv, act => act.Ignore())
              .ReverseMap();

                CreateMap<MoedaSiteMercado, MoedaSiteMercadoViewModel>()
                .ForMember(dest => dest.IDMOEDA, opt => opt.MapFrom(src => src.IDMOEDA))
                .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA))
                .ForMember(dest => dest.IDSM, opt => opt.MapFrom(src => src.IDSM))
                .ForMember(dest => dest.DataHora, opt => opt.MapFrom(src => src.DTHRCAD))
                .ForMember(dest => dest.MoedaPdv, act => act.Ignore())
                .ReverseMap();
            #endregion

            #region forma pagamento
            CreateMap<FormaPagamento, FormaPagamentoViewModel>()
               .ForMember(dest => dest.IDEmpresa, opt => opt.MapFrom(src => src.IDEmpresa))
               .ForMember(dest => dest.Situacao, opt => opt.MapFrom(src => src.STFormaPagamento))
                .ForMember(dest => dest.Descricao, opt => opt.MapFrom(src => src.DSFormaPagamento))
               .ReverseMap();
            #endregion
        }
    }
}
