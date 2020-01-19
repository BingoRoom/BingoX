using BingoX.Domain;
using BingoX.Repository;
using SqlSugar;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Linq.Expressions;

namespace BingoX.SqlSugar
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlSugarRepositoryStringID<T> : SqlSugarRepository<T, string>, IRepositoryStringID<T> where T : class, IStringEntity<T>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SqlSugarRepositoryStringID(SqlSugarDbContext context) : base(context)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool Exist(string id)
        {
            return IsExist(n => n.ID == id);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlSugarRepositoryGuid<T> : SqlSugarRepository<T, Guid>, IRepositoryGuid<T> where T : class, IGuidEntity<T>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SqlSugarRepositoryGuid(SqlSugarDbContext context) : base(context)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool Exist(Guid id)
        {
            return IsExist(n => n.ID == id);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlSugarRepositorySnowflake<T> : SqlSugarRepository<T, long>, IRepositorySnowflake<T> where T : class, ISnowflakeEntity<T>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SqlSugarRepositorySnowflake(SqlSugarDbContext context) : base(context)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool Exist(long id)
        {
            return IsExist(n => n.ID == id);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class SqlSugarRepositoryIdentity<T> : SqlSugarRepository<T, int>, IRepositoryIdentity<T> where T : class, IIdentityEntity<T>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SqlSugarRepositoryIdentity(SqlSugarDbContext context) : base(context)
        {
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public override bool Exist(int id)
        {
            return IsExist(n => n.ID == id);
        }
    }
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="pkType"></typeparam>
    public class SqlSugarRepository<T, pkType> : IRepository<T, pkType>, IRepositoryReturn<T>, IRepositorySql<T>, IRepositoryExpression<T> where T : class, IEntity<T, pkType>, new()
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        public SqlSugarRepository(SqlSugarDbContext context)
        {
            if (context == null) throw new ArgumentNullException(nameof(context));
            if (context is SqlSugarDbContext == false) throw new NotSupportedException("构造参数不支持此类型");
            Context = context;
            Wrapper = new SqlSugarWrapper<T>(Context);
            UnitOfWork = new SqlSugarUnitOfWork(Context);
        }
        /// <summary>
        /// 
        /// </summary>
        protected internal SqlSugarWrapper<T> Wrapper { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SqlSugarUnitOfWork UnitOfWork { get; private set; }
        /// <summary>
        /// 
        /// </summary>
        public SqlSugarDbContext Context { get; private set; }
        IUnitOfWork IRepository.UnitOfWork { get { return UnitOfWork; } }
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="TModel"></typeparam>
        /// <returns></returns>
        protected internal SqlSugarWrapper<TModel> CreateWapper<TModel>() where TModel : class, new()
        {
            return new SqlSugarWrapper<TModel>(Context);
        }

        /// <summary>
        /// 
        /// </summary>
        public bool IsLock
        {
            get { return Wrapper.IsLock; }
            set
            {
                Wrapper.IsLock = value;
            }
        }

        #region 新增 
        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"> 实体对象 </param> 
        /// <returns>操作影响的行数</returns>
        public int Add(T entity)
        {
            var result = Wrapper.Insertable(entity).ExecuteCommand();
            return result;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entites">泛型集合</param>
        /// <returns>操作影响的行数</returns>
        public int AddRange(IEnumerable<T> entites)
        {
            IInsertable<T> result = null;
            if (entites is T[]) result = Wrapper.Insertable((T[])entites);
            else if (entites is List<T>) result = Wrapper.Insertable((List<T>)entites);
            else result = Wrapper.Insertable(entites.ToList());
            return result.ExecuteCommand();
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entity"> 实体对象 </param> 
        /// <returns>返回实体</returns>
        public T AddReturnEntity(T entity)
        {
            var result = Wrapper.Insertable(entity).ExecuteReturnEntity();
            return result;
        }

        /// <summary>
        /// 新增
        /// </summary> 
        /// <param name="entity"> 实体对象 </param> 
        /// <returns>返回bool, 并将identity赋值到实体</returns>
        public bool AddReturnBool(T entity)
        {
            var result = Wrapper.Insertable(entity).ExecuteCommandIdentityIntoEntity();
            return result;
        }

        /// <summary>
        /// 新增
        /// </summary>
        /// <param name="entites">泛型集合</param>
        /// <returns>返回bool, 并将identity赋值到实体</returns>
        public bool AddReturnBool(List<T> entites)
        {
            var result = Wrapper.Insertable(entites).ExecuteCommandIdentityIntoEntity();
            return result;
        }

        #endregion

        #region 修改 

        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <param name="entity"> 实体对象 </param> 
        /// <returns>操作影响的行数</returns>
        public int Update(T entity)
        {
            var result = Wrapper.Updateable(entity).ExecuteCommand();
            return result;
        }

        /// <summary>
        /// 修改
        /// </summary>
        /// <param name="update"> 实体对象 </param> 
        /// <param name="where"> 条件 </param> 
        /// <returns>操作影响的行数</returns>
        public int Update(Expression<Func<T, T>> update, Expression<Func<T, bool>> where)
        {
            var result = Wrapper.Updateable().SetColumns(update).Where(where).ExecuteCommand();
            return result;
        }

        /// <summary>
        /// 修改（主键是更新条件）
        /// </summary>
        /// <param name="entites"> 实体对象集合 </param> 
        /// <returns>操作影响的行数</returns>
        public int UpdateRange(IEnumerable<T> entites)
        {
            IUpdateable<T> result = null;
            if (entites is T[]) result = Wrapper.Updateable((T[])entites);
            else if (entites is List<T>) result = Wrapper.Updateable((List<T>)entites);
            else result = Wrapper.Updateable(entites.ToList());
            return result.ExecuteCommand();
        }

        #endregion

        #region 删除

        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="entity"> 实体对象 </param> 
        /// <returns>操作影响的行数</returns>
        public int Delete(T entity)
        {
            var result = Wrapper.Deleteable(entity).ExecuteCommand();
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="where"> 条件 </param> 
        /// <returns>操作影响的行数</returns>
        public int Delete(Expression<Func<T, bool>> where)
        {
            var result = Wrapper.Deleteable().Where(where).ExecuteCommand();
            return result;
        }
        /// <summary>
        /// 删除
        /// </summary>
        /// <param name="pkArray">待删主键集合</param>
        /// <returns>操作影响的行数</returns>
        public int Delete(pkType[] pkArray)
        {
            var result = Wrapper.Deleteable().In(pkArray).ExecuteCommand();
            return result;
        }

        #endregion

        #region 查询

        /// <summary>
        /// 查询
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns></returns>
        public T Query(Expression<Func<T, bool>> whereLambda)
        {
            return Wrapper.Queryable().Where(whereLambda).First();
        }

        /// <summary>
        /// 查询所有记录
        /// </summary>
        /// <returns></returns>
        public IList<T> QueryAll()
        {
            return Wrapper.Queryable().ToList();
        }

        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <returns>实体</returns>
        public IList<T> Where(Expression<Func<T, bool>> whereLambda)
        {
            return Wrapper.Queryable().Where(whereLambda).ToList();
        }
        /// <summary>
        /// 查询集合
        /// </summary>
        /// <param name="sql">sql</param>
        /// <returns>实体</returns>
        public IList<T> Where(string sql)
        {
            return Wrapper.SqlQueryable(sql).ToList();
        }
        /// <summary>
        /// 查询前多少条数据
        /// </summary>
        /// <param name="whereLambda">查询表达式</param>
        /// <param name="num">数量</param>
        /// <returns></returns>
        public IList<T> Take(Expression<Func<T, bool>> whereLambda, int num)
        {
            var datas = Wrapper.Queryable().Where(whereLambda).Take(num).ToList();
            return datas;
        }

        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="whereLambda">查询表达式</param> 
        /// <returns></returns>
        public T Get(Expression<Func<T, bool>> whereLambda)
        {
            var datas = Wrapper.Queryable().Where(whereLambda).First();
            return datas;
        }
        /// <summary>
        /// 查询单条数据
        /// </summary>
        /// <param name="id">泛型参数(集合成员的类型</param>
        /// <returns></returns>
        public T GetId(pkType id)
        {
            var datas = Wrapper.Queryable().In(id).First();
            return datas;
        }
        /// <summary>
        /// 是否存在
        /// </summary>
        /// <param name="whereLambda">查询表达式</param> 
        /// <returns></returns>
        public bool IsExist(Expression<Func<T, bool>> whereLambda)
        {
            var datas = Wrapper.Queryable().Any(whereLambda);
            return datas;
        }
        /// <summary>
        /// 分组查询
        /// </summary>
        /// <param name="specification">查询规格</param>
        /// <param name="total">总记录数</param>
        /// <returns>当前页查询结果集</returns>
        public IList<T> PageList(ISpecification<T> specification, ref int total)
        {
            var datas = Wrapper.PageList(specification.PageIndex, specification.PageSize,
                specification.ToExpression(),
                specification.ToStorExpression(),
                ref total);
            return datas;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public virtual bool Exist(pkType id)
        {
            throw new NotImplementedException();
        }

        #endregion

    }
}
