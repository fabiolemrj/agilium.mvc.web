using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Interfaces.IService;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using FluentValidation;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    /// <summary>
    /// Servico para gerenciamento de CaUsuarioIdentity (IdentityUser para login).
    /// Opera como ponte entre o Identity e a entidade de dominio Usuario.
    /// O CaUsuarioIdentity.Id (string/GUID) vincula-se via Usuario.idUserAspNet.
    /// </summary>
    public class CaUsuarioService : BaseService, ICaUsuarioService
    {
        private readonly ICaUsuarioRepository _caUsuarioRepository;
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaAuthRepository _empresaAuthRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public CaUsuarioService(
            INotificador notificador,
            ICaUsuarioRepository caUsuarioRepository,
            IEmpresaAuthRepository empresaAuthRepository,
            IEmpresaRepository empresaRepository,
            IUsuarioRepository usuarioRepository) : base(notificador)
        {
            _caUsuarioRepository = caUsuarioRepository;
            _empresaAuthRepository = empresaAuthRepository;
            _empresaRepository = empresaRepository;
            _usuarioRepository = usuarioRepository;
        }

        #region Metodos Privados

        /// <summary>
        /// Valida o CaUsuarioIdentity manualmente (nao usa ExecutarValidacao do BaseService
        /// porque CaUsuarioIdentity nao herda de Entity).
        /// </summary>
        private bool ValidarCaUsuario(CaUsuarioIdentity caUsuario)
        {
            var validation = new CaUsuarioValidation();
            var result = validation.Validate(caUsuario);
            if (!result.IsValid)
            {
                foreach (var erro in result.Errors)
                    Notificar(erro.ErrorMessage);
                return false;
            }
            return true;
        }

        #endregion

        #region CRUD Basico

        public async Task<bool> Adicionar(CaUsuarioIdentity caUsuarioIdentity)
        {
            if (!ValidarCaUsuario(caUsuarioIdentity))
                return false;

            await _caUsuarioRepository.Adicionar(caUsuarioIdentity);
            return true;
        }

        public async Task<bool> Atualizar(CaUsuarioIdentity caUsuarioIdentity)
        {
            if (!ValidarCaUsuario(caUsuarioIdentity))
                return false;

            await _caUsuarioRepository.Atualizar(caUsuarioIdentity);
            return true;
        }

        public async Task<bool> Remover(string id)
        {
            try
            {
                await _caUsuarioRepository.Remover(id);
                return true;
            }
            catch
            {
                Notificar("Erro ao remover usuario. Pode haver vinculos ativos.");
                return false;
            }
        }

        public async Task<CaUsuarioIdentity> ObterPorId(string id)
        {
            return await _caUsuarioRepository.ObterPorId(id);
        }

        public async Task<List<CaUsuarioIdentity>> ObterTodos()
        {
            return await _caUsuarioRepository.ObterTodos();
        }

        public async Task<PagedResult<CaUsuarioIdentity>> ObterPaginado(string filtro, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var lista = await _caUsuarioRepository.ObterTodos();

            if (!string.IsNullOrEmpty(filtro))
                lista = lista.Where(x => x.Usuario?.nome?.ToUpper().Contains(filtro.ToUpper()) == true).ToList();

            return new PagedResult<CaUsuarioIdentity>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        #endregion

        #region Consultas Especificas

        public async Task<CaUsuarioIdentity> ObterPorUserName(string userName)
        {
            var lista = await _caUsuarioRepository.Buscar(x => x.UserName == userName, "Usuario");
            return lista.FirstOrDefault();
        }

        public async Task<CaUsuarioIdentity> ObterPorEmail(string email)
        {
            var lista = await _caUsuarioRepository.Buscar(x => x.Email == email, "Usuario");
            return lista.FirstOrDefault();
        }

        public async Task<CaUsuarioIdentity> ObterPorCpf(string cpf)
        {
            var lista = await _usuarioRepository.Buscar(x => x.cpf == cpf);
            var usuario = lista.FirstOrDefault();
            if (usuario == null) return null;

            return await _caUsuarioRepository.ObterPorId(usuario.idUserAspNet);
        }

        public async Task<CaUsuarioIdentity> ObterPorUserAspNetId(string idUserAspNet)
        {
            var lista = await _usuarioRepository.Buscar(x => x.idUserAspNet == idUserAspNet);
            var usuario = lista.FirstOrDefault();
            if (usuario == null) return null;

            return await _caUsuarioRepository.ObterPorId(usuario.idUserAspNet);
        }

        public async Task<List<CaUsuarioIdentity>> ObterPorNome(string nome)
        {
            var lista = await _caUsuarioRepository.Buscar(x => x.Usuario.nome.ToUpper().Contains(nome.ToUpper()), "Usuario");
            return lista.OrderBy(x => x.Usuario.nome).ToList();
        }

        public async Task<PagedResult<CaUsuarioIdentity>> ObterPorNomePaginado(string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var filtro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _caUsuarioRepository.Buscar(x => x.Usuario.nome.ToUpper().Contains(filtro.ToUpper()), "Usuario");

            return new PagedResult<CaUsuarioIdentity>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        #endregion

        #region Ativacao / Desativacao

        public async Task<bool> Ativar(string id)
        {
            var caUsuario = await _caUsuarioRepository.ObterPorId(id);
            if (caUsuario == null)
            {
                Notificar("Usuario nao encontrado.");
                return false;
            }

            caUsuario.LockoutEnabled = false;
            await _caUsuarioRepository.Atualizar(caUsuario);
            return true;
        }

        public async Task<bool> Desativar(string id)
        {
            var caUsuario = await _caUsuarioRepository.ObterPorId(id);
            if (caUsuario == null)
            {
                Notificar("Usuario nao encontrado.");
                return false;
            }

            caUsuario.LockoutEnabled = true;
            await _caUsuarioRepository.Atualizar(caUsuario);
            return true;
        }

        #endregion

        #region Empresas Associadas

        public async Task<List<EmpresaAuth>> ObterEmpresasPorUsuario(long id)
        {
            var empresas = await _empresaAuthRepository.Obter(x => x.IDUSUARIO == id, "Empresa");
            return empresas.ToList();
        }

        public async Task<EmpresaAuth> ObterEmpresaPorId(long idUsuario, long idEmpresa)
        {
            var empresas = await _empresaAuthRepository.Obter(x => x.IDEMPRESA == idEmpresa && x.IDUSUARIO == idUsuario);
            return empresas.FirstOrDefault();
        }

        public async Task<List<Empresa>> ObterEmpresasDisponiveisAssociacao(long idUsuario)
        {
            var empresasAssociadas = await ObterEmpresasPorUsuario(idUsuario);
            var todasEmpresas = await _empresaRepository.ObterTodos();

            return todasEmpresas
                .Where(x => empresasAssociadas.All(y => y.IDEMPRESA != x.Id))
                .ToList();
        }

        public async Task<bool> AssociarEmpresa(long idUsuario, long idEmpresa)
        {
            var existe = await ObterEmpresaPorId(idUsuario, idEmpresa);
            if (existe != null)
            {
                Notificar("Usuario ja esta associado a esta empresa.");
                return false;
            }

            var empresaAuth = new EmpresaAuth(idEmpresa, idUsuario);
            await _empresaAuthRepository.AdicionarSemSalvar(empresaAuth);
            await _empresaAuthRepository.SaveChanges();
            return true;
        }

        public async Task<bool> DesassociarEmpresa(long idUsuario, long idEmpresa)
        {
            var empresaAuth = await ObterEmpresaPorId(idUsuario, idEmpresa);
            if (empresaAuth == null)
            {
                Notificar("Associacao nao encontrada.");
                return false;
            }

            await _empresaAuthRepository.RemoverSemSalvar(empresaAuth);
            await _empresaAuthRepository.SaveChanges();
            return true;
        }

        public async Task<bool> DesassociarTodasEmpresas(long idUsuario)
        {
            var empresas = await ObterEmpresasPorUsuario(idUsuario);
            await _empresaAuthRepository.RemoverSemSalvar(empresas);
            await _empresaAuthRepository.SaveChanges();
            return true;
        }

        #endregion

        #region Transacional

        public async Task Salvar()
        {
            _caUsuarioRepository?.SaveChanges();
        }

        #endregion

        public void Dispose()
        {
            _caUsuarioRepository?.Dispose();
            _empresaAuthRepository?.Dispose();
            _empresaRepository?.Dispose();
            _usuarioRepository?.Dispose();
        }
    }
}
