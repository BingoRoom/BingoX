using BingoX.Repository;
using System.Collections.Generic;

namespace BingoX.SqlSugar
{
    public class SqlSugarSqlFacade : ISqlFacade
    {


        public SqlSugarSqlFacade(SqlSugarDbContext context)
        {
            this.Context = context;
        }
        public SqlSugarDbContext Context { get; private set; }

        public int ExecuteNonQuery(string sqlcommand)
        {
            var connection = Context.Client.Ado.Connection;
            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            return cmd.ExecuteNonQuery();
        }

        public object ExecuteScalar(string sqlcommand)
        {
            var connection = Context.Client.Ado.Connection;
            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            return cmd.ExecuteScalar();
        }
        public void TransactionExecute(IEnumerable<string> sqlcommands)
        {
            var connection = Context.Client.Ado.Connection;
            var transaction = connection.BeginTransaction();
            string sqlcommand = string.Empty;
            try
            {
                var cmd = connection.CreateCommand();
                cmd.Transaction = transaction;
                foreach (var item in sqlcommands)
                {
                    sqlcommand = item;
                    cmd.CommandText = sqlcommand;
                    cmd.ExecuteNonQuery();
                }
                transaction.Commit();
            }
            catch (System.Exception ex)
            {
                transaction.Rollback();
                throw new RepositoryOperationException("执行语句出错：" + sqlcommand, ex);
            }

        }
    }
}
