using BingoX.Domain;
using System;
#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#else
#endif
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BingoX.DataAccessor.EF
{
    /// <summary>
    /// 表示一个支持EF特性的数据访问器
    /// </summary>
    /// <typeparam name="TEntity"></typeparam>
    public interface IEFDataAccessor<TEntity> : IDataAccessor<TEntity> where TEntity : class, IEntity<TEntity>
    {
        /// <summary>
        /// 根据条件查询记录。该方法查询出的集合为EF跟踪的集合，可直接提交更新。
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        IList<TEntity> WhereTracking(Expression<Func<TEntity, bool>> whereLambda);
        /// <summary>
        /// 根据条件查询记录。该方法查询出的集合为EF跟踪的集合，可直接提交更新。
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        IList<TEntity> TakeTracking(Expression<Func<TEntity, bool>> whereLambda, int num);
    }
}
