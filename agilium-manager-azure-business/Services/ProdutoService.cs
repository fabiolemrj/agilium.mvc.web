using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;
using System.Transactions;

namespace agilium.api.business.Services
{
    public class ProdutoService : BaseService, IProdutoService
    {
        private readonly IProdutoDepartamentoRepository _produtoDepartamentoRepository;
        private readonly IProdutoMarcaRepository _produtoMarcaRepository;
        private readonly IGrupoProdutoRepository _grupoProdutoRepository;
        private readonly ISubGrupoProdutoRepository _subGrupoProdutoRepository;
        private readonly IProdutoRepository _produtoRepository;
        private readonly IProdutoCodigoBarraRepository _produtoCodigoBarraRepository;
        private readonly IProdutoPrecoRepository _produtoPrecoRepository;
        private readonly IProdutoFotoRepository _produtoFotoRepository;
        private readonly IDapperRepository _dapperRepository;
        private readonly IProdutoDapper _produtoDapperRepository;

        public ProdutoService(INotificador notificador,
            IProdutoDepartamentoRepository produtoDepartamentoRepository,
            IProdutoMarcaRepository produtoMarcaRepository,
            IGrupoProdutoRepository grupoProdutoRepository,
            ISubGrupoProdutoRepository subGrupoProdutoRepository,
            IProdutoRepository produtoRepository,
            IProdutoCodigoBarraRepository produtoCodigoBarraRepository,
            IProdutoPrecoRepository produtoPrecoRepository,
            IProdutoFotoRepository produtoFotoRepository,
            IDapperRepository dapperRepository,
            IProdutoDapper produtoDapper) : base(notificador)
        {
            _produtoDepartamentoRepository = produtoDepartamentoRepository;
            _produtoMarcaRepository = produtoMarcaRepository;
            _grupoProdutoRepository = grupoProdutoRepository;
            _subGrupoProdutoRepository = subGrupoProdutoRepository;
            _produtoRepository = produtoRepository;
            _produtoCodigoBarraRepository = produtoCodigoBarraRepository;
            _produtoPrecoRepository = produtoPrecoRepository;
            _produtoFotoRepository = produtoFotoRepository;
            _dapperRepository = dapperRepository;
            _produtoDapperRepository = produtoDapper;
        }

        public async Task Salvar()
        {
            await _produtoRepository.SaveChanges();
        }

        public void Dispose()
        {
            _produtoDepartamentoRepository?.Dispose();           
            _grupoProdutoRepository?.Dispose();
            _produtoMarcaRepository?.Dispose();
            _subGrupoProdutoRepository.Dispose();
            _produtoCodigoBarraRepository?.Dispose();           
            _produtoPrecoRepository?.Dispose();
            _produtoFotoRepository?.Dispose();
            _produtoRepository?.Dispose();;
        }

        #region Produto

