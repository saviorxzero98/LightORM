using System;
using System.Data;

namespace LightORM.UnitOfWorks
{
    public interface IUnitOfWork : IDisposable
    {
        /// <summary>
        /// Get Database Connection
        /// </summary>
        public IDbConnection Connection { get; }

        /// <summary>
        /// Saves the change.
        /// </summary>
        /// <returns></returns>
        void SaveChange();
    }
}
