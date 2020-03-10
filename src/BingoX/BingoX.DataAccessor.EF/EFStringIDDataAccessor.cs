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
    public class EFStringIDDataAccessor<TEntity> : EFDataAccessor<TEntity>, IDataAccessorInclude<TEntity> where TEntity : class, IStringEntity<TEntity>
    {
        public EFStringIDDataAccessor(EfDbContext context) : base(context)
        {
        }

        public override bool Exist(object id)
        {
            var list = DbSet.AsNoTracking<TEntity>();
            return list.Any(n => n.ID == (string)id);
        }

        public override void Delete(object[] pkArray)
        {

            var pks = pkArray.OfType<string>().ToArray();
            var list = Where(n => pks.Contains(n.ID)).ToArray();
            foreach (var obj in list)
            {
#if Standard
                var entityEntry = context.Remove(obj);
#else
                var entityEntry = context.Entry(obj);
#endif
                entityEntry.State = EntityState.Deleted;

            }

        }

        public override TEntity GetId(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            IQueryable<TEntity> query = DbSet;
            if (include != null) query = include(query);
            var guid = (string)id;
            return query.FirstOrDefault(n => n.ID == guid);
        }
    }
}
