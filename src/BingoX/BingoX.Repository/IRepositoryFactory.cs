using BingoX.Domain;

namespace BingoX.Repository
{
    public interface IRepositoryFactory
    {
        IRepository<TDomain> Create<TDomain>(string name) where TDomain : IEntity<TDomain>;
        TRepository CreateRepository<TRepository>(string name) where TRepository : class, IRepository;
        IRepository<TDomain, TEntity> Create<TDomain, TEntity>(string name) where TEntity : IEntity<TEntity>  ;
    }
}