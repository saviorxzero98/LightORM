namespace LightORM.EntityFrameworkCore.DataQuery.Sorts
{
    public class MultiSortOptions : IMultiSortOptions
    {
        /// <summary>
        /// 排序
        /// </summary>
        public List<ISortOptions> Sorts { get; set; } = new List<ISortOptions>();

        public MultiSortOptions()
        {
        }
        public MultiSortOptions(List<ISortOptions> sorts)
        {
            Sorts = (sorts != null) ? new List<ISortOptions>(sorts) : new List<ISortOptions>();
        }
        public MultiSortOptions(ISortOptions options)
        {
            Sorts = new List<ISortOptions>();

            if (options != null)
            {
                Sorts.Add(options);
            }
        }
        public MultiSortOptions(IMultiSortOptions options)
        {
            Sorts = (options != null && options.Sorts != null) ?
                        new List<ISortOptions>(options.Sorts) :
                        new List<ISortOptions>();
        }
        public MultiSortOptions(IEnumerable<ISortOptions> sorts)
        {
            Sorts = (sorts != null) ?
                        new List<ISortOptions>(sorts) :
                        new List<ISortOptions>();
        }
    }
}
