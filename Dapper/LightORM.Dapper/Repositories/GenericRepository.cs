using Dapper;
using LightORM.Dapper.Adapters;
using LightORM.Dapper.Entities;
using LightORM.Dapper.UnitOfWorks;
using SqlKata;
using SqlKata.Execution;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using static LightORM.Dapper.Extensions.SqlKataExtenstions;

namespace LightORM.Dapper.Repositories
{
    public class GenericRepository<TEntity> : IRepository<TEntity> where TEntity : class
    {
        #region Constant

        /// <summary>
        /// Select All
        /// </summary>
        protected const string SelectAllColumn = "*";

        /// <summary>
        /// Select No Limit
        /// </summary>
        protected const int SelectNoLimit = 0;

        #endregion


        #region Property

        /// <summary>
        /// Unit of Work
        /// </summary>
        private readonly IUnitOfWork _unitOfWork;

        /// <summary>
        /// Database Adapter
        /// </summary>
        private readonly IDbAdapter _dbAdapter;

        /// <summary>
        /// Get Database Connection
        /// </summary>
        protected IDbConnection Connection { get => _unitOfWork.Connection; }

        #endregion


        public GenericRepository(IDbAdapter dbAdapter, IUnitOfWork unitOfWork)
        {
            _dbAdapter = dbAdapter;
            _unitOfWork = unitOfWork;
        }


        #region Common

        /// <summary>
        /// Get TableName
        /// </summary>
        /// <returns></returns>
        public string GetTableName()
        {
            dynamic tableattr = typeof(TEntity).GetCustomAttributes(false)
                                               .SingleOrDefault(attr => attr.GetType().Name == nameof(TableAttribute));
            var name = string.Empty;

            if (tableattr != null)
            {
                name = tableattr.Name;
            }

            return name;
        }

        /// <summary>
        /// 建立 SQL Query Factory
        /// </summary>
        /// <param name="connection"></param>
        /// <returns></returns>
        public QueryFactory CreateSqlQueryFactory()
        {
            if (_dbAdapter == null)
            {
                throw new NullReferenceException("DbAdapter is null.");
            }

            var factory = new QueryFactory(Connection, _dbAdapter.GetSqlCompiler());
            return factory;
        }

        #endregion


        #region Select

        /// <summary>
        /// Get First
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public TEntity Get(QueryOptions options)
        {
            if (Connection != null)
            {
                // 取得 SQL Query
                Query sqlQuery = GetSelectSqlQuery(options);

                // 執行 SQL
                var factory = CreateSqlQueryFactory();
                var result = factory.FromQuery(sqlQuery)
                                    .FirstOrDefault<TEntity>();
                return result;
            }
            return default(TEntity);
        }

        /// <summary>
        /// Get All
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public IEnumerable<TEntity> GetAll(QueryOptions options)
        {
            var results = new List<TEntity>();
            if (Connection != null)
            {
                // 取得 SQL Query
                Query sqlQuery = GetSelectSqlQuery(options);

                // 執行 SQL
                var factory = CreateSqlQueryFactory();
                results = factory.FromQuery(sqlQuery)
                                 .Get<TEntity>()
                                 .ToList();
            }

            return results;
        }

        /// <summary>
        /// Get Count
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        public int GetCount(QueryOptions options)
        {
            if (Connection != null)
            {
                if (_dbAdapter == null)
                {
                    throw new NullReferenceException("DbAdapter is null.");
                }

                // 取得 SQL Query
                Query sqlQuery = GetSelectCountSqlQuery(options);

                // 執行 SQL
                var factory = CreateSqlQueryFactory();
                var result = factory.FromQuery(sqlQuery)
                                    .FirstOrDefault<RecordCountEntity>();

                if (result != null)
                {
                    return result.Count;
                }
            }
            return 0;
        }

        /// <summary>
        /// Create Select Sql Statement
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected Query GetSelectSqlQuery(QueryOptions options)
        {
            // 建立 SQL Statement
            Query query = new Query(GetTableName());

            // Select Column
            if (options.Columns != null && options.Columns.Any())
            {
                query = query.Select(options.Columns.ToArray());
            }
            else
            {
                query = query.Select(SelectAllColumn);
            }
            return GetSelectSqlQuery(query, options);
        }

        /// <summary>
        ///  Create Select Sql Statement
        /// </summary>
        /// <param name="query"></param>
        /// <param name="options"></param>
        /// <returns></returns>
        protected Query GetSelectSqlQuery(Query query, QueryOptions options)
        {
            if (options == null)
            {
                return query;
            }

            // Limit
            if (options.Limit > 0)
            {
                query = query.Limit(options.Limit);
            }

            // Offset
            if (options.Offset >= 0)
            {
                query = query.Offset(options.Offset);
            }

            // Where
            if (options.WhereConditions != null && options.WhereConditions.Any())
            {
                query = query.WhereColumns(options.WhereConditions);
            }

            // Order By
            if (options.OrderByConditions != null &&
                options.OrderByConditions.Any())
            {
                query = query.OrderByColumns(options.OrderByConditions);
            }
            return query;
        }

