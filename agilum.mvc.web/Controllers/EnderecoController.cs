using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IService;
using agilum.mvc.web.ViewModels.Endereco;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;

namespace agilum.mvc.web.Controllers
{
    [Authorize]
    [Route("endereco")]
    public class EnderecoController : MainController
    {
        private readonly IEnderecoService _enderecoService;
 

        public EnderecoController(IEnderecoService enderecoService, IMapper mapper, INotificador notificador, 
            IConfiguration configuration, IUser appUser, IUtilDapperRepository utilDapperRepository, ILogService logService) : base(notificador, configuration, appUser, utilDapperRepository, logService, mapper)
        {
            _enderecoService = enderecoService;
 
        }

        [Route("buscar-cep")]
        public async Task<IActionResult> BuscarCep(string cep)
        {
            if(string.IsNullOrEmpty(cep))
                return NotFound();
            var objetoCep = await _enderecoService.ObterCepPorNumeroCep(cep);
            if (objetoCep == null)
            {
                var msgErro = "Numero de Cep não localizado";
                NotificarErro(msgErro);
                return NotFound(msgErro);
            }
            var cepViewModel = _mapper.Map<CepViewModel>(objetoCep);

            return Ok(cepViewModel);
        }
    }
}
