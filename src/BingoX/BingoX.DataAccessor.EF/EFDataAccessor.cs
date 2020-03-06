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

        public virtual int Add(TEntity entity)
        {
            DbSet.Add(entity);
            return 1;
        }

        public virtual int AddRange(IEnumerable<TEntity> entites)
        {
            DbSet.AddRange(entites);
            return entites.Count();
        }

        public abstract TEntity GetId(object id);

        public abstract bool Exist(object id);

        public virtual IList<TEntity> QueryAll()
        {
            var query = DbSet.AsNoTracking<TEntity>();
            if (SetInclude != null) query = SetInclude(query);
            return query.ToList<TEntity>();
        }

        public virtual IList<TEntity> PageList(ISpecification<TEntity> specification, ref int total)
        {
            var query = DbSet.AsNoTracking<TEntity>().Where(specification.ToExpression());
            query = OrderBy(query, specification.ToStorExpression());
            if (SetInclude != null) query = SetInclude(query);
            total = query.Count();
            if (specification.PageSize == 0) specification.PageSize = 20;
            return query.Skip(specification.PageIndex * specification.PageSize).Take(specification.PageSize).ToList();
        }

        public virtual int Update(TEntity entity)
        {
#if Standard
            var entityEntry = context.Update(entity);
#else
            var entityEntry = context.Entry(entity);
#endif
            entityEntry.State = EntityState.Modified;
            return 1;
        }

        public virtual int UpdateRange(IEnumerable<TEntity> entities)
        {
            var count = entities.Count();
#if Standard
            context.UpdateRange(entities);
#else
            foreach (var entity in entities)
            {
                Update(entity);
            }
#endif
            return count;
        }

        public abstract int Delete(object[] pkArray);

        public virtual int Delete(TEntity entity)
        {
#if Standard
            var entityEntry = context.Remove(entity);
#else
            var entityEntry = context.Entry(entity);
#endif
            entityEntry.State = EntityState.Deleted;
            return 1;
        }

        public void BeginTran(IsolationLevel level = IsolationLevel.ReadCommitted)
        {
            unitOfWork.BeginTran(level);
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
            var query = DbSet.AsNoTracking<TEntity>().Where(whereLambda);
            if (SetInclude != null) query = SetInclude(query);
            return query.ToList();
        }

        public virtual bool Exist(Expression<Func<TEntity, bool>> whereLambda)
        {
            return DbSet.AsNoTracking<TEntity>().Where(whereLambda).Any();
        }

        public virtual int Update(Expression<Func<TEntity, TEntity>> update, Expression<Func<TEntity, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        public virtual int Delete(Expression<Func<TEntity, bool>> whereLambda)
        {
            var list = DbSet.AsNoTracking<TEntity>().Where(whereLambda).ToList<TEntity>();
            list.Select(n => Delete(n));
            return list.Count();
        }

        public virtual IList<TEntity> Take(Expression<Func<TEntity, bool>> whereLambda, int num)
        {
            var query = DbSet.AsNoTracking<TEntity>();
            if (whereLambda != null) query = query.Where(whereLambda);
            if (SetInclude != null) query = SetInclude(query);
            return query.Take(num).ToList();
        }

        public virtual IList<TEntity> QueryAll(Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            if (include == null) return QueryAll();
            var query = include(DbSet.AsNoTracking<TEntity>());
            return query.ToList<TEntity>();
        }

        public virtual IList<TEntity> PageList(ISpecification<TEntity> specification, ref int total, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            if (include == null) return PageList(specification, ref total);
            var query = DbSet.AsNoTracking<TEntity>().Where(specification.ToExpression());
            query = OrderBy(query, specification.ToStorExpression());
            query = include(query);
            total = query.Count();
            if (specification.PageSize == 0) specification.PageSize = 20;
            return query.Skip(specification.PageIndex * specification.PageSize).Take(specification.PageSize).ToList();
        }

        public virtual IList<TEntity> Where(Expression<Func<TEntity, bool>> whereLambda, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            if (include == null) return Where(whereLambda);
            var query = include(DbSet.AsNoTracking<TEntity>().Where(whereLambda));
            return query.ToList<TEntity>();
        }

        public virtual IList<TEntity> Take(Expression<Func<TEntity, bool>> whereLambda, int num, Func<IQueryable<TEntity>, IQueryable<TEntity>> include)
        {
            if (include == null) return Take(whereLambda, num);
            var query = DbSet.AsNoTracking<TEntity>();
            if (whereLambda != null) query = query.Where(whereLambda);
            query = include(query);
            return query.Take(num).ToList();
        }

        public abstract TEntity GetId(object id, Func<IQueryable<TEntity>, IQueryable<TEntity>> include);

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

        public IList<TEntity> WhereTracking(Expression<Func<TEntity, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }

        public IList<TEntity> TakeTracking(Expression<Func<TEntity, bool>> whereLambda, int num)
        {
            throw new NotImplementedException();
        }

        public TEntity Get(Expression<Func<TEntity, bool>> whereLambda)
        {
            throw new NotImplementedException();
        }
    }
}
