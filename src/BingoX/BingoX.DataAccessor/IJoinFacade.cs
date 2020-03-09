using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BingoX.DataAccessor
{
    public interface IJoinFacade
    {
        IJoinQueryable<TOuter, TInner, TResult> Query<TOuter, TInner, TKey, TResult>(Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
             where TOuter : class, new()
             where TInner : class, new()
             where TKey : IEquatable<TKey>;
    }

    public interface IJoinQueryable<TOuter, TInner, TResult>
    {
        TResult First(Expression<Func<TOuter, TInner, bool>> whereLambda);
        TResult First();
        IList<TResult> ToList(Expression<Func<TOuter, TInner, bool>> whereLambda);
        IList<TResult> ToList();
    }
}
