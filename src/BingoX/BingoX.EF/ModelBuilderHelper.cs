#if Standard
using Microsoft.EntityFrameworkCore;
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using System.Reflection;
using BingoX.Helper;
using System.Linq;
using System;

namespace BingoX.EF
{
    public static class ModelBuilderHelper
    {
#if Standard
        public static void ModelCreating(this ModelBuilder modelBuilder, ModelMappingOption option)
        {
            if (option == null || option.AssemblEntity == null || option.AssemblyMappingConfig == null) return;
       
            //System.Type[] entityclass = GetEntities(option.AssemblEntity, typeof(Domain.IEntity<,>));
            //var dataclass = GetConfies(entityAssemblyName, entityclass);
            //var method = typeof(ModelBuilder).GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(n => n.Name == "ApplyConfiguration" && n.GetParameters()[0].ParameterType.Name == "IEntityTypeConfiguration`1");
            //foreach (var item in dataclass)
            //{
            //    var genericTypes = item.BaseType;
            //    var obj = item.ImplementedType.CreateInstance();
            //    var addmethod = method.MakeGenericMethod(genericTypes.GenericTypeArguments);
            //    addmethod.FastInvoke(modelBuilder, obj);
            //}
        }

        //private static AssemblyScanResult[] GetConfies(string configAssemblyName, Type[] entityclass)
        //{
        //    var assembly = Assembly.Load(configAssemblyName);
        //    var genericType = typeof(IEntityTypeConfiguration<>);
        //    return assembly.GetImplementedClassWithOneArgumentGenericType(genericType, entityclass);
        //}
#endif
        private static Type[] GetEntities(string entityAssemblyName, Type baseEntity)
        {
            var assembly = Assembly.Load(entityAssemblyName);
            var entityclass = assembly.GetImplementedClass(baseEntity);
            return entityclass;
        }
    }


}
