using BingoX.ComponentModel;
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
        IRepositoryUnitOfWork UnitOfWork { get; }
    }
    /// <summary>
    /// 表示一个领域实体与数据库实体为同一个实体类的领域仓储
    /// </summary>
    /// <typeparam name="TDomain">领域实体</typeparam>
    /// <typeparam name="pkType">主键类型</typeparam>
    public interface IRepository<TDomain> : IRepository<TDomain, TDomain>
        where TDomain : IDomainEntry, IEntity<TDomain>
    {
        //   void SetDb(string dbName);
    }
    /// <summary>
    /// 表示一个领域实体与数据库实体不为同一个实体类的领域仓储
    /// </summary>
    /// <typeparam name="TDomain">领域实体</typeparam>
    /// <typeparam name="Entity">数据库实体（DAO）</typeparam>
    /// <typeparam name="pkType">主键类型</typeparam>
    public interface IRepository<TDomain, TEntity> : IRepository
        where TDomain : IDomainEntry
        where TEntity : IEntity<TEntity>

    {
        /// <summary>
        /// 批量新增记录
        /// </summary>
        /// <param name="entites"待新增记录集合</param>
        /// <returns>受影响记录数</returns>
        void AddRange(IEnumerable<TDomain> entites);
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="entity">待新增记录实体</param>
        /// <returns>受影响记录数</returns>
        void Add(TDomain entity);
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
        void Update(TDomain entity);
        /// <summary>
        /// 批量更新记录
        /// </summary>
        /// <param name="entitys">待更新记录集合</param>
        /// <returns>受影响记录数</returns>
        void UpdateRange(IEnumerable<TDomain> entitys);
        /// <summary>
        /// 批量删除记录
        /// </summary>
        /// <param name="pkArray">待删除记录主键集合</param>
        /// <returns>受影响记录数</returns>
        void Delete(object[] pkArray);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="entity">待删除记录实体</param>
        /// <returns>受影响记录数</returns>
        void Delete(TDomain entity);
        /// <summary>
        /// 根据条件查询记录
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        IList<TDomain> Where(Expression<Func<TEntity, bool>> whereLambda);
        /// <summary>
        /// 根据条件判断记录是否存在
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        bool Exist(Expression<Func<TEntity, bool>> whereLambda);
        /// <summary>
        /// 根据条件更新记录
        /// </summary>
        /// <param name="update">如何更新实体的委托</param>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        void Update(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> whereLambda);
        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        void Delete(Expression<Func<TEntity, bool>> whereLambda);
        /// <summary>
        /// 返回查询结果的前N条
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <param name="num">返回记录条数</param>
        /// <returns></returns>
        IList<TDomain> Take(Expression<Func<TEntity, bool>> whereLambda, int num);
    }
}