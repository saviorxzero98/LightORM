using LightORM.EntityFrameworkCore.DataQuery;
using LightORM.EntityFrameworkCore.Extensions;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace LightORM.EntityFrameworkCore.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        private DbContext Context { get; set; }

        public GenericRepository(DbContext context)
        {
            Context = context;
        }


        /// <summary>
        /// Get
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public TEntity? Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).FirstOrDefault();
        }

        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public List<TEntity> GetList(Expression<Func<TEntity, bool>>? predicate, 
                                     DataQueryOptions? options)
        {
            IQueryable<TEntity> queryable = Context.Set<TEntity>();

            if (predicate != null) 
            {
                queryable = queryable.Where(predicate);
            }

            queryable = queryable.Sort(options)
                                 .Page(options);

            return queryable.ToList();
        }

        /// <summary>
        /// Get List
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public List<TEntity> GetList(DataQueryOptions options)
        {
            IQueryable<TEntity> queryable = Context.Set<TEntity>();

            queryable = queryable.Filter(options.Filters)
                                 .Sort(options)
                                 .Page(options);

            return queryable.ToList();
        }


        /// <summary>
        /// Get Count
        /// </summary>
        /// <param name="predicate"></param>
        /// <returns></returns>
        public int GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).Count();
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            return 1;
        }
        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            return 1;
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            return 1;
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            return 1;
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public Task<int> DeleteAsync(TEntity entity)
        {
            return Task.FromResult(Delete(entity));
        }
    
        /// <summary>
        /// Get Queryable
        /// </summary>
        /// <returns></returns>
        public Task<IQueryable<TEntity>> GetQueryableAsync()
        {
            IQueryable<TEntity> queryable = Context.Set<TEntity>();
            return Task.FromResult(queryable);
        }
    }
}
