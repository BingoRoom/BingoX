#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Data.SqlClient;
#else

#endif
using BingoX.Repository;
using System.Collections.Generic;

namespace BingoX.EF
{
    public class EFSqlFacade : ISqlFacade
    {


        public EFSqlFacade(EfDbContext context)
        {
            this.Context = context;
        }
        public EfDbContext Context { get; private set; }

        public int ExecuteNonQuery(string sqlcommand)
        {
#if Standard
            var connection = Context.Database.GetDbConnection();
#else
            var connection = Context.Database.Connection;
#endif
            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            return cmd.ExecuteNonQuery();
        }




        public object ExecuteScalar(string sqlcommand)
        {
#if Standard
            var connection = Context.Database.GetDbConnection();
#else
            var connection = Context.Database.Connection;
#endif
            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            return cmd.ExecuteScalar();
        }

        public void TransactionExecute(IEnumerable<string> sqlcommands)
        {
#if Standard
            var connection = Context.Database.GetDbConnection();
#else
            var connection = Context.Database.Connection;
#endif
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
