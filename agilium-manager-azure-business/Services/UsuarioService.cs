using agilium.api.business.Interfaces;
using agilium.api.business.Interfaces.IRepository;
using agilium.api.business.Models;
using agilium.api.business.Models.Validations;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class UsuarioService : BaseService, IUsuarioService
    {
        private readonly IUsuarioRepository _usuarioRepository;
        private readonly IEmpresaAuthRepository _empresaAuthRepository;
        private readonly IEmpresaRepository _empresaRepository;

        public UsuarioService(INotificador notificador,
                                IUsuarioRepository usuarioRepository,
                                IEmpresaAuthRepository empresaAuthRepository,
                                IEmpresaRepository empresaRepository) : base(notificador)
        {
            _usuarioRepository = usuarioRepository;
            _empresaAuthRepository = empresaAuthRepository;
            _empresaRepository = empresaRepository;
        }

        public async Task<bool> Adicionar(Usuario usuario)
        {
            if (!ExecutarValidacao(new UsuarioValidation(), usuario)) return false;
         
            await _usuarioRepository.Adicionar(usuario);

            return true;
        }

        public async Task Adicionar(EmpresaAuth empresaAuth)
        {
            await _empresaAuthRepository.AdicionarSemSalvar(empresaAuth);
        }

        public async Task AdicionarLista(IEnumerable<EmpresaAuth> empresaAuth)
        {
            await _empresaAuthRepository.AdicionarLista(empresaAuth) ;
            await _empresaAuthRepository.SaveChanges();
        }

        public async Task<bool> AtivarUsuario(long id)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterPorId(id);

                if (usuario == null)
                    return false;
                usuario.Ativar();
                await Atualizar(usuario);

                return true;
            }
            catch
            {

                return false;
            }
        }

        public async Task<bool> Atualizar(Usuario usuario)
        {
            if (!ExecutarValidacao(new UsuarioValidation(), usuario)) return false;

            await _usuarioRepository.Atualizar(usuario);

            return true;
        }

        public async Task Atualizar(EmpresaAuth empresaAuth)
        {
            await _empresaAuthRepository.AtualizarSemSalvar(empresaAuth);
        }

        public async Task<bool> AtualizarFoto(Usuario usuario)
        {
            if (!ExecutarValidacao(new UsuarioFotoValidation(), usuario)) return false;

            await _usuarioRepository.Atualizar(usuario);

            return true;
        }

        public async Task AtualizarLista(IEnumerable<EmpresaAuth> empresaAuth)
        {
            await _empresaAuthRepository.AtualizarLista(empresaAuth); 
        }

        public async Task<bool> DesativarUsuario(long id)
        {
            try
            {
                var usuario = await _usuarioRepository.ObterPorId(id);
                
                if (usuario == null)
                    return false;

                usuario.Desativar();
                await Atualizar(usuario);

                return true;
            }
            catch
            {

                return false;
            }
        }

        public void Dispose()
        {
            _usuarioRepository.Dispose();
            _empresaAuthRepository?.Dispose();
        }

        public async Task<EmpresaAuth> ObterEmpresaPorId(long idUsuario, long idEmpresa)
        {
            return _empresaAuthRepository.Obter(x=>x.IDEMPRESA == idEmpresa && x.IDUSUARIO == idUsuario).Result.FirstOrDefault();
        }

        public async Task<List<Empresa>> ObterEmpresasDisponiveisAssociacao(long idUsuario)
        {
            var listaEmpresaPorUsuario = await ObterEmpresasPorUsuario(idUsuario);
            var listaEmpresas = await _empresaRepository.ObterTodos();

            var listaResultado = listaEmpresas.Where(x => listaEmpresaPorUsuario.ToList().All(y => y.IDEMPRESA != x.Id)).ToList();

            return listaResultado;
        }

        public async Task<List<EmpresaAuth>> ObterEmpresasPorUsuario(long id)
        {
            return _empresaAuthRepository.Obter(x => x.IDUSUARIO == id,"Empresa").Result.ToList();
        }

        public async Task<Usuario> ObterPorUsuarioAspNetPorId(string id)
        {
            var listaUsuarios = await _usuarioRepository.Buscar(x => x.idUserAspNet == id);
            return listaUsuarios.FirstOrDefault();
        }

        public async Task<Usuario> ObterPorUsuarioPorCpf(string idUserAspNet)
        {
            var listaUsuarios = await _usuarioRepository.Buscar(x => x.cpf == idUserAspNet);
            return listaUsuarios.FirstOrDefault();
        }

        public async Task<Usuario> ObterPorUsuarioPorId(long id)
        {
            return await _usuarioRepository.ObterPorId(id); 
        }

        public async Task<List<Usuario>> ObterTodosUsuarios()
        {
            return await _usuarioRepository.ObterTodos();
        }

        public async Task<List<Usuario>> ObterTodosUsuariosValidos()
        {
            var retorno = new List<Usuario>();
            var usuarios = await _usuarioRepository.ObterTodos();
            usuarios.ForEach(usu =>
            {
                if (!string.IsNullOrEmpty(usu.nome))
                    retorno.Add(usu);
            });
            return retorno;
        }

        public async Task<PagedResult<Usuario>> ObterTodosUsuarios( int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var lista = await _usuarioRepository.ObterTodos();

            return new PagedResult<Usuario> { 
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };

           // return lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList();
        }

        public async Task<List<Usuario>> ObterUsuariosPorNome(string nome)
        {
            var lista = await _usuarioRepository.Buscar(x => x.nome.ToUpper().Contains(nome.ToUpper()), "EmpresasAuth");
            return lista.OrderBy(usuario => usuario.nome).ToList();
        }

        public async Task<PagedResult<Usuario>> ObterUsuariosPorNome(string nome, int page = 1, int pageSize = 15)
        {
            int pagina = page > 0 ? page : 1;
            var _nomeParametro = string.IsNullOrEmpty(nome) ? string.Empty : nome;

            var lista = await _usuarioRepository.Buscar(x => x.nome.ToUpper().Contains(_nomeParametro.ToUpper()), "EmpresasAuth");

            return new PagedResult<Usuario>
            {
                List = lista.Skip((pagina - 1) * pageSize).Take(pageSize).ToList(),
                TotalResults = lista.Count(),
                PageIndex = page,
                PageSize = pageSize
            };
        }

        public async Task<bool> Remover(long id)
        {
            try
            {
                await _usuarioRepository.Remover(id);
                return true;
            }
            catch
            {
                return false;
            }
        
        }

        public async Task<bool> Remover(long idUsuario, long idEmpresa)
        {
            try
            {
                var empresaAuth = ObterEmpresaPorId(idUsuario, idEmpresa).Result;
                await _empresaAuthRepository.RemoverSemSalvar(empresaAuth);
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> RemoverEmpresaUsuarioPorUsuario(long idUsuario)
        {
            var empresas = ObterEmpresasPorUsuario(idUsuario).Result;
            //empresas.ForEach(async empresa => {
            //    await Remover(empresa.IDUSUARIO, empresa.IDEMPRESA);
            //});
            await RemoverLista(empresas);
            return true;
        }

        public async Task RemoverLista(IEnumerable<EmpresaAuth> empresaAuth)
        {
            await _empresaAuthRepository.RemoverSemSalvar(empresaAuth) ;
            await _empresaAuthRepository.SaveChanges();
        }

        public async Task Salvar()
        {
            _usuarioRepository?.SaveChanges();
        }
    }
}
