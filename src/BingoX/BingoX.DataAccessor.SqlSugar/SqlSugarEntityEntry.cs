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

        public virtual object Entity { get; private set; }
        public SqlSugarPropertyValues CurrentValues { get; private set; }

        internal abstract int ExecuteCommand();
    }
    sealed class SqlSugarEntityEntry<TEntity> : SqlSugarEntityEntry where TEntity : class, new()
    {
        private readonly SqlSugarClient database;
        private readonly TEntity entity;

        public SqlSugarEntityEntry(SqlSugarClient database, TEntity entity, SqlSugarPropertyValues currentValues) : base(entity, currentValues)
        {
            this.database = database;
            this.entity = entity;
        }

        internal override int ExecuteCommand()
        {
            switch (State)
            {
                case SqlSugarEntityState.Added:
                    {
                        var insertable = database.Insertable<TEntity>(entity);
                        return insertable.ExecuteCommand();
                    }
                case SqlSugarEntityState.Modified:
                    {
                        var updateable = database.Updateable<TEntity>(entity);
                        return updateable.ExecuteCommand();
                    }

                case SqlSugarEntityState.Deleted:
                    {
                        var deleteable = database.Deleteable<TEntity>(entity);
                        return deleteable.ExecuteCommand();
                    }

                case SqlSugarEntityState.Unchanged:
                    break;
                default:
                    break;
            }
            return 0;
        }
    }
}
