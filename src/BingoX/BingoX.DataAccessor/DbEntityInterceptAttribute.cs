
using System;

namespace BingoX.DataAccessor
{
    [System.AttributeUsage(AttributeTargets.Class, Inherited = false, AllowMultiple = true)]
    public sealed class DbEntityInterceptAttribute : Attribute, IDbEntityIntercept
    {
        public DbEntityInterceptAttribute(Type aopType, int order = 0, InterceptDIEnum interceptDI = InterceptDIEnum.Scoped)
        {
            if (aopType == null)
            {
                throw new ArgumentNullException(nameof(aopType));
            }
            if (!typeof(IDbEntityIntercept).IsAssignableFrom(aopType)) throw new RepositoryException("类型错误，没有实现接口BingoX.DataAccessor.IDbEntityIntercept");
            AopType = aopType;
            Order = order;
            DI = interceptDI;
        }
        public DbEntityInterceptAttribute(IDbEntityIntercept intercept, int order = 0, InterceptDIEnum interceptDI = InterceptDIEnum.None)
        {
            Intercept = intercept;
            Order = order;
            DI = interceptDI;
        }
        public Type AopType { get; private set; }
        public IDbEntityIntercept Intercept { get; private set; }
        public int Order { get; private set; }

        public InterceptDIEnum DI { get; private set; }
    }
}