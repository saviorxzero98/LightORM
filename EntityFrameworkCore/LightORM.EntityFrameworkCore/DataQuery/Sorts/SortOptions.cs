namespace LightORM.EntityFrameworkCore.DataQuery.Sorts
{
    public class SortOptions : ISortOptions
    {
        /// <summary>
        /// 排序方向
        /// </summary>
        /// <remarks>
        /// asc：升冪排序 (預設值);
        /// desc：降冪排序
        /// </remarks>
        public SortDirections Direction { get; set; } = SortDirections.Asc;

        /// <summary>
        /// 排序欄位
        /// </summary>
        public string Field { get; set; } = string.Empty;


        public SortOptions() : this(string.Empty)
        {
        }
        public SortOptions(string field, SortDirections direction = SortDirections.Asc)
        {
            Direction = direction;
            Field = field;
        }
        public SortOptions(ISortOptions sortOptions)
        {
            Direction = sortOptions.Direction;
            Field = sortOptions.Field;
        }
    }
}
