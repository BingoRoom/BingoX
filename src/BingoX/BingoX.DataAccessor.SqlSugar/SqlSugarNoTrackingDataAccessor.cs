using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using SqlSugar;

namespace BingoX.DataAccessor.SqlSugar
{
    public sealed class SqlSugarNoTrackingDataAccessor<TEntity> : INoTrackingDataAccessor<TEntity> where TEntity : class, IEntity<TEntity>, new()
    {



        public SqlSugarNoTrackingDataAccessor(SqlSugarDataAccessor<TEntity> sqlSugarDataAccessor)
        {
            Context = sqlSugarDataAccessor.Context;
        }

        readonly SqlSugarDbContext Context;


        public void Add(TEntity entity)
        {
            Context.ChangeTracker.EntityEntries.Add(new NoTrackingAddEntry(Context.Database.Insertable<TEntity>(entity)));
        }
        public void AddRange(IEnumerable<TEntity> entities)
        {
            
            Context.ChangeTracker.EntityEntries.Add(new NoTrackingAddEntry(Context.Database.Insertable<TEntity>(entities.ToArray())));
        }
      
        public void Delete(Expression<Func<TEntity, bool>> whereLambda)
        {
            Context.ChangeTracker.EntityEntries.Add(new NoTrackingDeleteEntry(Context.Database.Deleteable<TEntity>().Where(whereLambda)));
        }


        public void Update(Expression<Func<TEntity, TEntity>> columns, Expression<Func<TEntity, bool>> whereLambda)
        {
            var update = Context.Database.Updateable<TEntity>(columns);
            update = update.Where(whereLambda);
            Context.ChangeTracker.EntityEntries.Add(new NoTrackingUpdateEntry(update));
        }

        class NoTrackingUpdateEntry : SqlSugarChangeTracker.NoTrackingEntry
        {

            public NoTrackingUpdateEntry(IUpdateable<TEntity> updateable)
            {
                this.updateable = updateable;
            }
            public override object Entity { get { return typeof(TEntity).FullName; } }
            private IUpdateable<TEntity> updateable;

            public override int ExecuteCommand()
            {
                return updateable.ExecuteCommand();
            }

            internal override string ToSql()
            {
                return updateable.ToSql().ToString();
            }
        } 
        class NoTrackingAddEntry : SqlSugarChangeTracker.NoTrackingEntry
        {
            private readonly IInsertable<TEntity> insertable;

            public NoTrackingAddEntry(IInsertable<TEntity> insertable)
            {
                this.insertable = insertable;
            }
            public override object Entity { get { return typeof(TEntity).FullName; } }
            public override int ExecuteCommand()
            {
                return insertable.ExecuteCommand();
            }

            internal override string ToSql()
            {
                return insertable.ToSql().ToString();
            }
        }
        class NoTrackingDeleteEntry : SqlSugarChangeTracker.NoTrackingEntry
        {

            public NoTrackingDeleteEntry(IDeleteable<TEntity> deleteable)
            {
                this.deleteable = deleteable;
            }

            public override object Entity { get { return typeof(TEntity).FullName; } }
            private IDeleteable<TEntity> deleteable;

            public override int ExecuteCommand()
            {
                return deleteable.ExecuteCommand();
            }

            internal override string ToSql()
            {
                return deleteable.ToSql().ToString();
            }
        }
    }
}
