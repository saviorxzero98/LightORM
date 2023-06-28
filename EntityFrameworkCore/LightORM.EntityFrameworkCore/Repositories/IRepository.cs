using System.Linq.Expressions;

namespace LightORM.EntityFrameworkCore.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        TEntity Get(Expression<Func<TEntity, bool>> predicate);

        List<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate);

        int GetCount(Expression<Func<TEntity, bool>> predicate);

        int Insert(TEntity entity);
        Task<int> InsertAsync(TEntity entity);

        int Update(TEntity entity);
        Task<int> UpdateAsync(TEntity entity);

        int Delete(TEntity entity);

        Task<int> DeleteAsync(TEntity entity);
    }
}
