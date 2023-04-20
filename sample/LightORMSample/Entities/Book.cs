using Dapper;

namespace LightORM.Sample.Entities
{
    [Table("BookRepository")]
    public class Book
    {
        [Key]
        public int Id { get; set; }

        public string Name { get; set; } = string.Empty;

        public string Author { get; set; } = string.Empty;

        public bool IsAvailable { get; set; }

        public DateTime CreateDate { get; set; }
    }
}
