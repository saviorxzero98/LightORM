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
            var book2 = new Book()
            {
                Id = 2,
                Name = "Cinderella",
                Author = "Charles Perrault",
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
                var insterResult2 = repository.Insert(book2);
                await unitOfWork.SaveChangeAsync();

                // Get
                var query = new QueryOptions().SetWhereConditions(WhereCondition.Where(nameof(Book.Id), book.Id));
                var insertedBook = repository.Get(query);

                // Update
                insertedBook.IsAvailable = false;
                var updateResult = repository.Update(insertedBook);
                await unitOfWork.SaveChangeAsync();

                // Get again
                var updatedBook = repository.Get(query);

                // Get All
                var allBooks = repository.GetAll(new QueryOptions());

                // Delete
                foreach (var deleteBook in allBooks)
                {
                    var deleteResult = repository.Delete(deleteBook);
                }
                //var deleteResult = repository.Delete(WhereCondition.Where(nameof(Book.Id), book.Id));
                //var deleteResult2 = repository.Delete(WhereCondition.Where(nameof(Book.Id), book2.Id));

                await unitOfWork.SaveChangeAsync();
            }
        }
    }
}