using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class FormaPagamentoService : BaseService, IFormaPagamentoService
    {
        private readonly IFormaPagamentoRepository _formaPagamentoRepository;
        
        public FormaPagamentoService(INotificador notificador, IFormaPagamentoRepository formaPagamentoRepository) : base(notificador)
        {
            _formaPagamentoRepository = formaPagamentoRepository;
        }

        public async Task Salvar()
        {
            await _formaPagamentoRepository.SaveChanges();
        }

        public void Dispose()
        {
            _formaPagamentoRepository?.Dispose();
        }

        #region forma pagamento
        public async Task<bool> Adicionar(FormaPagamento formaPagamento)
        {
            if (!ExecutarValidacao(new FormaPagamentoValidation(), formaPagamento))
                return false;

            await _formaPagamentoRepository.AdicionarSemSalvar(formaPagamento);
            return true;
        }

        public async Task Apagar(long id)
        {
            if (!PodeApagar(id).Result)
            {
                Notificar("Não foi possível apagar o registro!");
                return;
            }
            else
                await _formaPagamentoRepository.RemoverSemSalvar(id);
        }

        public async Task<bool> Atualizar(FormaPagamento formaPagamento)
        {
            if (!ExecutarValidacao(new FormaPagamentoValidation(), formaPagamento))
                return false;

            await _formaPagamentoRepository.AtualizarSemSalvar(formaPagamento);
            return true;
        }

        public async Task<List<FormaPagamento>> ObterPorDescricao(string descricao)
        {
            return _formaPagamentoRepository.Buscar(x => x.DSFormaPagamento.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<FormaPagamento> ObterPorId(long id)
        {
            return await _formaPagamentoRepository.ObterPorId(id);
        }

        public async Task<PagedResult<FormaPagamento>> ObterPorPaginacao(long idEmpresa, string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _formaPagamentoRepository.Buscar(x => x.IDEmpresa == idEmpresa && x.DSFormaPagamento.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<FormaPagamento>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<List<FormaPagamento>> ObterTodas()
        {
            return await _formaPagamentoRepository.ObterTodos();
        }

        #endregion
        #region metodos privado
        private async Task<bool> PodeApagar(long idEstoque)
        {
            return true;
        }


        #endregion

    }
}
