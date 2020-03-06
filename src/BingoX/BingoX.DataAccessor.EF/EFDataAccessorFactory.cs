using System;
using System.Collections;
using BingoX.Helper;
using BingoX.Domain;
using System.Linq;
using System.Collections.Generic;
using System.Reflection;

namespace BingoX.DataAccessor.EF
{
    public class EFDataAccessorFactory<TContext> : IDataAccessorFactory where TContext : EfDbContext
    {
        private readonly IServiceProvider serviceProvider;
        private readonly DataAccessorBuilderInfo dataAccessorBuilderInfo;
        private static readonly IDictionary<Type, ConstructorInfo> cache = new Dictionary<Type, ConstructorInfo>();
        public EFDataAccessorFactory(IServiceProvider serviceProvider, DataAccessorBuilderInfo dataAccessorBuilderInfo, string connectionString)
        {
            this.serviceProvider = serviceProvider;
            this.dataAccessorBuilderInfo = dataAccessorBuilderInfo;
            ConnectionString = connectionString;
            dbcontext = CreateDbContext();
           
        }
        /// <summary>
        /// 数据库上下文
        /// </summary>
        public TContext dbcontext { get;private set; }
        /// <summary>
        /// 连接字符串名称
        /// </summary>
        public string ConnectionString { get; protected set; }

        private TContext CreateDbContext()
        {
            var constructor = typeof(TContext).GetConstructors().FirstOrDefault(n => n.GetParameters().Count() == 1);
            if (constructor == null) throw new DataAccessorException("找不到符合条件的EfDbContext构造函数，创建数据库上下文失败");
            var context = FastReflectionExtensions.FastInvoke(constructor, dataAccessorBuilderInfo.DbContextOption) as TContext;
            context.SetServiceProvider(serviceProvider);
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
            where TEntity :  IEntity<TEntity>
            where TDataAccessor : IDataAccessor<TEntity>
        {
            var resultType = typeof(TDataAccessor);
            if (cache.ContainsKey(resultType)) return FastReflectionExtensions.FastInvoke<TDataAccessor>(cache[resultType], dbcontext);
            if (resultType.Equals(typeof(IDataAccessor<TEntity>)) || !resultType.IsInterface) throw new DataAccessorException($"{nameof(TDataAccessor)}必须为{nameof(IDataAccessor<TEntity>)}的派生接口");
            var types = dataAccessorBuilderInfo.DataAccessorAssembly.GetImplementedClass<TDataAccessor>();
            if (types.Length == 0) throw new DataAccessorException($"找不到接口{nameof(TDataAccessor)}的实现类");
            var implClass = types[0];
            var constructor = implClass.GetConstructors().FirstOrDefault(n => n.GetParameters().Count() == 1);
            if(constructor == null) throw new DataAccessorException($"找不到接口{nameof(TDataAccessor)}的实现类适合的构造器");
            cache.Add(resultType, constructor);
            return FastReflectionExtensions.FastInvoke<TDataAccessor>(constructor, dbcontext);
        }

        public IDataAccessor<TEntity> CreateByEntity<TEntity>() where TEntity :  IEntity<TEntity>
        {
            var resultType = typeof(TEntity);
            if (cache.ContainsKey(resultType)) return FastReflectionExtensions.FastInvoke<IDataAccessor<TEntity>>(cache[resultType], dbcontext);
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
            if (typeDataAccessor == null) throw new DataAccessorException("不支持的TEntity泛型类型，构建DataAccessor失败。TEntity的类型必须派生于IGuidEntity、IIdentityEntity、ISnowflakeEntity、IStringEntity。");
            var constructor = typeDataAccessor.GetConstructors().FirstOrDefault(n => n.GetParameters().Count() == 1);
            if (constructor == null) throw new DataAccessorException("找不到符合条件的DataAccessor构造函数，创建数据库上下文失败");
            cache.Add(resultType, constructor);
            return FastReflectionExtensions.FastInvoke(constructor, dbcontext) as IDataAccessor<TEntity>;
        }
    }
}
