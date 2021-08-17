#if Standard 
#else

#endif
using System;
using System.Collections.Generic;
using System.Text;
using System.Data.Common;
using BingoX.Domain;
using BingoX.Helper;
using System.Data;
using System.Linq.Expressions;
using SqlSugar;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarSqlFacade : ISqlFacade
    {


        private readonly List<IDbCommand> cmds = new List<IDbCommand>();

        public SqlSugarSqlFacade(SqlSugarDbContext context)
        {
            Context = context;


        }

        public SqlSugarDbContext Context { get; }

        public void Commit()
        {
            var connection = Context.Database.Ado.Connection;
            var tran = connection.BeginTransaction(IsolationLevel.ReadCommitted);
            if (connection.State != ConnectionState.Open) connection.Open();
            string sql = string.Empty;
            try
            {

                foreach (var item in cmds)
                {
                    item.Transaction = tran;
                    item.Connection = connection;
                    sql = item.CommandText;
                    item.ExecuteNonQuery();
                }
                tran.Commit();
                cmds.Clear();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                throw new DataAccessorException($"{sql} 执行失败", ex);
            }

        }

        public void AddCommand(string sqlcommand)
        {

            var cmd = Context.Database.Ado.Connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            cmds.Add(cmd);
        }

        public object ExecuteScalar(string sqlcommand)
        {

            return Context.Database.Ado.GetScalar(sqlcommand);

        }
        public void Truncate<TEntity>() where TEntity : class, IEntity<TEntity>
        {
            var attr = typeof(TEntity).GetCustomAttribute<CanTruncateAttribute>(true);
            if (attr == null) throw new DataAccessorException($"{nameof(TEntity)}没打CanTruncateAttribute标签，不能执行数据清除操作");
            if (string.IsNullOrEmpty(attr.Tablename)) throw new DataAccessorException($"{nameof(TEntity)}实体的CanTruncateAttribute标签没设置表名，无法执行数据清除操作");
            Context.Database.Ado.ExecuteCommand($"Truncate table {attr.Tablename}");
       
        }


        T ISqlFacade.Query<T>(string sqlcommand)
        {
            return Context.Database.SqlQueryable<T>(sqlcommand).First();
        }

        public IList<T> QueryList<T>(string sqlcommand) where T : class, new()
        {
            return Context.Database.SqlQueryable<T>(sqlcommand).ToList();
        }
        void IUnitOfWork.BeginTransaction()
        {

        }
        void IUnitOfWork.Rollback()
        {

        }
        void IUnitOfWork.SaveChanges()
        {

        }

    }
}
