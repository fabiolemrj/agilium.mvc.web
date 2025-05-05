using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.Extension;
using agilium.api.manager.Services;
using agilium.api.manager.ViewModels.ConfigViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.manager.V1
{
    [Authorize]
    [Route("api/v{version:apiVersion}/config")]
    [ApiVersion("1.0")]
    [ModelBinder(typeof(JsonWithFilesFormDataModelBinder), Name = "json")]
    public class ConfigController : MainController
    {

        private readonly IConfigService _configService;
        private readonly IMapper _mapper;
        private const string _nomeEntidade = "Configuração";

        public ConfigController(INotificador notificador, IUser appUser, 
                                IConfigService configService, IMapper mapper, IConfiguration configuration, 
                                IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, appUser,configuration,utilDapperRepository,logService)
        {
            _configService = configService;
            _mapper = mapper;
        }

        #region Endpoints
        [HttpGet("configuracoes")]
        //[ClaimsAuthorize("EMPRESA", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<ConfigIndexViewModel>>> IndexPagination([FromQuery] int page = 1, [FromQuery] int ps = 15, [FromQuery] string q = null, [FromQuery] long idEmpresa = 0)
        {            
            var lista = (await ObterListaPaginado(idEmpresa, q, page, ps));
            ViewBag.Pesquisa = q;

            return CustomResponse(lista);
        }


        [HttpGet("obter-config-por-chave")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigIndexViewModel>> ObterPorId([FromQuery] string chave, [FromQuery] long idEmpresa)
        {
            var objeto = _mapper.Map<ConfigIndexViewModel>(await _configService.ObterPorChave(chave, idEmpresa));
            return CustomResponse(objeto);
        }

        [HttpGet("obter-por-empresa/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ConfigIndexViewModel>>> ObterTodosPorEmpresa(long idEmpresa)
        {
            var objetos = _mapper.Map<IEnumerable<ConfigIndexViewModel>>(await _configService.ObterTodosPorEmpresa(idEmpresa));
            return CustomResponse(objetos);
        }

        [HttpPut("{idEmpresa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequestSizeLimit(60000000)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(long idEmpresa,[FromBody] IEnumerable<ConfigIndexViewModel> viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            viewModel.ToList().ForEach( config=> {
                if (config.IDEMPRESA.HasValue && !string.IsNullOrEmpty(config.CHAVE))
                {
                    _configService.Atualizar(_mapper.Map<Config>(config));
                    if (config.Arquivo != null)
                        if(!UploadArquivoAlternativo(config.Arquivo, idEmpresa.ToString()).Result)
                        {
                            NotificarErro($"Erro ao tentar salvar arquivo: {config.CHAVE}");
                        }
                }


            });
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "Atualizar", "Web"));
                return CustomResponse(msgErro);
            }
            await _configService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {viewModel}", "Config", "Atualizar", null);
            return CustomResponse(viewModel);
        }

        [HttpPut("atualiza-config/{idEmpresa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        [RequestSizeLimit(60000000)]
        //[Produces("application/json")]
        public async Task<ActionResult> Atualizar(long idEmpresa, [FromBody] ConfigIndexViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            
            if (viewModel.IDEMPRESA.HasValue && !string.IsNullOrEmpty(viewModel.CHAVE))
            {
                await _configService.Atualizar(_mapper.Map<Config>(viewModel));
            };

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "Atualizar", "Web"));
                return CustomResponse(msgErro);
            }
            await _configService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {Deserializar(viewModel)}", "Config", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        [HttpPost("atualiza-certificado-1")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        //[RequestSizeLimit(40000000)]
        //[Produces("application/json")]
        public async Task<ActionResult> AtualizarCertificado(ConfigIndexViewModel certificado)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

                //await _configService.Atualizar(_mapper.Map<Config>(certificado));
                //if (certificado.Arquivo != null)
                //    if (!UploadArquivoAlternativo(certificado.Arquivo).Result)
                //    {
                //        NotificarErro($"Erro ao tentar salvar arquivo: {certificado.CHAVE}");
                //    }
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "AtualizarCertificado", "Web"));
                return CustomResponse(msgErro);
            }
            await _configService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {Deserializar(certificado)}", "Config", "AtualizarCertificado", null);
            return CustomResponse(certificado);
        }

        [HttpPost("atualiza-certificado")]
        public async Task<ActionResult> EnviaArquivo(long idEmpresa,  IFormFile arquivo)
        {
            if (!UploadArquivoAlternativo(arquivo, idEmpresa.ToString()).Result)
            {
                NotificarErro($"Erro ao tentar fazer upload do certificado");
            }

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "EnviaArquivo", "Web"));
                return CustomResponse(msgErro);
            }

            return CustomResponse("Certificado salvo");
        }


        [HttpPut("config-imagem/{chave}")]
        public async Task<ActionResult> AtualizaConfigImagem(string chave, long idEmpresa, IFormFile arquivo)
        {
            var configImagem = _configService.ObterConfigImagemPorChave(chave, idEmpresa).Result;

            if(configImagem == null)
            {
                NotificarErro("erro ao tenta atualizar configuração de imagem");
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "AtualizaConfigImagem", "Web"));
                return CustomResponse(msgErro);
            }

            var memoryStream = new MemoryStream();
            await arquivo.CopyToAsync(memoryStream);
            
            configImagem.IMG = memoryStream.ToArray();

            await _configService.Atualizar(configImagem);
  
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Config", "AtualizaConfigImagem", "Web"));
                return CustomResponse(msgErro);
            }
            await _configService.Salvar();
            LogInformacao($"Objeto atualizar com sucesso {Deserializar(configImagem)}", "Config", "AtualizaConfigImagem", null);
            return CustomResponse("Certificado salvo");
        }


        [HttpGet("config-imagem/{idEmpresa}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<ConfigImagemViewModel>>> ObterCongiImagemPorEmpresa(long idEmpresa)
        {
            var lista = new List<ConfigImagemViewModel>();

            _configService.ObterTodosConfigImagem(idEmpresa).Result.ToList().ForEach( item => {
                var model = new ConfigImagemViewModel();

                if(item.IMG != null)
                    model.ImagemConvertida = String.Format("data:image/png;base64,{0}", Utils.ConverterByteToBase64(item.IMG));
                model.IDEMPRESA = item.IDEMPRESA;
                model.CHAVE = item.CHAVE;
                model.IMG = item.IMG;

                lista.Add(model);
            });
          
            var objetos = _mapper.Map<IEnumerable<ConfigImagemViewModel>>(lista);

            return CustomResponse(objetos);
        }

        [HttpGet("config-imagem-por-id/{idEmpresa}/{chave}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<ConfigImagemViewModel>> ObterCongiImagemPorEmpresa(long idEmpresa, string chave)
        {
            var objetos = _mapper.Map<ConfigImagemViewModel>(await _configService.ObterConfigImagemPorChave(chave, idEmpresa));
            objetos.ImagemConvertida = String.Format("data:image/png;base64,{0}", Utils.ConverterByteToBase64(objetos.IMG));
            return CustomResponse(objetos);
        }

        #endregion

        #region private
        private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string idEmpresa)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça uma imagem!");
                return false;
            }

            var imgPrefixo = Guid.NewGuid() + "_";
            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/certificado", idEmpresa + arquivo.FileName);
            if (System.IO.File.Exists(path))
            {
                NotificarErro("Já existe um arquivo com este nome!");
                return false;
            }

            using (var stream = new FileStream(path, FileMode.Create))
            { 
                await arquivo.CopyToAsync(stream);
            }

            return true;
        }

        private async Task<agilium.api.business.Models.PagedResult<ConfigIndexViewModel>> ObterListaPaginado(long idEmpresa,string filtro, int page, int pageSize)
        {
            var retorno = await _configService.ObterPorDescricaoPaginacao(idEmpresa,filtro, page, pageSize);

            var lista = _mapper.Map<IEnumerable<ConfigIndexViewModel>>(retorno.List);

            return new agilium.api.business.Models.PagedResult<ConfigIndexViewModel>()
            {
                List = lista,
                PageIndex = retorno.PageIndex,
                PageSize = retorno.PageSize,
                Query = retorno.Query,
                //ReferenceAction = "IndexPagination",
                TotalResults = retorno.TotalResults
            };
        }
        #endregion
    }
}
