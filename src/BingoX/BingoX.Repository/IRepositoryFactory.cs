using BingoX.Domain;

namespace BingoX.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<TDomain> Create<TDomain>(string name) where TDomain : IEntity<TDomain>;
        IRepository<TDomain, TEntity> Create<TDomain, TEntity>(string name) where TEntity : IEntity<TEntity>;
    }
}