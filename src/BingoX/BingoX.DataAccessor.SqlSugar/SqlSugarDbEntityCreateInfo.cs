

using System.Collections.Generic;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarDbEntityCreateInfo : DbEntityCreateInfo
    {
 
    

        public SqlSugarDbEntityCreateInfo(object entityEntry, IDictionary<string, object> currentValues)
            : base(entityEntry, currentValues)
        { 
        }
 
        public override void SetValue(string name, object value)
        {

            if (CurrentValues.ContainsKey(name)) CurrentValues[name] = value;
        }
    }

#if Standard
#endif
}
