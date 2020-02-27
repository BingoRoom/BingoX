using BingoX.Domain;
using System;
using System.Collections.Generic;

namespace BingoX.DataAccessor
{
    public interface IDataAccessor<TEntity> where TEntity : class, IEntity<TEntity>
    {
        void Add(TEntity t);
        TEntity GetId(object id);////more Remove   Qeuery Update PageList WhereByLambda GetByLambda     
         
   //     IList<TEntity> PageList(ISpecification<TEntity> specification, ref int total);
        void BeginTran();
        void Commit();
        void Rollback();
    }
}
