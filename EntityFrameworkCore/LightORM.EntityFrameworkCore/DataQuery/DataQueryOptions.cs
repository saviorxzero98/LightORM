namespace LightORM.EntityFrameworkCore.DataQuery
{
    public class DataQueryOptions : IDataQueryOptions
    {
        #region 分頁 (Page)

        /// <summary>
        /// 忽略幾筆資料
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// 取得幾筆資料
        /// </summary>
        public int Limit { get; set; } = 0;

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
