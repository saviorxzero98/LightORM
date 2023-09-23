namespace LightORM.EntityFrameworkCore.DataQuery
{
    public class DataQueryOptions : IDataQueryOptions
    {
        #region 分頁

        /// <summary>
        /// 每一頁的筆數
        /// </summary>
        /// <remarks>
        /// 0：不分頁；
        /// 1 ~ N：限制 1 ~ N 筆
        /// </remarks>
        public int PageSize { get; set; } = 0;

        /// <summary>
        /// 第 N 頁
        /// </summary>
        public int PageNumber { get; set; } = 0;

        #endregion


        #region 排序

        /// <summary>
        /// 排序
        /// </summary>
        public List<DataSortField> Sorts { get; set; } = new List<DataSortField>();

        #endregion


        #region 篩選

        /// <summary>
        /// 篩選的邏輯 (AND, OR)
        /// </summary>
        public DataFilterLogic FilterLogic { get; set; } = DataFilterLogic.And;

        /// <summary>
        /// 篩選條件
        /// </summary>
        public List<DataFilter> Filters { get; set; } = new List<DataFilter>();

        #endregion
    }
}
