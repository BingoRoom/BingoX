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
    public class AndSpecification<T> : Specification<T> where T : class
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;
        private Expression<Func<T, bool>> rightExpression;

        public AndSpecification(Specification<T> left, Specification<T> right)
        {
            _right = right;
            _left = left;
        }
        public AndSpecification(Specification<T> left, Expression<Func<T, bool>> right)
        {
            rightExpression = right;
            _left = left;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            Expression<Func<T, bool>> leftExpression = _left.ToExpression();
            if (rightExpression == null) rightExpression = _right.ToExpression();

            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.AndAlso(leftExpression.Body, rightExpression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return finalExpr;

        }
    }
    public class OrSpecification<T> : Specification<T> where T : class
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;
        private Expression<Func<T, bool>> rightExpression;


        public OrSpecification(Specification<T> left, Specification<T> right)
        {
            _right = right;
            _left = left;
        }
        public OrSpecification(Specification<T> left, Expression<Func<T, bool>> right)
        {
            rightExpression = right;
            _left = left;
        }


        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            if (rightExpression == null) rightExpression = _right.ToExpression();
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.OrElse(leftExpression.Body, rightExpression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return finalExpr;
        }
    }
    public class NotSpecification<T> : Specification<T> where T : class
    {
        private readonly Specification<T> _left;
        private readonly Specification<T> _right;
        private Expression<Func<T, bool>> rightExpression;

        public NotSpecification(Specification<T> left, Specification<T> right)
        {
            _right = right;
            _left = left;
        }
        public NotSpecification(Specification<T> left, Expression<Func<T, bool>> right)
        {
            rightExpression = right;
            _left = left;
        }

        public override Expression<Func<T, bool>> ToExpression()
        {
            var leftExpression = _left.ToExpression();
            if (rightExpression == null) rightExpression = _right.ToExpression();
            var paramExpr = Expression.Parameter(typeof(T));
            var exprBody = Expression.NotEqual(leftExpression.Body, rightExpression.Body);
            exprBody = (BinaryExpression)new ParameterReplacer(paramExpr).Visit(exprBody);
            var finalExpr = Expression.Lambda<Func<T, bool>>(exprBody, paramExpr);

            return finalExpr;


        }
    }
    public class Specification<T> : ISpecification<T> where T : class
    {
        public Specification()
        {
            PageIndex = 0;
            PageSize = 20;
        }
        static Expression<Func<T, bool>> True() { return f => true; }
        protected internal Expression<Func<T, bool>> SearchPredicate = True();
        protected internal Expression<Func<T, object>> OrderPredicate;
        public int PageIndex { get; set; }
        public int PageSize { get; set; }
        public bool OrderType { get; private set; }
        public virtual bool IsSatisfiedBy(T entity)
        {
            return SearchPredicate.Compile().Invoke(entity);

        }
        public virtual Expression<Func<T, object>> ToStorExpression()
        {
            return OrderPredicate;
        }
        public virtual Expression<Func<T, bool>> ToExpression()
        {
            return SearchPredicate;
        }

        public virtual Specification<T> And(Specification<T> specification)
        {

            return new AndSpecification<T>(this, specification);
        }
        public virtual Specification<T> Not(Specification<T> specification)
        {
            return new NotSpecification<T>(this, specification);
        }

        public virtual Specification<T> Or(Specification<T> specification)
        {

            return new OrSpecification<T>(this, specification);
        }

        public virtual Specification<T> And(Expression<Func<T, bool>> expression)
        {
            return new AndSpecification<T>(this, expression);
        }

        public virtual Specification<T> Or(Expression<Func<T, bool>> expression)
        {
            return new OrSpecification<T>(this, expression);
        }

        public virtual Specification<T> Not(Expression<Func<T, bool>> expression)
        {
            return new NotSpecification<T>(this, expression);
        }

        public virtual void Orderby(Expression<Func<T, object>> orderExpression)
        {
            OrderPredicate = orderExpression;

        }
        public virtual void OrderbyDesc(Expression<Func<T, object>> orderExpression)
        {
            OrderPredicate = orderExpression;

        }

    }
}