using SqlSugar;

namespace BingoX.DataAccessor.SqlSugar
{

    sealed class SqlSugarEntityEntryDeleteable<T> : SqlSugarEntityEntry where T : class, new()
    {
        private readonly SqlSugarClient database;
        public SqlSugarEntityEntryDeleteable(SqlSugarClient database, dynamic[] primaryKeyValues) : base(primaryKeyValues, null)
        {
            this.database = database;
            this.primaryKeyValues = primaryKeyValues;
            State = SqlSugarEntityState.Deleted;
        }

        readonly dynamic[] primaryKeyValues;
        internal override int ExecuteCommand()
        {
            return database.Deleteable<T>(primaryKeyValues).ExecuteCommand();
        }
    }
    public abstract class SqlSugarEntityEntry
    {
        public SqlSugarEntityEntry(object entity, SqlSugarPropertyValues currentValues)
        {
            Entity = entity;
            CurrentValues = currentValues;

        }

        public SqlSugarEntityState State { get; set; }

        public object Entity { get; private set; }
        public SqlSugarPropertyValues CurrentValues { get; private set; }

        internal abstract int ExecuteCommand();
    }
    sealed class SqlSugarEntityEntry<T> : SqlSugarEntityEntry where T : class, new()
    {
        private readonly SqlSugarClient database;
        private readonly T entity;

        public SqlSugarEntityEntry(SqlSugarClient database, T entity, SqlSugarPropertyValues currentValues) : base(entity, currentValues)
        {
            this.database = database;
            this.entity = entity;
        }

        internal override int ExecuteCommand()
        {
            switch (State)
            {
                case SqlSugarEntityState.Added:
                    return database.Insertable<T>(entity).ExecuteCommand();
                case SqlSugarEntityState.Modified:
                    return database.Updateable<T>(entity).ExecuteCommand();
                case SqlSugarEntityState.Deleted:
                    return database.Deleteable<T>(entity).ExecuteCommand();
                case SqlSugarEntityState.Unchanged:
                    break;
                default:
                    break;
            }
            return 0;
        }
    }
}
