using agilium.webapp.manager.mvc.Interfaces;
using agilium.webapp.manager.mvc.ViewModels.Endereco;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Controllers
{
    
    public class EnderecoController : MainController
    {
        private readonly IEnderecoService _enderecoService;
        
        public EnderecoController(IEnderecoService enderecoService)
        {
            _enderecoService = enderecoService;
        }

        [HttpGet]
        public async Task<IActionResult> BuscarCep(string cep)
        {
            var objetoCep = await _enderecoService.ObterCepPorNumeroCep(cep);
            if (objetoCep != null)
            {
                return new JsonResult(objetoCep);
            }
            return new JsonResult(new { erro = "Cep não localizado" });
        }
    }
}
