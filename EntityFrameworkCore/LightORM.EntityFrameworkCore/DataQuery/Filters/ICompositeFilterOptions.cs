namespace LightORM.EntityFrameworkCore.DataQuery.Filters
{
    /// <summary>
    /// 複雜的篩選設定
    /// </summary>
    public interface ICompositeFilterOptions : IFilterOptions
    {
        /// <summary>
        /// 子篩選的邏輯 (AND 或 OR)
        /// </summary>
        FilterLogic Logic { get; set; }

        /// <summary>
        /// 子篩選
        /// </summary>
        List<ICompositeFilterOptions> Filters { get; set; }
    }
}
