using System;
using BingoX.Helper;
using BingoX.Domain;
#if Standard
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif
using System.Linq;

namespace BingoX.DataAccessor.EF
{
    public class EFDataAccessorFactory<TContext> : IDataAccessorFactory where TContext : EfDbContext
    {
        private readonly IServiceProvider serviceProvider;
#if Standard
        private readonly DbContextOptions<TContext> dbContextOptions;

        public EFDataAccessorFactory(IServiceProvider serviceProvider, DbContextOptions<TContext> dbContextOptions)
        {
            this.dbContextOptions = dbContextOptions;
#else
        private readonly string connectionString;

        public EFDataAccessorFactory(IServiceProvider serviceProvider, string connectionString)
        {
            this.connectionString = connectionString;
#endif
            this.serviceProvider = serviceProvider;
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
#if Standard
            var context = FastReflectionExtensions.FastInvoke(constructor, dbContextOptions) as TContext;
#else
            var context = FastReflectionExtensions.FastInvoke(constructor, connectionString) as TContext;
#endif
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
        public IDataAccessor Create<TEntity, TDataAccessor>() where TDataAccessor : IDataAccessor where TEntity : class, IEntity<TEntity>
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
