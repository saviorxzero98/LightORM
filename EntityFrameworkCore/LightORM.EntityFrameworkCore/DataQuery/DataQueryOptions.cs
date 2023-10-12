using LightORM.EntityFrameworkCore.DataQuery.Filters;
using LightORM.EntityFrameworkCore.DataQuery.Pages;
using LightORM.EntityFrameworkCore.DataQuery.Sorts;

namespace LightORM.EntityFrameworkCore.DataQuery
{
    public class DataQueryOptions : IDataQueryOptions
    {
        public IPageOptions? Page { get; set; }

        public IMultiSortOptions? Sorts { get; set; }

        public IMultiFilterOptions? Filters { get; set; }
        public bool IgnoreCase { get; set; } = false;


        public DataQueryOptions()
        {

        }
        public DataQueryOptions(bool ignoreCase)
        {
            IgnoreCase = ignoreCase;
        }


        #region 設定分頁

        /// <summary>
        /// 設定分頁
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataQueryOptions SetPage(IPageOptions? options)
        {
            if (options != null)
            {
                Page = new PageOptions(options);
            }
            return this;
        }
        /// <summary>
        /// 設定分頁
        /// </summary>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public DataQueryOptions SetPage(int offset = 0, int limit = 0)
        {
            offset = (offset >= 0) ? offset : 0;
            limit = (limit >= 0) ? limit : 0;

            Page = new PageOptions(offset, limit);
            return this;
        }
        /// <summary>
        /// 設定分頁
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataQueryOptions SetPage(ISlidingPageOptions? options)
        {
            if (options != null)
            {
                Page = new PageOptions(options);
            }
            return this;
        }

        #endregion


        #region 設定排序

        /// <summary>
        /// 設定排序 (單欄)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataQueryOptions SetSort(ISortOptions? options)
        {
            if (options != null)
            {
                Sorts = new MultiSortOptions(options);
            }
            return this;
        }
        /// <summary>
        /// 設定排序 (單欄)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public DataQueryOptions SetSort(string field, 
                                        SortDirections direction = SortDirections.Asc)
        {
            var options = new SortOptions(field, direction);
            Sorts = new MultiSortOptions(options);
            return this;
        }


        /// <summary>
        /// 設定排序 (多欄)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataQueryOptions SetSorts(IMultiSortOptions? options)
        {
            if (options != null)
            {
                Sorts = new MultiSortOptions(options);
            }
            return this;
        }
        /// <summary>
        /// 設定排序 (多欄)
        /// </summary>
        /// <param name="sorts"></param>
        /// <returns></returns>
        public DataQueryOptions SetSorts(IEnumerable<ISortOptions> sorts)
        {
            if (sorts != null)
            {
                Sorts = new MultiSortOptions(sorts);
            }
            return this;
        }

        #endregion


        #region 設定篩選

        /// <summary>
        /// 設定篩選 (單個欄位)
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="op"></param>
        /// <returns></returns>
        public DataQueryOptions SetFilter(string field, string value,
                                          FilterOperators op = FilterOperators.StartsWith)
        {
            var filter = new FilterOptions(field, value, op);
            Filters = new MultiFilterOptions(filter);
            return this;
        }
        /// <summary>
        /// 設定篩選 (單個欄位)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataQueryOptions SetFilter(IFilterOptions? options)
        {
            if (options != null)
            {
                Filters = new MultiFilterOptions(options);
            }
            return this;
        }


        /// <summary>
        /// 設定篩選 (多個欄位)
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public DataQueryOptions SetFilters(IMultiFilterOptions? options)
        {
            if (options != null)
            {
                Filters = new MultiFilterOptions(options);
            }
            return this;
        }
        /// <summary>
        /// 設定篩選 (多個欄位)
        /// </summary>
        /// <param name="filters"></param>
        /// <param name="logic"></param>
        /// <returns></returns>
        public DataQueryOptions SetFilters(IEnumerable<IFilterOptions> filters,
                                           FilterLogics logic = FilterLogics.And)
        {
            if (filters != null)
            {
                Filters = new MultiFilterOptions(filters.ToList(), logic);
            }
            return this;
        }

        #endregion
    }
}
