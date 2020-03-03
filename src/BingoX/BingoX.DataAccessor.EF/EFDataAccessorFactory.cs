using System;
using BingoX.Helper;
using BingoX.Domain;
using System.Linq;

namespace BingoX.DataAccessor.EF
{
    public class EFDataAccessorFactory<TContext> : IDataAccessorFactory where TContext : EfDbContext
    {
        private readonly IServiceProvider serviceProvider;
        private readonly DataAccessorBuilderInfo dataAccessorBuilderInfo;
        public EFDataAccessorFactory(IServiceProvider serviceProvider, DataAccessorBuilderInfo dataAccessorBuilderInfo)
        {
            this.serviceProvider = serviceProvider;
            this.dataAccessorBuilderInfo = dataAccessorBuilderInfo;
            dbcontext = CreateScopeDbContext();
            dbcontext.SetServiceProvider(serviceProvider);
        }
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public TContext dbcontext { get; set; }
        /// <summary>
        /// 连接字符串名称
        /// </summary>
        public string ConnectionName { get; protected set; }

        private TContext CreateScopeDbContext()
        {
            var constructor = typeof(TContext).GetConstructors().FirstOrDefault(n => n.GetParameters().Count() == 1);
            if (constructor == null) throw new DataAccessorException("找不到符合条件的EfDbContext构造函数，创建数据库上下文失败");
            var context = FastReflectionExtensions.FastInvoke(constructor, dataAccessorBuilderInfo.DbContextOption) as TContext;
            return context;
        }

        public IServiceProvider GetServiceProvider()
        {
            return serviceProvider;
        }

        void IDataAccessorFactory.AddIndluce<TEntity>(Func<TEntity, IQueryable<TEntity>> include)
        {
            throw new NotImplementedException();
        }
        public TDataAccessor Create<TEntity, TDataAccessor>()
            where TEntity : class, IEntity<TEntity>
            where TDataAccessor : IDataAccessor<TEntity>
        {
            throw new NotImplementedException();
        }

        public IDataAccessor<TEntity> CreateByEntity<TEntity>() where TEntity : class, IEntity<TEntity>
        {
            Type typeDataAccessor = null;
            if (typeof(TEntity).IsAssignableFrom(typeof(IGuidEntity<TEntity>)))
            {
                typeDataAccessor = typeof(EFGuidDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            else if (typeof(TEntity).IsAssignableFrom(typeof(IIdentityEntity<TEntity>)))
            {
                typeDataAccessor = typeof(EFIdentityDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            else if (typeof(TEntity).IsAssignableFrom(typeof(ISnowflakeEntity<TEntity>)))
            {
                typeDataAccessor = typeof(EFSnowflakeDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            else if (typeof(TEntity).IsAssignableFrom(typeof(IStringEntity<TEntity>)))
            {
                typeDataAccessor = typeof(EFStringIDDataAccessor<>).MakeGenericType(new Type[] { typeof(TEntity) });
            }
            if (typeDataAccessor == null) throw new DataAccessorException("不支持的TEntity泛型类型，构建DataAccessor失败。TEntity的类型必须派生于IGuidEntity、IIdentityEntity、ISnowflakeEntity、IStringEntity。");
            var constructor = typeDataAccessor.GetConstructors().FirstOrDefault(n => n.GetParameters().Count() == 1);
            if (constructor == null) throw new DataAccessorException("找不到符合条件的DataAccessor构造函数，创建数据库上下文失败");
            return FastReflectionExtensions.FastInvoke(constructor, dbcontext) as IDataAccessor<TEntity>;
        }
    }
}
