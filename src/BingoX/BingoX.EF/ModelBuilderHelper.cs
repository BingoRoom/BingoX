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
            if (!option.IsEffective()) return;
            ModelCreating(modelBuilder, option.EntityAssemblyName, option.ConfigAssemblyName, option.BaseEntity);
        }

        public static void ModelCreating(this ModelBuilder modelBuilder, string configAssemblyName, string entityAssemblyName, Type baseEntity)
        {
            System.Type[] entityclass = GetEntities(configAssemblyName, baseEntity);
            System.Type[] dataclass = GetConfies(entityAssemblyName, entityclass);
            var method = typeof(ModelBuilder).GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(n => n.Name == "ApplyConfiguration" && n.GetParameters()[0].ParameterType.Name == "IEntityTypeConfiguration`1");
            foreach (var item in dataclass)
            {
                var genericTypes = item.GetInterfaces()[0];
                var obj = item.CreateInstance();
                var addmethod = method.MakeGenericMethod(genericTypes.GenericTypeArguments);
                addmethod.FastInvoke(modelBuilder, obj);
            }
        }

        private static Type[] GetConfies(string configAssemblyName, Type[] entityclass)
        {
            var assembly = Assembly.Load(configAssemblyName);
            var genericType = typeof(IEntityTypeConfiguration<>);
            return assembly.GetImplementedClassWithOneArgumentGenericType(genericType, entityclass);
        }
#endif
        private static Type[] GetEntities(string entityAssemblyName, Type baseEntity)
        {
            var assembly = Assembly.Load(entityAssemblyName);
            var entityclass = assembly.GetImplementedClass(baseEntity);
            return entityclass;
        }
    }


}
