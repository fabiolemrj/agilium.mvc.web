using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.pdv.Controllers;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using System;
using agilium.api.pdv.ViewModels;
using Microsoft.AspNetCore.Http;

namespace agilium.api.pdv.V1
{
    [Route("api/v{version:apiVersion}/usuarios")]
    [ApiVersion("1.0")]
    [Authorize]
    public class UsuarioController : MainController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly IUsuarioFotoEntityService _usuarioFotoServiceService;

        public UsuarioController(INotificador notificador, IUser appUser, IConfiguration configuration, IUtilDapperRepository utilDapperRepository, ILogService logService,
            IUsuarioService usuarioService, IMapper mapper, IUsuarioFotoEntityService usuarioFotoServiceService) : base(notificador, appUser, configuration, utilDapperRepository, logService)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
            _usuarioFotoServiceService = usuarioFotoServiceService;
        }

        [HttpPost("atualizar-foto-usuario")]
        [RequestSizeLimit(600000000)]

        public async Task<ActionResult<bool>> AtualizarFoto([FromBody] UsuarioFotoViewModel fotoViewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            var usuarioAtualizacao = await _usuarioService.ObterPorUsuarioPorId(Convert.ToInt64(fotoViewModel.id));

            fotoViewModel.ImagemUpLoad = await ConverterToIFormFile(await ConverterToIFormFile(fotoViewModel.ImagemConvertida), fotoViewModel.NomeArquivo, fotoViewModel.NomeArquivoExtensao);
            var imgPrefixo = Guid.NewGuid() + "_";

            fotoViewModel.Foto = imgPrefixo + fotoViewModel.ImagemUpLoad.FileName;
            fotoViewModel.Ativo = "1";
            usuarioAtualizacao.AutalizarFoto(fotoViewModel.Foto);

            var resultado = await _usuarioService.AtualizarFoto(usuarioAtualizacao);
            if (!resultado)
            {
                NotificarErro("Erro ao tentar atualizar foto");
                return CustomResponse();
            }

            var imagemConvertida = await ConverterToIFormFile(fotoViewModel.ImagemConvertida);

            var usuarioFotoExistente = await ObterFotoUsuarioPorId(usuarioAtualizacao.idUserAspNet);

            if (usuarioFotoExistente != null)
            {
                var usuarioFoto = new UsuarioFotoEntity(usuarioFotoExistente.Id, usuarioAtualizacao.idUserAspNet, fotoViewModel.Foto, imagemConvertida, DateTime.Now, usuarioAtualizacao.Id);

                await _usuarioFotoServiceService.Atualizar(usuarioFoto);
            }
            else
            {
                var usuarioFoto = new UsuarioFotoEntity(usuarioAtualizacao.idUserAspNet, fotoViewModel.Foto, imagemConvertida, DateTime.Now, usuarioAtualizacao.Id);

                await _usuarioFotoServiceService.Adicionar(usuarioFoto);
            }

            return CustomResponse(resultado);
        }

        [HttpGet("obter-foto-usuario-por-id/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioFotoViewModel>> ObterFotoUsuario(string id)
        {
            var objeto = await ObterFotoUsuarioEntityPorId(id);

            return CustomResponse(objeto);
        }

        [HttpGet("obter-por-userId/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioPadrao>> ObterPorUserId(string id)
        {

            // long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<UsuarioPadrao>(await _usuarioService.ObterPorUsuarioAspNetPorId(id));
            return CustomResponse(objeto);
        }

        // [ClaimsAuthorize("Usuario", "Atualizar")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioPadrao>> Atualizar(string id, [FromBody] UsuarioPadrao viewModel)
        {

            if (id != viewModel.id)
            {
                NotificarErro("O id informado não é o mesmo que foi passado na query");
                return CustomResponse(viewModel);
            }

            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _usuarioService.Atualizar(_mapper.Map<Usuario>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Usuario", "Atualizar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Usuario", "Atualizar", null);
            return CustomResponse(viewModel);
        }


        #region private
        private async Task<UsuarioFotoViewModel> ObterFotoUsuarioEntityPorId(string id)
        {
            try
            {
                var usuarioFoto = await _usuarioFotoServiceService.ObterPorUsuarioFotoPorId(id);

                var objeto = new UsuarioFotoViewModel();

                if (usuarioFoto == null)
                {
                    var usuario = await _usuarioService.ObterPorUsuarioAspNetPorId(id);
                    if (usuario != null)
                    {
                        objeto.id = usuario.Id.ToString();
                        objeto.idAspNetUser = usuario.idUserAspNet;
                        objeto.Ativo = usuario.ativo;
                    }
                    else
                    {
                        objeto.idAspNetUser = id;
                    }
                    objeto.DataCadastro = DateTime.Now;
                    return objeto;
                }

                objeto.ImagemConvertida = String.Format("data:image/png;base64,{0}", await ConverterByteToBase64(usuarioFoto.Imagem));
                objeto.idAspNetUser = usuarioFoto.IdUsuarioAspNet;
                objeto.DataCadastro = usuarioFoto.DataCadastro;
                objeto.Ativo = "1";
                objeto.NomeArquivo = usuarioFoto.NomeArquivo;
                objeto.id = usuarioFoto.IdUsuario.ToString();

                //  
                return objeto;
            }
            catch (Exception ex)
            {
                return null;
            }

        }

        private async Task<UsuarioFotoEntity> ObterFotoUsuarioPorId(string id)
        {
            return await _usuarioFotoServiceService.ObterPorUsuarioFotoPorId(id);
        }
        #endregion
    }
}
