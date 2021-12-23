using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;

namespace BingoX.DynamicSearch
{
    public class DynamicTableInfoCollection : IEnumerable<DynamicTableInfo>
    {
        public void AdddRange(DynamicTableInfo[] tables)
        {
            foreach (var item in tables)
            {
                dic.Add(item.Code, item);
            }

        }

        public int Count { get { return dic.Count; } }
        public void AddTable(DynamicTableInfo info)
        {
            dic.Add(info.Code, info);
        }

        public IEnumerator<DynamicTableInfo> GetEnumerator()
        {
            return dic.Values.GetEnumerator();
        }

        IEnumerator IEnumerable.GetEnumerator()
        {
            return dic.Values.GetEnumerator();
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
