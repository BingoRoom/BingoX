using BingoX.DataAccessor;
using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BingoX.Repository
{
    public abstract class Repository : IRepository
    {
        private readonly RepositoryContextOptions options;

        public Repository(RepositoryContextOptions options)
        {
            this.options = options;
            Mapper = options.Mapper;
            UnitOfWork = new RepositoryUnitOfWork();
        }
        public virtual RepositoryUnitOfWork UnitOfWork { get; protected set; }
        protected internal IRepositoryMapper Mapper { get; set; }

        IUnitOfWork IRepository.UnitOfWork { get { return UnitOfWork; } }
        /// <summary>
        /// 创建数据访问器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体类型</typeparam>
        /// <typeparam name="TDataAccessor">数据访问器的派生接口类型</typeparam>
        /// <param name="dbName">连接字符串名称</param>
        /// <returns></returns>
        public TDataAccessor CreateWrapper<TEntity, TDataAccessor>(string dbName = null)
            where TEntity : class, IEntity<TEntity>
            where TDataAccessor : IDataAccessor<TEntity>
        {
            if (options.DataAccessorFactories == null || options.DataAccessorFactories.Count == 0) throw new RepositoryException("DataAccessorFactory集合为空");
            var factory = string.IsNullOrEmpty(dbName) ? options.DataAccessorFactories.First().Value : options.DataAccessorFactories[dbName];
            if (factory == null) throw new RepositoryException("DataAccessorFactory集合为空");
            var dataAccessor = factory.Create<TEntity, TDataAccessor>();
            UnitOfWork.Add(dataAccessor.UnitOfWork);
            return dataAccessor;
        }
        /// <summary>
        /// 创建数据访问器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体类型</typeparam>
        /// <param name="dbName">连接字符串名称</param>
        /// <returns></returns>
        public IDataAccessor<TEntity> CreateWrapper<TEntity>(string dbName = null) where TEntity : IEntity<TEntity>
        {
            if (options.DataAccessorFactories == null || options.DataAccessorFactories.Count == 0) throw new RepositoryException("DataAccessorFactory集合为空");
            var factory = string.IsNullOrEmpty(dbName) ? options.DataAccessorFactories.First().Value : options.DataAccessorFactories[dbName];
            if (factory == null) throw new RepositoryException("DataAccessorFactory集合为空");
            var dataAccessor = factory.CreateByEntity<TEntity>();
            UnitOfWork.Add(dataAccessor.UnitOfWork);
            return dataAccessor;
        }

        /// <summary>
        /// 创建数据访问器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体类型</typeparam>
        /// <param name="dbName">连接字符串名称</param>
        /// <returns></returns>
        public ISqlFacade CreateSqlFacade(string dbName = null) 
        {
            if (options.DataAccessorFactories == null || options.DataAccessorFactories.Count == 0) throw new RepositoryException("DataAccessorFactory集合为空");
            var factory = string.IsNullOrEmpty(dbName) ? options.DataAccessorFactories.First().Value : options.DataAccessorFactories[dbName];
            if (factory == null) throw new RepositoryException("DataAccessorFactory集合为空");
            var sqlFacade = factory.CreateSqlFacade();
           
            return sqlFacade;
        }
    }

    public class Repository<TDomain, TEntity> : Repository, IRepository<TDomain, TEntity> where TEntity : IEntity<TEntity>
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

        public virtual IList<TDomain> Where(Expression<Func<TEntity, bool>> whereLambda)
        {
            Check();
            var domainEntities = Wrapper.Where(whereLambda);
            return this.ProjectToList<TEntity, TDomain>(domainEntities);
        }

        public virtual bool Exist(Expression<Func<TEntity, bool>> whereLambda)
        {
            Check();
            return Wrapper.Exist(whereLambda);
        }

        public virtual int Update(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        public virtual int Delete(Expression<Func<TEntity, bool>> whereLambda)
        {
            Check();
            return Wrapper.Delete(whereLambda);
        }

        public virtual IList<TDomain> Take(Expression<Func<TEntity, bool>> whereLambda, int num)
        {
            Check();
            var domainEntities = Wrapper.Take(whereLambda, num);
            return this.ProjectToList<TEntity, TDomain>(domainEntities);
        }
    }

    public class Repository<TDomain> : Repository<TDomain, TDomain>, IRepository<TDomain> where TDomain : class, IEntity<TDomain>
    {
        public Repository(RepositoryContextOptions options) : base(options)
        {

        }
    }
}