
using System;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 表示一个数据库操作拦截器特性
    /// </summary>
    [AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DbEntityInterceptAttribute : Attribute, IDbEntityIntercept
    {
        /// <summary>
        /// 创建一个数据库操作拦截器特性
        /// </summary>
        /// <param name="aopType">拦截器类型</param>
        /// <param name="order">拦截器执行拦截的顺序</param>
        /// <param name="interceptDI">拦截器在DI中的生命周期</param>
        public DbEntityInterceptAttribute(Type aopType, int order = 0, InterceptDIEnum interceptDI = InterceptDIEnum.Scoped)
        {
            if (aopType == null)
            {
                throw new ArgumentNullException(nameof(aopType));
            }
            if (!typeof(IDbEntityIntercept).IsAssignableFrom(aopType)) throw new DataAccessorException("类型错误，没有实现接口BingoX.DataAccessor.IDbEntityIntercept");
            AopType = aopType;
            Order = order;
            DI = interceptDI;
        }
        /// <summary>
        /// 创建一个数据库操作拦截器特性
        /// </summary>
        /// <param name="intercept">拦截器</param>
        /// <param name="order">拦截器执行拦截的顺序</param>
        /// <param name="interceptDI">拦截器在DI中的生命周期</param>
        public DbEntityInterceptAttribute(IDbEntityIntercept intercept, int order = 0, InterceptDIEnum interceptDI = InterceptDIEnum.None)
        {
            Intercept = intercept;
            Order = order;
            DI = interceptDI;
        }
        /// <summary>
        /// 拦截器类型
        /// </summary>
        public Type AopType { get; private set; }
        /// <summary>
        /// 拦截器
        /// </summary>
        public IDbEntityIntercept Intercept { get; private set; }
        /// <summary>
        /// 拦截器执行拦截的顺序
        /// </summary>
        public int Order { get; private set; }
        /// <summary>
        /// 拦截器在DI中的生命周期
        /// </summary>
        public InterceptDIEnum DI { get; private set; }
    }
}