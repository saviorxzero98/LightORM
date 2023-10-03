namespace LightORM.EntityFrameworkCore.DataQuery.Filters
{
    public class MultiFilterOptions : IMultiFilterOptions
    {
        /// <summary>
        /// 篩選的邏輯 (AND, OR)
        /// </summary>
        public FilterLogic Logic { get; } = FilterLogic.And;


        // <summary>
        /// 多個篩選條件
        /// </summary>
        public List<IFilterOptions> Filters { get; set; } = new List<IFilterOptions>();


        public MultiFilterOptions()
        {

        }
        public MultiFilterOptions(List<IFilterOptions> filters, FilterLogic filterLogic = FilterLogic.And)
        {
            Logic = filterLogic;
            Filters = (filters != null) ? filters : new List<IFilterOptions>();
        }
        public MultiFilterOptions(IMultiFilterOptions options)
        {
            Logic = options.Logic;
            Filters = (options.Filters != null) ? new List<IFilterOptions>(options.Filters) : new List<IFilterOptions>();
        }
        public MultiFilterOptions(IFilterOptions filter)
        {
            Logic = FilterLogic.And;
            Filters = new List<IFilterOptions>();

            if (filter != null)
            {
                Filters.Add(filter);
            }
        }
    }
}
