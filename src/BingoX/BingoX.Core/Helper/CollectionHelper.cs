using System.Collections;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;

namespace BingoX.Helper
{
    
    public static class CollectionHelper
    {
        public static List<T> AsList<T>(this IEnumerable<T> collection)
        {
            if (collection == null) return new List<T>();
            return new List<T>(collection);
        }
        public static BindingList<T> AsBindingList<T>(this IEnumerable<T> collection)
        {
            if (collection == null) return new BindingList<T>();
            return new BindingList<T>(collection.ToList());
        }
      
        public static List<TTarget> AsList<TSource, TTarget>(this IEnumerable<TSource> collection)
        {
            if (collection == null) return new List<TTarget>();
            return new List<TTarget>(collection.Cast<TTarget>());
        }
    }
}
