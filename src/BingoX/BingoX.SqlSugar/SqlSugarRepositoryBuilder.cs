using BingoX.Domain;
using BingoX.Repository;

namespace BingoX.SqlSugar
{
    public class SqlSugarRepositoryBuilder : IRepositoryBuilder
    {
        private readonly SqlSugarDbContext context;

        public SqlSugarRepositoryBuilder(SqlSugarDbContext context)
        {
            this.context = context as SqlSugarDbContext;
            UnitOfWork = new SqlSugarUnitOfWork(this.context);
        }

        public SqlSugarUnitOfWork UnitOfWork { get; private set; }

        IUnitOfWork IRepositoryBuilder.UnitOfWork { get { return this.UnitOfWork; } }

        public SqlSugarRepository<T, pkType> Cretae<T, pkType>() where T : class, IEntity<T, pkType>, new()
        {
            return new SqlSugarRepository<T, pkType>(context);
        }

        IRepository<T, pkType> IRepositoryBuilder.Cretae<T, pkType>()
        {
            return Cretae<T, pkType>();
        }
    }
}
