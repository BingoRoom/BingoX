#if Standard
#else

#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BingoX.DataAccessor.EF
{
    public class EFJoinFacade : IJoinFacade
    {
        public EFJoinFacade(EfDbContext context)
        {
            Context = context;
        }
        public EfDbContext Context { get; }
        public IJoinQueryable<TOuter, TInner, TResult> Query<TOuter, TInner, TKey, TResult>(
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
            where TOuter : class, new()
            where TInner : class, new()
            where TKey : IEquatable<TKey>
        {

            return new EFJoinQueryable<TOuter, TInner, TKey, TResult>(Context, outerKeySelector, innerKeySelector, resultSelector);

        }

    }
    public class EFJoinQueryable<TOuter, TInner, TKey, TResult> : IJoinQueryable<TOuter, TInner, TResult>
           where TOuter : class, new()
           where TInner : class, new()
           where TKey : IEquatable<TKey>
    {
        public EFJoinQueryable(EfDbContext context,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            Context = context;
            this.resultSelector = resultSelector.Compile();
            this.outerKeySelector = outerKeySelector.Compile();
            this.innerKeySelector = innerKeySelector.Compile();
        }

        readonly Func<TOuter, TKey> outerKeySelector;
        readonly Func<TInner, TKey> innerKeySelector;
        readonly Func<TOuter, TInner, TResult> resultSelector;
        public EfDbContext Context { get; }

        private IQueryable<TResult> Where(Expression<Func<TOuter, TInner, bool>> whereLambda)
        {
            var outerSet = Context.Set<TOuter>();
            var inner1Set = Context.Set<TInner>();          
            var whereKey = whereLambda.Compile();
            var query = (from outer in outerSet
                         join inner1 in inner1Set on outerKeySelector(outer) equals innerKeySelector(inner1)
                         where whereKey(outer, inner1)
                         select resultSelector(outer, inner1));
            return query;
        }
        private IQueryable<TResult> Where( )
        {
            var outerSet = Context.Set<TOuter>();
            var inner1Set = Context.Set<TInner>();
            var query = (from outer in outerSet
                         join inner1 in inner1Set on outerKeySelector(outer) equals innerKeySelector(inner1)
                         select resultSelector(outer, inner1));
            return query;
        }
        public IList<TResult> ToList(Expression<Func<TOuter, TInner, bool>> whereLambda)
        {           
            return Where(whereLambda).ToList();
        }
        public IList<TResult> ToList()
        {
            return Where().ToList();
        }

        public TResult First(Expression<Func<TOuter, TInner, bool>> whereLambda)
        {

            return Where(whereLambda).FirstOrDefault();
        }
        public TResult First()
        { 
            return Where().FirstOrDefault();
        }
    }
}
