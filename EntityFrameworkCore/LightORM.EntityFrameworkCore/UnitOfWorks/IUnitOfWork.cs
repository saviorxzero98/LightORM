using Microsoft.EntityFrameworkCore;

namespace LightORM.EntityFrameworkCore.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        // <summary>
        /// Database Context
        /// </summary>
        DbContext Context { get; }

        /// <summary>
        /// Saves the change.
        /// </summary>
        /// <returns></returns>
        Task<int> SaveChangeAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
