namespace LightORM.EntityFrameworkCore.DataQuery
{
    public interface IDataFilterOptions
    {
        /// <summary>
        /// 篩選的邏輯 (AND, OR)
        /// </summary>
        DataFilterLogic FilterLogic { get; }

        /// <summary>
        /// 篩選條件
        /// </summary>
        List<DataFilter> Filters { get; }
    }

    public class DataFilter
    {
        /// <summary>
        /// 篩選的欄位
        /// </summary>
        public string Field { get; set; } = string.Empty;

        /// <summary>
        /// 篩選的欄位值
        /// </summary>
        public string Value { get; set; } = string.Empty;

        /// <summary>
        /// 篩選的運算子
        /// </summary>
        public DataFilterOperator Operator { get; set; }


        #region Sub-fileters

        /// <summary>
        /// 子篩選的邏輯 (AND 或 OR)
        /// </summary>
        public DataFilterLogic Logic { get; set; }

        /// <summary>
        /// 子篩選
        /// </summary>
        public List<DataFilter> Filters { get; set; } = new List<DataFilter>();

        #endregion

        public DataFilter()
        {

        }
        public DataFilter(string field, string value) : this (field, value, DataFilterOperator.Equals)
        {

        }
        public DataFilter(string field, string value, DataFilterOperator op)
        {
            Field = field;
            Value = value;
            Operator = op;
        }
    }

    public enum DataFilterOperator
    {
        /// <summary>
        /// 包含
        /// </summary>
        Contains,

        /// <summary>
        /// 不包含
        /// </summary>
        DoesNotContain,

        /// <summary>
        /// 相等
        /// </summary>
        Equals,

        /// <summary>
        /// 不相等
        /// </summary>
        NotEquals,

        /// <summary>
        /// 字首相同
        /// </summary>
        StartsWith,

        /// <summary>
        /// 字尾相同
        /// </summary>
        EndsWith,

        /// <summary>
        /// 為空
        /// </summary>
        IsNull,

        /// <summary>
        /// 不為空
        /// </summary>
        IsNotNull,

        /// <summary>
        /// 為空字串
        /// </summary>
        IsEmpty,

        /// <summary>
        /// 不為空字串
        /// </summary>
        IsNotEmpty,

        /// <summary>
        /// 大於或等於
        /// </summary>
        GreaterThanOrEqual,

        /// <summary>
        /// 小於或等於
        /// </summary>
        LessThanOrEqual,

        /// <summary>
        /// 大於
        /// </summary>
        GreaterThan,

        /// <summary>
        /// 小於
        /// </summary>
        LessThan
    }

    public enum DataFilterLogic
    {
        /// <summary>
        /// AND
        /// </summary>
        And,

        /// <summary>
        /// OR
        /// </summary>
        Or
    }
}
