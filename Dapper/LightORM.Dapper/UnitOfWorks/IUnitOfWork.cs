using System;
using System.Data;
using System.Threading.Tasks;

namespace LightORM.Dapper.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Get Database Connection
        /// </summary>
        IDbConnection Connection { get; }

        /// <summary>
        /// Saves the change.
        /// </summary>
        /// <returns></returns>
        Task<bool> SaveChangeAsync();
    }
}
