using System;
using System.Collections.Generic;
using System.Linq;

namespace BingoX.DynamicSearch
{
    public class DynamicTableInfoCollection
    {
        public void AdddRange(DynamicTableInfo[] tables)
        {
            foreach (var item in tables)
            {
                dic.Add(item.Code, item);
            }

        }
        public void AddTable(DynamicTableInfo info)
        {
            dic.Add(info.Code, info);
        }
        class StringEqualityComparer : IEqualityComparer<string>
        {
            public bool Equals(string x, string y)
            {
                return string.Equals(x, y, StringComparison.InvariantCultureIgnoreCase);
            }

            public int GetHashCode(string obj)
            {

                return obj == null ? 0 : obj.GetHashCode();
            }
        }
        Dictionary<string, DynamicTableInfo> dic = new Dictionary<string, DynamicTableInfo>(new StringEqualityComparer());

        public DynamicTableInfo this[string key]
        {
            get { return dic[key]; }
        }
    }

}
