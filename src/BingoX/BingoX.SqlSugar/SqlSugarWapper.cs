using SqlSugar;
using System;
using System.Collections.Generic;
using System.Linq.Expressions;

namespace BingoX.SqlSugar
{
    public class SqlSugarWrapper<T> where T : class ,new()
    {
        public SqlSugarWrapper(SqlSugarDbContext context)
        {
            Context = context;
        }
        public SqlSugarDbContext Context { get; private set; }
        public bool IsLock { get; set; }

        #region wapper

        public IInsertable<T> Insertable(List<T> entitys)
        {
            var insertable = Context.Client.Insertable(entitys);
            insertable = UpdLock(insertable);
            return insertable;
        }
        public IInsertable<T> Insertable(T[] entitys)
        {
            var insertable = Context.Client.Insertable(entitys);
            insertable = UpdLock(insertable);
            return insertable;
        }
        public IInsertable<T> Insertable(T entity)
        {
            var insertable = Context.Client.Insertable(entity);
            insertable = UpdLock(insertable);
            return insertable;
        }

        public IUpdateable<T> Updateable(T[] entities)
        {
            var updateable = Context.Client.Updateable(entities);
            updateable = UpdLock(updateable);
            return updateable;
        }

        public IUpdateable<T> Updateable(T entity)
        {
            var updateable = Context.Client.Updateable(entity);
            updateable = UpdLock(updateable);
            return updateable;
        }
        public IUpdateable<T> Updateable()
        {
            var updateable = Context.Client.Updateable<T>();
            updateable = UpdLock(updateable);
            return updateable;
        }
        public IUpdateable<T> Updateable(List<T> entites)
        {
            var updateable = Context.Client.Updateable(entites);
            updateable = UpdLock(updateable);
            return updateable;
        }

        public IDeleteable<T> Deleteable()
        {
            var deleteable = Context.Client.Deleteable<T>();
            deleteable = UpdLock(deleteable);
            return deleteable;
        }
        public IDeleteable<T> Deleteable(T entity)
        {
            var deleteable = Context.Client.Deleteable<T>(entity);
            deleteable = UpdLock(deleteable);
            return deleteable;
        }

        private IDeleteable<T> UpdLock(IDeleteable<T> deleteable)
        {
            if (IsLock) deleteable = deleteable.With(SqlWith.UpdLock);
            return deleteable;
        }

        private IInsertable<T> UpdLock(IInsertable<T> insertable)
        {
            if (IsLock) insertable = insertable.With(SqlWith.UpdLock);
            return insertable;
        }

        private IUpdateable<T> UpdLock(IUpdateable<T> updateable)
        {
            if (IsLock) updateable = updateable.With(SqlWith.UpdLock);
            return updateable;
        }

        public ISugarQueryable<T> Queryable()
        {
            return Context.Client.Queryable<T>().With(SqlWith.NoLock);
        }

        public ISugarQueryable<T> SqlQueryable(string sql)
        {
            return Context.Client.SqlQueryable<T>(sql).With(SqlWith.NoLock);
        }

        public IList<T> PageList(int pageIndex, int pageSize, Expression<Func<T, bool>> whereExpression, Expression<Func<T, object>> orderbyExpression, bool isdesc, ref int total)
        {
            var quey = Queryable().Where(whereExpression);
            if (orderbyExpression != null) quey = quey.OrderBy(orderbyExpression, isdesc ? OrderByType.Desc : OrderByType.Asc);
            if (pageIndex > -1 && pageSize != 0)
            {
                return quey.ToPageList(pageIndex, pageSize, ref total);
            }
            else
            {
                total = quey.Count();
                var list = quey.ToList();
                return list;
            }
        }

        #endregion
    }
}
