using BingoX.Domain;
using BingoX.Helper;
using System;

namespace BingoX.Repository
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly RepositoryContextOptions options;

        public RepositoryFactory(RepositoryContextOptions options)
        {
            this.options = options;
        }

        static readonly Type[] ConstructorTypes = { typeof(RepositoryContextOptions), typeof(string) };
        public IRepository<TDomain> Create<TDomain>(string name) where TDomain : IEntity<TDomain>, IDomainEntry
        {
            var repository = CreateRepository(typeof(Repository<>).MakeGenericType(typeof(TDomain)), name);
            return repository as IRepository<TDomain>;
        }

        private object CreateRepository(Type repositoryType, string name)
        {

            var constructorinfo = repositoryType.GetConstructor(ConstructorTypes);
            return constructorinfo.FastInvoke(options, name);
        }

        public IRepository<TDomain, TEntity> Create<TDomain, TEntity>(string name) where TEntity : IEntity<TEntity> where TDomain : IDomainEntry
        {
            var repository = CreateRepository(typeof(Repository<,>).MakeGenericType(typeof(TDomain), typeof(TEntity)), name);
            return repository as IRepository<TDomain, TEntity>;
        }
    }
}