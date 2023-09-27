namespace LightORM.EntityFrameworkCore.DataQuery
{
    public interface IDataSlidingPageOptions
    {
        /// <summary>
        /// 每一頁的筆數
        /// </summary>
        /// <remarks>
        /// 0：不分頁；
        /// 1 ~ N：限制 1 ~ N 筆
        /// </remarks>
        int PageSize { get; }

        /// <summary>
        /// 第 N 頁
        /// </summary>
        int PageNumber { get; }
    }
}
