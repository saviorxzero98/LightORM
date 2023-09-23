namespace LightORM.EntityFrameworkCore.DataQuery
{
    public interface IDataSortOptions
    {
        /// <summary>
        /// 排序
        /// </summary>
        List<DataSortField> Sorts { get; }
    }

    public class DataSortField
    {
        /// <summary>
        /// 排序方式 (ASC, DESC)
        /// </summary>
        public DataSortDirection Direction { get; set; } = DataSortDirection.Asc;

        /// <summary>
        /// 篩選的欄位
        /// </summary>
        public string Field { get; set; } = string.Empty;


        public DataSortField()
        {
        }
        public DataSortField(string field) : this (field, DataSortDirection.Asc)
        {
        }
        public DataSortField(string field, DataSortDirection direction)
        {
            Field = field;
            Direction = direction;
        }
    }

    public enum DataSortDirection
    {
        /// <summary>
        /// 正向排序
        /// </summary>
        Asc,

        /// <summary>
        /// 反向排序
        /// </summary>
        Desc
    }
}
