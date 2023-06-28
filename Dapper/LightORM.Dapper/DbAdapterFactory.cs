using LightORM.Dapper.Adapters;

namespace LightORM.Dapper
{
    public static class DbAdapterFactory
    {
        /// <summary>
        /// 建立 Database Adapter
        /// </summary>
        /// <param name="type"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IDbAdapter CreateDbAdapter(DbAdapterType type, string connectionString)
        {
            switch (type)
            {
                case DbAdapterType.SqlServer:
                    return new SqlServerDbAdapter(connectionString);

                case DbAdapterType.Postgres:
                    return new PostgresDbAdapter(connectionString);

                case DbAdapterType.Sqlite:
                    return new SqliteDbAdapter(connectionString);

                case DbAdapterType.MySql:
                    return new MySqlDbAdapter(connectionString);
            }
            return null;
        }

        /// <summary>
        /// 建立 Database Adapter
        /// </summary>
        /// <param name="typeName"></param>
        /// <param name="connectionString"></param>
        /// <returns></returns>
        public static IDbAdapter CreateDbAdapter(string typeName, string connectionString)
        {
            DbAdapterType type = ToDbAdapterType(typeName);
            return CreateDbAdapter(type, connectionString);
        }

        /// <summary>
        /// 解析資料庫類型
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        public static DbAdapterType ToDbAdapterType(string typeName)
        {
            if (string.IsNullOrEmpty(typeName))
            {
                return DbAdapterType.None;
            }

            switch (typeName.ToLower())
            {
                case "mssql":
                case "sqlserver":
                    return DbAdapterType.SqlServer;

                case "sqlite":
                case "sqlite3":
                    return DbAdapterType.Sqlite;

                case "pgsql":
                case "postgre":
                case "postgres":
                case "postgresql":
                    return DbAdapterType.Postgres;

                case "mysql":
                case "mariadb":
                    return DbAdapterType.MySql;

                default:
                    return DbAdapterType.None;
            }
        }
    }
}
