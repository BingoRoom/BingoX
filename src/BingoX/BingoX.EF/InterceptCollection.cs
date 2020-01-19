#if Standard
#else

using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Common;
using System.Data.Entity.Core.Objects;
#endif
using BingoX.Repository;
using System;
using System.Collections.ObjectModel;

namespace BingoX.EF
{
    public class InterceptCollection : Collection<IDbEntityIntercept>
    {
        
        public IDbEntityIntercept Add<TFilterType>(InterceptDIEnum interceptDI = InterceptDIEnum.None) where TFilterType : IDbEntityIntercept
        {
            return Add(typeof(TFilterType), order: 0, interceptDI: interceptDI);
        }
        public IDbEntityIntercept Add(Type filterType)
        {
            if (filterType == null)
            {
                throw new ArgumentNullException(nameof(filterType));
            }

            return Add(filterType, order: 0);
        }
        /// <summary>
        /// Adds a type representing an <see cref="IFilterMetadata"/>.
        /// </summary>
        /// <param name="filterType">Type representing an <see cref="IFilterMetadata"/>.</param>
        /// <param name="order">The order of the added filter.</param>
        /// <returns>An <see cref="IFilterMetadata"/> representing the added type.</returns>
        /// <remarks>
        /// Filter instances will be created using
        /// <see cref="Microsoft.Extensions.DependencyInjection.ActivatorUtilities"/>.
        /// Use <see cref="AddService(Type)"/> to register a service as a filter.
        /// </remarks>
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

    }
}
