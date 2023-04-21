using Microsoft.EntityFrameworkCore;

namespace LightORM.EntityFramework.UnitOfWorks
{
    public interface IUnitOfWork
    {
        // <summary>
        /// Database Context
        /// </summary>
        public DbContext Context { get; }

        /// <summary>
        /// Saves the change.
        /// </summary>
        /// <returns></returns>
        void SaveChange();
    }
}
