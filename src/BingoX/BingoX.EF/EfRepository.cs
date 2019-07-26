#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;
using BingoX.Repository;

namespace BingoX.EF
{
    public class EfRepositoryIdentity<T> : EfRepository<T, int>, IRepositoryIdentity<T> where T : class, IIdentityEntity<T>, new()
    {
        public EfRepositoryIdentity(EfDbContext context) : base(context)
        {
        }
        /// <summary>
        /// 指定ID记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T GetId(int id)
        {

            var list = Wrapper.Find();
            list = SetInclude(list);
            return list.FirstOrDefault(n => n.ID == id);
        }
    }
    public class EfRepositorySnowflake<T> : EfRepository<T, long>, IRepositorySnowflake<T> where T : class, ISnowflakeEntity<T>, new()
    {
        public EfRepositorySnowflake(EfDbContext context) : base(context)
        {
        }  /// <summary>
           /// 指定ID记录
           /// </summary>
           /// <typeparam name="T"></typeparam>
           /// <returns></returns>
        public override T GetId(long id)
        {

            var list = Wrapper.Find();
            list = SetInclude(list);
            return list.FirstOrDefault(n => n.ID == id);
        }
    }
    public class EfRepositoryStringID<T> : EfRepository<T, string>, IRepositoryStringID<T> where T : class, IStringEntity<T>, new()
    {
        public EfRepositoryStringID(EfDbContext context) : base(context)
        {
        }  /// <summary>
           /// 指定ID记录
           /// </summary>
           /// <typeparam name="T"></typeparam>
           /// <returns></returns>
        public override T GetId(string id)
        {

            var list = Wrapper.Find();
            list = SetInclude(list);
            return list.FirstOrDefault(n => n.ID == id);
        }
    }
    public class EfRepositoryGuid<T> : EfRepository<T, Guid>, IRepository<T, Guid>, IRepositoryGuid<T> where T : class, IGuidEntity<T>, new()
    {
        public EfRepositoryGuid(EfDbContext context) : base(context)
        {
        }
        /// <summary>
        /// 指定ID记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public override T GetId(Guid id)
        {

            var list = Wrapper.Find();
            list = SetInclude(list);
            return list.FirstOrDefault(n => n.ID == id);
        }
    }
    public class EfRepository<T, pkType> : IRepository<T, pkType>, IRepositoryExpression<T> where T : class, IEntity<T, pkType>, new()
    {
        public EfRepository(EfDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (context is EfDbContext == false) throw new NotSupportedException("不支持此类型");
            Context = context;
            Wrapper = CreateWrapper<T>();
            UnitOfWork = new EfUnitOfWork(Context);
        }
        protected internal EfWrapper<T> Wrapper { get; private set; }
        public EfUnitOfWork UnitOfWork { get; private set; }
        public EfDbContext Context { get; private set; }
        IUnitOfWork IRepository.UnitOfWork { get { return UnitOfWork; } }
        protected internal EfWrapper<TModel> CreateWrapper<TModel>() where TModel : class, new()
        {
            return new EfWrapper<TModel>(Context);
        }

        protected virtual IQueryable<T> SetInclude(IQueryable<T> list)
        {

            return list;
        }




        #region 新增 
        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 </param> 

