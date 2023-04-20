using SqlKata;

namespace LightORM.Entities
{
    internal class RecordCountEntity
    {
        [Column("count")]
        public int Count { get; set; }
    }
}
