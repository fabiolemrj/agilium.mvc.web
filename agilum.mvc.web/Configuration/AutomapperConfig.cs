using agilium.api.business.Enums;
using agilium.api.business.Models;
using agilum.mvc.web.ViewModels.Contato;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.EmpresaUsuario;
using agilum.mvc.web.ViewModels.Endereco;
using agilum.mvc.web.ViewModels.Produtos;
using agilum.mvc.web.ViewModels.UnidadeViewModel;
using agilum.mvc.web.ViewModels.Usuarios;
using AutoMapper;

namespace agilum.mvc.web.Configuration
{
    public class AutomapperConfig : Profile
    {
        public AutomapperConfig()
        {
            #region Controle Acessso / Usuarios
            CreateMap<Usuario, UsuarioViewModel>().ReverseMap();
            CreateMap<agilium.api.business.Models.PagedResult<Usuario>, agilium.api.business.Models.PagedResult<UsuarioViewModel>>().ReverseMap();

            CreateMap<Usuario, UsuarioPadrao>()
                     .ForMember(dest => dest.dtnasc, opt => opt.MapFrom(src => src.DataCadastro.ToString()))
                     .ForMember(dest => dest.idperfilManager, opt => opt.MapFrom(src => src.id_perfil))
                      .ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo)).ReverseMap();
            //.ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo == "1" || src.ativo == "S" ? "Ativo" : "Inativo"));
            CreateMap<agilium.api.business.Models.PagedResult<Usuario>, agilium.api.business.Models.PagedResult<UsuarioPadrao>>().ReverseMap()
              .ForMember(dest => dest.List, opt => opt.MapFrom(src => src.List));

            CreateMap<Usuario, ListaUsuarioViewModel>()
                .ForMember(dest => dest.ativo, opt => opt.MapFrom(src => src.ativo == "1" ? "Ativo" : "Inativo"))
                .ForMember(dest => dest.NomeUsuario, opt => opt.MapFrom(src => src.nome))
                .ForMember(dest => dest.id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.CpfUsuario, opt => opt.MapFrom(x => x.cpf));


            CreateMap<EmpresaAuth, EmpresaUsuarioViewModel>()
                  // .ForMember(dest => dest.NomeEmpresa, opt => opt.MapFrom(src => src.Empresa.NMRZSOCIAL))
                  .ForMember(dest => dest.IDEMPRESA, opt => opt.MapFrom(src => src.IDEMPRESA.ToString()))
                  .ForMember(dest => dest.IDUSUARIO, opt => opt.MapFrom(src => src.IDUSUARIO.ToString()))
                .ReverseMap();
                     
            
            #endregion

            #region Unidade
            CreateMap<Unidade, UnidadeIndexViewModel>().ReverseMap();
            #endregion

            #region Empresa
            CreateMap<Empresa, EmpresaViewModel>().ReverseMap();

            CreateMap<Empresa, EmpresaCreateViewModel>()
                .ForMember(origem => origem.STMICROEMPRESA, opt => opt.MapFrom(src => src.STMICROEMPRESA))
                .ForMember(origem => origem.IDENDERECO, opt => opt.MapFrom(src => src.IDENDERECO))
                .ForMember(origem => origem.CDEMPRESA, opt => opt.MapFrom(src => src.CDEMPRESA))
                .ForMember(origem => origem.NMRZSOCIAL, opt => opt.MapFrom(src => src.NMRZSOCIAL))
                .ForMember(origem => origem.NMFANTASIA, opt => opt.MapFrom(src => src.NMFANTASIA))
                .ForMember(origem => origem.DSINSCREST, opt => opt.MapFrom(src => src.DSINSCREST))
                .ForMember(origem => origem.DSINSCRESTVINC, opt => opt.MapFrom(src => src.DSINSCRESTVINC))
                .ForMember(origem => origem.DSINSCRMUN, opt => opt.MapFrom(src => src.DSINSCRMUN))
                .ForMember(origem => origem.NMDISTRIBUIDORA, opt => opt.MapFrom(src => src.NMDISTRIBUIDORA))
                .ForMember(origem => origem.NUREGJUNTACOM, opt => opt.MapFrom(src => src.NUREGJUNTACOM))
                .ForMember(origem => origem.NUCAPARM, opt => opt.MapFrom(src => src.NUCAPARM))
                .ForMember(origem => origem.STLUCROPRESUMIDO, opt => opt.MapFrom(src => src.STLUCROPRESUMIDO))
                .ForMember(origem => origem.TPEMPRESA, opt => opt.MapFrom(src => src.TPEMPRESA))
                .ForMember(origem => origem.CRT, opt => opt.MapFrom(src => src.CRT))
                .ForMember(origem => origem.IDCSC, opt => opt.MapFrom(src => src.IDCSC))
                .ForMember(origem => origem.CSC, opt => opt.MapFrom(src => src.CSC))
                .ForMember(origem => origem.NUCNAE, opt => opt.MapFrom(src => src.NUCNAE))
                .ForMember(origem => origem.IDCSC_HOMOL, opt => opt.MapFrom(src => src.IDCSC_HOMOL))
                .ForMember(origem => origem.IDLOJA_SITEMARCADO, opt => opt.MapFrom(src => src.IDLOJA_SITEMARCADO))
                .ForMember(origem => origem.CLIENTID_SITEMERCADO, opt => opt.MapFrom(src => src.CLIENTID_SITEMERCADO))
                .ForMember(origem => origem.CLIENTSECRET_SITEMERCADO, opt => opt.MapFrom(src => src.CLIENTSECRET_SITEMERCADO))
            .ForMember(origem => origem.ContatosEmpresa, opt => opt.MapFrom(src => src.ContatoEmpresas))
            .ForMember(origem => origem.ContatosEmpresa, opt => opt.MapFrom(src => src.ContatoEmpresas))
             .ForMember(origem => origem.Endereco, opt => opt.MapFrom(src => src.Endereco))
            .ReverseMap();
            #endregion

            #region Contato
            CreateMap<Contato, ContatoIndexViewModel>().ReverseMap();
            CreateMap<ContatoEmpresa, ContatoEmpresaViewModel>()
               .ForMember(origem => origem.Contato, opt => opt.MapFrom(src => src.Contato))
               //.ForMember(origem => origem.Empresa, opt => opt.MapFrom(src => src.Empresa))
               .ReverseMap();

            #endregion

            #region Endereco
            CreateMap<Endereco, EnderecoIndexViewModel>()
                    .ForMember(origem => origem.NumeroCep, opt => opt.MapFrom(src => src.Cep))
                    .ForMember(origem => origem.Numero, opt => opt.MapFrom(src => src.Numero))
                    .ForMember(origem => origem.Uf, opt => opt.MapFrom(src => src.Uf))
                    .ForMember(origem => origem.Cidade, opt => opt.MapFrom(src => src.Cidade))
                    .ForMember(origem => origem.Bairro, opt => opt.MapFrom(src => src.Bairro))
                    .ForMember(origem => origem.Logradouro, opt => opt.MapFrom(src => src.Logradouro))
                    .ForMember(origem => origem.Complemento, opt => opt.MapFrom(src => src.Complemento))
                    .ForMember(origem => origem.Ibge, opt => opt.MapFrom(src => src.Ibge))
                    .ForMember(origem => origem.Pais, opt => opt.MapFrom(src => src.Pais))
                    .ForMember(origem => origem.Id, opt => opt.MapFrom(src => src.Id))
                    .ReverseMap();
            CreateMap<Cep, CepViewModel>().ReverseMap();
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
        }
    }
}
