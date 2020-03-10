using BingoX.Domain;
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
    public class EFIdentityDataAccessor<TEntity> : EFDataAccessor<TEntity>, IDataAccessorInclude<TEntity> where TEntity : class, IIdentityEntity<TEntity>
    {
        public EFIdentityDataAccessor(EfDbContext context) : base(context)
        {
        }
         

       

        public override bool Exist(object id)
        {
            var list = DbSet.AsNoTracking<TEntity>();
            return list.Any(n => n.ID == (int)id);
        }

        public override void Delete(object[] pkArray)
        {
            int count = 0;
            var list = Where(n => pkArray.Contains(n.ID)).ToArray();
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

        }

        public override TEntity GetId(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            if (include != null) return GetId(id);
            var query = include(DbSet.AsNoTracking<TEntity>());
            return query.FirstOrDefault(n => n.ID == (int)id);
        }
    }
}
