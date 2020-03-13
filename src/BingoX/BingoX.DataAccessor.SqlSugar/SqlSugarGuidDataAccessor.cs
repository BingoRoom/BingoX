using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using BingoX.Helper;
using System.Data;
using System.Linq.Expressions;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarGuidDataAccessor<TEntity> : SqlSugarDataAccessor<TEntity> where TEntity : class, IGuidEntity<TEntity>, new()
    {
        public SqlSugarGuidDataAccessor(SqlSugarDbContext context) : base(context)
        {

        }




        public override bool Exist(object id)
        {
            var query = DbSet.AsQueryable();
            var guid = (Guid)id;
            return query.Any(n => n.ID == guid);
        }

        public override TEntity GetId(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {

            var guid = (Guid)id;
            var entity = DbSet.AsQueryable().First(n => n.ID == guid);
            if (include == null) return entity;
            return include(new[] { entity }.AsQueryable()).FirstOrDefault();
        }
    }
}
