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
using BingoX.ComponentModel;

namespace BingoX.EF
{
    public static class ModelBuilderHelper
    {
#if Standard


        public static void ModelCreating(this ModelBuilder modelBuilder, Assembly configAssembly)
        {

            var dataclass = GetConfies(configAssembly);
            var method = typeof(ModelBuilder).GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(n => n.Name == "ApplyConfiguration" && n.GetParameters()[0].ParameterType.Name == "IEntityTypeConfiguration`1");

            foreach (var item in dataclass)
            {
                var genericTypes = item.BaseType;
                var obj = item.ImplementedType.CreateInstance();
                var addmethod = method.MakeGenericMethod(genericTypes.GenericTypeArguments);
                addmethod.FastInvoke(modelBuilder, obj);
            }
        }

        private static AssemblyScanResult[] GetConfies(Assembly configAssembl)
        {

            var genericType = typeof(IEntityTypeConfiguration<>);
            var scaner = new AssemblyScanClass(configAssembl, genericType);
            return scaner.Find(null);
        }
#endif
    }

}
