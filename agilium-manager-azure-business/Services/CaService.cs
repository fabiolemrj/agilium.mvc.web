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
    public class CaService : BaseService, ICaService
    {
        private readonly ICaPerfilRepository _caPerfilRepository;
        private readonly ICaPermissaoItemRepository _caPermissaoItemRepository;
        private readonly ICaModeloRepository _caModeloRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly ICaAreaManagerRepository _caAreaManagerRepository;
        private readonly ICaPerfilManagerRepository _caPerfilManagerRepository;
        private readonly ICaPermissaoManagerRepository  _caPermissaoManagerRepository;
        private readonly ICaRepositoryDapper _caRepositoryDapper;
        private readonly IDapperRepository _dapperRepository;

        public CaService(INotificador notificador, ICaPerfilRepository caPerfilRepository,
            ICaPermissaoItemRepository caPermissaoItemRepository, ICaModeloRepository caModeloRepository,
            IUsuarioRepository usuarioRepository, ICaAreaManagerRepository caAreaManagerRepository,
            ICaPerfilManagerRepository caPerfilManagerRepository, IDapperRepository dapperRepository,
            ICaPermissaoManagerRepository caPermissaoManagerRepository,ICaRepositoryDapper caRepositoryDapper) : base(notificador)
        {
            _caPerfilRepository = caPerfilRepository;
            _caPermissaoItemRepository = caPermissaoItemRepository;
            _caModeloRepository = caModeloRepository;
            _usuarioRepository = usuarioRepository;
            _caAreaManagerRepository = caAreaManagerRepository;
            _caPerfilManagerRepository = caPerfilManagerRepository;
            _caPermissaoManagerRepository = caPermissaoManagerRepository;
            _caRepositoryDapper = caRepositoryDapper;
            _dapperRepository = dapperRepository;
        }

        #region perfil
        public async Task<bool> AdicionarPerfil(CaPerfil caPerfil)
        {
            if (!ExecutarValidacao(new CaPerfilValidation(), caPerfil)) return false;

            await _caPerfilRepository.Adicionar(caPerfil);
            return true;
        }


        public async Task<bool> ApagarPerfil(long idPerfil)
        {
            var perfilUsado = false;

            if (perfilUsado)
            {
                Notificar("Este Perfil não pode ser apagado, pois é utilizado em um modelo existente!");
                return false;
            }
            await _caPerfilRepository.Remover(idPerfil);
            return true;
        }


        public async Task<bool> AtivarPerfil(long idPerfil)
        {
            var perfil = await ObterPerfilPorId(idPerfil);
            if (perfil == null)
            {
                return false;
            }
            perfil.AlterarSituacao(perfil.Situacao == "A" ? "I" : "A");
            await AtualizarPerfil(perfil);
            return true;
        }

        public async  Task<bool> AtualizarPerfil(CaPerfil caPerfil)
        {
            if (!ExecutarValidacao(new CaPerfilValidation(), caPerfil)) return false;

            await _caPerfilRepository.AtualizarSemSalvar(caPerfil);
            return true;
        }

        public void Dispose()
        {
            _caPerfilRepository?.Dispose();
            _caPermissaoItemRepository?.Dispose();
            _caModeloRepository?.Dispose();
            _usuarioRepository?.Dispose();
            _caAreaManagerRepository?.Dispose();
            _caPerfilManagerRepository?.Dispose();
            _caPermissaoManagerRepository?.Dispose();
        }


        public async Task<CaPerfil> ObterPerfilPorDescricao(string descricao)
        {
            var perfis = await _caPerfilRepository.Buscar(x => x.Descricao.ToUpper().Contains(descricao.ToUpper()));
            return perfis.FirstOrDefault();
        }

        public async Task<CaPerfil> ObterPerfilPorId(long idPerfil)
        {
            return await _caPerfilRepository.ObterPorId(idPerfil);
        }

        public async Task<CaPerfil> ObterCompletoPerfilPorId(long idPerfil)
        {
            var perfis = await _caPerfilRepository.Buscar(x => x.Id == idPerfil, "Modelos");
            return perfis.FirstOrDefault();
        }

        public async Task<IEnumerable<CaPerfil>> ObterTodosPerfis()
        {
            return _caPerfilRepository.ObterTodos().Result;
        }

        public async Task<PagedResult<CaPerfil>> ObterUsuariosPorDescricao(long idempresa, string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _caPerfilRepository.Buscar(x => x.idEmpresa == idempresa && x.Descricao.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<CaPerfil>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.Descricao),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        #endregion

        #region Permissao Item
        public async Task<bool> AdicionarPermissaoItem(CaPermissaoItem caPermissaoItem)
        {
            if (!ExecutarValidacao(new CaPermissaoItemValidation(), caPermissaoItem)) return false;

            await _caPermissaoItemRepository.Adicionar(caPermissaoItem);
            return true;
        }
            
        public async Task<bool> ApagarPermissaoItem(long id)
        {

            var permissaoItemUsado = false;

            if (permissaoItemUsado)
            {
                Notificar("Esta Permissao não pode ser apagada, pois é utilizado em um modelo existente!");
                return false;
            }
            await _caPermissaoItemRepository.Remover(id);
            return true;
        }

        public async Task<bool> AtivarPermissao(long id)
        {
            var permissao = _caPermissaoItemRepository.ObterPorId(id).Result;
            if (permissao == null)
            {
                return false;
            }
            permissao.MudarSituacao(permissao.Situacao == "A" ? "I" : "A");
            await _caPermissaoItemRepository.Atualizar(permissao);
            return true;
        }


        public async Task<PagedResult<CaPermissaoItem>> ObterPermissaoItemPorDescricao(string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _caPermissaoItemRepository.Buscar(x => x.Descricao.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<CaPermissaoItem>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList().OrderBy(x => x.Descricao),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<IEnumerable<CaPermissaoItem>> ObterTodosListaPermissao()
        {
            return await _caPermissaoItemRepository.ObterTodos();
        }

        public async Task<IEnumerable<Usuario>> ObterUsuariosPorPerfil(long idPerfil)
        {
            return _usuarioRepository.Obter(x => x.idPerfil == idPerfil).Result;
        }

        #endregion

        #region Modelos
        public async Task<bool> ApagarModeloItem(long idModelo)
        {
            await _caModeloRepository.Remover(idModelo);
            return true;
        }

    

        public Task DisposeAppUser()
        {
            throw new NotImplementedException();
        }

        public async Task<IEnumerable<CaModelo>> ObterModelosPorPerfil(long idPerfil)
        {
            return _caModeloRepository.Obter(x => x.idPerfil == idPerfil, "CaPerfil", "CaPermissaoItem").Result;
        }

        public async Task<bool> AdicionarModeloItem(CaModelo caModelo)
        {
            if (!ExecutarValidacao(new CaModeloValidation(), caModelo)) return false;

            await _caModeloRepository.Adicionar(caModelo);
            return true;
        }
        #endregion

        #region PerfilWeb
        public async Task<int> Salvar()
        {
            return await _caModeloRepository?.SaveChanges();
        }

        public async Task<bool> ApagarModelosPorPerfil(CaModelo caModelo)
        {
            var modelo = _caModeloRepository.Obter(x => x.idPerfil == caModelo.idPerfil && x.idPermissao == caModelo.idPermissao).Result.FirstOrDefault();
            if(modelo == null) return false;
           
            await _caModeloRepository.Remover(modelo.Id);
            return true;
        }

        public async Task<bool> ApagarModeloItemInd(long idPermissao,  long idPerfil)
        {
            var modelo = _caModeloRepository.Obter(x => x.idPerfil == idPerfil && x.idPermissao == idPermissao).Result.FirstOrDefault();
            if (modelo == null) return false;

            await _caModeloRepository.Remover(modelo.Id);
            return true;
        }

        private async Task<bool> ApagarModeloItemInd(long idModelo)
        {
            var modelo = _caModeloRepository.Obter(x => x.Id == idModelo).Result.FirstOrDefault();
            if (modelo == null) return false;

            await _caModeloRepository.RemoverSemSalvar(idModelo);
            return true;
        }


        public async Task<bool> ApagarModelosPorPerfil(long idModelo)
        {
            var modelos = ObterModelosPorPerfil(idModelo).Result.ToList();

            var novosModelos = new List<CaModelo>();
            modelos.ToList().ForEach(m => {
                novosModelos.Add(m);
            });

            modelos.Clear();

            await _caModeloRepository.RemoverSemSalvar(novosModelos);
            //novosModelos.ToList().ForEach(async x => await ApagarModeloItemInd(x.Id));

            return true;
        }

        public void ApagarModelos(IEnumerable<CaModelo> modelos)
        {
            //_caModeloRepository.RemoverSemSalvar(modelos);
            //_caModeloRepository.SaveChanges();
            modelos.ToList().ForEach(mod => {
                ApagarModelos(mod);
            });
        }

        public void AtualizarPerfilSincr(CaPerfil perfil)
        {
            //perfil.Modelos.ToList().ForEach(mod => {
            //    AdicionarModeloSincronizaa(mod);
            //});
            _caPerfilRepository.AtualizarSincrona(perfil);
            _caPerfilRepository.SaveChanges();
        }



        public void AdicionarModeloSincronizaa(CaModelo caModelo)
        {
            _caModeloRepository.AdicionarSincrona(caModelo);
            _caModeloRepository.SaveChanges();
        }

        public void AtualizarModeloSincronizaa(CaModelo caModelo)
        {
            _caModeloRepository.AtualizarSincrona(caModelo);
        }

        public void ApagarModelos(CaModelo modelo)
        {
            _caModeloRepository.RemoverSemSalvar(modelo);
            _caModeloRepository.SaveChanges();
        }
        #endregion

        #region CaManager
        public async Task<IEnumerable<CaAreaManager>> ObterTodasCaAreas()
        {
            var perfis = await _caAreaManagerRepository.ObterTodos();
            return perfis.OrderBy(x=>x.hierarquia).OrderBy(x=>x.ordem);
        }

        public async Task<IEnumerable<CaPerfiManager>> ObterTodosCaPerfilPorDescricao(string descricao)
        {
            var perfis = await _caPerfilManagerRepository.Buscar(x=>x.Descricao.ToUpper().Contains(descricao.ToUpper()));
            return perfis.OrderBy(x => x.Descricao);
        }

        public async Task<CaPerfiManager> ObterPerfilManagerPorId(int id)
        {
            return  _caPerfilManagerRepository.Obter(x=>x.IdPerfil == id).Result.FirstOrDefault();
        }

        public async Task<CaPerfiManager> ObterCompletoPerfilManagerPorId(int idPerfil)
        {
            var perfil = await _caPerfilManagerRepository.Buscar(x => x.IdPerfil == idPerfil, "CaPermissaoManagers");
            return perfil.FirstOrDefault();
        }

        public async Task<PagedResult<CaPerfiManager>> ObterPerfilPorDescricaoPaginacao(string descricao, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(descricao) ? string.Empty : descricao;

            var lista = await _caPerfilManagerRepository.Buscar(x => x.Descricao.ToUpper().Contains(_nomeParametro.ToUpper()));

            return new PagedResult<CaPerfiManager>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> AdicionarPerfil(CaPerfiManager perfil)
        {
            if (!ExecutarValidacao(new CaPerfilManagerValidation(), perfil)) return false;

            await _caPerfilManagerRepository.Adicionar(perfil);
            return true;
        }

        public async Task<bool> AtualizarPerfil(CaPerfiManager perfil)
        {
            if (!ExecutarValidacao(new CaPerfilManagerValidation(), perfil)) return false;

            await _caPerfilManagerRepository.Atualizar(perfil);
            return true;
        }

        public async Task<bool> AdicionarPermissoes(List<CaPermissaoManager> permissoes)
        {
            var perfil = permissoes.FirstOrDefault();
          
            try
            {
                await _dapperRepository.BeginTransaction();
                var msgErro = "Erro ao tentar adicionar permissoes ao perfil";

                if (await _caRepositoryDapper.RemoverPermissoesPorPerfil(perfil.IdPerfil)) 
                {
                    foreach (var permissao in permissoes)
                    {
                        if (!_caRepositoryDapper.AdicionarPermissaoPorPerfil(permissao).Result)
                        {
                            Notificar(msgErro);
                            break;
                        }
                    }
                }  
                else Notificar(msgErro);
                  
                if (!TemNotificacao())
                {
                    await _dapperRepository.Commit();
                    return true;
                }                    
                else
                {
                    await _dapperRepository.Rollback();
                    Notificar(msgErro);
                    return false;
                }
            }
            catch (Exception ex)
            {
                Notificar(ex.Message);
                return false;
            }
             
        }

        public async Task<IEnumerable<CaPerfiManager>> ObterTodosCaPerfilManager()
        {
            return await _caPerfilManagerRepository.ObterTodos();
        }

        private async Task<bool> AdicionarPermissoesPerfil(List<CaPermissaoManager> permissoes)
        {
            permissoes.ForEach(x => {
                _caPermissaoManagerRepository.AdicionarSemSalvar(x);
            });
            return true;
        }

        private async Task<bool> ApagarCaPermissoesPorPerfil(long id)
        {
            if (!PodeApagarPermissaoPorPerfil(id).Result)
            {
                Notificar("Não foi possível apagar as permissoes do perfil!");
                return false;
            }
            var lista = _caPermissaoManagerRepository.Obter(x => x.IdPerfil == id).Result.ToList();
            lista.ForEach(x =>
            {
                _caPermissaoManagerRepository.RemoverSemSalvar(x);
            });

            return true;
        }

        private async Task<bool> PodeApagarPermissaoPorPerfil(long idContato)
        {
            return true;
        }

   
        #endregion
    }
}
