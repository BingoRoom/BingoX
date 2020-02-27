using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace BingoX.Repository
{
 
  
    /// <summary>
    /// 仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    /// <typeparam name="pkType">主键类型</typeparam>
    public interface IRepository<TDomain, pkType> : IRepository where TDomain : IAggregate
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

    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
    /// <summary>
    /// 支持字符串主键的仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositoryStringID<TDomain> : IRepository<TDomain, string> where TDomain : IAggregate
    {

    }
    /// <summary>
    /// 支持GUID主键的仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositoryGuid<TDomain> : IRepository<TDomain, Guid> where TDomain : IAggregate
    {

    }
    /// <summary>
    /// 支持雪花主键的仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositorySnowflake<TDomain> : IRepository<TDomain, long> where TDomain : IAggregate
    {

    }
    /// <summary>
    /// 支持自增主键的仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositoryIdentity<TDomain> : IRepository<TDomain, int> where TDomain : IAggregate
    {
      

    }

    /// <summary>
    /// 支持表达式查询的仓储表接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositoryExpression<TDomain> where TDomain : class
    {
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>查询结果</returns>
        TDomain Get(Expression<Func<TDomain, bool>> whereLambda);
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="whereLambda">表达式</param>
        /// <returns></returns>
        bool IsExist(Expression<Func<TDomain, bool>> whereLambda);
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>查询结果集合</returns>
        IList<TDomain> Where(Expression<Func<TDomain, bool>> whereLambda);
        /// <summary>
        /// 获取指定条数的查询
        /// </summary>
        /// <param name="whereLambda">表达式</param>
        /// <param name="num">条数</param>
        /// <returns>查询结果集合</returns>
        IList<TDomain> Take(Expression<Func<TDomain, bool>> whereLambda, int num);
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="update">更新内容表达式</param>
        /// <param name="where">更新条件表达式</param>
        /// <returns>受影响记录数</returns>
        int Update(Expression<Func<TDomain, TDomain>> update, Expression<Func<TDomain, bool>> where);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="where">删除条件表达式</param>
        /// <returns>受影响记录数</returns>
        int Delete(Expression<Func<TDomain, bool>> where);

    }

}