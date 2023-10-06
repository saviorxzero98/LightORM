namespace LightORM.EntityFrameworkCore.DataQuery.Sorts
{
    public interface IMultiSortOptions
    {
        /// <summary>
        /// 排序
        /// </summary>
        List<ISortOptions> Sorts { get; set; }
    }
}
