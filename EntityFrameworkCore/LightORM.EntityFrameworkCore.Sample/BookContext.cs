using LightORM.EntityFrameworkCore.Sample.Entities;
using Microsoft.EntityFrameworkCore;

namespace LightORM.EntityFrameworkCore.Sample
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        private DatabaseType _datebaseType;
        private string _connectionString;

        public BookContext(DatabaseType databaseType, string connectionString)
        {
            _datebaseType = databaseType;
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                switch (_datebaseType)
                {
                    case DatabaseType.InMemory:
                        optionsBuilder.UseInMemoryDatabase(_connectionString);
                        break;

                    case DatabaseType.SqlServer:
                        optionsBuilder.UseSqlServer(_connectionString); 
                        break;

                    case DatabaseType.Postgres:
                        optionsBuilder.UseNpgsql(_connectionString);
                        break;

                    case DatabaseType.Sqlite:
                        optionsBuilder.UseSqlite(_connectionString);
                        break;

                    case DatabaseType.MySql:
                        optionsBuilder.UseMySql(_connectionString, ServerVersion.AutoDetect(_connectionString));
                        break;
                }
            }

            optionsBuilder.LogTo(Console.WriteLine, Microsoft.Extensions.Logging.LogLevel.Information);
        }
    }
}
