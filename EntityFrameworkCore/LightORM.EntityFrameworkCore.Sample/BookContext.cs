using LightORM.EntityFrameworkCore.Sample.Entities;
using Microsoft.EntityFrameworkCore;

namespace LightORM.EntityFrameworkCore.Sample
{
    public class BookContext : DbContext
    {
        public DbSet<Book> Books { get; set; }

        private string _connectionString;

        public BookContext(string connectionString)
        {
            _connectionString = connectionString;
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            if (!optionsBuilder.IsConfigured)
            {
                optionsBuilder.UseSqlite(_connectionString);
            }
        }
    }
}
