using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using agilium.api.business.Enums;
using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.manager.Controllers;
using agilium.api.manager.Extension;
using agilium.api.manager.ViewModels;
using agilium.api.manager.ViewModels.ControleAcessoViewModel;
using agilium.api.manager.ViewModels.EmpresaViewModel;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Cors;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using static agilium.api.manager.ViewModels.UserViewModel;

namespace agilium.api.manager.V1
{
    [Route("api/v{version:apiVersion}/usuarios")]
    [ApiVersion("1.0")]
    [Authorize]
    public class UsuarioController : MainController
    {
        private readonly IUsuarioService _usuarioService;
        private readonly IMapper _mapper;
        private readonly ILogger<UsuarioController> _logger;
       // private readonly IUsuarioFotoService _productServices;
        private readonly IUsuarioFotoEntityService _usuarioFotoServiceService;
        private readonly IEmpresaService _empresaService;
        private readonly ICaRepositoryDapper _caRepositoryDapper;
        private readonly ICaService _caService;

        public UsuarioController(INotificador notificador,
                                 IMapper mapper,
                                 IUsuarioService usuarioService,
                                 IUser appUser,
                                 ILogger<UsuarioController> logger,
                                 IEmpresaService empresaService,
             IUsuarioFotoEntityService usuarioFotoServiceService,
             ICaRepositoryDapper caRepositoryDapper, ICaService caService,
            IConfiguration configuration, IUtilDapperRepository utilDapperRepository,
            ILogService logService) : base(notificador, appUser, configuration, utilDapperRepository,logService)
        {
            _usuarioService = usuarioService;
            _mapper = mapper;
            _logger = logger;
            _empresaService = empresaService;
            _usuarioFotoServiceService = usuarioFotoServiceService;
            _caRepositoryDapper = caRepositoryDapper;
            _caService = caService;
        }

