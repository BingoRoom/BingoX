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
        private readonly IList<SqlSugarEntityEntry> entityEntries = new List<SqlSugarEntityEntry>();

        internal void Clear()
        {
            entityEntries.Clear();
        }

        public SqlSugarEntityEntry[] Entries()
        {
            return entityEntries.ToArray();
        }

        public void RollbackTran()
        {
            database.Ado.RollbackTran();
        }

        public void SaveChanges()
        {
            object entity = null;
            SqlSugarEntityState state = SqlSugarEntityState.Unchanged;
            try
            {
                database.Ado.BeginTran();
                foreach (var item in entityEntries)
                {
                    entity = item.Entity;
                    state = item.State;
                    item.ExecuteCommand();
                }
                database.Ado.CommitTran();
            }
            catch (Exception ex)
            {
                database.Ado.RollbackTran();
                throw new DataAccessorException("执行出错" + state + entity, ex);
            }

        }


        internal void AddDeleteable<T>(T entity) where T : class, new()
        {
            SqlSugarPropertyValues propertyValues = GetPrproety(entity);
            entityEntries.Add(new SqlSugarEntityEntry<T>(database, entity, propertyValues) { State = SqlSugarEntityState.Deleted });
        }
        internal void AddDeleteablePrimaryKeyValue<T>(dynamic[] primaryKeyValues) where T : class, new()
        {

            entityEntries.Add(new SqlSugarEntityEntryDeleteable<T>(database, primaryKeyValues));


        }

        internal void AddInsertable<T>(T entity) where T : class, new()
        {
            SqlSugarPropertyValues propertyValues = GetPrproety(entity);
            entityEntries.Add(new SqlSugarEntityEntry<T>(database, entity, propertyValues) { State = SqlSugarEntityState.Added });

        }

        internal void AddUpdateable<T>(T entity) where T : class, new()
        {
            SqlSugarPropertyValues propertyValues = GetPrproety(entity);

            entityEntries.Add(new SqlSugarEntityEntry<T>(database, entity, propertyValues) { State = SqlSugarEntityState.Modified });
        }

        private SqlSugarPropertyValues GetPrproety(object entity)
        {
            var entityInfo = database.EntityMaintenance.GetEntityInfo(entity.GetType());
            SqlSugarPropertyValues propertyValues = new SqlSugarPropertyValues(entity) { Properties = entityInfo.Columns.Select(n => n.PropertyName).ToArray() };
            return propertyValues;
        }
    }
}
