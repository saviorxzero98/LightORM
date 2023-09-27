using LightORM.EntityFrameworkCore.DataQuery;
using System.Linq.Expressions;
using System.Reflection;

namespace LightORM.EntityFrameworkCore.Extensions
{
    public static class FilterExpressionBuilder
    {
        private static readonly MethodInfo _containsMethod = typeof(string).GetMethod(nameof(DataFilterOperator.Contains), 
                                                                                      new[] { typeof(string) });
        private static readonly MethodInfo _startsWithMethod = typeof(string).GetMethod(nameof(DataFilterOperator.StartsWith),
                                                                                        new[] { typeof(string) });
        private static readonly MethodInfo _endsWithMethod = typeof(string).GetMethod(nameof(DataFilterOperator.EndsWith), 
                                                                                      new[] { typeof(string) });
        private static Expression<Func<T, bool>> DefaultExpression<T>() => _ => true;


        /// <summary>
        /// 取得 Filter Expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="filterLogic"></param>
        /// <param name="filters"></param>
        /// <returns></returns>
        public static Expression<Func<T, bool>>? GetFilterExpression<T>(DataFilterLogic filterLogic, IEnumerable<DataFilter> filters)
        {
            if (!(filters is object) || !filters.Any())
            {
                return DefaultExpression<T>();
            }

            var param = Expression.Parameter(typeof(T), "item");
            var filterList = filters
                .ToList()
                .Where(filter =>
                {
                    var property = typeof(T).GetProperty(filter.Field, 
                                                         BindingFlags.IgnoreCase |
                                                         BindingFlags.Public |
                                                         BindingFlags.Instance |
                                                         BindingFlags.Static);
                    return (property is object && 
                            !string.IsNullOrEmpty(filter.Value));
                })
                .Select(filter => Expression.Lambda<Func<T, bool>>(GetFilterExpression<T>(filter.Operator,
                                                                                          param, 
                                                                                          filter.Value, 
                                                                                          filter.Field), 
                        param));

            return CombineExpressions(filterList, filterLogic);
        }

        /// <summary>
        /// 合併 Expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expressions"></param>
        /// <param name="filterLogic"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        private static Expression<Func<T, bool>> CombineExpressions<T>(IEnumerable<Expression<Func<T, bool>>> expressions,
                                                                       DataFilterLogic filterLogic)
        {
            if (!(expressions is object) || !expressions.Any())
            {
                return DefaultExpression<T>();
            }

            Expression<Func<T, bool>> result = null;

            foreach (var expression in expressions)
            {
                if (!(result is object))
                {
                    result = expression;
                    continue;
                }

                switch (filterLogic)
                {
                    case DataFilterLogic.And:
                        result = VisitExpression(Expression.AndAlso, result, expression);
                        break;

                    case DataFilterLogic.Or:
                        result = VisitExpression(Expression.OrElse, result, expression);
                        break;

                    default:
                        continue;
                }
            }

            return result ?? DefaultExpression<T>();
        }

        /// <summary>
        /// Visit Expression
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="method"></param>
        /// <param name="left"></param>
        /// <param name="right"></param>
        /// <returns></returns>
        private static Expression<Func<T, bool>> VisitExpression<T>(Func<Expression, Expression, BinaryExpression> method,
                                                                    Expression<Func<T, bool>> left,
                                                                    Expression<Func<T, bool>> right)
        {
            var parameter = Expression.Parameter(typeof(T));

            var leftVisitor = new ReplaceExpressionVisitor(left.Parameters[0], parameter);
            var newLeft = leftVisitor.Visit(left.Body);

            var rightVisitor = new ReplaceExpressionVisitor(right.Parameters[0], parameter);
            var newRight = rightVisitor.Visit(right.Body);

            return Expression.Lambda<Func<T, bool>>(method(newLeft, newRight), parameter);
        }

