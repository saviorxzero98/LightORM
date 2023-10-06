namespace LightORM.EntityFrameworkCore.DataQuery.Filters
{
    public interface IMultiFilterOptions
    {
        /// <summary>
        /// 篩選的邏輯 (AND 或 OR)
        /// </summary>
        FilterLogics Logic { get; set; }

        /// <summary>
        /// 篩選
        /// </summary>
        List<IFilterOptions> Filters { get; set; }
    }
}
