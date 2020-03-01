#if Standard
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;

namespace BingoX.DataAccessor.EF
{
    /// <summary>
    /// 用于ASP.NET CORE + EF + DDD模式开发的项目配置选项
    /// </summary>
    public abstract class BingoEFOptions
    {
        public BingoEFOptions()
        {
            Intercepts = new InterceptCollection();
        }
        /// <summary>
        /// 数据库拦截器集合
        /// </summary>
        public InterceptCollection Intercepts { get; private set; }
        /// <summary>
        /// 仓储的程序集
        /// </summary>
        public Assembly AssemblyRepository { get; set; }
        /// <summary>
        /// 工厂的程序集
        /// </summary>
        public Assembly AssemblyFactory { get; set; }
        /// <summary>
        /// EF数据库映射的程序集
        /// </summary>
        public Assembly AssemblyMappingConfig { get; set; }
        /// <summary>
        /// 数据库实体的程序集
        /// </summary>
        public Assembly AssemblyEntity { get; set; }
        /// <summary>
        /// 领域事件的程序集
        /// </summary>
        public Assembly AssemblyDomainEventHandler { get; set; }
        /// <summary>
        /// 领域服务的程序集
        /// </summary>
        public Assembly AssemblyDomainService { get; set; }
    }
    public class BingoEFOptions<TContext> : BingoEFOptions where TContext : EfDbContext
    {
        /// <summary>
        /// EF数据库上下文选项
        /// </summary>
        public DbContextOptions<TContext> DbContextOptions { get; set; }
    }
}
#endif