        /// <returns>操作影响的行数</returns>
        public int Add(T entity)
        {
            Wrapper.Add(entity);

            return 1;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entites">泛型集合</param>

        /// <returns>操作影响的行数</returns>
        public int AddRange(IEnumerable<T> entites)
        {
            var count = entites.Count();
            Wrapper.AddRange(entites);
            return count;
        }



        #endregion

        #region 修改 

        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 </param> 
        /// <returns>操作影响的行数</returns>
        public int Update(T entity)
        {
#if Standard
            var entityEntry = Context.Update(entity);
            entityEntry.State = EntityState.Modified;
#else
            var entityEntry = Context.Entry(entity);
            entityEntry.State = EntityState.Modified;
#endif
            return 1;
        }



        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entitys"> 实体对象集合 </param> 
        /// <param name="isLock"> 是否加锁 </param> 
        /// <returns>操作影响的行数</returns>
        public int UpdateRange(IEnumerable<T> entites)
        {
            var count = entites.Count();
#if Standard

            Context.UpdateRange(entites);
#else
            foreach (var entity in entites)
            {
                var entityEntry = Context.Entry(entity);
                entityEntry.State = EntityState.Modified;
            }
#endif
            return count;

        }
        #endregion

        #region 删除


        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="entity"> 实体对象 </param> 
        /// <returns>操作影响的行数</returns>
        public int Delete(T entity)
        {
#if Standard
            var entityEntry = Context.Remove(entity);

#else
            var entityEntry = Context.Entry(entity);
#endif
            entityEntry.State = EntityState.Deleted;
            return 1;
        }

        /// <summary>
        /// 删除
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <typeparam name="pkType">成员主键类型</typeparam>
        /// <param name="pkArray">待删主键集合</param>

        /// <returns>操作影响的行数</returns>
        public virtual int Delete(pkType[] pkArray)
        {
            int count = 0;
            foreach (var item in pkArray)
            {
                var obj = Wrapper.DbSet.Find(item);
#if Standard
                var entityEntry = Context.Remove(obj);
#else
                var entityEntry = Context.Entry(obj);
#endif
                entityEntry.State = EntityState.Deleted;
                count++;
            }
            return count;
        }

        #endregion

        #region 查询



        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public virtual IList<T> QueryAll()
        {

            var list = Wrapper.QueryAll();
            list = SetInclude(list);
            return list.ToList();
        }

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型)</typeparam>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        public virtual IList<T> Where(Expression<Func<T, bool>> whereLambda)
        {
            var list = Wrapper.QueryAll().Where(whereLambda);
            list = SetInclude(list);
            return list.ToList();
        }



        /// <summary>
        /// 查询前多少条数据
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public virtual IList<T> Take(Expression<Func<T, bool>> whereLambda, int num)
        {
            var list = Wrapper.QueryAll().Where(whereLambda);
            list = SetInclude(list);
            return list.Take(num).ToList();
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <param name="whereLambda">查询表达式</param> 
        /// <returns></returns>
        public virtual T Get(Expression<Func<T, bool>> whereLambda)
        {
            var list = Wrapper.Find();
            list = SetInclude(list);
            return list.FirstOrDefault(whereLambda);

        }
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <param name="whereLambda">查询表达式</param> 
        /// <returns></returns>
        public virtual T GetId(pkType id)
        {
            var obj = Wrapper.DbSet.Find(id);

            return obj;
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <typeparam name="T">泛型参数(集合成员的类型</typeparam>
        /// <param name="whereLambda">查询表达式</param> 
        /// <returns></returns>
        public virtual bool IsExist(Expression<Func<T, bool>> whereLambda)
        {

            return Wrapper.IsExist(whereLambda);
        }

        public virtual IList<T> PageList(ISpecification<T> specification, ref int total)
        {
            var list = Wrapper.QueryAll().Where(specification.ToExpression());
            var orderBy = specification.ToStorExpression();
            if (orderBy != null)
            {
                list = specification.OrderType ? list.OrderBy(orderBy) : list.OrderByDescending(orderBy);
            }
            total = list.Count();
            if (specification.PageSize != 0)
            {
                var skip = specification.PageIndex * specification.PageSize;
                list = list.Skip(skip).Take(specification.PageSize);
            }
            list = SetInclude(list);
            return list.ToList();

        }



        int IRepositoryExpression<T>.Update(Expression<Func<T, T>> update, Expression<Func<T, bool>> where)
        {
            throw new NotImplementedException();
        }

        public virtual int Delete(Expression<Func<T, bool>> where)
        {
            var objects = Wrapper.DbSet.Where(where);
            int count = objects.Count();
            foreach (var obj in objects)
            {
#if Standard
                var entityEntry = Context.Remove(obj);
#else
                var entityEntry = Context.Entry(obj);
#endif

                entityEntry.State = EntityState.Deleted;
            }
            return count;
        }


        #endregion



    }
}
