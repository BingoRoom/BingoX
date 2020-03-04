using BingoX.DataAccessor;
using BingoX.Domain;
using BingoX.Helper;
using System;
using System.Collections;
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
    public interface IRepository<TDomain> : IRepository<TDomain, TDomain>
        where TDomain : IEntity<TDomain>
    {

    }
    /// <summary>
    /// 表示一个领域实体与数据库实体不为同一个实体类的领域仓储
    /// </summary>
    /// <typeparam name="TDomain">领域实体</typeparam>
    /// <typeparam name="Entity">数据库实体（DAO）</typeparam>
    /// <typeparam name="pkType">主键类型</typeparam>
    public interface IRepository<TDomain, TEntity> : IRepository
        where TEntity : IEntity<TEntity>

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
        TDomain GetId(object id);
        /// <summary>
        /// 根据主键判断记录是否存在
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>是否存在</returns>
        bool Exist(object id);
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
        int Delete(object[] pkArray);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity">待删除记录实体</param>
        /// <returns>受影响记录数</returns>
        int Delete(TDomain entity);
    }
    public sealed class UnitOfWork : IUnitOfWork
    {
        readonly IList<IUnitOfWork> units = new List<IUnitOfWork>();
        public void Commit()
        {
            try
            {
                foreach (var item in units)
                {
                    item.Commit();
                }
            }
            catch (Exception ex)
            {
                Rollback();
                throw new DataAccessorException("提交事务失败", ex);
            }

        }

        public void Rollback()
        {
            try
            {
                foreach (var item in units)
                {
                    item.Rollback();
                }
            }
            catch (Exception ex)
            {

                throw new DataAccessorException("回滚事务失败", ex);
            }

        }

        internal void Add(IUnitOfWork unitOfWork)
        {
            if (units.Contains(unitOfWork)) return;
            units.Add(unitOfWork);
        }

        internal void Reomve(IUnitOfWork unitOfWork)
        {
            units.Remove(unitOfWork);
        }
    }
    public abstract class Repository : IRepository
    {
        private readonly RepositoryContextOptions options;


        public Repository(RepositoryContextOptions options)
        {
            this.options = options;
            Mapper = options.Mapper;
            UnitOfWork = new UnitOfWork();
        }
        public virtual UnitOfWork UnitOfWork { get; protected set; }
        protected internal IRepositoryMapper Mapper { get; set; }

        IUnitOfWork IRepository.UnitOfWork { get { return UnitOfWork; } }

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
            var dataAccessor = factory.Create<TEntity, TDataAccessor>();
            UnitOfWork.Add(dataAccessor.UnitOfWork);
            return dataAccessor;
        }
        /// <summary>
        /// 创建数据访问器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体类型</typeparam>
        /// <param name="connName">连接字符串名称</param>
        /// <returns></returns>
        public IDataAccessor<TEntity> CreateWrapper<TEntity>(string connName = null) where TEntity : IEntity<TEntity>
        {
            if (options.DataAccessorFactories == null || options.DataAccessorFactories.Count == 0) throw new RepositoryException("DataAccessorFactory集合为空");
            var factory = string.IsNullOrEmpty(connName) ? options.DataAccessorFactories.First().Value : options.DataAccessorFactories[connName];
            if (factory == null) throw new RepositoryException("DataAccessorFactory集合为空");
            var dataAccessor = factory.CreateByEntity<TEntity>();
            UnitOfWork.Add(dataAccessor.UnitOfWork);
            return dataAccessor;
        }
    }

    public class Repository<TDomain, TEntity> : Repository, IRepository<TDomain, TEntity>
        where TEntity : IEntity<TEntity>

    {

        protected virtual IDataAccessor<TEntity> Wrapper { get; private set; }



        public Repository(RepositoryContextOptions options) : base(options)
        {
            if (options.DataAccessorFactories.Count == 1) Wrapper = CreateWrapper<TEntity>();


        }

        private void Check()
        {
            if (Wrapper == null) throw new RepositoryException("数据访问器不存在");
            if (!typeof(TEntity).Equals(typeof(TDomain)) && Mapper == null) throw new RepositoryException("当TDomain与TEntity类型不同时Mapper不能为空");
        }

        public virtual int Add(TDomain domain)
        {
            Check();
            var entity = this.ProjectTo<TEntity>(domain);
            return Wrapper.Add(entity);

        }

        public virtual int AddRange(IEnumerable<TDomain> domains)
        {
            Check();
            var entities = this.ProjectToList<TDomain, TEntity>(domains);
            return Wrapper.AddRange(entities);

        }

        public virtual int Delete(object[] pkArray)
        {
            Check();
            return Wrapper.Delete(pkArray);
        }

        public virtual int Delete(TDomain domain)
        {
            Check();
            var entity = this.ProjectTo<TEntity>(domain);
            return Wrapper.Delete(entity);

        }

        public virtual bool Exist(object id)
        {
            Check();
            return Wrapper.Exist(id);
        }

        public virtual TDomain GetId(object id)
        {
            Check();
            return this.ProjectTo<TDomain>(Wrapper.GetId(id));

        }

        public virtual IList<TDomain> QueryAll()
        {
            Check();
            var list = Wrapper.QueryAll();
            return this.ProjectToList<TEntity, TDomain>(list);

        }

        public virtual IList<TDomain> PageList(ISpecification<TEntity> specification, ref int total)
        {
            Check();
            var list = Wrapper.PageList(specification, ref total);
            return this.ProjectToList<TEntity, TDomain>(list);

        }

        public virtual int Update(TDomain domain)
        {
            Check();
            var entity = this.ProjectTo<TEntity>(domain);
            return Wrapper.Update(entity);
        }

        public virtual int UpdateRange(IEnumerable<TDomain> domains)
        {
            Check();
            var entities = this.ProjectToList<TDomain, TEntity>(domains);
            return Wrapper.UpdateRange(entities);

        }
    }
    public class Repository<TDomain> : Repository<TDomain, TDomain>, IRepository<TDomain> where TDomain : class, IEntity<TDomain>
    {
        public Repository(RepositoryContextOptions options) : base(options)
        {

        }
    }
    static class RepositoryExtensions
    {

        public static IList<TDestination> ProjectToList<TSource, TDestination>(this Repository repository, IEnumerable<TSource> sources)
        {

            if (sources.IsEmpty()) return BingoX.Utility.EmptyUtility<TDestination>.EmptyList;
            if (typeof(TSource).Equals(typeof(TDestination))) return sources.OfType<TDestination>().ToArray();
            return repository.Mapper.ProjectToList<TSource, TDestination>(sources);
        }
        public static TDestination ProjectTo<TDestination>(this Repository repository, object source)

        {
            if (source == null) return default(TDestination);
            if (source.GetType().Equals(typeof(TDestination))) return (TDestination)source;
            return repository.Mapper.ProjectTo<TDestination>(source);
        }

    }
}