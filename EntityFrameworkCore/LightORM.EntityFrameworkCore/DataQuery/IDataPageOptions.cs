namespace LightORM.EntityFrameworkCore.DataQuery
{
    public interface IDataPageOptions
    {
        /// <summary>
        /// 忽略幾筆資料
        /// </summary>
        int Offset { get; set; }

        /// <summary>
        /// 取得幾筆資料
        /// </summary>
        int Limit { get; set; }
    }
}
