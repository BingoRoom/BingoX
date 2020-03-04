using BingoX.DataAccessor;
using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace BingoX.Repository
{
    /// <summary>
    /// 表示一个领域仓储
    /// </summary>
    public interface IRepository
    {
        /// <summary>
        /// 事务单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
    /// <summary>
    /// 表示一个领域实体与数据库实体为同一个实体类的领域仓储
    /// </summary>
    /// <typeparam name="TDomain">领域实体</typeparam>
    /// <typeparam name="pkType">主键类型</typeparam>
    public interface IRepository<TDomain, pkType> : IRepository<TDomain, TDomain, pkType> 
        where TDomain : class, IEntity<TDomain, pkType>
    {
        
    }
    /// <summary>
    /// 表示一个领域实体与数据库实体不为同一个实体类的领域仓储
    /// </summary>
    /// <typeparam name="TDomain">领域实体</typeparam>
    /// <typeparam name="Entity">数据库实体（DAO）</typeparam>
    /// <typeparam name="pkType">主键类型</typeparam>
    public interface IRepository<TDomain, TEntity, pkType> : IRepository 
        where TEntity : class, IEntity<TEntity, pkType> 
        where TDomain : class
    {
        /// <summary>
        /// 批量新增记录
        /// </summary>
        /// <param name="entites"待新增记录集合</param>
        /// <returns>受影响记录数</returns>
        int AddRange(IEnumerable<TDomain> entites);
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="entity">待新增记录实体</param>
        /// <returns>受影响记录数</returns>
        int Add(TDomain entity);
        /// <summary>
        /// 根据主键查询记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>查询结果</returns>
        TDomain GetId(pkType id);
        /// <summary>
        /// 根据主键判断记录是否存在
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>是否存在</returns>
        bool Exist(pkType id);
        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <returns>查询结果集合</returns>
        IList<TDomain> QueryAll();
        /// <summary>
        /// 分页查询记录
        /// </summary>
        /// <param name="specification">分页规格</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        IList<TDomain> PageList(ISpecification<TEntity> specification, ref int total);
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity">待更新记录实体</param>
        /// <returns>受影响记录数</returns>
        int Update(TDomain entity);
        /// <summary>
        /// 批量更新记录
        /// </summary>
        /// <param name="entitys">待更新记录集合</param>
        /// <returns>受影响记录数</returns>
        int UpdateRange(IEnumerable<TDomain> entitys);
        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="pkArray">待删除记录主键集合</param>
        /// <returns>受影响记录数</returns>
        int Delete(pkType[] pkArray);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity">待删除记录实体</param>
        /// <returns>受影响记录数</returns>
        int Delete(TDomain entity);
    }

    public abstract class Repository : IRepository
    {
        private readonly RepositoryContextOptions options;

        public IUnitOfWork UnitOfWork { get; protected set; }

        public Repository(RepositoryContextOptions options)
        {
            this.options = options;
        }
        /// <summary>
        /// 创建数据访问器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体类型</typeparam>
        /// <typeparam name="TDataAccessor">数据访问器的派生接口类型</typeparam>
        /// <param name="connName">连接字符串名称</param>
        /// <returns></returns>
        public TDataAccessor CreateWrapper<TEntity, TDataAccessor>(string connName = null) 
            where TEntity : class, IEntity<TEntity>
            where TDataAccessor : IDataAccessor<TEntity>
        {
            if (options.DataAccessorFactories == null || options.DataAccessorFactories.Count == 0) throw new RepositoryException("DataAccessorFactory集合为空");
            var factory = string.IsNullOrEmpty(connName) ? options.DataAccessorFactories.First().Value : options.DataAccessorFactories[connName];
            if (factory == null) throw new RepositoryException("DataAccessorFactory集合为空");
            return factory.Create<TEntity, TDataAccessor>();
        }
        /// <summary>
        /// 创建数据访问器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体类型</typeparam>
        /// <param name="connName">连接字符串名称</param>
        /// <returns></returns>
        public IDataAccessor<TEntity> CreateWrapper<TEntity>(string connName = null) where TEntity : class, IEntity<TEntity>
        {
            if (options.DataAccessorFactories == null || options.DataAccessorFactories.Count == 0) throw new RepositoryException("DataAccessorFactory集合为空");
            var factory = string.IsNullOrEmpty(connName) ? options.DataAccessorFactories.First().Value : options.DataAccessorFactories[connName];
            if (factory == null) throw new RepositoryException("DataAccessorFactory集合为空");
            return factory.CreateByEntity<TEntity>();
        }
    }

    public abstract class Repository<TDomain, TEntity, pkType> : Repository, IRepository<TDomain, TEntity, pkType> 
        where TEntity : class, IEntity<TEntity, pkType>
        where TDomain : class
    {
        public IDataAccessor<TEntity> Wrapper { get; protected set; }

        public IRepositoryMapper Mapper { get; }

        public Repository(RepositoryContextOptions options) : base(options)
        {
            if (options.DataAccessorFactories.Count == 1) Wrapper = CreateWrapper<TEntity>();
            Mapper = options.Mapper;

        }

        private void Check()
        {
            if (Wrapper == null) throw new RepositoryException("数据访问器不存在");
            if (UnitOfWork == null) UnitOfWork = Wrapper.UnitOfWork;
            if (!typeof(TEntity).IsAssignableFrom(typeof(TDomain)) && Mapper == null) throw new RepositoryException("当TDomain与TEntity类型不同时Mapper不能为空");
        }

        public virtual int Add(TDomain entity)
        {
            Check();
            if (typeof(TEntity).IsAssignableFrom(typeof(TDomain))) return Wrapper.Add(entity as TEntity);
            return Wrapper.Add(Mapper.ProjectTo<TDomain, TEntity>(entity));
        }

        public virtual int AddRange(IEnumerable<TDomain> entities)
        {
            Check();
            if (typeof(TEntity).IsAssignableFrom(typeof(TDomain))) return Wrapper.AddRange(entities.Select(n => n as TEntity));
            return Wrapper.AddRange(Mapper.ProjectToList<TDomain, TEntity>(entities));
        }

        public virtual int Delete(pkType[] pkArray)
        {
            Check();
            return Wrapper.Delete(pkArray.Select(n => n as object).ToArray());
        }

        public virtual int Delete(TDomain entity)
        {
            Check();
            if (typeof(TEntity).IsAssignableFrom(typeof(TDomain))) return Wrapper.Delete(entity as TEntity);
            return Wrapper.Delete(Mapper.ProjectTo<TDomain, TEntity>(entity));
        }

        public virtual bool Exist(pkType id)
        {
            Check();
            return Wrapper.Exist(id as object);
        }

        public virtual TDomain GetId(pkType id)
        {
            Check();
            if (typeof(TEntity).IsAssignableFrom(typeof(TDomain))) return Wrapper.GetId(id as object) as TDomain;
            return Mapper.ProjectTo<TEntity, TDomain>(Wrapper.GetId(id as object));
        }

        public virtual IList<TDomain> QueryAll()
        {
            Check();
            if (typeof(TEntity).IsAssignableFrom(typeof(TDomain))) return Wrapper.QueryAll().Select(n => n as TDomain).ToList();
            return Mapper.ProjectToList<TEntity, TDomain>(Wrapper.QueryAll()).ToList();
        }

        public IList<TDomain> PageList(ISpecification<TEntity> specification, ref int total)
        {
            Check();
            if (typeof(TEntity).IsAssignableFrom(typeof(TDomain))) return Wrapper.PageList(specification, ref total).Select(n => n as TDomain).ToList();
            return Mapper.ProjectToList<TEntity, TDomain>(Wrapper.PageList(specification, ref total)).ToList();
        }

        public virtual int Update(TDomain entity)
        {
            Check();
            if (typeof(TEntity).IsAssignableFrom(typeof(TDomain))) return Wrapper.Update(entity as TEntity);
            return Wrapper.Update(Mapper.ProjectTo<TDomain, TEntity>(entity));
        }

        public virtual int UpdateRange(IEnumerable<TDomain> entities)
        {
            Check();
            if (typeof(TEntity).IsAssignableFrom(typeof(TDomain))) return Wrapper.UpdateRange(entities.Select(n => n as TEntity));
            return Wrapper.UpdateRange(Mapper.ProjectToList<TDomain, TEntity>(entities));
        }
    }

    public abstract class Repository<TDomain, pkType> : Repository<TDomain, TDomain, pkType>, IRepository<TDomain, pkType> where TDomain : class, IEntity<TDomain, pkType>
    {
        public Repository(RepositoryContextOptions options) : base(options)
        {

        }
    }
}