using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BingoX.Helper;
using System.Data;
using SqlSugar;

namespace BingoX.DataAccessor.SqlSugar
{
    public abstract class SqlSugarDataAccessor<TEntity> : IDataAccessor<TEntity> where TEntity : class, IEntity<TEntity>, new()
    {
        protected readonly SqlSugarDbContext context;

        protected readonly SqlSugarUnitOfWork unitOfWork;
        protected readonly SqlSugarDbSet<TEntity> DbSet;
        public SqlSugarDataAccessor(SqlSugarDbContext context)
        {
            this.context = context;

            DbSet = context.Set<TEntity>();
            unitOfWork = new SqlSugarUnitOfWork(context);
        }


        public IUnitOfWork UnitOfWork => unitOfWork;

        Func<IQueryable<TEntity>, IQueryable<TEntity>> IDataAccessor<TEntity>.SetInclude { get; set; }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);

        }

        public virtual void AddRange(IEnumerable<TEntity> entites)
        {
            DbSet.AddRange(entites);

        }

        public abstract TEntity GetId(object id);

        public virtual bool Exist(object id)
        {
            var entity = GetId(id);
            return entity != null;
        }

        public virtual IList<TEntity> QueryAll()
        {
            var query = DbSet.AsQueryable();

            return query.ToList();
        }

        public virtual IList<TEntity> PageList(ISpecification<TEntity> specification, ref int total)
        {
            var query = DbSet.AsQueryable().Where(specification.ToExpression());
            query = OrderBy(query, specification.ToStorExpression());


            return query.ToPageList(specification.PageIndex, specification.PageSize, ref total);
        }

        public virtual void Update(TEntity entity)
        {

            DbSet.Update(entity);

        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {

            DbSet.UpdateRange(entities);


        }

        public virtual void Delete(object[] pkArray)
        {
            DbSet.RemoveRangePrimaryKeys(pkArray);
        }

        public virtual void Delete(TEntity entity)
        {
            DbSet.Remove(entity);

        }


        public void Commit()
        {
            unitOfWork.Commit();
        }

        public void Rollback()
        {
            unitOfWork.Rollback();
        }

        public virtual IList<TEntity> Where(Expression<Func<TEntity, bool>> whereLambda)
        {

            return DbSet.AsQueryable().Where(whereLambda).ToList();
        }

        public virtual bool Exist(Expression<Func<TEntity, bool>> whereLambda)
        {
            return DbSet.AsQueryable().Where(whereLambda).Any();
        }

        public virtual void Update(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> whereLambda)
        {
            var updetefunc = update.Compile();
            var list = DbSet.AsQueryable().Where(whereLambda).ToList();
            foreach (var item in list)
            {
                Update(updetefunc(item));
            }

        }

        public virtual void Delete(Expression<Func<TEntity, bool>> whereLambda)
        {
            var list = DbSet.AsQueryable().Where(whereLambda).ToList();
            DbSet.RemoveRange(list);

        }

        public virtual IList<TEntity> Take(Expression<Func<TEntity, bool>> whereLambda, int num)
        {
            var query = DbSet.AsQueryable().Where(whereLambda);
            return query.Take(num).ToList();
        }





        protected ISugarQueryable<TEntity> OrderBy(ISugarQueryable<TEntity> source, params OrderModelField<TEntity>[] orderByPropertyList)
        {
            if (orderByPropertyList.IsEmpty()) return source;
            ISugarQueryable<TEntity> orderedQueryable;
            OrderModelField<TEntity> item = orderByPropertyList.First();

            orderedQueryable = source.OrderBy(item.OrderPredicates, item.IsDesc ? OrderByType.Desc : OrderByType.Desc);
            for (int i = 1; i < orderByPropertyList.Length; i++)
            {
                item = orderByPropertyList[i];
                orderedQueryable = orderedQueryable.OrderBy(item.OrderPredicates, item.IsDesc ? OrderByType.Desc : OrderByType.Desc);
            }
            return orderedQueryable;
        }



        public TEntity Get(Expression<Func<TEntity, bool>> whereLambda)
        {
            return DbSet.AsQueryable().First(whereLambda);
        }
    }
}
