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
        /// 连接字符串名称
        /// </summary>
        string ConnectionString { get; }
        /// <summary>
        /// 创建一个数据访问器
        /// </summary>
        /// <typeparam name="TDataAccessor">派生于IDataAccessor<TEntity>的接口</typeparam>
        /// <returns></returns>
        TDataAccessor Create<TEntity, TDataAccessor>() where TDataAccessor : IDataAccessor<TEntity> where TEntity :   IEntity<TEntity>;
        /// <summary>
        /// 创建一个指定DAO的数据访问器
        /// </summary>
        /// <typeparam name="TEntity">数据库实体</typeparam>
        /// <returns></returns>
        IDataAccessor<TEntity> CreateByEntity<TEntity>() where TEntity :   IEntity<TEntity>;
        /// <summary>
        /// 添加关联查询
        /// </summary>
        /// <typeparam name="TEntity">数据库实体</typeparam>
        /// <param name="include"></param>
        void AddIndluce<TEntity>(Func<TEntity, IQueryable<TEntity>> include) where TEntity :  IEntity<TEntity>;

        /// <summary>
        /// 创建SQL命令门面
        /// </summary>
        /// <returns></returns>
        ISqlFacade CreateSqlFacade();
        /// <summary>
        /// 获取DI集合
        /// </summary>
        /// <returns></returns>
        IServiceProvider GetServiceProvider();
    }
}
