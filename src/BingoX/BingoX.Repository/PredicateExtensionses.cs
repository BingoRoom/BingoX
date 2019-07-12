using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BingoX.Repository
{
    /// <summary>
    /// 表达式扩展
    /// </summary>
    public static class PredicateExtensionses
    {
        /// <summary>
        /// 返回真表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> True<T>() { return f => true; }
        /// <summary>
        /// 返回假表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static Expression<Func<T, bool>> False<T>() { return f => false; }
        /// <summary>
        /// 返回执行逻辑与之后的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expLeft">表达式操作数</param>
        /// <param name="expRight">表达式操作数</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> And<T>(this Expression<Func<T, bool>> expLeft, Expression<Func<T, bool>> expRight)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(expLeft.Body);
            var right = parameterReplacer.Replace(expRight.Body);
            var body = Expression.And(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        /// <summary>
        /// 返回执行逻辑或之后的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expLeft">表达式操作数</param>
        /// <param name="expRight">表达式操作数</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> Or<T>(this Expression<Func<T, bool>> expLeft, Expression<Func<T, bool>> expRight)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(expLeft.Body);
            var right = parameterReplacer.Replace(expRight.Body);
            var body = Expression.Or(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);
        }
        /// <summary>
        /// 返回执行逻辑非之后的表达式
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="expLeft">表达式操作数</param>
        /// <param name="expRight">表达式操作数</param>
        /// <returns></returns>
        public static Expression<Func<T, bool>> NotEqual<T>(this Expression<Func<T, bool>> expLeft, Expression<Func<T, bool>> expRight)
        {
            var candidateExpr = Expression.Parameter(typeof(T), "candidate");
            var parameterReplacer = new ParameterReplacer(candidateExpr);

            var left = parameterReplacer.Replace(expLeft.Body);
            var right = parameterReplacer.Replace(expRight.Body);
            var body = Expression.NotEqual(left, right);

            return Expression.Lambda<Func<T, bool>>(body, candidateExpr);

        }
    }
}
