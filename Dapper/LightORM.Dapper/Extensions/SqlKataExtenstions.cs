using Dapper;
using SqlKata;
using System.Collections.Generic;
using System.Linq;

namespace LightORM.Dapper.Extensions
{
    public static class SqlKataExtenstions
    {
        /// <summary>
        /// Select All Columns
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public static Query SelectAllColumns(this Query query)
        {
            return query.Select("*");
        }

        /// <summary>
        /// Select Column
        /// </summary>
        /// <param name="query"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static Query SelectColumn(this Query query, ColumnQuery column)
        {
            if (column == null)
            {
                return query;
            }
            return query.Select(column.ToRaw());
        }

        /// <summary>
        /// Select Columns
        /// </summary>
        /// <param name="query"></param>
        /// <param name="columns"></param>
        /// <returns></returns>
        public static Query SelectColumns(this Query query, IEnumerable<ColumnQuery> columns)
        {
            if (columns == null)
            {
                return query;
            }

            string[] columnRaws = columns.Select(c => c.ToRaw()).ToArray();

            return query.Select(columnRaws);
        }


        #region SQL Result

        /// <summary>
        /// 建立 Dapper DynamicParameters
        /// </summary>
        /// <param name="sqlResult"></param>
        /// <returns></returns>
        public static DynamicParameters GetSqlParameters(this SqlResult sqlResult)
        {
            Dictionary<string, object> bindings = sqlResult.NamedBindings;
            var sqlParameters = new DynamicParameters();

            foreach (var binding in bindings)
            {
                sqlParameters.Add(binding.Key, binding.Value);
            }

            return sqlParameters;
        }

        #endregion


        #region Select With Aggregate Function

        /// <summary>
        /// Select Max
        /// </summary>
        /// <param name="query"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static Query SelectMax(this Query query, ColumnQuery column)
        {
            if (column == null)
            {
                return query;
            }
            return SelectByAggregateFunction(query, "Max", column);
        }

        /// <summary>
        /// Select Min
        /// </summary>
        /// <param name="query"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static Query SelectMin(this Query query, ColumnQuery column)
        {
            if (column == null)
            {
                return query;
            }
            return SelectByAggregateFunction(query, "Min", column);
        }

        /// <summary>
        /// Select Count
        /// </summary>
        /// <param name="query"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static Query SelectCount(this Query query, ColumnQuery column)
        {
            if (column == null)
            {
                return query;
            }
            return SelectByAggregateFunction(query, "Count", column);
        }

        /// <summary>
        /// Select Sum
        /// </summary>
        /// <param name="query"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static Query SelectSum(this Query query, ColumnQuery column)
        {
            if (column == null)
            {
                return query;
            }
            return SelectByAggregateFunction(query, "Sum", column);
        }

        /// <summary>
        /// Select Sum
        /// </summary>
        /// <param name="query"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        public static Query SelectAvg(this Query query, ColumnQuery column)
        {
            if (column == null)
            {
                return query;
            }
            return SelectByAggregateFunction(query, "Avg", column);
        }


        /// <summary>
        /// Select By Aggregate Function
        /// </summary>
        /// <param name="query"></param>
        /// <param name="function"></param>
        /// <param name="column"></param>
        /// <returns></returns>
        private static Query SelectByAggregateFunction(Query query, string function,
                                                       ColumnQuery column)
        {
            string columnName = column.ColumnName;
            string tableName = column.TableName;
            string aliasName = column.AliasName;

            if (string.IsNullOrEmpty(columnName))
            {
                return query;
            }

            string raw = $"[{columnName}]";

            if (!string.IsNullOrEmpty(tableName))
            {
                raw = $"[{tableName}].{raw}";
            }

            raw = $"{function}({raw})";

            if (!string.IsNullOrEmpty(aliasName))
            {
                raw = $"{raw} AS [{aliasName}]";
            }
            return query.SelectRaw(raw);
        }

        #endregion


