using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX
{
    /// <summary>
    /// 
    /// </summary>
    public static class DelegateComparer
    {
        /// <summary>
        /// 
        /// </summary>                
        /// <returns></returns>
        public static DelegateComparer<string> ComparerString()
        {
            return new DelegateComparer<string>((xx, yy) => string.Equals(xx, yy, StringComparison.CurrentCultureIgnoreCase));
        }
    }

    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="TSource"></typeparam>
    public class DelegateComparer<TSource> : IEqualityComparer<TSource>
    {

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcEquals"></param>
        /// <returns></returns>
        public static DelegateComparer<TSource> Create(Func<TSource, TSource, bool> funcEquals)
        {
            return new DelegateComparer<TSource>(funcEquals);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="funcEquals"></param>
        /// <exception cref="ArgumentNullException"></exception>
        public DelegateComparer(Func<TSource, TSource, bool> funcEquals)
        {
            if (funcEquals == null) throw new ArgumentNullException("funcEquals");
            FuncEquals = funcEquals;
        }

        protected Func<TSource, TSource, bool> FuncEquals;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <returns></returns>
        public bool Equals(TSource x, TSource y)
        {

            return FuncEquals(x, y);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public int GetHashCode(TSource obj)
        {
            if (typeof(TSource).IsClass && obj == null) return -1;
            return obj.GetHashCode();
        }
    }
}
