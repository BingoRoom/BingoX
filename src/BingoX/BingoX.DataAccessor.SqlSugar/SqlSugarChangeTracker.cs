﻿using SqlSugar;
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
