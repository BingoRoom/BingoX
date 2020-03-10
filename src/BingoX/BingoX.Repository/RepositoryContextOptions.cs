using BingoX.DataAccessor;
using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Repository
{
    /// <summary>
    /// 仓储上下文选项
    /// </summary>
    public class RepositoryContextOptions
    {
        public RepositoryContextOptions(IRepositoryMapper mapper)
        {
            DataAccessorFactories = new Dictionary<string, IDataAccessorFactory>();
            Mapper = mapper;
        }
        public RepositoryContextOptions(IRepositoryMapper mapper,IList<IDbEntityIntercept> intercepts):this(mapper)
        {
            Intercepts = new InterceptCollection(intercepts);
        }
        /// <summary>
        /// 数据操作器工厂集合
        /// </summary>
        public IDictionary<string, IDataAccessorFactory> DataAccessorFactories { get; private set; }
        /// <summary>
        /// 映射器。用于在仓储中领域实体与数据库实体间的相互映射
        /// </summary>
        public IRepositoryMapper Mapper { get; private set; }
        public string DefaultConnectionName { get; set; }

        public virtual InterceptCollection Intercepts { get; internal set; }
        /// <summary>
        /// 添加关联查询委托
        /// </summary>
        /// <typeparam name="TEntity"></typeparam>
        /// <param name="setIndluce"></param>
        public void AddIndluce<TEntity>(Func<TEntity, IQueryable<TEntity>> setIndluce) where TEntity : class, IEntity<TEntity>
        {

        }
    }
}
