namespace LightORM.EntityFrameworkCore.DataQuery.Pages
{
    public class PageOptions : IPageOptions
    {
        /// <summary>
        /// 忽略幾筆資料
        /// </summary>
        public int Offset { get; set; } = 0;

        /// <summary>
        /// 取得幾筆資料
        /// </summary>
        /// <remarks>
        /// 0：取得所有資料；
        /// 1 ~ N：取 1 ~ N 筆資料
        /// </remarks>
        public int Limit { get; set; } = 0;


        public PageOptions()
        {
        }
        public PageOptions(int offset = 0, int limit = 0)
        {
            Offset = offset;
            Limit = limit;
        }
        public PageOptions(IPageOptions options)
        {
            if (options != null)
            {
                Offset = options.Offset;
                Limit = options.Limit;
            }
            else
            {
                Offset = 0;
                Limit = 0;
            }
        }
        public PageOptions(ISlidingPageOptions options)
        {
            if (options != null)
            {
                Limit = (options.PageSize >= 0) ? options.PageSize : 0;
                Offset = (options.PageNumber < 1) ? 0: (options.PageNumber - 1) * Limit;
            }
            else
            {
                Offset = 0;
                Limit = 0;
            }
        }
    }
}
