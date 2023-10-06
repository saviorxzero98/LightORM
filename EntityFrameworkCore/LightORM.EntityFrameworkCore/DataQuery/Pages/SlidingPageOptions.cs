namespace LightORM.EntityFrameworkCore.DataQuery.Pages
{
    public class SlidingPageOptions : ISlidingPageOptions
    {
        /// <summary>
        /// 每一頁的筆數
        /// </summary>
        /// <remarks>
        /// 0：不分頁；
        /// 1 ~ N：限制 1 ~ N 筆
        /// </remarks>
        public int PageSize { get; } = 0;

        /// <summary>
        /// 第 N 頁
        /// </summary>
        public int PageNumber { get; } = 0;


        public SlidingPageOptions()
        {
            PageSize = 0;
            PageNumber = 0;
        }
        public SlidingPageOptions(int pageSize, int pageNumber)
        {
            PageSize = pageSize;
            PageNumber = pageNumber;
        }
    }
}
