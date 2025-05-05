
using agilium.webapp.manager.mvc.ViewModels;
using agilium.webapp.manager.mvc.ViewModels.Config;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.webapp.manager.mvc.Interfaces
{
    public interface IConfigServices
    {
        Task<PagedViewModel<ConfigIndexViewModel>> ObterConfigurcoesPorEmpresaEChave(long idEmpresa, string nome, int page = 1, int pageSize = 15);
        Task<ResponseResult> Atualizar(long idEmpresa, IEnumerable<ChaveValorViewModel> contatoEmpresa);
        Task<ResponseResult> Atualizar(long idEmpresa, ChaveValorViewModel chaveValorViewModel);
        Task<ResponseResult> AtualizarCertificado(long idEmpresa, ChaveValorViewModel chaveValorViewModel);
        Task<ResponseResult> AtualizarCertificado(long idEmpresa, ConfigIndexViewModel chaveValorViewModel);
        Task<ConfigIndexViewModel> ObterPorChave(string chave, long idEmpresa);
        Task<List<ConfigIndexViewModel>> ObterTodosPorEmpresa(long idEmpresa);

        Task<ResponseResult> AtualizarCertificado(long idEmpresa, IFormFile certificado);
        Task<IEnumerable<ConfigImagemViewModel>> ObterCongiImagemPorEmpresa(long idEmpresa);
        Task<ConfigImagemViewModel> ObterConfigImagemPorId(long idEmpresa, string chave);
        Task<ResponseResult> Atualizar(ConfigImagemViewModel model);
    }
}
