using BingoX.Helper;
using System.Collections.Generic;
using System.Linq;

namespace BingoX.Repository
{
    internal static class RepositoryExtensions
    {
        public static IList<TDestination> ProjectToList<TSource, TDestination>(this Repository repository, IEnumerable<TSource> sources)
        {
            if (sources.IsEmpty()) return BingoX.Utility.EmptyUtility<TDestination>.EmptyList;
            if (typeof(TSource).Equals(typeof(TDestination))) return sources.OfType<TDestination>().ToArray();
            return repository.Mapper.ProjectToList<TSource, TDestination>(sources);
        }
        public static TDestination ProjectTo<TDestination>(this Repository repository, object source)
        {
            if (source == null) return default(TDestination);
            if (source.GetType().Equals(typeof(TDestination))) return (TDestination)source;
            return repository.Mapper.ProjectTo<TDestination>(source);
        }
    }
}