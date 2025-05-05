using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class UsuarioFotoEntityService : BaseService, IUsuarioFotoEntityService
    {
        private readonly IUsuarioFotoRepository _usuarioRepository;
        public UsuarioFotoEntityService(INotificador notificador,
            IUsuarioFotoRepository usuarioRepository) : base(notificador)
        {
            _usuarioRepository = usuarioRepository;
        }

        public async Task<bool> Adicionar(UsuarioFotoEntity usuario)
        {
            await _usuarioRepository.Adicionar(usuario);

            return true;
        }

        public async Task<bool> Atualizar(UsuarioFotoEntity usuario)
        {
            await _usuarioRepository.Atualizar(usuario);

            return true;
        }

        public async Task<bool> AtualizarFoto(UsuarioFotoEntity usuario)
        {
            await _usuarioRepository.Atualizar(usuario);

            return true;
        }

        public void Dispose()
        {
            _usuarioRepository.Dispose();
        }

        public async Task<UsuarioFotoEntity> ObterPorUsuarioFotoPorId(long id)
        {
            var listaUsuarios = await _usuarioRepository.Buscar(x => x.Id == id);
            return listaUsuarios.FirstOrDefault();
        }

        public async Task<UsuarioFotoEntity> ObterPorUsuarioFotoPorId(string id)
        {
            var listaUsuarios = await _usuarioRepository.Buscar(x => x.IdUsuarioAspNet == id);
            return listaUsuarios.FirstOrDefault();
        }

        public async Task<bool> Remover(long id)
        {
            await _usuarioRepository.Remover(id);
            return true;
        }
    }
}
