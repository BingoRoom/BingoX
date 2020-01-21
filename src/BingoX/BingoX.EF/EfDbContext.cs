#if Standard
using Microsoft.EntityFrameworkCore;
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using BingoX.Repository;
using System.Collections.Generic;
using BingoX.ComponentModel;
using BingoX.Helper;
using System.Reflection;
using System.Linq;

namespace BingoX.EF
{
#if Standard
    public abstract class EfDbContext : Microsoft.EntityFrameworkCore.DbContext, IDbContext
    {
        public EfDbContext(DbContextOptions options) : base(options)
        {
            RootContextData = new Dictionary<string, object>();
        }
        internal const string DIConst = "ServiceProvider";
        public IDictionary<string, object> RootContextData { get; private set; }
        public void SetServiceProvider(System.IServiceProvider serviceProvider)
        {
            if (RootContextData.ContainsKey(DIConst)) RootContextData[DIConst] = serviceProvider;
            else RootContextData.Add(DIConst, serviceProvider);
        }
        public System.IServiceProvider GetServiceProvider()
        {
            return RootContextData.ContainsKey(DIConst) ? RootContextData[DIConst] as System.IServiceProvider : null;
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            var serviceProvider = GetServiceProvider();
            if (serviceProvider == null) return;
            var option = serviceProvider.GetService(typeof(BingoEFOptions)) as BingoEFOptions;
            if (option == null || option.AssemblyMappingConfig == null) return;
            var method = typeof(ModelBuilder).GetMethods(BindingFlags.Public | BindingFlags.Instance).FirstOrDefault(n => n.Name == "ApplyConfiguration" && n.GetParameters()[0].ParameterType.Name == "IEntityTypeConfiguration`1");
            var scaner = new AssemblyScanClass(option.AssemblyMappingConfig, typeof(IEntityTypeConfiguration<>));
            foreach (var item in scaner.Find())
            {
                var genericTypes = item.BaseType;
                var obj = item.ImplementedType.CreateInstance();
                var addmethod = method.MakeGenericMethod(genericTypes.GenericTypeArguments);
                addmethod.FastInvoke(modelBuilder, obj);
            }
            //   modelBuilder.ModelCreating(new ModelMappingOption { BaseEntity = typeof(BaseEntity), ConfigAssemblyName = "YinYun.Application.Data", EntityAssemblyName = "YinYun.Application.Domain" });
        }
    }


#else
    public  abstract  class EfDbContext : System.Data.Entity.DbContext, IDbContext
    {
        public EfDbContext(string nameOrConnectionString) : base(nameOrConnectionString) { }
        public EfDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model) { }
        public EfDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection) { }
        public EfDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext) { }
        public EfDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection,model,contextOwnsConnection) { } 
            

    }

#endif
}
