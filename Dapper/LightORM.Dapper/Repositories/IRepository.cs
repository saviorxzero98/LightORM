using System.Collections.Generic;
using System.Threading.Tasks;
using static LightORM.Dapper.Extensions.SqlKataExtenstions;

namespace LightORM.Dapper.Repositories
{
    public interface IRepository<TEntity> where TEntity : class
    {
        string GetTableName();

        TEntity Get(QueryOptions options);

        IEnumerable<TEntity> GetAll(QueryOptions options);

        int GetCount(QueryOptions options);

        int Insert(TEntity entity);
        Task<int> InsertAsync(TEntity entity);

        int Update(TEntity entity, List<WhereCondition> whereConditions);
        Task<int> UpdateAsync(TEntity entity, List<WhereCondition> whereConditions);

        int Delete(List<WhereCondition> whereConditions);
        Task<int> DeleteAsync(List<WhereCondition> whereConditions);
    }
}
