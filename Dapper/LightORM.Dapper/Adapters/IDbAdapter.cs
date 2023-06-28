using SqlKata.Compilers;
using System.Data;

namespace LightORM.Dapper.Adapters
{
    public enum DbAdapterType { None, SqlServer, Postgres, Sqlite, MySql }

    public interface IDbAdapter
    {
        /// <summary>
        /// Connection String
        /// </summary>
        string ConnectionString { get; }

        /// <summary>
        /// Database Adapter Type
        /// </summary>
        string AdapterType { get; }


        /// <summary>
        /// 建立 Database Conneciton
        /// </summary>
        /// <returns></returns>
        IDbConnection CreateDbConnection();

        /// <summary>
        /// 取得 SQL Compiler
        /// </summary>
        /// <returns></returns>
        Compiler GetSqlCompiler();
    }
}
