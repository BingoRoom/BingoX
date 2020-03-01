using System;
using System.Collections.ObjectModel;

namespace BingoX.DataAccessor
{
    /// <summary>
    /// 数据库操作拦截器集合
    /// </summary>
    public class InterceptCollection : Collection<IDbEntityIntercept>
    {
        public void Add<TFilterType>(InterceptDIEnum interceptDI = InterceptDIEnum.None) where TFilterType : IDbEntityIntercept
        {
            Add(typeof(TFilterType), order: 0, interceptDI: interceptDI);
        }

        public void Add(Type filterType)
        {
            if (filterType == null)
            {
                throw new ArgumentNullException(nameof(filterType));
            }

            Add(filterType, order: 0);
        }

        public IDbEntityIntercept Add(Type filterType, int order, InterceptDIEnum interceptDI = InterceptDIEnum.None)
        {
            if (filterType == null)
            {
                throw new ArgumentNullException(nameof(filterType));
            }

            if (!typeof(IDbEntityIntercept).IsAssignableFrom(filterType))
            {

                throw new ArgumentException("", nameof(filterType));
            }

            var filter = new DbEntityInterceptAttribute(filterType, order, interceptDI);
            Add(filter);
            return filter;
        }

        public void Add<TFilterType>(TFilterType intercept) where TFilterType : IDbEntityIntercept
        {
            if (intercept is DbEntityInterceptAttribute)
            {
                base.Add(intercept);
                return;
            }
            var filter = new DbEntityInterceptAttribute(intercept);
            base.Add(filter);
        }
    }
}
