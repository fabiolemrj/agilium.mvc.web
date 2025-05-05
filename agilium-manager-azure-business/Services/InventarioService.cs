using agilium.api.business.Enums;
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
    public class InventarioService : BaseService, IInventarioService
    {
        private readonly IInventarioRepository _inventarioRepository;
        private readonly IInventarioItemRepository _inventarioItemRepository;
        private readonly IInventarioDapperRepository _inventarioDapperRepository;
        private readonly IDapperRepository _dapperRepository;
        private readonly IProdutoDapper _produtoDapper;
        private readonly IPerdaDapperRepository _perdaDapperRepository;
        public InventarioService(INotificador notificador, IInventarioDapperRepository inventarioDapperRepository, 
                                IInventarioItemRepository inventarioItemRepository, IInventarioRepository inventarioRepository, 
                                IDapperRepository dapperRepository, IProdutoDapper produtoDapper, IPerdaDapperRepository perdaDapperRepository) : base(notificador)
        {
            _inventarioDapperRepository = inventarioDapperRepository;
            _inventarioItemRepository = inventarioItemRepository;
            _inventarioRepository = inventarioRepository;
            _dapperRepository = dapperRepository;
            _produtoDapper = produtoDapper;
            _perdaDapperRepository = perdaDapperRepository;
        }

        public void Dispose()
        {
            _inventarioItemRepository?.Dispose();
            _inventarioRepository?.Dispose();
        }

        public async Task Salvar()
        {
            await _inventarioRepository.SaveChanges();
        }

        #region Inventario
        
        public async Task Adicionar(Inventario inventario)
        {
            if (!ExecutarValidacao(new InventarioValidation(), inventario))
                return;

            await _inventarioRepository.AdicionarSemSalvar(inventario);
        }

        public async Task Apagar(long id)
        {
            if (await PodeApagarInventario(id))
            {
                Notificar("Não é possivel apagar este inventario pois o mesmo está sendo utilizado");
                return;
            }
            await _inventarioRepository.RemoverSemSalvar(id);
        }
    

        public async Task Atualizar(Inventario inventario)
        {
            if (!ExecutarValidacao(new InventarioValidation(), inventario))
                return;

            await _inventarioRepository.AtualizarSemSalvar(inventario);
        }

        public async Task<Inventario> ObterPorId(long id)
        {
            return _inventarioRepository.ObterPorId(id).Result;
        }

        public async Task<PagedResult<Inventario>> ObterPorPaginacao(long idEmpresa, string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;

            var descricaoUpper = !string.IsNullOrEmpty(descricao) ? descricao.ToUpper(): string.Empty;
            var lista = _inventarioRepository.Obter(x => x.IDEMPRESA == idEmpresa && x.DSINVENT.ToUpper().Contains(descricaoUpper)).Result;

            return new PagedResult<Inventario>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<Inventario>> ObterTodas(long idEmpresa)
        {
            return _inventarioRepository.Obter(x => x.IDEMPRESA == idEmpresa).Result;
        }
        #endregion

        #region Item
        public async Task Adicionar(InventarioItem inventarioItem)
        {
            if (!ExecutarValidacao(new InventarioItemValidation(), inventarioItem))
                return;

            await _inventarioItemRepository.AdicionarSemSalvar(inventarioItem);
        }
        public async Task ApagarItem(long id)
        {
            if (await PodeApagarInventarioItem(id))
            {
                Notificar("Não é possivel apagar este inventario Item pois o mesmo está sendo utilizado");
                return;
            }
            await _inventarioItemRepository.RemoverSemSalvar(id);
        }

        public async Task<InventarioItem> ObterItemPorId(long id)
        {
            return _inventarioItemRepository.ObterPorId(id).Result;
        }

        public async Task<List<InventarioItem>> ObterItensPorInventario(long id)
        {
            return _inventarioItemRepository.Obter(x=>x.IDINVENT == id).Result.ToList();
        }
        
        public async Task Atualizar(InventarioItem inventarioItem)
        {
            if (!ExecutarValidacao(new InventarioItemValidation(), inventarioItem))
                return;

            await _inventarioItemRepository.AtualizarSemSalvar(inventarioItem);
        }

        #endregion

        #region Dapper
        public async Task<bool> IncluirProdutosPorEstoque(long idEstoque, long idInvent)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                var produtosEstoque = _inventarioDapperRepository.ObterProdutosPorEstoque(idEstoque).Result;
                produtosEstoque.ForEach(async item => {
                    if (_inventarioDapperRepository.PodeIncluirProdutoEstoque(idInvent, item.IDPRODUTO.Value).Result)
                    {
                        await _inventarioDapperRepository.IncluirProdutoItemEstoque(idInvent, item.IDPRODUTO.Value);
                    }
                });
                
                resultado = !TemNotificacao();

                if (resultado)
                    await _dapperRepository.Commit();
                else
                    await _dapperRepository.Rollback();
            }
            catch (Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);

                await _dapperRepository.Rollback();
            }
            return resultado;
        }

        public async Task<List<Produto>> ObetrProdutoDisponvelInventario(long idEmpresa, long idInventario)
        {
            return await _inventarioDapperRepository.ObterProdutosDisponiveisParaInventarioPorIdInventario(idInventario, idEmpresa);
        }

        public async Task<bool> IncluirProdutoInventario(List<InventarioItem> itens)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                itens.ForEach(async item => {
                    if (_inventarioDapperRepository.PodeIncluirProdutoEstoque(item.IDINVENT.Value, item.IDPRODUTO.Value).Result)
                    {
                        await _inventarioDapperRepository.IncluirProdutoItemEstoque(item.IDINVENT.Value, item.IDPRODUTO.Value);
                    }
                });

                resultado = !TemNotificacao();

                if (resultado)
                    await _dapperRepository.Commit();
                else
                    await _dapperRepository.Rollback();
            }
            catch (Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);

                await _dapperRepository.Rollback();
            }
            return resultado;
        }

        public async Task<bool> AlterarInventarioItem(List<InventarioItem> itens, long idUsuario)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                itens.ForEach(async item => {
                    if(item.NUQTDANALISE.HasValue)
                        await _inventarioDapperRepository.EditarInventarioItem(item.Id, item.NUQTDANALISE.Value, idUsuario);
                });

                resultado = !TemNotificacao();

                if (resultado)
                    await _dapperRepository.Commit();
                else
                    await _dapperRepository.Rollback();
            }
            catch (Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);

                await _dapperRepository.Rollback();
            }
            return resultado;
        }

        public async Task<bool> ApagarInventarioItem(List<InventarioItem> itens)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();
                
                itens.ForEach(async item => {
                    await _inventarioDapperRepository.ApagarInventarioItem(item.Id);
                });

                resultado = !TemNotificacao();

                if (resultado)
                    await _dapperRepository.Commit();
                else
                    await _dapperRepository.Rollback();
            }
            catch (Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);

                await _dapperRepository.Rollback();
            }
            return resultado;
        }

        public async Task<bool> ConcluirInventario(long idInventario, ESituacaoInventario situacaoInventario, long idUsuario)
        {
            var resultado = false;
            try
            {
                await _dapperRepository.BeginTransaction();

                if(situacaoInventario != ESituacaoInventario.Execucao)
                {
                    Notificar($"A situação do inventario deve estar como 'Em Execução'");
                    return false;
                }

                if (_inventarioDapperRepository.ExisteItemNaoInventariado(idInventario).Result)
                {
                    Notificar($"Inventário não pode ser concluído, pois ainda existem itens que não foram inventariados!");
                    return false;
                }

                var inventario = _inventarioDapperRepository.ObterInventarioPorIdInventario(idInventario).Result;
                
                if (inventario == null)
                {
                    Notificar($"Erro ao tentar concluir Inventário #001!");
                    return false;
                }

                var itensInventario = _inventarioDapperRepository.ObterInventarioItemPorIdInventario(idInventario).Result;

                itensInventario.ForEach(async item => {
                    if (item.IDPRODUTO.HasValue)
                    {
                        var produto = _produtoDapper.ObterProdutoPorId(item.IDPRODUTO.Value).Result;
                        await _inventarioDapperRepository.AtualizarValorCustoMedio(item.IDINVENT.Value, produto.VLCUSTOMEDIO.HasValue ? produto.VLCUSTOMEDIO.Value : 0);
                    }

                    var quantidadePerda = (item.NUQTDESTOQUE.HasValue? item.NUQTDESTOQUE.Value : 0) - (item.NUQTDANALISE.HasValue ? item.NUQTDANALISE.Value: 0);
                    
                    int tipoMovimentacaoPerda = 0;
                    
                    var mensagemPerda = "";
                    
                    if(quantidadePerda > 0)
                    {
                        tipoMovimentacaoPerda = (int)ETipoMovimentoPerda.Perda;
                        mensagemPerda = $"Perda gerada pelo inventário código {inventario.CDINVENT}";
                    }else if(quantidadePerda < 0)
                    {
                        tipoMovimentacaoPerda = (int)ETipoMovimentoPerda.Sobra;
                        mensagemPerda = $"Sobra gerada pelo inventário código {inventario.CDINVENT}";
                    }

                    if(tipoMovimentacaoPerda > 0)
                    {
                        await _perdaDapperRepository.InserirPerda(inventario.IDEMPRESA.Value,inventario.IDESTOQUE.Value,0,item.IDPRODUTO.Value,idUsuario,
                                                                    ETipoPerda.AcertoSaldo,(ETipoMovimentoPerda)tipoMovimentacaoPerda,quantidadePerda ,mensagemPerda);
                    }
                });

                await _inventarioDapperRepository.AlterarSituacao(idInventario, ESituacaoInventario.Concluida);
                
                resultado = !TemNotificacao();

                if (resultado)
                    await _dapperRepository.Commit();
                else
                    await _dapperRepository.Rollback();
            }
            catch (Exception ex)
            {
                do
                {
                    Notificar(ex.Message);
                }
                while (ex != null);

                await _dapperRepository.Rollback();
            }
            return resultado;
        }

        #endregion

        #region private
        private async Task<bool> PodeApagarInventario(long id) => true;
        private async Task<bool> PodeApagarInventarioItem(long id) => true;

        #endregion
    }
}
