using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace agilium.api.business.Interfaces
{
    public interface IRepositoryMongo<T>
    {
        IQueryable<T> QueryAll();
        void Update(Expression<Func<T, bool>> filter, T obj);
        T Query(Expression<Func<T, bool>> filter);
        void Insert(T obj);
        Task RemoveAsync(Expression<Func<T, bool>> filter);
        Task Replace(Expression<Func<T, string>> filter, string campo);
    }
}
