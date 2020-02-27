using BingoX.Domain;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.DataAccessor.EF
{
    //public class EFDataAccessorFactory : IDataAccessorFactory
    //{
    //    public IServiceProvider GetServiceProvider()
    //    {
    //        throw new NotImplementedException();
    //    }

    //    IDataAccessor<TEntity> IDataAccessorFactory.GetDataAccessor<TEntity>()
    //    {
    //        throw new NotImplementedException();
    //    }
    //}
    public abstract class EFDataAccessor<TEntity> : IDataAccessor<TEntity> where TEntity : class, IEntity<TEntity>
    {
        public virtual void Add(TEntity t)
        {
            //ef dbcontent Add
        }

        public virtual void BeginTran()
        {
            throw new NotImplementedException();
        }

        public virtual void Commit()
        {
            throw new NotImplementedException();
        }

        public virtual void Rollback()
        {
            throw new NotImplementedException();
        }

        TEntity IDataAccessor<TEntity>.GetId(object id)
        {
            throw new NotImplementedException();
        }
    }
    public class EFGuidDataAccessor<TEntity> : EFDataAccessor<TEntity>, IDataAccessor<TEntity> where TEntity : class, IGuidEntity<TEntity>
    {
        public virtual TEntity GetId(Guid id)
        {
            //ef dbcontent DbSet GetId  
            throw new NotImplementedException();
        }
        TEntity IDataAccessor<TEntity>.GetId(object id)
        {
            return GetId((Guid)id);
        }
    }
    public class EFSnowflakeDataAccessor<TEntity> : EFDataAccessor<TEntity>, IDataAccessor<TEntity> where TEntity : class, ISnowflakeEntity<TEntity>
    {
        public virtual TEntity GetId(long id)
        {
            throw new NotImplementedException();
        }
        TEntity IDataAccessor<TEntity>.GetId(object id)
        {
            return GetId((long)id);
        }
    }
}
