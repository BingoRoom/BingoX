using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace BingoX.ComponentModel.Data
{
    public interface IPagingList
    {
        int Total { get; }
        int PageSize { get; }
        int PageIndex { get; }
        ICollection Items { get; }
    }

    public interface IPagingList<TModel> : IPagingList 
    {
        new ICollection<TModel> Items { get; }
    }
    public sealed class PagingList<TModel> : IPagingList<TModel>, IPagingList
    {
        public PagingList(IList<TModel> collection, int pageIndex, int pageSize, int total)
        {
            PageIndex = pageIndex;
            PageSize = pageSize;
            Total = total;
            Items = new ReadOnlyCollection<TModel>(collection);

        }
        public int PageSize { get; private set; }
        public int PageIndex { get; private set; }
        public ReadOnlyCollection<TModel> Items { get; private set; }
        public int Total { get; private set; }
        ICollection IPagingList.Items { get { return Items; } }
        ICollection<TModel> IPagingList<TModel>.Items { get { return Items; } }
    }
}