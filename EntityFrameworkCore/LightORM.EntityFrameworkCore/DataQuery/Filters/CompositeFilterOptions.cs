namespace LightORM.EntityFrameworkCore.DataQuery.Filters
{
    public class CompositeFilterOptions : ICompositeFilterOptions
    {
        /// <summary>
        /// 篩選的欄位
        /// </summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// 篩選的值
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 篩選方式
        /// </summary>
        public FilterOperators Operator { get; set; } = FilterOperators.StartsWith;

        /// <summary>
        /// 篩選的邏輯 (AND, OR)
        /// </summary>
        public FilterLogics Logic { get; set; } = FilterLogics.And;

        /// <summary>
        /// 多個篩選條件
        /// </summary>
        public List<ICompositeFilterOptions> Filters { get; set; } = new List<ICompositeFilterOptions>();

        
        public CompositeFilterOptions() 
        {
            Logic = FilterLogics.And;
            Filters = new List<ICompositeFilterOptions>();
        }
        public CompositeFilterOptions(List<ICompositeFilterOptions> filters, FilterLogics logic)
        {
            Logic = logic;
            Filters = filters;
        }
        public CompositeFilterOptions(IFilterOptions filter)
        {
            Logic = FilterLogics.And;
            Filters = new List<ICompositeFilterOptions>();

            if (filter != null)
            {
                Filters.Add((ICompositeFilterOptions) filter);
            }
        }
    }
}