        public async Task Adicionar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto))
                return;

            await _produtoRepository.AdicionarSemSalvar(produto);
        }

        public async Task Atualizar(Produto produto)
        {
            if (!ExecutarValidacao(new ProdutoValidation(), produto))
                return;

            await _produtoRepository.AtualizarSemSalvar(produto);
        }

        public async Task Apagar(long id)
        {
            if (await ExisteProdutoUtilizado(id))
            {
                Notificar("Não é possivel apagar este produto pois o mesmo está sendo utilizado");
                return;
            }
            await _produtoRepository.RemoverSemSalvar(id);
        }

        public async Task<Produto> ObterPorId(long id)
        {
            return await _produtoRepository.ObterPorId(id);
        }

        public async Task<List<Produto>> ObterPorDescricao(string descricao)
        {
            return _produtoRepository.Buscar(x => x.NMPRODUTO.ToUpper().Contains(descricao.ToUpper())).Result.ToList();
        }

        public async Task<PagedResult<Produto>> ObterPorPaginacao(long idEmpresa, string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _produtoRepository.Buscar(x => x.idEmpresa == idEmpresa && x.NMPRODUTO.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<Produto>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.CDPRODUTO),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            }; ;
        }

        public async Task<Produto> ObterCompletoPorId(long id)
        {
            var lista = _produtoRepository.Obter(x => x.Id == id).Result;

            return lista.FirstOrDefault();
        }

        public async Task<List<Produto>> ObterTodas(long idEmpresa)
        {
            return _produtoRepository.Obter(x=>x.idEmpresa == idEmpresa).Result.ToList();
        }

        public async Task<double> ObterPrecoAtual(long idProduto)
        {
            var produto = _produtoRepository.ObterPorId(idProduto).Result;
            return (produto != null && produto.NUPRECO.HasValue)? produto.NUPRECO.Value : 0.0;
        }

        public async Task AtualizarIBPTTodosProdutos()
        {

            try
            {
                await _dapperRepository.BeginTransaction();
                var produtos = await _produtoDapperRepository.ObterProdutosParaAtualizarIbpt();
                
                produtos.ForEach(async prod =>
                {
                    await _produtoDapperRepository. AtualizarIBPTPorProduto(prod);
                });

                if (!TemNotificacao())
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
        }
        #endregion

        #region ProdutoDepartamento
        public async Task<bool> Adicionar(ProdutoDepartamento produtoDepartamento)
        {
            if (!ExecutarValidacao(new ProdutoDepartamentoValidation(), produtoDepartamento))
                return false;

            await _produtoDepartamentoRepository.AdicionarSemSalvar(produtoDepartamento) ;
            return true;
        }

        public async Task<bool> ApagarDepartamento(long id)
        {
            if (await ExisteProdutoDepartamentoUtilizado(id))
            {
                Notificar("Não é possivel apagar este departamento pois o mesmo está sendo utilizado");
                return false;
            }
            await _produtoDepartamentoRepository.RemoverSemSalvar(id);
            return true;
        }

        public async Task<bool> Atualizar(ProdutoDepartamento produtoDepartamento)
        {
            if (!ExecutarValidacao(new ProdutoDepartamentoValidation(), produtoDepartamento))
                return false;

            await _produtoDepartamentoRepository.AtualizarSemSalvar(produtoDepartamento);
            return true; 
        }


        public async Task<bool> Existe(ProdutoDepartamento produtoDepartamento)
        {
            Expression<Func<ProdutoDepartamento, bool>> expression = x => x.CDDEP.Trim().ToUpper() == produtoDepartamento.CDDEP.Trim().ToUpper() && x.idEmpresa == produtoDepartamento.idEmpresa
                                                  && x.NMDEP.Trim().ToUpper() == produtoDepartamento.NMDEP.Trim().ToUpper();

            var result = _produtoDepartamentoRepository.Obter(expression).Result.Any();
            return result;

        }

        public async Task<PagedResult<ProdutoDepartamento>> ObterPaginacaoPorDescricao(long idempresa, string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _produtoDepartamentoRepository.Buscar(x => x.idEmpresa == idempresa && x.NMDEP.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<ProdutoDepartamento>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.CDDEP),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<ProdutoDepartamento> ObterPorIdDepartamento(long id)
        {
            return await _produtoDepartamentoRepository.ObterPorId(id);
        }
    

        public async Task<IEnumerable<ProdutoDepartamento>> ObterTodosDepartamento()
        {
            return await _produtoDepartamentoRepository.ObterTodos();
        }

        public async Task<IEnumerable<ProdutoDepartamento>> ObterTodosDepartamento(params string[] includes)
        {
            return await _produtoDepartamentoRepository.ObterTodos();
        }

        #endregion
            
        #region ProdutoMarca
        public async Task<bool> Adicionar(ProdutoMarca produtoMarca)
        {
            if (!ExecutarValidacao(new ProdutoMarcaValidation(), produtoMarca))
                return false;

            await _produtoMarcaRepository.AdicionarSemSalvar(produtoMarca);
            return true; 
        }

        public async Task<IEnumerable<ProdutoMarca>> ObterTodosMarca()
        {
            return await _produtoMarcaRepository.ObterTodos();
        }

        public async Task<IEnumerable<ProdutoMarca>> ObterTodosProdutoMarca(params string[] includes)
        {
            return await _produtoMarcaRepository.ObterTodos();
        }

        public async Task<ProdutoMarca> ObterPorIdMarca(long id)
        {
            return await _produtoMarcaRepository.ObterPorId(id);
        }

        public async Task<bool> Existe(ProdutoMarca produtoMarca)
        {
            Expression<Func<ProdutoMarca, bool>> expression = x => x.CDMARCA.Trim().ToUpper() == produtoMarca.CDMARCA.Trim().ToUpper() && x.idEmpresa == produtoMarca.idEmpresa
                                                  && x.NMMARCA.Trim().ToUpper() == produtoMarca.NMMARCA.Trim().ToUpper();

            var result = _produtoMarcaRepository.Obter(expression).Result.Any();
            return result;
        }

        public async Task<bool> Atualizar(ProdutoMarca produtoMarca)
        {
            if (!ExecutarValidacao(new ProdutoMarcaValidation(), produtoMarca))
                return false;

            await _produtoMarcaRepository.AtualizarSemSalvar(produtoMarca);
            return true;
        }

        public async Task<bool> ApagarProdutoMarca(long id)
        {
            if (await ExisteProdutoMarcaUtilizado(id))
            {
                Notificar("Não é possivel apagar esta marca pois o mesmo está sendo utilizado");
                return false;
            }
            await _produtoMarcaRepository.RemoverSemSalvar(id);
            return true;
        }

        public async Task<PagedResult<ProdutoMarca>> ObterMarcaPaginacaoPorDescricao(long idempresa, string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _produtoMarcaRepository.Buscar(x => x.idEmpresa == idempresa && x.NMMARCA.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<ProdutoMarca>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.CDMARCA),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }
        #endregion

        #region Grupo

        public async Task<bool> Adicionar(GrupoProduto grupoProduto)
        {
            if (!ExecutarValidacao(new GrupoProdutoValidation(), grupoProduto))
                return false;

            await _grupoProdutoRepository.AdicionarSemSalvar(grupoProduto);
            return true;
        }

        public async Task<IEnumerable<GrupoProduto>> ObterTodosGrupos()
        {
            return await _grupoProdutoRepository.ObterTodos();
        }

        public async Task<bool> Existe(GrupoProduto grupoProduto)
        {
            Expression<Func<GrupoProduto, bool>> expression = x => x.CDGRUPO.Trim().ToUpper() == grupoProduto.CDGRUPO.Trim().ToUpper() && x.idEmpresa == grupoProduto.idEmpresa
                                                 && x.Nome.Trim().ToUpper() == grupoProduto.Nome.Trim().ToUpper();

            var result = _grupoProdutoRepository.Obter(expression).Result.Any();
            return result;
        }

        public async Task<bool> Atualizar(GrupoProduto grupoProduto)
        {
            if (!ExecutarValidacao(new GrupoProdutoValidation(), grupoProduto))
                return false;

            await _grupoProdutoRepository.AtualizarSemSalvar(grupoProduto);
            return true;
        }

        public async Task<bool> ApagarProdutoGrupo(long id)
        {
            if (await ExisteGrupoUtilizado(id))
            {
                Notificar("Não é possivel apagar esta marca pois o mesmo está sendo utilizado");
                return false;
            }
            await _grupoProdutoRepository.RemoverSemSalvar(id);
            return true;
        }

        public async Task<PagedResult<GrupoProduto>> ObterGrupoPaginacaoPorDescricao(long idempresa, string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _grupoProdutoRepository.Buscar(x => x.idEmpresa == idempresa && x.Nome.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<GrupoProduto>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.CDGRUPO),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<GrupoProduto> ObterPorIdGrupo(long id)
        {
            return await _grupoProdutoRepository.ObterPorId(id);
        }

        #endregion

        #region SubGrupo

        public async Task<bool> Adicionar(SubGrupoProduto subGrupoProduto)
        {
            if (!ExecutarValidacao(new SubGrupoProdutoValidation(), subGrupoProduto))
                return false;

            await _subGrupoProdutoRepository.AdicionarSemSalvar(subGrupoProduto);
            return true;
        }

        public async Task<IEnumerable<SubGrupoProduto>> ObterTodosSubGrupos()
        {
            return await _subGrupoProdutoRepository.ObterTodos();
        }

        public async Task<bool> Existe(SubGrupoProduto subGrupoProduto)
        {
            Expression<Func<SubGrupoProduto, bool>> expression = x => x.NMSUBGRUPO.Trim().ToUpper() == subGrupoProduto.NMSUBGRUPO.Trim().ToUpper() && x.IDGRUPO == subGrupoProduto.IDGRUPO;

            var result = _subGrupoProdutoRepository.Obter(expression).Result.Any();
            return result;
        }

        public async Task<bool> Atualizar(SubGrupoProduto subGrupoProduto)
        {
            if (!ExecutarValidacao(new SubGrupoProdutoValidation(), subGrupoProduto))
                return false;

            await _subGrupoProdutoRepository.AtualizarSemSalvar(subGrupoProduto);
            return true;
        }

        public async Task<bool> ApagarProdutoSubGrupo(long id)
        {
            if (await ExisteGrupoUtilizado(id))
            {
                Notificar("Não é possivel apagar esta marca pois o mesmo está sendo utilizado");
                return false;
            }
            await _subGrupoProdutoRepository.RemoverSemSalvar(id);
            return true;
        }

        public async Task<PagedResult<SubGrupoProduto>> ObterSubGrupoPaginacaoPorDescricao(long idGrupo, string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _subGrupoProdutoRepository.Buscar(x => x.IDGRUPO == idGrupo && x.NMSUBGRUPO.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<SubGrupoProduto>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.NMSUBGRUPO),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<SubGrupoProduto> ObterPorIdSubGrupo(long id)
        {
            return await _subGrupoProdutoRepository.ObterPorId(id);
        }

        #endregion

        #region Codigo de Barra

        public async Task Adicionar(ProdutoCodigoBarra produto)
        {
         
            await _produtoCodigoBarraRepository.AdicionarSemSalvar(produto);
        }
    
        public async Task Atualizar(ProdutoCodigoBarra produto)
        {
            await _produtoCodigoBarraRepository.AtualizarSemSalvar(produto);
        }

        public async Task ApagarCodigoBarra(long id)
        {
            if (await ExisteCodigoBarraProdutoUtilizado(id))
            {
                Notificar("Não é possivel apagar esta codigo de barra pois o mesmo está sendo utilizado");
                return;
            }
            await _produtoCodigoBarraRepository.RemoverSemSalvar(id);
        }

        public async Task<ProdutoCodigoBarra> ObterCodigoBarraPorId(long id)
        {
            return await _produtoCodigoBarraRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<ProdutoCodigoBarra>> ObterTodosCodigoBarraPorProduto(long idProduto)
        {
            return await _produtoCodigoBarraRepository.Obter(x => x.IDPRODUTO == idProduto);
        }
        #endregion

        #region produtoPreco

        public async Task Adicionar(ProdutoPreco produto)
        {
            if (!ExecutarValidacao(new ProdutoPrecoValidation(), produto))
                return;

            await _produtoPrecoRepository.AdicionarSemSalvar(produto);
        }

        public async Task Atualizar(ProdutoPreco produto)
        {
            if (!ExecutarValidacao(new ProdutoPrecoValidation(), produto))
                return;

            await _produtoPrecoRepository.AtualizarSemSalvar(produto);
        }

        public async Task ApagarPreco(long id)
        {
            if (await ExistePrecoProdutoUtilizado(id))
            {
                Notificar("Não é possivel apagar este preco de produto pois o mesmo está sendo utilizado");
                return;
            }
            await _produtoPrecoRepository.RemoverSemSalvar(id);
        }

        public async Task<ProdutoPreco> ObterPrecoPorId(long id)
        {
            return await _produtoPrecoRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<ProdutoPreco>> ObterPrecoPorProduto(long idProduto)
        {
            return await _produtoPrecoRepository.Obter(x => x.idProduto == idProduto);
        }
        #endregion

        #region Produto Foto

        public async Task Adicionar(ProdutoFoto produto)
        {
            if (!ExecutarValidacao(new ProdutoFotoValidation(), produto))
                return;

            await _produtoFotoRepository.AdicionarSemSalvar(produto);
        }

        public async Task Atualizar(ProdutoFoto produto)
        {
            if (!ExecutarValidacao(new ProdutoFotoValidation(), produto))
                return;

            await _produtoFotoRepository.Atualizar(produto);

        }

        public async Task ApagarFoto(long id)
        {
            if (await ExisteFotoProdutoUtilizado(id))
            {
                Notificar("Não é possivel apagar esta foto de produto pois o mesmo está sendo utilizado");
                return;
            }
            await _produtoFotoRepository.RemoverSemSalvar(id);
        }

        public async Task<ProdutoFoto> ObterFotoPorId(long id)
        {
            return await _produtoFotoRepository.ObterPorId(id);
        }

        public async Task<IEnumerable<ProdutoFoto>> ObterFotoPorProduto(long idProduto)
        {
            return await _produtoFotoRepository.Obter(x => x.idProduto == idProduto);
        }

        #endregion

        #region private
        private async Task<bool> ExisteProdutoDepartamentoUtilizado(long idContato)
        {
            var resultado = false;
            //resultado = _produtoRepository.Obter(x => x.Id == idContato).Result.Any();

            return resultado;
        }

        private async Task<bool> ExisteProdutoMarcaUtilizado(long idContato)
        {
            var resultado = false;
            //resultado = _produtoRepository.Obter(x => x.Id == idContato).Result.Any();

            return resultado;
        }

        private async Task<bool> ExisteGrupoUtilizado(long idproduto)
        {
            var resultado = false;
            //resultado = _produtoRepository.Obter(x => x.Id == idContato).Result.Any();

            return resultado;
        }

        private async Task<bool> ExisteProdutoUtilizado(long idproduto)
        {
            var resultado = false;
            //resultado = _produtoRepository.Obter(x => x.Id == idContato).Result.Any();

            return resultado;
        }

        private async Task<bool> ExisteCodigoBarraProdutoUtilizado(long idproduto)
        {
            var resultado = false;
            //resultado = _produtoRepository.Obter(x => x.Id == idproduto).Result.Any();

            return resultado;
        }

        private async Task<bool> ExistePrecoProdutoUtilizado(long idproduto)
        {
            var resultado = false;
            //resultado = _produtoRepository.Obter(x => x.Id == idproduto).Result.Any();

            return resultado;
        }
        
        private async Task<bool> ExisteFotoProdutoUtilizado(long idproduto)
        {
            var resultado = false;
            //resultado = _produtoRepository.Obter(x => x.Id == idproduto).Result.Any();

            return resultado;
        }

        #endregion
    }
}