        private static Expression GetFilterExpression<T>(DataFilterOperator filterOperator, 
                                                         ParameterExpression param, 
                                                         string filterValue, string fieldName)
        {
            MemberExpression member = Expression.Property(param, fieldName);
            ConstantExpression constant = GetConstantExpression(filterValue, member);
            Expression expression = GetExpression(filterOperator, member, constant);
            return expression;
        }

        /// <summary>
        /// Get Constant Expression
        /// </summary>
        /// <param name="filterValue"></param>
        /// <param name="member"></param>
        /// <returns></returns>
        private static ConstantExpression GetConstantExpression(string filterValue, MemberExpression member)
        {
            if (member.Type == typeof(bool) || member.Type == typeof(bool?))
            {
                return Expression.Constant(bool.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(byte) || member.Type == typeof(byte?))
            {
                return Expression.Constant(byte.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(char) || member.Type == typeof(char?))
            {
                return Expression.Constant(char.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(DateTime) || member.Type == typeof(DateTime?))
            {
                return Expression.Constant(DateTime.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(DateTimeOffset) || member.Type == typeof(DateTimeOffset?))
            {
                return Expression.Constant(DateTimeOffset.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(decimal) || member.Type == typeof(decimal?))
            {
                return Expression.Constant(decimal.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(double) || member.Type == typeof(double?))
            {
                return Expression.Constant(double.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(float) || member.Type == typeof(float?))
            {
                return Expression.Constant(float.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(Guid) || member.Type == typeof(Guid?))
            {
                return Expression.Constant(Guid.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(int) || member.Type == typeof(int?))
            {
                return Expression.Constant(int.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(long) || member.Type == typeof(long?))
            {
                return Expression.Constant(long.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(sbyte) || member.Type == typeof(sbyte?))
            {
                return Expression.Constant(sbyte.Parse(filterValue), member.Type);
            }
            else if (member.Type == typeof(short) || member.Type == typeof(short?))
            {
                return Expression.Constant(short.Parse(filterValue), member.Type);
            }
            else
            {
                return Expression.Constant(filterValue, member.Type);
            }
        }

        /// <summary>
        /// Get Expression
        /// </summary>
        /// <param name="filterOperator"></param>
        /// <param name="member"></param>
        /// <param name="constant"></param>
        /// <returns></returns>
        private static Expression GetExpression(DataFilterOperator filterOperator, 
                                                MemberExpression member, 
                                                ConstantExpression constant)
        {
            switch (filterOperator)
            {
                case DataFilterOperator.GreaterThanOrEqual:
                    return Expression.GreaterThanOrEqual(member, constant);
 
                case DataFilterOperator.LessThanOrEqual:
                    return Expression.LessThanOrEqual(member, constant);

                case DataFilterOperator.GreaterThan:
                    return Expression.GreaterThan(member, constant);

                case DataFilterOperator.LessThan:
                    return Expression.LessThan(member, constant);

                case DataFilterOperator.Equals:
                    return Expression.Equal(member, constant);

                case DataFilterOperator.NotEquals:
                    return Expression.NotEqual(member, constant);

                case DataFilterOperator.Contains:
                    return Expression.Call(member, _containsMethod, constant);

                case DataFilterOperator.DoesNotContain:
                    return Expression.Not(Expression.Call(member, _containsMethod, constant));

                case DataFilterOperator.EndsWith:
                    return Expression.Call(member, _endsWithMethod, constant);

                case DataFilterOperator.StartsWith:
                    return Expression.Call(member, _startsWithMethod, constant);

                default:
                    return default(Expression);
            }
        }

        /// <summary>
        /// Expression
        /// </summary>
        private class ReplaceExpressionVisitor : ExpressionVisitor
        {
            private readonly Expression _oldValue;
            private readonly Expression _newValue;

            public ReplaceExpressionVisitor(Expression oldValue, Expression newValue)
            {
                _oldValue = oldValue;
                _newValue = newValue;
            }

            public override Expression Visit(Expression node)
            {
                if (node == _oldValue)
                {
                    return _newValue;
                }
                else
                {
                    return base.Visit(node);
                }
            }
                
        }
    }
}
