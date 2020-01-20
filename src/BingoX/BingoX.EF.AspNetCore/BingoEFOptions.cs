

using Microsoft.EntityFrameworkCore;
using System.Reflection;

namespace BingoX.EF
{

    public class BingoEFOptions<TContext> where TContext : EfDbContext
    {
        public BingoEFOptions()
        {
            Intercepts = new InterceptCollection();
        }
        public InterceptCollection Intercepts { get; private set; }

        public Assembly AssemblyRepository { get; set; }
        public Assembly AssemblyFactory { get; set; }
        public Assembly AssemblyDomainEventHandler { get; set; }

        public DbContextOptions<TContext> DbContextOptions { get; set; }
    }
}
