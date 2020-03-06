 
using System.Collections.Generic;

namespace BingoX.DataAccessor.SqlSugar
{
    public class SqlSugarDbEntityChangeInfo : DbEntityChangeInfo
    {
  
        public SqlSugarDbEntityChangeInfo(object entityEntry, IDictionary<string, object> currentValues, IDictionary<string, object> originalValues, IDictionary<string, object> changeValues)
         : base(entityEntry, currentValues, originalValues, changeValues)
        { 
        }
 

        public virtual void Remove(string name)
        {
            if (ChangeValues.ContainsKey(name)) ChangeValues.Remove(name);
        }
        public override void SetValue(string name, object value)
        {
            if (ChangeValues.ContainsKey(name)) ChangeValues[name] = value;
            else if (CurrentValues.ContainsKey(name))
            {
                ChangeValues.Add(name, value);
            }
        }
    }

#if Standard
#endif
}
