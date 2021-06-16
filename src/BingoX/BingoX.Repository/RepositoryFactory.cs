using BingoX.ComponentModel;
using BingoX.Domain;
using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BingoX.Repository
{
    public class RepositoryFactory : IRepositoryFactory
    {
        private readonly RepositoryContextOptions options;
        private readonly IList<AssemblyScanResult> repositoryTypes;
        private readonly IServiceProvider serviceProvider;
        public RepositoryFactory(IServiceProvider serviceProvider, RepositoryContextOptions options, AssemblyScanResult[] repositoryTypes)
        {
            this.options = options;
            this.repositoryTypes = new List<AssemblyScanResult>(repositoryTypes);
            this.serviceProvider = serviceProvider;
        }
        public void AddRepository(Type baseType, Type implementedType)
        {
            if (baseType is null)
            {
                throw new ArgumentNullException(nameof(baseType));
            }

            if (implementedType is null)
            {
                throw new ArgumentNullException(nameof(implementedType));
            }
            if (implementedType.IsAbstract) throw new Exception($"{implementedType} 不能是抽像类");
            if (!implementedType.IsAssignableFrom(baseType)) throw new Exception($"{implementedType} 不是继承于 {implementedType}");
            repositoryTypes.Add(new AssemblyScanResult(baseType, implementedType));
        }
        static readonly Type[] ConstructorTypes = { typeof(RepositoryContextOptions) };
        public IRepository<TDomain> Create<TDomain>() where TDomain : IEntity<TDomain>
        {


            var typeRepository = typeof(IRepository<>).MakeGenericType(typeof(TDomain));
            var entityScope = repositoryTypes.FirstOrDefault(n => n.BaseType == typeRepository);
            object repository = null;
            if (entityScope != null)
            {
                typeRepository = entityScope.ImplementedType;
            }
            else
            {
                typeRepository = typeof(Repository<>).MakeGenericType(typeof(TDomain));
            }
            repository = CreateRepository(typeRepository);
            return repository as IRepository<TDomain>;
        }

        private object CreateRepository(Type repositoryType)
        {
            var constructorinfo = repositoryType.GetConstructor(ConstructorTypes);
            if (constructorinfo == null) throw new RepositoryException("没有对应的构造函数,不能指定db");
            return constructorinfo.FastInvoke(options);
        }

        public IRepository<TDomain, TEntity> Create<TDomain, TEntity>() where TEntity : IEntity<TEntity>
        {
            var repository = CreateRepository(typeof(Repository<,>).MakeGenericType(typeof(TDomain), typeof(TEntity)));
            return repository as IRepository<TDomain, TEntity>;
        }

        public TRepository CreateRepository<TRepository>() where TRepository : Repository
        {
            var repository = CreateRepository(typeof(TRepository));
            return (TRepository)repository;
        }
    }
}