using System;
using System.Collections.ObjectModel;
using System.Linq;

namespace BingoX.DynamicSearch
{
    public class DynamicDataAccessorCollection: Collection<IDynamicDataAccessor>
    {
        public IDynamicDataAccessor this[string key]
        {
            get { return this.FirstOrDefault(n=>string.Equals(key,n.Node,StringComparison.CurrentCultureIgnoreCase)); }
        }
    
    }

}
