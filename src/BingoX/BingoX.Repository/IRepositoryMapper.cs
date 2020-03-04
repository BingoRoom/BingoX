using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace BingoX.Repository
{
    /// <summary>
    /// 仓储使用的映射器
    /// </summary>
    public interface IRepositoryMapper
    {
        TDestination ProjectTo<TSource, TDestination>(TSource obj);
        TDestination ProjectTo<TDestination>(object obj);

        IList<TDestination> ProjectToList<TSource, TDestination>(IEnumerable<TSource> list);

        TDestination[] ProjectToArray<TSource, TDestination>(IEnumerable<TSource> list);
    }
}
