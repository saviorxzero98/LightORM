namespace LightORM.EntityFrameworkCore.DataQuery.Filters
{
    /// <summary>
    /// 篩選設定
    /// </summary>
    public interface IFilterOptions
    {
        /// <summary>
        /// 篩選的欄位
        /// </summary>
        string Field { get; set; }

        /// <summary>
        /// 篩選的欄位值
        /// </summary>
        string Value { get; set; }

        /// <summary>
        /// 篩選的運算子
        /// </summary>
        FilterOperators Operator { get; set; }
    }
}
