using LightORM.EntityFrameworkCore.DataQuery;
using LightORM.EntityFrameworkCore.DataQuery.Filters;
using LightORM.EntityFrameworkCore.DataQuery.Pages;
using LightORM.EntityFrameworkCore.DataQuery.Sorts;
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
        /// <param name="options"></param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, IDataQueryOptions? options)
        {
            if (options != null && options.Page != null)
            {
                return queryable.Page(options.Page);
            }
            return queryable;
        }

        /// <summary>
        /// 分頁
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="offset"></param>
        /// <param name="limit"></param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, int offset = 0, int limit = 0)
        {
            if (limit > 0)
            {
                return queryable.Skip(offset).Take(limit);
            }
            else
            {
                if (offset > 0)
                {
                    return queryable.Skip(offset);
                }
                else
                {
                    return queryable;
                }
            }
        }

        /// <summary>
        /// 分頁
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="page"></param>
        /// <returns></returns>
        public static IQueryable<T> Page<T>(this IQueryable<T> queryable, IPageOptions? page)
        {
            if (page != null)
            {
                return queryable.Page(page.Offset, page.Limit);
            }
            else
            {
                return queryable;
            }            
        }


        /// <summary>
        /// 分頁
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        public static IQueryable<T> SlidingPage<T>(this IQueryable<T> queryable, int pageNumber, int pageSize)
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
        /// <param name="slidingPage"></param>
        /// <returns></returns>
        public static IQueryable<T> SlidingPage<T>(this IQueryable<T> queryable, ISlidingPageOptions slidingPage)
        {
            if (slidingPage != null)
            {
                return queryable.SlidingPage(slidingPage.PageNumber, slidingPage.PageSize);
            }
            else
            {
                return queryable;
            }
        }

        #endregion


        #region 排序

        /// <summary>
        /// 排序
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, IDataQueryOptions? options)
        {
            if (options != null && options.Sorts != null)
            {
                return queryable.Sort(options.Sorts);
            }
            return queryable;
        }

        /// <summary>
        /// 排序 (單個欄位)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="field"></param>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, string field, SortDirections direction)
        {
            if (!string.IsNullOrEmpty(field))
            {
                return queryable.Sort(new SortOptions(field, direction));
            }
            else
            {
                return queryable;
            }
        }
        /// <summary>
        /// 排序 (單個欄位)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, ISortOptions? options)
        {
            if (options != null)
            {
                // 排序
                return queryable.Sort(new MultiSortOptions(options));
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
        /// <param name="optionList"></param>
        /// <returns></returns>
        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, IEnumerable<ISortOptions> optionList)
        {
            if (optionList != null && optionList.Any())
            {
                // 排序
                return SortExpressionBuilder.ApplySort(queryable, optionList);
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
        public static IQueryable<T> Sort<T>(this IQueryable<T> queryable, IMultiSortOptions? sort)
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
        /// <param name="options"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, 
                                              IDataQueryOptions? options,
                                              bool ignoreCase = false)
        {
            if (options != null && options.Filters != null)
            {
                return queryable.Filter(options.Filters, ignoreCase);
            }
            return queryable;
        }

        /// <summary>
        /// 篩選欄位 (單個欄位)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <param name="type"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, 
                                              string field,
                                              string value, 
                                              FilterOperators type = FilterOperators.StartsWith,
                                              bool ignoreCase = false)
        {
            if (!string.IsNullOrEmpty(field))
            {
                var options = new FilterOptions(field, value, type);
                return queryable.Filter(options, ignoreCase);
            }
            else
            {
                return queryable;
            }
        }
        /// <summary>
        /// 篩選欄位 (單個欄位)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable,
                                              IFilterOptions? options,
                                              bool ignoreCase = false)
        {
            if (options != null)
            {
                return queryable.Filter(new MultiFilterOptions(options), ignoreCase);
            }
            else
            {
                return queryable;
            }
        }


        /// <summary>
        ///  篩選欄位 (多個欄位)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="filters"></param>
        /// <param name="logic"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable,
                                              IEnumerable<IFilterOptions> filters,
                                              FilterLogics logic = FilterLogics.And,
                                              bool ignoreCase = false)
        {
            if (filters != null)
            {
                return queryable.Filter(new MultiFilterOptions(filters, logic), ignoreCase);
            }
            else
            {
                return queryable;
            }
        }
        /// <summary>
        /// 篩選欄位 (多個欄位)
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="queryable"></param>
        /// <param name="options"></param>
        /// <param name="ignoreCase"></param>
        /// <returns></returns>
        public static IQueryable<T> Filter<T>(this IQueryable<T> queryable, 
                                              IMultiFilterOptions? options,
                                              bool ignoreCase = false)
        {
            if (options != null)
            {
                var expressions = FilterExpressionBuilder.GetFilterExpression<T>(options.Logic,
                                                                                 options.Filters,
                                                                                 ignoreCase);

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
