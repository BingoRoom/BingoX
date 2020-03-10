using System;
using System.Collections;
using BingoX.Helper;
using BingoX.Domain;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace BingoX.DataAccessor.EF
{
    public class EFDataAccessorFactory<TContext> : DataAccessorFactory<TContext>, IDataAccessorFactory where TContext : EfDbContext
    {

        public EFDataAccessorFactory(IServiceProvider serviceProvider, DataAccessorBuilderInfo dataAccessorBuilderInfo, string connectionString) : base(serviceProvider, dataAccessorBuilderInfo, connectionString)
        {


        }

        public virtual EFSqlFacade CreateSqlFacade()
        {
            return new EFSqlFacade(DbContext);
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
                typeDataAccessor = typeof(EFGuidDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            else if (typeof(IIdentityEntity<TEntity>).IsAssignableFrom(typeof(TEntity)))
            {
                typeDataAccessor = typeof(EFIdentityDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            else if (typeof(ISnowflakeEntity<TEntity>).IsAssignableFrom(typeof(TEntity)))
            {
                typeDataAccessor = typeof(EFSnowflakeDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            else if (typeof(IStringEntity<TEntity>).IsAssignableFrom(typeof(TEntity)))
            {
                typeDataAccessor = typeof(EFStringIDDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            return typeDataAccessor;
        }
    }
}
