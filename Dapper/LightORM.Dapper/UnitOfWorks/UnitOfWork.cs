using LightORM.Dapper.Adapters;
using System.Data;
using System.Threading.Tasks;

namespace LightORM.Dapper.UnitOfWorks
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
        public Task<bool> SaveChangeAsync()
        {
            try
            {
                _transaction.Commit();
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                return Task.FromResult(true);

            }
            catch
            {
                _transaction.Rollback();
                _transaction.Dispose();
                _transaction = _connection.BeginTransaction();
                return Task.FromResult(false);
            }
        }
    }
}
