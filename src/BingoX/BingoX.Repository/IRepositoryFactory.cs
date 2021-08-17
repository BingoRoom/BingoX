using BingoX.Domain;
using System;

namespace BingoX.Repository
{
    public interface IRepositoryFactory
    {

        TRepository CreateRepository<TRepository>() where TRepository : Repository;
        IRepository<TDomain> Create<TDomain>() where TDomain : IEntity<TDomain>;
        IRepository<TDomain, TEntity> Create<TDomain, TEntity>() where TEntity : IEntity<TEntity>;

        void AddRepository(Type baseType, Type implementedType);
    }
}