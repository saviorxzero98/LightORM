namespace LightORM.EntityFrameworkCore.DataQuery.Filters
{
    public enum FilterOperator
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
}
