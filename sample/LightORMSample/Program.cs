using LightORM;
using LightORM.Adapters;
using LightORM.Repositories;
using LightORM.Sample.Entities;
using LightORM.UnitOfWorks;
using static LightORM.Extensions.SqlKataExtenstions;

namespace LightORMSample
{
    internal class Program
    {
        static void Main(string[] args)
        {
            Console.WriteLine("Demo");
            Demo();
        }

        static void Demo()
        {
            // Book
            var book = new Book()
            {
                Id = 1,
                Name = "Alice in Wonderland",
                Author = "Charles Lutwidge Dodgson",
                IsAvailable= true,
                CreateDate = DateTime.Now
            };

            string connectionString = "Data Source=Library.db;";
            var dbAdapter = DbAdapterFactory.CreateDbAdapter(DbAdapterType.Sqlite, connectionString);

            using (var unitOfWork = new UnitOfWork(dbAdapter))
            {
                var repository = new GenericRepository<Book>(dbAdapter, unitOfWork);

                // Insert
                var insertResult = repository.Insert(book);
                unitOfWork.SaveChange();

                // Get
                var query = new QueryOptions().SetWhereConditions(new List<WhereCondition>()
                {
                    WhereCondition.Where(nameof(Book.Id), book.Id)
                });
                var insertedBook = repository.Get(query);

                // Update
                insertedBook.IsAvailable = false;
                var updateResult = repository.Update(insertedBook);
                unitOfWork.SaveChange();

                // Delete
                var deleteResult = repository.Delete(new List<WhereCondition>() { WhereCondition.Where(nameof(Book.Id), book.Id) });
                unitOfWork.SaveChange();
            }
        }
    }
}