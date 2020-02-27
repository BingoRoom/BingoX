
using System;
using System.Collections.Generic;

namespace BingoX.DataAccessor
{
    public class DbEntityChangeInfo : DbEntityInfo
    {
        public DbEntityChangeInfo(object entity, IDictionary<string, object> currentValues, IDictionary<string, object> originalValues, IDictionary<string, object> changeValues) : base(entity)
        {



            if (currentValues == null)
            {
                throw new ArgumentNullException(nameof(currentValues));
            }

            if (originalValues == null)
            {
                throw new ArgumentNullException(nameof(originalValues));
            }

            if (changeValues == null)
            {
                throw new ArgumentNullException(nameof(changeValues));
            }

            CurrentValues = currentValues;
            OriginalValues = originalValues;
            ChangeValues = changeValues;

        }
        public IDictionary<string, object> CurrentValues { get; private set; }
        public IDictionary<string, object> OriginalValues { get; private set; }
        public IDictionary<string, object> ChangeValues { get; private set; }
        public virtual void SetValue(string name, object value)
        {
            if (ChangeValues.ContainsKey(name)) ChangeValues[name] = value;
            else if (CurrentValues.ContainsKey(name))
            {
                ChangeValues.Add(name, value);
            }
        }
    }
}