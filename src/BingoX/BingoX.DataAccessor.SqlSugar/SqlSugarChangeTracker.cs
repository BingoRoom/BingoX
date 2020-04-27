using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace BingoX.DataAccessor.SqlSugar
{

    public class SqlSugarChangeTracker
    {



        public SqlSugarChangeTracker(SqlSugarClient database)
        {
            this.database = database;
        }
        private readonly SqlSugarClient database;
        internal readonly IList<ISqlSugarEntityEntry> EntityEntries = new List<ISqlSugarEntityEntry>();
      
        internal abstract class NoTrackingEntry: ISqlSugarEntityEntry
        {
            public abstract object Entity { get;  }

            public SqlSugarEntityState State { get { return SqlSugarEntityState.NoTracking; } }

          

            public abstract int ExecuteCommand();

            internal abstract string ToSql();
        }

        internal void Clear()
        {
            EntityEntries.Clear();
        }

        public SqlSugarEntityEntry[] Entries()
        {
            return EntityEntries.OfType<SqlSugarEntityEntry>().ToArray();
        }




        internal void AddDeleteable<T>(T entity) where T : class, new()
        {
            SqlSugarPropertyValues propertyValues = GetPrproety(entity);
            EntityEntries.Add(new SqlSugarEntityEntry<T>(database, entity, propertyValues) { State = SqlSugarEntityState.Deleted });
        }
        internal void AddDeleteablePrimaryKeyValue<T>(dynamic[] primaryKeyValues) where T : class, new()
        {

            EntityEntries.Add(new SqlSugarEntityEntryDeleteable<T>(database, primaryKeyValues));


        }

        internal void AddInsertable<T>(T entity) where T : class, new()
        {
            SqlSugarPropertyValues propertyValues = GetPrproety(entity);
            EntityEntries.Add(new SqlSugarEntityEntry<T>(database, entity, propertyValues) { State = SqlSugarEntityState.Added });

        }

        internal void AddUpdateable<T>(T entity) where T : class, new()
        {
            SqlSugarPropertyValues propertyValues = GetPrproety(entity);

            EntityEntries.Add(new SqlSugarEntityEntry<T>(database, entity, propertyValues) { State = SqlSugarEntityState.Modified });
        }

        private SqlSugarPropertyValues GetPrproety(object entity)
        {
            var entityInfo = database.EntityMaintenance.GetEntityInfo(entity.GetType());
            SqlSugarPropertyValues propertyValues = new SqlSugarPropertyValues(entity) { Properties = entityInfo.Columns.Select(n => n.PropertyName).ToArray() };
            return propertyValues;
        }
    }
}
