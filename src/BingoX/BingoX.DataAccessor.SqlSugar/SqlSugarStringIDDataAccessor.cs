using BingoX.Domain;
using System; 
using System.Collections.Generic;
using System.Linq;
using BingoX.Helper;
using System.Data;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarStringIDDataAccessor<TEntity> : SqlSugarDataAccessor<TEntity>    where TEntity : class, IStringEntity<TEntity>, new()
    {
        public SqlSugarStringIDDataAccessor(SqlSugarDbContext context) : base(context)
        {
        }



        public override TEntity GetId(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {

            var guid = (string)id;
            var entity = DbSet.AsQueryable().First(n => n.ID == guid);
            if (include == null) return entity;
            return include(new[] { entity }.AsQueryable()).FirstOrDefault();
        }
        public override bool Exist(object id)
        {
            var list = DbSet.AsQueryable();
            return list.Any(n => n.ID == (string)id);
        }

         
    }
}
