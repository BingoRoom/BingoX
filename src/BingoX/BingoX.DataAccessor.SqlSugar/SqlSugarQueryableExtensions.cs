
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BingoX.DataAccessor.SqlSugar
{
    namespace Expressions
    {
        public static class SqlSugarQueryableExtensions
        {
            public static IIncludableQueryable<TEntity, TProperty> Include<TEntity, TProperty>(this IQueryable<TEntity> source, Expression<Func<TEntity, TProperty>> navigationPropertyPath) where TEntity : class
            {
                throw new NotImplementedException();
            }


            public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, TPreviousProperty> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class
            {
                throw new NotImplementedException();
            }

            public static IIncludableQueryable<TEntity, TProperty> ThenInclude<TEntity, TPreviousProperty, TProperty>(this IIncludableQueryable<TEntity, IEnumerable<TPreviousProperty>> source, Expression<Func<TPreviousProperty, TProperty>> navigationPropertyPath) where TEntity : class
            {
                throw new NotImplementedException();
            }
        }

        public interface IIncludableQueryable<out TEntity, out TProperty> : IQueryable<TEntity>, IEnumerable<TEntity>, IQueryable
        {
        }
    }
}
