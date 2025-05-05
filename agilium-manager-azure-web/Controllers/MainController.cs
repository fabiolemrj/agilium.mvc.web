using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using agilium.webapp.manager.mvc.ViewModels;

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace agilium.webapp.manager.mvc.Controllers
{
    public class MainController : Controller
    {
        protected bool ResponsePossuiErros(ResponseResult resposta)
        {

            if (resposta != null && resposta.Errors.Mensagens.Any())
            {
                foreach (var mensagem in resposta.Errors.Mensagens)
                {
                    ModelState.AddModelError(string.Empty, mensagem);
                    MensagemErro(mensagem);
                }

                return true;
            }
           // else if(resposta == null)
            //{
            //    ModelState.AddModelError(string.Empty, "Reposta da requisição está nula!");
            //    return true;
            //}
            

            return false;
        }

        protected string ObterErrosResponse(ResponseResult resposta)
        {
            var result = "";
            if (resposta != null && resposta.Errors.Mensagens.Any())
            {
                foreach (var mensagem in resposta.Errors.Mensagens)
                {
                    result += mensagem + "\r\n";
                }
            }

            return result;
        }

        protected bool ExibirErros()
        {
            if (!OperacaoValida())
            {
                foreach (var mensagem in ModelState.Values)
                {
                    mensagem.Errors.ToList().ForEach(erro =>
                    {
                        MensagemErro(erro.ErrorMessage);
                    });
                }

                return true;
            }

            return false;
        }

        protected bool ResponsePossuiErrosStatusCode(ResponseResult resposta)
        {
            return resposta.Status >= 400;
        }

        protected void AdicionarErroValidacao(string mensagem)
        {
            ModelState.AddModelError(string.Empty, mensagem);
        }

        protected bool OperacaoValida()
        {
            return ModelState.ErrorCount == 0;
        }

        protected void MensagemErro(string msg)
        {
            TempData["TipoMensagem"] = "danger";
            TempData["Mensagem"] = msg;
        }

        protected  IFormFile ConverterToIFormFile(byte[] file, string fileName, string fileNameExtensao)
        {
            var stream = new MemoryStream(file);
            return new FormFile(stream, 0, stream.Length, fileName, fileNameExtensao);
        }

        protected byte[] ConverterToIbyteArray(string fileBase64)
        {
            return Convert.FromBase64String(fileBase64);
        }

        protected long ObterIdEmpresaSelecionada()
        {

            var idSelecionada = HttpContext.Session.GetString("idEmpresaSelecionado");
            if (string.IsNullOrEmpty(idSelecionada) || (Convert.ToInt64(idSelecionada) <= 0))
                return 0;

            return Convert.ToInt64(idSelecionada);
        }

        protected string RetirarPontos(string valor)
        {
            return valor.Replace(".", "").Replace("-", "").Replace("/", "").Replace(",", "");
        }
    }
}