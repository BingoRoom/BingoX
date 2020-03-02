using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace BingoX.DataAccessor.EF
{
    public interface IDataAccessorInclude<TEntity> where TEntity : class, IEntity<TEntity>
    {
        /// <summary>
        /// 根据主键查询记录
        /// </summary>
        /// <param name="id">主键</param>
        /// <returns>查询结果</returns>
        TEntity GetId(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include);
        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <returns>查询结果集合</returns>
        IList<TEntity> QueryAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> include);
        /// <summary>
        /// 分页查询记录
        /// </summary>
        /// <param name="specification">分页规格</param>
        /// <param name="total">总记录数</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">specification为null</exception>
        IList<TEntity> PageList(ISpecification<TEntity> specification, ref int total, Func<IQueryable<TEntity>, IQueryable<TEntity>> include);
        /// <summary>
        /// 根据条件查询记录
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <returns></returns>
        /// <exception cref="ArgumentException">whereLambda为null</exception>
        IList<TEntity> Where(Expression<Func<TEntity, bool>> whereLambda, Func<IQueryable<TEntity>, IQueryable<TEntity>> include);
        /// <summary>
        /// 返回查询结果的前N条
        /// </summary>
        /// <param name="whereLambda">查询条件表达式</param>
        /// <param name="num">返回记录条数</param>
        /// <returns></returns>
        IList<TEntity> Take(Expression<Func<TEntity, bool>> whereLambda, int num, Func<IQueryable<TEntity>, IQueryable<TEntity>> include);
    }
}
