

#if Standard
using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BingoX.EF
{
    public abstract class BingoEFOptions
    {
        public BingoEFOptions()
        {
            Intercepts = new InterceptCollection();
        }
        public InterceptCollection Intercepts { get; private set; }

        public Assembly AssemblyRepository { get; set; }
        public Assembly AssemblyFactory { get; set; }
        public Assembly AssemblyMappingConfig { get; set; }
        public Assembly AssemblyDomainEventHandler { get; set; }
    }
    public class BingoEFOptions<TContext> : BingoEFOptions where TContext : EfDbContext
    {


        public DbContextOptions<TContext> DbContextOptions { get; set; }
    }
}
#endif