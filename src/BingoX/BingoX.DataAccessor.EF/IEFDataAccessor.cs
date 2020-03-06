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
    public interface IEFDataAccessor<TEntity> : IDataAccessor<TEntity> where TEntity : class, IEntity<TEntity>
    {
        /// <summary>
        /// 根据条件查询记录
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        IList<TEntity> WhereTracking(Expression<Func<TEntity, bool>> whereLambda);
        /// <summary>
        /// 根据条件查询记录
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        IList<TEntity> TakeTracking(Expression<Func<TEntity, bool>> whereLambda, int num);
    }
}
