using BingoX.Domain;

namespace BingoX.Repository
{
    /// <summary>
    /// 支持自增主键的仓储接口
    /// </summary>
    /// <typeparam name="TDomain">领域实体</typeparam>
    public interface IRepositoryIdentity<TDomain> : IRepository<TDomain, int> where TDomain : class, IIdentityEntity<TDomain>
    {
        new TDomain GetId(int id);

        new bool Exist(int id);

        new int Delete(int[] pkArray);
    }

    public class RepositoryIdentity<TDomain> : Repository<TDomain, int>, IRepositoryIdentity<TDomain> where TDomain : class, IIdentityEntity<TDomain>
    {
        public RepositoryIdentity(RepositoryContextOptions options) : base(options)
        {

        }
        public override TDomain GetId(int id)
        {
            return base.GetId(id);
        }

        public override bool Exist(int id)
        {
            return base.Exist(id);
        }

        public override int Delete(int[] pkArray)
        {
            return base.Delete(pkArray);
        }
    }
}