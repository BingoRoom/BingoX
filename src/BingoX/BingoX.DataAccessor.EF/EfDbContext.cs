#if Standard
using Microsoft.EntityFrameworkCore;
#else
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using System.Collections.Generic;
using System;

namespace BingoX.DataAccessor.EF
{
#if Standard
    public abstract class EfDbContext : DbContext, IDbContext
    {
        public EfDbContext(DbContextOptions options) : base(options)
        {
           
        }
#else
    public abstract class EfDbContext : System.Data.Entity.DbContext, IDbContext
    {
        public EfDbContext(string nameOrConnectionString) : base(nameOrConnectionString)
        {
         
        }
        public EfDbContext(string nameOrConnectionString, DbCompiledModel model) : base(nameOrConnectionString, model)
        {
 
        }
        public EfDbContext(DbConnection existingConnection, bool contextOwnsConnection) : base(existingConnection, contextOwnsConnection)
        {
             
        }
        public EfDbContext(ObjectContext objectContext, bool dbContextOwnsObjectContext) : base(objectContext, dbContextOwnsObjectContext)
        {
          
        }
        public EfDbContext(DbConnection existingConnection, DbCompiledModel model, bool contextOwnsConnection) : base(existingConnection, model, contextOwnsConnection)
        {
          
        }
#endif
   
        /// <summary>
        /// 服务提供其在辅助数据字典中的键名
        /// </summary>
        internal const string DIConst = "ServiceProvider";
        /// <summary>
        /// 数据库上下文辅助数据字典
        /// </summary>
        public IDictionary<string, object> RootContextData { get; private set; } = new Dictionary<string, object>();
        /// <summary>
        /// 设置服务提供器
        /// </summary>
        /// <param name="serviceProvider"></param>
        public void SetServiceProvider(System.IServiceProvider serviceProvider)
        {
            if (RootContextData.ContainsKey(DIConst)) RootContextData[DIConst] = serviceProvider;
            RootContextData.Add(DIConst, serviceProvider);
        }
        /// <summary>
        /// 获取服务提供器
        /// </summary>
        /// <returns></returns>
        public IServiceProvider GetServiceProvider()
        {
            return RootContextData.ContainsKey(DIConst) ? RootContextData[DIConst] as System.IServiceProvider : null;
        }
    }
}
