using System;
using System.Linq.Expressions;

namespace BingoX.Repository
{
    /// <summary>
    /// 数据操作规格
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public interface ISpecification<T> where T : class
    {

        bool OrderType { get; }
        /// <summary>
        /// 当前页码
        /// </summary>
        int PageIndex { get; set; }
        /// <summary>
        /// 页长
        /// </summary>
        int PageSize { get; set; }
        /// <summary>
        /// 数据操作规格表达式
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, bool>> ToExpression();
        /// <summary>
        /// 数据排序规格表达式
        /// </summary>
        /// <returns></returns>
        Expression<Func<T, object>> ToStorExpression();
        /// <summary>
        /// 判断数据操作规格是否有效
        /// </summary>
        /// <param name="entity">数据库实体</param>
        /// <returns>是否有效</returns>
        bool IsSatisfiedBy(T entity);
        /// <summary>
        /// 升序排序
        /// </summary>
        /// <param name="orderExpression">排序规格表达式</param>
        void Orderby(Expression<Func<T, object>> orderExpression);
        /// <summary>
        /// 降序排序
        /// </summary>
        /// <param name="orderExpression">排序规格表达式</param>
        void OrderbyDesc(Expression<Func<T, object>> orderExpression);
        /// <summary>
        /// 构建逻辑与规格
        /// </summary>
        /// <param name="specification">规格操作数</param>
        /// <returns></returns>
        Specification<T> And(Specification<T> specification);
        /// <summary>
        /// 构建逻辑非规格
        /// </summary>
        /// <param name="specification">规格操作数</param>
        /// <returns></returns>
        Specification<T> Not(Specification<T> specification);
        /// <summary>
        /// 构建逻辑或规格
        /// </summary>
        /// <param name="specification">规格操作数</param>
        /// <returns></returns>
        Specification<T> Or(Specification<T> specification);
        /// <summary>
        /// 构建逻辑与规格
        /// </summary>
        /// <param name="expression">表达式操作数</param>
        /// <returns></returns>
        Specification<T> And(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 构建逻辑或规格
        /// </summary>
        /// <param name="expression">表达式操作数</param>
        /// <returns></returns>
        Specification<T> Or(Expression<Func<T, bool>> expression);
        /// <summary>
        /// 构建逻辑非规格
        /// </summary>
        /// <param name="expression">表达式操作数</param>
        /// <returns></returns>
        Specification<T> Not(Expression<Func<T, bool>> expression);
    }
    public class Specification<T> : ISpecification<T> where T : class
    {
        public Specification()
        {
            PageIndex = 0;
            PageSize = 20;
        }

        protected internal Expression<Func<T, bool>> searchPredicate = PredicateExtensionses.True<T>();
        protected internal Expression<Func<T, object>> orderPredicate;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool OrderType { get; private set; }
        public virtual bool IsSatisfiedBy(T entity)
        {
            return searchPredicate.Compile().Invoke(entity);

        }
        public virtual Expression<Func<T, object>> ToStorExpression()
        {
            return orderPredicate;
        }
        public virtual Expression<Func<T, bool>> ToExpression()
        {
            return searchPredicate;
        }

        public virtual Specification<T> And(Specification<T> specification)
        {
            var ex = specification.ToExpression();
            searchPredicate = searchPredicate.And(ex);
            return this;
        }
        public virtual Specification<T> Not(Specification<T> specification)
        {
            var ex = specification.ToExpression();
            searchPredicate = searchPredicate.NotEqual(ex);
            return this;
        }

        public virtual Specification<T> Or(Specification<T> specification)
        {
            var ex = specification.ToExpression();
            searchPredicate = searchPredicate.Or(ex);
            return this;
        }

        public virtual Specification<T> And(Expression<Func<T, bool>> expression)
        {
            searchPredicate = searchPredicate.And(expression);
            return this;
        }

        public virtual Specification<T> Or(Expression<Func<T, bool>> expression)
        {
            searchPredicate = searchPredicate.Or(expression);
            return this;
        }

        public virtual Specification<T> Not(Expression<Func<T, bool>> expression)
        {
            searchPredicate = searchPredicate.NotEqual(expression);
            return this;
        }

        public virtual void Orderby(Expression<Func<T, object>> orderExpression)
        {
            orderPredicate = orderExpression;

        }
        public virtual void OrderbyDesc(Expression<Func<T, object>> orderExpression)
        {
            orderPredicate = orderExpression;

        }
    }
}