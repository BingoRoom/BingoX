using System;

namespace BingoX.ComponentModel.Data
{
    public class QueryDescriptor
    {
        private int pageIndex;
        private int pageSize;

        public int PageSize
        {
            get
            {
                return pageSize;
            }

            set
            {
                if (pageSize == value) return;
                if (value < 0) throw new Exception("不能小于0");
                pageSize = value;
            }
        }
        public int PageIndex
        {
            get { return pageIndex; }
            set
            {
                if (pageIndex == value) return;
                if (value < 0) throw new Exception("不能小于0");
                pageIndex = value;

            }
        }
        public System.Collections.Generic.IDictionary<string, string> Filters { get; set; }
        public SortFieldCollection Sorting { get; set; }
    }
 
}