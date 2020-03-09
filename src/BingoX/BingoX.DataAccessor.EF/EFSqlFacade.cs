#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Data.SqlClient;
#else

#endif
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Data.Common;
using BingoX.Domain;
using BingoX.Helper;
using System.Linq.Expressions;

namespace BingoX.DataAccessor.EF
{
    public class EFSqlFacade : ISqlFacade
    {
        private DbTransaction tran;

        private readonly DbConnection connection;

        private readonly List<DbCommand> cmds = new List<DbCommand>();

        public EFSqlFacade(EfDbContext context)
        {
            Context = context;
#if Standard
            connection = Context.Database.GetDbConnection();
#else
            connection = Context.Database.Connection;
#endif
        }

        public EfDbContext Context { get; }

        public void Commit()
        {
            string sql = string.Empty;
            try
            {
                tran = connection.BeginTransaction();
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

        public void ExecuteNonQuery(string sqlcommand)
        {

            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            cmds.Add(cmd);
        }

        public object ExecuteScalar(string sqlcommand)
        {

            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            return cmd.ExecuteScalar();
        }
        public void Truncate<TEntity>() where TEntity : class, IEntity<TEntity>
        {
            var attr = typeof(TEntity).GetCustomAttribute<CanTruncateAttribute>(true);
            if (attr == null) throw new DataAccessorException($"{nameof(TEntity)}没打CanTruncateAttribute标签，不能执行数据清除操作");
            if (string.IsNullOrEmpty(attr.Tablename)) throw new DataAccessorException($"{nameof(TEntity)}实体的CanTruncateAttribute标签没设置表名，无法执行数据清除操作");
            ExecuteNonQuery($"Truncate table [{attr.Tablename}]");
        }
        public void Rollback()
        {
            if (tran != null) tran.Rollback();
            cmds.Clear();
        }

        //        public void TransactionExecute(IEnumerable<string> sqlcommands)
        //        {
        //#if Standard
        //            var connection = Context.Database.GetDbConnection();
        //#else
        //            var connection = Context.Database.Connection;
        //#endif
        //            var transaction = connection.BeginTransaction();
        //            string sqlcommand = string.Empty;
        //            try
        //            {
        //                var cmd = connection.CreateCommand();
        //                cmd.Transaction = transaction;
        //                foreach (var item in sqlcommands)
        //                {
        //                    sqlcommand = item;
        //                    cmd.CommandText = sqlcommand;
        //                    cmd.ExecuteNonQuery();
        //                }
        //                transaction.Commit();
        //            }
        //            catch (System.Exception ex)
        //            {
        //                transaction.Rollback();
        //                throw new DataAccessorException("执行语句出错：" + sqlcommand, ex);
        //            }
        //        }

        T ISqlFacade.Query<T>(string sqlcommand)
        {
            throw new NotImplementedException();
        }

        IList<T> ISqlFacade.QueryList<T>(string sqlcommand)
        {
            throw new NotImplementedException();
        }
    }
}
