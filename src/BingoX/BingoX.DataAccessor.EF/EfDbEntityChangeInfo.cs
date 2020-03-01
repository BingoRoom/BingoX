#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
#else

using System.Data.Entity.Infrastructure;
#endif
using System.Collections.Generic;

namespace BingoX.DataAccessor.EF
{
    public class EfDbEntityChangeInfo : DbEntityChangeInfo
    {
#if Standard
        private readonly EntityEntry entityEntry;
        public EfDbEntityChangeInfo(EntityEntry entityEntry, IDictionary<string, object> currentValues, IDictionary<string, object> originalValues, IDictionary<string, object> changeValues)
         : base(entityEntry.Entity, currentValues, originalValues, changeValues)
        {
#else
        private readonly DbEntityEntry entityEntry;
        public EfDbEntityChangeInfo(DbEntityEntry entityEntry, IDictionary<string, object> currentValues, IDictionary<string, object> originalValues, IDictionary<string, object> changeValues)
           : base(entityEntry.Entity, currentValues, originalValues, changeValues)
        {
#endif

            this.entityEntry = entityEntry;
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
