#if Standard 
#else

#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SqlSugar;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarJoinFacade : IJoinFacade
    {
        public SqlSugarJoinFacade(SqlSugarDbContext context)
        {
            Context = context;
        }
        public SqlSugarDbContext Context { get; }


        public IJoinQueryable<TOuter, TInner, TResult> Query<TOuter, TInner, TKey, TResult>(Expression<Func<TOuter, TKey>> outerKeySelector, Expression<Func<TInner, TKey>> innerKeySelector, Expression<Func<TOuter, TInner, TResult>> resultSelector)
          where TOuter : class, new()
          where TInner : class, new()
          where TKey : IEquatable<TKey>
        {
            return new SqlSugarJoinQueryable<TOuter, TInner, TKey, TResult>(Context, outerKeySelector, innerKeySelector, resultSelector);
        }
    }

    public class SqlSugarJoinQueryable<TOuter, TInner, TKey, TResult> : IJoinQueryable<TOuter, TInner, TResult>
          where TOuter : class, new()
          where TInner : class, new()
          where TKey : IEquatable<TKey>
    {
        public SqlSugarJoinQueryable(SqlSugarDbContext context,
            Expression<Func<TOuter, TKey>> outerKeySelector,
            Expression<Func<TInner, TKey>> innerKeySelector,
            Expression<Func<TOuter, TInner, TResult>> resultSelector)
        {
            Context = context;
            this.resultSelector = resultSelector;
            var outerSelector = outerKeySelector.Compile();
            var innerSelector = innerKeySelector.Compile();
            Queryable = Context.Database.Queryable<TOuter, TInner>((outer, inner) =>
               new object[] {
                   JoinType.Inner,
                  outerSelector(outer).Equals(innerSelector(inner))
               }
            );
        }
        readonly ISugarQueryable<TOuter, TInner> Queryable;


        readonly Expression<Func<TOuter, TInner, TResult>> resultSelector;
        public SqlSugarDbContext Context { get; }

        public IList<TResult> ToList(Expression<Func<TOuter, TInner, bool>> whereLambda)
        {


            return Queryable.Where(whereLambda).Select(resultSelector).ToList();
        }
        public IList<TResult> ToList()
        {
            return Queryable.Select(resultSelector).ToList();
        }

        public TResult First(Expression<Func<TOuter, TInner, bool>> whereLambda)
        {
            return Queryable.Where(whereLambda).Select(resultSelector).First();
        }
        public TResult First()
        {
            return Queryable.Select(resultSelector).First();
        }
    }
}
