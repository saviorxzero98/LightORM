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
            var book2 = new Book()
            {
                Id = 2,
                Name = "Cinderella",
                Author = "Charles Perrault",
                IsAvailable = true,
                CreateDate = DateTime.Now
            };
            var book3 = new Book()
            {
                Id = 3,
                Name = "Cinderellb",
                Author = "Charles Perrault",
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
                var insterResult2 = repository.Insert(book2);
                var insterResult3 = repository.Insert(book3);
                await unitOfWork.SaveChangeAsync();

                // Get
                var insertedBook = repository.Get(e => e.Id == book.Id);

                if (insertedBook != null)
                {
                    // Update
                    insertedBook.IsAvailable = false;
                    var updateResult = repository.UpdateAsync(insertedBook);
                    await unitOfWork.SaveChangeAsync();
                }


                // Get again
                var updatedBook = repository.Get(e => e.Id == book.Id);
                var updatedBook2 = repository.Get(e => e.Id == book2.Id);
                var updatedBook3 = repository.Get(e => e.Id == book3.Id);

                if (updatedBook != null)
                {
                    // Delete
                    var deleteResult = repository.DeleteAsync(updatedBook);
                    await unitOfWork.SaveChangeAsync();
                }
                if (updatedBook2 != null)
                {
                    // Delete
                    var deleteResult = repository.DeleteAsync(updatedBook2);
                    await unitOfWork.SaveChangeAsync();
                }
                if (updatedBook3 != null)
                {
                    // Delete
                    var deleteResult = repository.DeleteAsync(updatedBook3);
                    await unitOfWork.SaveChangeAsync();

                }
            }
        }
    }
}