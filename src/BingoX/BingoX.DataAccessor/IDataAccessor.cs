using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq.Expressions;

namespace BingoX.DataAccessor
{
  
    /// <summary>
    /// 表示一个指定DAO的数据访问器
    /// </summary>
    /// <typeparam name="TEntity">DAO类型</typeparam>
    public interface IDataAccessor<TEntity>  where TEntity :   IEntity<TEntity>
    {
        /// <summary>
        /// 事务单元
        /// </summary>
        IUnitOfWork UnitOfWork { get; }
        /// <summary>
        /// 创建SQL命令门面
        /// </summary>
        /// <returns></returns>
        ISqlFacade CreateSqlFacade();
        /// <summary>
        /// 新增记录
        /// </summary>
        /// <param name="entity">待新增记录实体</param>
        /// <returns>受影响记录数</returns>
        int Add(TEntity entity);
        /// <summary>
        /// 批量新增记录
        /// </summary>
        /// <param name="entites"待新增记录集合</param>
        /// <returns>受影响记录数</returns>
        int AddRange(IEnumerable<TEntity> entites);
        /// <summary>
        /// 根据主键查询记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>查询结果</returns>
        TEntity GetId(object id);
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
        IList<TEntity> QueryAll();
        /// <summary>
        /// 分页查询记录
        /// </summary>
        /// <param name="specification">分页规格</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">specification为null</exception>
        IList<TEntity> PageList(ISpecification<TEntity> specification, ref int total);
        /// <summary>
        /// 更新记录
        /// </summary>
        /// <param name="entity">待更新记录实体</param>
        /// <returns>受影响记录数</returns>
        int Update(TEntity entity);
        /// <summary>
        /// 批量更新记录
        /// </summary>
        /// <param name="entitys">待更新记录集合</param>
        /// <returns>受影响记录数</returns>
        int UpdateRange(IEnumerable<TEntity> entities);
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
        int Delete(TEntity entity);
        /// <summary>
        /// 开启事务
        /// </summary>
        void BeginTran(IsolationLevel level = IsolationLevel.ReadCommitted);
        /// <summary>
        /// 提交事务
        /// </summary>
        void Commit();
        /// <summary>
        /// 回滚事务
        /// </summary>
        void Rollback();
        /// <summary>
        /// 根据条件查询记录
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        IList<TEntity> Where(Expression<Func<TEntity, bool>> whereLambda);
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
        int Update(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> whereLambda);
        /// <summary>
        /// 根据条件删除记录
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        int Delete(Expression<Func<TEntity, bool>> whereLambda);
        /// <summary>
        /// 返回查询结果的前N条
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <param name="num">返回记录条数</param>
        /// <returns></returns>
        IList<TEntity> Take(Expression<Func<TEntity, bool>> whereLambda, int num);
    }
}
