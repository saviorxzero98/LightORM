using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LightORM.EntityFramework.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        int GetCount(Expression<Func<TEntity, bool>> predicate);

        int Insert(TEntity entity);
        Task<int> InsertAsync(TEntity entity);

        int Update(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);

        int Delete(TEntity entity);

        Task<int> DeleteAsync(TEntity entity);
    }
}
