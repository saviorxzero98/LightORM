namespace LightORM.EntityFrameworkCore.DataQuery.Filters
{
    public class FilterOptions : IFilterOptions
    {
        /// <summary>
        /// 篩選的欄位
        /// </summary>
        public string Field { get; set; }

        /// <summary>
        /// 篩選的值
        /// </summary>
        public string Value { get; set; }

        /// <summary>
        /// 篩選方式
        /// </summary>
        public FilterOperator Operator { get; set; } = FilterOperator.StartsWith;


        public FilterOptions() : this(string.Empty, string.Empty)
        {
        }
        public FilterOptions(string field, string value, FilterOperator op = FilterOperator.StartsWith)
        {
            Field = field;
            Value = value;
            Operator = op;
        }
        public FilterOptions(IFilterOptions options)
        {
            if (options != null)
            {
                Field = options.Field;
                Value = options.Value;
                Operator = options.Operator;
            }
            else
            {
                Field = string.Empty;
                Value = string.Empty;
                Operator = FilterOperator.StartsWith;
            }
        }
    }
}
