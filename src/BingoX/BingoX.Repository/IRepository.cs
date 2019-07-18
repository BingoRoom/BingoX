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
    public interface IRepository<T, pkType> : IRepository where T : class, IEntity<T, pkType>
    {

        /// <summary>
        /// 批量新增记录
        /// </summary>
        /// <param name="entites"待新增记录集合</param>
        /// <returns>受影响记录数</returns>
        int AddRange(IEnumerable<T> entites);
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="entity">待新增记录实体</param>
        /// <returns>受影响记录数</returns>
        int Add(T entity);
        /// <summary>
        /// 根据主键查询记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>查询结果</returns>
        T GetId(pkType id);
        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <returns>查询结果集合</returns>
        IList<T> QueryAll();
        /// <summary>
        /// 分页查询
        /// </summary>
        /// <param name="specification">分页规格</param>
        /// <param name="total">总记录数</param>
        /// <returns>当前页记录集合</returns>
        IList<T> PageList(ISpecification<T> specification, ref int total);
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity">待更新记录实体</param>
        /// <returns>受影响记录数</returns>
        int Update(T entity);
        /// <summary>
        /// 批量更新记录
        /// </summary>
        /// <param name="entitys">待更新记录集合</param>
        /// <returns>受影响记录数</returns>
        int UpdateRange(IEnumerable<T> entitys);
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
        int Delete(T entity);
    }

    public interface IRepository
    {
        IUnitOfWork UnitOfWork { get; }
    }
    /// <summary>
    /// 支持字符串主键的仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositoryStringID<T> : IRepository<T, string> where T : class, IStringEntity<T>
    {

    }
    /// <summary>
    /// 支持GUID主键的仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositoryGuid<T> : IRepository<T, Guid> where T : class, IGuidEntity<T>
    {

    }
    /// <summary>
    /// 支持雪花主键的仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositorySnowflake<T> : IRepository<T, long> where T : class, ISnowflakeEntity<T>
    {

    }
    /// <summary>
    /// 支持自增主键的仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositoryIdentity<T> : IRepository<T, int> where T : class, IIdentityEntity<T>
    {

    }
    /// <summary>
    /// 支持返回新增记录信息的仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositoryReturn<T> where T : class
    {
        /// <summary>
        /// 批量新增记录
        /// </summary>
        /// <param name="entites">待新增记录集合</param>
        /// <returns>是否新增成功</returns>
        bool AddReturnBool(List<T> entites);
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="entity">待新增记录</param>
        /// <returns>是否新增成功</returns>
        bool AddReturnBool(T entity);
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="entity">待新增记录</param>
        /// <returns>新增成功的记录实体</returns>
        T AddReturnEntity(T entity);
    }
    /// <summary>
    /// 支持SQL语句查询的仓储接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositorySql<T> where T : class
    {
        /// <summary>
        /// SQL语句查询
        /// </summary>
        /// <param name="sql">SQL语句</param>
        /// <returns>查询结果集合</returns>
        IList<T> Where(string sql);
    }
    /// <summary>
    /// 仓储构建器接口
    /// </summary>
    public interface IRepositoryBuilder
    {
        /// <summary>
        /// 构建仓储
        /// </summary>
        /// <typeparam name="T">数据库实体类型</typeparam>
        /// <typeparam name="pkType">主键类型</typeparam>
        /// <returns>仓储实例</returns>
        IRepository<T, pkType> Cretae<T, pkType>() where T : class, IEntity<T, pkType>, new();
        /// <summary>
        /// 事务管理器
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
    }
    /// <summary>
    /// 数据库表异步锁接口
    /// </summary>
    public interface ICanLock
    {
        /// <summary>
        /// 获取或设置当前操作表的锁定状态
        /// </summary>
        bool IsLock { get; set; }
    }
    /// <summary>
    /// 支持表达式查询的仓储表接口
    /// </summary>
    /// <typeparam name="T">数据库实体类型</typeparam>
    public interface IRepositoryExpression<T> where T : class
    {
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>查询结果</returns>
        T Get(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 判断记录是否存在
        /// </summary>
        /// <param name="whereLambda">表达式</param>
        /// <returns></returns>
        bool IsExist(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 查询记录
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>查询结果集合</returns>
        IList<T> Where(Expression<Func<T, bool>> whereLambda);
        /// <summary>
        /// 获取指定条数的查询
        /// </summary>
        /// <param name="whereLambda">表达式</param>
        /// <param name="num">条数</param>
        /// <returns>查询结果集合</returns>
        IList<T> Take(Expression<Func<T, bool>> whereLambda, int num);
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="update">更新内容表达式</param>
        /// <param name="where">更新条件表达式</param>
        /// <returns>受影响记录数</returns>
        int Update(Expression<Func<T, T>> update, Expression<Func<T, bool>> where);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="where">删除条件表达式</param>
        /// <returns>受影响记录数</returns>
        int Delete(Expression<Func<T, bool>> where);

    }

}