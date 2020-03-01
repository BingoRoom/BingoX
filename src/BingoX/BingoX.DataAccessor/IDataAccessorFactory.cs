using BingoX.Domain;
using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个数据操作器的构造器
    /// </summary>
    public interface IDataAccessorFactory
    {
        /// <summary>
        /// 获取数据操作器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体</typeparam>
        /// <returns></returns>
        IDataAccessor<TEntity> Create<TEntity>() where TEntity : class, IEntity<TEntity>;
        /// <summary>
        /// 添加关联查询
        /// </summary>
        /// <typeparam name="TEntity">数据库实体</typeparam>
        /// <param name="include"></param>
        void AddIndluce<TEntity>(Func<TEntity, IQueryable<TEntity>> include) where TEntity : class, IEntity<TEntity>;
        /// <summary>
        /// 获取DI集合
        /// </summary>
        /// <returns></returns>
        IServiceProvider GetServiceProvider();
    }
}
