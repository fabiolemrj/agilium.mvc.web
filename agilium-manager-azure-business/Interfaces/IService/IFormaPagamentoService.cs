using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces.IService
{
    public interface IFormaPagamentoService: IDisposable
    {

        Task Salvar();
        Task<PagedResult<FormaPagamento>> ObterPorPaginacao(long idEmpresa, string descricao, int page = 1, int pageSize = 15);
        Task<bool> Adicionar(FormaPagamento formaPagamento);
        Task<bool> Atualizar(FormaPagamento formaPagamento);
        Task Apagar(long id);
        Task<FormaPagamento> ObterPorId(long id);
        Task<List<FormaPagamento>> ObterPorDescricao(string descricao);
        Task<List<FormaPagamento>> ObterTodas();
    }
}
