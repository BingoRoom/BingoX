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
    public class EfDbEntityCreateInfo : DbEntityCreateInfo
    {
#if Standard
        private readonly EntityEntry entityEntry;

        public EfDbEntityCreateInfo(EntityEntry entityEntry, IDictionary<string, object> currentValues)
            : base(entityEntry.Entity, currentValues)
        {
#else
        private readonly DbEntityEntry entityEntry;
        public EfDbEntityCreateInfo(DbEntityEntry entityEntry, IDictionary<string, object> currentValues)
            : base(entityEntry.Entity, currentValues)
        {
#endif
            this.entityEntry = entityEntry;
        }
        public override void SetValue(string name, object value)
        {

            if (CurrentValues.ContainsKey(name)) CurrentValues[name] = value;
        }
    }

#if Standard
#endif
}
