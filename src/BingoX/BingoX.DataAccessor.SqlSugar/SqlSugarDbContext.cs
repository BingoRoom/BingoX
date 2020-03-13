
using System.Collections.Generic;
using System;
using SqlSugar;
using System.Linq;
using System.Linq.Expressions;
using System.Collections;

namespace BingoX.DataAccessor.SqlSugar
{
    public abstract class SqlSugarDbContext : IDbContext
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="config"></param>
        public SqlSugarDbContext(ConnectionConfig config)
        {
            Database = new SqlSugarClient(config);
            ChangeTracker = new SqlSugarChangeTracker(Database);
        }
        /// <summary>
        /// SqlSugar客户端
        /// </summary>
        public SqlSugarClient Database { get; private set; }

        public SqlSugarChangeTracker ChangeTracker { get; private set; }


        public SqlSugarDbSet<TEntity> Set<TEntity>() where TEntity : class, new()
        {
            return new SqlSugarDbSet<TEntity>(Database, ChangeTracker);
        }

        /// <summary>
        /// 服务提供其在辅助数据字典中的键名
        /// </summary>
        internal const string DIConst = "ServiceProvider";
        /// <summary>
        /// 数据库上下文辅助数据字典
        /// </summary>
        public IDictionary<string, object> RootContextData { get; private set; } = new Dictionary<string, object>();
        /// <summary>
        /// 设置服务提供器
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void SetServiceProvider(System.IServiceProvider serviceProvider)
        {
            if (RootContextData.ContainsKey(DIConst)) RootContextData[DIConst] = serviceProvider;
            RootContextData.Add(DIConst, serviceProvider);
        }
        /// <summary>
        /// 获取服务提供器
        /// </summary>
        /// <returns></returns>
        public IServiceProvider GetServiceProvider()
        {
            return RootContextData.ContainsKey(DIConst) ? RootContextData[DIConst] as System.IServiceProvider : null;
        }


        public void SaveChanges()
        {
            object entity = null;
            SqlSugarEntityState state = SqlSugarEntityState.Unchanged;
            try
            {

                foreach (var item in this.ChangeTracker.Entries().Where(n => n.State != SqlSugarEntityState.Unchanged))
                {
                    entity = item.Entity;
                    state = item.State;
                    item.ExecuteCommand();
                }

            }
            catch (Exception ex)
            {

                throw new DataAccessorException("执行出错" + state + entity, ex);
            }

        }


    }
}
