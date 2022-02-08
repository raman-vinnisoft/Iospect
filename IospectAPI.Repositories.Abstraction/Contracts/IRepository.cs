using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace IospectAPI.Repositories.Abstraction.Contracts
{
    public  interface IRepository<T>
    {
        Task<T> Add(T entity);
        Task<T> Get(Expression<Func<T, bool>> expression);
        Task<List<T>> GetAll(Expression<Func<T, bool>> expression = null);
        Task<bool> Delete(long id);
        Task<bool> Update(T entity);

    }
}
