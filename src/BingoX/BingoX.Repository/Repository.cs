using BingoX.DataAccessor;
using BingoX.Domain;
using BingoX.Helper;
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
        public virtual RepositoryUnitOfWork UnitOfWork { get; private set; }
        protected internal IRepositoryMapper Mapper { get; set; }

        IRepositoryUnitOfWork IRepository.UnitOfWork => UnitOfWork;


        /// <summary>
        /// 创建数据访问器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体类型</typeparam>
        /// <typeparam name="TDataAccessor">数据访问器的派生接口类型</typeparam>
        /// <param name="dbName">连接字符串名称</param>
        /// <returns></returns>
        protected TDataAccessor CreateWrapper<TEntity, TDataAccessor>(string dbName = null)
            where TEntity : class, IEntity<TEntity>
            where TDataAccessor : IDataAccessor<TEntity>
        {
            IDataAccessorFactory factory = GetFactory(dbName);
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
        protected IDataAccessor<TEntity> CreateWrapper<TEntity>(string dbName = null) where TEntity : IEntity<TEntity>
        {
            IDataAccessorFactory factory = GetFactory(dbName);
            var dataAccessor = factory.CreateByEntity<TEntity>();
            UnitOfWork.Add(dataAccessor.UnitOfWork);
            return dataAccessor;
        }

        /// <summary>
        /// 创建Sql访问器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体类型</typeparam>
        /// <param name="dbName">连接字符串名称</param>
        /// <returns></returns>
        protected ISqlFacade CreateSqlFacade(string dbName = null)
        {
            IDataAccessorFactory factory = GetFactory(dbName);
            var sqlFacade = factory.CreateSqlFacade();

            return sqlFacade;
        }


        private IDataAccessorFactory GetFactory(string dbName)
        {
            if (options.DataAccessorFactories == null || options.DataAccessorFactories.Count == 0) throw new RepositoryException("DataAccessorFactory集合为空");
            var factory = string.IsNullOrEmpty(dbName) ? options.DataAccessorFactories.First().Value : options.DataAccessorFactories.GetValue(dbName);
            if (factory == null) throw new RepositoryException("DataAccessorFactory集合为空");
            return factory;
        }
        /// <summary>
        /// 提交事务
        /// </summary>
        public void Commit()
        {
            this.UnitOfWork.Commit();
        }
        /// <summary>
        /// 回滚事务
        /// </summary>
        public void Rollback()
        {
            this.UnitOfWork.Rollback();
        }
    }

    public class Repository<TDomain, TEntity> : Repository, IRepository<TDomain, TEntity>   
        where TEntity : IEntity<TEntity>
    {

        protected virtual IDataAccessor<TEntity> Wrapper { get; private set; }

        public Repository(RepositoryContextOptions options) : base(options)
        {
            if (options.DataAccessorFactories.Count == 1) Wrapper = CreateWrapper<TEntity>();
            else if (!string.IsNullOrEmpty(options.DefaultConnectionName)) Wrapper = CreateWrapper<TEntity>(options.DefaultConnectionName);
        }
        public Repository(RepositoryContextOptions options, string dbname) : base(options)
        {
            Wrapper = CreateWrapper<TEntity>(dbname);

        }
        private void Check()
        {
            if (Wrapper == null) throw new RepositoryException("数据访问器不存在");
            if (!typeof(TEntity).Equals(typeof(TDomain)) && Mapper == null) throw new RepositoryException("当TDomain与TEntity类型不同时Mapper不能为空");
        }

        public virtual void Add(TDomain domain)
        {
            Check();
            var entity = this.ProjectTo<TEntity>(domain);
            Wrapper.Add(entity);
        }

        public virtual void AddRange(IEnumerable<TDomain> domains)
        {
            Check();
            var entities = this.ProjectToList<TDomain, TEntity>(domains);
            Wrapper.AddRange(entities);
        }

        public virtual void Delete(object[] pkArray)
        {
            Check();
            Wrapper.Delete(pkArray);
        }

        public virtual void Delete(TDomain domain)
        {
            Check();
            var entity = this.ProjectTo<TEntity>(domain);
            Wrapper.Delete(entity);
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

        public virtual void Update(TDomain domain)
        {
            Check();
            var entity = this.ProjectTo<TEntity>(domain);
            Wrapper.Update(entity);
        }

        public virtual void UpdateRange(IEnumerable<TDomain> domains)
        {
            Check();
            var entities = this.ProjectToList<TDomain, TEntity>(domains);
            Wrapper.UpdateRange(entities);
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



        public virtual void Delete(Expression<Func<TEntity, bool>> whereLambda)
        {
            Check();
            Wrapper.Delete(whereLambda);
        }

        public virtual IList<TDomain> Take(Expression<Func<TEntity, bool>> whereLambda, int num)
        {
            Check();
            var domainEntities = Wrapper.Take(whereLambda, num);
            return this.ProjectToList<TEntity, TDomain>(domainEntities);
        }

        public TDomain Get(Expression<Func<TEntity, bool>> whereLambda)
        {
            Check();
            return Where(whereLambda).FirstOrDefault();
        }

       
    }

    public class Repository<TDomain> : Repository<TDomain, TDomain>, IRepository<TDomain>
        where TDomain : class, IEntity<TDomain>
    {
        public Repository(RepositoryContextOptions options) : base(options)
        {

        }
        public Repository(RepositoryContextOptions options, string dbname) : base(options, dbname)
        {
           
        }

      
        


    }
}