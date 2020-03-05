using BingoX.Helper;
using System;
using System.Collections.Generic;
using System.Linq;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个数据库操作拦截器管理器
    /// </summary>
    public interface IDbEntityInterceptManagement
    {
        /// <summary>
        /// 向拦截器缓存添加拦截器
        /// </summary>
        /// <param name="dbEntityIntercept">实现接口类型(IDbEntityIntercept)的拦截器实例</param>
        void AddGlobalIntercept(Type dbEntityIntercept);
        /// <summary>
        /// 向拦截器缓存添加拦截器
        /// </summary>
        /// <param name="dbEntityIntercept">特性类型(DbEntityInterceptAttribute)的拦截器实例</param>
        void AddGlobalIntercept(DbEntityInterceptAttribute dbEntityIntercept);
        /// <summary>
        /// 批量向拦截器缓存添加拦截器
        /// </summary>
        /// <param name="dbEntityIntercepts">特性类型(DbEntityInterceptAttribute)的拦截器实例集合</param>
        void AddRangeGlobalIntercepts(IEnumerable<DbEntityInterceptAttribute> dbEntityIntercepts);
        /// <summary>
        /// 获取或构建从指定数据库实体的标签上获取到的所有实现了接口IDbEntityIntercept的类型的实例。
        /// </summary>
        /// <param name="entityType">数据库实体</param>
        /// <returns>实例集合</returns>
        IEnumerable<IDbEntityIntercept> GetAops(Type entityType);
        /// <summary>
        /// 获取数据库实体上的DbEntityInterceptAttribute类型的标签
        /// </summary>
        /// <param name="entityType">数据库实体类型</param>
        /// <returns></returns>
        IEnumerable<DbEntityInterceptAttribute> GetAttributes(Type entityType);
    }

    public class DbEntityInterceptManagement : IDbEntityInterceptManagement
    {
        protected readonly IDictionary<Type, IEnumerable<DbEntityInterceptAttribute>> dictionary = new Dictionary<Type, IEnumerable<DbEntityInterceptAttribute>>();
        protected readonly List<DbEntityInterceptAttribute> global = new List<DbEntityInterceptAttribute>();
        protected readonly IServiceProvider serviceProvider;

        public DbEntityInterceptManagement(IServiceProvider serviceProvider)
        {
            this.serviceProvider = serviceProvider;
        }
        /// <summary>
        /// 从DI获取服务
        /// </summary>
        /// <typeparam name="T">服务类型</typeparam>
        /// <param name="type">服务类型</param>
        /// <returns></returns>
        protected T GetService<T>(Type type)
        {
            object obj = null;
#if Standard
            obj = serviceProvider.GetRequiredService(type);
#else
            obj = serviceProvider.GetService(type);
#endif
            if (obj is T) return (T)obj;
            return default(T);
        }
        /// <summary>
        /// 从DI获取服务
        /// </summary>
        /// <param name="type">服务类型</param>
        /// <returns></returns>
        protected object GetService(Type type)
        {
            object obj = null;
#if Standard
            obj = serviceProvider.GetRequiredService(type);
#else
            obj = serviceProvider.GetService(type);
#endif
            return obj;
        }

        public IEnumerable<IDbEntityIntercept> GetAops(Type entityType)
        {
            var attributes = GetAttributes(entityType);
            if (attributes.IsEmpty()) return null;
            var aops = attributes.Select(n =>
            {
                var intercept = n.Intercept;
                if (intercept == null) intercept = GetService<IDbEntityIntercept>(n.AopType);

                if (intercept == null)
                {
                    var constructor = n.AopType.GetConstructors().FirstOrDefault();
                    var par = constructor.GetParameters();
                    if (par.Length == 0) intercept = constructor.Invoke(null) as IDbEntityIntercept;
                    else
                    {
                        var parms = par.Select(x => GetService(x.ParameterType)).ToArray();
                        intercept = constructor.Invoke(parms) as IDbEntityIntercept;
                    }
                }
                return intercept;
            }).Where(n => n != null);
            return aops;
        }

        public IEnumerable<DbEntityInterceptAttribute> GetAttributes(Type entityType)
        {
            if (entityType == null)
            {
                throw new ArgumentNullException(nameof(entityType));
            }

            IEnumerable<DbEntityInterceptAttribute> intercepts;
            if (dictionary.ContainsKey(entityType))
            {
                intercepts = dictionary[entityType];
            }
            else
            {
                intercepts = entityType.GetCustomAttributesIncludeBaseType<DbEntityInterceptAttribute>().ToArray();
                dictionary.Add(entityType, intercepts);
            }
            return global.Union(intercepts).ToArray();
        }

        public void AddGlobalIntercept(Type dbEntityIntercept)
        {
            if (!typeof(IDbEntityIntercept).IsAssignableFrom(dbEntityIntercept)) throw new LogicException("类型不为IDbEntityIntercept");
            global.Add(new DbEntityInterceptAttribute(dbEntityIntercept));
        }

        public void AddGlobalIntercept(DbEntityInterceptAttribute dbEntityIntercept)
        {
            global.Add(dbEntityIntercept);
        }

        public void AddRangeGlobalIntercepts(IEnumerable<DbEntityInterceptAttribute> dbEntityIntercepts)
        {
            global.AddRange(dbEntityIntercepts);
        }
    }
}
