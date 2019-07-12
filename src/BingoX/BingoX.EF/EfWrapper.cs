#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#else

using System.Data.Entity;
#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;

namespace BingoX.EF
{
    public class EfWrapper<T> where T : class, new()
    {
        private EfDbContext context;
        protected internal DbSet<T> DbSet { get; private set; }
        public EfWrapper(EfDbContext context)
        {
            this.context = context;
            DbSet = context.Set<T>();
        }
        public IQueryable<T> Find()
        {
            return DbSet;
        }
#if Standard
        public EntityEntry<T> Add(T entity)
        {
            return DbSet.Add(entity);
        }
#else
        public T Add(T entity)
        {
            return DbSet.Add(entity);
        }
#endif
        public void AddRange(IEnumerable<T> entities)
        {
            DbSet.AddRange(entities);
        }

        public IQueryable<T> QueryAll()
        {
            return DbSet.AsNoTracking();
        }

        public T Get(Expression<Func<T, bool>> whereLambda)
        {
            return DbSet.FirstOrDefault(whereLambda);
        }
        public bool IsExist(Expression<Func<T, bool>> whereLambda)
        {
            return DbSet.Any(whereLambda);
        }
    }
}
