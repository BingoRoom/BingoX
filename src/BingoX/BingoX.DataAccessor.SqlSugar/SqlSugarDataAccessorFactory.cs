using System;
using System.Collections;
using BingoX.Helper;
using BingoX.Domain;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarDataAccessorFactory<TContext> : DataAccessorFactory<TContext>, IDataAccessorFactory where TContext : SqlSugarDbContext
    {


        public SqlSugarDataAccessorFactory(IServiceProvider serviceProvider, DataAccessorBuilderInfo dataAccessorBuilderInfo, string connectionString) : base(serviceProvider, dataAccessorBuilderInfo, connectionString)
        {


        }




        public virtual SqlSugarSqlFacade CreateSqlFacade()
        {
            return new SqlSugarSqlFacade(DbContext);
        }
   

        /// <summary>
        /// 创建SQL命令门面
        /// </summary>
        /// <returns></returns>
        protected override ISqlFacade AbstractSqlFacade()
        {
            return CreateSqlFacade();
        }

     



        protected override Type GetDataAccessorType<TEntity>()
        {
            Type typeDataAccessor = null;
            if (typeof(IGuidEntity<TEntity>).IsAssignableFrom(typeof(TEntity)))
            {
                typeDataAccessor = typeof(SqlSugarGuidDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            else if (typeof(IIdentityEntity<TEntity>).IsAssignableFrom(typeof(TEntity)))
            {
                typeDataAccessor = typeof(SqlSugarIdentityDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            else if (typeof(ISnowflakeEntity<TEntity>).IsAssignableFrom(typeof(TEntity)))
            {
                typeDataAccessor = typeof(SqlSugarSnowflakeDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            else if (typeof(IStringEntity<TEntity>).IsAssignableFrom(typeof(TEntity)))
            {
                typeDataAccessor = typeof(SqlSugarStringIDDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            if (typeDataAccessor == null) throw new DataAccessorException("不支持的TEntity泛型类型，构建DataAccessor失败。TEntity的类型必须派生于IGuidEntity、IIdentityEntity、ISnowflakeEntity、IStringEntity。");
            return typeDataAccessor;
        }

        IJoinFacade IDataAccessorFactory.CreateJoinFacade()
        {
            return CreateJoinFacade();
        }
        ISqlFacade IDataAccessorFactory.CreateSqlFacade()
        {
            return CreateSqlFacade();
        }
    }
}
