using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BingoX.Repository
{
    /// <summary>
    /// 支持表达式查询的仓储表接口
    /// </summary>
    /// <typeparam name="TDomain">领域实体</typeparam>
    public interface IRepositoryExpression<TDomain>
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
        bool Exist(Expression<Func<TDomain, bool>> whereLambda);
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
        ///// <summary>
        ///// 更新记录
        ///// </summary>
        ///// <param name="update">更新内容表达式</param>
        ///// <param name="where">更新条件表达式</param>
        ///// <returns>受影响记录数</returns>
        //int Update(Expression<Func<TDomain, TDomain>> update, Expression<Func<TDomain, bool>> where);
        /// <summary>
        /// 删除记录
        /// </summary>
        /// <param name="where">删除条件表达式</param>
        /// <returns>受影响记录数</returns>
        void Delete(Expression<Func<TDomain, bool>> where);

    }
}