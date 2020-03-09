
using System.Collections.Generic;
using SqlSugar;
using System.Linq;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarDbSet<TEntity> where TEntity : class, new()
    {
        private readonly SqlSugarClient sqlSugar;
        private readonly SqlSugarChangeTracker changeTracker;

        public SqlSugarDbSet(SqlSugarClient sqlSugar, SqlSugarChangeTracker changeTracker)
        {
            this.sqlSugar = sqlSugar;
            this.changeTracker = changeTracker;
        }

        public virtual ISugarQueryable<TEntity> AsQueryable()
        {
            return sqlSugar.Queryable<TEntity>();
        }

        public virtual void Add(TEntity entity)
        {
            changeTracker.AddInsertable(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Add(item);
            }
        }

        public virtual  void RemoveRangePrimaryKeys(dynamic[] primaryKeyValue)
        {
          
            changeTracker.AddDeleteablePrimaryKeyValue<TEntity>(primaryKeyValue);
        }
        public virtual void Remove(TEntity entity)
        {
            changeTracker.AddDeleteable<TEntity>(entity);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Remove(item);
            }
        }

       
     


        public virtual  void Update(TEntity entity)
        {
            changeTracker.AddUpdateable(entity);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Update(item);
            }
        }
         

    }
}
