﻿using BingoX.Domain;
using System;
#if Standard
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
#endif
using System.Collections.Generic;
using System.Linq;
using BingoX.Helper;
using System.Data;

namespace BingoX.DataAccessor.EF
{
    public class EFSnowflakeDataAccessor<TEntity> : EFDataAccessor<TEntity>, IDataAccessorInclude<TEntity> where TEntity : class, ISnowflakeEntity<TEntity>
    {
        public EFSnowflakeDataAccessor(EfDbContext context) : base(context)
        {
        }

        List<TEntity> GetId(long[] id)
        {
            var query = DbSet.AsNoTracking<TEntity>();
            if (SetInclude != null) query = SetInclude(query);
            return query.Where(n => id.Contains(n.ID)).ToList();
        }

        public override TEntity GetId(object id)
        {
            var query = DbSet.AsQueryable();
            if (SetInclude != null) query = SetInclude(query);
            return query.FirstOrDefault(n => n.ID == (long)id);
        }

        public override bool Exist(object id)
        {
            var list = DbSet.AsNoTracking<TEntity>();
            return list.Any(n => n.ID == (long)id);
        }

        public override int Delete(object[] pkArray)
        {
            int count = 0;
            var list = GetId(pkArray.Select(n => (long)n).ToArray());
            foreach (var obj in list)
            {
#if Standard
                var entityEntry = context.Remove(obj);
#else
                var entityEntry = context.Entry(obj);
#endif
                entityEntry.State = EntityState.Deleted;
                count++;
            }
            return count;
        }

        public override TEntity GetId(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            if (include == null) return GetId(id);
            var query = include(DbSet.AsNoTracking<TEntity>());
            return query.FirstOrDefault(n => n.ID == (long)id);
        }
    }
}
