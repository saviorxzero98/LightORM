using Microsoft.EntityFrameworkCore;

namespace LightORM.EntityFramework.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        public DbContext Context { get; private set; }

        public UnitOfWork(DbContext context)
        {
            Context = context;
        }

        public void Dispose()
        {
            Context.Dispose();
        }

        public void SaveChange()
        {
            Context.SaveChanges();
        }
    }
}
