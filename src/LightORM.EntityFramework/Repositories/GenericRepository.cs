using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Threading.Tasks;

namespace LightORM.EntityFramework.Repositories
{
    public class GenericRepository<TEntity>: IRepository<TEntity> where TEntity : class
    {
        private DbContext Context { get; set; }

        public GenericRepository(DbContext context)
        {
            Context = context;
        }


        public TEntity Get(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).FirstOrDefault();
        }

        public IEnumerable<TEntity> GetAll(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).ToList();
        }

        public int GetCount(Expression<Func<TEntity, bool>> predicate)
        {
            return Context.Set<TEntity>().Where(predicate).Count();
        }

        public int Insert(TEntity entity)
        {
            Context.Set<TEntity>().Add(entity);
            return 1;
        }
        public async Task<int> InsertAsync(TEntity entity)
        {
            await Context.Set<TEntity>().AddAsync(entity);
            return 1;
        }

        public int Update(TEntity entity)
        {
            Context.Set<TEntity>().Update(entity);
            return 1;
        }
        public Task<int> UpdateAsync(TEntity entity)
        {
            return Task.FromResult(Update(entity));
        }

        public int Delete(TEntity entity)
        {
            Context.Set<TEntity>().Remove(entity);
            return 1;
        }

        public Task<int> DeleteAsync(TEntity entity)
        {
            return Task.FromResult(Delete(entity));
        }
    }
}
