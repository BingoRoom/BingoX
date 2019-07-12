using BingoX.Repository;

namespace BingoX.EF
{
    public abstract class EfRepositoryBuilder : IRepositoryBuilder
    {
        private readonly EfDbContext context;

        public EfRepositoryBuilder(EfDbContext context)
        {
            this.context = context as EfDbContext;
            UnitOfWork = new EfUnitOfWork(this.context);
        }

        public EfUnitOfWork UnitOfWork { get; private set; }

        IUnitOfWork IRepositoryBuilder.UnitOfWork { get { return this.UnitOfWork; } }

        public EfRepository<T, pkType> Cretae<T, pkType>() where T : class, IEntity<T, pkType>, new()
        {
            return new EfRepository<T, pkType>(context);
        }

        IRepository<T, pkType> IRepositoryBuilder.Cretae<T, pkType>()
        {
            return Cretae<T, pkType>();
        }
    }
}