        /// <summary>
        /// Create Select Count Sql Statement
        /// </summary>
        /// <param name="options"></param>
        /// <returns></returns>
        protected Query GetSelectCountSqlQuery(QueryOptions options)
        {
            // 建立 SQL Statement
            Query query = new Query(GetTableName());

            // Where
            if (options.WhereConditions != null && options.WhereConditions.Any())
            {
                query = query.WhereColumns(options.WhereConditions);
            }

            // Count
            query.AsCount();
            return query;
        }

        #endregion


        #region Insert

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Insert(TEntity entity)
        {
            if (Connection != null && entity != null)
            {
                // 執行 SQL
                var factory = CreateSqlQueryFactory();
                int result = factory.Query(GetTableName())
                                    .Insert(entity);
                return result;
            }
            return 0;
        }

        /// <summary>
        /// Insert
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public async Task<int> InsertAsync(TEntity data)
        {
            if (Connection != null && data != null)
            {
                // 執行 SQL
                var factory = CreateSqlQueryFactory();
                int result = await factory.Query(GetTableName())
                                          .InsertAsync(data);
                return result;
            }
            return 0;
        }

        #endregion


        #region Update

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public int Update(TEntity entity, List<WhereCondition> whereConditions)
        {
            if (Connection != null && entity != null)
            {
                if (whereConditions.Any())
                {
                    // 取得 SQL Query
                    Query sqlQuery = GetUpdateSqlQuery(whereConditions);

                    // 執行 SQL
                    var factory = CreateSqlQueryFactory();
                    int result = factory.FromQuery(sqlQuery)
                                        .Update(entity);
                    return result;
                }
                else
                {
                    int result = Connection.Update(entity);
                    return result;
                }
            }
            return 0;
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public int Update(TEntity entity, params WhereCondition[] whereConditions)
        {
            if (whereConditions != null)
            {
                return Update(entity, whereConditions.ToList());
            }
            return Update(entity, new List<WhereCondition>());
        }

        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public async Task<int> UpdateAsync(TEntity entity, List<WhereCondition> whereConditions)
        {
            if (Connection != null && entity != null)
            {
                if (whereConditions.Any())
                {
                    // 取得 SQL Query
                    Query sqlQuery = GetUpdateSqlQuery(whereConditions);

                    // 執行 SQL
                    var factory = CreateSqlQueryFactory();
                    int result = await factory.FromQuery(sqlQuery)
                                              .UpdateAsync(entity);
                    return result;
                }
                else
                {
                    int result = Connection.Update(entity);
                    return result;
                }
            }
            return 0;
        }
        /// <summary>
        /// Update
        /// </summary>
        /// <param name="entity"></param>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public Task<int> UpdateAsync(TEntity entity, params WhereCondition[] whereConditions)
        {
            if (whereConditions != null)
            {
                return UpdateAsync(entity, whereConditions.ToList());
            }
            return UpdateAsync(entity);
        }


        /// <summary>
        /// Create Update SQL Query
        /// </summary>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        protected Query GetUpdateSqlQuery(List<WhereCondition> whereConditions)
        {
            // 建立 SQL Query
            Query query = new Query(GetTableName());

            // Where
            if (whereConditions != null && whereConditions.Any())
            {
                query = query.WhereColumns(whereConditions);
            }
            return query;
        }

        #endregion


        #region Delete

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public int Delete(params WhereCondition[] whereConditions)
        {
            if (whereConditions != null)
            {
                return Delete(whereConditions.ToList());
            }
            return 0;
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public int Delete(List<WhereCondition> whereConditions)
        {
            if (Connection != null)
            {
                // 取得 SQL Query
                Query sqlQuery = GetDeleteSqlQuery(whereConditions);

                // 執行 SQL
                var factory = CreateSqlQueryFactory();
                int result = factory.FromQuery(sqlQuery)
                                    .Delete();
                return result;
            }
            return 0;
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public int Delete(TEntity entity)
        {
            if (entity != null && Connection != null)
            {
                return Connection.Delete(entity);
            }
            return 0;
        }

        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public Task<int> DeleteAsync(params WhereCondition[] whereConditions)
        {
            if (whereConditions != null)
            {
                return DeleteAsync(whereConditions.ToList());
            }
            return Task.FromResult(0);
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(List<WhereCondition> whereConditions)
        {
            if (Connection != null)
            {
                // 取得 SQL Query
                Query sqlQuery = GetDeleteSqlQuery(whereConditions);

                // 執行 SQL
                var factory = CreateSqlQueryFactory();
                int result = await factory.FromQuery(sqlQuery)
                                          .DeleteAsync();
                return result;
            }
            return 0;
        }
        /// <summary>
        /// Delete
        /// </summary>
        /// <param name="entity"></param>
        /// <returns></returns>
        public async Task<int> DeleteAsync(TEntity entity)
        {
            if (entity != null && Connection != null)
            {
                return await Connection.DeleteAsync(entity);
            }
            return 0;
        }

        /// <summary>
        /// Create Delete SQL Quert
        /// </summary>
        /// <param name="whereConditions"></param>
        /// <returns></returns>
        protected Query GetDeleteSqlQuery(List<WhereCondition> whereConditions = null)
        {
            // 建立 SQL Statement
            Query query = new Query(GetTableName());

            // Where
            if (whereConditions != null && whereConditions.Any())
            {
                query = query.WhereColumns(whereConditions);
            }

            // Update
            query.AsDelete();

            return query;
        }

        #endregion
    }
}
