using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using BingoX.Helper;
using System.Data;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarSnowflakeDataAccessor<TEntity> : SqlSugarDataAccessor<TEntity> where TEntity : class, ISnowflakeEntity<TEntity>,new()
    {
        public SqlSugarSnowflakeDataAccessor(SqlSugarDbContext context) : base(context)
        {
        }

        public override TEntity GetId(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {

            var guid = (long)id;
            var entity = DbSet.AsQueryable().First(n => n.ID == guid);
            if (include == null) return entity;
            return include(new[] { entity }.AsQueryable()).FirstOrDefault();
        }

        public override bool Exist(object id)
        {
            var query = DbSet.AsQueryable();
            var guid = (long)id;
            return query.Any(n => n.ID == guid);
        }

    }
}
