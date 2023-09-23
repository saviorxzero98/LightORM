using LightORM.EntityFrameworkCore.DataQuery;
using System.Linq.Dynamic.Core;

namespace LightORM.EntityFrameworkCore.Extensions
{
    public static class QueryableExtensions
    {
        #region 分頁

        /// <summary>
        /// 分頁
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, int pageNumber, int pageSize)
        {
            if (pageSize > 1)
            {
                // 分頁
                var take = pageSize;
                var skip = (pageNumber < 1) ? 0 : (pageNumber - 1) * take;

                return queryable.Skip(skip).Take(take);
            }
            else
            {
                // 不分頁
                return queryable;
            }
        }

        /// <summary>
        /// 分頁
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="dataPage"></param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, IDataPageOptions dataPage)
        {
            return queryable.Page(dataPage.PageNumber, dataPage.PageSize);
        }

        #endregion


        #region 排序

        /// <summary>
        /// 排序 (單個欄位)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="field"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, string field, DataSortDirection direction)
        {
            if (!string.IsNullOrEmpty(field))
            {
                string orderExpression = $"{field} {direction}";

                return queryable.OrderBy(orderExpression);
            }
            return queryable;
        }

        /// <summary>
        /// 排序 (多個欄位)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="sortFields"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, IEnumerable<DataSortField> sortFields)
        {
            if (sortFields != null && sortFields.Any())
            {
                // 排序
                return SortExpressionBuilder.ApplySort(queryable, sortFields);
            }
            else
            {
                return queryable;
            }
        }

        /// <summary>
        /// 排序 (多個欄位)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="sort"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, IDataSortOptions sort)
        {
            if (sort != null)
            {
                return queryable.Sort(sort.Sorts);
            }
            else
            {
                return queryable;
            }
        }

        #endregion


        #region 篩選

        /// <summary>
        /// 篩選欄位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="filter"></param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, DataFilter filter)
        {
            if (filter != null)
            {
                var expressions = FilterExpressionBuilder.GetFilterExpression<T>(DataFilterLogic.And, 
                                                                                 new List<DataFilter>() { filter });
                
                if (expressions != null)
                {
                    return queryable.Where(expressions);
                }
                return queryable;
            }
            else
            {
                return queryable;
            }
        }

        /// <summary>
        /// 篩選欄位
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, IDataFilterOptions filters)
        {
            if (filters != null)
            {
                var expressions = FilterExpressionBuilder.GetFilterExpression<T>(filters.FilterLogic, filters.Filters);

                if (expressions != null)
                {
                    queryable = queryable.Where(expressions);
                    return queryable;
                }
                return queryable;                
            }
            else
            {
                return queryable;
            }
        }

        #endregion
    }
}
