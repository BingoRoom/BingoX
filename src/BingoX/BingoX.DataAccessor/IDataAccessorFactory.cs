using BingoX.Domain;
using System;
using System.Linq;

namespace BingoX.DataAccessor
{
    public interface IDataAccessorFactory
    {
        IDataAccessor<TEntity> GetDataAccessor<TEntity>() where TEntity : class, IEntity<TEntity>;
        void AddIndluce<TEntity>(Func<TEntity, IQueryable<TEntity>> include) where TEntity : class, IEntity<TEntity>;
        // void AddIndluce(Type typeclass);
        System.IServiceProvider GetServiceProvider();
    }
}
