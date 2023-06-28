using SqlKata;

namespace LightORM.Dapper.Entities
{
    internal class RecordCountEntity
    {
        [Column("count")]
        public int Count { get; set; }
    }
}