        [HttpGet("teste")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> Teste()
        {
            return Ok("Sucesso");

        }

        [HttpGet("{id}")]
        [ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioPadrao>> Obter(string id)
        {
            long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<UsuarioPadrao>(await _usuarioService.ObterPorUsuarioPorId(_id));
            return CustomResponse(objeto);
        }


        [HttpGet("obter-todos")]
        [ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UsuarioPadrao>>> ObterTodos()
        {
            var listaUsuarios = _mapper.Map<List<UsuarioPadrao>>(await _usuarioService.ObterTodosUsuarios());
            return CustomResponse(listaUsuarios);
        }


        [HttpGet("obter-todos-paginacao")]
        [ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<business.Models.PagedResult<UsuarioPadrao>>> ObterTodos([FromQuery] int page = 1, [FromQuery] int ps = 15)
        {
            var listaUsuarios = (await _usuarioService.ObterTodosUsuarios(page, ps));
            var listaPadraoConvertida = _mapper.Map<business.Models.PagedResult<UsuarioPadrao>>(listaUsuarios);
            var listaPerfis = await _caService.ObterTodosCaPerfilManager();

            listaPadraoConvertida.List.ToList().ForEach(x =>
            {
                var descricaoPerfil = "";
                if (x.idperfilManager != null)
                    descricaoPerfil = listaPerfis.FirstOrDefault(p => p.IdPerfil.ToString() == x.idperfilManager).Descricao;
                if(!string.IsNullOrEmpty(descricaoPerfil))
                    x.PerfilDescricao = descricaoPerfil;

            });
            return CustomResponse(listaPadraoConvertida);
        }


        [HttpGet("obter-por-userId/{id}")]
        [ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioPadrao>> ObterPorUserId(string id)
        {
      
           // long _id = Convert.ToInt64(id);
            var objeto = _mapper.Map<UsuarioPadrao>(await _usuarioService.ObterPorUsuarioAspNetPorId(id));
            return CustomResponse(objeto);
        }

        [HttpGet("obter-por-nome/{nome}")]
        [ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UsuarioPadrao>>> ObterPorNome(string nome)
        {
            var lista = _mapper.Map<List<UsuarioPadrao>>(await _usuarioService.ObterUsuariosPorNome(nome));
            return CustomResponse(lista);
        }

        [HttpGet("obter-por-nome-pag/{nome}")]
        [ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<List<UsuarioPadrao>>> ObterPorNome(string nome, [FromQuery] int page = 1, [FromQuery] int ps = 15)
        {
            var lista = (await _usuarioService.ObterUsuariosPorNome(nome, page, ps));
            var listaPadraoConvertida = _mapper.Map<business.Models.PagedResult<UsuarioPadrao>>(lista);
            var listaPerfis = await _caService.ObterTodosCaPerfilManager();

            listaPadraoConvertida.List.ToList().ForEach(x =>
            {
                var descricaoPerfil = "";
                if (x.idperfil != null)
                    descricaoPerfil = listaPerfis.FirstOrDefault(p => p.IdPerfil.ToString() == x.idperfilManager).Descricao;

                x.PerfilDescricao = descricaoPerfil;

            });
            return CustomResponse(listaPadraoConvertida);
        }

        // [ClaimsAuthorize("Usuario", "Adicionar")]
        [HttpPost]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // [Produces("application/json")]
        public async Task<ActionResult> Adicionar([FromBody] UsuarioPadrao viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _usuarioService.Adicionar(_mapper.Map<Usuario>(viewModel));
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Usuario", "Adicionar", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Usuario", "Adicionar", null);
            return CustomResponse(viewModel);
        }

        // [ClaimsAuthorize("Usuario", "Atualizar")]
        [HttpPut("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<UsuarioPadrao>> Atualizar(string id, [FromBody]  UsuarioPadrao viewModel)
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

        //   [ClaimsAuthorize("Usuario", "Excluir")]
        [HttpDelete("{id}")]
        public async Task<ActionResult<UsuarioViewModel>> Excluir(long id)
        {
            var viewModel = await _usuarioService.ObterPorUsuarioPorId(id);

            if (viewModel == null) return NotFound();

            await _usuarioService.Remover(id);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Usuario", "Excluir", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Usuario", "Excluir", null);

            return CustomResponse(viewModel);
        }

        //[ClaimsAuthorize("Produto", "Adicionar")]
        //[HttpPost("atualizar-foto-usuario")]
        //[RequestSizeLimit(60000000)]
        //public async Task<ActionResult<bool>> AdicionarAlternativo([FromBody] UsuarioFotoViewModel fotoViewModel)
        //{
        //    if (!ModelState.IsValid) return CustomResponse(ModelState);

        //    await AtualizarFoto(fotoViewModel);
        //    var usuarioAtualizacao = await _usuarioService.ObterPorUsuarioPorId(Convert.ToInt64(fotoViewModel.id));

        //    fotoViewModel.ImagemUpLoad = await ConverterToIFormFile(await ConverterToIFormFile(fotoViewModel.ImagemConvertida), fotoViewModel.NomeArquivo , fotoViewModel.NomeArquivoExtensao);
        //    var imgPrefixo = Guid.NewGuid() + "_";
        //    //if (!await UploadArquivoAlternativo(fotoViewModel.ImagemUpLoad, imgPrefixo))
        //    //{
        //    //    return CustomResponse(ModelState);
        //    //}

        //    fotoViewModel.Foto = imgPrefixo + fotoViewModel.ImagemUpLoad.FileName;
        //    usuarioAtualizacao.AutalizarFoto(fotoViewModel.Foto);

        //    var resultado = await _usuarioService.AtualizarFoto(usuarioAtualizacao);
        //    if (!resultado)
        //    {
        //        NotificarErro("Erro ao tentar atualizar foto");
        //        return CustomResponse();
        //    }

        //    var usuarioFotoExistente = ObterUsuarioFotoDocumentoMongo(usuarioAtualizacao.idUserAspNet);

        //    if (usuarioFotoExistente != null && !string.IsNullOrEmpty(usuarioFotoExistente.Id))
        //    {
        //        var usuarioFoto = new UsuarioFoto(usuarioFotoExistente.Id, usuarioAtualizacao.idUserAspNet, usuarioAtualizacao.nome, fotoViewModel.Foto,
        //                                        fotoViewModel.ImagemConvertida, DateTime.Now, usuarioAtualizacao.ativo,
        //                                        fotoViewModel.Foto, usuarioAtualizacao.Id);
        //        _productServices.Update(usuarioAtualizacao.idUserAspNet, usuarioFoto);
        //    }
        //    else
        //    {
        //        var usuarioFoto = new UsuarioFoto(usuarioAtualizacao.idUserAspNet, usuarioAtualizacao.nome, fotoViewModel.Foto,
        //                                        fotoViewModel.ImagemConvertida, DateTime.Now, usuarioAtualizacao.ativo,
        //                                        fotoViewModel.Foto, usuarioAtualizacao.Id);
        //        _productServices.Insert(usuarioFoto);
        //    }


        //    //await _productServices.Save(usuarioFoto);

        //    //if (!resultado)
        //        //await ApagarArquivo(fotoViewModel.Foto);

        //    return CustomResponse(resultado);
        //}

        [HttpGet("obter-empresas-por-usuario/{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmpresaUsuarioViewModel>>> ObterEmpresaUsuario(long id) 
        {
            var listaEmpresaUsuario = _usuarioService.ObterEmpresasPorUsuario(id).Result;
            var lista = new List<EmpresaUsuarioViewModel>();// _mapper.Map<List<EmpresaUsuarioViewModel>>(listaEmpresaUsuario);
            listaEmpresaUsuario.ForEach(empresaUsuario => {
                var objetoConvertido = _mapper.Map<EmpresaUsuarioViewModel>(empresaUsuario);
                
                if(empresaUsuario.Empresa != null) objetoConvertido.NomeEmpresa = empresaUsuario.Empresa.NMRZSOCIAL;

                lista.Add(objetoConvertido);
            });
            return CustomResponse(lista);
        }

        [HttpGet("obter-empresa-usuario-por-id/{idUsuario}/{idEmpresa}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<IEnumerable<EmpresaUsuarioViewModel>>> ObterEmpresaUsuarioPorId([FromQuery]long idUsuario, [FromQuery] long idEmpresa)
        {
            var usuarioEmpresa = _usuarioService.ObterEmpresaPorId(idUsuario,idEmpresa).Result;
            return CustomResponse(usuarioEmpresa);
        }

        [HttpPost("empresa-usuario")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // [Produces("application/json")]
        public async Task<ActionResult> AdicionarEmpresaUsuario([FromBody] EmpresaUsuarioViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            await _usuarioService.Adicionar(_mapper.Map<EmpresaAuth>(viewModel));

            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Usuario", "AdicionarEmpresaUsuario", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }

            await _usuarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Usuario", "AdicionarEmpresaUsuario", null);
            return CustomResponse(viewModel);
        }

        [HttpPost("empresas-usuarios")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // [Produces("application/json")]
        public async Task<ActionResult> AdicionarEmpresasUsuarios([FromBody] List<EmpresaUsuarioViewModel> viewModel, [FromQuery] long idUsuario)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);
            
           
            if (!_usuarioService.RemoverEmpresaUsuarioPorUsuario(idUsuario).Result)
            {
                NotificarErro("Não foi possivel fazer a associação da empresa ao usuario");
            }
            
            await _usuarioService.Salvar();

            await _usuarioService.AdicionarLista(_mapper.Map<List<EmpresaAuth>>(viewModel));
            
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes("Usuario", "AdicionarEmpresasUsuarios", "Web", Deserializar(viewModel)));
                return CustomResponse(msgErro);
            }

            await _usuarioService.Salvar();
            LogInformacao($"sucesso: {Deserializar(viewModel)}", "Usuario", "AdicionarEmpresasUsuarios", null);
            return CustomResponse(viewModel);
        }

        [HttpGet("obter-empresas-disponiveis/{idUsuario}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult> ObterEmpresasPorUsuarioDisponiveis( long idUsuario)
        {
            var empresasUsuarioViewModel = new EmpresasAssociadasViewModel();
            empresasUsuarioViewModel.EmpresasAssociadas = _mapper.Map<List<EmpresaUsuarioViewModel>>(_usuarioService.ObterEmpresasPorUsuario(idUsuario).Result);
            //empresasUsuarioViewModel.EmpresasDisponiveisAssociacao= _mapper.Map<List<EmpresaViewModel>>(_usuarioService.ObterEmpresasDisponiveisAssociacao(idUsuario).Result);
            var empresas = _empresaService.ObterPorDescricao("").Result;
            empresasUsuarioViewModel.Empresas = _mapper.Map<List<EmpresaViewModel>>(empresas);

            return CustomResponse(empresasUsuarioViewModel);
        }

        [HttpDelete("{idUsuario}/{idEmpresa}")]
        public async Task<ActionResult<UsuarioViewModel>> Excluir([FromQuery]long idUsuario, [FromQuery] long idEmpresa)
        {
            await _usuarioService.Remover(idUsuario, idEmpresa);
            LogInformacao($"sucesso: idempresa:{idEmpresa} idusuario:{idUsuario}", "Usuario", "Excluir", null);
            return CustomResponse("sucesso");
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

        [HttpPost("atualizar-foto-usuario")]
        [RequestSizeLimit(60000000)]
      
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

        [HttpGet("obter-usuario-perfis/{id}")]
        //[ClaimsAuthorize("USUARIO", "CONSULTA")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<ActionResult<SelecionarPerfilViewModel>> ObterUsuarioPerfil(string id)
        {
            var usuario = await _usuarioService.ObterPorUsuarioAspNetPorId(id);
            var idperfil = usuario.idPerfil.HasValue ? usuario.idPerfil.Value : 0;
            var perfis = _mapper.Map<List<PerfilIndexViewModel>>(await _caRepositoryDapper.ObterPerfil(idperfil));

            var selecionaPerfil = new SelecionarPerfilViewModel() {
                IdAspNetUser = usuario.idUserAspNet,
                NomeUsuario = usuario.nome,
                idPerfil = usuario.idPerfil.HasValue?usuario.idPerfil.Value:0,
                PerfilAtual = usuario.idPerfil.HasValue ? perfis.FirstOrDefault(x => x.Id == usuario.idPerfil).Descricao : "",
                Perfis = perfis.Where(x => x.Id != usuario.idPerfil).ToList()
            };

            return CustomResponse(selecionaPerfil);
        }

        [HttpPost("selecionar-perfil")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        // [Produces("application/json")]
        public async Task<ActionResult> SelecionarPerfil([FromBody] SelecionarPerfilViewModel viewModel)
        {
            if (!ModelState.IsValid) return CustomResponse(ModelState);

            var idPerfil = viewModel.idPerfil.HasValue ? viewModel.idPerfil.Value : 0;
            await _caRepositoryDapper.AtualizarUsuarioPorPerfil(idPerfil, viewModel.IdAspNetUser);
            if (!OperacaoValida())
            {
                var msgErro = string.Join("\n\r", ObterNotificacoes());
                return CustomResponse(msgErro);
            }

            return CustomResponse("Perfil atribuido com sucesso");

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
                    if(usuario != null)
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

        //private UsuarioFoto ObterUsuarioFotoDocumentoMongo(string idUserAspNet)
        //{
        //    return  _productServices.Query(idUserAspNet);
        //}

        private async Task<bool> UploadArquivoAlternativo(IFormFile arquivo, string imgPrefixo)
        {
            if (arquivo == null || arquivo.Length == 0)
            {
                NotificarErro("Forneça uma imagem!");
                return false;
            }

            //var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/Foto", imgPrefixo + arquivo.FileName);
            
            var path = await MontarCaminhoArquivoFotoUsuario(imgPrefixo + arquivo.FileName);

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

        private async Task<bool> ApagarArquivo(string arquivo)
        {
            // Delete a file by using File class static method...

            var path = await MontarCaminhoArquivoFotoUsuario(arquivo);
            if (System.IO.File.Exists(path))                
            {
                System.IO.File.Delete(path);               
                return true;
            }

            return false;
        }

        private async Task<FormFile> ObterImagem(string arquivo)
        {
            var path = await MontarCaminhoArquivoFotoUsuario(arquivo);
            //Open the stream and read it back.
            using (var stream = System.IO.File.OpenRead(path))
            {
                FormFile file = new FormFile(stream, 0, stream.Length, null, Path.GetFileName(stream.Name))
                {
                    Headers = new HeaderDictionary(),
                    ContentType = "image/jpeg"
                };

                return file;
            }
        }

        private async Task<Byte[]> ObterImagemFile(string arquivo)
        {
            var path = await MontarCaminhoArquivoFotoUsuario(arquivo);

            byte[] binaryImage = System.IO.File.ReadAllBytes(path);
            return binaryImage;
        }

        private async Task<string> MontarCaminhoArquivoFotoUsuario(string arquivo)
        {
            return Path.Combine(Directory.GetCurrentDirectory(), "wwwroot\\Foto", arquivo);
        }

      
        #endregion
    }
}