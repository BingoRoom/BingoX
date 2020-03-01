#if Standard
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.Extensions.DependencyInjection;
#else
using System.Data.Entity.Infrastructure;
#endif

namespace BingoX.DataAccessor.EF
{
    public class EfDbEntityDeleteInfo : DbEntityDeleteInfo
    {
#if Standard
        private readonly EntityEntry entityEntry;


        public EfDbEntityDeleteInfo(EntityEntry entityEntry) : base(entityEntry.Entity)
        {
#else
        private readonly DbEntityEntry entityEntry;
        public EfDbEntityDeleteInfo(DbEntityEntry entityEntry) : base(entityEntry.Entity)
        {
#endif
            this.entityEntry = entityEntry;
        }
    }

#if Standard
#endif
}
