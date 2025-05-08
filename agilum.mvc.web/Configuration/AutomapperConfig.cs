using agilium.api.business.Models;
using agilum.mvc.web.ViewModels.Contato;
using agilum.mvc.web.ViewModels.Empresa;
using agilum.mvc.web.ViewModels.EmpresaUsuario;
using agilum.mvc.web.ViewModels.Endereco;
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
        }
    }
}
