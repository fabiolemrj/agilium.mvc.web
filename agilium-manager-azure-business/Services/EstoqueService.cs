using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.CustomReturn.ReportViewModel.EstoqueReportViewModel;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class EstoqueService : BaseService, IEstoqueService
    {
        private readonly IEstoqueRepository _estoqueRepository;
        private readonly IEstoqueProdutoRepository _estoqueProdutoRepository;
        private readonly IEstoqueHistoricoRepository _estoqueHistoricoRepository;
        private readonly IDapperRepository _dapperRepository;
        private readonly IEstoqueDapperRepository _estoqueDapperRepository;

        public EstoqueService(INotificador notificador,
            IEstoqueRepository estoqueRepository,
            IEstoqueProdutoRepository estoqueProdutoRepository,
            IEstoqueHistoricoRepository estoqueHistoricoRepository,
            IDapperRepository dapperRepository,
            IEstoqueDapperRepository estoqueDapperRepository) : base(notificador)
        {
            _estoqueRepository = estoqueRepository;
            _estoqueProdutoRepository = estoqueProdutoRepository;
            _estoqueHistoricoRepository = estoqueHistoricoRepository;
            _dapperRepository = dapperRepository;
            _estoqueDapperRepository = estoqueDapperRepository;
        }

        #region Estoque
        public async Task Adicionar(Estoque estoque)
        {
            if (!ExecutarValidacao(new EstoqueValidation(), estoque))
                return;

            await _estoqueRepository.AdicionarSemSalvar(estoque);
        }

        public async Task Apagar(long id)
        {
            if(!PodeApagarEstoque(id).Result)
            {
                Notificar("Não foi possível apagar o estoque, pois este possui historicos!");
                return;
            }
            else 
                await _estoqueRepository.RemoverSemSalvar(id); 
        }

        public async Task Atualizar(Estoque estoque)
        {
            if (!ExecutarValidacao(new EstoqueValidation(), estoque))
                return;

            await _estoqueRepository.AtualizarSemSalvar(estoque);
        }


        public async Task<Estoque> ObterCompletoPorId(long id)
        {
            var lista = _estoqueRepository.Obter(x => x.Id == id).Result;
            return lista.FirstOrDefault();
        }

        public async Task<List<Estoque>> ObterPorDescricao(string descricao)
        {
            return _estoqueRepository.Buscar(x => x.Descricao.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<PagedResult<Estoque>> ObterPorDescricaoPaginacao(long idEmpresa, string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _estoqueRepository.Buscar(x => x.idEmpresa == idEmpresa && x.Descricao.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<Estoque>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<Estoque> ObterPorId(long id)
        {
            return _estoqueRepository.ObterPorId(id).Result;
        }

        public async Task<List<Estoque>> ObterTodas()
        {
            return _estoqueRepository.ObterTodos().Result;
        }

        #endregion

        #region Estoque Produto

        public async Task Atualizar(EstoqueProduto estoque)
        {
            await _estoqueProdutoRepository.AtualizarSemSalvar(estoque);
        }

        public async Task<List<EstoqueProduto>> ObterProdutoEstoquePorProduto(long idProduto)
        {
            return _estoqueProdutoRepository.Buscar(x => x.IDPRODUTO == idProduto).Result.ToList();
        }

        public async Task<EstoqueProduto> ObterProdutoPorId(long id)
        {
            return _estoqueProdutoRepository.ObterPorId(id).Result;
        }

        public async Task ApagarProduto(long id)
        {
            await _estoqueProdutoRepository.RemoverSemSalvar(id);
        }

        public async Task Adicionar(EstoqueProduto estoque)
        {
            await _estoqueProdutoRepository.AdicionarSemSalvar(estoque);
        }


        public async Task<List<EstoqueProduto>> ObterProdutoEstoquePorEstoque(long idEstoque)
        {
            return _estoqueProdutoRepository.Buscar(x => x.IDESTOQUE == idEstoque).Result.ToList();
        }

        #endregion

        #region Historico Estoque

        public async Task Adicionar(EstoqueHistorico estoque)
        {
            await _estoqueHistoricoRepository.AdicionarSemSalvar(estoque);
        }

        public async Task Atualizar(EstoqueHistorico estoque)
        {
            await _estoqueHistoricoRepository.AtualizarSemSalvar(estoque);
        }

        public async Task ApagarHistorico(long id)
        {
            await _estoqueHistoricoRepository.RemoverSemSalvar(id);
        }

        public async Task<EstoqueHistorico> ObterHistoricoPorId(long id)
        {
            return _estoqueHistoricoRepository.ObterPorId(id).Result;
        }

        public async Task<List<EstoqueHistorico>> ObterHistoricoEstoquePorProduto(long idProduto)
        {
            return _estoqueHistoricoRepository.Buscar(x => x.IDPRODUTO == idProduto).Result.ToList();
        }
        #endregion

        public void Dispose()
        {
            _estoqueProdutoRepository?.Dispose();
            _estoqueHistoricoRepository?.Dispose();
            _estoqueRepository?.Dispose();
        }

        public async Task Salvar()
        {
            await _estoqueRepository.SaveChanges();
        }

        #region Report
        public async Task<List<EstoquePosicaoReport>> ObterRelatorioPosicaoEstoque(long idEstoque)
        {
            var resultado = new List<EstoquePosicaoReport>();
            try
            {
               // await _dapperRepository.BeginTransaction();

                var produtos = await _estoqueDapperRepository.ObterPosicaoEstoque(idEstoque);

                produtos.ForEach(data => {
                    data.Situacao = data.Quantidade < data.NuQtdMin ? 1 : 2;
                    resultado.Add(data);
                });

               // if (!TemNotificacao())
               ////     await _dapperRepository.Commit();
               // else
               //     await _dapperRepository.Rollback();
            }
            catch (Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);

                //await _dapperRepository.Rollback();
            }
            return resultado;
        }
        #endregion

        #region metodos privado
        private async Task<bool> PodeApagarEstoque(long idEstoque)
        {
            return false;
        }


        #endregion

    }
}
