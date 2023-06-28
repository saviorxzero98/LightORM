using MySqlConnector;
using SqlKata.Compilers;
using System.Data;

namespace LightORM.Dapper.Adapters
{
    public class MySqlDbAdapter : IDbAdapter
    {
        protected string _connectionString;
        /// <summary>
        /// Connection String
        /// </summary>
        public string ConnectionString { get => _connectionString; }

        /// <summary>
        /// Database Adapter Type
        /// </summary>
        public string AdapterType { get => DbAdapterType.MySql.ToString().ToLower(); }


        public MySqlDbAdapter(string connectionString)
        {
            _connectionString = connectionString;
        }

        /// <summary>
        /// 建立 MySQL Database Connection
        /// </summary>
        /// <returns></returns>
        public IDbConnection CreateDbConnection()
        {
            return new MySqlConnection(ConnectionString);
        }

        /// <summary>
        /// 建立 MySQL SQL Compiler
        /// </summary>
        /// <returns></returns>
        public Compiler GetSqlCompiler()
        {
            return new MySqlCompiler();
        }
    }
}
