using LightORM.Adapters;
using System;
using System.Data;

namespace LightORM.UnitOfWorks
{
    public class UnitOfWork : IUnitOfWork
    {
        private IDbAdapter _adapter;

        private IDbConnection _connection;
        public IDbConnection Connection { get => _connection; }

        private IDbTransaction _transaction;

        public UnitOfWork(IDbAdapter dbAdapter)
        {
            _adapter = dbAdapter;
            _connection = _adapter.CreateDbConnection();
            _connection.Open();
            _transaction = _connection.BeginTransaction();
        }

        /// <summary>
        /// Close
        /// </summary>
        public void Dispose()
        {
            _transaction.Connection?.Close();
            _transaction.Connection?.Dispose();
            _transaction.Dispose();
        }

        /// <summary>
        /// Save Change
        /// </summary>
        public void SaveChange()
        {
            try
            {
                _transaction.Commit();
            }
            catch (Exception e)
            {
                _transaction.Rollback();
            }
            finally
            {
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
            }
        }
    }
}
