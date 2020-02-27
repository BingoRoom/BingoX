
using System;
using System.Collections.Generic;

namespace BingoX.DataAccessor
{
    public class DbEntityCreateInfo : DbEntityInfo
    {
        public DbEntityCreateInfo(object entity, IDictionary<string, object> currentValues) : base(entity)
        {


            if (currentValues == null)
            {
                throw new ArgumentNullException(nameof(currentValues));
            }

            CurrentValues = currentValues;
        }
        public IDictionary<string, object> CurrentValues { get; private set; }

        public virtual void SetValue(string name, object value)
        {
            if (CurrentValues.ContainsKey(name)) CurrentValues[name] = value;
        }
    }
}