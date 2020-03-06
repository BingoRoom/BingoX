
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
            var insertable= sqlSugar.Insertable(entity);
            changeTracker.AddInsertable(entity);
        }

        public virtual void AddRange(IEnumerable<TEntity> entities)
        {
            foreach (var item in entities)
            {
                Add(item);
            }
        }

        public virtual void AddRange(params TEntity[] entities)
        {
        
            foreach (var item in entities)
            {
                Add(item);
            }
        }
        public virtual  void RemoveRangePrimaryKeys(object[] primaryKeyValue)
        {
            var deleteable = sqlSugar.Deleteable<TEntity>().In(primaryKeyValue);
            changeTracker.Add(deleteable);
        }
        public virtual void Remove(TEntity entity)
        {
            var deleteable = sqlSugar.Deleteable(entity);
            changeTracker.Add(deleteable);
        }

        public virtual void RemoveRange(IEnumerable<TEntity> entities)
        {
            var deleteable = sqlSugar.Deleteable(entities.ToList());
            changeTracker.Add(deleteable);
        }

        public virtual void RemoveRange(params TEntity[] entities)
        {
            var deleteable = sqlSugar.Deleteable(entities.ToList());
            changeTracker.Add(deleteable);
        }
     


        public virtual  void Update(TEntity entity)
        {
            var updateable = sqlSugar.Updateable(entity);
            changeTracker.Add(updateable);
        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {
            var updateable = sqlSugar.Updateable(entities.ToList());
            changeTracker.Add(updateable);
        }

        public virtual void UpdateRange(params TEntity[] entities)
        {
            var updateable = sqlSugar.Updateable(entities.ToList());
            changeTracker.Add(updateable);
        }


    }
}
