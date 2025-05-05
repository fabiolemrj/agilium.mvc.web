using agilium.api.business.Models;
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
            #endregion

            #region Unidade
            CreateMap<Unidade, UnidadeIndexViewModel>().ReverseMap();
            #endregion
        }
    }
}
