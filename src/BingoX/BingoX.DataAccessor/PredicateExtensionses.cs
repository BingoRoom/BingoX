using System;
using System.Collections.Generic;
using System.Linq.Expressions;
using System.Text;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表达式扩展
    /// </summary>
    static class PredicateExtensionses
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
    }
}
