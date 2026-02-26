using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IEmpresaService: IDisposable
    {
        Task Adicionar(Empresa empresa);
        Task Atualizar(Empresa empresa);
        Task Atualizar(Empresa empresaDb, object model);
        Task Apagar(long id);
        Task<Empresa> ObterPorId(long id);
        Task<List<Empresa>> ObterPorDescricao(string descricao);
        Task<PagedResult<Empresa>> ObterPorDescricaoPaginacao(string descricao, int page = 1, int pageSize = 15);
        Task<Empresa> ObterCompletoPorId(long id);
        Task<List<Empresa>> ObterTodas();
        Task Salvar();
        Task<string> GerarCodigo();
        Task<Empresa> ObterPorIdCompleto(long id);
        Task<Empresa> ObterPorIdCompletoTracking(long id);

        Task<bool> EditarEmpresa(Empresa empresa);
    }
}