        /// <summary>
        /// And Where Column
        /// </summary>
        /// <param name="query"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public static Query WhereColumn(this Query query, WhereCondition whereCondition)
        {
            if (whereCondition == null)
            {
                return query;
            }

            // 檢查是否為 Sub Where
            if (whereCondition.SubWhere != null &&
                whereCondition.SubWhere.Any())
            {
                return query.Where(q => q.WhereColumns(whereCondition.SubWhere));
            }

            // 檢查是否有 Operation
            if (string.IsNullOrEmpty(whereCondition.Operation))
            {   // Equals
                return query.Where(whereCondition.Column, whereCondition.Value);
            }

            // 處理 Operation
            switch (whereCondition.Operation.ToUpper())
            {
                case WhereOperation.Null:
                    return query.WhereNull(whereCondition.Column);

                case WhereOperation.Like:
                    return query.WhereLike(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Starts:
                    return query.WhereStarts(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Ends:
                    return query.WhereEnds(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Contains:
                    return query.WhereContains(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                default:
                    return query.Where(whereCondition.Column, whereCondition.Operation, whereCondition.Value);
            }
        }

        /// <summary>
        /// And Where Column Not
        /// </summary>
        /// <param name="query"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public static Query WhereColumnNot(this Query query, WhereCondition whereCondition)
        {
            if (whereCondition == null)
            {
                return query;
            }

            // 檢查是否為 Sub Where
            if (whereCondition.SubWhere != null &&
                whereCondition.SubWhere.Any())
            {
                return query.WhereNot(q => q.WhereColumns(whereCondition.SubWhere));
            }

            // 檢查是否有 Operation
            if (string.IsNullOrEmpty(whereCondition.Operation))
            {   // Equals
                return query.WhereNot(whereCondition.Column, whereCondition.Value);
            }

            // 處理 Operation
            switch (whereCondition.Operation.ToUpper())
            {
                case WhereOperation.Null:
                    return query.WhereNotNull(whereCondition.Column);

                case WhereOperation.Like:
                    return query.WhereNotLike(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Starts:
                    return query.WhereNotStarts(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Ends:
                    return query.WhereNotEnds(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Contains:
                    return query.WhereNotContains(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                default:
                    return query.WhereNot(whereCondition.Column, whereCondition.Operation, whereCondition.Value);
            }
        }

        /// <summary>
        /// Or Where Column
        /// </summary>
        /// <param name="query"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public static Query OrWhereColumn(this Query query, WhereCondition whereCondition)
        {
            if (whereCondition == null)
            {
                return query;
            }

            // 檢查是否為 Sub Where
            if (whereCondition.SubWhere != null &&
                whereCondition.SubWhere.Any())
            {
                return query.OrWhere(q => q.WhereColumns(whereCondition.SubWhere));
            }

            // 檢查是否有 Operation
            if (string.IsNullOrEmpty(whereCondition.Operation))
            {   // Equals
                return query.OrWhere(whereCondition.Column, whereCondition.Value);
            }

            // 處理 Operation
            switch (whereCondition.Operation.ToUpper())
            {
                case WhereOperation.Null:
                    return query.OrWhereNull(whereCondition.Column);

                case WhereOperation.Like:
                    return query.OrWhereLike(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Starts:
                    return query.OrWhereStarts(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Ends:
                    return query.OrWhereEnds(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Contains:
                    return query.OrWhereContains(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                default:
                    return query.OrWhere(whereCondition.Column, whereCondition.Operation, whereCondition.Value);
            }
        }

        /// <summary>
        /// Or Where Column
        /// </summary>
        /// <param name="query"></param>
        /// <param name="whereCondition"></param>
        /// <returns></returns>
        public static Query OrWhereColumnNot(this Query query, WhereCondition whereCondition)
        {
            if (whereCondition == null)
            {
                return query;
            }

            // 檢查是否為 Sub Where
            if (whereCondition.SubWhere != null &&
                whereCondition.SubWhere.Any())
            {
                return query.OrWhereNot(q => q.WhereColumns(whereCondition.SubWhere));
            }

            // 檢查是否有 Operation
            if (string.IsNullOrEmpty(whereCondition.Operation))
            {   // Equals
                return query.OrWhereNot(whereCondition.Column, whereCondition.Value);
            }

            // 處理 Operation
            switch (whereCondition.Operation.ToUpper())
            {
                case WhereOperation.Null:
                    return query.OrWhereNotNull(whereCondition.Column);

                case WhereOperation.Like:
                    return query.OrWhereNotLike(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Starts:
                    return query.OrWhereNotStarts(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Ends:
                    return query.OrWhereNotEnds(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                case WhereOperation.Contains:
                    return query.OrWhereNotContains(whereCondition.Column, whereCondition.Value, whereCondition.CaseSensitive);

                default:
                    return query.OrWhereNot(whereCondition.Column, whereCondition.Operation, whereCondition.Value);
            }
        }

        /// <summary>
        /// Where Columns
        /// </summary>
        /// <param name="query"></param>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public static Query WhereColumns(this Query query, List<WhereCondition> whereConditions)
        {
            Query outQuery = query;

            foreach (WhereCondition whereCondition in whereConditions)
            {
                switch (whereCondition.WhereType)
                {
                    case WhereCondictionType.And:
                        outQuery = outQuery.WhereColumn(whereCondition);
                        break;

                    case WhereCondictionType.AndNot:
                        outQuery = outQuery.WhereColumnNot(whereCondition);
                        break;

                    case WhereCondictionType.Or:
                        outQuery = outQuery.OrWhereColumn(whereCondition);
                        break;

                    case WhereCondictionType.OrNot:
                        outQuery = outQuery.OrWhereColumnNot(whereCondition);
                        break;
                }
            }
            return outQuery;
        }

        /// <summary>
        /// Order By Column
        /// </summary>
        /// <param name="query"></param>
        /// <param name="orderByCondition"></param>
        /// <returns></returns>
        public static Query OrderByColumns(this Query query, OrderByCondition orderByCondition)
        {
            if (orderByCondition == null ||
                orderByCondition.Columns == null ||
                !orderByCondition.Columns.Any(c => !string.IsNullOrWhiteSpace(c)))
            {
                return query;
            }

            List<string> columns = orderByCondition.Columns
                                                    .Where(c => !string.IsNullOrWhiteSpace(c))
                                                    .ToList();
            if (orderByCondition.IsDesc)
            {
                query = query.OrderByDesc(columns.ToArray());
            }
            else
            {
                query = query.OrderBy(columns.ToArray());
            }
            return query;
        }

        /// <summary>
        /// Order By Column
        /// </summary>
        /// <param name="query"></param>
        /// <param name="orderByConditions"></param>
        /// <returns></returns>
        public static Query OrderByColumns(this Query query, List<OrderByCondition> orderByConditions)
        {
            if (orderByConditions != null &&
                orderByConditions.Any())
            {
                foreach (var orderByCondition in orderByConditions)
                {
                    query = query.OrderByColumns(orderByCondition);
                }
            }
            return query;
        }

        /// <summary>
        /// SQLKata Where Condition
        /// </summary>
        public class WhereCondition
        {
            #region Internal Flag

            /// <summary>
            /// Where類型，
            /// 0：AND,
            /// 1：OR,
            /// 2：AND NOT,
            /// 3：OR NOT
            /// </summary>
            internal WhereCondictionType WhereType { get; set; }

            /// <summary>
            /// 字串 LIKE 比對時，是否忽略大小寫，true：區分大小寫；false 不分大小寫
            /// </summary>
            internal bool CaseSensitive { get; set; }

            #endregion


            #region Property

            /// <summary>
            /// 欄位名稱
            /// </summary>
            public string Column { get; set; }

            /// <summary>
            /// 比對運算
            /// </summary>
            public string Operation { get; set; } = string.Empty;

            /// <summary>
            /// 欄位值
            /// </summary>
            public object Value { get; set; }

            /// <summary>
            /// SubWhere Condiction
            /// </summary>
            public List<WhereCondition> SubWhere { get; set; } = new List<WhereCondition>();

            #endregion


            #region Constructor

            public WhereCondition(WhereCondictionType type, string column, object value)
            {
                Column = column;
                Operation = string.Empty;
                Value = value;
                WhereType = type;
                CaseSensitive = true;
            }
            public WhereCondition(WhereCondictionType type, string column, string op, object value)
            {
                Column = column;
                Operation = op;
                Value = value;
                WhereType = type;
                CaseSensitive = true;
            }
            public WhereCondition(WhereCondictionType type, List<WhereCondition> subWhere)
            {
                Column = string.Empty;
                Operation = string.Empty;
                SubWhere = subWhere ?? new List<WhereCondition>();
                WhereType = type;
                CaseSensitive = true;
            }

            #endregion


            #region And Where

            /// <summary>
            /// And Where
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static WhereCondition Where(string column, object value)
            {
                return new WhereCondition(WhereCondictionType.And, column, value);
            }
            /// <summary>
            /// And Where
            /// </summary>
            /// <param name="column"></param>
            /// <param name="op"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static WhereCondition Where(string column, string op, object value)
            {
                return new WhereCondition(WhereCondictionType.And, column, op, value);
            }
            /// <summary>
            /// And Where
            /// </summary>
            /// <param name="subWhere"></param>
            /// <returns></returns>
            public static WhereCondition Where(List<WhereCondition> subWhere)
            {
                return new WhereCondition(WhereCondictionType.And, subWhere);
            }
            /// <summary>
            /// And Where Like
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <param name="compareType"></param>
            /// <param name="ignoreCase"></param>
            /// <returns></returns>
            public static WhereCondition WhereString(string column, string value,
                                                     string compareType = WhereOperation.Like, bool ignoreCase = false)
            {
                string op = WhereOperation.Like;
                switch (compareType.ToUpper())
                {
                    case WhereOperation.Starts:
                    case WhereOperation.Ends:
                    case WhereOperation.Contains:
                        op = compareType.ToUpper();
                        break;
                }

                return new WhereCondition(WhereCondictionType.And, column, op, value)
                {
                    CaseSensitive = !ignoreCase
                };
            }
            /// <summary>
            /// And Where Null
            /// </summary>
            /// <param name="column"></param>
            /// <returns></returns>
            public static WhereCondition WhereNull(string column)
            {
                return new WhereCondition(WhereCondictionType.And, column, WhereOperation.Null, null);
            }


            /// <summary>
            /// And Where Not
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static WhereCondition WhereNot(string column, object value)
            {
                return new WhereCondition(WhereCondictionType.AndNot, column, value);
            }
            /// <summary>
            /// And Where Not
            /// </summary>
            /// <param name="column"></param>
            /// <param name="op"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static WhereCondition WhereNot(string column, string op, object value)
            {
                return new WhereCondition(WhereCondictionType.AndNot, column, op, value);
            }
            /// <summary>
            /// And Where Not
            /// </summary>
            /// <param name="subWhere"></param>
            /// <returns></returns>
            public static WhereCondition WhereNot(List<WhereCondition> subWhere)
            {
                return new WhereCondition(WhereCondictionType.AndNot, subWhere);
            }
            /// <summary>
            /// And Where Not Like
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <param name="compareType"></param>
            /// <returns></returns>
            public static WhereCondition WhereNotString(string column, string value,
                                                        string compareType = WhereOperation.Like, bool ignoreCase = false)
            {
                string op = WhereOperation.Like;
                switch (compareType.ToUpper())
                {
                    case WhereOperation.Starts:
                    case WhereOperation.Ends:
                    case WhereOperation.Contains:
                        op = compareType.ToUpper();
                        break;
                }

                return new WhereCondition(WhereCondictionType.AndNot, column, op, value)
                {
                    CaseSensitive = !ignoreCase
                };
            }
            /// <summary>
            /// And Where Not Null
            /// </summary>
            /// <param name="column"></param>
            /// <returns></returns>
            public static WhereCondition WhereNotNull(string column)
            {
                return new WhereCondition(WhereCondictionType.AndNot, column, WhereOperation.Null, null);
            }

            #endregion


            #region Or Where

            /// <summary>
            /// Or Where
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static WhereCondition OrWhere(string column, object value)
            {
                return new WhereCondition(WhereCondictionType.Or, column, value);
            }
            /// <summary>
            /// Or Where
            /// </summary>
            /// <param name="column"></param>
            /// <param name="op"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static WhereCondition OrWhere(string column, string op, object value)
            {
                return new WhereCondition(WhereCondictionType.Or, column, op, value);
            }
            /// <summary>
            /// Or Where
            /// </summary>
            /// <param name="subWhere"></param>
            /// <returns></returns>
            public static WhereCondition OrWhere(List<WhereCondition> subWhere)
            {
                return new WhereCondition(WhereCondictionType.Or, subWhere);
            }
            /// <summary>
            /// Or Where Like
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <param name="compareType"></param>
            /// <param name="ignoreCase"></param>
            /// <returns></returns>
            public static WhereCondition OrWhereString(string column, string value,
                                                       string compareType = WhereOperation.Like, bool ignoreCase = false)
            {
                string op = WhereOperation.Like;
                switch (compareType.ToUpper())
                {
                    case WhereOperation.Starts:
                    case WhereOperation.Ends:
                    case WhereOperation.Contains:
                        op = compareType.ToUpper();
                        break;
                }

                return new WhereCondition(WhereCondictionType.Or, column, op, value)
                {
                    CaseSensitive = !ignoreCase
                };
            }
            /// <summary>
            /// Or Where Null
            /// </summary>
            /// <param name="column"></param>
            /// <returns></returns>
            public static WhereCondition OrWhereNull(string column)
            {
                return new WhereCondition(WhereCondictionType.Or, column, WhereOperation.Null, null);
            }


            /// <summary>
            /// Or Where Not
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static WhereCondition OrWhereNot(string column, object value)
            {
                return new WhereCondition(WhereCondictionType.OrNot, column, value);
            }
            /// <summary>
            /// Or Where Not
            /// </summary>
            /// <param name="column"></param>
            /// <param name="op"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static WhereCondition OrWhereNot(string column, string op, object value)
            {
                return new WhereCondition(WhereCondictionType.OrNot, column, op, value);
            }
            /// <summary>
            /// Or Where Not
            /// </summary>
            /// <param name="subWhere"></param>
            /// <returns></returns>
            public static WhereCondition OrWhereNot(List<WhereCondition> subWhere)
            {
                return new WhereCondition(WhereCondictionType.OrNot, subWhere);
            }
            /// <summary>
            /// Or Where Not Like
            /// </summary>
            /// <param name="column"></param>
            /// <param name="value"></param>
            /// <param name="compareType"></param>
            /// <param name="ignoreCase"></param>
            /// <returns></returns>
            public static WhereCondition OrWhereNotString(string column, string value,
                                                          string compareType = WhereOperation.Like, bool ignoreCase = false)
            {
                string op = WhereOperation.Like;
                switch (compareType.ToUpper())
                {
                    case WhereOperation.Starts:
                    case WhereOperation.Ends:
                    case WhereOperation.Contains:
                        op = compareType.ToUpper();
                        break;
                }

                return new WhereCondition(WhereCondictionType.OrNot, column, op, value)
                {
                    CaseSensitive = !ignoreCase
                };
            }
            /// <summary>
            /// Or Where Not Null
            /// </summary>
            /// <param name="column"></param>
            /// <returns></returns>
            public static WhereCondition OrWhereNotNull(string column)
            {
                return new WhereCondition(WhereCondictionType.OrNot, column, WhereOperation.Null, null);
            }

            #endregion
        }

        /// <summary>
        /// SQLKata Where Condiction Type
        /// </summary>
        public enum WhereCondictionType
        {
            And = 0,
            Or = 1,
            AndNot = 2,
            OrNot = 3
        }

        /// <summary>
        /// SQLKata Where Operation
        /// </summary>
        public class WhereOperation
        {
            public const string Like = "LIKE";

            public const string Null = "NULL";

            public const string Starts = "STARTS";

            public const string Ends = "ENDS";

            public const string Contains = "CONTAINS";
        }

        /// <summary>
        /// Order By Condition
        /// </summary>
        public class OrderByCondition
        {
            /// <summary>
            /// Is DESC
            /// </summary>
            public bool IsDesc { get; set; } = false;

            /// <summary>
            /// Column
            /// </summary>
            public List<string> Columns { get; set; }


            public OrderByCondition()
            {

            }
            public OrderByCondition(bool isDesc, List<string> columns)
            {
                IsDesc = isDesc;

                if (columns != null)
                {
                    Columns = new List<string>(columns);
                    Columns = Columns.Where(c => !string.IsNullOrWhiteSpace(c))
                                     .ToList();
                }
                else
                {
                    Columns = new List<string>();
                }
            }
            public OrderByCondition(bool isDesc, params string[] columns)
            {
                IsDesc = isDesc;

                if (columns != null)
                {
                    Columns = new List<string>(columns);
                    Columns = Columns.Where(c => !string.IsNullOrWhiteSpace(c))
                                     .ToList();
                }
                else
                {
                    Columns = new List<string>();
                }
            }

            public static OrderByCondition Asc(params string[] columns)
            {
                return new OrderByCondition(false, columns);
            }
            public static OrderByCondition Asc(List<string> columns)
            {
                return new OrderByCondition(false, columns);
            }
            public static OrderByCondition Desc(params string[] columns)
            {
                return new OrderByCondition(true, columns);
            }
            public static OrderByCondition Desc(List<string> columns)
            {
                return new OrderByCondition(true, columns);
            }
        }

        public class ColumnQuery
        {
            /// <summary>
            /// Table
            /// </summary>
            public string TableName { get; set; } = string.Empty;

            /// <summary>
            /// Column Name
            /// </summary>
            public string ColumnName { get; set; } = string.Empty;

            /// <summary>
            /// Alias Name
            /// </summary>
            public string AliasName { get; set; } = string.Empty;


            public ColumnQuery()
            {

            }
            public ColumnQuery(string columnName, string aliasName = null)
            {
                ColumnName = columnName;
                AliasName = aliasName;
            }

            /// <summary>
            /// Column Name
            /// </summary>
            /// <param name="columnName"></param>
            /// <returns></returns>
            public static ColumnQuery Column(string columnName)
            {
                return new ColumnQuery(columnName);
            }
            /// <summary>
            /// Table Name
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public static ColumnQuery Table(string tableName)
            {
                return new ColumnQuery() { TableName = tableName };
            }

            /// <summary>
            /// Table Name
            /// </summary>
            /// <param name="tableName"></param>
            /// <returns></returns>
            public ColumnQuery Of(string tableName)
            {
                TableName = tableName;
                return this;
            }
            /// <summary>
            /// Column Name
            /// </summary>
            /// <param name="columnName"></param>
            /// <returns></returns>
            public ColumnQuery Col(string columnName)
            {
                ColumnName = columnName;
                return this;
            }
            /// <summary>
            /// As Alias Name
            /// </summary>
            /// <param name="aliasName"></param>
            /// <returns></returns>
            public ColumnQuery As(string aliasName)
            {
                AliasName = aliasName;
                return this;
            }


            /// <summary>
            /// To Raw
            /// </summary>
            /// <returns></returns>
            public string ToRaw()
            {
                string raw = ColumnName.Trim();

                if (!string.IsNullOrEmpty(TableName))
                {
                    raw = $"{TableName.Trim()}.{raw}";
                }

                if (!string.IsNullOrEmpty(AliasName))
                {
                    raw = $"{raw} AS {AliasName}";
                }

                return raw;
            }
        }

        public class QueryOptions
        {

            public List<string> Columns { get; set; }

            public int Limit { get; set; } = 0;

            public int Offset { get; set; } = 0;

            public List<WhereCondition> WhereConditions { get; set; }

            public List<OrderByCondition> OrderByConditions { get; set; }


            public QueryOptions()
            {

            }
            public QueryOptions(List<string> columns = null)
            {
                Columns = columns;
            }

            /// <summary>
            /// Set Limit
            /// </summary>
            /// <param name="limit"></param>
            /// <returns></returns>
            public QueryOptions SetLimit(int limit)
            {
                Limit = limit;
                return this;
            }
            /// <summary>
            /// Set Offset
            /// </summary>
            /// <param name="offset"></param>
            /// <returns></returns>
            public QueryOptions SetOffset(int offset)
            {
                Offset = offset;
                return this;
            }
            /// <summary>
            /// Set Offset & Limit
            /// </summary>
            /// <param name="offset"></param>
            /// <param name="limit"></param>
            /// <returns></returns>
            public QueryOptions SetLimitOffset(int offset, int limit)
            {
                Offset = offset;
                Limit = limit;
                return this;
            }


            /// <summary>
            /// Add Where Condition
            /// </summary>
            /// <param name="whereCondition"></param>
            /// <returns></returns>
            public QueryOptions AddWhereCondition(WhereCondition whereCondition)
            {
                if (whereCondition != null)
                {
                    if (WhereConditions == null)
                    {
                        WhereConditions = new List<WhereCondition>();
                    }
                    WhereConditions.Add(whereCondition);
                }
                return this;
            }
            /// <summary>
            /// Add Where Conditions
            /// </summary>
            /// <param name="whereConditions"></param>
            /// <returns></returns>
            public QueryOptions AddWhereConditions(List<WhereCondition> whereConditions)
            {
                if (whereConditions != null && whereConditions.Any())
                {
                    if (WhereConditions == null)
                    {
                        WhereConditions = new List<WhereCondition>();
                    }
                    WhereConditions.AddRange(whereConditions);
                }
                return this;
            }
            /// <summary>
            /// Set Where Conditions
            /// </summary>
            /// <param name="whereConditions"></param>
            /// <returns></returns>
            public QueryOptions SetWhereConditions(List<WhereCondition> whereConditions)
            {
                if (whereConditions != null)
                {
                    WhereConditions = whereConditions;
                }
                return this;
            }


            /// <summary>
            /// Add Order By Condition
            /// </summary>
            /// <param name="orderbyCondition"></param>
            /// <returns></returns>
            public QueryOptions AddOrderByCondition(OrderByCondition orderbyCondition)
            {
                if (orderbyCondition != null)
                {
                    if (OrderByConditions == null)
                    {
                        OrderByConditions = new List<OrderByCondition>();
                    }
                    OrderByConditions.Add(orderbyCondition);
                }
                return this;
            }
            /// <summary>
            /// Add Order By Conditions
            /// </summary>
            /// <param name="orderbyConditions"></param>
            /// <returns></returns>
            public QueryOptions AddOrderByConditions(List<OrderByCondition> orderbyConditions)
            {
                if (orderbyConditions != null && orderbyConditions.Any())
                {
                    if (OrderByConditions == null)
                    {
                        OrderByConditions = new List<OrderByCondition>();
                    }
                    OrderByConditions.AddRange(orderbyConditions);
                }
                return this;
            }
            /// <summary>
            /// Set Order By Conditions
            /// </summary>
            /// <param name="orderbyConditions"></param>
            /// <returns></returns>
            public QueryOptions SetOrderByConditions(List<OrderByCondition> orderbyConditions)
            {
                if (orderbyConditions != null)
                {
                    OrderByConditions = orderbyConditions;
                }
                return this;
            }
        }
    }
}
