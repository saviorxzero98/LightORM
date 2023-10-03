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
        public FilterOperator Operator { get; set; } = FilterOperator.StartsWith;

        /// <summary>
        /// 篩選的邏輯 (AND, OR)
        /// </summary>
        public FilterLogic Logic { get; set; } = FilterLogic.And;

        /// <summary>
        /// 多個篩選條件
        /// </summary>
        public List<ICompositeFilterOptions> Filters { get; set; } = new List<ICompositeFilterOptions>();

        
        public CompositeFilterOptions() 
        {
            Logic = FilterLogic.And;
            Filters = new List<ICompositeFilterOptions>();
        }
        public CompositeFilterOptions(List<ICompositeFilterOptions> filters, FilterLogic logic)
        {
            Logic = logic;
            Filters = filters;
        }
        public CompositeFilterOptions(IFilterOptions filter)
        {
            Logic = FilterLogic.And;
            Filters = new List<ICompositeFilterOptions>();

            if (filter != null)
            {
                Filters.Add((ICompositeFilterOptions) filter);
            }
        }
    }
}
