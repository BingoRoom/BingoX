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
        protected internal readonly SqlSugarDbContext Context;

        protected readonly SqlSugarUnitOfWork unitOfWork;
        protected readonly SqlSugarDbSet<TEntity> DbSet;
        public SqlSugarDataAccessor(SqlSugarDbContext context)
        {
            this.Context = context;

            DbSet = context.Set<TEntity>();
            unitOfWork = new SqlSugarUnitOfWork(context);
        }


        public IUnitOfWork UnitOfWork => unitOfWork;

        public Func<IQueryable<TEntity>, IQueryable<TEntity>> SetInclude { get; set; }

        public virtual void Add(TEntity entity)
        {
            DbSet.Add(entity);

        }

        public virtual void AddRange(IEnumerable<TEntity> entites)
        {
            DbSet.AddRange(entites);

        }

        public virtual TEntity GetId(object id)
        {
            return GetId(id, SetInclude);
        }

        public virtual bool Exist(object id)
        {
            var entity = GetId(id);
            return entity != null;
        }

        public virtual IList<TEntity> QueryAll()
        {
            return QueryAll(SetInclude);
        }

        public virtual IList<TEntity> PageList(ISpecification<TEntity> specification, ref int total)
        {
            return PageList(specification, ref total, SetInclude);
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
            return Where(whereLambda, SetInclude);
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
            return Take(whereLambda, num, SetInclude);
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
            return Get(whereLambda, SetInclude);
        }

        #region SetInclude

        public virtual IList<TEntity> QueryAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            var query = DbSet.AsQueryable().ToList();
            if (include == null) return query;
            return include(query.AsQueryable()).ToList();

        }

        public virtual IList<TEntity> PageList(ISpecification<TEntity> specification, ref int total, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {

            var query = DbSet.AsQueryable().Where(specification.ToExpression());
            query = OrderBy(query, specification.ToStorExpression());
            total = query.Count();
            if (specification.PageSize == 0) specification.PageSize = 20;
            query = query.Skip(specification.PageIndex * specification.PageSize).Take(specification.PageSize);
            var list = query.ToList();
            if (include == null) return list;
            return include(list.AsQueryable()).ToList();
        }

        public virtual IList<TEntity> Where(Expression<Func<TEntity, bool>> whereLambda, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            var query = DbSet.AsQueryable().Where(whereLambda);
            var list = query.ToList();
            if (include == null) return list;
            return include(list.AsQueryable()).ToList();
        }

        public virtual IList<TEntity> Take(Expression<Func<TEntity, bool>> whereLambda, int num, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {

            var query = DbSet.AsQueryable();
            if (whereLambda != null) query = query.Where(whereLambda);
            query = query.Take(num);
            var list = query.ToList();
            if (include == null) return list;
            return include(list.AsQueryable()).ToList();
        }

        public abstract TEntity GetId(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include);




        public IList<TEntity> WhereTracking(Expression<Func<TEntity, bool>> whereLambda, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            var query = DbSet.AsQueryable().Where(whereLambda);
            var list = query.ToList();
            if (include == null) return list;
            return include(list.AsQueryable()).ToList();
        }

        public IList<TEntity> TakeTracking(Expression<Func<TEntity, bool>> whereLambda, int num, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            var query = DbSet.AsQueryable();
            if (whereLambda != null) query = query.Where(whereLambda);
            query = query.Take(num);
            var list = query.ToList();
            if (include == null) return list;
            return include(list.AsQueryable()).ToList();

        }

        public TEntity Get(Expression<Func<TEntity, bool>> whereLambda, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            var query = DbSet.AsQueryable();
            if (whereLambda != null) query = query.Where(whereLambda);
            var entity = query.First();

            if (include == null) return entity;
            return include(new[] { entity }.AsQueryable()).FirstOrDefault();
        }

        public int Count(Expression<Func<TEntity, bool>> whereLambda)
        {
            return DbSet.AsQueryable().Where(whereLambda).Count();
        }


        public SqlSugarNoTrackingDataAccessor<TEntity> AsNoTracking()
        {

            return new SqlSugarNoTrackingDataAccessor<TEntity>(this);
        }

        INoTrackingDataAccessor<TEntity> IDataAccessor<TEntity>.AsNoTracking()
        {
            return AsNoTracking();
        }
        #endregion

    }
}
