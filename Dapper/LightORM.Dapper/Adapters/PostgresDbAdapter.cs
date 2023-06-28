using Npgsql;
using SqlKata.Compilers;
using System.Data;

namespace LightORM.Dapper.Adapters
{
    public class PostgresDbAdapter : IDbAdapter
    {
        protected string _connectionString;
        /// <summary>
        /// Connection String
        /// </summary>
        public string ConnectionString { get => _connectionString; }

        /// <summary>
        /// Database Adapter Type
        /// </summary>
        public string AdapterType { get => DbAdapterType.Postgres.ToString().ToLower(); }


        public PostgresDbAdapter(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 建立 PostgreSQL Database Connection
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateDbConnection()
        {
            return new NpgsqlConnection(ConnectionString);
        }

        /// <summary>
        /// 建立 PostgreSQL SQL Compiler
        /// </summary>
        /// <returns></returns>
        public Compiler GetSqlCompiler()
        {
            return new PostgresCompiler();
        }
    }
}
