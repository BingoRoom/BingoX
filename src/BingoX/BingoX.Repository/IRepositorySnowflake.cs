using BingoX.Domain;

namespace BingoX.Repository
{
    /// <summary>
    /// 支持雪花主键的仓储接口
    /// </summary>
    /// <typeparam name="TDomain">领域实体</typeparam>
    public interface IRepositorySnowflake<TDomain> : IRepository<TDomain, long> where TDomain : class, ISnowflakeEntity<TDomain>
    {
        new TDomain GetId(long id);

        new bool Exist(long id);

        new int Delete(long[] pkArray);
    }

    public class RepositorySnowflake<TDomain> : Repository<TDomain, long>, IRepositorySnowflake<TDomain> where TDomain : class, ISnowflakeEntity<TDomain>
    {
        public RepositorySnowflake(RepositoryContextOptions options) : base(options)
        {

        }
        public override TDomain GetId(long id)
        {
            return base.GetId(id);
        }

        public override bool Exist(long id)
        {
            return base.Exist(id);
        }

        public override int Delete(long[] pkArray)
        {
            return base.Delete(pkArray);
        }
    }
}