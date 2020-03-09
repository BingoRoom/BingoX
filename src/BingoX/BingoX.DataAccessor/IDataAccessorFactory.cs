using BingoX.Domain;
using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Reflection;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个数据操作器的构造器
    /// </summary>
    public interface IDataAccessorFactory
    {
        /// <summary>
        /// 连接字符串名称
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// 创建一个数据访问器
        /// </summary>
        /// <typeparam name="TDataAccessor">派生于IDataAccessor<TEntity>的接口</typeparam>
        /// <returns></returns>
        TDataAccessor Create<TEntity, TDataAccessor>() where TDataAccessor : IDataAccessor<TEntity> where TEntity : IEntity<TEntity>;
        /// <summary>
        /// 创建一个指定DAO的数据访问器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体</typeparam>
        /// <returns></returns>
        IDataAccessor<TEntity> CreateByEntity<TEntity>() where TEntity : IEntity<TEntity>;
        /// <summary>
        /// 添加关联查询
        /// </summary>
        /// <typeparam name="TEntity">数据库实体</typeparam>
        /// <param name="include"></param>
   //     void AddIndluce<TEntity>(Func<TEntity, IQueryable<TEntity>> include) where TEntity :  IEntity<TEntity>;

        /// <summary>
        /// 创建SQL命令门面
        /// </summary>
        /// <returns></returns>
        ISqlFacade CreateSqlFacade();
        /// <summary>
        /// 创建 关联查询 门面
        /// </summary>
        /// <returns></returns>
        IJoinFacade CreateJoinFacade();
        /// <summary>
        /// 获取DI集合
        /// </summary>
        /// <returns></returns>
        IServiceProvider GetServiceProvider();
    }

    public abstract class DataAccessorFactory<TContext> : IDataAccessorFactory where TContext : class, IDbContext
    {
        protected readonly IServiceProvider serviceProvider;
        protected readonly DataAccessorBuilderInfo dataAccessorBuilderInfo;
        protected static readonly IDictionary<Type, ConstructorInfo> cache = new Dictionary<Type, ConstructorInfo>();
        public DataAccessorFactory(IServiceProvider serviceProvider, DataAccessorBuilderInfo dataAccessorBuilderInfo, string connectionString)
        {
            this.serviceProvider = serviceProvider;
            this.dataAccessorBuilderInfo = dataAccessorBuilderInfo;
            ConnectionString = connectionString;
            DbContext = CreateDbContext();

        }


        /// <summary>
        /// 数据库上下文
        /// </summary>
        public TContext DbContext { get; private set; }
        /// <summary>
        /// 连接字符串名称
        /// </summary>
        public string ConnectionString { get; protected set; }

        protected virtual TContext CreateDbContext()
        {
            var constructor = typeof(TContext).GetConstructors().FirstOrDefault(n => n.GetParameters().Count() == 1);
            if (constructor == null) throw new DataAccessorException("找不到符合条件的SqlSugarDbContext构造函数，创建数据库上下文失败");
            var context = FastReflectionExtensions.FastInvoke(constructor, dataAccessorBuilderInfo.DbContextOption) as TContext;
            context.SetServiceProvider(serviceProvider);
            return context;
        }
        protected abstract Type GetDataAccessorType<TEntity>() where TEntity : IEntity<TEntity>;

        public virtual IDataAccessor<TEntity> CreateByEntity<TEntity>() where TEntity : IEntity<TEntity>
        {
            var resultType = typeof(TEntity);
            if (cache.ContainsKey(resultType)) return FastReflectionExtensions.FastInvoke<IDataAccessor<TEntity>>(cache[resultType], DbContext);
            Type typeDataAccessor = GetDataAccessorType<TEntity>();
            if (typeDataAccessor == null) throw new DataAccessorException($"找不到接口IDataAccessor<{ nameof(TEntity) }>的实现类");
            var constructor = typeDataAccessor.GetConstructors().FirstOrDefault(n => n.GetParameters().Count() == 1);
            if (constructor == null) throw new DataAccessorException("找不到符合条件的DataAccessor构造函数，创建数据库上下文失败");
            cache.Add(resultType, constructor);
            return FastReflectionExtensions.FastInvoke(constructor, DbContext) as IDataAccessor<TEntity>;
        }
        public virtual TDataAccessor Create<TEntity, TDataAccessor>()
           where TEntity : IEntity<TEntity>
           where TDataAccessor : IDataAccessor<TEntity>
        {
            var resultType = typeof(TDataAccessor);
            if (cache.ContainsKey(resultType)) return FastReflectionExtensions.FastInvoke<TDataAccessor>(cache[resultType], DbContext);
            var typeDataAccessor = GetDataAccessorType<TEntity>();

            if (resultType.Equals(typeof(IDataAccessor<TEntity>))) throw new DataAccessorException($"{nameof(TDataAccessor)}必须为{nameof(IDataAccessor<TEntity>)}的派生接口");
            var types = dataAccessorBuilderInfo.DataAccessorAssembly.GetImplementedClass<TDataAccessor>();

            Type implClass = null;
            if (types.Length == 0)
            {
                implClass = GetDataAccessorType<TEntity>();
            }
            else
            {
                implClass = types[0];
            }
            if (implClass == null) throw new DataAccessorException($"找不到接口{nameof(TDataAccessor)}的实现类");
            var constructor = implClass.GetConstructors().FirstOrDefault(n => n.GetParameters().Count() == 1);
            if (constructor == null) throw new DataAccessorException($"找不到接口{nameof(TDataAccessor)}的实现类适合的构造器");
            cache.Add(resultType, constructor);
            return FastReflectionExtensions.FastInvoke<TDataAccessor>(constructor, DbContext);
        }

        public IServiceProvider GetServiceProvider()
        {
            return serviceProvider;
        }

        ISqlFacade IDataAccessorFactory.CreateSqlFacade()
        {
            throw new NotImplementedException();
        }
        IJoinFacade IDataAccessorFactory.CreateJoinFacade()
        {
            throw new NotImplementedException();
        }
    }
}
