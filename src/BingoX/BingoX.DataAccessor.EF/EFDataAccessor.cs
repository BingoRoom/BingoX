using BingoX.Domain;
using System;
#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#else
using System.Data.Entity;
#endif
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using BingoX.Helper;
using System.Data;
using System.ComponentModel.DataAnnotations.Schema;

namespace BingoX.DataAccessor.EF
{
    public abstract class EFDataAccessor<TEntity> : IEFDataAccessor<TEntity>, IDataAccessor<TEntity>, IDataAccessorInclude<TEntity> where TEntity : class, IEntity<TEntity>
    {
        protected readonly EfDbContext context;

        protected readonly EfUnitOfWork unitOfWork;

        public EFDataAccessor(EfDbContext context)
        {
            this.context = context;
            DbSet = context.Set<TEntity>();
            unitOfWork = new EfUnitOfWork(context);
        }

        protected internal DbSet<TEntity> DbSet { get; private set; }
        public IUnitOfWork UnitOfWork => unitOfWork;
        /// <summary>
        /// 设置外键关联查询的委托
        /// </summary>
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
            return this.GetId(id, SetInclude);
        }

        public abstract bool Exist(object id);

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

#if Standard
            var entityEntry = DbSet.Update(entity); 
            entityEntry.State = EntityState.Modified;
#else
            var entityEntry = context.Entry(entity);
            entityEntry.State = EntityState.Modified;
#endif

        }

        public virtual void UpdateRange(IEnumerable<TEntity> entities)
        {


            foreach (var entity in entities)
            {
                Update(entity);
            }


        }

        public abstract void Delete(object[] pkArray);

        public virtual void Delete(TEntity entity)
        {
#if Standard
            var entityEntry = context.Remove(entity);
#else
            var entityEntry = context.Entry(entity);
#endif
            entityEntry.State = EntityState.Deleted;

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
            return DbSet.AsNoTracking<TEntity>().Where(whereLambda).Any();
        }

        public virtual void Update(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        public virtual void Delete(Expression<Func<TEntity, bool>> whereLambda)
        {
            var list = DbSet.AsNoTracking<TEntity>().Where(whereLambda).ToList<TEntity>();
            foreach (var item in list)
            {
                Delete(item);
            }
        }

        public virtual IList<TEntity> Take(Expression<Func<TEntity, bool>> whereLambda, int num)
        {

            return Take(whereLambda, num, SetInclude);
        }
        public IList<TEntity> WhereTracking(Expression<Func<TEntity, bool>> whereLambda)
        {
            return WhereTracking(whereLambda, SetInclude);
        }

        public IList<TEntity> TakeTracking(Expression<Func<TEntity, bool>> whereLambda, int num)
        {
            return TakeTracking(whereLambda, num, SetInclude);
        }

        public TEntity Get(Expression<Func<TEntity, bool>> whereLambda)
        {

            return Get(whereLambda, SetInclude);
        }
        #region SetInclude

        public virtual IList<TEntity> QueryAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            var query = DbSet.AsNoTracking<TEntity>();
            if (include != null) query = include(query);
            return query.ToList<TEntity>();
        }

        public virtual IList<TEntity> PageList(ISpecification<TEntity> specification, ref int total, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {

            var query = DbSet.AsNoTracking<TEntity>().Where(specification.ToExpression());
            query = OrderBy(query, specification.ToStorExpression());
            total = query.Count();
            if (specification.PageSize == 0) specification.PageSize = 20;
            query = query.Skip(specification.PageIndex * specification.PageSize).Take(specification.PageSize);
            if (include != null) query = include(query);
            return query.ToList();
        }

        public virtual IList<TEntity> Where(Expression<Func<TEntity, bool>> whereLambda, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            var query = DbSet.AsNoTracking<TEntity>().Where(whereLambda);

            if (include != null) query = include(query);
            return query.ToList<TEntity>();
        }

        public virtual IList<TEntity> Take(Expression<Func<TEntity, bool>> whereLambda, int num, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {

            var query = DbSet.AsNoTracking<TEntity>();
            if (whereLambda != null) query = query.Where(whereLambda);
            query = query.Take(num);
            if (include != null) query = include(query);
            return query.ToList();
        }

        public abstract TEntity GetId(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include);




        public IList<TEntity> WhereTracking(Expression<Func<TEntity, bool>> whereLambda, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            var query = DbSet.Where(whereLambda);
            if (include != null) query = include(query);
            return query.ToList();
        }

        public IList<TEntity> TakeTracking(Expression<Func<TEntity, bool>> whereLambda, int num, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            IQueryable<TEntity> query = DbSet;
            if (whereLambda != null) query = query.Where(whereLambda);
            if (include != null) query = include(query);
            return query.Take(num).ToList();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> whereLambda, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            IQueryable<TEntity> query = DbSet;
            if (whereLambda != null) query = query.Where(whereLambda);
            if (include != null) query = include(query);
            return query.FirstOrDefault();
        }

        #endregion

        protected IQueryable<TEntity> OrderBy(IQueryable<TEntity> source, params OrderModelField<TEntity>[] orderByPropertyList)
        {
            if (orderByPropertyList.IsEmpty()) return source;
            IOrderedQueryable<TEntity> orderedQueryable;
            OrderModelField<TEntity> item = orderByPropertyList.First();
            orderedQueryable = item.IsDesc ? source.OrderByDescending(item.OrderPredicates) : source.OrderBy(item.OrderPredicates);
            for (int i = 1; i < orderByPropertyList.Length; i++)
            {
                item = orderByPropertyList[i];
                orderedQueryable = item.IsDesc ? orderedQueryable.ThenBy(item.OrderPredicates) : orderedQueryable.ThenByDescending(item.OrderPredicates);
            }
            return orderedQueryable;
        }
    }
}
