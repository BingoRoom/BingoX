using SqlSugar;
using System;
using System.Collections.Generic;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarChangeTracker
    {
        private SqlSugarClient database;

        public SqlSugarChangeTracker(SqlSugarClient database)
        {
            this.database = database;
        }

        public SqlSugarEntityEntry[] Entries()
        {
            throw new NotImplementedException();
        }

        public void SaveChanges()
        {
            throw new NotImplementedException();
        }

     
        internal void AddDeleteable<TEntity>(object entity) where TEntity : class, new()
        {
            throw new NotImplementedException();
        }

        internal void AddInsertable (object entity)  
        {
            throw new NotImplementedException();
        }

        internal void AddUpdateable(object entity)  
        {
            throw new NotImplementedException();
        }
    }
    public class SqlSugarEntityEntry
    {
        public SqlSugarEntityState State { get; set; }

        public object Entity { get; private set; }
        public SqlSugarPropertyValues CurrentValues { get; private set; }
        public SqlSugarPropertyValues OriginalValues { get; private set; }
    }
    public class SqlSugarPropertyValues
    {
        public object this[string key]
        {
            get { throw new NotImplementedException(); }
            set
            {
            }
        }

        public string[] Properties { get; internal set; }

        public void SetValues(IDictionary<string, object> changeValues)
        {
            throw new NotImplementedException();
        }
    }
    public enum SqlSugarEntityState
    {
        Added,
        Modified,
        Deleted,
        Unchanged
    }
}
