using agilium.api.business.Interfaces;
using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Services
{
    public class UsuarioFotoService : IUsuarioFotoService
    {
        private readonly IRepositoryMongo<UsuarioFoto> _usuarioFotoRepository;

        public UsuarioFotoService(IRepositoryMongo<UsuarioFoto> usuarioFotoRepository)
        {
            _usuarioFotoRepository = usuarioFotoRepository;
        }

        public void Insert(UsuarioFoto usuarioFoto)
        {
            _usuarioFotoRepository.Insert(usuarioFoto);
        }

        public UsuarioFoto Query(Expression<Func<UsuarioFoto, bool>> filter)
        {
            var result = _usuarioFotoRepository.Query( filter);

            return result;
        }

        public UsuarioFoto Query(string id)
        {
            Expression<Func<UsuarioFoto, bool>> filtro = s => s.IdUsuarioAspNet == id;
            var result = _usuarioFotoRepository.Query(filtro);
            return result;
        }
        public IQueryable<UsuarioFoto> QueryAll()
        {
            var result = _usuarioFotoRepository.QueryAll();

            return result;
        }

        public async Task RemoveAsync(string id)
        {
            Expression<Func<UsuarioFoto, bool>> filtro = s => s.IdUsuarioAspNet == id;
            await _usuarioFotoRepository.RemoveAsync(filtro);
        }

        public async Task Save(UsuarioFoto obj)
        {
            Expression<Func<UsuarioFoto, bool>> filtro = s => s.IdUsuarioAspNet == obj.IdUsuarioAspNet;
            var usuarioFotoExistente = Query(obj.IdUsuarioAspNet);
           
            if(usuarioFotoExistente != null)
            {
                Update(obj.IdUsuarioAspNet, obj);
            }
            else
            {
                Insert(obj);
            }
        }

        public void Update(string id, UsuarioFoto usuarioFoto)
        {
            Expression<Func<UsuarioFoto, bool>> filtro = s => s.IdUsuarioAspNet == id;

            _usuarioFotoRepository.Update(filtro, usuarioFoto);
        }
    }
}
