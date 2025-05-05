using agilium.api.business.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces
{
    public interface IUsuarioFotoService
    {
        IQueryable<UsuarioFoto> QueryAll();

        UsuarioFoto Query(Expression<Func<UsuarioFoto, bool>> filter);
        UsuarioFoto Query(string id);
        void Insert(UsuarioFoto usuarioFoto);
        void Update(string id, UsuarioFoto usuarioFoto);

        Task Save(UsuarioFoto obj);
        Task RemoveAsync(string id);
    }

    public interface IUsuarioFotoEntityService:IDisposable
    {
        Task<bool> Adicionar(UsuarioFotoEntity usuario);
        Task<bool> Atualizar(UsuarioFotoEntity usuario);
        Task<bool> AtualizarFoto(UsuarioFotoEntity usuario);
        Task<bool> Remover(long id);
        Task<UsuarioFotoEntity> ObterPorUsuarioFotoPorId(long id);
        Task<UsuarioFotoEntity> ObterPorUsuarioFotoPorId(string id);
    }
}
