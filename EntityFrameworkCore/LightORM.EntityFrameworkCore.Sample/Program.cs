using LightORM.EntityFrameworkCore.Repositories;
using LightORM.EntityFrameworkCore.Sample.Entities;
using LightORM.EntityFrameworkCore.UnitOfWorks;

namespace LightORM.EntityFrameworkCore.Sample
{
    internal class Program
    {
        static async Task Main(string[] args)
        {
            Console.WriteLine("Demo LiteORM.EfCore");
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

            var dbContext = new BookContext(DatabaseType.Sqlite, connectionString);
            using (var unitOfWork = new UnitOfWork(dbContext))
            {
                var repository = new GenericRepository<Book>(dbContext);

                // Insert
                var insertResult = repository.Insert(book);
                await unitOfWork.SaveChangeAsync();

                // Get
                var insertedBook = repository.Get(e => e.Id == book.Id);

                // Update
                insertedBook.IsAvailable = false;
                var updateResult = repository.UpdateAsync(insertedBook);
                await unitOfWork.SaveChangeAsync();

                // Get again
                var updatedBook = repository.Get(e => e.Id == book.Id);

                // Delete
                var deleteResult = repository.DeleteAsync(updatedBook);
                await unitOfWork.SaveChangeAsync();
            }
        }
    }
}