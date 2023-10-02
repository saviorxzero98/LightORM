using LightORM.EntityFrameworkCore.DataQuery;
using System.Linq.Expressions;

namespace LightORM.EntityFrameworkCore.Extensions
{
    public static class SortExpressionBuilder
    {
        public static IQueryable<T> ApplySort<T>(IQueryable<T> queryable, IEnumerable<DataSortField> sorts)
        {
            if (sorts == null || 
                !sorts.Any())
            {
                return queryable;
            }

            // 是否為 IOrderedQueryable Type
            var isOrderedType = queryable.Expression.Type.Equals(typeof(IOrderedQueryable<T>));
            var sortList = sorts.ToList();
            var itemType = typeof(T);
            var parameter = Expression.Parameter(itemType, "item");
            foreach (var sort in sortList)
            {
                var property = typeof(T).GetProperty(sort.Field);

                if (property == null)
                {
                    continue;
                }
                if (queryable == null)
                {
                    continue;
                }

                var propertyAccess = Expression.MakeMemberAccess(parameter, property);
                var sortExpression = Expression.Lambda(propertyAccess, parameter);
                MethodCallExpression methodCall;

                switch (sort.Direction)
                {
                    case DataSortDirection.Asc:
                        // 第1個先 OrderBy，後面皆是 ThenBy
                        if (isOrderedType)
                        {
                            methodCall = Expression.Call(typeof(Queryable),
                                                         nameof(Queryable.ThenBy),
                                                         new Type[] { itemType, property.PropertyType },
                                                         queryable.Expression,
                                                         Expression.Quote(sortExpression));
                        }
                        else
                        {
                            methodCall = Expression.Call(typeof(Queryable),
                                                         nameof(Queryable.OrderBy),
                                                         new Type[] { itemType, property.PropertyType },
                                                         queryable.Expression,
                                                         Expression.Quote(sortExpression));
                        }
                        break;

                    case DataSortDirection.Desc:
                        // 第1個先 OrderBy，後面皆是 ThenBy
                        if (isOrderedType)
                        {
                            methodCall = Expression.Call(typeof(Queryable),
                                                         nameof(Queryable.ThenByDescending),
                                                         new Type[] { itemType, property.PropertyType },
                                                         queryable.Expression,
                                                         Expression.Quote(sortExpression));
                        }
                        else
                        {
                            methodCall = Expression.Call(typeof(Queryable),
                                                         nameof(Queryable.OrderByDescending),
                                                         new Type[] { itemType,
                                                         property.PropertyType },
                                                         queryable.Expression,
                                                         Expression.Quote(sortExpression));
                        }
                        break;

                    default:
                        throw new NotImplementedException($"Unsupported sort direction: {sort.Direction}");
                }

                queryable = queryable.Provider.CreateQuery<T>(methodCall) as IOrderedQueryable<T>;
                isOrderedType = true;
            }
            return queryable;
        }
    }
}
