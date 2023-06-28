using LightORM.Dapper.Adapters;
using LightORM.Dapper.Repositories;
using LightORM.Dapper.Sample.Entities;
using LightORM.Dapper.UnitOfWorks;
using static LightORM.Dapper.Extensions.SqlKataExtenstions;

namespace LightORM.Dapper.Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Demo LiteORM.Dapper");
            await DemoAsync();
        }

        static async Task DemoAsync()
        {
            // Book
            var book = new Book()
            {
                Id = 1,
                Name = "Alice in Wonderland",
                Author = "Charles Lutwidge Dodgson",
                IsAvailable = true,
                CreateDate = DateTime.Now
            };

            string connectionString = "Data Source=Library.db;";
            var dbAdapter = DbAdapterFactory.CreateDbAdapter(DbAdapterType.Sqlite, connectionString);

            using (var unitOfWork = new UnitOfWork(dbAdapter))
            {
                var repository = new GenericRepository<Book>(dbAdapter, unitOfWork);

                // Insert
                var insertResult = repository.Insert(book);
                await unitOfWork.SaveChangeAsync();

                // Get
                var query = new QueryOptions().SetWhereConditions(new List<WhereCondition>()
                {
                    WhereCondition.Where(nameof(Book.Id), book.Id)
                });
                var insertedBook = repository.Get(query);

                // Update
                insertedBook.IsAvailable = false;
                var updateResult = repository.UpdateAsync(insertedBook);
                await unitOfWork.SaveChangeAsync();

                // Get again
                var updatedBook = repository.Get(query);

                // Delete
                var deleteResult = repository.DeleteAsync(new List<WhereCondition>() { WhereCondition.Where(nameof(Book.Id), book.Id) });
                await unitOfWork.SaveChangeAsync();
            }
        }
    }
}