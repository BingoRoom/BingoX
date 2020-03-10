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
using System.Data;

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
                Open();
                tran = connection.BeginTransaction();
                foreach (var item in cmds)
                {
                    item.Transaction = tran;
                    item.Connection = connection;
                    sql = item.CommandText;
                    item.ExecuteNonQuery();
                }
                tran.Commit();
                Close();
                cmds.Clear();
            }
            catch (Exception ex)
            {
                tran.Rollback();
                Close();
                throw new DataAccessorException($"{sql} 执行失败", ex);
            }

        }
        private void Open()
        {
            if (connection.State != ConnectionState.Open) connection.Open();
        }
        private void Close()
        {
            if (connection.State != ConnectionState.Closed) connection.Close();
        }
        public void AddCommand(string sqlcommand)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            cmds.Add(cmd);
        }

        public object ExecuteScalar(string sqlcommand)
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            Open();
            var scalar = cmd.ExecuteScalar();
            Close();
            return scalar;
        }
        public void Truncate<TEntity>() where TEntity : class, IEntity<TEntity>
        {
            var attr = typeof(TEntity).GetCustomAttribute<CanTruncateAttribute>(true);
            if (attr == null) throw new DataAccessorException($"{nameof(TEntity)}没打CanTruncateAttribute标签，不能执行数据清除操作");
            if (string.IsNullOrEmpty(attr.Tablename)) throw new DataAccessorException($"{nameof(TEntity)}实体的CanTruncateAttribute标签没设置表名，无法执行数据清除操作");
            AddCommand($"Truncate table {attr.Tablename}");
        }
        public void Rollback()
        {
            if (tran != null) tran.Rollback();
            cmds.Clear();
        }


        public T Query<T>(string sqlcommand) where T : class, new()
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            Open();
            var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            T entity = DataReaderHelper.Cast<T>(reader);
            reader.Close();
            Close();
            return entity;
        }

        public IList<T> QueryList<T>(string sqlcommand) where T : class, new()
        {
            var cmd = connection.CreateCommand();
            cmd.CommandText = sqlcommand;
            connection.Open();
            var reader = cmd.ExecuteReader(CommandBehavior.CloseConnection);
            IList<T> entities = DataReaderHelper.GetList<T>(reader);
            reader.Close();
            Close();
            return entities;
        }

        void IUnitOfWork.BeginTransaction()
        {
            throw new NotImplementedException();
        }

        void IUnitOfWork.SaveChanges()
        {
            throw new NotImplementedException();
        }
    }
}
