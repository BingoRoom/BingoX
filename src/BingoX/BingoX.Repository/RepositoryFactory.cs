﻿using BingoX.ComponentModel;
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
        static readonly Type[] ConstructorTypes = { typeof(RepositoryContextOptions), typeof(string) };
        public IRepository<TDomain> Create<TDomain>(string name) where TDomain : IEntity<TDomain>
        {

            if (string.IsNullOrEmpty(name)) return serviceProvider.GetService(typeof(IRepository<TDomain>)) as IRepository<TDomain>;

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
            repository = CreateRepository(typeRepository, name);
            return repository as IRepository<TDomain>;
        }

        private object CreateRepository(Type repositoryType, string name)
        {
            var constructorinfo = repositoryType.GetConstructor(ConstructorTypes);
            if (constructorinfo == null) throw new RepositoryException("没有对应的构造函数,不能指定db");
            return constructorinfo.FastInvoke(options, name);
        }

        public IRepository<TDomain, TEntity> Create<TDomain, TEntity>(string name) where TEntity : IEntity<TEntity>
        {
            var repository = CreateRepository(typeof(Repository<,>).MakeGenericType(typeof(TDomain), typeof(TEntity)), name);
            return repository as IRepository<TDomain, TEntity>;
        }

        public TRepository CreateRepository<TRepository>(string name) where TRepository : class, IRepository
        {
            var repository = CreateRepository(typeof(TRepository), name);
            return (TRepository)repository;
        }
    }
}