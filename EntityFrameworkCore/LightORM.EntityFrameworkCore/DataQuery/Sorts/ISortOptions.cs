namespace LightORM.EntityFrameworkCore.DataQuery.Sorts
{
    public interface ISortOptions
    {
        /// <summary>
        /// 排序方向
        /// </summary>
        /// <remarks>
        /// asc：升冪排序 (預設值);
        /// desc：降冪排序
        /// </remarks>
        SortDirections Direction { get; set; }

        /// <summary>
        /// 排序欄位
        /// </summary>
        string Field { get; set; }
    }
}
